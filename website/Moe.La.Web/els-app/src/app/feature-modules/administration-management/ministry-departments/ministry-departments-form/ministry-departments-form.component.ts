import { Component, OnInit, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { debounceTime, map } from 'rxjs/operators';
import { MinistrySectorService } from 'app/core/services/ministry-sectors.service';
import { QueryObject } from 'app/core/models/query-objects';

@Component({
  selector: 'app-ministry-departments-form',
  templateUrl: './ministry-departments-form.component.html',
  styleUrls: ['./ministry-departments-form.component.css']
})
export class MinistryDepartmentFormComponent implements OnInit, AfterViewInit, OnDestroy {

  ministryDepartmentId: number = 0;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: 9999,
  };
  ministrySectors: any = [];
  ministryDepartment: any;

  constructor(
    private fb: FormBuilder,
    public ministrySectorService: MinistrySectorService,
    public ministryDepartmentService: MinistryDepartmentService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<MinistryDepartmentFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.ministryDepartmentId)
      this.ministryDepartmentId = this.data.ministryDepartmentId;
  }

  ngOnInit() {
    this.init();
    ///
    this.populateMinistrySectors();
    ///
    if (this.ministryDepartmentId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.ministryDepartmentService.get(this.ministryDepartmentId).subscribe(
          (result: any) => {
            this.ministryDepartment = result.data;
            this.populateMinistryDepartmentForm(result.data);
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
      ministrySectorId: [null, Validators.compose([Validators.required])],

    });
  }

  populateMinistryDepartmentForm(ministryDepartment: any) {
    this.form.patchValue({
      id: ministryDepartment?.id,
      name: ministryDepartment?.name,
      ministrySectorId: ministryDepartment?.ministrySectorId
    });
  }

  populateMinistrySectors() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.ministrySectorService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.ministrySectors = result.data.items;
          this.loaderService.stopLoading()
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }
  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.ministryDepartmentId
      ? this.ministryDepartmentService.update(this.form.value)
      : this.ministryDepartmentService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.ministryDepartmentId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          this.alert.error("فشلت عملية الحفظ !");
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
    if (nameCtrl.valid && this.ministryDepartment) {
      if (this.ministryDepartment.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.ministryDepartmentService
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
