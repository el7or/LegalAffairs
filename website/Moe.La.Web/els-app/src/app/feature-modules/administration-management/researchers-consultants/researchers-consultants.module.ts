import { DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { ResearcherFormComponent } from './researcher-form/researcher-form.component';
import { ResearcherListComponent } from './researcher-list/researcher-list.component';
import { ResearchersConsultantsRoutingModule } from './researchers-consultants-routing.module';

@NgModule({
  declarations: [ResearcherListComponent, ResearcherFormComponent],
  imports: [
    SharedModule,
    ResearchersConsultantsRoutingModule
  ],
  providers: [DatePipe]
})
export class ResearchersConsultantsModule { }
