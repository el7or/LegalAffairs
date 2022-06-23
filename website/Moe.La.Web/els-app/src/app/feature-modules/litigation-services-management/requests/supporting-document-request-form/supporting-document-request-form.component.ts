import { Location } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
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
import { CaseSupportingDocumentRequest, CaseSupportingDocumentRequestItem, Request } from 'app/core/models/supporting-document-request';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { Utilities } from 'app/shared/utilities';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { QueryObject, TemplateQueryObject } from 'app/core/models/query-objects';
import { LetterTemplateService } from 'app/core/services/letter-template.service';
import { RequestLetterService } from 'app/core/services/request-letter.service';

import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';

@Component({
  selector: 'app-document-request-form',
  templateUrl: './supporting-document-request-form.component.html',
  styleUrls: ['./supporting-document-request-form.component.css']
})
export class CaseSupportingDocumentRequestFormComponent implements OnInit {
  columnsToDisplay = [
    'position',
    'name',
    'actions',
  ];

  hearingId: any;
  caseId: any;
  requestId: any;

  requestFull: any;
  oldRequest!: CaseSupportingDocumentRequest;

  documentRequest: CaseSupportingDocumentRequest = new CaseSupportingDocumentRequest();

  documentRequestItems!: MatTableDataSource<CaseSupportingDocumentRequestItem>;
  ministryDepartments: KeyValuePairs[] = [];

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  form: FormGroup = Object.create(null);
  requestLetterform: FormGroup = Object.create(null);
  public Editor = CustomEditor;
  public config = {
    language: 'ar',
  };

  private subs = new Subscription();

  @ViewChild(CaseSupportingDocumentRequestFormComponent, { static: true }) table: CaseSupportingDocumentRequestFormComponent | any;
  dataChanged: boolean = false;

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
    private letterTemplateService:LetterTemplateService,
    private requestLetterService:RequestLetterService

  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      this.hearingId = params.get('hearingId');
      this.caseId = params.get('caseId');
      ///
      this.requestId = params.get('requestId');
    });
  }

  ngOnInit() {
    this.init();
    this.populateMinistryDepartments();
    //this.populateLetterTemplates();
    ///
    if (this.requestId) {
      this.subs.add(
        this.requestService.getDocumentRequest(this.requestId).subscribe((result: any) => {
          this.requestFull = result.data;
          this.setDocumentRequest(result.data);
          this.populateForm(result.data);
          ////
          this.documentRequestItems = new MatTableDataSource(this.documentRequest.documents);
          ///
          this.oldRequest = Utilities.cloneObject(this.documentRequest);
        })
      );
    }
  }

  populateForm(documentRequest: CaseSupportingDocumentRequest) {
    this.form.patchValue({
      consigneeDepartmentId: documentRequest.consigneeDepartment.id,
     // note: documentRequest.request.note,
    });
  }

  /**
   * Component init.
   */
  private init(): void {

    this.form = this.fb.group({
      consigneeDepartmentId: ["", Validators.compose([Validators.required, Validators.maxLength(100)])],
     // note: ["", Validators.compose([Validators.required, Validators.maxLength(400)])],
    });

    this.requestLetterform=this.fb.group({
      id:[0],
      letterId:["", Validators.compose([Validators.required, Validators.maxLength(100)])],
      requestId:[this.requestId],
      text:[""]
    })
  }

  setDocumentRequest(documentRequest: CaseSupportingDocumentRequest) {
    this.documentRequest.id = documentRequest.id;
    this.documentRequest.hearingId = documentRequest.hearingId;
    this.documentRequest.parentId = documentRequest.parentId;
    this.documentRequest.caseId = documentRequest.caseId;
    //this.documentRequest.request.note = documentRequest.request.note;
    this.documentRequest.consigneeDepartmentId = documentRequest.consigneeDepartmentId;
    this.documentRequest.replyNote = documentRequest.replyNote;
    this.documentRequest.documents = documentRequest.documents;
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
  isNext:boolean=false;
  onSubmit() {
    if (this.documentRequest.documents.length == 0) {
      this.alert.error("الرجاء إضافة المستندات المطلوبة!");
      return;
    }
    //this.documentRequest.request.note = this.form.value.note;
    this.documentRequest.consigneeDepartmentId = this.form.value.consigneeDepartmentId;
    if (this.requestId) {
      if (!this.checkDataChanged()) {
        this.alert.error("لم تتم عمليه الحفظ. لا يوجد اي تغيير في بيانات الطلب!!");
        return;
      }

      this.subs.add(
        this.requestService.editDocumentRequest(this.documentRequest).subscribe(result => {
          this.alert.succuss("تم تعديل صيغة الطلب بنجاح");
          this.isNext=true;
          //this.router.navigateByUrl('/requests');
        }, error => { })
      );
    }
    // }
    else {
      this.documentRequest.hearingId = this.hearingId;
      this.documentRequest.caseId = +this.caseId;
      this.subs.add(
        this.requestService.createDocumentRequest(this.documentRequest).subscribe((result:any) => {
          this.alert.succuss("تم ارسال طلبك بنجاح");
          this.isNext=true;
          this.requestId=result.data.id;
          this.requestLetterform.controls['requestId'].setValue(result.data.id);
          this.populateLetterTemplates();
         }, error => { })
      );
    }
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
        /////
      }
    });
  }

  openDialog(name: any, Index: number) {
    let dialogRef = this.dialog.open(CaseSupportingDocumentRequestItemFormComponent, {
      width: '30em',
      data: { name: name, Index: Index }
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (result) => {
          if (result) {
            if (Index == -1) {
              let document: CaseSupportingDocumentRequestItem = {
                documentRequestId: 0,
                name: result,
              };
              this.documentRequest.documents.push(document);
            }
            else {
              this.documentRequest.documents[Index].name = result;
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

letterTemplates:any[]=[];
  populateLetterTemplates(){
    let letterTemplateQuery=new TemplateQueryObject({pageSize:9999,page:1,sortBy:"name"})
    this.subs.add(this.letterTemplateService.getWithQuery(letterTemplateQuery).subscribe((res:any)=>{

      this.letterTemplates=res.data.items;
    },(error)=>{

    }))
  }

  getContent(letterId:any)
  {
    this.subs.add(this.requestLetterService.getReplaceDocumentRequestContent(this.requestId,letterId).subscribe(
      (res:any)=>{
        this.requestLetterform.controls['text'].setValue(res.data);
      }
      ,(error)=>{

      }
    ))
  }

  onRequestLetterSubmit(){
    this.subs.add(this.requestLetterService.create(this.requestLetterform.value).subscribe(res=>{
      this.alert.succuss("تم الحفظ بنجاح");
      this.router.navigateByUrl('/requests');
    },(error)=>{

    }))
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
