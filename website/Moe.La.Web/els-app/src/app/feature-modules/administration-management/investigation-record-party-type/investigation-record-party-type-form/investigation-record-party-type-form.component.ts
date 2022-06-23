import { Component, OnInit, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { InvestigationRecordPartyTypeService } from 'app/core/services/investigation-record-party-type.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-investigation-record-party-type-form',
  templateUrl: './investigation-record-party-type-form.component.html',
  styleUrls: ['./investigation-record-party-type-form.component.css']
})
export class InvestigationRecordPartyTypeFormComponent implements OnInit, AfterViewInit, OnDestroy {

  investigationRecordTypeId: number = 0;
  investigationRecordType: KeyValuePairs = new KeyValuePairs();
  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public investigationRecordTypeService: InvestigationRecordPartyTypeService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<InvestigationRecordPartyTypeFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.investigationRecordTypeId)
      this.investigationRecordTypeId = this.data.investigationRecordTypeId;
  }

  ngOnInit() {
    this.init();
    ///
    if (this.investigationRecordTypeId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.investigationRecordTypeService.get(this.investigationRecordTypeId).subscribe(
          (result: any) => {
            this.investigationRecordType = result.data;
            this.loaderService.stopLoading();
            this.populateInvestigationRecordTypeForm(result.data);
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

  populateInvestigationRecordTypeForm(investigationRecordType: any) {
    this.form.patchValue({
      id: investigationRecordType?.id,
      name: investigationRecordType?.name,
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.investigationRecordTypeId
      ? this.investigationRecordTypeService.update(this.form.value)
      : this.investigationRecordTypeService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.investigationRecordTypeId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = this.investigationRecordTypeId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
          console.error(error);
        }
      )
    );
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
    if (nameCtrl.valid && this.investigationRecordType) {
      if (this.investigationRecordType.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.investigationRecordTypeService
          .isNameExists(name)
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
