import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlannerViewComponent } from './planner-view/planner-view.component';
import { AuthorizeGuard } from '../../api-authorization/authorize.guard';
import { EventDetailsComponent } from './event-details/event-details.component';

const routes: Routes = [
  {
    path: 'global',
    component: PlannerViewComponent
  },
  {
    path: 'global/add',
    component: EventDetailsComponent
  },
  {
    path: 'global/:id',
    component: EventDetailsComponent
  },
  {
    path: 'personal',
    component: PlannerViewComponent,
    canActivate: [AuthorizeGuard]
  },
  {
    path: 'personal/add',
    component: EventDetailsComponent,
    canActivate: [AuthorizeGuard]
  },
  {
    path: 'personal/:id',
    component: EventDetailsComponent,
    canActivate: [AuthorizeGuard]
  },
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class PlannerRoutingModule {
}
