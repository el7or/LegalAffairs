import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InvestigationRecordPartyTypeListComponent } from './investigation-record-party-type-list/investigation-record-party-type-list.component';

const routes: Routes = [
  {
    path: '',
    component: InvestigationRecordPartyTypeListComponent,
    data: {
      title: 'أنواع الطرف',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'أنواع الطرف' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvestigationRecordPartyTypeRoutingModule { }
