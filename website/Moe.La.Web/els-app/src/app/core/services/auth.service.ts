import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppRole } from '../models/role';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

import { DataService } from './data.service';
import { AppSettingsService } from './app-settings.service';


@Injectable({ providedIn: 'root' })
export class AuthService extends DataService {
  jwtHelper = new JwtHelperService();

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);

  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  private user: any = null;

  private rolesDepartment: any = null;


  private refreshTokenTimeout;

  constructor(
    http: HttpClient,
    private appSettingsService: AppSettingsService,
    private router: Router
  ) {
    super(environment.API_URL + 'users', http);
    //this.loggedIn = !!localStorage.getItem('auth_token');
    //// ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    //// header component resulting in authed user nav links disappearing despite the fact user is still logged in
    //this._authNavStatusSource.next(this.loggedIn);

    if (sessionStorage.getItem('auth_token')) {
      this.user = this.jwtHelper.decodeToken(sessionStorage.getItem('auth_token') || undefined)
    }
    else this.user = null;

    if (sessionStorage.getItem('roles_departments')) {
      this.rolesDepartment = JSON.parse(sessionStorage.getItem('roles_departments'))
    }
    else this.rolesDepartment = null;
  }

  setLoggedIn() {
    // critical
    this.loggedIn = true;
  }

  login(userName: any, password: any, rule?: string) {
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    let api = rule
      ? environment.API_URL + 'auth/login/' + rule
      : environment.API_URL + 'auth/login';
    return this.http
      .post(api, JSON.stringify({ userName, password }), { headers })
      .map((response: any) => {
        let result = response;

        if (result && result.data.auth_token) {
          sessionStorage.setItem('auth_token', result.data.auth_token);

          sessionStorage.setItem('roles_departments', JSON.stringify(result.data.roles_departments));
          this.loggedIn = true;
          this._authNavStatusSource.next(true);
          this.appSettingsService.getAppSettings();
          this.user = this.jwtHelper.decodeToken(result.data.auth_token);
          this.rolesDepartment = result.data.roles_departments;
          return true;
        }
      });
    //.catch(this.handleError);
  }


  private startRefreshTokenTimer(Expires: any) {
    // set a timeout to refresh the token a minute before it expires
    this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), Expires * 10 * 1000);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }

  refreshToken() {
    return this.http.post(environment.API_URL + 'auth/refresh-token', {});
  }
  getToken(userId: string) {
    return this.http
      .get(environment.API_URL + 'auth/get-token/' + userId)

      .subscribe((result: any) => {
        if (result && result.auth_token)
          sessionStorage.setItem('auth_token', result.auth_token);
      });
  }

  logout() {
    window.sessionStorage.removeItem('auth_token');
    sessionStorage.removeItem('roles_departments');
    this.stopRefreshTokenTimer();
    this.loggedIn = false;
    this._authNavStatusSource.next(false);
    this.router.navigate(['/auth/login']);
  }

  get isLoggedIn(): boolean {
    return this.loggedIn;
  }

  get currentUser() {
    let token = sessionStorage.getItem('auth_token');

    if (!token) return null;

    return this.jwtHelper.decodeToken(token);
  }

  get userRoleDepartments() {
    return this.rolesDepartment;
  }

  get currentUserRole() {
    if (Array.isArray(this.currentUser.roles))
      return 'متعدد';

    let userRule = this.currentUser.roles;

    if (userRule == 'Admin')
      return 'مدير النظام';
    else if (userRule == 'GeneralSupervisor')
      return 'المشرف العام';
    else if (userRule == 'DepartmentManager')
      return 'مدير الإدارة';
    else if (userRule == 'RegionsSupervisor')
      return 'مشرف المناطق';
    else if (userRule == 'BranchManager')
      return 'مدير المنطقة';
    else if (userRule == 'LegalConsultant')
      return 'مستشار قانوني';
    else if (userRule == 'LegalResearcher')
      return 'باحث قانوني';
    else if (userRule == 'Investigator')
      return 'محقق';
    else if (userRule == 'InvestigationManager')
      return 'مدير التحقيقات';
    else if (userRule == 'AdministrativeCommunicationSpecialist')
      return 'مختص الاتصالات الادارية';
    else if (userRule == 'AllBoardsHead')
      return 'رئيس اللجان';
    else if (userRule == 'MainBoardHead')
      return 'أمين اللجنة الرئيسية';
    else if (userRule == 'SubBoardHead')
      return 'أمين اللجنة الفرعية';
    else if (userRule == 'Distributor')
      return 'موزع المعاملات'
    else if (userRule == 'DataEntry')
      return 'مدخل بيانات'

    return userRule;
  }

  checkRole(roleName: AppRole, departmentId?: number): boolean {

    if (this.rolesDepartment.length != 0 && departmentId) {
      if (Array.isArray(this.rolesDepartment)) {
        if (this.rolesDepartment.
          findIndex(r => r.roleName == roleName && r.departmentId == departmentId) > -1)
          return true;
      }
      if (this.rolesDepartment[0].roleName == roleName && this.rolesDepartment[0].departmentId == departmentId)
        return true;

    }
    else {
      if (this.user?.roles) {
        if (Array.isArray(this.user?.roles))
          if (this.user.roles.indexOf(roleName) > -1) return true;

        if (this.user.roles == roleName) return true;
      }
      return false;
    }
  }

  get isTokenExpired(): boolean {
    if (!this.currentUser)
      return true;
    return (Math.floor((new Date).getTime() / 1000)) >= this.currentUser.exp;
  }

}
