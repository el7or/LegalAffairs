import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { AlertService } from 'app/core/services/alert.service';
import { ConsultationService } from 'app/core/services/consultation.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-consultation-review-form',
  templateUrl: './consultation-review-form.component.html',
  styleUrls: ['./consultation-review-form.component.css']
})
export class ConsultationReviewFormComponent implements OnInit {
  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  consultationId: any;
  consultationStatus: any;
  returnedType: any;
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ConsultationReviewFormComponent>,
    private alert: AlertService,
    private consultationService: ConsultationService,
    private loaderService: LoaderService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.consultationId) this.consultationId = this.data.consultationId;
    if (this.data.consultationStatus) this.consultationStatus = this.data.consultationStatus;
    if (this.data.returnedType) this.returnedType = this.data.returnedType;
  }

  ngOnInit(): void {
    this.init();
  }

  private init(): void {
    this.form = this.fb.group({
      id: [this.consultationId],
      departmentVision: ['', Validators.compose([Validators.required])],
      consultationStatus: [this.consultationStatus],
      consultation: [null],
      returnedType: [this.returnedType]
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.consultationService.consultationReview(this.form.value).subscribe(result => {
        this.alert.succuss("تمت إعادة الطلب للصياغة بنجاح");
        this.loaderService.stopLoading();
        this.onCancel(result);
        this.router.navigateByUrl('/consultation')
      }, error => {
        this.loaderService.stopLoading();
        this.alert.error("فشلت عملية إعادة  الطلب للصياغة");
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

}


