import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { BranchesRoutingModule } from './branches-routing.module';
import { BranchListComponent } from './branch-list/branch-list.component';
import { BranchFormComponent } from './branch-form/branch-form.component';


@NgModule({
  declarations: [BranchListComponent, BranchFormComponent],
  imports: [
    SharedModule,
    BranchesRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class BranchesModule { }
