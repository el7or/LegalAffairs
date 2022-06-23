import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { forkJoin, Subscription } from 'rxjs';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { UserService } from 'app/core/services/user.service';
import { MimService } from 'app/core/services/active-directory.service';
import { AlertService } from 'app/core/services/alert.service';
import { BranchService } from 'app/core/services/branch.service';
import { RoleService } from 'app/core/services/role.service';
import { JobTitleService } from 'app/core/services/job-title.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { myUserDepartmentRole, SaveUser, User } from 'app/core/models/user';
import { AppRole } from 'app/core/models/role';
import { RoleClaimService } from 'app/core/services/role-claim.service';

@Component({
  selector: 'app-user-form-dialog',
  templateUrl: './user-form-dialog.component.html',
  styleUrls: ['./user-form-dialog.component.css'],
})
export class UserFormDialogComponent implements OnInit, OnDestroy {
  // The user object to be saved.
  user: User;
  form: FormGroup;
  searchText: string = '';

  // Lookups.
  branches: KeyValuePairs[] = [];
  branchDepartments: KeyValuePairs[] = [];
  jobTitles: KeyValuePairs[] = [];
  allClaims: any = [];
  roles: any[] = [];
  allRoles: any[] = [];
  disRoles: any[] = [];
  requiredDepartmentSelection: boolean = false;

  subs = new Subscription();

  disabledClaim: boolean = true;

  checkedRoles: string[] = [];
  mimUser: SaveUser | undefined;

