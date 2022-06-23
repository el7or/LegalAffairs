import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MinistryDepartmentListComponent } from './ministry-departments-list/ministry-department-list.component';

const routes: Routes = [
  {
    path: '',
    component: MinistryDepartmentListComponent,
    data: {
      title: 'إدارات الوزارة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'إدارات الوزارة' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MinistryDepartmentsRoutingModule { }
