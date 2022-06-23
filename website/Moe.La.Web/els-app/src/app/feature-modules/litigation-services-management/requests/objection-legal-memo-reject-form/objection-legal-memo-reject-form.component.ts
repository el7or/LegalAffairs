import { LoaderService } from './../../../../core/services/loader.service';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import Swal from 'sweetalert2';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-objection-legal-memo-reject-form',
  templateUrl: './objection-legal-memo-reject-form.component.html',
  styleUrls: ['./objection-legal-memo-reject-form.component.css']
})
export class ObjectionLegalMemoRejectFormComponent implements OnInit, OnDestroy {

  form: FormGroup = Object.create(null);

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  private subs = new Subscription();
  constructor(
    private requestService: RequestService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ObjectionLegalMemoRejectFormComponent>,
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
      requestStatus: [this.RequestStatus.Rejected]
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
          this.requestService.replyObjectionLegalMemoRequest(this.form.value).subscribe(result => {
            this.alert.succuss("تم رفض الطلب بنجاح");
            this.loaderService.stopLoading();
            this.onCancel(this.form.controls['replyNote'].value);
          }, (error) => {
            console.error(error);
            this.loaderService.stopLoading();
            this.alert.error("فشلت عملية رفض الطلب");
          }));
      }
    });

  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

}
