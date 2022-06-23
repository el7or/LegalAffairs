import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { JobTitleListComponent } from './job-title-list/job-title-list.component';

const routes: Routes = [{
  path: '',
  component: JobTitleListComponent,
  data: {
    title: 'المسميات الوظيفية',
    urls: [{ title: 'الرئيسية', url: '/' }, { title: 'المسميات الوظيفية' }],
  },
},];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class JobTitlesRoutingModule { }
