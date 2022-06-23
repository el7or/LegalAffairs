import { Component, OnInit, OnDestroy, ChangeDetectorRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Subscription } from 'rxjs';

import { MemoQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AppRole } from 'app/core/models/role';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';
import { Department } from 'app/core/enums/Department';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { LegalMemoList } from 'app/core/models/legal-memo';
import { AuthService } from 'app/core/services/auth.service';
import { LegalMemoService } from 'app/core/services/legal-memo.service';

@Component({
  selector: 'app-legal-memo-review-list',
  templateUrl: './legal-memo-review-list.component.html',
  styleUrls: ['./legal-memo-review-list.component.css'],
})
export class LegalMemoReviewListComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'name',
    'createdOn',
    'creationTime',
    'createdByUser',
    'status',
    // 'updatedOn',
    'raisedOn',
    // 'updateTime',
    'actions',
  ];

  dataSource: MatTableDataSource<LegalMemoList> = new MatTableDataSource<LegalMemoList>();
  @ViewChild(MatSort) sort!: MatSort;
  legalMemoStatus: any;
  queryObject: MemoQueryObject = new MemoQueryObject({ isReview: true, pageSize: 999 });
  currentPage: number = 0;
  PAGE_SIZE: number = 999;
  raised: boolean;
  memosCreators: KeyValuePairs[] = [];
  searchForm: FormGroup = Object.create(null);
  private subs = new Subscription();
  public get LegalMemoStatus(): typeof LegalMemoStatus {
    return LegalMemoStatus;
  }
  isConsultant: boolean = this.authService.checkRole(AppRole.LegalConsultant, Department.Litigation);
  constructor(
    private legalMemoService: LegalMemoService,
    private router: Router,
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit() {
    this.raised = this.authService.currentUser?.roles.includes(AppRole.SubBoardHead) || this.authService.currentUser?.roles.includes(AppRole.LegalConsultant)
    this.init();
    this.populateMemos();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
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
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  init() {
    this.searchForm = this.fb.group({
      searchText: [''],
    });
  }

  populateMemos() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalMemoService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.dataSource = new MatTableDataSource(result.data.items);
          this.applyFilter();
          this.memosCreators = result.data.items.map((m: LegalMemoList) => {
            return m.createdByUser;
          });
          this.loaderService.stopLoading();
          this.memosCreators = Array.from(
            this.memosCreators
              .reduce((m, t) => m.set(t.name, t), new Map())
              .values()
          );
          this.queryObject = new MemoQueryObject({ isReview: true, pageSize: 999 });
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  readLegalMemo(legalMemoId: number) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(this.legalMemoService.readLegalMemo(legalMemoId)
      .subscribe((res: any) => {
        this.alert.succuss("تمت فتح المذكرة للمراجعة بنجاح");
        this.router.navigateByUrl('/memos/review/' + legalMemoId);
        this.loaderService.stopLoading();
      }, (error) => {
        this.alert.error("فشلت عملية فتح المذكرة للمراجعة");
        this.loaderService.stopLoading();
        console.error(error);
      }))
  }

  applyFilter(page: number = 0) {
    this.currentPage = page;
    let searchText = this.searchForm.controls['searchText'].value.trim().toLowerCase()
    this.dataSource.filteredData = this.dataSource.data.filter(m => m.name.includes(searchText)
      || m.status.name.includes(searchText)
      || m.createdByUser.name.includes(searchText)
      || m.createdOn.includes(searchText)
      || m.updatedOn.includes(searchText)
    );
  }

  onPageChange(page: number) {
    this.applyFilter(page);
  }

  public get AppRole(): typeof AppRole {
    return AppRole;
  }

}
