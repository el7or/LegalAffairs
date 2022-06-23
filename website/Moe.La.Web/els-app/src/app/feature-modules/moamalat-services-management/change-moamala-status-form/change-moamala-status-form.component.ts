import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { QueryObject, SubWorkItemTypeQueryObject, UserQueryObject, WorkItemTypeQueryObject } from 'app/core/models/query-objects';
import { MoamalaStatuses, MoamalaSteps } from 'app/core/enums/MoamalaStatuses';
import { MoamalatService } from 'app/core/services/moamalat.service';
import { UserService } from 'app/core/services/user.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { UserList } from 'app/core/models/user';
import { MoamalaChangeStatus } from 'app/core/models/moamalat';
import { WorkItemTypeService } from 'app/core/services/work-item-type.service';
import { BranchService } from 'app/core/services/branch.service';
import { ConfidentialDegrees } from 'app/core/enums/ConfidentialDegrees';
import { SubWorkItemTypeService } from 'app/core/services/sub-work-item-type.service';

@Component({
  selector: 'app-change-moamala-status-form',
  templateUrl: './change-moamala-status-form.component.html',
  styleUrls: ['./change-moamala-status-form.component.css'],
})
export class ChangeMoamalaStatusFormComponent implements OnInit {
  moamalaChangeStatusData: MoamalaChangeStatus;

  form: FormGroup;

  //lists
  branches: KeyValuePairs[] = [];
  departments: KeyValuePairs[] = [];
  usersList: UserList[] = [];
  users: UserList[] = [];
  filteredUsers!: UserList[] | undefined;

  queryObject: QueryObject = new QueryObject({
    sortBy: 'parent',
    pageSize: 9999,
  });
  workItemTypes: any;
  subWorkItemTypes: any;
  subs = new Subscription();

  public get MoamalaStatuses(): typeof MoamalaStatuses {
    return MoamalaStatuses;
  }
  public get MoamalaSteps(): typeof MoamalaSteps {
    return MoamalaSteps;
  }
  public get ConfidentialDegrees(): typeof ConfidentialDegrees {
    return ConfidentialDegrees;
  }
  constructor(
    public workItemTypeService: WorkItemTypeService,
    public subWorkItemTypeService: SubWorkItemTypeService,
    private fb: FormBuilder,
    private alert: AlertService,
    public dialogRef: MatDialogRef<ChangeMoamalaStatusFormComponent>,
    private loaderService: LoaderService,
    private moamalatService: MoamalatService,
    private userService: UserService,
    public branchService: BranchService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.moamalaChangeStatusData = this.data.moamalaChangeStatusData;
  }

  ngOnInit() {

    if (this.moamalaChangeStatusData.status == MoamalaStatuses.Referred) {
      if (this.moamalaChangeStatusData.currentStep == MoamalaSteps.Branch) {
        this.initReferredToBranch();
        this.populateBranch();
        if (this.moamalaChangeStatusData.branchId)
          this.populatedepartments(this.moamalaChangeStatusData.branchId);
        if (this.moamalaChangeStatusData.departmentId)
          this.populateWorkItemTypes(this.moamalaChangeStatusData.departmentId);
        if (this.moamalaChangeStatusData.workItemTypeId)
          this.populateSubWorkItemTypes(this.moamalaChangeStatusData.workItemTypeId);
      }
      else if (this.moamalaChangeStatusData.currentStep == MoamalaSteps.Department) {
        this.initReferredToDepartment();
        this.populateBranch();
        this.populatedepartments(this.moamalaChangeStatusData.branchId);
        if (this.moamalaChangeStatusData.departmentId)
          this.populateWorkItemTypes(this.moamalaChangeStatusData.departmentId);
        if (this.moamalaChangeStatusData.workItemTypeId)
          this.populateSubWorkItemTypes(this.moamalaChangeStatusData.workItemTypeId);
      }
    }
    else if (this.moamalaChangeStatusData.status == MoamalaStatuses.Assigned) {
      this.initAssigned();
      this.populateBranch();
      if (this.moamalaChangeStatusData.confidentialDegree == ConfidentialDegrees.Normal || this.moamalaChangeStatusData.branchId) {
        this.populatedepartments(this.moamalaChangeStatusData.branchId);
        this.populateWorkItemTypes(this.moamalaChangeStatusData.departmentId);
        this.populateUsers(this.moamalaChangeStatusData.workItemTypeId);
        this.populateSubWorkItemTypes(this.moamalaChangeStatusData.workItemTypeId);
      }
      //this.populateWorkItemTypes();
    }
    else if (this.moamalaChangeStatusData.status == MoamalaStatuses.MoamalaReturned) {
      this.initReturned();
    }
  }

