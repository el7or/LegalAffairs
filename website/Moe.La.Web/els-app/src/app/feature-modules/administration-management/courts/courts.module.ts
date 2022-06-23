import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { CourFormComponent } from './court-form/court-form.component';
import { CourtListComponent } from './court-list/court-list.component';
import { CourtsListRoutingModule } from './courts-routing.module';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

@NgModule({
  declarations: [
    CourtListComponent,
    CourFormComponent
  ],
  imports: [
    SharedModule,
    CourtsListRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class CourtsModule { }
