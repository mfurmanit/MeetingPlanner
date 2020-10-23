import { Component, OnInit } from '@angular/core';
import { CalendarEvent, DAYS_OF_WEEK } from 'angular-calendar';
import { COLORS, POLISH_MONTHS } from '../../shared/helpers/calendar.helper';
import { registerLocaleData } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import localePl from '@angular/common/locales/pl';

registerLocaleData(localePl);
@Component({
  selector: 'app-planner-view',
  templateUrl: './planner-view.component.html',
  styleUrls: ['./planner-view.component.scss']
})
export class PlannerViewComponent implements OnInit {

  locale = 'pl';
  monthName = '';
  viewDate: Date = new Date();
  monthNames = POLISH_MONTHS;
  events: CalendarEvent[];
  readonly view = 'month';
  readonly weekStartsOn = DAYS_OF_WEEK.MONDAY;

  constructor(private translateService: TranslateService) {
  }

  ngOnInit(): void {
    this.getCurrentMonthName();
    this.createEvents();
  }

  viewDateChange(): void {
    this.getCurrentMonthName();
  }

  private getCurrentMonthName(): void {
    this.monthName = `${this.monthNames[this.viewDate.getMonth()]} ${this.viewDate.getFullYear()}`;
  }

  private createEvents(): void {
    this.events = [
      {
        start: new Date(),
        end: new Date(),
        title: 'Przykładowe wydarzenie',
        color: COLORS.global,
        allDay: true
      } as CalendarEvent,
      {
        start: new Date(),
        end: new Date(),
        title: 'Przykładowe wydarzenie 2',
        color: COLORS.personal,
        allDay: true
      } as CalendarEvent
    ];
  }

}
