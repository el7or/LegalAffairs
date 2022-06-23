import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { AlertService } from 'app/core/services/alert.service';
import Swal from 'sweetalert2';
import {
  GroupNames,
  Attachment,
} from 'app/core/models/attachment';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { LoaderComponent } from '../loader/loader.component';
import { AttacmentService } from 'app/core/services/attacment.service';
import { LoaderService } from 'app/core/services/loader.service';
import { UploadComponent } from 'app/shared/components/attachments/upload/upload.component'
import { AttachmentQueryObject } from 'app/core/models/query-objects';
import { AttachmentTypeService } from 'app/core/services/attachment-type.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';

@Component({
  selector: 'app-attachments',
  templateUrl: './attachments.component.html',
  styleUrls: ['./attachments.component.css'],
})

export class AttachmentsComponent implements OnInit, OnChanges, OnDestroy {
  /**
   * Case or Hearing or LegalMemo ...etc
   */
  @Input() groupName: GroupNames;
  @Input() isTypeRequired = true;

  @Input() readOnly: boolean = false;

  @Input('attachmentsToUpdate') attachmentsToUpdate: Attachment[] = [];

  @Output('set-attachments-list') setAttachmentsList = new EventEmitter<any>(); // value will returns to $event variable

  displayedColumns: string[] = [
    // 'position',
    'name',
    'attachmentType',
    'size',
    'createdOn',
    'actions',
  ];
  form: FormGroup = Object.create(null);

  attachmentsDataSource!: MatTableDataSource<Attachment>;
  attachmentsList: Attachment[] = [];
  filteredAttachmentsList: Attachment[] = [];

  @ViewChild(UploadComponent, { static: false }) uploadComponenet;

  AcceptedFiletypes = ["jpg", "jpeg", "png", "pdf", "docx", "doc"];
  MaxBytes = 1024000000;
  FileTypeOrSizeError: boolean = false;
  @ViewChild('fileInput', { static: true }) fileInput: ElementRef = null!;

  nameInputValue: string = "";
  selectedRecordId: number;
  attachmentsTypes: KeyValuePairs[] = [];

  private subs = new Subscription();
  typeInputValue: any = 'اختر نوع المرفق';
  typeInputValueId: any;
  nameInputValueExtension: string;
  searchText = "";
  notCategorized = "غير مصنف"
  
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }

  constructor(
    private attachmentService: AttacmentService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private dialog: MatDialog,
    private attachmentTypeService: AttachmentTypeService,
    private fb: FormBuilder

  ) { }


  ngOnInit(): void {
    this.form = this.fb.group({
      name: [],
    });

    this.changeSize(this.attachmentsList);

    this.populateAttachmentsType();
  }


  ngOnChanges(changes: SimpleChanges) {
    if (changes['attachmentsToUpdate']) {
      this.attachmentsList = changes['attachmentsToUpdate'].currentValue;
      this.changeSize(this.attachmentsList);
      this.onSearch();
    }
  }

  populateAttachmentsType() {
    let queryObject: AttachmentQueryObject = {
      sortBy: 'id',
      isSortAscending: true,
      page: 1,
      pageSize: 9999,
      groupName: this.groupName,
    };
    this.attachmentTypeService.getWithQuery(queryObject).subscribe(
      (result: any) => {
        this.attachmentsTypes = result.data.items;
      },
      (error) => {
        console.error(error);
        this.loaderService.stopLoading();
      },
      () => { }
    );
  }

  changeAttachment(event = null) {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    const files: any = event || nativeElement.files;

    this.FileTypeOrSizeError = false;

    const acceptedFiles = Object.values(files).filter((f: any) => this.AcceptedFiletypes.includes(f.name.toLowerCase().split('.').pop()) && f.size <= this.MaxBytes);


    if (acceptedFiles.length == files.length) {
      if (this.isTypeRequired) {
        this.uploadWithGroupName(files);
      }
      else {
        this.uploadFile(files);
      }
    }
    else
      this.FileTypeOrSizeError = true;
  }

  uploadWithGroupName(files) {
    const dialogRef = this.dialog.open(UploadComponent, {
      width: '30em',
      data: { groupName: this.groupName }
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(type => {
        if (type) this.uploadFile(files, type.id);
      })
    );

  }
  uploadFile(files, type = null) {
    for (var i = 0; i < files.length; i++) {
      var file = files[i];
      this.loaderService.startLoading(LoaderComponent);
      this.attachmentService.uploadAttachment(file, type)
        .subscribe(
          (result: any) => {
            this.loaderService.stopLoading();
            let attachmentToAdd = result.data;
            attachmentToAdd.formateSize = this.formateBytes(attachmentToAdd.size);

            this.attachmentsList.push(attachmentToAdd);

            // reset file input
            this.fileInput.nativeElement.value = "";
            //////

            this.onSearch();

            this.setAttachmentsList.emit(this.attachmentsList);
          },
          (err: any) => {
            this.loaderService.stopLoading();
            this.alert.error("فشلت عملية الحفظ");
          }
        );

    }

  }
  onDownload(id: string, name: string) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.attachmentService.downloadFile(id, name).subscribe(
        (data: any) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.download = name;
          link.click();
          this.loaderService.stopLoading();
          link.remove();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onDelete(item: Attachment) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام عملية الحذف؟',
      icon: 'error',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'حذف',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value && item.id) {

        if (item.isDraft)
          this.attachmentsList.splice(this.attachmentsList.indexOf(item), 1);
        else
          item.isDeleted = true;


        this.onSearch();
        this.setAttachmentsList.emit(this.attachmentsList);
      }
    });
  }

  changeSize(attachments) {
    for (var i = 0; i < attachments.length; i++) {
      var size = attachments[i].size;
      attachments[i].formateSize = this.formateBytes(size);
    }
  }
  formateBytes(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  onSelectRecord(id, name: string, type, typeId) {

    this.selectedRecordId = id;
    this.typeInputValue = type;
    this.typeInputValueId = typeId ? typeId : "";

    this.nameInputValueExtension = name.slice(name.lastIndexOf('.'), name.length);
    this.nameInputValue = name.slice(0, name.lastIndexOf('.'));

  }

  onUpdateRecord(item: Attachment) {
    item.name = this.nameInputValue + this.nameInputValueExtension;
    item.attachmentTypeId = this.typeInputValueId;
    item.attachmentType = this.attachmentsTypes.find(a => a.id == this.typeInputValueId)?.name;


    this.onSelectRecord(null, '', '', null);
    this.onSearch();
    this.setAttachmentsList.emit(this.attachmentsList);
  }

  onSearch() {
    this.filteredAttachmentsList = this.attachmentsList.filter(item =>
      (item.name.includes(this.searchText) 
      || item.attachmentType?.includes(this.searchText)
      || (!item.attachmentType && this.notCategorized.includes(this.searchText)))
      && !item.isDeleted);
    this.attachmentsDataSource = new MatTableDataSource(this.filteredAttachmentsList);

  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
