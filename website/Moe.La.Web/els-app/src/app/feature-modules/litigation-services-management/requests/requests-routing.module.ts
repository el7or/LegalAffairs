import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RequestViewComponent } from './request-view/request-view.component';
import { RequestFormComponent } from './request-form/request-form.component';
import { RequestListComponent } from './request-list/request-list.component';
import { AttachedLetterRequestFormComponent } from './attached-letter-request-form/attached-letter-request-form.component';
import { AppRole } from 'app/core/models/role';
import { RoleGuard } from 'app/core/guards/role.guard';
import { Department } from 'app/core/enums/Department';
import { ExportCaseJudgmentRequestFormComponent } from './export-case-judgment-request-form/export-case-judgment-request-form.component';
import { ConsultationSupportingDocumentFormComponent } from './consultation-request-form/consultation-request-form.component';
import { SupportingDocumentRequestWizardComponent } from './supporting-document-request-wizard/supporting-document-request-wizard.component';
import { ChangeHearingResearcherRequestFormComponent } from './change-hearing-researcher-request-form/change-hearing-researcher-request-form.component';

const routes: Routes = [
  {
    path: '',
    component: RequestListComponent,
    data: {
      title: 'جدول طلباتي',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'طلباتي' }],
    },
  },
  {
    path: 'change-researcher-request',
    component: RequestFormComponent,
    data: {
      title: 'إضافة طلب تغيير باحث',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'إضافة طلب جديد' },
      ],
    },
  },
  {
    path: 'change-hearing-researcher-request/:hearingId',
    component: ChangeHearingResearcherRequestFormComponent,
    data: {
      title: 'طلب تغيير المكلف بحضور الجلسة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'طلب تغيير المكلف بحضور الجلسة' },
      ],
    },
  },
  {
    path: 'view/:id/:type',
    component: RequestViewComponent,
    data: {
      title: 'عرض بيانات طلب',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'عرض بيانات طلب' },
      ],
    },
  },
  {
    path: 'review/:id/:type',
    component: RequestViewComponent,
    data: {
      title: 'مراجعة بيانات طلب',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'مراجعة بيانات طلب' },
      ],
    },
  },
  {
    path: 'attached-letter/:parentId',
    component: AttachedLetterRequestFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إضافة خطاب إلحاقى',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'إضافة خطاب إلحاقي' },
      ],
      rolesDepartment: [{ role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] }]
    },
  },
  {
    path: 'edit-attached-letter/:id',
    component: AttachedLetterRequestFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إعادة صياغة طلب خطاب إلحاقي',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'إعادة صياغة طلب خطاب إلحاقي' },
      ],
      rolesDepartment: [{ role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] }]
    },
  },
  {
    path: 'document-request/reformulate/:requestId',
    component: SupportingDocumentRequestWizardComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إعادة صياغة طلب المستندات الداعمة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'إعادة صياغة طلب المستندات الداعمة' },
      ],
      rolesDepartment: [{ role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] }]
    },
  },

  {
    path: 'document-request/edit/:requestId',
    component: SupportingDocumentRequestWizardComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تعديل طلب المستندات الداعمة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'تعديل طلب المستندات الداعمة' },
      ],
      rolesDepartment: [{ role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] }]
    },
  },
  {
    path: 'document-request/:hearingId/:caseId',
    component: SupportingDocumentRequestWizardComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'طلب مستندات داعمة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'طلب مستندات داعمة' },
      ],
      rolesDepartment: [{ role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] }]
    },
  },
  {
    path: 'document-request/related/edit/:parentId',
    component: AttachedLetterRequestFormComponent,
    // canActivate: [RoleGuard],
    data: {
      title: 'إعادة صياغةالخطاب الإلحاقى',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'إعادة صياغة الخطاب الإلحاقى' },
      ],
      rolesDepartment: [{ role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] }]
    },
  },
  {
    path: 'export-case-judgment-request',
    component: ExportCaseJudgmentRequestFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'طلب تصدير نموذج الحكم للجهة المعنية',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'طلب تصدير نموذج الحكم للجهة المعنية' },
      ],
      rolesDepartment: [
        { role: AppRole.DepartmentManager, departmentIds: [Department.Litigation] },
        { role: AppRole.AdministrativeCommunicationSpecialist}
      ]
    },
  },
  {
    path: 'export-case-judgment-request/reformulate/:id',
    component: ExportCaseJudgmentRequestFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إعادة صياغة طلب تصدير نموذج الحكم للجهة المعنية',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'إعادة صياغة طلب تصدير نموذج الحكم للجهة المعنية' },
      ],
      // roles: [AppRole.LitigationManager],
      rolesDepartment: [{ role: AppRole.DepartmentManager, departmentIdss: [Department.Litigation] }]
    },
  },
  {
    path: 'export-case-judgment-request/edit/:id',
    component: ExportCaseJudgmentRequestFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تعديل طلب تصدير نموذج الحكم للجهة المعنية',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'تعديل طلب تصدير نموذج الحكم للجهة المعنية' },
      ],
      rolesDepartment: [{ role: AppRole.DepartmentManager, departmentIdss: [Department.Litigation] }]
    },
  },
  {
    path: 'consultation-request/reformulate/:id',
    component: ConsultationSupportingDocumentFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تعديل طلب نواقص',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'تعديل طلب نواقص' },
      ],
      rolesDepartment: [{ role: AppRole.Investigator, departmentIds: null },
      { role: AppRole.LegalResearcher, departmentIds: [-1] },
      { role: AppRole.LegalConsultant, departmentIds: [-1] }]
    },
  },
  {
    path: 'consultation-request/:consultationId/:moamalaId',
    component: ConsultationSupportingDocumentFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'طلب نواقص',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'طلباتي', url: 'requests' },
        { title: 'طلب نواقص' },
      ],
      rolesDepartment: [{ role: AppRole.Investigator, departmentIds: null },
      { role: AppRole.LegalResearcher, departmentIds: [-1] },
      { role: AppRole.LegalConsultant, departmentIds: [-1] }]
    },
  },


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RequestsRoutingModule { }
