import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

import { FieldMissionTypeService } from 'app/core/services/field-mission-type.service';
import { Court } from 'app/core/models/court';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AfterViewInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-field-mission-type-form',
  templateUrl: './field-mission-type-form.component.html',
  styleUrls: ['./field-mission-type-form.component.css'],
})
export class FieldMissionTypeFormComponent implements OnInit , AfterViewInit, OnDestroy{

  fieldMissionTypeId: number = 0;
  fieldMissionType: any = null;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  fieldMissionTypes: any[];
  constructor(
    private fb: FormBuilder,
    public fieldMissionTypeService: FieldMissionTypeService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<FieldMissionTypeFormComponent>,
    private loaderService: LoaderService,
    private router: Router,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.fieldMissionTypeId)
      this.fieldMissionTypeId = this.data.fieldMissionTypeId;

    if (this.data.fieldMissionTypes)
      this.fieldMissionTypes = this.data.fieldMissionTypes;
  }

  ngOnInit() {
    this.init();
    ///
    if (this.fieldMissionTypeId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.fieldMissionTypeService.get(this.fieldMissionTypeId).subscribe(
          (result: any) => {
            this.loaderService.stopLoading();
            this.fieldMissionType= result.data;
            this.populateCourtForm(result.data);
          },
          (error) => {
            this.loaderService.stopLoading();
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      name: [null, Validators.compose([Validators.required])],
    });
  }

  populateCourtForm(court: Court) {
    this.form.patchValue({
      id: court?.id,
      name: court?.name,
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    let field = this.fieldMissionTypes.filter(x => x.name == this.form.value.name)
    if (field != null) {
      Swal.fire({
        text: 'يوجد مهمة ميدانية بنفس الاسم بالفعل',
        confirmButtonText: 'حسناً',
      }).then((result: any) => {
        if (result.value) {
          this.loaderService.stopLoading();
          // this.onCancel(result);
        }
      });
    }
    else {
      let result$ = this.fieldMissionTypeId
        ? this.fieldMissionTypeService.update(this.form.value)
        : this.fieldMissionTypeService.create(this.form.value);

      this.subs.add(
        result$.subscribe(
          (res) => {
            this.loaderService.stopLoading();
            this.onCancel(res);
            let message = this.fieldMissionTypeId
              ? 'تمت عملية التعديل بنجاح'
              : 'تمت عملية الإضافة بنجاح';
            this.alert.succuss(message);
          },
          (error) => {
            this.loaderService.stopLoading();
            let message = this.fieldMissionTypeId
              ? 'فشلت عملية التعديل !'
              : 'فشلت عملية الإضافة !';
            this.alert.error(message);
            console.error(error);
          }
        )
      );

    }

  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
/**
   * Check whether the name field value is unique or not.
   * @param name The name value.
   */
  private checkName(name: string): void {
    const nameCtrl = this.form.get('name');
    if (nameCtrl.valid && this.fieldMissionType) {
      if (this.fieldMissionType.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.fieldMissionTypeService
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

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
