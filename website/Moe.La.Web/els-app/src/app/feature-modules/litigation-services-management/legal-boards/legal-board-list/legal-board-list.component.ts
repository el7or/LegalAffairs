import { Location } from '@angular/common';
import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
  ChangeDetectorRef,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';

import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { LegalBoardListItem } from 'app/core/models/legal-board';
import { LegalBoardStatus } from 'app/core/models/legal-board-status';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';

@Component({
  selector: 'app-legal-board-list',
  templateUrl: './legal-board-list.component.html',
  styleUrls: ['./legal-board-list.component.css'],
})
export class LegalBoardListComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'name',
    'createdOn',
    'status',
    'board-type',
    'boardHead',
    'actions',
  ];
  public legalBoardStatus = LegalBoardStatus;

  LegalBoardList: LegalBoardListItem[] = [];

  queryObject: QueryObject = new QueryObject({
    sortBy: 'name',
    pageSize: 9999,
  });

  currentPage: number = 0;
  PAGE_SIZE: number = 20;

  dataSource = new MatTableDataSource<LegalBoardListItem>();

  searchForm: FormGroup = Object.create(null);
  @ViewChild(MatSort) sort!: MatSort;

  private subs = new Subscription();

  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  isAdmin = this.authService.checkRole(AppRole.Admin);
  constructor(
    public legalBoardService: LegalBoardService,
    private dialog: MatDialog,
    private alert: AlertService,
    private location: Location,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    public authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.searchForm = this.fb.group({
      searchText: [''],
    });
    this.populateLegalBoards();
  }
  ngAfterViewInit() {
    this.cdr.detectChanges();

    this.subs.add(
      this.sort.sortChange.subscribe((result: any) => {
        this.queryObject.sortBy = result.active;
        this.queryObject.isSortAscending = result.direction == 'asc';
        this.populateLegalBoards();
      })
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  populateLegalBoards() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalBoardService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.dataSource = new MatTableDataSource(result.data.items);
          this.applyFilter();
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

  applyFilter(page: number = 0) {
    this.currentPage = page;
    let searchText = this.searchForm.controls['searchText'].value
      .trim()
      .toLowerCase();
    this.dataSource.filteredData = this.dataSource.data.filter(
      (m) =>
        m.name.includes(searchText) ||
        m.status.name.includes(searchText) ||
        m.type.includes(searchText)
    );
  }

  onPageChange(page: number) {
    this.applyFilter(page);
  }

  resetControls() {
    this.dialog.closeAll();
  }

  onChangeStatus(legalBoardId: number, isActivated: number) {
    let text =
      isActivated != LegalBoardStatus.Activated
        ? 'هل أنت متأكد من إتمام عملية التعطيل لهذه اللجنة؟'
        : 'هل أنت متأكد من إتمام عملية التفعيل لهذه اللجنة؟';
    let confirmText =
      isActivated == LegalBoardStatus.Activated ? 'تفعيل' : 'تعطيل';
    let successMessage =
      isActivated != LegalBoardStatus.Activated
        ? 'تم تعطيل اللجنة  بنجاح'
        : 'تم تفعيل اللجنة  بنجاح';
    let errorMessage =
      isActivated == LegalBoardStatus.Activated
        ? 'فشل تعطيل اللجنة'
        : 'فشل تفعيل اللجنة';
    Swal.fire({
      title: 'تأكيد',
      text: text,
      icon: isActivated != LegalBoardStatus.Activated ? 'warning' : 'success',
      showCancelButton: true,
      confirmButtonColor:
        isActivated != LegalBoardStatus.Activated ? '#f44336' : '#28a745',
      confirmButtonText: confirmText,
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.legalBoardService
            .changeStatus(legalBoardId, isActivated)
            .subscribe(
              () => {
                this.loaderService.stopLoading();
                this.alert.succuss(successMessage);
                this.populateLegalBoards();
                this.resetControls();
              },
              (error) => {
                console.error(error);
                this.loaderService.stopLoading();
                this.alert.error(errorMessage);
              }
            )
        );
      }
    });
  }
}
