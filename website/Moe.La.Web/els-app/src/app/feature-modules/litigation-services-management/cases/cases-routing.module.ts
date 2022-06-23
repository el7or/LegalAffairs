import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CaseListComponent } from './case-list/case-list.component';
import { CaseViewComponent } from './case-view/case-view.component';
import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
import { RoleGuard } from 'app/core/guards/role.guard';
import { CreateCaseComponent } from './create-case/create-case.component';
import { JudgmentReceivedComponent } from './judgment-received/judgment-received.component';

const routes: Routes = [
  {
    path: '',
    component: CaseListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'سجل القضايا',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'سجل القضايا' }
      ],
      rolesDepartment: [
        { role: AppRole.BranchManager },
        { role: AppRole.GeneralSupervisor },
        { role: AppRole.LegalConsultant, departmentId: Department.Litigation },
        { role: AppRole.LegalResearcher, departmentId: Department.Litigation },
        { role: AppRole.DepartmentManager, departmentId: Department.Litigation },
        { role: AppRole.RegionsSupervisor },
        { role: AppRole.AdministrativeCommunicationSpecialist },
        { role: AppRole.DataEntry }
      ]
    },
  },
  {
    path: 'view/:id',
    component: CaseViewComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تفاصيل قضية',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'القضايا', url: 'cases' },
        { title: 'تفاصيل قضية' },
      ],
      rolesDepartment: [
        { role: AppRole.GeneralSupervisor },
        { role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] },
        { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] },
        { role: AppRole.RegionsSupervisor, departmentIds: null },
        { role: AppRole.BranchManager, departmentIds: null },
        { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] },
        { role: AppRole.SubBoardHead, departmentIds: null },
        { role: AppRole.MainBoardHead, departmentIds: null },
        { role: AppRole.DataEntry }
      ]
    },
  },
  {
    path: 'create',
    component: CreateCaseComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إنشاء قضية',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'القضايا', url: 'cases' },
        { title: 'إنشاء قضية' },
      ],
      rolesDepartment: [
        { role: AppRole.DepartmentManager, departmentId: Department.Litigation },
        { role: AppRole.DataEntry }
      ]
    },
  },
  {
    path: 'edit/:id',
    component: CreateCaseComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تعديل بيانات قضية',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'القضايا', url: 'cases' },
        { title: 'تعديل بيانات قضية' },
      ],
      rolesDepartment: [
        { role: AppRole.DataEntry },
        { role: AppRole.DepartmentManager, departmentId: Department.Litigation },
        { role: AppRole.LegalResearcher, departmentId: Department.Litigation },
      ]
    },
  },
  {
    path: ':id/judgment-received',
    component: JudgmentReceivedComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'استلام صك الحكم',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'استلام صك الحكم' },
      ],
      rolesDepartment: [
        { role: AppRole.DataEntry },
        { role: AppRole.DepartmentManager, departmentId: Department.Litigation },
        { role: AppRole.LegalResearcher, departmentId: Department.Litigation },
      ]
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CasesRoutingModule { }
