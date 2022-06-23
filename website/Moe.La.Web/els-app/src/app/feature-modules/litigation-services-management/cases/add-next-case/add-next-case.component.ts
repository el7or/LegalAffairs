import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { SuggestedOpinon } from 'app/core/enums/SuggestedOpinon';
import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourtQueryObject } from 'app/core/models/query-objects';
import { CourtService } from 'app/core/services/court.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';

@Component({
  selector: 'app-add-next-case',
  templateUrl: './add-next-case.component.html',
  styleUrls: ['./add-next-case.component.css']
})
export class AddNextCaseComponent implements OnInit {
  private subs = new Subscription();
  caseId: any;
  case: any;
  form: FormGroup = Object.create(null);
  courts: any = [];

  public get SuggestedOpinon(): typeof SuggestedOpinon {
    return SuggestedOpinon;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  constructor(
    private dialogRef: MatDialogRef<AddNextCaseComponent>,
    private fb: FormBuilder,
    private courtService: CourtService,
    private caseService: CaseService,
    private loaderService: LoaderService,
    private alert: AlertService,
    private hijriConverter: HijriConverterService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.caseId = this.data.case?.id;
    this.case = this.data.case;

  }

  ngOnInit() {
    this.populateCourts(this.case?.caseSource?.id, this.case?.litigationType?.id);

    this.form = this.fb.group({
      caseNumberInSource: [null, Validators.compose([Validators.required])],
      startDate: [null, Validators.compose([Validators.required])],
      circleNumber: [null, Validators.compose([Validators.required])],
      courtId: [null, Validators.compose([Validators.required])],
      relatedCaseId: [this.case.id, Validators.compose([Validators.required])],
      caseSource: [this.case?.caseSource?.id]
    });


  }

  populateCourts(category, litigationType) {
    this.loaderService.startLoading(LoaderComponent);

    let queryObject: CourtQueryObject = new CourtQueryObject({
      pageSize: 999,
      courtCategory: category,
      litigationType: litigationType + 1
    });
    this.subs.add(
      this.courtService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.loaderService.stopLoading();

          this.courts = result.data.items;
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    this.form.get('startDate').setValue(this.hijriConverter.calendarDateToDate(
      this.form.get('startDate')?.value?.calendarStart
    ));

    this.subs.add(
      this.caseService.addNext(this.form.value).subscribe(
        (result: any) => {
          this.loaderService.stopLoading();
          this.alert.succuss('تمت عملية الحفظ بنجاح');
          this.dialogRef.close(result);
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية الحفظ !');
        }
      )
    );
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }
}

