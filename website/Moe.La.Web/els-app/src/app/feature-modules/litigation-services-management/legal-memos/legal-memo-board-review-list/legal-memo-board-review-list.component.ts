import { Component, OnInit, ViewChild, TemplateRef, ChangeDetectorRef } from '@angular/core';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import Swal from 'sweetalert2';

import { LegalMemoService } from 'app/core/services/legal-memo.service';
import { AuthService } from 'app/core/services/auth.service';
import { AlertService } from 'app/core/services/alert.service';
import { LegalMemoList } from 'app/core/models/legal-memo';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { MemoQueryObject } from 'app/core/models/query-objects';
import { LoaderService } from 'app/core/services/loader.service';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { AssignMemoToBoardFormComponent } from '../assign-memo-to-board-form/assign-memo-to-board-form.component';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';
import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';

@Component({
  selector: 'app-legal-memo-board-review-list',
  templateUrl: './legal-memo-board-review-list.component.html',
  styleUrls: ['./legal-memo-board-review-list.component.css']
})
export class LegalMemoBoardReviewListComponent implements OnInit {
  displayedColumns: string[] = [
    'position',
    'id',
    'name',
    'status',
    'updatedOn',
    'secondSubCategory',
    'actions',
  ];
  legalMemoStatus: any;
  dataSource: MatTableDataSource<LegalMemoList> = new MatTableDataSource<LegalMemoList>();
  @ViewChild(MatSort) sort!: MatSort;
  currentPage: number = 0;
  PAGE_SIZE: number = 20;
  queryObject: MemoQueryObject = new MemoQueryObject();

  memos: LegalMemoList[] = [];

  memosCreators: KeyValuePairs[] = [];
  public get LegalMemoStatus(): typeof LegalMemoStatus {
    return LegalMemoStatus;
  }
  private subs = new Subscription();
  @ViewChild('template', { static: true }) template:
    | TemplateRef<any>
    | undefined;
  searchForm: FormGroup = Object.create(null);
  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);
  constructor(
    private legalMemoService: LegalMemoService,
    private boardService: LegalBoardService,
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private router: Router,
    private cdr: ChangeDetectorRef,
  ) {
  }
  ngOnInit() {
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
          this.queryObject.isBoardReview = true;
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
    this.queryObject.status = [LegalMemoStatus.Rejected , LegalMemoStatus.RaisingMainBoardHead ];
    this.queryObject.isBoardReview = true;
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalMemoService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
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
          this.queryObject = new MemoQueryObject();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  applyFilter(page: number = 0) {
    this.currentPage = page;
    let searchText = this.searchForm.controls['searchText'].value.trim().toLowerCase()
    this.dataSource.filteredData = this.dataSource.data.filter(m => m.name.includes(searchText)
      || m.status?.name.includes(searchText)
      || m.updatedOn.includes(searchText)
      || m.secondSubCategory.includes(searchText)

    );
  }

  onPageChange(page: number) {
    this.applyFilter(page);
  }

  changeLgalMemoStatus(legalMemoId: number, legalMemoStatusId: number) {

    this.subs.add(this.legalMemoService.changeLegalMemoStatus(legalMemoId, legalMemoStatusId)
      .subscribe((res: any) => {
        this.alert.succuss("تمت إحالة المذكرة إلى اللجنة  الرئيسية.");
        this.populateMemos();

      }, (error) => {
        this.alert.error("فشلت عملية التغيير");
        this.loaderService.stopLoading();
        console.error(error);
      }))
  }
  readLegalMemo(legalMemoId: number) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(this.legalMemoService.readLegalMemo(legalMemoId)
      .subscribe((res: any) => {

        this.alert.succuss("تمت فتح المذكرة للمراجعة بنجاح");
        this.router.navigateByUrl('/memos/board-review/' + legalMemoId);
        this.loaderService.stopLoading();
      }, (error) => {
        this.alert.error("فشلت عملية فتح المذكرة للمراجعة");
        this.loaderService.stopLoading();
        console.error(error);
      }))
  }

}
