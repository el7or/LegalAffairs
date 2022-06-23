import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestService } from 'app/core/services/request.service';
import { RequestTypes } from 'app/core/enums/RequestTypes';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { SendingTypes } from 'app/core/enums/SendingTypes';
import { TemplateImageComponent } from '../supporting-document-request-wizard/template-image/template-image.component';
import { MatDialog } from '@angular/material/dialog';
import { RequestLetterService } from 'app/core/services/request-letter.service';
import { TemplateQueryObject } from 'app/core/models/query-objects';
import { LetterTemplateService } from 'app/core/services/letter-template.service';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';

import { LetterTemplateTypes } from 'app/core/enums/LetterTemplateTypes';
import { RequestLetter } from 'app/core/models/supporting-document-request';
import { LetterTemplate } from 'app/core/models/letterTemplate';
import { Constants } from 'app/core/constants';
import { AttachedLetterRequest } from 'app/core/models/attached-letter-request';

@Component({
  selector: 'app-attached-letter-request-form',
  templateUrl: './attached-letter-request-form.component.html',
  styleUrls: ['./attached-letter-request-form.component.css'],
})
export class AttachedLetterRequestFormComponent implements OnInit, OnDestroy {
  form: FormGroup = Object.create(null);
  parentRequest: any;
  parentId: number = 0;
  requestId: number = 0;
  letterTemplates: LetterTemplate[] = [];

  attachedLetterRequest: AttachedLetterRequest = {
    id: 0,
    hearingId: 0,
    parentId: 0,
    caseId: 0,
    request: {
      id: 0,
      requestType: RequestTypes.RequestAttachedLetter,
      requestStatus: RequestStatus.New,
      sendingType: SendingTypes.User,
      letter: new RequestLetter()
    },
  };

  baseUrl = Constants.BASE_URL;
  public Editor = CustomEditor;
  public config = {
    language: 'ar',
  };
  private subs = new Subscription();

  constructor(
    private activatedRouter: ActivatedRoute,
    private requestService: RequestService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private dialog: MatDialog,
    private requestLetterService: RequestLetterService,
    private letterTemplateService: LetterTemplateService,
    public location: Location
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var PId = params.get('parentId');
      var reqId = params.get('id');

      if (PId) {
        this.parentId = +PId;
      }
      if (reqId) {
        this.requestId = +reqId;
      }
    });
  }

  ngOnInit() {
    this.initForm();
    this.populateLetterTemplates();

    // add new attached letter request
    if (this.parentId) {
      this.populateParentRequestDetails(this.parentId);
    }

    // update attached letter request
    if (this.requestId) {
      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.requestService.getAttachedLetterRequest(this.requestId).subscribe(
          (result: any) => {
            if (result.data != null) {

              this.attachedLetterRequest = result.data;

              this.parentId = result.data.parentId;
              this.populateParentRequestDetails(this.parentId);

              this.attachedLetterRequest.request.requestStatus =
                RequestStatus.Modified;
              this.populateForm(this.attachedLetterRequest);
              this.loaderService.stopLoading();
            }
          },
          (error) => {
            console.error(error);
            this.loaderService.stopLoading();
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }

  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  initForm() {
    this.form = this.fb.group({
      text: [null, Validators.compose([Validators.required])],
    });
  }

  populateParentRequestDetails(parentId: number) {
    this.subs.add(
      this.requestService.getDocumentRequest(parentId).subscribe(
        (result: any) => {
          if (result.data) {
            this.parentRequest = result.data;
            this.attachedLetterRequest.caseId = this.parentRequest.caseId;
            this.attachedLetterRequest.hearingId = this.parentRequest.hearingId;
            this.attachedLetterRequest.consigneeDepartmentId = this.parentRequest.consigneeDepartment.id;
            this.attachedLetterRequest.parentId = this.parentRequest.request.id;

          }
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
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
      type: LetterTemplateTypes.AttachedLetter,
      name: ''
    };
    this.subs.add(this.letterTemplateService.getWithQuery(queryObject).subscribe((res: any) => {
      this.letterTemplates = res.data.items;
    }, (error) => {
      console.error(error);
      this.alert.error('فشل تحميل قائمة النماذج !');
    }))
  }

  populateForm(attachedLetterRequest: AttachedLetterRequest) {

    this.form.patchValue({
      text: attachedLetterRequest.request.letter.text,
    });
  }

  openImageDialog(name: any, image: any) {
    this.dialog.open(TemplateImageComponent, {
      width: '600px',
      data: { name: name, image: image }
    });
  }

  getContent(letterId: any) {
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(this.requestLetterService.getReplaceDocumentRequestContent(this.parentId, letterId).subscribe(
      (res: any) => {
        this.loaderService.stopLoading();

        this.form.controls['text'].setValue(res.data);
      }
      , () => {
        this.loaderService.stopLoading();

      }
    ));
  }

  onSubmit() {
    if (this.attachedLetterRequest.request.letter.text == this.form.value.text) {
      this.alert.error("لم تتم عمليه الحفظ. لا يوجد اي تغيير في بيانات الطلب!!");
      return;
    }
    this.loaderService.startLoading(LoaderComponent);

    this.attachedLetterRequest.request.letter.text = this.form.value.text;
    this.attachedLetterRequest.request.letter.requestId = this.requestId;
 
    if (!this.requestId) {
      this.subs.add(
        this.requestService
          .createAttachedLetterRequest(this.attachedLetterRequest)
          .subscribe(
            (result: any) => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الإضافة بنجاح');
              this.location.back();
            },
            (error) => {
              console.error(error);
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الإضافة !');
            }
          )
      );
    } else {
      this.subs.add(
        this.requestService
          .editAttachedLetterRequest(this.attachedLetterRequest)
          .subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية إعادة صياغة الطلب الإلحاقى بنجاح');
              this.location.back();
            },
            (error) => {
              console.error(error);
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الإضافة !');
            }
          )
      );
    }
  }
}
