import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from '../../../../app/shared/shared.module';
import { PartyTypesRoutingModule } from './party-types-routing.module';
import { PartyTypeFormComponent } from './party-type-form/party-type-form.component';
import { PartyTypeListComponent } from './party-type-list/party-type-list.component';

@NgModule({
  declarations: [PartyTypeFormComponent, PartyTypeListComponent],
  imports: [
    SharedModule,
    PartyTypesRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class PartyTypesModule { }
