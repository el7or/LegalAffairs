import { AttacmentService } from '../../../../core/services/attacment.service';
import {
  Component,
  Input,
  OnInit,
  Inject
} from '@angular/core';
import { Subscription } from 'rxjs';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from '../../loader/loader.component';
import {
  GroupNames,
  Attachment,
} from 'app/core/models/attachment';
import { AttachmentTypeService } from 'app/core/services/attachment-type.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoaderService } from 'app/core/services/loader.service';
import { AttachmentQueryObject } from 'app/core/models/query-objects';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {

  groupName!: GroupNames;

  form: FormGroup = Object.create(null);
  attachmentsTypes: KeyValuePairs[] = [];

  private subs = new Subscription();
  files: any[] = [];
  id: number = null;


  constructor(
    private loaderService: LoaderService,
    private attachmentTypeService: AttachmentTypeService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<UploadComponent>,
    @Inject(MAT_DIALOG_DATA) public data) {

    this.files = data.files;
    this.groupName = data.groupName;
    this.id = data.id;
  }

  ngOnInit(): void {
    this.init();
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
  private init(): void {
    this.form = this.fb.group({
      attachmentsTypeId: [this.id, Validators.compose([Validators.required])],
    });
  }


  onSubmit() {
    this.onCancel({ id: this.form.value.attachmentsTypeId != 0 ? this.form.value.attachmentsTypeId : null, name: this.attachmentsTypes.find(a => a.id == this.form.value.attachmentsTypeId)?.name });
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }


  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
