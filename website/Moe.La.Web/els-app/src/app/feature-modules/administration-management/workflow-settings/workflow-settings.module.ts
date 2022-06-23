import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { WorkflowSettingsRoutingModule } from './workflow-settings-routing.module';
import { WorkflowTypeListComponent } from './workflow-types/workflow-type-list/workflow-type-list.component';
import { WorkflowTypeFormComponent } from './workflow-types/workflow-type-form/workflow-type-form.component';
import { WorkflowStatusListComponent } from './workflow-statuses/workflow-status-list/workflow-status-list.component';
import { WorkflowStatusFormComponent } from './workflow-statuses/workflow-status-form/workflow-status-form.component';
import { WorkflowActionListComponent } from './workflow-types/workflow-type-form/workflow-actions/workflow-action-list/workflow-action-list.component';
import { WorkflowActionFormComponent } from './workflow-types/workflow-type-form/workflow-actions/workflow-action-form/workflow-action-form.component';
import { WorkflowStepFormComponent } from './workflow-types/workflow-type-form/workflow-steps/workflow-step-form/workflow-step-form.component'
import { WorkflowStepListComponent } from './workflow-types/workflow-type-form/workflow-steps/workflow-step-list/workflow-step-list.component';
import { WorkflowTypeComponent } from './workflow-types/workflow-type-form/workflow-type/workflow-type.component';
import { WorkflowTypeResolver } from './workflow-types/workflow-type-form/resolvers/workflow-type.resolver';
import { WorkflowTypeViewComponent } from './workflow-types/workflow-type-view/workflow-type-view.component';
import { WorkflowActionsListComponent } from './workflow-types/workflow-type-view/workflow-actions-list/workflow-actions-list.component';
import { WorkflowStepsListComponent } from './workflow-types/workflow-type-view/workflow-steps-list/workflow-steps-list.component';
import { WorkflowStepActionsComponent } from './workflow-types/workflow-type-view/workflow-step-actions-list/workflow-step-actions-list.component';

@NgModule({
  declarations: [
    WorkflowTypeListComponent,
    WorkflowTypeFormComponent,
    WorkflowStatusListComponent,
    WorkflowStatusFormComponent,
    WorkflowActionListComponent,
    WorkflowActionFormComponent,
    WorkflowStepFormComponent,
    WorkflowStepListComponent,
    WorkflowTypeComponent,
    WorkflowTypeViewComponent,
    WorkflowActionsListComponent,
    WorkflowStepsListComponent,
    WorkflowStepActionsComponent
  ],
  imports: [
    SharedModule,
    WorkflowSettingsRoutingModule
  ],
  providers: [
    WorkflowTypeResolver
  ]
})
export class WorkflowSettingsModule { }
