import { Location } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { UserService } from 'app/core/services/user.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { RequestService } from 'app/core/services/request.service';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { Utilities } from 'app/shared/utilities';
import { ConsultationSupportingDocument } from 'app/core/models/consultation-request';
import { RequestTypes } from 'app/core/enums/RequestTypes';

@Component({
  selector: 'app-consultation-request-form',
  templateUrl: './consultation-request-form.component.html',
  styleUrls: ['./consultation-request-form.component.css']
})
export class ConsultationSupportingDocumentFormComponent implements OnInit {


  requestId: any;

  requestFull: any;
  oldRequest!: ConsultationSupportingDocument;

  consultationSupportingDocument: ConsultationSupportingDocument = new ConsultationSupportingDocument();
  consultationId: string;
  moamalaId: string;


  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  form: FormGroup = Object.create(null);

  // public Editor = ClassicEditor;
  // public config = {
  //   language: 'ar',
  // };

  private subs = new Subscription();

  @ViewChild(ConsultationSupportingDocumentFormComponent, { static: true }) table: ConsultationSupportingDocumentFormComponent | any;
  dataChanged: boolean = false;

  constructor(
    private activatedRouter: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private alert: AlertService,
    public userService: UserService,
    public location: Location,
    public authService: AuthService,
    private requestService: RequestService

  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      this.requestId = params.get('id');
      this.consultationSupportingDocument.consultationId = +params.get('consultationId');
      this.consultationSupportingDocument.moamalaId = +params.get('moamalaId');

    });
  }

  ngOnInit() {
    this.init();
    ///
    if (this.requestId) {
      this.subs.add(
        this.requestService.getConsultationSupportingDocument(this.requestId).subscribe((result: any) => {
          this.requestFull = result.data;
          this.setConsultationSupportingDocument(result.data);
          this.populateForm(result.data);
          ///
          this.oldRequest = Utilities.cloneObject(this.consultationSupportingDocument);
        })
      );
    }
  }

  populateForm(ConsultationSupportingDocument: ConsultationSupportingDocument) {
    this.form.patchValue({
      note: ConsultationSupportingDocument.request.note,
    });
  }

  /**
   * Component init.
   */
  private init(): void {

    this.form = this.fb.group({
      note: [null, Validators.compose([Validators.required, Validators.maxLength(400)])],
    });
  }

  setConsultationSupportingDocument(ConsultationSupportingDocument: ConsultationSupportingDocument) {
    this.consultationSupportingDocument.requestId = ConsultationSupportingDocument.requestId;
    this.consultationSupportingDocument.consultationId = ConsultationSupportingDocument.consultationId;
    this.consultationSupportingDocument.moamalaId = ConsultationSupportingDocument.moamalaId;
  }


  checkDataChanged() {
    return JSON.stringify(this.consultationSupportingDocument).toLowerCase() != JSON.stringify(this.oldRequest).toLowerCase();
  }

  onSubmit() {
    this.consultationSupportingDocument.request.note = this.form.value.note;
    this.consultationSupportingDocument.request.requestType = RequestTypes.ConsultationSupportingDocument;
    if (this.requestId) {
      if (!this.checkDataChanged()) {
        this.alert.error("لم تتم عمليه الحفظ. لا يوجد اي تغيير في بيانات الطلب!!");
        return;
      }

      this.subs.add(
        this.requestService.editConsultationSupportingDocument(this.consultationSupportingDocument).subscribe(result => {
          this.alert.succuss("تم تعديل صيغة الطلب بنجاح");
          this.router.navigateByUrl('/requests');
        }, error => { })
      );
    }
    // }
    else {
      this.subs.add(
        this.requestService.createConsultationSupportingDocument(this.consultationSupportingDocument).subscribe(result => {
          this.alert.succuss("تم ارسال طلبك بنجاح");
          this.router.navigateByUrl('/requests');
        }, error => { })
      );
    }
  }



  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
