import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';

import { AlertService } from 'app/core/services/alert.service';
import { HearingService } from 'app/core/services/hearing.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { HearingUpdateDto } from 'app/core/models/hearing';
import { Attachment, GroupNames } from 'app/core/models/attachment';

@Component({
  selector: 'app-hearing-update-form',
  templateUrl: './hearing-update-form.component.html',
  styleUrls: ['./hearing-update-form.component.css']
})
export class HearingUpdateFormComponent implements OnInit {

  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  hearingUpdate: HearingUpdateDto = new HearingUpdateDto();
  hearingId: number = 0;
  HearingUpdateId: number = 0;
  attachments: Attachment[] = [];
  public Editor = CustomEditor;
  public config = {
    language: 'ar'
  };

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<HearingUpdateFormComponent>,
    private alert: AlertService,
    private hearingService: HearingService,
    private loaderService: LoaderService,
    private router: Router,
    public location: Location,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.HearingUpdateId) {
      this.HearingUpdateId = this.data.HearingUpdateId;
    }
    if (this.data.hearingId) {
      this.hearingId = this.data.hearingId;
    }
    if (this.data.hearingUpdateObject != null) {
      this.hearingUpdate = this.data.hearingUpdateObject;
    }
  }

  ngOnInit(): void {
    this.init();
  }


  private init(): void {
    this.populateHearingUpdate();
    this.form = this.fb.group({
      id: [0],
      hearingId: [this.hearingId],
      text: ['', Validators.compose([Validators.required])],
      attachments: [null]
    });
  }

  populateHearingUpdate() {
    if (this.HearingUpdateId != 0) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.hearingService.getHearingUpdate(this.HearingUpdateId).subscribe((result: any) => {
          this.hearingUpdate = result.data;
          this.attachments = this.hearingUpdate.attachments;

          this.patchForm(this.hearingUpdate);
          this.loaderService.stopLoading();
        }
          , error => {
            this.loaderService.stopLoading();
            this.alert.error("فشلت عملية جلب تحديث للجلسة");
            this.onCancel();
          }));

    }
  }

  private patchForm(hearingUpdate: any): void {
    this.form.patchValue({
      id: hearingUpdate.id,
      hearingId: hearingUpdate.hearingId,
      text: hearingUpdate.text,
      attachments: hearingUpdate.attachments
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    Object.assign(this.hearingUpdate, this.form.value);
    this.hearingUpdate.attachments = this.attachments;

    if (this.HearingUpdateId == 0) {
      this.subs.add(
        this.hearingService.addHearingUpdate(this.hearingUpdate).subscribe(result => {
          this.alert.succuss("تم إضافة تحديث للجلسة بنجاح");
          this.loaderService.stopLoading();
          this.onCancel(result);
        }, error => {
          this.loaderService.stopLoading();
          this.alert.error("فشلت عملية إضافة تحديث للجلسة");
          this.onCancel();
        }));
    }
    else {
      this.subs.add(
        this.hearingService.editHearingUpdate(this.hearingUpdate).subscribe(result => {
          this.alert.succuss("تم تعديل تحديث للجلسة بنجاح");
          this.loaderService.stopLoading();
          this.onCancel(result);
        }, error => {
          this.loaderService.stopLoading();
          this.alert.error("فشلت عملية تعديل تحديث للجلسة");
          this.onCancel();
        }));
    }
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

  onHearingAttachment(list: any) {
    this.attachments = list;
    //this.form.controls['attachments'].setValue(this.attachments);
  }
}
