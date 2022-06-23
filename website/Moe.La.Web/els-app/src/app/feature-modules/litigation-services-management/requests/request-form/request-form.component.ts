import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';

import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { LegalMemoService } from 'app/core/services/legal-memo.service';

@Component({
  selector: 'app-request-form',
  templateUrl: './request-form.component.html',
  styleUrls: ['./request-form.component.css']
})
export class RequestFormComponent implements OnInit {

  caseId: number = 0;
  memoId: number = 0;
  noteMaxValue: 225;
  memoDetails: any;
  requestTypes: any;

  form: FormGroup = Object.create(null);
  private subs = new Subscription();

  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }

  constructor(
    private requestService: RequestService,
    private legalMemoService: LegalMemoService,
    private activatedRouter: ActivatedRoute,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    public location: Location,
    private router: ActivatedRoute) {
    this.activatedRouter.paramMap.subscribe((params) => {

      this.caseId = +this.router.snapshot.queryParamMap.get('caseId');

      this.memoId = +this.router.snapshot.queryParamMap.get('memoId');


    });
  }

  ngOnInit() {

    this.form = this.fb.group({
      id: [0],
      caseId: [this.caseId],
      note: [null, [Validators.required, Validators.maxLength(255)]],
      legalMemoId: [null],
      currentResearcherId: [null],
      // requestStatus: [RequestStatus.New],
      // requestType: [RequestTypes.RequestResearcherChange],
    });

    this.populateMemoDetails();

    //this.populateRequestTypes();

    if (this.memoId)//new request
      this.form.controls['legalMemoId'].setValue(this.memoId);

  }

  populateMemoDetails() {
    if (this.memoId) {
      this.subs.add(
        this.legalMemoService.get(this.memoId).subscribe(
          (result: any) => {
            this.memoDetails = result.data;

            this.form.patchValue({
              currentResearcherId: result.data.createdByUser.id,
              caseId: result.data.initialCaseId,
            });

            this.loaderService.stopLoading();
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }));
    }
  }

  onSubmit() {

    this.loaderService.startLoading(LoaderComponent);

    var result$ = this.requestService.changeResearcherRequest(this.form.value);
    this.subs.add(
      result$.subscribe((result: any) => {
        this.loaderService.stopLoading();
        let message = 'تم تقديم الطلب برقم ' + result.data.id + ' بنجاح ';
        this.alert.succuss(message);

        this.location.back();

        // if (this.memoId != 0)
        //   this.router.navigateByUrl('/memos/board-review-list');
        // else if (this.caseId != 0)
        //   this.router.navigateByUrl('/cases/view/' + this.caseId);
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

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}


