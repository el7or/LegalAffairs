import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router,
    public authService: AuthService) { }

  canActivate(router: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    if (this.authService.isTokenExpired) {
      this.authService.logout();
    }

    if (window.sessionStorage.getItem('auth_token')) {
      return true;
    } else {
      this.router.navigate(['auth', 'login'], {
        queryParams: { returnUrl: state.url },
      });
      return false;
    }
  }
}
