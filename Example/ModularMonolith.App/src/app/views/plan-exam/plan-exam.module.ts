import { NgModule } from '@angular/core';
import {PlanExamComponent} from './plan-exam.component';
import {RouterModule} from '@angular/router';
import {routes} from './plan-exam-routing.module';

@NgModule({
  declarations: [
    PlanExamComponent
  ],
  imports: [
    RouterModule.forChild(routes),
  ],
  providers: []
})
export class PlanExamModule {

}
