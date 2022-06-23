import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';

import { AlertService } from 'app/core/services/alert.service';
import { UserService } from 'app/core/services/user.service';
import { AuthService } from 'app/core/services/auth.service';
import { CaseSupportingDocumentRequestItemFormComponent } from '../supporting-document-request-item-form/supporting-document-request-item-form.component';
import { RequestService } from 'app/core/services/request.service';
import { CaseSupportingDocumentRequest, CaseSupportingDocumentRequestItem, RequestLetter } from 'app/core/models/supporting-document-request';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { Utilities } from 'app/shared/utilities';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { QueryObject, TemplateQueryObject } from 'app/core/models/query-objects';
import { LetterTemplateService } from 'app/core/services/letter-template.service';
import { RequestLetterService } from 'app/core/services/request-letter.service';
import { Constants } from 'app/core/constants';

import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';
import { TemplateImageComponent } from './template-image/template-image.component';
import { LetterTemplate } from 'app/core/models/letterTemplate';
import { LetterTemplateTypes } from 'app/core/enums/LetterTemplateTypes';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-supporting-document-request-wizard',
  templateUrl: './supporting-document-request-wizard.component.html',
  styleUrls: ['./supporting-document-request-wizard.component.css']
})
export class SupportingDocumentRequestWizardComponent implements OnInit, OnDestroy {
  columnsToDisplay = [
    'position',
    'name',
    'actions',
  ];

  hearingId?: number;
  caseId?: number;
  requestId?: number;
  isNext: boolean = false;
  oldRequest!: CaseSupportingDocumentRequest;
  letterTemplates: LetterTemplate[] = [];
  baseUrl = Constants.BASE_URL;
  createdOn: Date;
  createdOnHigri: string = "";
  documentRequest: CaseSupportingDocumentRequest = new CaseSupportingDocumentRequest();

  documentRequestItems!: MatTableDataSource<CaseSupportingDocumentRequestItem>;
  ministryDepartments: KeyValuePairs[] = [];
  letter: RequestLetter;

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  form: FormGroup = Object.create(null);
  requestLetterform: FormGroup = Object.create(null);
  public Editor = CustomEditor;
  public config = {
    language: 'ar',
    toolbar: {
      items: [
        'heading',
        '|',
        'bold',
        'italic',
        'link',
        'bulletedList',
        'numberedList',
        'alignment',
        'blockQuote',
        'undo',
        'redo',
        'fontColor',
        'fontFamily',
        'fontSize',
        'underline',
        'insertTable'
      ]
    },
    alignment: {
      options: ['left', 'right', 'center', 'justify']
    },
    allowedContent: true,
    extraAllowedContent: '*(*);*{*}'
  };

  private subs = new Subscription();

  @ViewChild(SupportingDocumentRequestWizardComponent, { static: true }) table: SupportingDocumentRequestWizardComponent | any;
  dataChanged: boolean = false;
  @ViewChild('stepper') stepper;

