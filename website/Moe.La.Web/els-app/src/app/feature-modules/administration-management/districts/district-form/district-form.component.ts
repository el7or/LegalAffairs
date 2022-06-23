import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { CityService } from 'app/core/services/city.service';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { DistrictService } from 'app/core/services/district.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AfterViewInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-district-form',
  templateUrl: './district-form.component.html',
  styleUrls: ['./district-form.component.css']
})
export class DistrictFormComponent implements OnInit, AfterViewInit, OnDestroy {

  districtId: number = 0;
  district: any = null;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  cities: any;

  constructor(
    private fb: FormBuilder,
    public districtService: DistrictService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<DistrictFormComponent>,
    private loaderService: LoaderService,
    public cityService: CityService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.districtId)
      this.districtId = this.data.districtId;
  }

  ngOnInit() {
    this.init();
    this.populateCities();

    ///
    if (this.districtId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.districtService.get(this.districtId).subscribe(
          (result: any) => {
            this.district=result.data;
            this.populatedistricttForm(result.data);
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
      cityId: ["", Validators.compose([Validators.required])],

    });
  }

  populatedistricttForm(district: any) {
    this.form.patchValue({
      id: district?.id,
      name: district?.name,
      cityId: district?.cityId,

    });
  }

  populateCities() {

    let queryObject: QueryObject = {
      sortBy: 'name',
      isSortAscending: true,
      page: 1,
      pageSize: 999,
    };

    this.subs.add(
      this.cityService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.cities = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }
  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.districtId
      ? this.districtService.update(this.form.value)
      : this.districtService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.districtId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          let message = this.districtId
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
    if (nameCtrl.valid && this.district) {
      if (this.district.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.districtService
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
