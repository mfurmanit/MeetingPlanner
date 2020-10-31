import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { isEmpty } from '../../shared/helpers/application.helper';
import { Event } from '../../shared/model/event';
import { ApplicationService, SnackBarService } from '../../shared/services';
import { TranslateService } from '@ngx-translate/core';
import { Notification } from '../../shared/model/notification';
import { NotificationUnit, NotificationUnitLabel, Quantities } from '../../shared/model/notification-unit';

@Component({
  selector: 'app-event-notifications',
  templateUrl: './event-notifications.component.html',
  styleUrls: ['./event-notifications.component.scss']
})
export class EventNotificationsComponent implements OnInit, OnDestroy, OnChanges {

  @Input() event: Event;
  @Input() formGroup: FormGroup;
  units: NotificationUnitLabel[];
  private readonly subscriptions = new Subscription();

  constructor(private formBuilder: FormBuilder,
              private snackBar: SnackBarService,
              private translateService: TranslateService,
              private appService: ApplicationService) {
    this.snackBar.translate(translateService);
    this.listenForLanguageChange();
  }

  ngOnInit() {
    this.initUnitsLabels();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.event) {
      this.initNotifications();
    }
  }

  get notificationsArray() {
    return this.formGroup.get('notifications') as FormArray;
  }

  addNotification(): void {
    this.notificationsArray.push(this.initForm({
      id: null,
      quantity: 14,
      unit: NotificationUnit.Days
    } as Notification));
    this.snackBar.openSnackBar('messages.notificationAdded', true, true);
  }

  deleteNotification(formControl: FormControl): void {
    const index = this.notificationsArray.controls.indexOf(formControl);
    this.notificationsArray.removeAt(index);
    this.snackBar.openSnackBar('messages.notificationDeleted', true, true);
  }

  resolveQuantities(control: FormControl): number[] {
    const unitControl = control.get('unit');
    if (unitControl && unitControl.value) {
      const quantity = Quantities.get(unitControl.value);
      this.setQuantityValidators(control, quantity);
      return this.createArrayWithNumbers(quantity);
    } else {
      this.setQuantityValidators(control, 23);
      return this.createArrayWithNumbers(23); // default form value is 14 hours
    }
  }

  private initUnitsLabels(): void {
    this.units = Object.values(NotificationUnit).map(unit => Object.assign({
      value: unit,
      key: `units.${unit.toString().toLowerCase()}`
    }));
  }

  private initNotifications(): void {
    if (!isEmpty(this.event?.notifications)) {
      this.event.notifications.forEach(notification => {
        this.notificationsArray.push(this.initForm(notification));
      });
    }
  }

  private initForm(notification?: Notification): FormGroup {
    return this.formBuilder.group({
      id: [notification.id],
      quantity: [notification.quantity, Validators.required],
      unit: [notification.unit, Validators.required]
    });
  }

  private createArrayWithNumbers(length: number) {
    return Array.from({ length: length }, (_, i) => i + 1);
  }

  private setQuantityValidators(control: FormControl, quantity: number): void {
    const quantityControl = control.get('quantity');
    quantityControl.clearValidators();
    quantityControl.setValidators([Validators.required, Validators.max(quantity)]);
    quantityControl.updateValueAndValidity();
  }

  private listenForLanguageChange() {
    this.subscriptions.add(this.appService.appLangState.subscribe(lang => {
      this.translateService.use(lang);
    }));
  }
}
