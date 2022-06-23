import { TaskReportComponent } from './task-report/task-report.component';
import { CaseReportComponent } from './case-report/case-report.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HearingReportComponent } from './hearing-report/hearing-report.component';

const routes: Routes = [
  {
    path: 'cases',
    component: CaseReportComponent,
    data: {
      title: 'تقرير القضايا',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'تقرير القضايا' }],
    },
  },
  {
    path: 'hearings',
    component: HearingReportComponent,
    data: {
      title: 'تقرير الجلسات',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'تقرير الجلسات' }],
    },
  },
  {
    path: 'tasks',
    component: TaskReportComponent,
    data: {
      title: 'تقرير المهام',
      urls: [{ title: 'الرئيسية', url: '/' }, { title: 'تقرير المهام' }],
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportsRoutingModule {}
