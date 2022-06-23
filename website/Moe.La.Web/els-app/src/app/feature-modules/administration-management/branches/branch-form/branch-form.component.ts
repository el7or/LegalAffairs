import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';

import { BranchQueryObject } from 'app/core/models/query-objects';
import { BranchService } from 'app/core/services/branch.service';
import { DepartmentService } from 'app/core/services/department.service';
import { Branch } from 'app/core/models/branch';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AfterViewInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-branch-form',
  templateUrl: './branch-form.component.html',
  styleUrls: ['./branch-form.component.css'],
})
export class BranchFormComponent implements OnInit, AfterViewInit, OnDestroy {
  departmentId: number = 0;
  department: any = null;
  parents: Branch[] = [];
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  queryObject: BranchQueryObject = {
    sortBy: 'id',
    isSortAscending: true,
    page: 1,
    pageSize: 100,
    isParent: true,
  };

  public departmentType = DepartmentType;
  departments: [];

  constructor(
    private fb: FormBuilder,
    public branchService: BranchService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<BranchFormComponent>,
    private loaderService: LoaderService,
    private departmentsService: DepartmentService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.departmentId) this.departmentId = this.data.departmentId;
  }

  ngOnInit() {
    this.init();

    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.branchService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.parents = result.data.items;
          this.loaderService.stopLoading();
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
          console.error(error);
        }
      )
    );

    this.subs.add(this.departmentsService.getAll().subscribe(
      (result: any) => {
        this.departments = result.data.items;
      }, (error) => {
        this.alert.error('فشلت عملية جلب البيانات !');
        this.loaderService.stopLoading();
        console.error(error);
      }));

    if (this.departmentId) {
      this.subs.add(
        this.branchService.get(this.departmentId).subscribe(
          (result: any) => {
            this.department = result.data;
            this.populateBranch(result.data);
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
      departmentTypeId: ["", Validators.compose([Validators.required])],
      parentId: [""],
      departments: [null, Validators.required]
    });
  }

  populateBranch(department: Branch) {
    this.form.patchValue({
      id: department?.id,
      name: department?.name,
      departmentTypeId: department?.parentId
        ? this.departmentType.Administration
        : this.departmentType.Region,
      parentId: department?.parentId,
      departments: department.departments
    });
  }

  onSubmit() {
    if (this.form.get('departmentTypeId')?.value == this.departmentType.Region)
      this.form.get('parentId')?.setValue(null);
    ///
    this.loaderService.startLoading(LoaderComponent);
    let result$ = this.departmentId
      ? this.branchService.update(this.form.value)
      : this.branchService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.departmentId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = this.departmentId
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

  onNameBlur() {
    const nameCtrl = this.form.get('name');

    if (this.form.value.name) {
      this.queryObject.isParent = false;
      this.queryObject.name = this.form.value.name;
      this.subs.add(
        this.branchService
          .getWithQuery(this.queryObject)
          .subscribe((res: any) => {
            if (res.data.items[0])
              if (res.data.items[0].id != this.form.value.id)
                nameCtrl?.setErrors({ nameExists: true });
          })
      );
    }
  }

  onSelectDepartmentType() {
    const ctrlParentId = this.form.controls['parentId'];
    if (this.form.value.departmentTypeId == this.departmentType.Region) {
      ctrlParentId.clearValidators();
      ctrlParentId.updateValueAndValidity();
    } else {
      ctrlParentId.setValidators(Validators.compose([Validators.required]));
      ctrlParentId.updateValueAndValidity();
    }
  }
  /**
     * Check whether the name field value is unique or not.
     * @param name The name value.
     */
  private checkName(name: string): void {
    const nameCtrl = this.form.get('name');
    if (nameCtrl.valid && this.departmentId != 0) {
      if (this.department.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.branchService
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

export enum DepartmentType {
  Region = '1',
  Administration = '2',
}


