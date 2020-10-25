import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { InputType } from '../../shared/inputs';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { isNullOrUndefined } from '../../shared/helpers/application.helper';
import { Location } from '@angular/common';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})
export class EventDetailsComponent implements OnInit, OnDestroy {
  isGlobal: boolean = true;
  isEditMode: boolean = false;
  form: FormGroup = null;
  textArea = InputType.TEXTAREA;
  event: Event;

  private readonly subscriptions = new Subscription();

  constructor(private formBuilder: FormBuilder,
              private location: Location,
              private router: Router,
              private route: ActivatedRoute) {
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

  addEvent(form: FormGroup): void {
    // to implement
  }

  updateEvent(form: FormGroup): void {
    // to implement
  }

  private getEvent(eventId: string): void {
    // this.subscriptions.add(
    //   this.service.getOneById(eventId).subscribe(event => {
    //     this.event = event;
    //     this.form.patchValue(event);
    //   })
    // );
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
      title: [null],
      description: [null],
      date: [null],
      hourFrom: [null],
      hourTo: [null],
      withTime: [false],
      recurring: [false],
      global: [this.isGlobal],
      notifications: this.formBuilder.array([])
    });
  }
}
