import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlannerViewComponent } from './planner-view/planner-view.component';
import { CalendarModule } from 'angular-calendar';
import { SharedModule } from '../shared/shared.module';
import { PlannerRoutingModule } from './planner-routing.module';

@NgModule({
  declarations: [PlannerViewComponent],
  imports: [
    CommonModule,
    SharedModule,
    PlannerRoutingModule,
    CalendarModule
  ]
})
export class PlannerModule {
}
