import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs/Rx';

import { AlertService } from 'app/core/services/alert.service';
import { InvestigationQuestionService } from 'app/core/services/investigation-question.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-investigation-question-status-form',
  templateUrl: './investigation-question-status-form.component.html',
  styleUrls: ['./investigation-question-status-form.component.css']
})
export class InvestigationQuestionStatusFormComponent implements OnInit {

  private subs = new Subscription();
  questionsStatuses: any[] = [];
  questionId: any;
  question: any;
  status: any;

  constructor(
    public investigationQuestionService: InvestigationQuestionService,
    private fb: FormBuilder,
    private alert: AlertService,
    private loaderService: LoaderService,
    private dialogRef: MatDialogRef<InvestigationQuestionStatusFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.questionId) this.questionId = this.data.questionId;
    if (this.data.status) this.status = this.data.status;

  }
  form: FormGroup = Object.create(null);

  ngOnInit(): void {
    this.init();
    this.populateQuestionsStatuses();
  }

  private init(): void {
    this.form = this.fb.group({
      id: [this.questionId],
      status: [this.status.id],
    });
  }

  populateQuestionsStatuses() {
    this.subs.add(
      this.investigationQuestionService.GetQuestionsStatuses().subscribe(
        (result: any) => {
          this.questionsStatuses = result;
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
    this.subs.add(
      this.investigationQuestionService.changeStatus(this.questionId, this.form.value.status).subscribe(result => {
        this.alert.succuss("تم تصنيف السؤال بنجاح");
        this.loaderService.stopLoading();
        this.onCancel(result);
      }, error => {
        this.loaderService.stopLoading();
        this.alert.error("فشلت عملية تصنيف السؤال ");
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

}

