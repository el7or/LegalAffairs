import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { IdentityTypeListComponent } from './identity-type-list/identity-type-list.component';

const routes: Routes = [
  {
    path: '',
    component: IdentityTypeListComponent,
    data: {
      title: 'أنواع الهوية',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'أنواع الهوية' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IdentityTypesRoutingModule {}
