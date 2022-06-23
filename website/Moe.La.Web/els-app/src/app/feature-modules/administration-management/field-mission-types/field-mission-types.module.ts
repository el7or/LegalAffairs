import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { SharedModule } from 'app/shared/shared.module';

import { FieldMissionTypeFormComponent } from './field-mission-type-form/field-mission-type-form.component';
import { FieldMissionTypeListComponent } from './field-mission-type-list/field-mission-type-list.component';
import { FieldMissionTypesRoutingModule } from './field-mission-types-routing.module';

@NgModule({
  declarations: [
    FieldMissionTypeListComponent,
    FieldMissionTypeFormComponent
  ],
  imports: [
    SharedModule,
    FieldMissionTypesRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class FieldMissionTypesModule { }
