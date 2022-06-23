import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { ResearcherQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { RequestService } from 'app/core/services/request.service';
import { ResearcherConsultantService } from 'app/core/services/researcher-consultant.service';
import { UserService } from 'app/core/services/user.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-change-hearing-researcher-accept-form',
  templateUrl: './change-hearing-researcher-accept-form.component.html',
  styleUrls: ['./change-hearing-researcher-accept-form.component.css'],
})
export class ChangeHearingResearcherAcceptFormComponent implements OnInit, OnDestroy {
  researcherId: string | undefined;
  //newResearcherId: string | undefined;
  researchers: KeyValuePairs[] = [];

  form: FormGroup = Object.create(null);
  private subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private requestService: RequestService,
    private dialogRef: MatDialogRef<ChangeHearingResearcherAcceptFormComponent>,
    private alert: AlertService,
    private loaderService: LoaderService,
    public researcherConsultantService: ResearcherConsultantService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    if (this.data.researcherId) this.researcherId = this.data.researcherId;
    //if (this.data.newResearcherId) this.newResearcherId = this.data.newResearcherId;
  }

  ngOnInit(): void {
    this.init();
    this.form.controls['id'].setValue(this.data.requestId);
    this.form.controls['newResearcherId'].setValue(this.data.newResearcherId);

    this.populateResearchers();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  /**
 * Component init.
 */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      currentResearcherId: [this.researcherId],
      newResearcherId: ["", Validators.compose([Validators.required])],
      replyNote: [null, Validators.compose([Validators.required])],
    });
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
            .filter((m: any) => m.researcherId != this.researcherId) // we will remove the current resarcher from the list
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
    this.subs.add(
      this.requestService.acceptChangeResearcherToHearing(this.form.value).subscribe(result => {
        this.onCancel(this.form.controls['replyNote'].value);
      }, error => {
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }
}
