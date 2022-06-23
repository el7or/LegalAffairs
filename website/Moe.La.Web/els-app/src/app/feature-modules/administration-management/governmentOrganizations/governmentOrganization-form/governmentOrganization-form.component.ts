import { Component, OnInit, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { AlertService } from 'app/core/services/alert.service';
import { GovernmentOrganizationService } from 'app/core/services/governmentOrganization.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-governmentOrganization-form',
  templateUrl: './governmentOrganization-form.component.html',
  styleUrls: ['./governmentOrganization-form.component.css']
})

export class GovernmentOrganizationFormComponent implements OnInit, AfterViewInit, OnDestroy {

  governmentOrganizationId: number = 0;
  governmentOrganization: KeyValuePairs = new KeyValuePairs();
  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public governmentOrganizationService: GovernmentOrganizationService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<GovernmentOrganizationFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.governmentOrganizationId)
      this.governmentOrganizationId = this.data.governmentOrganizationId;
  }

  ngOnInit() {
    this.init();
    ///
    if (this.governmentOrganizationId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.governmentOrganizationService.get(this.governmentOrganizationId).subscribe(
          (result: any) => {
            this.governmentOrganization = result.data;
            this.populateGovernmentOrganizationtForm(result.data);
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

  populateGovernmentOrganizationtForm(governmentOrganization: KeyValuePairs) {
    this.form.patchValue({
      id: governmentOrganization?.id,
      name: governmentOrganization?.name,
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.governmentOrganizationId
      ? this.governmentOrganizationService.update(this.form.value)
      : this.governmentOrganizationService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.governmentOrganizationId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          let message = this.governmentOrganizationId
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
    if (nameCtrl.valid && this.governmentOrganization) {
      if (this.governmentOrganization.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.governmentOrganizationService
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
