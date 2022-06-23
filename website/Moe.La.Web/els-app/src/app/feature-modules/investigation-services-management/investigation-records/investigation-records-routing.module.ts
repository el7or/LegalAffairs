import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RoleGuard } from 'app/core/guards/role.guard';
import { AppRole } from 'app/core/models/role';
import { InvestigationRecordFormComponent } from './investigation-record-form/investigation-record-form.component';
import { InvestigationRecordListComponent } from './investigation-record-list/investigation-record-list.component';

const routes: Routes = [
  {
    path: '',
    component: InvestigationRecordListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'محاضر التحقيقات',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'محاضر التحقيقات' }],
      // roles: [AppRole.Investigator],
      rolesDepartment: [{ role: AppRole.Investigator, departmentIds: null }]
    },
  },
  {
    path: 'new/:investigationId',
    component: InvestigationRecordFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إضافة محضر تحقيق',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'محاضر التحقيقات', url: '/investigation-records' }, { title: 'إضافة محضر تحقيق' }],
      roles: [AppRole.Investigator],
      rolesDepartment: [{ role: AppRole.Investigator, departmentIds: null }]
    },
  },
  {
    path: 'edit/:id',
    component: InvestigationRecordFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تعديل محضر تحقيق',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'محاضر التحقيقات', url: '/investigation-records' }, { title: 'تعديل محضر تحقيق' }],
      // roles: [AppRole.Investigator],
      rolesDepartment: [{ role: AppRole.Investigator, departmentIds: null }]
    },
  },


];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InvestigationRecordsRoutingModule { }
