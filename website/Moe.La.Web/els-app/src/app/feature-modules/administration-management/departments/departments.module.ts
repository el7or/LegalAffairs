import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { DepartmentsRoutingModule } from './departments-routing.module';
import { DepartmentFormComponent } from './department-form/department-form.component';
import { DepartmentListComponent } from './department-list/department-list.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [DepartmentFormComponent, DepartmentListComponent],
  imports: [
    SharedModule,    
    DepartmentsRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class DepartmentsModule { }
