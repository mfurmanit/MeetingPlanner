import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlannerViewComponent } from './planner-view/planner-view.component';
import { CalendarModule } from 'angular-calendar';
import { SharedModule } from '../shared/shared.module';
import { PlannerRoutingModule } from './planner-routing.module';
import { EventDetailsComponent } from './event-details/event-details.component';
import { EventNotificationsComponent } from './event-notifications/event-notifications.component';

@NgModule({
  declarations: [
    PlannerViewComponent,
    EventDetailsComponent,
    EventNotificationsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PlannerRoutingModule,
    CalendarModule
  ]
})
export class PlannerModule {
}
