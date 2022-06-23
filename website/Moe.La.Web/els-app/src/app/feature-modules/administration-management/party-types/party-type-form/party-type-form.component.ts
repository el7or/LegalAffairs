import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { PartyTypeService } from 'app/core/services/party-type.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AfterViewInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-party-type-form',
  templateUrl: './party-type-form.component.html',
  styleUrls: ['./party-type-form.component.css']
})
export class PartyTypeFormComponent implements OnInit , AfterViewInit, OnDestroy{

  partyTypeId: number = 0;
  partyType: KeyValuePairs = new KeyValuePairs();
  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public partyTypeService: PartyTypeService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<PartyTypeFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.partyTypeId)
      this.partyTypeId = this.data.partyTypeId;
  }

  ngOnInit() {
    this.init();
    ///
    if (this.partyTypeId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.partyTypeService.get(this.partyTypeId).subscribe(
          (result: any) => {
            this.populatePartyTypetForm(result.data);
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

  populatePartyTypetForm(partyType: KeyValuePairs) {
    this.form.patchValue({
      id: partyType?.id,
      name: partyType?.name,
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.partyTypeId
      ? this.partyTypeService.update(this.form.value)
      : this.partyTypeService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.partyTypeId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = this.partyTypeId
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
      if (nameCtrl.valid && this.partyType) {
        if (this.partyType.name === name) {
          nameCtrl.markAsPristine();
        }
        else {
          nameCtrl.markAsPending();
          this.partyTypeService
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
