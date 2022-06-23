import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { Department } from 'app/core/enums/Department';
import { RoleGuard } from 'app/core/guards/role.guard';
import { AppRole } from 'app/core/models/role';
import { MoamalatInboundListComponent } from './moamalat-inbound-list/moamalat-inbound-list.component';
import { MoamalatOutboundListComponent } from './moamalat-outbound-list/moamalat-outbound-list.component';

const routes: Routes = [
  {
    path: 'outbound',
    component: MoamalatOutboundListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'المعاملات الصادرة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'المعاملات' },
        { title: 'المعاملات الصادرة', url: 'moamalat/outbound' },
      ],
      // roles: [
      //   AppRole.GeneralSupervisor,
      // ],
      rolesDepartment: [
        { role: AppRole.GeneralSupervisor, departmentIds: null }]
    },
  },
  {
    path: 'inbound',
    component: MoamalatInboundListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'المعاملات الواردة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'المعاملات' },
        { title: 'المعاملات الواردة', url: 'moamalat/inbound' },
      ],
      // roles: [
      //   AppRole.LitigationManager,
      // ],
      rolesDepartment: [{ role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] }]
    },
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MoamalatTransactionsRoutingModule { }
