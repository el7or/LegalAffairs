import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { CaseService } from 'app/core/services/case.service';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';
import { GroupNames } from 'app/core/models/attachment';
import { CaseDetails } from 'app/core/models/case';
import { MinistrySectorService } from 'app/core/services/ministry-sectors.service';
import { MinistrySectorQueryObject, QueryObject } from 'app/core/models/query-objects';

@Component({
  selector: 'app-receive-judgment-instrument',
  templateUrl: './receive-judgment-instrument.component.html',
  styleUrls: ['./receive-judgment-instrument.component.css']
})
export class ReceiveJudgmentInstrumentComponent implements OnInit, OnDestroy {
  private subs = new Subscription();
  form: FormGroup = Object.create(null);
  attachments: any = [];
  filesCount: number = 0;
  caseId: number = 0;
  edit: boolean = false;
  caseRuleId: number = 0;
  judgementResults: any;
  ministryDepartments: any = [];
  ministrySectors: any = [];

  case: CaseDetails = new CaseDetails;
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  constructor(
    private activatedRouter: ActivatedRoute,
    private caseService: CaseService,
    public ministrySectorService: MinistrySectorService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private hijriConverter: HijriConverterService,
    private ministryDepartmentService: MinistryDepartmentService,
    public location: Location
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.caseId = +id;
      }
    });
  }

  ngOnInit(): void {
    this.loaderService.startLoading(LoaderComponent);
    this.init();
    this.populateJudgementResults();
    this.populateMeoSectors();
    this.populateCase();
  }
  init() {
    this.form = this.fb.group({
      id: [this.caseId],
      ruleNumber: [null],
      caseNumber: [null],
      judgmentBrief: [null, Validators.compose([Validators.maxLength(500)])],
      judgementText: [null, Validators.compose([Validators.required, Validators.maxLength(2000)])],
      judgementResult: ["", Validators.compose([Validators.required])],
      judgmentReasons: [null, Validators.compose([Validators.maxLength(500)])],
      feedback: [null, Validators.compose([Validators.maxLength(500)])],
      finalConclusions: [null, Validators.compose([Validators.maxLength(500)])],
      importRefNo: [null],
      importRefDate: [null],
      exportRefNo: [null],
      exportRefDate: [null],
      attachments: [null, Validators.compose([Validators.required])],
      ministrySectorId: [null, Validators.compose([Validators.required])],
      caseRuleMinistryDepartments: [null, Validators.compose([Validators.required])],

    });
  }
  populateCase() {
    this.subs.add(
      this.caseService.get(this.caseId).subscribe((result: any) => {
        if (result.isSuccess) {
          this.case = result.data;
          if (this.case.caseRule != null) {
            this.caseRuleId = result.data.id;
            this.patchForm(this.case.caseRule);
            this.onJudgmentAttachment(this.case.caseRule.attachments);
            this.edit = true;
          }
          this.loaderService.stopLoading();
        }
      }, (error) => {
        this.alert.error("فشلت  عملية جلب البيانات");
        this.loaderService.stopLoading();
        console.error(error);
      }));
  }

  populateJudgmentInstrument() {
    this.subs.add(
      this.caseService.getreceivingJudgmentInstrument(this.caseId).subscribe((result: any) => {
        if (result.isSuccess) {
          if (result.data != null) {
            this.caseRuleId = result.data.id;
            this.patchForm(result.data);
            this.onJudgmentAttachment(result.data.attachments);
            this.edit = true;
          }
        }
      }, (error) => {
        this.alert.error("فشلت  عملية جلب البيانات");
        console.error(error);
      }));
  }

  populateMoeDepartments(sector) {
    let queryObject: MinistrySectorQueryObject = {
      sortBy: 'name',
      isSortAscending: true,
      page: 1,
      pageSize: 9999,
      ministrySectorId: sector
    };
    this.subs.add(
      this.ministryDepartmentService.getWithQuery(queryObject).subscribe((result: any) => {
        if (result.isSuccess) {
          this.ministryDepartments = result.data.items;
        }
      }, (error) => {
        this.alert.error("فشلت  عملية جلب البيانات");
        console.error(error);
      }));
  }

  populateJudgementResults() {
    this.subs.add(
      this.caseService.getJudgementResults().subscribe(
        (result: any) => {
          this.judgementResults = result;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }
  populateMeoSectors() {
    let queryObject: QueryObject = {
      sortBy: 'name',
      isSortAscending: true,
      page: 1,
      pageSize: 9999,
    };
    this.subs.add(
      this.ministrySectorService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.ministrySectors = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }
  patchForm(receivingJudgment: any) {
    this.populateMoeDepartments(receivingJudgment.ministrySectorId);

    this.form.patchValue({
      id: receivingJudgment.id,
      ruleNumber: receivingJudgment.ruleNumber,
      caseNumber: receivingJudgment.caseNumber,
      judgmentBrief: receivingJudgment.judgmentBrief,
      judgementText: receivingJudgment.judgementText,
      judgementResult: receivingJudgment.judgementResult.id,
      caseRuleMinistryDepartments: receivingJudgment.caseRuleMinistryDepartments?.map((d: any) => {
        return d.id;
      }),
      judgmentReasons: receivingJudgment.judgmentReasons,
      feedback: receivingJudgment.feedback,
      finalConclusions: receivingJudgment.finalConclusions,
      importRefNo: receivingJudgment.importRefNo,
      importRefDate: receivingJudgment.importRefDate,
      exportRefNo: receivingJudgment.exportRefNo,
      exportRefDate: receivingJudgment.exportRefDate,
      ministrySectorId: receivingJudgment.ministrySectorId
    });
    this.attachments = receivingJudgment.attachments;
  }

  onSelectMinistrySector(sector) {
    this.form.controls["caseRuleMinistryDepartments"].setValue(null);
    this.form.controls["caseRuleMinistryDepartments"].updateValueAndValidity();
    this.populateMoeDepartments(sector);

  }

  onJudgmentAttachment(list: any) {
    this.attachments = list;
    this.form.controls['attachments'].setValue(this.attachments);
  }

  onSubmit() {
    this.form.value.importRefDate = this.hijriConverter.calendarDateToDate(
      this.form.get('importRefDate')?.value?.calendarStart);
    this.form.value.exportRefDate = this.hijriConverter.calendarDateToDate(
      this.form.get('exportRefDate')?.value?.calendarStart);
    this.loaderService.startLoading(LoaderComponent);
    const dataToSend: any = this.form.value;
    dataToSend.attachments = this.attachments;
    if (this.edit) {
      this.subs.add(
        this.caseService.editReceivingJudgmentInstrument(dataToSend).subscribe(result => {
          this.loaderService.stopLoading();
          this.location.back();
          this.alert.succuss("تم نعديل استلام  صك  الحكم بنجاح");

        }, (error) => {
          this.loaderService.stopLoading();
          this.alert.error("فشلت عملية  استلام  صك  الحكم");
          console.error(error);
        })
      );
    }
    else {
      this.subs.add(
        this.caseService.receivingJudgmentInstrument(dataToSend).subscribe(result => {
          this.loaderService.stopLoading();
          this.location.back();
          this.alert.succuss("تم  استلام  صك  الحكم بنجاح");

        }, (error) => {
          this.loaderService.stopLoading();
          this.alert.error("فشلت عملية  استلام  صك  الحكم");
          console.error(error);
        }))
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
