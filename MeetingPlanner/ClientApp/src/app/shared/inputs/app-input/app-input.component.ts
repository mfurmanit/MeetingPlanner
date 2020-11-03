import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { isNullOrUndefined } from '../../helpers/application.helper';

export enum InputType {
  INPUT, TEXTAREA
}

@Component({
  selector: 'app-input',
  templateUrl: './app-input.component.html',
  styleUrls: ['./app-input.component.scss']
})
export class AppInputComponent implements OnInit, OnDestroy {
  @Input() form: FormGroup;
  @Input() controlName: string;
  @Input() groupName: string;
  @Input() type = InputType.INPUT;
  @Input() valueType: string = 'text';
  @Input() rows = 6;
  @Input() placeholderToTranslate: string;
  @Input() tooltip: string;
  @Input() required: boolean = false;
  @Input() readonly: boolean = false;
  @Input() maxLength: number = 255;
  @Input() minLength: number = null;
  @Input() min: number = null;
  @Input() max: number = null;
  @Input() hideCounter: boolean = false;
  @Input() digitInput = false;

  counter: number = 0;
  control: AbstractControl;
  inputType = InputType.INPUT;

  private readonly subscription = new Subscription();

  ngOnInit(): void {
    this.control = !this.groupName ? this.form.get(this.controlName)
      : this.form.get(this.groupName).get(this.controlName);

    if (!isNullOrUndefined(this.control.value)) {
      this.counter = this.control.value.toString().length;
    }

    this.subscription.add(this.control.valueChanges
      .subscribe(res => !isNullOrUndefined(res) ? this.counter = res.toString().length : this.counter = 0));
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