  /**
   * init forms.
   */
  private initReferredToBranch(): void {
    this.form = this.fb.group({
      note: [null],
      branchId: [this.moamalaChangeStatusData.branchId, Validators.compose([Validators.required])],
      departmentId: [this.moamalaChangeStatusData.departmentId],
      workItemTypeId: [this.moamalaChangeStatusData.workItemTypeId],
      subWorkItemTypeId: [this.moamalaChangeStatusData.subWorkItemTypeId]
    });
  }
  private initReferredToDepartment(): void {
    this.form = this.fb.group({
      note: [null],
      branchId: [{ value: this.moamalaChangeStatusData.branchId, disabled: this.moamalaChangeStatusData.status == MoamalaStatuses.Referred && this.moamalaChangeStatusData.currentStep == MoamalaSteps.Department }, Validators.compose([Validators.required])],
      departmentId: [this.moamalaChangeStatusData.departmentId, Validators.compose([Validators.required])],
      workItemTypeId: [this.moamalaChangeStatusData.workItemTypeId],
      subWorkItemTypeId: [this.moamalaChangeStatusData.subWorkItemTypeId]
    });
  }
  private initReturned(): void {
    this.form = this.fb.group({
      note: [null, Validators.compose([Validators.required])],
    });
  }
  private initAssigned(): void {
    this.form = this.fb.group({
      branchId: [{ value: this.moamalaChangeStatusData.branchId, disabled: this.moamalaChangeStatusData.status == MoamalaStatuses.Assigned && this.moamalaChangeStatusData.confidentialDegree != ConfidentialDegrees.Confidential }, Validators.compose([Validators.required])],
      departmentId: [{ value: this.moamalaChangeStatusData.departmentId, disabled: this.moamalaChangeStatusData.status == MoamalaStatuses.Assigned && this.moamalaChangeStatusData.confidentialDegree != ConfidentialDegrees.Confidential }, Validators.compose([Validators.required])],
      workItemTypeId: [this.moamalaChangeStatusData.workItemTypeId, Validators.compose([Validators.required])],
      subWorkItemTypeId: [this.moamalaChangeStatusData.subWorkItemTypeId],
      assignedTo: [null, Validators.compose([Validators.required])],
      note: [null],
    });
    this.form.controls['assignedTo'].valueChanges.subscribe(
      value => {
        if (!value?.id)
          this.form.get('assignedTo')?.setErrors({ selectedValue: true });
      }
    );
  }

