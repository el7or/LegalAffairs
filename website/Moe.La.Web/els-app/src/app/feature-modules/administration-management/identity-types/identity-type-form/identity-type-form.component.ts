import { Component, OnInit, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { IdentityTypeService } from 'app/core/services/identity-type.service';
import { Court } from 'app/core/models/court';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-identity-type-form',
  templateUrl: './identity-type-form.component.html',
  styleUrls: ['./identity-type-form.component.css'],
})
export class IdentityTypeFormComponent implements OnInit, AfterViewInit, OnDestroy {

  identityTypeId: number = 0;
  identityType: KeyValuePairs = new KeyValuePairs();
  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public identityTypeService: IdentityTypeService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<IdentityTypeFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.identityTypeId)
      this.identityTypeId = this.data.identityTypeId;
  }

  ngOnInit() {
    this.init();
    ///
    if (this.identityTypeId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.identityTypeService.get(this.identityTypeId).subscribe(
          (result: any) => {
            this.identityType = result.data;
            this.loaderService.stopLoading();
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

    let result$ = this.identityTypeId
      ? this.identityTypeService.update(this.form.value)
      : this.identityTypeService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.identityTypeId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = this.identityTypeId
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
    if (nameCtrl.valid && this.identityType) {
      if (this.identityType.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.identityTypeService
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
