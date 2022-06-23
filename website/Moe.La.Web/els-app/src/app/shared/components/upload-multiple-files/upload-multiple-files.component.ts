import { Component, ElementRef, EventEmitter, Input, OnChanges, Output, ViewChild } from '@angular/core';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { AttachmentTypeService } from 'app/core/services/attachment-type.service';
import { Attachment, GroupNames } from '../../../core/models/attachment';
import { LoaderComponent } from '../loader/loader.component';
import Swal from 'sweetalert2';
import { MatTableDataSource } from '@angular/material/table';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { QueryObject } from 'app/core/models/query-objects';
import { AttacmentService } from 'app/core/services/attacment.service';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'upload-multiple-files',
  templateUrl: './upload-multiple-files.component.html',
  styleUrls: ['./upload-multiple-files.component.css']
})
export class UploadMultipleFilesComponent {

  @ViewChild('fileInput', { static: true }) fileInput: ElementRef = null!;
  @Input("attachment-type") attachmentType: boolean = true;
  @Output("attachments") allAttachments = new EventEmitter<any>(); // value will returns to $event variable

   /**
   * Case or Hearing or LegalMemo ...etc
   */
  @Input() groupName!: GroupNames;
  /**
   * CaseId or HearingId or LegalMemoId ...etc
   */
  @Input() groupId!: number;

  @Output("uploaded-attachments") returnUploadedAttachments = new EventEmitter(); // value will returns to $event variable


  uploadedAttachments:Guid[]=[];

  queryObject: QueryObject = new QueryObject();

  attachmentTypesList: KeyValuePairs[] = [];

  columnsToDisplay = [
    'position',
    'file',
    'attachmentType',
    'actions',
  ];

  attachmentsList: Attachment[] = [];
  attachments!: MatTableDataSource<Attachment>;

  constructor(private alert: AlertService,
    private attacmentService:AttacmentService,
    private attachmentTypeService: AttachmentTypeService,
    private loaderService: LoaderService,) {

  }

  uploadAttachment() {
    if (this.attachmentType) {
      this.loaderService.startLoading(LoaderComponent);
      this.queryObject.pageSize = 9999;
      this.attachmentTypeService.getWithQuery(this.queryObject).subscribe((result: any) => {
        this.loaderService.stopLoading();
        if (result.data.items.length) {
          for (var i = 0; i < result.data.items.length; i++) {
            this.attachmentTypesList[result.data.items[i].id] = result.data.items[i].name;
          }

          Swal.fire({
            icon: "info",
            title: "اختار نوع الملف:",
            input: "select",
            inputOptions: this.attachmentTypesList,
            confirmButtonText: "حفظ",
            showCancelButton: true,
            cancelButtonText: "إلغاء",
            cancelButtonColor: "#d33",
            showCloseButton: true,
          }).then((result) => {
            if (result.value) {
              this.completeUpload(result.value);
            } else {
              var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
              nativeElement.value = "";
            }
          });
        } else {
          this.completeUpload();
        }

      },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
        },
        () => { }
      );
    } else {
      this.completeUpload();
    }
  }

  completeUpload1(attachTypeId?: any) {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    const files: any = nativeElement.files;
    // multiple files ////
    for (var i = 0; i < files.length; i++) {
      var file = files[i];
      let saveAttachment: any = {};
      saveAttachment.file = file;
      saveAttachment.name = file.name;
      saveAttachment.attachmentTypeId = attachTypeId;
      this.attachmentsList.push(saveAttachment);
    }

    this.attachments = new MatTableDataSource(this.attachmentsList);
    // send it to component
    this.allAttachments.emit(this.attachmentsList);

    nativeElement.value = "";
  }

  completeUpload(attachmentTypeId?: any) {

    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    var files = nativeElement.files || [];
    for (var i = 0; i < files.length; i++) {
      var file = files[i];
 
      this.attacmentService.uploadAttachment(
        file
        )
        .subscribe(
          (result:any) => {
            
            this.attachmentsList.push(result.data);
            this.attachments = new MatTableDataSource(this.attachmentsList);
            this.returnUploadedAttachments.emit(this.attachmentsList.map(file=>file.id));            
          },
          (err:any) => {
            this.alert.error("فشلت عملية الحفظ");
          }
        );
    }
    nativeElement.value = "";
  }


  onDeleteAttachment(index: number) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل تريد حذف هذا المرفق من قائمة المرفقات؟',
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'قبول',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.attachmentsList.splice(index, 1);
        this.attachments = new MatTableDataSource(this.attachmentsList);
        this.allAttachments.emit(this.attachmentsList);
      }
    });
  }

}