  populateBranch() {
    this.subs.add(
      this.branchService
        .getWithQuery(new QueryObject({
          sortBy: 'id',
          pageSize: 9999,
        }))
        .subscribe(
          (result: any) => {
            this.branches = result.data.items;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
    );
  }

  populatedepartments(branchId?: number) {
    if (branchId)
      this.subs.add(
        this.branchService.getDepartments(branchId).subscribe(
          (result: any) => {
            this.departments = result.data;

            if (this.moamalaChangeStatusData.branchId != branchId) {
              this.workItemTypes = [];
              this.subWorkItemTypes = [];
              this.filteredUsers = [];
              this.form.get('departmentId').setValue("");
              this.form.get('workItemTypeId').setValue("");
              this.form.get('subWorkItemTypeId').setValue("");
              this.form.get('assignedTo')?.setValue("");
            }
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    else {
      this.departments = [];
      this.workItemTypes = [];
      this.subWorkItemTypes = [];
    }
  }

  populateUsers(workItemType) {
    let userQueryObject = new UserQueryObject({
      sortBy: 'name',
      pageSize: 9999,
      branchId: this.moamalaChangeStatusData.branchId ? this.moamalaChangeStatusData.branchId : this.form.get("branchId").value,
      departmetId: this.moamalaChangeStatusData.departmentId ? this.moamalaChangeStatusData.departmentId : this.form.get("departmentId").value,
      //hasConfidentialPermission: this.moamalaChangeStatusData.confidentialDegree == ConfidentialDegrees.Confidential,
      workItemTypeId: workItemType
    });
    if (workItemType)
      this.subs.add(
        this.userService.getWithQuery(userQueryObject).subscribe(
          (result: any) => {
            if (result.isSuccess) {
              this.users = result.data.items;
              this.filteredUsers = this.users = this.users.filter((u: UserList) => {
                return u.roles.some(r => r.name != "DepartmentManager")
              });

            }
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    else
      this.filteredUsers = [];
  }

  filterUsers() {
    let filterValue = this.form.get('assignedTo')?.value?.toLowerCase();
    if (filterValue) {
      this.filteredUsers = this.users.filter(user => (user.roles.map(r => r.nameAr).join(' ') + ' / ' + user.firstName + ' ' + user.lastName).toLowerCase().includes(filterValue));
    } else {
      this.filteredUsers = this.users;
    }
  }

  displayFn(user?: UserList): string | undefined {
    return user ? user.roles.map(r => r.nameAr).join(' / ') + ' / ' + user.firstName + ' ' + user.lastName : '';
  }

  optionUserName(user: UserList) {
    return user ? user.roles.map(r => r.nameAr).join(' / ') + ' / ' + user.firstName + ' ' + user.lastName : null;
  }

  populateWorkItemTypes(departmentId) {
    if (departmentId)
      this.subs.add(
        this.workItemTypeService.getWithQuery(new WorkItemTypeQueryObject({
          sortBy: 'id',
          pageSize: 9999,
          isSortAscending: false,
          departmentId: departmentId//this.moamalaChangeStatusData.departmentId
        })).subscribe(
          (result: any) => {
            this.workItemTypes = result.data.items;
            if (this.moamalaChangeStatusData.departmentId != departmentId) {
              this.subWorkItemTypes = [];
              this.filteredUsers = [];
              this.form.get('workItemTypeId').setValue(null);
              this.form.get('subWorkItemTypeId').setValue(null);
              this.form.get('assignedTo')?.setValue(null);
            }
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    else {
      this.workItemTypes = [];
      this.subWorkItemTypes = [];
      this.filteredUsers = [];
      this.form.get('workItemTypeId').setValue("");
      this.form.get('subWorkItemTypeId').setValue("");
      this.form.get('assignedTo')?.setValue("");
    }
  }

  populateSubWorkItemTypes(workItemType) {
    if (workItemType)
      this.subs.add(
        this.subWorkItemTypeService.getWithQuery(new SubWorkItemTypeQueryObject({
          sortBy: 'id',
          pageSize: 9999,
          workItemTypeId: workItemType
        })).subscribe(
          (result: any) => {
            this.subWorkItemTypes = result.data.items;
            this.form.get('assignedTo')?.setValue("");
            this.filteredUsers = [];
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    else {
      this.subWorkItemTypes = [];
      this.form.get('assignedTo')?.setValue("");
      this.filteredUsers = [];
    }
  }
  onSubmit() {
    if (this.moamalaChangeStatusData.status != MoamalaStatuses.MoamalaReturned) {
      this.form.controls['branchId'].enable();
      this.form.controls['departmentId'].enable();
    }
    var moamalaChangeStatus: MoamalaChangeStatus = {
      moamalaId: this.moamalaChangeStatusData.moamalaId,
      status: this.moamalaChangeStatusData.status,
      branchId: this.form.value.branchId,
      departmentId: this.form.value.departmentId,
      assignedToId: this.form.value.assignedTo?.id,
      workItemTypeId: this.form.value.workItemTypeId,
      subWorkItemTypeId: this.form.value.subWorkItemTypeId,
      note: this.form.value.note
    }

    this.loaderService.startLoading(LoaderComponent);
    if (this.moamalaChangeStatusData.status == MoamalaStatuses.MoamalaReturned) {
      this.subs.add(
        this.moamalatService
          .return(this.moamalaChangeStatusData.moamalaId, this.form.value.note)
          .subscribe(
            (res) => {
              this.loaderService.stopLoading();
              this.onCancel(res);
            },
            (error) => {
              console.error(error);
              this.loaderService.stopLoading();
            }
          )
      );
    }
    else {
      this.subs.add(
        this.moamalatService
          .changeStatus(moamalaChangeStatus)
          .subscribe(
            (res) => {
              this.loaderService.stopLoading();
              this.onCancel(res);
            },
            (error) => {
              console.error(error);
              this.loaderService.stopLoading();
            }
          )
      );
    }

  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
