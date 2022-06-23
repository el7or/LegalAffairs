import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UserListComponent } from './user-list/user-list.component';
import { UserFormComponent } from './user-form/user-form.component';
import { UserViewComponent } from './user-view/user-view.component';
import { AppRole } from 'app/core/models/role';
import { RoleGuard } from 'app/core/guards/role.guard';

const routes: Routes = [
  {
    path: '',
    component: UserListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'المستخدمين',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المستخدمين' }],
      // roles: [AppRole.Admin],
      rolesDepartment: [{ role: AppRole.Admin, departmentIds: null }]
    },
  },
  {
    path: 'new',
    component: UserFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إعداد مستخدم جديد',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المستخدمين', url: 'users' }, { title: 'إعداد مستخدم جديد' }],
      //roles: [AppRole.Admin],
      rolesDepartment: [{ role: AppRole.Admin, departmentIds: null }]
    },
  },
  {
    path: 'edit/:id',
    component: UserFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تعديل بيانات مستخدم',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المستخدمين', url: 'users' }, { title: 'تعديل بيانات مستخدم' }],
      rolesDepartment: [{ role: AppRole.Admin, departmentIds: null }]
    },
  },
  {
    path: 'view/:id',
    component: UserViewComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'تفاصيل بيانات مستخدم',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المستخدمين', url: 'users' }, { title: 'تفاصيل بيانات مستخدم' }],
      rolesDepartment: [{ role: AppRole.Admin, departmentIds: null }]
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UsersRoutingModule { }
