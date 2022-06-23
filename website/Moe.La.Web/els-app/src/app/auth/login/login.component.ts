import { LoaderService } from './../../core/services/loader.service';
import { AuthService } from './../../core/services/auth.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { Subscription } from 'rxjs';
import { AlertService } from 'app/core/services/alert.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit, OnDestroy {
  public form: FormGroup = Object.create(null);
  subs = new Subscription();
  status = false;
  panelOpenState = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private loaderService: LoaderService,
    private alert: AlertService) { }

  ngOnInit() {
    this.form = this.fb.group({
      username: [null, Validators.compose([Validators.required])],
      password: [null, Validators.compose([Validators.required])],
    });
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.authService
        .login(this.form.value.username, this.form.value.password)
        .subscribe(
          (result: any) => {
            this.loaderService.stopLoading();
            if (result) {
              let returnUrl = this.route.snapshot.queryParamMap.get(
                'returnUrl'
              );
              this.alert.succuss('تم تسجيل الدخول بنجاح');
              this.router.navigate([returnUrl || '/']);
            }
          },
          (error) => {
            console.error(error);
            this.loaderService.stopLoading();
            // this.alert.error('فشل في تسجيل الدخول');
          }
        )
    );
  }

  clickEvent() {
    this.status = !this.status;
  }
}
