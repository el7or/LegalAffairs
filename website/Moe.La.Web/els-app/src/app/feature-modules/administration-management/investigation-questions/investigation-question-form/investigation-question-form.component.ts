import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { InvestigationQuestionService } from 'app/core/services/investigation-question.service'
import { AfterViewInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { debounceTime, map } from 'rxjs/operators';


@Component({
  selector: 'app-investigation-question-form',
  templateUrl: './investigation-question-form.component.html',
  styleUrls: ['./investigation-question-form.component.css']
})
export class InvestigationQuestionFormComponent implements OnInit, AfterViewInit, OnDestroy {

  questionId: number = 0;
  question: any = null;

  public form: FormGroup = Object.create(null);
  private subs = new Subscription();

  constructor(private fb: FormBuilder,
    public InvestigationQuestionService: InvestigationQuestionService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<InvestigationQuestionFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    if (this.data.questionId)
      this.questionId = this.data.questionId;
  }

  ngOnInit(): void {
    this.init();


    if (this.questionId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.InvestigationQuestionService.get(this.questionId).subscribe(
          (result: any) => {
            this.InvestigationQuestionService = result.dat;
            this.populateInvestigationQuestionForm(result.data);
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

  private init(): void {
    this.form = this.fb.group({
      id: [0],
      question: [null, Validators.compose([Validators.required])]
    });
  }

  populateInvestigationQuestionForm(investigationQuestion: any) {
    this.form.patchValue({
      id: investigationQuestion.id,
      question: investigationQuestion.question
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    // this.form.controls['question'].setValue(this.form.controls['question'].value+"؟");
    let result$ = this.questionId
      ? this.InvestigationQuestionService.update(this.form.value)
      : this.InvestigationQuestionService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.questionId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = this.questionId
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
  /**
     * Check whether the name field value is unique or not.
     * @param name The name value.
     */
  private checkName(name: string): void {
    const nameCtrl = this.form.get('name');
    if (nameCtrl.valid && this.question) {
      if (this.question.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.InvestigationQuestionService
          .isNameExists(this.form.value)
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

}
