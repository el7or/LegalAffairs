import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CourtListComponent } from './court-list/court-list.component';

const routes: Routes = [
  {
    path: '',
    component: CourtListComponent,
    data: {
      title: 'المحاكم',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المحاكم' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CourtsListRoutingModule {}
