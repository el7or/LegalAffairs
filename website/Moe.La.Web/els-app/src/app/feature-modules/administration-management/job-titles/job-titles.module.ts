import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { JobTitlesRoutingModule } from './job-titles-routing.module';
import { JobTitleListComponent } from './job-title-list/job-title-list.component';
import { JobTitleFormComponent } from './job-title-form/job-title-form.component';

@NgModule({
  declarations: [JobTitleListComponent, JobTitleFormComponent],
  imports: [
    SharedModule,
    JobTitlesRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class JobTitlesModule { }
