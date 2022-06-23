import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WorkItemTypeListComponent } from './work-item-type-list/work-item-type-list.component';

const routes: Routes = [
  {
    path: '',
    component: WorkItemTypeListComponent,
    data: {
      title: 'أنواع العمل',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'أنواع العمل' }],
    },
  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkItemTypeRoutingModule { }
