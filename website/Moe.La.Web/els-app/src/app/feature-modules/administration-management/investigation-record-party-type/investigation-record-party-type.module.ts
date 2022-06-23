import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { InvestigationRecordPartyTypeRoutingModule } from './investigation-record-party-type-routing.module';
import { InvestigationRecordPartyTypeListComponent } from './investigation-record-party-type-list/investigation-record-party-type-list.component';
import { InvestigationRecordPartyTypeFormComponent } from './investigation-record-party-type-form/investigation-record-party-type-form.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [InvestigationRecordPartyTypeListComponent, InvestigationRecordPartyTypeFormComponent],
  imports: [
    SharedModule,
    InvestigationRecordPartyTypeRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class InvestigationRecordPartyTypeModule { }
