import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConsultationFormRegulationsAndLawsComponent } from './consultation-form-regulations-and-laws/consultation-form-regulations-and-laws.component';
import { Department } from 'app/core/enums/Department';
import { RoleGuard } from 'app/core/guards/role.guard';
import { AppRole } from 'app/core/models/role';
import { ConsultationFormComponent } from './consultation-form/consultation-form.component';
import { ConsultationListComponent } from './consultation-list/consultation-list.component';
import { ConsultationReviewViewComponent } from './consultation-review-view/consultation-review-view.component';

const routes: Routes = [
  {
    path: '',
    component: ConsultationListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'الإدارات الاستشارية',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'الإدارات الاستشارية' }],
      rolesDepartment: [
        { role: AppRole.LegalResearcher, departmentIds: [Department.Consulting, Department.Contracts, Department.Grievances, Department.HumanRights, Department.RegulationsAndLaws] },
        { role: AppRole.GeneralSupervisor },
        { role: AppRole.Distributor },
        { role: AppRole.DepartmentManager, departmentIds: [Department.Consulting, Department.Contracts, Department.Grievances, Department.HumanRights, Department.RegulationsAndLaws] }
      ]
    },

  },
  {
    path: 'edit/:id',
    component: ConsultationFormComponent,
    data: {
      title: 'تعديل النموذج',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'النماذج', url: 'consultation' },
        { title: 'تعديل النموذج' },
      ],
    },
  },
  {
    path: 'new/:moamalaId',
    component: ConsultationFormComponent,
    data: {
      title: 'اعداد النموذج',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'اعداد النموذج' },
      ],
    },
  },
  {
    path: 'review/:id',
    component: ConsultationReviewViewComponent,
    data: {
      title: 'مراجعة النموذج',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'النماذج', url: 'consultation' },
        { title: 'مراجعة النموذج' },
      ],
    },
  },
  {
    path: 'new-laws/:moamalaId',
    component: ConsultationFormRegulationsAndLawsComponent,
    data: {
      title: 'اعداد نموذج اللوائح والقرارات',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'النماذج', url: 'consultation' },
        { title: 'اعداد نموذج اللوائح والقرارات' },
      ],
    },
  },
  {
    path: 'edit-laws/:id',
    component: ConsultationFormRegulationsAndLawsComponent,
    data: {
      title: 'تعديل نموذج اللوائح والقرارات',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'النماذج', url: 'consultation' },
        { title: 'تعديل نموذج اللوائح والقرارات' },
      ],
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConsultationServicesManagementRoutingModule { }
