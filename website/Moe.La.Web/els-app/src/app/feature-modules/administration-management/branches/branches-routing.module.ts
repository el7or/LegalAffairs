import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { BranchListComponent } from './branch-list/branch-list.component';

const routes: Routes = [
  {
    path: '',
    component: BranchListComponent,
    data: {
      title: 'الفروع',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'الفروع' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BranchesRoutingModule {}
