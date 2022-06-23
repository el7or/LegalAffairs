import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AlertService } from 'app/core/services/alert.service';
import { Province } from 'app/core/models/province';
import { City } from 'app/core/models/city';
import { CityService } from 'app/core/services/city.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AfterViewInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-city-form',
  templateUrl: './city-form.component.html',
  styleUrls: ['./city-form.component.css'],
})
export class CityFormComponent implements OnInit, AfterViewInit, OnDestroy {

  cityId: number = 0;
  city: City = null;
  provinces: Province[] = [];
  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public cityService: CityService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<CityFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.cityId)
      this.cityId = this.data.cityId;
  }

  ngOnInit() {
    this.init();
    ///
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      this.cityService.getProvinces().subscribe(
        (result: any) => {
          this.provinces = result.data.items;
          if (this.cityId) {
            this.getCityDetails();
          }
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

  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      name: [null, Validators.compose([Validators.required])],
      provinceId: ["", Validators.compose([Validators.required])],
    });
  }

  getCityDetails() {
    this.subs.add(
      this.cityService.get(this.cityId).subscribe(
        (result: any) => {
          this.city = result.data;
          this.populateCityForm(result.data);
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          console.error(error);
        }
      )
    );
  }

  populateCityForm(city: City) {
    this.form.patchValue({
      id: city?.id,
      name: city?.name,
      provinceId: this.provinces.find((t) => t.name == city?.province)?.id ||
        0,
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.cityId
      ? this.cityService.update(this.form.value)
      : this.cityService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.cityId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          let message = this.cityId
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

  /**
   * Check whether the name field value is unique or not.
   * @param name The name value.
   */
  private checkName(name: string): void {
    const nameCtrl = this.form.get('name');
    if (nameCtrl.valid && this.city) {
      if (this.city.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.cityService
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
