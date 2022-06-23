import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from 'app/core/services/user.service';
import { LoaderService } from 'app/core/services/loader.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'app/core/services/auth.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AlertService } from 'app/core/services/alert.service';
import { Constants } from 'app/core/constants';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-user-view',
  templateUrl: './user-view.component.html',
  styleUrls: ['./user-view.component.css']
})
export class UserViewComponent implements OnInit, OnDestroy {
  showProfile: boolean = false;
  user: any;
  userId: any;
  subs = new Subscription();
  userAdmin: boolean = false;
  // myForm = new FormGroup({
  //   file: new FormControl('', [Validators.required]),
  // });
  signaturePath: any;
  baseUrl = Constants.BASE_URL + "/uploads/user-signatures/";

  constructor(
    private userService: UserService,
    private loaderService: LoaderService,
    private route: ActivatedRoute,
    public authService: AuthService,
    private router: Router,
    private alert: AlertService,
    private sanitizer: DomSanitizer
  ) {

    this.route.params.subscribe((p) => {
      this.userId = p["id"];
      if (this.userId === "") {
        this.router.navigateByUrl('/users');
        return;
      }
    });
  }

  ngOnInit(): void {
    this.PopulateUser();

    this.showProfile =
      this.router.url == "/users/view/" + this.authService.currentUser?.id;
  }

  userRoleDepartments: any = [];

  PopulateUser() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.userService.get(this.userId).subscribe(
        (result: any) => {
          this.user = result.data;

          this.signaturePath = this.sanitizer.bypassSecurityTrustUrl(this.user?.signature);

          this.user.phoneNumber = this.user.phoneNumber == null ? null : '0' + this.user.phoneNumber;
          if (this.user.pictureUrl != null)
            this.user.pictureUrl = Constants.BASE_URL + this.user.pictureUrl;
          this.user.roles.forEach((element: any) => {
            this.userAdmin = element.name == "admin";
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

  onDelete(userId: number) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام عملية الحذف؟',
      icon: 'error',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'حذف',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.userService.delete(userId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.router.navigateByUrl('/users');
            },
            (error) => {
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الحذف !');
              console.error(error);
            }
          )
        );
      }
    });
  }
}
