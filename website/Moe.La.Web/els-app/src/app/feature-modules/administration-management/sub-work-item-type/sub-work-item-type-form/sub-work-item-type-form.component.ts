import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { forkJoin, Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { SubWorkItemTypeService } from 'app/core/services/sub-work-item-type.service';
import { WorkItemTypeService } from 'app/core/services/work-item-type.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AfterViewInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-sub-work-item-type-form',
  templateUrl: './sub-work-item-type-form.component.html',
  styleUrls: ['./sub-work-item-type-form.component.css']
})
export class SubWorkItemTypeFormComponent implements OnInit, AfterViewInit, OnDestroy {

  subWorkItemTypeId: number = 0;
  subWorkItemType: any = null;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  workItemTypes: any;
  constructor(
    private fb: FormBuilder,
    public workItemTypeService: WorkItemTypeService,
    public subWorkItemTypeService: SubWorkItemTypeService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<SubWorkItemTypeFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.subWorkItemTypeId)
      this.subWorkItemTypeId = this.data.subWorkItemTypeId;
  }

  ngOnInit() {
    this.init();
    ///
    var sources = [this.workItemTypeService.getWithQuery({
      sortBy: 'name',
      isSortAscending: true,
      page: 1,
      pageSize: 999,
    })
    ];
    if (this.subWorkItemTypeId) {
      this.loaderService.startLoading(LoaderComponent);
      sources.push(
        this.subWorkItemTypeService.get(this.subWorkItemTypeId));
    }
    this.subs.add(
      forkJoin(sources).subscribe(
        (res: any) => {
          this.loaderService.stopLoading();
          this.subWorkItemType=res[1].data;
          this.workItemTypes = res[0].data.items;
          this.populateSubWorkItemTypetForm(res[1].data);
        },
        (err) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب البيانات !');
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
      workItemTypeId: [""]
    });
  }

  populateSubWorkItemTypetForm(subWorkItemType: any) {
    this.form.patchValue({
      id: subWorkItemType?.id,
      name: subWorkItemType?.name,
      workItemTypeId: subWorkItemType?.workItemTypeId
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    let result$ = this.subWorkItemTypeId
      ? this.subWorkItemTypeService.update(this.form.value)
      : this.subWorkItemTypeService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.subWorkItemTypeId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = this.subWorkItemTypeId
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
      if (nameCtrl.valid && this.subWorkItemType) {
        if (this.subWorkItemType.name === name) {
          nameCtrl.markAsPristine();
        }
        else {
          nameCtrl.markAsPending();
          this.subWorkItemTypeService
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
