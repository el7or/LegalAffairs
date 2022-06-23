import { NgModule } from '@angular/core';
import { DatePipe } from '@angular/common';
import { CasesReceivedForPleadingRoutingModule } from './cases-received-for-pleading-routing.module';
import { CasesReceivedForPleadingComponent } from './cases-received-for-pleading-list/cases-received-for-pleading-list.component';
import { DatePickerDualModule } from './../../../shared/components/date-picker-dual/date-picker-dual.module';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [CasesReceivedForPleadingComponent],
  imports: [
    CasesReceivedForPleadingRoutingModule,
    SharedModule,
    DatePickerDualModule
  ],
  providers: [DatePipe]
})
export class CasesReceivedForPleadingModule { }
