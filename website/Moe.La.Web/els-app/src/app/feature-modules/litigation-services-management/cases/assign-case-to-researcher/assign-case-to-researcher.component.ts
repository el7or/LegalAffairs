import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { ResearcherQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { LoaderService } from 'app/core/services/loader.service';
import { ResearcherConsultantService } from 'app/core/services/researcher-consultant.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-assign-case-to-researcher',
  templateUrl: './assign-case-to-researcher.component.html',
  styleUrls: ['./assign-case-to-researcher.component.css']
})
export class AssignCaseToResearcherComponent implements OnInit {

  caseId: number = 0;
  researchers: KeyValuePairs<string>[] = [];

  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    private caseService: CaseService,
    public researcherConsultantService: ResearcherConsultantService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<AssignCaseToResearcherComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {

    if (this.data.caseId)
      this.caseId = this.data.caseId;
  }

  ngOnInit() {
    this.init();
    this.populateResearchers();
  }

  populateResearchers() {
    let queryObject: ResearcherQueryObject = new ResearcherQueryObject( {
      sortBy: 'researcher',
      pageSize: 999,
      hasConsultant: true
    });
    this.loaderService.startLoading(LoaderComponent);

    this.subs.add(
      this.researcherConsultantService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.researchers = result.data.items.map( (researcher:any)=>{ return {
              id:researcher.researcherId,
              name:researcher.researcher
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

  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      researcherId: ["", Validators.compose([Validators.required])],
      notes: [null],
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSubmit() {

    this.loaderService.startLoading(LoaderComponent);

    let researcherId = this.form.controls["researcherId"].value;
    let notes = this.form.controls["notes"].value;

    this.subs.add(
      this.caseService
        .assignToResearcher(this.caseId, researcherId, notes)
        .subscribe(
          () => {
            this.dialogRef.close('succuss');
            this.loaderService.stopLoading();
            this.alert.succuss('تم اختيار الباحث بنجاح');
          },
          (error) => {
            this.dialogRef.close(null);
            this.loaderService.stopLoading();
            this.alert.error('فشلت عملية اختيار الباحث');
            console.error(error);
          }
        )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
