import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NgxMaterialTimepickerTheme } from 'ngx-material-timepicker';

@Component({
  selector: 'app-time-input',
  templateUrl: './time-input.component.html',
  styleUrls: ['./time-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TimeInputComponent {
  @Input() form: FormGroup;
  @Input() groupName: string;
  @Input() controlName: string;
  @Input() placeholder: string;
  @Input() cssClass: string = 'field';
  @Input() required = false;

  ngoTheme: NgxMaterialTimepickerTheme = {
    container: {
      bodyBackgroundColor: '#fff',
      primaryFontFamily: 'Roboto, sans-serif'
    },
    dial: {
      dialBackgroundColor: '#c2185b',
      dialActiveColor: '#fff',
      dialInactiveColor: '#fff'
    },
    clockFace: {
      clockFaceBackgroundColor: '#c2185b',
      clockHandColor: '#fff',
      clockFaceTimeInactiveColor: '#fff',
      clockFaceInnerTimeInactiveColor: '#fff',
      clockFaceTimeActiveColor: '#000'
    }
  };

}
