import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { UsersRoutingModule } from './users-routing.module';
import { UserListComponent } from './user-list/user-list.component';
import { UserFormComponent } from './user-form/user-form.component';
import { UserViewComponent } from './user-view/user-view.component';
import { UserFormDialogComponent } from './user-form-dialog/user-form-dialog.component';
import { UserSignatureComponent } from './user-signature/user-signature.component';

@NgModule({
  declarations: [
    UserListComponent,
    UserFormComponent,
    UserViewComponent,
    UserFormDialogComponent,
    UserSignatureComponent,
  ],
  imports: [
    SharedModule,
    UsersRoutingModule,
    SweetAlert2Module.forChild()
  ],
  providers: [DatePipe]
})
export class UsersModule { }
