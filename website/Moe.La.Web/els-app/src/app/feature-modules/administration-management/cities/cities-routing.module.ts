import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CityListComponent } from './city-list/city-list.component';

const routes: Routes = [
  {
    path: '',
    component: CityListComponent,
    data: {
      title: 'المدن',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المدن' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CitiesRoutingModule { }
