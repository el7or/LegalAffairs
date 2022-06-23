import { Component, OnInit, OnDestroy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';

import { MemoQueryObject, QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';
import { AppRole } from 'app/core/models/role';
import { DeleteLegalMemoComponent } from '../delete-legal-memo/delete-legal-memo.component';
import { SecondSubCategoryService } from 'app/core/services/second-sub-category.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { LegalMemoList } from 'app/core/models/legal-memo';
import { AuthService } from 'app/core/services/auth.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { LegalMemoService } from 'app/core/services/legal-memo.service';

@Component({
  selector: 'app-legal-memo-list',
  templateUrl: './legal-memo-list.component.html',
  styleUrls: ['./legal-memo-list.component.css'],
})
export class LegalMemoListComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'name',
    'type',
    'status',
    'updatedOn',
    'secondSubCategory',
    'createdBy',
    'actions',
  ];

  legalMemoStatus: any;
  totalItems!: number;
  queryObject: MemoQueryObject = new MemoQueryObject();
  searchText: string = '';
  showFilter: boolean = false;
  isNewStatus: boolean = false;
  secondSubCategories: any = [];
  memosCreators: KeyValuePairs[] = [];
  searchForm: FormGroup = Object.create(null);

  @ViewChild(MatSort) sort!: MatSort;
  dataSource!: MatTableDataSource<LegalMemoList>;

  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher);

  public get LegalMemoStatus(): typeof LegalMemoStatus {
    return LegalMemoStatus;
  }
  private subs = new Subscription();


  constructor(
    private memoService: LegalMemoService,
    private caseCategoryService: SecondSubCategoryService,
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private hijriConverter: HijriConverterService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.init();
    this.populateMemos();
    this.populateSecondSubCategories();
    this.populateLegalMemoStatus();
  }
  init() {
    this.searchForm = this.fb.group({
      name: [],
      secondSubCategoryId: [],
      status: [0],
      createdBy: [""],
      approvalFrom: [],
      approvalTo: [],
    });
  }
  ngAfterViewInit() {
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateMemos();
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
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
          this.memosCreators = result.data.items.map((m: LegalMemoList) => {
            return m.createdByUser;
          });
          this.loaderService.stopLoading();
          this.memosCreators = Array.from(
            this.memosCreators
              .reduce((m, t) => m.set(t.name, t), new Map())
              .values()
          );
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
          console.error(error);
        }
      )
    );
  }

  populateLegalMemoStatus() {
    if (!this.legalMemoStatus) {
      this.subs.add(
        this.memoService.getLegalMemoStatus().subscribe(
          (result: any) => {
            this.loaderService.stopLoading();
            this.legalMemoStatus = result;
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

  populateSecondSubCategories() {

    let queryObject: QueryObject = new QueryObject({ pageSize: 999 });
    this.subs.add(
      this.caseCategoryService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.secondSubCategories = result.data.items;
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
    this.queryObject.status = [];
    this.queryObject.name = this.searchForm.get('name')?.value;
    this.queryObject.secondSubCategoryId = this.searchForm.get('secondSubCategoryId')?.value;
    this.queryObject.status = this.searchForm.get('status')?.value;
    //this.queryObject.status.push(this.searchForm.get('status')?.value);
    // convert approval from date from hijri to miladi
    this.queryObject.approvalFrom = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('approvalFrom')?.value?.calendarStart
    );
    // convert approval to date hijri to miladi
    this.queryObject.approvalTo = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('approvalTo')?.value?.calendarStart
    );

    this.populateMemos()
  }

  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateMemos()
  }


  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMemos()
  }

  advancedFilter() {
    this.showFilter = !this.showFilter;

    if (!this.showFilter) {

      this.queryObject = new MemoQueryObject();
      this.populateMemos()
      this.searchForm.reset();
    }
  }

  checkStatus() {
    var newSelected = this.searchForm.get('status')?.value == LegalMemoStatus.New;
    if (newSelected) {
      this.searchForm.controls['createdBy'].setValue(this.authService.currentUser?.id);
      this.isNewStatus = true;
    }
  }

  onDelete(id: number) {
    let dialogRef = this.dialog.open(DeleteLegalMemoComponent, {
      width: '30em',
      data: { memoId: id }
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (result) => {
          if (result) {
            this.populateMemos();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
