import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { PartiesRoutingModule } from './parties-routing.module';
import { PartyListComponent } from './party-list/party-list.component';

@NgModule({
  declarations: [PartyListComponent],
  imports: [
    SharedModule,
    PartiesRoutingModule
  ]
})
export class PartiesModule { }
