import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { BranchService } from 'app/core/services/branch.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { MinistrySectorService } from 'app/core/services/ministry-sectors.service';
import { QueryObject } from 'app/core/models/query-objects';

@Component({
  selector: 'app-judgment-form',
  templateUrl: './judgment-form.component.html',
  styleUrls: ['./judgment-form.component.css'],
})
export class JudgmentFormComponent implements OnInit {
  caseRuleId!: number;
  caseId!: number;
  litigationTypes: any;
  judgementResults: any;
  departments: any[] = [];
  ministrySectors: any[] = [];

  form: FormGroup = Object.create(null);
  subs = new Subscription();
  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: 9999,
  };
  constructor(
    private fb: FormBuilder,
    private caseService: CaseService,
    public branchService: BranchService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<JudgmentFormComponent>,
    private loaderService: LoaderService,
    private hijriConverter: HijriConverterService,
    public ministrySectorService: MinistrySectorService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.caseId) this.caseId = this.data.caseId;
    if (this.data.caseRuleId) this.caseRuleId = this.data.caseRuleId;
  }

  ngOnInit(): void {
    this.buildForm();
    if (this.caseRuleId) {
      // get case rule data if caseRuleId have value
    }
    this.populateMinistrySectors();
    this.populateLitigationTypes();
    this.populateJudgementResults();
    this.populateBranch();
  }
  populateMinistrySectors() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.ministrySectorService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.ministrySectors = result.data.items;
          this.loaderService.stopLoading()
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }
  

  buildForm() {
    this.form = this.fb.group({
      id: [0],
      caseId: [this.caseId],
      refNo: [null],
      refDate: [null],
      judgementText: [null, Validators.compose([Validators.required])],
      litigationType: ["", Validators.compose([Validators.required])],
      judgementResult: ["", Validators.compose([Validators.required])],
      judgmentBrief: [null],
      ministrySectorId: [null,Validators.compose([Validators.required])],
      caseRuleGeneralManagementIds: [
        "",
        Validators.compose([Validators.required]),
      ],
    });
  }

  populateLitigationTypes() {
    if (!this.litigationTypes) {
      this.subs.add(
        this.caseService.getLitigationTypes().subscribe(
          (result: any) => {
            this.litigationTypes = result;
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

  populateJudgementResults() {
    if (!this.litigationTypes) {
      this.subs.add(
        this.caseService.getJudgementResults().subscribe(
          (result: any) => {
            this.judgementResults = result;
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

  populateBranch() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.branchService
        .getWithQuery({
          sortBy: 'parent',
          isSortAscending: true,
          page: 1,
          pageSize: 99999,
        })
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

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.patchValue({
      refDate: this.hijriConverter.calendarDateToDate(
        this.form.get('refDate')?.value?.calendarStart
      ),
    });
    // need to handle if  update rule
    let result$ = this.caseService.addCaseRule(this.form.value);
    this.subs.add(
      result$.subscribe(
        () => {
          this.loaderService.stopLoading();
          this.alert.succuss('تمت عملية الإضافة بنجاح');
          this.dialogRef.close();
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية الإضافة !');
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
