import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CaseService } from 'app/core/services/case.service';
import { CaseDetails, CaseListItem } from 'app/core/models/case';
import { CaseQueryObject, TemplateQueryObject } from 'app/core/models/query-objects';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { ExportCaseJudgmentRequest } from 'app/core/models/export-case-judgment-request';
import { HearingSearchCasesComponent } from '../../hearings/hearing-search-cases/hearing-search-cases.component';
import { RequestLetterService } from 'app/core/services/request-letter.service';
import { TemplateImageComponent } from '../supporting-document-request-wizard/template-image/template-image.component';
import { LetterTemplateTypes } from 'app/core/enums/LetterTemplateTypes';
import { LetterTemplateService } from 'app/core/services/letter-template.service';
import { LetterTemplate } from 'app/core/models/letterTemplate';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';
import { RequestTypes } from 'app/core/enums/RequestTypes';
import { SendingTypes } from 'app/core/enums/SendingTypes';
import { RequestLetter } from 'app/core/models/supporting-document-request';
import { Constants } from 'app/core/constants';

@Component({
  selector: 'app-export-case-judgment-request-form',
  templateUrl: './export-case-judgment-request-form.component.html',
  styleUrls: ['./export-case-judgment-request-form.component.css'],
})
export class ExportCaseJudgmentRequestFormComponent implements OnInit, OnDestroy {
  caseId: number = 0;
  form: FormGroup = Object.create(null);
  caseRuleOpenState = false;

  subs = new Subscription();

  exportCaseJudgmentRequest: ExportCaseJudgmentRequest = {
    id: 0,
    caseId: 0,
    replyNote: '',
    request: {
      id: 0,
      requestType: RequestTypes.RequestExportCaseJudgment,
      requestStatus: RequestStatus.New,
      sendingType: SendingTypes.User,
      letter: new RequestLetter(),
      note:''
    },
  };

  caseQueryObject: CaseQueryObject = new CaseQueryObject({
    pageSize: 9999,
    isFinalJudgment: true,
    isClosedCase: true,
    isCaseDataCompleted: true
  });

  caseDetails: CaseDetails = new CaseDetails();
  requestId: number;
  oldNote: string;
  oldtext: string;

