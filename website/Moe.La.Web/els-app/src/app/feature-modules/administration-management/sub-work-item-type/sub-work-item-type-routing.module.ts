import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SubWorkItemTypeListComponent } from './sub-work-item-type-list/sub-work-item-type-list.component';

const routes: Routes = [{
  path: '',
  component: SubWorkItemTypeListComponent,
  data: {
    title: 'أنواع العمل الفرعية',
    urls: [{ title: 'الرئيسية', url: '/' }, { title: 'أنواع العمل' }],
  }
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SubWorkItemTypeRoutingModule { }
