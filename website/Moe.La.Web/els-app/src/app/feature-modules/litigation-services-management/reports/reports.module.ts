import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { ReportsRoutingModule } from './reports-routing.module';
import { CaseReportComponent } from './case-report/case-report.component';
import { HearingReportComponent } from './hearing-report/hearing-report.component';
import { TaskReportComponent } from './task-report/task-report.component';


@NgModule({
  declarations: [CaseReportComponent, HearingReportComponent, TaskReportComponent],
  imports: [
    SharedModule,
    ReportsRoutingModule
  ]
})
export class ReportsModule { }
