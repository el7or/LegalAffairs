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

@Component({
  selector: 'app-hearing-legal-memo-form',
  templateUrl: './hearing-legal-memo-form.component.html',
  styleUrls: ['./hearing-legal-memo-form.component.css'],
})
export class HearingLegalMemoFormComponent implements OnInit, OnDestroy {


  displayedColumns: string[] = [
    'position',
    'name',
    'actions',
  ];
  queryObject: MemoQueryObject = new MemoQueryObject({ sortBy: 'createdOn', status: [LegalMemoStatus.Approved], type: LegalMemoTypes.Pleading });

  @ViewChild(MatSort) sort!: MatSort;
  dataSource!: MatTableDataSource<LegalMemoList>;
  totalItems!: number;

  form: FormGroup = Object.create(null);
  subs = new Subscription();
  hearingLegalMemo!: HearingLegalMemoReviewRequest;
  secondSubCategories: KeyValuePairs[] = [];
  hearingId: any;
  searchText: string = '';
  searchForm: FormGroup = Object.create(null);
  caseCategoryId: any;
  secondSubCategoryId: any;

  get legalMemo() {
    return this.form.get('legalMemo');
  }
  constructor(
    private fb: FormBuilder,
    public userService: UserService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<HearingLegalMemoFormComponent>,
    private loaderService: LoaderService,
    private memoService: LegalMemoService,
    private requestService: RequestService,
    public CaseCategoryService: SecondSubCategoryService,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.hearingId = this.data.id;
    if (this.data.secondSubCategoryId) {
      this.queryObject.secondSubCategoryId = this.data.secondSubCategoryId;
      this.secondSubCategoryId = this.data.secondSubCategoryId;
    }
  }

  ngOnInit() {
    this.init();
    this.populateMemos();
    this.populateSecondSubCategories();
  }



  init() {
    this.searchForm = this.fb.group({
      name: [],
      secondSubCategoryId: [{ value: this.secondSubCategoryId, disabled: this.secondSubCategoryId }],
    });
  }
  populateSecondSubCategories() {
    this.subs.add(
      this.CaseCategoryService
        .getWithQuery(this.queryObject)
        .subscribe(
          (result: any) => {
            this.secondSubCategories = result.data.items;
          },
          (error: any) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
    );
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
    this.queryObject.secondSubCategoryId = this.searchForm.get('secondSubCategoryId')?.value;

    this.populateMemos()
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMemos()
  }

  onSubmit(legalMemoId: number) {
    this.hearingLegalMemo = {
      HearingId: this.hearingId,
      LegalMemoId: legalMemoId
    };

    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.requestService.addLegalMemo(this.hearingLegalMemo).subscribe((res: any) => {
        this.loaderService.stopLoading();
        this.alert.succuss(' تم إنشاء طلب مراجعة إضافة مذكرة لجلسة برقم ' + res.data.id);
        this.onCancel("updated");
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
