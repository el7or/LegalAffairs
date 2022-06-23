import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { MinistryDepartmentsRoutingModule } from './ministry-departments-routing.module';
import { MinistryDepartmentListComponent } from './ministry-departments-list/ministry-department-list.component';
import { MinistryDepartmentFormComponent } from './ministry-departments-form/ministry-departments-form.component';

@NgModule({
  declarations: [MinistryDepartmentListComponent, MinistryDepartmentFormComponent],
  imports: [
    SharedModule,
    MinistryDepartmentsRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class MinistryDepartmentsModule { }
