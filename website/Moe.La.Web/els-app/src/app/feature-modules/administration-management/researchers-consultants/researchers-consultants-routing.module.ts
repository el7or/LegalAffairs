import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RoleGuard } from 'app/core/guards/role.guard';
import { AppRole } from 'app/core/models/role';
import { ResearcherListComponent } from './researcher-list/researcher-list.component';
import { Department } from 'app/core/enums/Department';

const routes: Routes = [
  {
    path: '',
    component: ResearcherListComponent,
    canActivate: [RoleGuard],
    data: {
      title: 'ربط الباحثين بالمستشار',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'ربط الباحثين بالمستشار' }],
      // roles: [AppRole.LitigationManager],
      rolesDepartment:[
        {role:AppRole.DepartmentManager,departmentIds:[Department.Litigation]}]
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResearchersConsultantsRoutingModule { }
