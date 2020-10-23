import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlannerViewComponent } from './planner-view/planner-view.component';

const routes: Routes = [
  {
    path: '',
    component: PlannerViewComponent
  }
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
