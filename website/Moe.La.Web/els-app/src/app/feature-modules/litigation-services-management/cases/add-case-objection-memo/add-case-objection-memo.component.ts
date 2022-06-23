import { Component, OnInit, OnDestroy, Inject, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';

import { UserService } from 'app/core/services/user.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { MemoQueryObject } from 'app/core/models/query-objects';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { LegalMemoService } from 'app/core/services/legal-memo.service';
import { LegalMemoList } from 'app/core/models/legal-memo';
import { HearingLegalMemoReviewRequest } from 'app/core/models/hearing';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';
import { RequestService } from 'app/core/services/request.service';
import { SecondSubCategoryService } from 'app/core/services/second-sub-category.service';
import { LegalMemoTypes } from 'app/core/enums/LegalMemoTypes';
import { ObjectionMemo } from 'app/core/models/objection-legal-memo';

@Component({
  selector: 'app-add-case-objection-memo',
  templateUrl: './add-case-objection-memo.component.html',
  styleUrls: ['./add-case-objection-memo.component.css']
})
export class AddCaseObjectionMemoComponent implements OnInit {

  displayedColumns: string[] = [
    'position',
    'name',
    'type',
    'category',
    'actions',
  ];
  queryObject: MemoQueryObject = new MemoQueryObject({ sortBy: 'createdOn', status: [LegalMemoStatus.Approved], type: LegalMemoTypes.Objection });

  @ViewChild(MatSort) sort!: MatSort;
  dataSource!: MatTableDataSource<LegalMemoList>;
  totalItems!: number;

  form: FormGroup = Object.create(null);
  subs = new Subscription();
  hearingLegalMemo!: HearingLegalMemoReviewRequest;
  secondSubCategories: KeyValuePairs[] = [];
  caseId: any;
  searchText: string = '';
  searchForm: FormGroup = Object.create(null);
  caseCategoryId: any;
  legalMemoObjection: ObjectionMemo;

  get legalMemo() {
    return this.form.get('legalMemo');
  }
  constructor(
    private fb: FormBuilder,
    public userService: UserService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<AddCaseObjectionMemoComponent>,
    private loaderService: LoaderService,
    private memoService: LegalMemoService,
    private requestService: RequestService,
    public CaseCategoryService: SecondSubCategoryService,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.caseId = this.data.caseId;
    this.queryObject.secondSubCategoryId = this.data.secondSubCategoryId;
    this.queryObject.initialCaseId = this.data.caseId;
  }

  ngOnInit() {
    this.init();
    this.populateMemos();
  }

  init() {
    this.searchForm = this.fb.group({
      name: [],
    });
  }

  populateMemos() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.memoService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.totalItems = result.data.totalItems;
          this.dataSource = new MatTableDataSource(result.data.items);
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

  onFilter() {
    this.queryObject.name = this.searchForm.get('name')?.value;
    this.populateMemos()
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMemos()
  }

  onSubmit(legalMemoId: number) {
    this.legalMemoObjection = {
      caseId: this.caseId,
      legalMemoId: legalMemoId,
      id: 0,
      replyDate: null,
      replyNote: null,
    };

    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.requestService.addObjectionMemoRequest(this.legalMemoObjection).subscribe((res: any) => {
        this.loaderService.stopLoading();
        this.alert.succuss(' تم إنشاء طلب مراجعة إضافة مذكرة اعتراضة برقم ' + res.data.id);
        this.onCancel(res);
      },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية الإضافة !');
        }
      )
    );

  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
