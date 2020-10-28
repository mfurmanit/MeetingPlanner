import { Component, OnDestroy, OnInit } from '@angular/core';
import { CalendarEvent, CalendarEventAction, CalendarEventTimesChangedEvent, DAYS_OF_WEEK } from 'angular-calendar';
import { POLISH_MONTHS } from '../../shared/helpers/calendar.helper';
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
import { ApplicationService } from '../../shared/services';

const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3',
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF',
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA',
  },
};

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
  monthNames = POLISH_MONTHS;
  refresh: Subject<any> = new Subject();
  activeDayIsOpen: boolean = false;

  actions: CalendarEventAction[] = [
    {
      label: '<i class="material-icons mat-icon" style="color: white;">edit</i>',
      a11yLabel: 'Edit',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        if (this.isGlobal) {
          this.router.navigate(['planner/global', event.id]);
        } else {
          this.router.navigate(['planner/personal', event.id]);
        }
      }
    },
    {
      label: '<i class="material-icons mat-icon">delete</i>',
      a11yLabel: 'Delete',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        console.log('To be implemented...');
      }
    },
  ];

  events: CalendarEvent[] = [];

  readonly view = 'month';
  readonly weekStartsOn = DAYS_OF_WEEK.MONDAY;
  private readonly subscription: Subscription = new Subscription();

  constructor(private translateService: TranslateService,
              private router: Router,
              private appService: ApplicationService,
              private service: EventService) {
    this.listenForLanguageChange();
  }

  ngOnInit(): void {
    this.isGlobal = this.router.url.includes('global');
    this.getCurrentMonthName();
    this.getEvents();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  getEvents(): void {
    const method = this.isGlobal ? this.service.getAllGlobal() : this.service.getAll();
    this.subscription.add(method.subscribe(events => this.mapEvents(events)));
  }

  viewDateChange(): void {
    this.getCurrentMonthName();
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      this.activeDayIsOpen = !((isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) || events.length === 0);
      this.viewDate = date;
    }
  }

  eventTimeChanged({ event,
                     newStart,
                     newEnd,
                   }: CalendarEventTimesChangedEvent): void {
    console.log('To be implemented...');
  }

  private mapEvents(events: Event[]): void {
    events.forEach((apiEvent: Event) => {
      this.events.push({
        id: apiEvent.id,
        start: startOfDay(new Date(apiEvent.date)),
        end: endOfDay(new Date(apiEvent.date)),
        title: apiEvent.title,
        color: colors.red,
        actions: this.actions,
        allDay: !apiEvent.withTime,
        resizable: {
          beforeStart: true,
          afterEnd: true,
        },
        draggable: true
      });
    });

    this.refresh.next();
  }

  private getCurrentMonthName(): void {
    this.monthName = `${this.monthNames[this.viewDate.getMonth()]} ${this.viewDate.getFullYear()}`;
  }

  private listenForLanguageChange() {
    this.subscription.add(this.appService.appLangState.subscribe(lang => {
      this.translateService.use(lang);
      this.locale = lang;
    }));
  }

}
