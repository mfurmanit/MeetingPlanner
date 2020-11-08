import { Component, OnDestroy, OnInit } from '@angular/core';
import { CalendarEvent, CalendarEventAction, DAYS_OF_WEEK } from 'angular-calendar';
import { registerLocaleData } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import localePl from '@angular/common/locales/pl';
import localeEn from '@angular/common/locales/en';
import localeEs from '@angular/common/locales/es';
import { endOfDay, isSameDay, isSameMonth, startOfDay, } from 'date-fns';
import { EventService } from '../../shared/services/event.service';
import { Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { Event } from '../../shared/model/event';
import { ApplicationService, SnackBarService } from '../../shared/services';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../shared/dialogs';
import { isEmpty, parseDateParam } from '../../shared/helpers/application.helper';

registerLocaleData(localePl);
registerLocaleData(localeEn);
registerLocaleData(localeEs);

@Component({
  selector: 'app-planner-view',
  templateUrl: './planner-view.component.html',
  styleUrls: ['./planner-view.component.scss']
})
export class PlannerViewComponent implements OnInit, OnDestroy {

  isGlobal: boolean = true;
  locale = 'pl';
  monthName = '';
  viewDate: Date = new Date();
  refresh: Subject<any> = new Subject();
  activeDayIsOpen: boolean = false;

  actions: CalendarEventAction[] = [
    {
      label: '<i class="material-icons mat-icon d-inline-flex align-middle">edit</i>',
      a11yLabel: 'Edit',
      onClick: ({ event }: { event: CalendarEvent }): void => this.editEvent(event)
    },
    {
      label: '<i class="material-icons mat-icon d-inline-flex align-middle">delete</i>',
      a11yLabel: 'Delete',
      onClick: ({ event }: { event: CalendarEvent }): void => this.openDeleteDialog(event)
    }
  ];

  eventColors: any = {
    primary: '#556df2',
    secondary: '#D1E8FF',
  };

  events: CalendarEvent[] = [];

  readonly view = 'month';
  readonly weekStartsOn = DAYS_OF_WEEK.MONDAY;
  private readonly themeClass = 'calendar-theme';
  private readonly subscription: Subscription = new Subscription();

  constructor(private translateService: TranslateService,
              private router: Router,
              private dialog: MatDialog,
              private snackBar: SnackBarService,
              private appService: ApplicationService,
              private service: EventService) {
    this.snackBar.translate(translateService);
    this.listenForLanguageChange();
  }

  ngOnInit(): void {
    document.body.classList.add(this.themeClass);
    this.isGlobal = this.router.url.includes('global');
    this.getEvents();
  }

  ngOnDestroy() {
    document.body.classList.remove(this.themeClass);
    this.subscription.unsubscribe();
  }

  getEvents(): void {
    const date = parseDateParam(this.viewDate);
    const method = this.isGlobal ? this.service.getAllGlobal(date) : this.service.getAllPersonal(date);
    this.subscription.add(method.subscribe(events => this.mapEvents(events)));
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      this.activeDayIsOpen = !((isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) || events.length === 0);
      this.viewDate = date;
    }
  }

  viewDateChange(): void {
    this.activeDayIsOpen = false;
    this.getEvents();
  }

  private editEvent(event: CalendarEvent): void {
    if (this.isGlobal) {
      this.router.navigate(['planner/global', event.id]);
    } else {
      this.router.navigate(['planner/personal', event.id]);
    }
  }

  private openDeleteDialog(event: CalendarEvent): void {
    this.subscription.add(this.dialog.open(ConfirmDialogComponent, {
        data: {
          action: this.translateService.instant('messages.eventDeletionHeader'),
          message: this.translateService.instant('messages.eventDeletionContent')
        }
      }
    ).afterClosed()
      .subscribe(accepted => accepted ? this.deleteEvent(event) : null));
  }

  private deleteEvent(event: CalendarEvent): void {
    this.subscription.add(this.service.delete(event.id as string).subscribe(() => {
      const index = this.events.indexOf(event);
      this.events.splice(index, 1);
      this.refresh.next();
      this.activeDayIsOpen = false;
      this.snackBar.openSnackBar('messages.eventDeleted', true, true);
    }));
  }

  private mapEvents(events: Event[]): void {
    this.events = [];

    events.forEach((apiEvent: Event) => {
      this.events.push({
        id: apiEvent.id,
        start: startOfDay(new Date(apiEvent.date)),
        end: endOfDay(new Date(apiEvent.date)),
        title: this.mapTitle(apiEvent),
        color: this.eventColors,
        actions: this.actions,
        resizable: {
          beforeStart: false,
          afterEnd: false,
        },
        draggable: false
      });
    });

    this.refresh.next();
  }

  private mapTitle(event: Event): string {
    if (!isEmpty(event.hourFrom) && !isEmpty(event.hourTo)) {
      return `${event.title}  |  ${event.hourFrom} - ${event.hourTo}`;
    } else if (!isEmpty(event.hourFrom)) {
      return `${event.title}  |  ${event.hourFrom}`;
    } else {
      return event.title;
    }
  }

  private listenForLanguageChange() {
    this.subscription.add(this.appService.appLangState.subscribe(lang => {
      this.translateService.use(lang);
      this.locale = lang;
    }));
  }
}
