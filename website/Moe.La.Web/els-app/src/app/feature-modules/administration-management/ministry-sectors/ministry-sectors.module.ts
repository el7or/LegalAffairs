import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { MinistrySectorFormComponent } from './ministry-sectors-form/ministry-sector-form.component';
import { MinistrySectorListComponent } from './ministry-sectors-list/ministry-sectors-list.component';
import { MinistrySectorsRoutingModule } from './ministry-sectors-routing.module'; 

@NgModule({
  declarations: [MinistrySectorListComponent, MinistrySectorFormComponent],
  imports: [
    SharedModule,
    MinistrySectorsRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class MinistrySectorsModule { }