  constructor(
    private activatedRouter: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private alert: AlertService,
    public userService: UserService,
    private dialog: MatDialog,
    public location: Location,
    public authService: AuthService,
    private requestService: RequestService,
    private ministryDepartmentService: MinistryDepartmentService,
    private letterTemplateService: LetterTemplateService,
    private requestLetterService: RequestLetterService,
    private loaderService: LoaderService,

  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      this.hearingId = +params.get('hearingId');
      this.caseId = +params.get('caseId');
      this.requestId = +params.get('requestId');


    });
  }

  ngOnInit() {
    this.init();
    this.populateMinistryDepartments();

    if (this.requestId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.requestService.getDocumentRequest(this.requestId).subscribe((result: any) => {
          this.loaderService.stopLoading();

          this.setDocumentRequest(result.data);
          this.populateForm(result.data);
          this.createdOn = result.data.request.createdOn;
          this.createdOnHigri = result.data.request.createdOnHigri;
          this.documentRequestItems = new MatTableDataSource(this.documentRequest.documents);
          this.oldRequest = Utilities.cloneObject(this.documentRequest);
        }, err => this.loaderService.stopLoading())
      );
    }
  }

  init() {

    this.form = this.fb.group({
      consigneeDepartmentId: ["", Validators.compose([Validators.required, Validators.maxLength(100)])],
    });

    this.requestLetterform = this.fb.group({
      requestId: [this.requestId, Validators.required],
      text: [null, Validators.required]
    });
  }

  populateForm(documentRequest: CaseSupportingDocumentRequest) {

    this.form.patchValue({
      consigneeDepartmentId: documentRequest.consigneeDepartment.id,
    });

    if (documentRequest.request?.letter) {
      this.requestLetterform.patchValue({
        requestId: documentRequest.request?.letter?.requestId || this.requestId,
        text: documentRequest.request?.letter?.text
      });
      this.letter = documentRequest.request?.letter;
    }
  }

  setDocumentRequest(documentRequest: any) {
    this.documentRequest.id = documentRequest.id;
    this.documentRequest.hearingId = documentRequest.hearingId;
    this.documentRequest.parentId = documentRequest.parentId;
    this.documentRequest.caseId = documentRequest.caseId;
    this.documentRequest.consigneeDepartmentId = documentRequest.consigneeDepartment.id;
    this.documentRequest.replyNote = documentRequest.replyNote;
    this.documentRequest.documents = documentRequest.documents;
    this.documentRequest.request.requestStatus = documentRequest.request?.requestStatus?.id;
  }
  checkDataChanged() {
    return JSON.stringify(this.documentRequest).toLowerCase() != JSON.stringify(this.oldRequest).toLowerCase();
  }

  populateMinistryDepartments() {
    let queryObject: QueryObject = new QueryObject();

    this.subs.add(
      this.ministryDepartmentService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.ministryDepartments = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
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
      type: LetterTemplateTypes.CaseSupportingDocumentLetter,
      name: ''
    };
    this.subs.add(this.letterTemplateService.getWithQuery(queryObject).subscribe((res: any) => {

      this.letterTemplates = res.data.items;
    }, (error) => {
      console.error(error);
      this.alert.error('فشلت عملية جلب البيانات !');
    }))
  }

  deleteDocument(docIndex: number) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل تريد حذف هذا المستند من قائمة المستندات المطلوبة؟',
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'نعم',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.documentRequest.documents.splice(docIndex, 1);
        this.documentRequestItems = new MatTableDataSource(this.documentRequest.documents);
      }
    });
  }

  openImageDialog(name: any, image: any) {
    this.dialog.open(TemplateImageComponent, {
      width: '600px',
      data: { name: name, image: image }
    });
  }

  openDialog(name: any, index: number) {
    let dialogRef = this.dialog.open(CaseSupportingDocumentRequestItemFormComponent, {
      width: '30em',
      data: { name: name, Index: index }
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (result) => {
          if (result) {
            if (index == -1) {
              let document: CaseSupportingDocumentRequestItem = {
                documentRequestId: 0,
                name: result,
              };
              this.documentRequest.documents.push(document);
            }
            else {
              this.documentRequest.documents[index].name = result;
            }

          }
          this.documentRequestItems = new MatTableDataSource(this.documentRequest.documents);

        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  getContent(letterId: any) {
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(this.requestLetterService.getReplaceDocumentRequestContent(this.requestId, letterId).subscribe(
      (res: any) => {
        this.loaderService.stopLoading();
        this.requestLetterform.controls['text'].setValue(res.data);
      }
      , (error) => {
        this.loaderService.stopLoading();
        console.error(error);
        this.alert.error('فشلت عملية جلب البيانات !');
      }
    ));
  }

  onSubmit() {
    if (this.documentRequest.documents.length == 0) {
      this.alert.error("الرجاء إضافة المستندات المطلوبة!");
      return;
    }
    this.documentRequest.consigneeDepartmentId = this.form.value.consigneeDepartmentId;

    if (this.requestId) {

      if (!this.checkDataChanged()) {
        this.alert.error("لم تتم عمليه الحفظ. لا يوجد اي تغيير في بيانات الطلب!!");
        return;
      }

      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.requestService.editDocumentRequest(this.documentRequest).subscribe(result => {
          this.loaderService.stopLoading();

          //this.replaceDeplartment();
          this.populateLetterTemplates();

          this.alert.succuss("تم تعديل صيغة الطلب بنجاح");
          this.isNext = true;
          this.stepper.selected.completed = true;
          this.stepper.next();

        }, error => { this.loaderService.stopLoading(); })
      );
    }
    else {

      this.loaderService.startLoading(LoaderComponent);

      this.documentRequest.hearingId = this.hearingId;
      this.documentRequest.caseId = +this.caseId;
      this.subs.add(
        this.requestService.createDocumentRequest(this.documentRequest).subscribe((result: any) => {
          this.loaderService.stopLoading();

          //this.replaceDeplartment();
          this.populateLetterTemplates();

          this.alert.succuss("تم ارسال طلبك بنجاح");
          this.isNext = true;
          this.stepper.selected.completed = true;
          this.stepper.next();
          this.requestId = result.data.id;
          this.requestLetterform.controls['requestId'].setValue(this.requestId);
        }, error => { this.loaderService.stopLoading(); })
      );
    }
  }

  raiseConsultant() {
    let requestStatus;

    if (this.documentRequest?.request?.requestStatus == RequestStatus.Draft)
      requestStatus = RequestStatus.New;
    else if (this.documentRequest?.request?.requestStatus == RequestStatus.Returned)
      requestStatus = RequestStatus.Modified;

    this.onRequestLetterSubmit(requestStatus);
  }

  onRequestLetterSubmit(status: RequestStatus = null) {
    const requestStatus = status ? status : this.documentRequest.request.requestStatus;

    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.letter ? this.requestLetterService.update({ ...this.requestLetterform.value, requestStatus })
      : this.requestLetterService.create({ ...this.requestLetterform.value, requestStatus });

    this.subs.add(result$.subscribe(res => {
      this.loaderService.stopLoading();
      this.alert.succuss("تم الحفظ بنجاح");
      this.router.navigateByUrl('/requests');
    }, (error) => {
      this.loaderService.stopLoading();
      console.error(error);
      this.alert.error('فشلت عملية الحفظ !');
    }));

  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
