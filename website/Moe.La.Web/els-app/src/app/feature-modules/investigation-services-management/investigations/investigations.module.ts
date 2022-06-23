import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InvestigationsRoutingModule } from './investigations-routing.module';
import { InvestigationListComponent } from './investigation-list/investigation-list.component';
import { SharedModule } from 'app/shared/shared.module';
import { InvestigationFormComponent } from './investigation-form/investigation-form.component';


@NgModule({
  declarations: [InvestigationListComponent, InvestigationFormComponent],
  imports: [
    CommonModule,
    InvestigationsRoutingModule,
    SharedModule
  ]
})
export class InvestigationsModule { }
