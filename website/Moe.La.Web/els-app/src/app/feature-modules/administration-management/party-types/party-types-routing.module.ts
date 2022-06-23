import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PartyTypeListComponent } from './party-type-list/party-type-list.component';

const routes: Routes = [{
  path: '',
  component: PartyTypeListComponent,
  data: {
    title: 'أنواع الخصوم',
    urls: [{ title: 'الرئيسية', url: '/' }, { title: 'أنواع الخصوم' }],
  },
},];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PartyTypesRoutingModule { }
