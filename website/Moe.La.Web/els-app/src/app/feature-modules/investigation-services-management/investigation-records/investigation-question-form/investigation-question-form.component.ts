import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { startWith, map } from 'rxjs/operators';
import { Observable, Subscription } from 'rxjs/Rx';

import { InvestigationRecordQuestion } from 'app/core/models/investigation-record';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { InvestigationQuestionService } from 'app/core/services/investigation-question.service';
import { UserService } from 'app/core/services/user.service';

@Component({
  selector: 'app-investigation-question-form',
  templateUrl: './investigation-question-form.component.html',
  styleUrls: ['./investigation-question-form.component.css']
})
export class InvestigationQuestionFormComponent implements OnInit {
  filteredQuestions$: Observable<any[]> = new Observable<any[]>();
  queryObject = new QueryObject({ pageSize: 99999 });
  questionForm: FormGroup = Object.create(null);
  private subs = new Subscription();
  questions: any[] = [];

  investigationQuestionsList: InvestigationRecordQuestion[] = [];

  @Input('questionsToUpdate') questionsToUpdate: any = [];

  @Input('partiesToUpdate') parties: any = [];


  @Output('questions-list') QuestionsList = new EventEmitter<any>(); // value will returns to $event variable

  // questions table
  displayedQuestionsColumns: string[] = [
    'question',
    'answer',
    'assignedTo',
    'actions',
  ];
  questionsDataSource!: MatTableDataSource<InvestigationRecordQuestion>;

  constructor(
    private activatedRouter: ActivatedRoute,
    private fb: FormBuilder,
    private alert: AlertService,
    public userService: UserService,
    public authService: AuthService,
    private investigationQuestionService: InvestigationQuestionService
  ) {

  }
  ngOnInit(): void {
    this.init();
    this.populateQuestions();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['questionsToUpdate']) {
      this.investigationQuestionsList = changes['questionsToUpdate'].currentValue;
      this.questionsDataSource = new MatTableDataSource(this.investigationQuestionsList);
    }

  }

  private init() {
    this.questionForm = this.fb.group({
      id: 0,
      question: [null],
      answer: [null],
      assignedTo: [""]
    });
  }
  private _filterQuestions(filterInput: string): any[] {
    return this.questions
      .filter((_question: any) => _question.question.toLowerCase().includes(filterInput));
  }

  populateQuestions() {
    this.subs.add(
      this.investigationQuestionService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.questions = result.data.items;
          this.filteredQuestions$ = this.questionForm.controls['question'].valueChanges.pipe(
            startWith(''),
            map((filterInput) => this._filterQuestions(filterInput))
          );
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );

  }

  addQuestion() {
    this.investigationQuestionsList.push({ id: 0, questionId: this.questionForm.value.question.id, question: this.questionForm.value.question.question ? this.questionForm.value.question.question : this.questionForm.value.question, answer: this.questionForm.value.answer, assignedTo: this.questionForm.value.assignedTo });
    this.questionsDataSource = new MatTableDataSource(this.investigationQuestionsList);
    this.QuestionsList.emit(this.investigationQuestionsList);

    this.questionForm.reset();
  }

  removeQuestion(index: number) {
    this.investigationQuestionsList.splice(index, 1);
    this.questionsDataSource = new MatTableDataSource(this.investigationQuestionsList);
    this.QuestionsList.emit(this.investigationQuestionsList);
  }

  displayQuestionName(question: any): string {
    return question?.question ? question.question : '';
  }

}
