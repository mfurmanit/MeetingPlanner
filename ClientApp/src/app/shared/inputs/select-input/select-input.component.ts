import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-select-input',
  templateUrl: './select-input.component.html',
  styleUrls: ['./select-input.component.scss']
})
export class SelectInputComponent implements OnInit {

  @Input() form: FormGroup;
  @Input() controlName: string;
  @Input() required: boolean = false;
  @Input() multiple: boolean = false;
  @Input() withDefault: boolean = false;
  @Input() placeholderToTranslate: string;
  @Input() groupName: string;
  @Input() values: Array<any>;
  @Input() customValue: string = null;
  @Input() customDisplay: string = null;
  @Input() customDisplayFunction: (value: any) => any = null;
  @Input() customDisplayTranslate: boolean = false;
  @Output() readonly onSelected = new EventEmitter();

  control: AbstractControl;

  comparatorFn = (firstValue: string, secondValue: string) => {
    return firstValue === secondValue;
  }

  ngOnInit(): void {
    this.control = !this.groupName ? this.form.get(this.controlName)
      : this.form.get(this.groupName).get(this.controlName);
  }

  selectionChanged(event: MatSelectChange): void {
    this.onSelected.emit(event);
  }

}
