import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription, forkJoin } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { RxwebValidators } from '@rxweb/reactive-form-validators';

import { AppRole } from 'app/core/models/role';
import { UserService } from 'app/core/services/user.service';
import { RoleService } from 'app/core/services/role.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { JobTitleService } from 'app/core/services/job-title.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { myUserDepartmentRole, User, UserDetails, UserRoleDepartment } from 'app/core/models/user';
import { AuthService } from 'app/core/services/auth.service';
import { RoleClaimService } from 'app/core/services/role-claim.service';
import { BranchService } from 'app/core/services/branch.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css'],
})
export class UserFormComponent implements OnInit {
  form: FormGroup;
  subs = new Subscription();
  user: User;

  // Lookups
  branches: KeyValuePairs[] = [];
  jobTitles: KeyValuePairs[] = [];
  allRoles: any[] = [];
  disRoles: any[] = [];
  allClaims: any = [];
  branchDepartments: any[] = [];

  checkedRoles: string[] = [];
  disabledClaim: boolean = true;

  signatureImage: any = '';
  checkedDistributableRoles: any[] = [];
  userId: string;
  requiredDepartmentSelection: boolean = false;
  constructor(
    private fb: FormBuilder,
    public userService: UserService,
    private roleservice: RoleService,
    private roleClaimService: RoleClaimService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private branchService: BranchService,
    private jobTitleService: JobTitleService,
    private route: ActivatedRoute,
    private router: Router,
    private sanitizer: DomSanitizer) {

  }

  ngOnInit() {
    this.init();

    this.subs = this.route.params.subscribe((params) => {
      this.userId = params['id'] || '';
      this.loaderService.startLoading(LoaderComponent);
      var sources = [
        this.branchService.getAll(),
        this.roleservice.getAll(),
        this.jobTitleService.getAll(),
      ];

      if (this.userId) {
        sources.push(this.userService.getUserDetails(this.userId));
      }

      this.subs.add(
        forkJoin(sources).subscribe(
          (res) => {
            let result: any = res;
            this.branches = result[0].data;
            this.allRoles = result[1].data.items.filter(r => r.name != AppRole.SubBoardHead && r.name != AppRole.BoardMember);
            this.jobTitles = result[2].data.items;
            this.disRoles = this.allRoles.filter(r => r.isDistributable);
            if (this.userId) {
              this.user = { ...this.user, ...result[3].data };
              this.populateUserForm(result[3].data);
              this.getClaims(result[3].data);
              this.onSelectBranch(this.user.branchId);
              //this.getUserRoleDepartment(result[3].data);
            }
            this.loaderService.stopLoading();
          },
          (error) => {
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
            console.error(error);
          }
        )
      );
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  /**
  * Component init.
  */
  private init(): void {
    this.form = this.fb.group({
      // Id: [''],
      // userName: [{ value: null }, Validators.required],
      // identityNumber: [{ value: '' }, Validators.required],
      // email: [{ value: null }, Validators.compose([Validators.required, RxwebValidators.email()])],
      // firstName: [{ value: null }, Validators.required],
      // secondName: [{ value: '' }, Validators.required],
      // thirdName: [{ value: '' }, Validators.required],
      // lastName: [{ value: null }, Validators.required],
      phoneNumber: [null, [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      branchId: ["", Validators.required],
      jobTitleId: [""],
      employeeNo: [null],
      extensionNumber: [null],
      enabled: [false],
      roles: [null],
      researchers: [[]],
      signature: [null, Validators.required]
    });

  }

  private populateUserForm(user: UserDetails) {
    this.form.patchValue({
      // Id: user.id,
      // firstName: user.firstName,
      // secondName: user.secondName,
      // thirdName: user.thirdName,
      // lastName: user.lastName,
      // email: user.email,
      // userName: user.userName,
      // identityNumber: user.identityNumber,
      phoneNumber: user.phoneNumber?.replace('0966', ''),
      branchId: user.branch?.id,
      jobTitleId: user.jobTitle?.id,
      employeeNo: user.employeeNo,
      extensionNumber: user.extensionNumber,
      enabled: user.enabled,
      roles: user.roles?.map((r: any) => {
        return r.name;
      }),
      researchers: [],
      signature: user.signature,
    });
    if (user.signature) {
      this.signatureImage = this.sanitizer.bypassSecurityTrustUrl(user.signature);
      this.form.controls['signature'].setErrors(null);
    }
  }

  getClaims(user: UserDetails) {
    this.roleClaimService.getAll()
      .subscribe((res: any) => {
        this.allClaims = res.data.items;
        this.form.addControl('claims', this.fb.array(this.allClaims.map(x => user.claims.find(c => c.claimValue == x.claimValue) ? x.claimValue : null)));
        const checkboxControl = (this.form.controls.claims as FormArray);
        this.subs = checkboxControl.valueChanges.subscribe(checkbox => {
          checkboxControl.setValue(
            checkboxControl.value.map((value: string, i: any) => value ? this.allClaims[i].claimValue : false),
            { emitEvent: false }
          );
        });
        ///
        this.onChangeRoles();
      });
  }

  checkRequiredDepartmentSelection() {
    var userRoleDepartmentRoleIds = this.myUserRoleDepartments.filter(d => d.departments.length > 0).map(d => d.role.id);
    this.requiredDepartmentSelection = this.checkedDistributableRoles.length > 0 && !this.checkedDistributableRoles.every(v => userRoleDepartmentRoleIds.includes(v.id));

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

  myUserRoleDepartments: myUserDepartmentRole[] = [];

  getUserRoleDepartment() {
    this.user.userRoleDepartments.forEach(role => {
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

  isDepartmentSelected(roleId, departmentId): boolean {
    let userRole = this.myUserRoleDepartments.find(r => r.role.id == roleId)
    if (userRole) {
      let roleDepartment = userRole.departments.find(d => d == departmentId)
      if (roleDepartment)
        return true;
    }
    return false;
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
          roleId: r.role.id,
          userId: this.userId
        })
      });
    });

    const data = {
      ...this.user,
      ...this.form.value,
      claims: checkboxControl.value.filter((value: any) => value),
      userRoleDepartments
    }

    let result$ = this.user.id
      ? this.userService.update(data)
      : this.userService.create(data);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          let message = this.user.id
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
          this.router.navigateByUrl('users');
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          this.alert.error("فشلت عملية الحفظ !");
        }
      )
    );
  }

  disableResearcherOrConsultant(roleName: string) {
    if (this.form.controls["roles"].value?.includes(AppRole.LegalConsultant) && roleName == "LegalResearcher")
      return true;
    else if (this.form.controls["roles"].value?.includes(AppRole.LegalResearcher) && roleName == "LegalConsultant")
      return true;
    else
      return false;
  }
  onSignatureChange(signatureImage: any) {
    this.form.controls['signature'].setValue(signatureImage);
  }
}
export class userDepartmentRole {
  departmentId: number = 0;
  checked: boolean;
}
