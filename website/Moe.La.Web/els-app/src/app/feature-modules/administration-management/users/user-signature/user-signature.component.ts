import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { Subscription } from 'rxjs';
import {
  GroupNames,
  Attachment,
} from 'app/core/models/attachment';
import { UploadComponent } from 'app/shared/components/attachments/upload/upload.component'
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { FileUploader } from 'ng2-file-upload';
import { Constants } from 'app/core/constants';

@Component({
  selector: 'app-user-signature',
  templateUrl: './user-signature.component.html',
  styleUrls: ['./user-signature.component.css']
})
export class UserSignatureComponent implements OnInit, OnDestroy {
  @Output('set-attachment') setAttachment = new EventEmitter<any>();
  @Input('signature-image') signatureImage?: any;
  baseUrl = Constants.BASE_URL;
  attachment: Attachment;
  @ViewChild(UploadComponent, { static: false }) uploadComponenet;
  image: any;
  AcceptedFiletypes = ["svg"];
  MaxBytes = 1024000000;
  FileTypeOrSizeError: boolean = false;
  @ViewChild('fileInput', { static: true }) fileInput: ElementRef = null!;
  private subs = new Subscription();

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  public uploader: FileUploader;
  constructor(private sanitizer: DomSanitizer) {

  }
  ngOnInit(): void {

  }

  changeAttachment(event = null) {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    const files: any = event || nativeElement.files;
    this.FileTypeOrSizeError = false;
    // for multiple files
    for (var i = 0; i < files.length; i++) {
      if (this.AcceptedFiletypes.includes(files[i].name.toLowerCase().split('.').pop()) && files[i].size <= this.MaxBytes) {
        //image preview
        const fileReader = new FileReader();

        fileReader.onload = async e => {
          this.signatureImage = this.sanitizer.bypassSecurityTrustUrl(fileReader.result as string);
          this.setAttachment.emit(this.signatureImage.changingThisBreaksApplicationSecurity);
        }

        fileReader.readAsDataURL(files[i]);
      }



    }
  }


  formateBytes(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  onDeletePhoto() {
    this.fileInput.nativeElement.value = '';
    this.setAttachment.emit(null);
    this.signatureImage = null;
  }
}
