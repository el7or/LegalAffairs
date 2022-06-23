import { NgModule } from "@angular/core";
import { FlexLayoutModule } from "@angular/flex-layout";
import { CommonModule } from '@angular/common';
import { UploadMultipleFilesComponent } from './upload-multiple-files.component'
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';


@NgModule({
    imports: [
        CommonModule,
        MatIconModule,
        MatButtonModule,
        FlexLayoutModule,
        MatTableModule
    ],
    declarations: [UploadMultipleFilesComponent],
    exports: [UploadMultipleFilesComponent],
})
export class UploadMultipleFilesModule { }
