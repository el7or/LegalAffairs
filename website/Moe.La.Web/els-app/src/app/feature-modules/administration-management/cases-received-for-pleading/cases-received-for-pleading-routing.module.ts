import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Department } from 'app/core/enums/Department';
import { AppRole } from 'app/core/models/role';
import { CasesReceivedForPleadingComponent } from './cases-received-for-pleading-list/cases-received-for-pleading-list.component'
const routes: Routes = [
    {
        path: '',
        component: CasesReceivedForPleadingComponent,
        data: {
          title: 'سجل القضايا للترافع',
          urls: [{ title: 'الرئيسية', url: '/' }, { title: 'سجل القضايا للترافع' }],
          rolesDepartment:[
            {role:AppRole.DepartmentManager,departmentIds:[Department.Litigation]},
            {role:AppRole.Admin,departmentIds:null}
          ]
    
        },
      },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CasesReceivedForPleadingRoutingModule {}
