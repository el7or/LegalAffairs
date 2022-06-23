import { Location } from '@angular/common';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { Component, OnDestroy, ViewChild, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { MainCaseDetails } from 'app/core/models/case';
import { CaseService } from 'app/core/services/case.service';
import { ActivatedRoute } from '@angular/router';
import { GroupNames } from 'app/core/models/attachment';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AlertService } from 'app/core/services/alert.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-create-case',
  templateUrl: './create-case.component.html',
  styleUrls: ['./create-case.component.css'],
  providers: [{
    provide: STEPPER_GLOBAL_OPTIONS, useValue: { displayDefaultIndicatorType: false }
  }]
})
export class CreateCaseComponent implements OnInit, OnDestroy {
  caseId: number = 0;
  moamalaId?: number;
  caseDetails!: MainCaseDetails;

  basicForm: FormGroup = Object.create(null);
  hearingForm: FormGroup = Object.create(null);
  partiesLength: number;
  attachments: any = [];
  attachmentsform: FormGroup = Object.create(null);
  //isMoamalatCompleted = false;
  isPartiesCompleted = false;
  isGroundsCompleted = false;
  @ViewChild('stepper') stepper;

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }

  private subs = new Subscription();

  constructor(public location: Location,
    private fb: FormBuilder,
    private caseService: CaseService,
    private route: ActivatedRoute,
    private loaderService: LoaderService,
    private alert: AlertService
  ) {
    this.route.queryParams.subscribe((params) => {
      this.moamalaId = params.moamalaId;
    });
    this.route.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.caseId = parseInt(id);
      }
    });
  }
  ngOnInit(): void {
    this.initBasicForm();
    this.initAttachmentsform();
    this.initHearingForm();
    if (this.caseId) {
      this.populateCase();
    }
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onAddCase(caseId) {
    this.caseId = caseId;
    if (this.caseId) {
      this.populateCase();
    }
  }

  populateCase() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.get(this.caseId).subscribe(
        (result: any) => {
          this.loaderService.stopLoading();
          this.caseDetails = result.data;
          if (this.caseDetails.caseParties.length > 0) {
            this.isPartiesCompleted = true;
            this.partiesLength = this.caseDetails?.caseParties?.length;
            //this.isMoamalatCompleted = true;
            this.isGroundsCompleted = true;
          }
          this.onCaseAttachment(this.caseDetails.attachments);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          this.alert.error("فشلت عملية جلب البيانات !");
        }
      )
    );
  }

  onStepChange(event: any): void {
    if (event.selectedIndex == 5) {
      this.populateCase();
    }
  }

  private initBasicForm(): void {
    this.basicForm = this.fb.group({
      id: [0],
      caseSource: [null, Validators.compose([Validators.required])],
      mainCategoryId: [null, Validators.compose([Validators.required])],
      firstSubCategoryId: [null, Validators.compose([Validators.required])],
      secondSubCategoryId: [null, Validators.compose([Validators.required])],
      caseNumberInSource: [null, Validators.compose([Validators.required])],
      litigationType: [null, Validators.compose([Validators.required])],
      legalStatus: [null, Validators.compose([Validators.required])],
      startDate: [null, Validators.compose([Validators.required])],
      courtId: [null, Validators.compose([Validators.required])],
      circleNumber: [null, Validators.compose([Validators.required])],
      subject: [null, Validators.compose([Validators.required])],
      caseDescription: [null, Validators.compose([Validators.required])],
      judgeName: [null],
      relatedCaseId: [null],
      relatedCaseNumber: [null],
      status: [CaseStatus.Draft],
      isManual: [true],
      notes: [null],
      moamalaId: [this.moamalaId]
    });
  }

  private initHearingForm(): void {
    this.hearingForm = this.fb.group({
      id: [0],
      caseId: [null, Validators.compose([Validators.required])],
      courtId: ["", Validators.compose([Validators.required])],
      litigationTypeId: [null],
      hearingNumber: [1, Validators.compose([Validators.required])],
      status: [{ value: 1, disabled: true }],
      statusAr: [{ value: null, disabled: true }],
      type: ["", Validators.compose([Validators.required])],
      hearingDate: [null, Validators.compose([Validators.required])],
      hearingTime: [null, Validators.compose([Validators.required])],
      circleNumber: [null],
      motions: [null, Validators.compose([Validators.required])],
      branchId: [null, Validators.compose([Validators.required])],
      attachments: [null]
    });
  }

  private initAttachmentsform() {
    this.attachmentsform = this.fb.group({
      attachments: [null]
    });
  }

  onCaseAttachment(list: any) {
    this.attachments = list;
    this.attachmentsform.controls['attachments'].setValue(this.attachments);
  }

  onSubmitAttachments(list: any) {
    this.loaderService.startLoading(LoaderComponent);
    this.onCaseAttachment(list);
    const data = {
      caseId: this.caseId,
      attachments: list
    };
    this.subs.add(
      this.caseService.updateAttachments(data).subscribe(
        () => {
          this.loaderService.stopLoading();
          this.populateCase();
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onSendToLitigationManager() {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام بيانات القضية لإرسالها إلى المرحلة التالية؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#28a745',
      confirmButtonText: 'إرسال',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.caseService.changeStatus(this.caseId, CaseStatus.ReceivedByLitigationManager).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تم إرسال القضية بنجاح');
              this.location.back();
            },
            (error) => {
              console.error(error);
              this.alert.error(error);
              this.loaderService.stopLoading();
            }
          )
        );
      }
    });
  }

  onAddParty(length: any) {
    this.partiesLength = length;
    if (!length) {
      this.isPartiesCompleted = false;
    }
  }

  onAddHearing(caseId) {
    this.populateCase();
  }
}
