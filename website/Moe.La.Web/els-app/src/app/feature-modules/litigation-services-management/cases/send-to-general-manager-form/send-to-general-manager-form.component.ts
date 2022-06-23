import { Component, OnInit, Inject } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { QueryObject } from 'app/core/models/query-objects';
import { BranchService } from 'app/core/services/branch.service';
import { Branch } from 'app/core/models/branch';

@Component({
  selector: 'app-send-to-general-manager-form',
  templateUrl: './send-to-general-manager-form.component.html',
  styleUrls: ['./send-to-general-manager-form.component.css'],
})
export class SentToBranchManagerFormComponent implements OnInit {
  caseId: number = 0;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  queryObject: QueryObject = new QueryObject({
    sortBy: 'parent',
    pageSize: 9999,
  });
  departments: Branch[] = [];
  constructor(
    private fb: FormBuilder,
    private alert: AlertService,
    public dialogRef: MatDialogRef<SentToBranchManagerFormComponent>,
    private loaderService: LoaderService,
    private caseService: CaseService,
    public branchService: BranchService,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.caseId) {
      this.caseId = this.data.caseId;
    }
  }

  ngOnInit() {
    this.init();
    this.populateBranch();
  }

  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      note: [null, Validators.compose([Validators.required])],
      branchId: ["", Validators.compose([Validators.required])],
    });


  }
  populateBranch() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.branchService
        .getWithQuery(this.queryObject)
        .subscribe(
          (result: any) => {
            this.departments = result.data.items;
            this.loaderService.stopLoading();
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
    );
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService
        .sendToBranchManager(this.caseId, this.form.value.note, this.form.value.branchId)
        .subscribe(
          (res) => {
            this.loaderService.stopLoading();
            this.alert.succuss('تم إحالة القضية بنجاح');
            this.onCancel(res);
          },
          (error) => {
            console.error(error);
            this.alert.error(error);
            this.loaderService.stopLoading();
          }
        )
    );

  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
