import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AppRole } from 'app/core/models/role';
import { RoleGuard } from 'app/core/guards/role.guard';
import { HearingListComponent } from '../hearings/hearing-list/hearing-list.component';
import { HearingFormComponent } from '../hearings/hearing-form/hearing-form.component';
import { HearingSummaryComponent } from '../hearings/hearing-summary/hearing-summary.component';
import { HearingReceivingJudgmentComponent } from '../hearings/hearing-receiving-judgment/hearing-receiving-judgment.component';
import { ReceiveJudgmentInstrumentComponent } from '../hearings/receive-judgment-instrument/receive-judgment-instrument.component';
import { HearingDetailsComponent } from '../hearings/hearing-details/hearing-details.component';
import { HearingUpdatesListComponent } from '../hearings/hearing-updates-list/hearing-updates-list.component';
import { Department } from 'app/core/enums/Department';

const routes: Routes = [

  {
    path: '',
    component: HearingListComponent,
    data: {
      title: 'الجلسات',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'الجلسات' }],
      rolesDepartment: [{ role: AppRole.BranchManager, specializedDepartmentIds: null },
      { role: AppRole.GeneralSupervisor, specializedDepartmentIds: null },
      { role: AppRole.LegalConsultant, specializedDepartmentIds: [Department.Litigation] },
      { role: AppRole.LegalResearcher, specializedDepartmentIds: [Department.Litigation] },
      { role: AppRole.DepartmentManager, specializedDepartmentIds: [Department.Litigation] },
      { role: AppRole.RegionsSupervisor, specializedDepartmentIds: null },
      { role: AppRole.Admin, specializedDepartmentIds: null },
      { role: AppRole.AdministrativeCommunicationSpecialist, specializedDepartmentIds: null },
      ]
    },
  },
  {
    path: 'new',
    component: HearingFormComponent,
    data: {
      title: 'إضافة جلسة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'إضافة جلسة' },
      ],
      roles: [AppRole.LegalResearcher],
    },
  },
  {
    path: 'edit/:id',
    component: HearingFormComponent,
    data: {
      title: 'تعديل جلسة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'تعديل جلسة' },
      ],
    },
  },
  {
    path: 'summary/:id',
    component: HearingSummaryComponent,
    data: {
      title: 'إغلاق جلسة منتهية',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'إغلاق جلسة منتهية' },
      ],
    },
  },
  {
    path: 'judgment/:id',
    component: HearingReceivingJudgmentComponent,
    data: {
      title: 'موعد استلام الحكم',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'إضافة موعد استلام الحكم' },
      ],
    },
  },
  {
    path: 'judgmentInstrument/:id',
    component: ReceiveJudgmentInstrumentComponent,
    data: {
      title: 'استلام صك الحكم',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'استلام صك الحكم' },
      ],
    },
  },
  {
    path: 'new/:caseId',
    component: HearingFormComponent,
    data: {
      title: 'إضافة جلسة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'إضافة جلسة' },
      ],
    },
  },
  {
    path: 'view/:id',
    component: HearingDetailsComponent,
    //canActivate: [RoleGuard],
    data: {
      title: 'تفاصيل الجلسة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'تفاصيل الجلسة' },
      ],
      roles: [AppRole.LegalResearcher],
    },
  },
  {
    path: 'hearing-updates',
    component: HearingUpdatesListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تحديثات الجلسة',
      urls: [
        { title: 'الرئيسية', url: '/' },
        { title: 'الجلسات', url: 'hearings' },
        { title: 'تحديثات الجلسة' },
      ],
      roles: [AppRole.LegalResearcher],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HearingsRoutingModule { }
