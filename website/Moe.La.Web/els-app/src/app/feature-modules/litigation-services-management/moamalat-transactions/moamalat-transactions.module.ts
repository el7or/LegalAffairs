import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';

import { SharedModule } from 'app/shared/shared.module';
import { MoamalatTransactionsRoutingModule } from './moamalat-transactions-routing.module';
import { MoamalatInboundListComponent } from './moamalat-inbound-list/moamalat-inbound-list.component';
import { MoamalatOutboundListComponent } from './moamalat-outbound-list/moamalat-outbound-list.component';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';

@NgModule({
  declarations: [
    MoamalatInboundListComponent,
    MoamalatOutboundListComponent
  ],
  imports: [
    SharedModule,
    MoamalatTransactionsRoutingModule,
    DatePickerDualModule
  ],
  providers: [DatePipe],
  exports: []

})
export class MoamalatTransactionsModule { }
