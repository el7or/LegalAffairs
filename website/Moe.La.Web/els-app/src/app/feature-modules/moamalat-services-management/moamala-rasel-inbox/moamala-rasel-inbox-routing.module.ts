import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRole } from 'app/core/models/role';
import { MoamalatRaselInboxListComponent } from './moamalat-rasel-inbox-list/moamalat-rasel-inbox-list.component';

const routes: Routes = [
  {
    path: '',
    component: MoamalatRaselInboxListComponent,
    data: {
      title: 'صندوق وارد راسل',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'صندوق وارد راسل' }],
      rolesDepartment: [
        { role: AppRole.Distributor, departmentId: null },
        { role: AppRole.GeneralSupervisor, departmentId: null },
      ]
    },
  },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MoamalaRaselInboxRoutingModule { }
