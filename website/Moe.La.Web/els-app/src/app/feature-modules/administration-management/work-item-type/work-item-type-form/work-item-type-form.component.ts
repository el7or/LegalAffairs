import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { forkJoin, Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { RoleService } from 'app/core/services/role.service';
import { WorkItemTypeService } from 'app/core/services/work-item-type.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { OnDestroy } from '@angular/core';
import { AfterViewInit } from '@angular/core';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-work-item-type-form',
  templateUrl: './work-item-type-form.component.html',
  styleUrls: ['./work-item-type-form.component.css']
})
export class WorkItemTypeFormComponent implements OnInit, AfterViewInit, OnDestroy {

  workItemTypeId: number = 0;
  workItemType: any = null;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  allRoles: any;
  constructor(
    private fb: FormBuilder,
    public workItemTypeService: WorkItemTypeService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<WorkItemTypeFormComponent>,
    private loaderService: LoaderService,
    private roleService: RoleService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.workItemTypeId)
      this.workItemTypeId = this.data.workItemTypeId;
  }

  ngOnInit() {
    this.init();
    ///
    if (this.workItemTypeId) {
      this.loaderService.startLoading(LoaderComponent);
      var sources = [
        this.workItemTypeService.get(this.workItemTypeId),
        this.roleService.getAll()
      ];

      this.subs.add(
        forkJoin(sources).subscribe(
          (res: any) => {
            this.loaderService.stopLoading();
            this.workItemType = res[0].data;
            this.populateWorkItemTypetForm(res[0].data);
            this.allRoles = res[1].data.items;
          },
          (err) => {
            this.loaderService.stopLoading();
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
      rolesIds: []
    });
  }

  populateWorkItemTypetForm(workItemType: any) {
    var roles = workItemType?.rolesIds.split(',');
    this.form.patchValue({
      id: workItemType?.id,
      name: workItemType?.name,
      rolesIds: roles
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    let workItemType = {
      id: this.form.get('id')?.value,
      name: this.form.get('name')?.value,
      rolesIds: this.form.get('rolesIds')?.value.toString()
    }

    let result$ =
      this.workItemTypeService.update(workItemType)

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = 'تمت عملية التعديل بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = 'فشلت عملية التعديل !';
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
    if (nameCtrl.valid && this.workItemType) {
      if (this.workItemType.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.workItemTypeService
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