  checkedDistributableRoles: any[] = [];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private mimService: MimService,
    private branchService: BranchService,
    private roleClaimService: RoleClaimService,
    private roleservice: RoleService,
    private jobTitleService: JobTitleService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<UserFormDialogComponent>,
    private loaderService: LoaderService
  ) {
  }

  ngOnInit() {
    this.init();
    ///
    this.loaderService.startLoading(LoaderComponent);

    var sources = [
      this.branchService.getAll(),
      this.roleservice.getAll(),
      this.jobTitleService.getAll(),
    ];

    this.getClaims();

    this.subs.add(
      forkJoin(sources).subscribe(
        (res) => {
          let result: any = res;
          this.branches = result[0].data;
          this.allRoles = result[1].data.items.filter(r => r.name != AppRole.SubBoardHead && r.name != AppRole.BoardMember);
          this.jobTitles = result[2].data.items;
          this.disRoles = this.allRoles.filter(r => r.isDistributable);
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
      Id: [''],
      userName: [{ value: '', disabled: true }, Validators.required],
      identityNumber: [{ value: '', disabled: true }, Validators.required],
      email: [{ value: '', disabled: true }, Validators.required],
      firstName: [{ value: '', disabled: true }, Validators.required],
      secondName: [{ value: '', disabled: true }, Validators.required],
      thirdName: [{ value: '', disabled: true }, Validators.required],
      lastName: [{ value: '', disabled: true }, Validators.required],
      phoneNumber: ['', Validators.required],
      branchId: ["", Validators.required],
      jobTitleId: [""],
      employeeNo: [null],
      extensionNumber: [null],
      enabled: [true],
      roles: [null, Validators.compose([Validators.required])],
      researchers: [[]],
      signature: [null, Validators.required]
    });
  }

  private populateUserForm(user: User) {

    this.form.patchValue({
      Id: user.id,
      firstName: user.firstName,
      secondName: user.secondName,
      thirdName: user.thirdName,
      lastName: user.lastName,
      email: user.email,
      userName: user.userName,
      branchId: user.branchId,
      jobTitleId: user.jobTitleId,
      identityNumber: user.identityNumber,
      employeeNo: user.employeeNo,
      extensionNumber: user.extensionNumber,
      enabled: user.enabled,
      signature: user.signature,
      phoneNumber: user.phoneNumber?.replace('0966', ''),
      roles: user.roles?.map((r: any) => {
        return r.name;
      }),
      researchers: [],

    });
    if (user.signature) {
      this.form.controls['signature'].setErrors(null);
    }
  }
  isDepartmentSelected(roleId, departmentId): boolean {
    let userRole = this.myUserRoleDepartments.find(r => r.role.id == roleId)
    if (userRole) {
      let roleDepartment = userRole.departments.find(d => d == departmentId)
      if (roleDepartment)
        return true;
    }
    return false;
  }

  onChangeRoles() {

    // get checked roles with claims
    var checkedRoles = this.form.controls["roles"].value;
    if (checkedRoles.includes(AppRole.Distributor))
      this.form.controls['claims'].enable();
    else {
      this.form.get('claims')?.setValue(this.allClaims.map((x: any) => false));
      this.form.controls['claims'].disable();
    }

    // get distributable roles
    this.checkedDistributableRoles = this.allRoles.filter(r => r.isDistributable && checkedRoles.includes(r.name));

    // remove not selected role from myUserRoleDepartments
    this.myUserRoleDepartments = this.myUserRoleDepartments.filter(r => this.checkedDistributableRoles.map(r => r.id).includes(r.role.id));


    // add not exists role to myUserRoleDepartments
    var myUserRoleDepartmentsIds = this.myUserRoleDepartments.map(r => r.role.id);
    var notAddedRoles = this.checkedDistributableRoles.filter(d => !myUserRoleDepartmentsIds.includes(d.id));
    notAddedRoles.forEach(r => {
      this.myUserRoleDepartments.push({
        role: {
          id: r.id,
          name: r.nameAr
        }, departments: []
      })
    });
    ////

    this.checkRequiredDepartmentSelection();
  }

  checkRequiredDepartmentSelection() {

    var userRoleDepartmentRoleIds = this.myUserRoleDepartments.filter(d => d.departments.length > 0).map(d => d.role.id);
    this.requiredDepartmentSelection = this.checkedDistributableRoles.length > 0 && !this.checkedDistributableRoles.every(v => userRoleDepartmentRoleIds.includes(v.id));
  }
  onSelectUserRoleDepartment(checked: any, roleId: string, departmentId: number) {
    var userRole = this.myUserRoleDepartments.find(d => d.role.id == roleId);

    if (checked) {
      // check if user role exists
      if (userRole) {
        // check if user role departments exists
        var userRoleDepartment = userRole.departments.find(d => d == departmentId);
        if (!userRoleDepartment)
          userRole.departments.push(departmentId);
      }
      else {
        this.myUserRoleDepartments.push({
          role: {
            id: roleId,
            name: ''
          },
          departments: [
            departmentId
          ]
        })
      }
    }
    else {
      // check if user role exists
      if (userRole) {
        // check if user role departments exists
        var userRoleDepartmentIndex = userRole.departments.findIndex(d => d == departmentId);
        if (userRoleDepartmentIndex > -1) {
          userRole.departments.splice(userRoleDepartmentIndex, 1);

        }
      }
    }
    this.checkRequiredDepartmentSelection();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSearch() {

    this.user = new User();
    this.user.firstName = "اول";
    this.user.secondName = "ثانى";
    this.user.thirdName = "ثالث";
    this.user.lastName = "اخير";

    this.user.identityNumber = this.searchText;
    this.user.userName = this.searchText;

    this.user.email = "test@meo.sa";

    this.populateUserForm(this.user);
    this.getUserRoleDepartment();
  }

  onSubmit() {


    this.loaderService.startLoading(LoaderComponent);

    // get checked claims
    const checkboxControl = (this.form.controls.claims as FormArray);

    // get selected user role departments
    var userRoleDepartments = [];
    this.myUserRoleDepartments.forEach(r => {
      r.departments.forEach(d => {
        userRoleDepartments.push({
          departmentId: d,
          roleId: r.role.id
        })
      });
    });

    const data = {
      ...this.user,
      ...this.form.value,
      claims: checkboxControl.value.filter((value: any) => value),
      userRoleDepartments
    }
    this.subs.add(
      this.userService.create(data).subscribe(
        (res: any) => {
          if (res.isSuccess) {
            this.loaderService.stopLoading();
            this.alert.succuss('تمت عملية الإضافة بنجاح');
            this.onCancel(res);
          }
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية الإضافة !');
        }
      )
    );

  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }

  getClaims() {
    this.roleClaimService.getAll()
      .subscribe((res: any) => {
        this.allClaims = res.data.items;
        this.form.addControl('claims', this.fb.array(this.allClaims.map((x: any) => false)));
        const checkboxControl = (this.form.controls.claims as FormArray);
        this.subs = checkboxControl.valueChanges.subscribe(checkbox => {
          checkboxControl.setValue(
            checkboxControl.value.map((value: string, i: any) => value ? this.allClaims[i].claimValue : false),
            { emitEvent: false }
          );
        });
      });
  }

  // disableAdminRole(roleName: string) {
  //   if (this.authService.checkRole(AppRole.Admin)) {
  //     return roleName == 'Admin';
  //   } else return false;
  // }
  myUserRoleDepartments: myUserDepartmentRole[] = [];

  getUserRoleDepartment() {
    this.user?.userRoleDepartments.forEach(role => {
      let userRole = this.myUserRoleDepartments.find(r => r.role?.id == role.roleId)
      if (userRole) {
        let roleDepartment = userRole.departments.find(d => d == role.departmentId)
        if (!roleDepartment) {
          userRole.departments.push(role.departmentId);
        }
      }
      else {
        let roleDepartment: myUserDepartmentRole = {
          role: {
            id: role.roleId,
            name: role.roleNameAr
          },
          departments: [role.departmentId]
        };
        this.myUserRoleDepartments.push(roleDepartment);
      }
    });
    this.checkRequiredDepartmentSelection();
  }
  disableResearcherOrConsultant(roleName: string) {
    if (this.form.controls["roles"].value?.includes(AppRole.LegalConsultant) && roleName == "LegalResearcher")
      return true;
    else if (this.form.controls["roles"].value?.includes(AppRole.LegalResearcher) && roleName == "LegalConsultant")
      return true;
    else
      return false;
  }
  onSelectBranch(value: any) {
    this.subs.add(this.branchService.getDepartments(value).subscribe(
      (res: any) => {
        this.branchDepartments = res.data;
        this.getUserRoleDepartment();
      }, (error) => {
        this.alert.error("فشلت  عملية جلب البيانات");
        console.error(error);
      }
    ));
  }

  // private populateActiveDirectoryForm(user: SaveUser) {
  //   this.onSelectGeneralDepartment(user.generalManagementId);
  //   this.form.patchValue({
  //     Id: user.id,
  //     firstName: user.firstName,
  //     lastName: user.lastName,
  //     email: user.email,
  //     userName: user.userName,
  //     generalManagementId: user.generalManagementId,
  //     jobTitleId: user.jobTitleId,
  //     identityNumber: user.identityNumber,
  //     extensionNumber: user.extensionNumber,
  //     //enabled: user.enabled,
  //     phoneNumber: user.phoneNumber?.replace('0966', ''),
  //     researchers: [],
  //   });
  // }

  onSignatureChange(image: any) {
    this.form.controls['signature'].setValue(image);
  }
}
