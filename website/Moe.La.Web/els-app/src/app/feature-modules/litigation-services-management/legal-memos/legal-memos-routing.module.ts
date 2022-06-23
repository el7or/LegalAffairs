import { BoardMeetingViewComponent } from './board-meeting-view/board-meeting-view.component';
import { BoardMeetingListComponent } from './board-meeting-list/board-meeting-list.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AppRole } from 'app/core/models/role';
import { LegalMemoViewComponent } from './legal-memo-view/legal-memo-view.component';
import { LegalMemoFormComponent } from './legal-memo-form/legal-memo-form.component';
import { LegalMemoListComponent } from './legal-memo-list/legal-memo-list.component';
import { LegalMemoReviewListComponent } from './legal-memo-review-list/legal-memo-review-list.component';
import { LegalMemoBoardReviewListComponent } from './legal-memo-board-review-list/legal-memo-board-review-list.component';
import { Department } from 'app/core/enums/Department';
import { RoleGuard } from 'app/core/guards/role.guard';

const routes: Routes = [
  {
    path: 'list',
    component: LegalMemoListComponent,
    data: {
      title: 'المذكرات',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المذكرات' }],
    },
  },
  {
    path: 'review-list',
    component: LegalMemoReviewListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'مذكرات للمراجعة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'مذكرات للمراجعة' }],
      rolesDepartment: [{ role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] },
      { role: AppRole.SubBoardHead, departmentIds: null }]
    },
  },
  {
    path: 'review/:id',
    component: LegalMemoViewComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'عرض بيانات مذكرة للمراجعة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'مذكرات للمراجعة', url: 'memos/review-list' }, { title: 'عرض بيانات مذكرة للمراجعة' }],
      rolesDepartment: [{ role: AppRole.LegalConsultant, departmentIds: [Department.Litigation] },
      { role: AppRole.SubBoardHead, departmentIds: null }]
    },
  },
  {
    path: 'board-review-list',
    component: LegalMemoBoardReviewListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'المذكرات الواردة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المذكرات الواردة' }],
      rolesDepartment: [
        { role: AppRole.MainBoardHead, departmentIds: null }]
    },
  },
  {
    path: 'board-review/:id',
    component: LegalMemoViewComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'عرض بيانات مذكرة للمراجعة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المذكرات الواردة', url: 'memos/board-review-list' }, { title: 'عرض بيانات مذكرة للمراجعة' }],
      rolesDepartment: [
        { role: AppRole.MainBoardHead, departmentIds: null }]
    },
  },
  {
    path: 'new',
    component: LegalMemoFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إعداد مذكرة جديدة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المذكرات', url: 'memos/list' }, { title: 'إعداد مذكرة جديدة' }],
      rolesDepartment: [
        { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] }
      ]
    },
  },
  {
    path: 'edit/:id',
    component: LegalMemoFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تعديل بيانات مذكرة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المذكرات', url: 'memos/list' }, { title: 'تعديل بيانات مذكرة' }],
      rolesDepartment: [
        { role: AppRole.LegalResearcher, departmentIds: [Department.Litigation] }
      ]
    },
  },
  {
    path: 'view/:id',
    component: LegalMemoViewComponent,
    data: {
      title: 'عرض بيانات مذكرة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المذكرات', url: 'memos/list' }, { title: 'عرض بيانات مذكرة' }],
    },
  },
  {
    path: 'meetings',
    component: BoardMeetingListComponent,
    data: {
      title: 'اجتماعات اللجان',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'اجتماعات اللجان' }],
    },
  },
  {
    path: 'meeting-view/:id',
    component: BoardMeetingViewComponent,
    data: {
      title: 'تفاصيل الاجتماع',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'اجتماعات اللجان', url: 'memos/meetings' }, { title: 'تفاصيل الاجتماع' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LegalMemosRoutingModule { }
