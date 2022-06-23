import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FieldMissionTypeListComponent } from './field-mission-type-list/field-mission-type-list.component';

const routes: Routes = [
  {
    path: '',
    component: FieldMissionTypeListComponent,
    data: {
      title: 'المهام الميدانية',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المهام الميدانية' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FieldMissionTypesRoutingModule {}
