import { Component, OnInit, Inject } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { QueryObject } from 'app/core/models/query-objects';
import { CaseStatus } from 'app/core/enums/CaseStatus';

@Component({
  selector: 'app-change-case-status-form',
  templateUrl: './change-case-status-form.component.html',
  styleUrls: ['./change-case-status-form.component.css'],
})
export class ChangeCaseStatusFormComponent implements OnInit {
  caseId: number = 0;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  caseStatus: any;

  queryObject: QueryObject = new QueryObject({
    sortBy: 'parent',
    pageSize: 9999,
  });
  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }
  constructor(
    private fb: FormBuilder,
    private alert: AlertService,
    public dialogRef: MatDialogRef<ChangeCaseStatusFormComponent>,
    private loaderService: LoaderService,
    private caseService: CaseService,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.caseId) {
      this.caseId = this.data.caseId;
      this.caseStatus = this.data.caseStatus;
    }
  }

  ngOnInit() {
    this.init();

  }

  /**
   * Component init.
   */
  private init(): void {

    this.form = this.fb.group({
      note: [null, Validators.compose([Validators.required])],
    });


  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService
        .changeStatus(this.caseId, this.caseStatus, this.form.value.note)
        .subscribe(
          (res) => {
            this.loaderService.stopLoading();
            this.alert.succuss('تم إحالة القضية بنجاح');
            this.onCancel(res);
          },
          (error) => {
            if (this.caseStatus == CaseStatus.ReturnedToRegionsSupervisor || this.caseStatus == CaseStatus.ReceivedByLitigationManager) {
              this.alert.error('فشلت عملية إعادة القضية !');
            }
            else { this.alert.error('فشلت عملية إحالة القضية !'); }
            this.loaderService.stopLoading();
            console.error(error);
          }
        )
    );

  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
