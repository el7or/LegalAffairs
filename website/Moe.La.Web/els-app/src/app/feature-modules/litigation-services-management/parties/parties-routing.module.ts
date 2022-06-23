import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PartyListComponent } from './party-list/party-list.component';

const routes: Routes = [
  {
    path: '',
    component: PartyListComponent,
    data: {
      title: 'الخصوم',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'الخصوم' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PartiesRoutingModule { }
