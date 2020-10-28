import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { InputType } from '../../shared/inputs';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { isNullOrUndefined } from '../../shared/helpers/application.helper';
import { Location } from '@angular/common';
import { EventService } from '../../shared/services/event.service';
import { Event } from '../../shared/model/event';
import { ApplicationService, SnackBarService } from '../../shared/services';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})
export class EventDetailsComponent implements OnInit, OnDestroy {
  isGlobal: boolean = true;
  isEditMode: boolean = false;
  form: FormGroup;
  textArea = InputType.TEXTAREA;
  event: Event;

  private readonly subscriptions = new Subscription();

  constructor(private formBuilder: FormBuilder,
              private snackBar: SnackBarService,
              private translateService: TranslateService,
              private appService: ApplicationService,
              private service: EventService,
              private location: Location,
              private router: Router,
              private route: ActivatedRoute) {
    this.listenForLanguageChange();
  }

  ngOnInit() {
    this.isGlobal = this.router.url.includes('global');
    this.form = this.initForm();
    this.checkParams();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  goBack() {
    this.location.back();
  }

  addEvent(): void {
    this.checkDataBeforeSave();
    this.subscriptions.add(this.service.save(this.form.value)
      .subscribe((event: Event) => {
        this.router.navigate(event.global ? ['planner/global'] : ['planner/personal']);
        this.snackBar.openSnackBar(event.global ? 'messages.globalEventAdded' : 'messages.personalEventAdded', true, true);
      }));
  }

  updateEvent(): void {
    this.checkDataBeforeSave();
    this.subscriptions.add(this.service.update(this.form.value, this.event.id)
      .subscribe(event => {
        this.snackBar.openSnackBar('messages.eventUpdated', true, true);
      }));
  }

  private checkDataBeforeSave(): void {
    if (!this.hasNullValue('hourFrom') || !this.hasNullValue('hourTo')) {
      this.form.get('withTime').patchValue(true);
    }
    this.form.get('global').patchValue(this.isGlobal);
  }

  private getEvent(eventId: string): void {
    const method: Observable<Event> = this.isGlobal ? this.service.getOneGlobalById(eventId) : this.service.getOneById(eventId);
    this.subscriptions.add(
      method.subscribe(event => {
        this.event = event;
        this.form.patchValue(event);
      })
    );
  }

  private checkParams(): void {
    this.subscriptions.add(
      this.route.params.subscribe(params => this.initData(params))
    );
  }

  private initData(params: Params): void {
    if (!isNullOrUndefined(params) && !isNullOrUndefined(params.id)) {
      this.isEditMode = true;
      this.getEvent(params.id);
    } else {
      this.isEditMode = false;
    }
  }

  private initForm(): FormGroup {
    return this.formBuilder.group({
      id: [null],
      title: [null, Validators.required],
      description: [null],
      date: [null, Validators.required],
      hourFrom: [null],
      hourTo: [null],
      withTime: [false],
      recurring: [false],
      global: [this.isGlobal],
      notifications: this.formBuilder.array([])
    });
  }

  private hasNullValue(controlName: string): boolean {
    return isNullOrUndefined(this.getControlValue(controlName));
  }

  private getControlValue(controlName: string): any {
    return this.form && this.form.get(controlName) && this.form.get(controlName).value ? this.form.get(controlName).value : null;
  }

  private listenForLanguageChange() {
    this.subscriptions.add(this.appService.appLangState.subscribe(lang => {
      this.translateService.use(lang);
    }));
  }
}
