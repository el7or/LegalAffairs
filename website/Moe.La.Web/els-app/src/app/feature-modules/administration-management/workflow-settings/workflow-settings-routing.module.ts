import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WorkflowTypeListComponent } from './workflow-types/workflow-type-list/workflow-type-list.component';
import { WorkflowTypeFormComponent } from './workflow-types/workflow-type-form/workflow-type-form.component';
import { WorkflowStatusListComponent } from './workflow-statuses/workflow-status-list/workflow-status-list.component';
import { WorkflowStatusFormComponent } from './workflow-statuses/workflow-status-form/workflow-status-form.component';
import { WorkflowTypeResolver } from './workflow-types/workflow-type-form/resolvers/workflow-type.resolver';
import { WorkflowTypeViewComponent } from './workflow-types/workflow-type-view/workflow-type-view.component';

const routes: Routes = [
  {
    path: 'workflow-types',
    component: WorkflowTypeListComponent,
    data: {
      title: 'أنواع سير العمل',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'أنواع سير العمل' }],
    }
  },
  {
    path: 'workflow-types/new',
    component: WorkflowTypeFormComponent,
    data: {
      title: 'إنشاء نوع سير عمل جديد',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'أنواع سير العمل', url: 'workflow-settings/workflow-types' },
        { title: 'إنشاء نوع سير عمل جديد' },
      ],
    }
  },
  {
    path: 'workflow-types/edit/:id',
    component: WorkflowTypeFormComponent,
    data: {
      title: 'تعديل نوع سير عمل',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'أنواع سير العمل', url: 'workflow-settings/workflow-types' },
        { title: 'تعديل نوع سير عمل' },
      ],
    },
    resolve: {
      workflowTypeView: WorkflowTypeResolver
    }
  },
  {
    path: 'workflow-types/view/:id',
    component: WorkflowTypeViewComponent,
    data: {
      title: 'تفاصيل نوع سير عمل',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'أنواع سير العمل', url: 'workflow-settings/workflow-types' },
        { title: 'تفاصيل نوع سير عمل' },
      ],
    },
    resolve: {
      workflowTypeView: WorkflowTypeResolver
    }
  },
  {
    path: 'workflow-statuses',
    component: WorkflowStatusListComponent,
    data: {
      title: 'حالات سير العمل',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'حالات سير العمل' }],
    }
  },
  {
    path: 'workflow-statuses/new',
    component: WorkflowStatusFormComponent,
    data: {
      title: 'حالات سير العمل',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'حالات سير العمل', url: 'workflow-settings/workflow-statuses' },
        { title: 'إنشاء إجراء سير عمل جديد' },
      ],
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkflowSettingsRoutingModule { }
