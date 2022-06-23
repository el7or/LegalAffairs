import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AlertService } from 'app/core/services/alert.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { CaseRatingService } from 'app/core/services/case-rating.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-case-rating-form',
  templateUrl: './case-rating-form.component.html',
  styleUrls: ['./case-rating-form.component.css']
})
export class CaseRatingFormComponent implements OnInit {


  caseRatingId: number = 0;
  public form: FormGroup = Object.create(null);
  private subs = new Subscription();

  constructor(private fb: FormBuilder,
    public caseRatingService: CaseRatingService,
    public dialogRef: MatDialogRef<CaseRatingFormComponent>,
    private loaderService: LoaderService,
    private alert: AlertService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    if (this.data.caseRatingId)
      this.caseRatingId = this.data.caseRatingId;
  }

  ngOnInit(): void {
    this.init();

    if (this.caseRatingId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.caseRatingService.get(this.caseRatingId).subscribe(
          (result: any) => {
            this.populateCaseRatingForm(result.data);
            this.loaderService.stopLoading();
          },
          (error) => {
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
            console.error(error);
          }
        )
      );
    }
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
      name: [null, Validators.compose([Validators.required])],
    });
  }
  populateCaseRatingForm(caseRating: KeyValuePairs) {
    this.form.patchValue({
      id: caseRating?.id,
      name: caseRating?.name
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.caseRatingId
      ? this.caseRatingService.update(this.form.value)
      : this.caseRatingService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.caseRatingId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = this.caseRatingId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
          console.error(error);
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }


}
