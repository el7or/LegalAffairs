import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { FeatureModulesRoutingModule } from './feature-modules-routing.module';
import { FeatureModulesComponent } from './feature-modules.component';
import { TasksWaitingComponent } from './litigation-services-management/tasks/tasks-waiting/tasks-waiting.component';
import { TasksCompletedComponent } from './litigation-services-management/tasks/tasks-completed/tasks-completed.component';

@NgModule({
  declarations: [FeatureModulesComponent, TasksWaitingComponent, TasksCompletedComponent],
  imports: [
    SharedModule,
    FeatureModulesRoutingModule
  ],
})
export class FeatureModulesModule { }
