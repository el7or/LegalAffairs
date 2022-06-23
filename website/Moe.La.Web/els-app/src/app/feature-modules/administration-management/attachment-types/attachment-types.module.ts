import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { AttachmentTypesRoutingModule } from './attachment-types-routing.module';
import { AttachmentTypeFormComponent } from './attachment-type-form/attachment-type-form.component';
import { AttachmentTypeListComponent } from './attachment-type-list/attachment-type-list.component';

@NgModule({
  declarations: [AttachmentTypeFormComponent, AttachmentTypeListComponent],
  imports: [
    SharedModule,
    AttachmentTypesRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class AttachmentTypesModule { }
