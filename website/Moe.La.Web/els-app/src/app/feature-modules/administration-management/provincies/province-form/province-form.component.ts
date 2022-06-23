import { Component, OnInit, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Province } from 'app/core/models/province';
import { EnumValue } from 'app/core/models/enum-value';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { ProvinceService } from 'app/core/services/province.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { debounceTime, map } from 'rxjs/operators';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';

@Component({
  selector: 'app-province-form',
  templateUrl: './province-form.component.html',
  styleUrls: ['./province-form.component.css'],
})

export class ProvinceFormComponent implements OnInit, AfterViewInit, OnDestroy {

  provinceId: number = 0;
  province: KeyValuePairs = new KeyValuePairs();

  provinceTypes: EnumValue[] = [];
  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public provinceService: ProvinceService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<ProvinceFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.provinceId)
      this.provinceId = this.data.provinceId;
  }

  ngOnInit() {
    this.init();
    ///

    if (this.provinceId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.provinceService.get(this.provinceId).subscribe(
          (result: any) => {
            this.province = result.data;
            this.populateprovinceForm(result.data);
            this.loaderService.stopLoading();
          },
          (error) => {
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
            console.error(error);
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

  populateprovinceForm(province: Province) {
    this.form.patchValue({
      id: province?.id,
      name: province?.name,
    });
  }

  /**
   * Check whether the name field value is unique or not.
   * @param name The name value.
   */
  private checkName(name: string): void {
    const nameCtrl = this.form.get('name');
    if (nameCtrl.valid && this.province) {
      if (this.province.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.provinceService
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

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.provinceId
      ? this.provinceService.update(this.form.value)
      : this.provinceService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.provinceId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          let message = this.provinceId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
