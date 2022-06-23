import { Component, OnInit, Inject, OnDestroy, AfterViewInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { GroupNames } from 'app/core/models/attachment';
import { AttachmentTypeService } from 'app/core/services/attachment-type.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { debounceTime, map } from 'rxjs/operators';
import { AttachmentType } from 'app/core/models/attachment-type';

@Component({
  selector: 'app-attachment-type-form',
  templateUrl: './attachment-type-form.component.html',
  styleUrls: ['./attachment-type-form.component.css']
})

export class AttachmentTypeFormComponent implements OnInit, AfterViewInit, OnDestroy {
  attachmentTypeId: number = 0;
  attachmentType: AttachmentType = null;
  form: FormGroup = Object.create(null);
  groupNames: any[];

  subs = new Subscription();

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }

  constructor(
    private fb: FormBuilder,
    private attachmentTypeService: AttachmentTypeService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<AttachmentTypeFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.attachmentTypeId)
      this.attachmentTypeId = this.data.attachmentTypeId;
  }

  ngOnInit() {
    this.init();

    this.populateGroupNames();
    if (this.attachmentTypeId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.attachmentTypeService.get(this.attachmentTypeId).subscribe(
          (result: any) => {
            this.attachmentType = result.data;
            this.patchAttachmentTypeForm(result.data);
            this.loaderService.stopLoading();
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }
  }

  ngAfterViewInit() {
    // Watch for name changes.
    this.form
      .get('name')
      .valueChanges.pipe(
        debounceTime(1000),
        map((value: string) => value.trim())
      )
      .subscribe((value: string) => {
        this.checkName(value);
      });
  }

  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      name: [null, Validators.compose([Validators.required])],
      groupName: ["", Validators.compose([Validators.required])]
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateGroupNames() {
    if (!this.groupNames) {
      this.subs.add(
        this.attachmentTypeService.getGroupName().subscribe(
          (result: any) => {
            this.groupNames = result;
            this.loaderService.stopLoading();
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }
  }

  patchAttachmentTypeForm(attachmentType: AttachmentType) {
    this.form.patchValue({
      id: attachmentType?.id,
      name: attachmentType?.name,
      groupName: attachmentType?.groupName,
    });
  }

  /**
   * Check whether the name field value is unique or not.
   * @param name The name value.
   */
  private checkName(name: string): void {
    const nameCtrl = this.form.get('name');
    if (nameCtrl.valid && this.attachmentType) {
      if (this.attachmentType.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.attachmentTypeService
          .isNameExists(this.form.value)
          .subscribe((result: any) => {
            if (result.data) {
              nameCtrl.setErrors({ uniqueName: true });
              nameCtrl.markAsTouched();
            } else {
              nameCtrl.updateValueAndValidity({ emitEvent: false });
            }
          });
      }
    }
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.attachmentTypeId
      ? this.attachmentTypeService.update(this.form.value)
      : this.attachmentTypeService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.attachmentTypeId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          this.alert.error("فشلت عملية الحفظ !");
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
