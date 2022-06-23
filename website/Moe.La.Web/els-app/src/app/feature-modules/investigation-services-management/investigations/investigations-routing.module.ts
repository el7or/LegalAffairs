import { InvestigationFormComponent } from './investigation-form/investigation-form.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RoleGuard } from 'app/core/guards/role.guard';
import { AppRole } from 'app/core/models/role';
import { InvestigationListComponent } from './investigation-list/investigation-list.component';
import { Department } from 'app/core/enums/Department';

const routes: Routes = [
  {
    path: '',
    component: InvestigationListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'التحقيقات',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'التحقيقات' }],
      // roles: [
      //   AppRole.Investigator,
      //   AppRole.InvestigationManager,
      //   AppRole.GeneralSupervisor,
      // ],
      rolesDepartment:[
        {role:AppRole.Investigator,departmentIds:null},
        {role:AppRole.DepartmentManager,departmentIds:[Department.Investiation]},
        {role:AppRole.GeneralSupervisor,departmentIds:null}
      ]
    },
  },
  {
    path: 'new',
    component: InvestigationFormComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'إضافة تحقيق',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: ' التحقيقات' , url:'/investigations'}, { title: 'إضافة تحقيق' }],
      //roles: [AppRole.InvestigationManager],
      rolesDepartment:[{role:AppRole.DepartmentManager,departmentId:Department.Investiation}]
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InvestigationsRoutingModule {}
