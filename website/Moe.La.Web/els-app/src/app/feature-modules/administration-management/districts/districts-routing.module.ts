import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DistrictListComponent } from './district-list/district-list.component';

const routes: Routes = [{
  path: '',
  component: DistrictListComponent,
  data: {
    title: 'الأحياء',
    urls: [{ title: 'الرئيسية', url: '/' }, { title: 'الأحياء' }],
  },
},];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DistrictsRoutingModule { }
