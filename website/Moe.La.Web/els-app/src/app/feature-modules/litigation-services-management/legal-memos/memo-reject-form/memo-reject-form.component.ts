import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LegalMemoService } from 'app/core/services/legal-memo.service';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';

@Component({
  selector: 'app-memo-reject-form',
  templateUrl: './memo-reject-form.component.html',
  styleUrls: ['./memo-reject-form.component.css']
})
export class MemoRejectFormComponent implements OnInit, OnDestroy {

  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  status: LegalMemoStatus;
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<MemoRejectFormComponent>,
    private alert: AlertService,
    private legalMemoService: LegalMemoService,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    this.init();
    this.form.controls['id'].setValue(this.data.id);
    this.status = this.data.id;

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
      note: [null, Validators.required],
    });
  }

  onSubmit() {

    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      this.legalMemoService.reject(this.form.value.id, this.form.value.note).subscribe(
        (result: any) => {

          this.loaderService.stopLoading();
          this.onCancel(result);

          this.alert.succuss("تم رفض المذكرة بنجاح");

        },
        (error) => {
          console.error(error);
          this.alert.error("فشلت عملية رفض المذكرة");
          this.loaderService.stopLoading();
        }));


  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

}
