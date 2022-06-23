import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProvinceListComponent } from './province-list/province-list.component';

const routes: Routes = [
  {
    path: '',
    component: ProvinceListComponent,
    data: {
      title: 'المناطق',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المناطق' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProvinciesRoutingModule { }
