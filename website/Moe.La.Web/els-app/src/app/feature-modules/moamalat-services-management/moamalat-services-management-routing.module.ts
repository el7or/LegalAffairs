import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MoamalatFormComponent } from './moamalat-form/moamalat-form.component';
import { MoamalatViewComponent } from './moamalat-view/moamalat-view.component';
import { MoamalatListComponent } from './moamalat-list/moamalat-list.component';
import { AppRole } from 'app/core/models/role';
import { RoleGuard } from 'app/core/guards/role.guard';

const routes: Routes = [
  {
    path: '',
    component: MoamalatListComponent,
    data: {
      title: 'معاملاتى',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'معاملاتى' }],
    },
  },
  {
    path: 'view/:id',
    component: MoamalatViewComponent,
    data: {
      title: 'تفاصيل المعاملة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'معاملاتى', url: 'moamalat' },
        { title: 'تفاصيل المعاملة' },
      ],
    },
  },
  {
    path: 'new',
    component: MoamalatFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إضافة معاملة جديدة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'معاملاتى', url: 'moamalat' },
        { title: 'إضافة معاملة جديدة' },
      ],
      rolesDepartment: [{ role: AppRole.GeneralSupervisor },
      { role: AppRole.Distributor }],
    },
  },
  {
    path: 'edit/:id',
    component: MoamalatFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تعديل بيانات المعاملة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'معاملاتى', url: 'moamalat' },
        { title: 'تعديل بيانات المعاملة' },
      ],
      rolesDepartment: [{ role: AppRole.GeneralSupervisor },
      { role: AppRole.Distributor }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MoamalatServicesManagementRoutingModule { }
