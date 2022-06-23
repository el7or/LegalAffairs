import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import Swal from 'sweetalert2';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-change-hearing-researcher-reject-form',
  templateUrl: './change-hearing-researcher-reject-form.component.html',
  styleUrls: ['./change-hearing-researcher-reject-form.component.css']
})
export class ChangeHearingResearcherRejectFormComponent implements OnInit, OnDestroy {

  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  constructor(
    private requestService: RequestService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ChangeHearingResearcherRejectFormComponent>,
    private alert: AlertService,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    this.init();
    this.form.controls['id'].setValue(this.data.requestId);
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  /**
 * Component init.
 */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      replyNote: [null, Validators.compose([Validators.required])],
    });
  }

  onSubmit() {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من رفض الطلب؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'رفض',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.requestService.rejectChangeResearcherToHearing(this.form.value).subscribe(result => {
            this.loaderService.stopLoading();
            this.onCancel(this.form.controls['replyNote'].value);
            this.alert.succuss("تم رفض الطلب بنجاح");
          }, (error) => {
            console.error(error);
            this.loaderService.stopLoading();
            this.alert.error("فشلت عملية رفض الطلب");
            this.onCancel();
          }));
      }
    });
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

}
