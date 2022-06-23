import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LegalBoardListComponent } from './legal-board-list/legal-board-list.component';
import { LegalBoardFormComponent } from './legal-board-form/legal-board-form.component';
import { AppRole } from 'app/core/models/role';
import { LegalBoardViewComponent } from './legal-board-view/legal-board-view.component';

const routes: Routes = [
  {
    path: 'list',
    component: LegalBoardListComponent,
    data: {
      title: 'تشكيل اللجان',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'تشكيل اللجان' }],
      rolesDepartment: [{ role: AppRole.MainBoardHead, departmentIds: null }]
    },
  },
  {
    path: 'new',
    component: LegalBoardFormComponent,
    data: {
      title: 'إعداد لجنة جديدة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'تشكيل اللجان', url: 'legal-boards/list' }, { title: 'إعداد لجنة جديدة' }],
      rolesDepartment: [{ role: AppRole.MainBoardHead, departmentIds: null }]
    },
  },
  {
    path: 'edit/:id',
    component: LegalBoardFormComponent,
    data: {
      title: 'تعديل بيانات لجنة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'اللجان القضائية', url: 'legal-boards/list' }, { title: 'تعديل بيانات لجنة' }],
      rolesDepartment: [
        { role: AppRole.MainBoardHead, departmentIds: null }]
    },
  },
  {
    path: 'view/:id',
    component: LegalBoardViewComponent,
    data: {
      title: 'تفاصيل لجنة',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'اللجان القضائية', url: 'legal-boards/view' }, { title: 'تفاصيل لجنة' }],
      rolesDepartment: [
        { role: AppRole.MainBoardHead, departmentIds: null }]
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LegalBoardsListRoutingModule { }
