import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';

import { RequestService } from 'app/core/services/request.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { ResearcherQueryObject } from 'app/core/models/query-objects';
import { ResearcherConsultantService } from 'app/core/services/researcher-consultant.service';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-change-hearing-researcher-request-form',
  templateUrl: './change-hearing-researcher-request-form.component.html',
  styleUrls: ['./change-hearing-researcher-request-form.component.css']
})
export class ChangeHearingResearcherRequestFormComponent implements OnInit, OnDestroy {

  hearingId: number = 0;
  noteMaxValue: 225;
  researchers: KeyValuePairs[] = [];
  form: FormGroup = Object.create(null);
  private subs = new Subscription();

  constructor(
    private requestService: RequestService,
    private researcherConsultantService: ResearcherConsultantService,
    private authService: AuthService,
    private activatedRouter: ActivatedRoute,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    public location: Location,
    private router: Router) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var hearingId = params.get('hearingId');
      if (hearingId != null) {
        this.hearingId = +hearingId;
      }

    });
  }

  ngOnInit() {

    this.form = this.fb.group({
      id: [0],
      hearingId: [this.hearingId],
      note: [null, [Validators.required, Validators.maxLength(255)]],
      newResearcherId: ["", Validators.compose([Validators.required])],
    });

    this.populateResearchers();
  }

  populateResearchers() {
    let queryObject: ResearcherQueryObject = new ResearcherQueryObject({
      sortBy: 'researcher',
      pageSize: 999,
      hasConsultant: true
    });
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      this.researcherConsultantService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.researchers = result.data.items
            .filter((m: any) => m.researcherId != this.authService.currentUser?.id) // we will remove the current resarcher from the list
            .map((researcher: any) => {
              return {
                id: researcher.researcherId,
                name: researcher.researcher
              };
            });

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

    var result$ = this.requestService.createChangeResearcherToHearingRequest(this.form.value);
    this.subs.add(
      result$.subscribe((result: any) => {
        this.loaderService.stopLoading();
        let message = 'تم تقديم الطلب برقم ' + result.data.id + ' بنجاح ';
        this.alert.succuss(message);

        this.location.back();

        //this.router.navigateByUrl('/hearings/view/' + this.hearingId);
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


