import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { InvestigationQuestionService } from 'app/core/services/investigation-question.service';
import { InvestigationQuestionFormComponent } from '../investigation-question-form/investigation-question-form.component';
import { InvestigationQuestionStatusFormComponent } from '../investigation-question-status-form/investigation-question-status-form.component';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';


@Component({
  selector: 'app-investigation-question-list',
  templateUrl: './investigation-question-list.component.html',
  styleUrls: ['./investigation-question-list.component.css']
})
export class InvestigationQuestionListComponent implements OnInit {

  displayedColumns: string[] = ['position', 'question', 'status', 'actions'];

  questions: any[] = [];
  allQuestions: any[] = [];

  searchText: string = '';
  showFilter: boolean = false;

  form: FormGroup = Object.create(null);

  queryResult: any = {
    totalItems: 0,
    items: [],
  };

  PAGE_SIZE: number = 20;

  queryObject: QueryObject = {
    sortBy: 'question',
    isSortAscending: true,
    page: 1,
    pageSize: this.PAGE_SIZE,
  };

  @ViewChild('template', { static: true }) template:
    | TemplateRef<any>
    | undefined;

  private subs = new Subscription();

  constructor(
    public investigationQuestionService: InvestigationQuestionService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      question: [],
    });
    this.populateInvestigationQuestions();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateInvestigationQuestions() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.investigationQuestionService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allQuestions = result.data.items;
          this.questions = this.allQuestions;
          this.loaderService.stopLoading()
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  onSearch() {
    if (this.searchText.length > 0)
      this.questions = this.allQuestions.filter(
        (item) =>
          item.question.includes(this.searchText) ||
          item.status.name.includes(this.searchText)
      );
    else this.questions = this.allQuestions;
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateInvestigationQuestions();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateInvestigationQuestions();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(InvestigationQuestionFormComponent, {
      width: '30em',
      data: { questionId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateInvestigationQuestions();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(questionId: number) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام عملية الحذف؟',
      icon: 'error',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'حذف',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.investigationQuestionService.delete(questionId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateInvestigationQuestions();
              this.resetControls();
            },
            (error) => {
              console.error(error);
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الحذف !');
            }
          )
        );
      }
    });
  }

  resetControls() {
    this.dialog.closeAll();
  }

  openChangeStatusModal(questionId: number, status: any) {
    const dialogRef = this.dialog.open(InvestigationQuestionStatusFormComponent, {
      width: '30em',
      data: { questionId: questionId, status: status },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateInvestigationQuestions();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }



}
