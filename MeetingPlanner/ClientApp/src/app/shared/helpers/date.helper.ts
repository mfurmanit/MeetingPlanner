import { NativeDateAdapter } from '@angular/material/core';

export class DateHelper extends NativeDateAdapter {

  createDate(year: number, month: number, date: number): Date {
    const localDate = super.createDate(year, month, date);
    const offset = localDate.getTimezoneOffset() * 60000;
    return new Date(localDate.getTime() - offset); // utcDate
  }

  format(date: Date, displayFormat: string): string {
    if (displayFormat === 'input') {
      const day   = date.getDate();
      const month = date.getMonth() + 1;
      const year  = date.getFullYear();

      return `${this._to2digit(day)}.${this._to2digit(month)}.${year}`;
    } else if (displayFormat === 'inputMonth') {
      const month = date.getMonth() + 1;
      const year  = date.getFullYear();

      return `${this._to2digit(month)}/${year}`;
    } else {
      return date.toDateString();
    }
  }

  private _to2digit(n: number): string {
    return (`00${n}`).slice(-2);
  }
}

export const APP_DATE_FORMATS = {
  parse: {
    dateInput: {month: 'short', year: 'numeric', day: 'numeric'}
  },
  display: {
    dateInput: 'input',
    monthYearLabel: 'inputMonth',
    dateA11yLabel: {year: 'numeric', month: 'long', day: 'numeric'},
    monthYearA11yLabel: {year: 'numeric', month: 'long'}
  }
};
