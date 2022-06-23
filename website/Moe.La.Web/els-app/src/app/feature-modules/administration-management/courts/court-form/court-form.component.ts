import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Court } from 'app/core/models/court';
import { EnumValue } from 'app/core/models/enum-value';
import { AlertService } from 'app/core/services/alert.service';
import { CourtService } from 'app/core/services/court.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AfterViewInit } from '@angular/core';
import { debounceTime, map } from 'rxjs/operators';


@Component({
  selector: 'app-court-form',
  templateUrl: './court-form.component.html',
  styleUrls: ['./court-form.component.css'],
})
export class CourFormComponent implements OnInit, AfterViewInit, OnDestroy {

  courtId: number = 0;
  court: Court = null;
  courtTypes: EnumValue[] = [];
  courtCategories: EnumValue[] = [];

  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public courtService: CourtService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<CourFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.courtId)
      this.courtId = this.data.courtId;
  }

  ngOnInit() {
    this.init();
    ///
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      this.courtService.getCourtTypes().subscribe(
        (result: any) => {
          this.courtTypes = result;
          this.loaderService.stopLoading();
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
          console.error(error);
        }
      )
    );
    this.subs.add(
      this.courtService.getCourtCategories().subscribe(
        (result: any) => {
          this.courtCategories = result;
          this.loaderService.stopLoading();
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
          console.error(error);
        }
      )
    );

    if (this.courtId) {
      this.subs.add(
        this.courtService.get(this.courtId).subscribe(
          (result: any) => {
            this.court = result.data;
            this.populateCourtForm(result.data);
          },
          (error) => {
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
      litigationType: [""],
      courtCategory: ["", Validators.compose([Validators.required])],

    });
  }

  populateCourtForm(court: Court) {
    this.form.patchValue({
      id: court?.id,
      name: court?.name,
      litigationType: this.courtTypes.find((t) => t.nameAr == court?.litigationType)?.value,
      courtCategory: this.courtCategories.find((t) => t.nameAr == court?.courtCategory)?.value || 0,
    });

  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.courtId
      ? this.courtService.update(this.form.value)
      : this.courtService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.courtId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          console.error(error);
          let message = this.courtId
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
    if (nameCtrl.valid && this.court) {
      if (this.court.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.courtService
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
