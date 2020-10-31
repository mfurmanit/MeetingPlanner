import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { MatCheckboxChange } from '@angular/material/checkbox';

@Component({
  selector: 'app-checkbox',
  templateUrl: './app-checkbox.component.html',
  styleUrls: ['./app-checkbox.component.scss']
})
export class AppCheckboxComponent implements OnInit {
  @Input() form: FormGroup;
  @Input() controlName: string;
  @Input() groupName: string;
  @Input() placeholderToTranslate: string;
  @Input() tooltip: string;
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() checked: boolean = false;
  @Input() simple: boolean = false;
  @Output() readonly isChecked = new EventEmitter();

  control: AbstractControl;

  ngOnInit(): void {
    if (this.simple === false) {
      this.control = !this.groupName ? this.form.get(this.controlName)
        : this.form.get(this.groupName).get(this.controlName);

      if (this.disabled) {
        this.control.disable();
      }
    }
  }

  onChange(event: MatCheckboxChange): void {
    this.isChecked.emit(event.checked);
  }

}
