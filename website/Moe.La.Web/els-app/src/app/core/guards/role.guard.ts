import { AuthService } from '../../core/services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import Swal from 'sweetalert2';

import { Department } from '../enums/Department';
import { AppRole, RoleDepartments } from '../models/role';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  jwtHelper = new JwtHelperService();

  constructor(public auth: AuthService, public router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const token: any = sessionStorage.getItem('auth_token');
    const userRoles: any[] = this.jwtHelper.decodeToken(token)?.roles;
    const userRoleDepartments: any[] = JSON.parse(sessionStorage.getItem('roles_departments'));

    const expectedRolesDepartment: RoleDepartments[] = route.data.rolesDepartment;
    let isRoleExist: boolean = false;

    expectedRolesDepartment.forEach((r) => {
      if (userRoleDepartments?.length != 0) {
        if (userRoleDepartments.some(rd => rd.roleName == r.role
          && (!r.departmentIds ||
            (r.departmentIds.includes(rd.departmentId)
              || r.departmentIds.includes(Department.All))))) {
          isRoleExist = true;
        }
      }

      if (r.departmentIds == null) {
        if (Array.isArray(userRoles)) {
          if (userRoles.indexOf(r.role) > -1) {
            isRoleExist = true;
          }
        }
        else
          if (userRoles == r.role) {
            isRoleExist = true;
          }
      }
    });

    if (!isRoleExist)
      Swal.fire({
        title: 'ممنوع !',
        text: 'ليس لديك صلاحية الدخول إلى هذه الصفحة',
        icon: 'error',
        confirmButtonText: 'حسناً',
      }).then((result: any) => {
        if (result.value) {
          this.router.navigateByUrl('/');
        }
      });
    return isRoleExist;
  }
}
