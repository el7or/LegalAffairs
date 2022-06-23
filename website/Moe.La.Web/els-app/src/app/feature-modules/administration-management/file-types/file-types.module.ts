import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { SharedModule } from 'app/shared/shared.module';

import { FileTypesRoutingModule } from './file-types-routing.module';
import { FileTypeFormComponent } from './file-type-form/file-type-form.component';
import { FileTypeListComponent } from './file-type-list/file-type-list.component';

@NgModule({
  declarations: [FileTypeFormComponent, FileTypeListComponent],
  imports: [
    FileTypesRoutingModule,
    SharedModule,
    SweetAlert2Module.forChild(),
  ]
})
export class FileTypesModule { }
