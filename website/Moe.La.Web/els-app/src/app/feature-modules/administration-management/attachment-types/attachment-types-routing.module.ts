import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AttachmentTypeListComponent } from './attachment-type-list/attachment-type-list.component';

const routes: Routes = [{
  path: '',
  component: AttachmentTypeListComponent,
  data: {
    title: 'أنواع المرفقات',
    urls: [{ title: 'الرئيسية', url: '/' }, { title: 'أنواع المرفقات' }],
  },
},];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttachmentTypesRoutingModule { }