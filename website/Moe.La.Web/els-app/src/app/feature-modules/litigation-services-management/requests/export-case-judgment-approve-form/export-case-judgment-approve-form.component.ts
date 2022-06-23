
import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { CaseClosingReasons } from 'app/core/enums/CaseClosingReasons';

@Component({
  selector: 'app-export-case-judgment-approve-form',
  templateUrl: './export-case-judgment-approve-form.component.html',
  styleUrls: ['./export-case-judgment-approve-form.component.css']
})
export class ExportCaseJudgmentApproveFormComponent implements OnInit {
  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  requestId: any;
  requestStatus: any;
  caseClosingReasons: any;
  closingType: CaseStatus = 0;
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ExportCaseJudgmentApproveFormComponent>,
    private alert: AlertService,
    private requestService: RequestService,
    private loaderService: LoaderService,
    private router: Router,
    private hijriConverter: HijriConverterService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.requestId) this.requestId = this.data.requestId;
    if (this.data.requestStatus) this.requestStatus = this.data.requestStatus;
    // if (this.data.closingType) this.closingType = this.data.closingType;
  }

  public get CaseClosingReasons(): typeof CaseClosingReasons {
    return CaseClosingReasons;
  }

  ngOnInit(): void {
    this.init();
    this.populateCaseClosingTypes();
  }
  private init(): void {
    this.form = this.fb.group({
      id: [this.requestId],
      replyNote: [''],
      requestStatus: [this.requestStatus],
      caseClosingType: [""],
      exportRefNo: [null, Validators.compose([Validators.required])],
      exportRefDate: [null, Validators.compose([Validators.required])]
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.value.exportRefDate = this.hijriConverter.calendarDateToDate(
      this.form.get('exportRefDate')?.value?.calendarStart);
    this.subs.add(
      this.requestService.replyExportCaseJudgmentRequest(this.form.value).subscribe(result => {
        this.alert.succuss("تمت اعتماد الطلب بنجاح");
        this.loaderService.stopLoading();
        this.onCancel(result);
        this.router.navigateByUrl('/requests')
      }, error => {
        this.loaderService.stopLoading();
        this.alert.error("فشلت عمليةاعتماد الطلب");
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

  populateCaseClosingTypes() {
    this.subs.add(
      this.requestService.getCaseClosingReasons().subscribe(
        (result: any) => {
          this.caseClosingReasons = result;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }
}
