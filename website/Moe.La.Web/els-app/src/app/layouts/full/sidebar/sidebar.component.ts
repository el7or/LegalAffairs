import {
  ChangeDetectorRef,
  Component,
  OnDestroy
} from '@angular/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { MediaMatcher } from '@angular/cdk/layout';


import { MenuItems } from '../../../shared/menu-items/menu-items';
import { AuthService } from 'app/core/services/auth.service';
import { RoleDepartments } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: []
})
export class AppSidebarComponent implements OnDestroy {
  public config: PerfectScrollbarConfigInterface = {};
  mobileQuery: MediaQueryList;
  currentUserRoles: any = this.authService.currentUser.roles;
  currentUserRolesDepartments = JSON.parse(sessionStorage.getItem('roles_departments'));

  private _mobileQueryListener: () => void;
  status = true;

  itemSelect: number[] = [];
  parentIndex = 0;
  childIndex = 0;

  setClickedRow(i: number, j: number) {
    this.parentIndex = i;
    this.childIndex = j;
  }
  subclickEvent() {
    this.status = true;
  }
  scrollToTop() {
    document.querySelector('.page-wrapper')?.scroll({
      top: 0,
      left: 0
    });
  }

  constructor(
    changeDetectorRef: ChangeDetectorRef,
    media: MediaMatcher,
    public menuItems: MenuItems,
    public authService: AuthService,

  ) {
    this.mobileQuery = media.matchMedia('(min-width: 768px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    // tslint:disable-next-line: deprecation
    this.mobileQuery.addListener(this._mobileQueryListener);
  }

  ngOnDestroy(): void {
    // tslint:disable-next-line: deprecation
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }
  checkUserRole(rolesDepartment: RoleDepartments[]) {
    let isRoleExist = false;
    rolesDepartment.forEach(rd => {
      if (this.currentUserRolesDepartments?.length != 0) {
        if (this.currentUserRolesDepartments?.some(userRole =>
          userRole.roleName == rd.role && (rd.departmentIds.includes(userRole.departmentId) || rd.departmentIds.includes(Department.All))))
          isRoleExist = true;
      }

      if (Array.isArray(this.currentUserRoles)) {
        if (this.currentUserRoles.some(userRole =>
          rd.role == userRole && rd.departmentIds == null))
          isRoleExist = true;
      }
      else
        if (this.currentUserRoles && rd.role == this.currentUserRoles)
          isRoleExist = true;
    })
    return isRoleExist;

    // if (this.currentUserRolesDepartments?.length != 0) {
    //   if (Array.isArray(this.currentUserRolesDepartments)) {
    //     return this.currentUserRolesDepartments.some(userRole =>
    //       rolesDepartment.find(rd => rd.role == userRole.roleName && rd.departmentId == userRole.departmentId));
    //   }            
    // }
    // // else {
    //   if (Array.isArray(this.currentUserRoles)) {
    //     alert("sss")
    //     return this.currentUserRoles.some(userRole =>
    //       rolesDepartment.find(rd => rd.role == userRole.roleName && rd.departmentId == null));
    //   }
    //   return this.currentUserRoles &&
    //     rolesDepartment.some(rd => rd.role == this.currentUserRoles);
  }
}
