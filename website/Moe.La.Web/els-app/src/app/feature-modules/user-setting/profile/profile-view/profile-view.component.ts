import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { UserService } from 'app/core/services/user.service';
import { LoaderService } from 'app/core/services/loader.service';
import { AuthService } from 'app/core/services/auth.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { Constants } from 'app/core/constants';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-profile-view',
  templateUrl: './profile-view.component.html',
  styleUrls: ['./profile-view.component.css']
})
export class ProfileViewComponent implements OnInit {
  user: any;
  subs = new Subscription();
  signaturePath:any;
  baseUrl=Constants.BASE_URL+"/uploads/user-signatures/";
  constructor(
    private userService: UserService,
    private loaderService: LoaderService,
    private route: ActivatedRoute,
    public authService: AuthService,
    private router: Router,
    private alert: AlertService,
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.PopulateUser();
  }
  userRoleDepartments: any = [];
  PopulateUser() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.userService.get(this.authService.currentUser?.id).subscribe(
        (result: any) => {
          this.user = result.data;

          this.signaturePath = this.sanitizer.bypassSecurityTrustUrl(this.user?.signature);


          this.user.phoneNumber = this.user.phoneNumber == null ? null : '0' + this.user.phoneNumber;
          if (this.user.picture != null)
            this.user.picture= Constants.BASE_URL + this.user.picture;
          this.user.roles.forEach((element: any) => {
          });
          this.user.roles = this.user.roles.filter
            (r => this.user.userRoleDepartments?.findIndex(rd => rd.roleName == r.name) < 0)
          this.loaderService.stopLoading();
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
          console.error(error);
        }));
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
