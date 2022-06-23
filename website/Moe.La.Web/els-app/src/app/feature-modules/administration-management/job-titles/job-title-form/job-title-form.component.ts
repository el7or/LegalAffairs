import { Component, OnInit, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AlertService } from 'app/core/services/alert.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { JobTitleService } from 'app/core/services/job-title.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-job-title-form',
  templateUrl: './job-title-form.component.html',
  styleUrls: ['./job-title-form.component.css']
})
export class JobTitleFormComponent implements OnInit, AfterViewInit, OnDestroy {
  jobTitleId: number = 0;
  jobTitle: KeyValuePairs = new KeyValuePairs();

  public form: FormGroup = Object.create(null);
  private subs = new Subscription();

  constructor(private fb: FormBuilder,
    public jobTitleService: JobTitleService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<JobTitleFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    if (this.data.jobTitleId)
      this.jobTitleId = this.data.jobTitleId;
  }

  ngOnInit(): void {
    this.init();


    if (this.jobTitleId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.jobTitleService.get(this.jobTitleId).subscribe(
          (result: any) => {
            this.jobTitle = result.data;
            this.populateJobTitleForm(result.data);
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
  }
  ngAfterViewInit() {
    // Watch for name changes.
    this.form
      .get('name')
      .valueChanges.pipe(
        debounceTime(1000),
        map((value: string) => value.trim())
      )
      .subscribe((value: string) => {
        this.checkName(value);
      });
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
      name: [null, Validators.compose([Validators.required])]
    });
  }

  populateJobTitleForm(jobTitle: KeyValuePairs) {
    this.form.patchValue({
      id: jobTitle?.id,
      name: jobTitle?.name
    });
  }

  /**
   * Check whether the name field value is unique or not.
   * @param name The name value.
   */
  private checkName(name: string): void {
    const nameCtrl = this.form.get('name');
    if (nameCtrl.valid && this.jobTitle) {
      if (this.jobTitle.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.jobTitleService
          .isNameExists(name)
          .subscribe((result: any) => {
            if (result.data) {
              nameCtrl.setErrors({ uniqueName: true });
              nameCtrl.markAsTouched();
            } else {
              nameCtrl.updateValueAndValidity({ emitEvent: false });
            }
          });
      }
    }
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.jobTitleId
      ? this.jobTitleService.update(this.form.value)
      : this.jobTitleService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.jobTitleId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          let message = this.jobTitleId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }

}
