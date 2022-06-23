import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from '../../../../app/shared/shared.module';
import { IdentityTypeListComponent } from './identity-type-list/identity-type-list.component';
import { IdentityTypeFormComponent } from './identity-type-form/identity-type-form.component';
import { IdentityTypesRoutingModule } from './identity-types-routing.module';
 
@NgModule({
  declarations: [
    IdentityTypeListComponent,
    IdentityTypeFormComponent
  ],
  imports: [
    SharedModule,
    IdentityTypesRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class IdentityTypesModule { }