  letterTemplates: LetterTemplate[] = [];
  baseUrl = Constants.BASE_URL;
  isNext: boolean = false;
  requestLetterform: FormGroup = Object.create(null);
  @ViewChild('stepper') stepper;

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  public Editor = CustomEditor;
  public config = {
    toolbar: {
      items: [
        'heading',
        '|',
        'bold',
        'italic',
        'underline',
        'link',
        'bulletedList',
        'numberedList',
        '|',
        'indent',
        'outdent',
        '|',
        'blockQuote',
        'insertTable',
        'undo',
        'redo',
        '|',
        "alignment:left", "alignment:right", "alignment:center"
      ]
    },
    image: {
      toolbar: [
        'imageStyle:full',
        'imageStyle:side',
        '|',
        'imageTextAlternative'
      ]
    },
    table: {
      contentToolbar: [
        'tableColumn', 'tableRow', 'mergeTableCells'
      ]
    },
    // This value must be kept in sync with the language defined in webpack.config.js.
    language: 'ar',
    dir: 'rtl',
    fontFamily: {
      supportAllValues: true
    },
    fontSize: {
      options: [9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 22, 24, 26],
      supportAllValues: true
    }
  };
  constructor(
    private fb: FormBuilder,
    public requestService: RequestService,
    private caseService: CaseService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private activatedRouter: ActivatedRoute,
    private router: Router,
    public location: Location,
    private dialog: MatDialog,
    private requestLetterService: RequestLetterService,
    private letterTemplateService: LetterTemplateService,

  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      this.requestId = +params.get('id');
    });
  }

  ngOnInit(): void {
    this.init();
    this.populateLetterTemplates();

    if (this.requestId) {
      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.requestService.getExportCaseJudgmentRequest(this.requestId).subscribe((result: any) => {
          this.loaderService.stopLoading();

          this.populateForm(result.data);

        }, err => this.loaderService.stopLoading())
      );
    }
  }

  init() {
    this.form = this.fb.group({
      id: [0],
      requestStatus: [RequestStatus.Draft],
      caseId: [null, Validators.compose([Validators.required])],
      note: [null, Validators.compose([Validators.required])]
    });

    this.requestLetterform = this.fb.group({
      text: [null, Validators.required],
      requestId: [this.requestId, Validators.required]

    });
  }
  populateForm(exportCaseJudgmentRequest: any) {

    this.onCaseChanged(exportCaseJudgmentRequest.caseId);

    this.oldNote = exportCaseJudgmentRequest.request.note;
    this.oldtext = exportCaseJudgmentRequest.request?.letter?.text;

    this.form.patchValue({
      id: exportCaseJudgmentRequest.request.id,
      requestStatus: exportCaseJudgmentRequest.request?.requestStatus?.id,
      caseId: exportCaseJudgmentRequest.caseId,
      note: exportCaseJudgmentRequest.request.note
    });

    this.requestLetterform.patchValue({
      text: exportCaseJudgmentRequest.request?.letter?.text,
      requestId: exportCaseJudgmentRequest.request?.id

    });
  }


  onChooseCase() {
    const dialogRef = this.dialog.open(HearingSearchCasesComponent, {
      width: '90%',
      height: '90%',
      data: { ...this.caseQueryObject, errorMsg: 'لا توجد قضايا في النظام الحكم لها نهائي لتصدير الحكم' },

    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res: CaseListItem) => {
          if (res) {
            this.loaderService.startLoading(LoaderComponent);
            this.form.patchValue({
              case: res.subject,
            });
            this.onCaseChanged(res.id);
          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  populateLetterTemplates() {
    let queryObject: TemplateQueryObject = {
      sortBy: 'name',
      isSortAscending: true,
      page: 1,
      pageSize: 9999,
      type: LetterTemplateTypes.CaseClosingLetter,
      name: ''
    };
    this.subs.add(this.letterTemplateService.getWithQuery(queryObject).subscribe((res: any) => {

      this.letterTemplates = res.data.items;
    }, (error) => {
      console.error(error);
      this.alert.error('فشل تحميل قائمة النماذج !');

    }))
  }

  onCaseChanged(caseId: any) {
    this.caseId = caseId;
    this.subs.add(
      this.caseService.get(caseId).subscribe((result: any) => {
        this.caseDetails = result.data;
        this.form.controls['caseId'].setValue(caseId);
        this.loaderService.stopLoading();
      })
    );

  }

  openImageDialog(name: any, image: any) {
    this.dialog.open(TemplateImageComponent, {
      width: '600px',
      data: { name: name, image: image }
    });
  }
  getContent(letterId: any) {
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(this.requestLetterService.getReplaceCaseCloseRequestContent(this.caseId, letterId).subscribe(
      (res: any) => {
        this.loaderService.stopLoading();

        this.requestLetterform.controls['text'].setValue(res.data);

      }
      , () => {
        this.loaderService.stopLoading();

      }
    ));
  }
  onSubmit(status, next = false) {
    this.exportCaseJudgmentRequest.request.note = this.form.value.note;
    this.exportCaseJudgmentRequest.caseId = this.form.value.caseId;
    this.exportCaseJudgmentRequest.id = this.form.value.id;

    this.exportCaseJudgmentRequest.request.letter = this.requestLetterform.value;

    this.exportCaseJudgmentRequest.request.requestStatus = status;

    if (this.requestId) {
      if (this.oldNote == this.form.value.note && this.oldtext == this.requestLetterform.value.text && status == this.RequestStatus.Modified && !next ) {
        this.alert.error("لم تتم عمليه الحفظ. لا يوجد اي تغيير في بيانات الطلب!!");
        return;
      }
      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.requestService.editExportCaseJudgmentRequest(this.exportCaseJudgmentRequest).subscribe(result => {
          this.loaderService.stopLoading();
          this.alert.succuss("تم تعديل الطلب بنجاح");
          if (next) {
            this.isNext = true;
            this.stepper.selected.completed = true;
            this.stepper.next();
          }
          else
            this.router.navigateByUrl('/requests');

        }, error => {
          console.error(error);
          this.loaderService.stopLoading();
          let message = 'فشلت عملية تعديل الطلب !';
          this.alert.error(message);
        })
      );
    }
    else {
      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.requestService
          .createExportCaseJudgmentRequest(this.exportCaseJudgmentRequest)
          .subscribe(
            (result: any) => {

              this.requestId = result.data.id;
              this.form.controls['id'].setValue(this.requestId);

              this.loaderService.stopLoading();
              this.alert.succuss('تم حفظ طلبك بنجاح');
              if (next) {
                this.isNext = true;
                this.stepper.selected.completed = true;
                this.stepper.next();
              }
              else
                this.router.navigateByUrl('/requests');

            },
            (error) => {
              console.error(error);
              this.loaderService.stopLoading();
              let message = 'فشلت عملية تقديم الطلب !';
              this.alert.error(message);
            }
          )
      );
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
