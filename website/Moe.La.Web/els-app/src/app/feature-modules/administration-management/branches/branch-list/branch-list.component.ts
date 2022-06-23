import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

import { BranchFormComponent } from '../branch-form/branch-form.component';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { Branch } from 'app/core/models/branch';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { BranchService } from 'app/core/services/branch.service';

@Component({
  selector: 'app-branch-list',
  templateUrl: './branch-list.component.html',
  styleUrls: ['./branch-list.component.css'],
})
export class BranchListComponent implements OnInit, AfterViewInit , OnDestroy {
  @ViewChild(MatSort) sort!: MatSort;
  displayedColumns: string[] = ['position', 'name', 'parent', 'actions'];
  PAGE_SIZE: number = 20;
  currentPage: number = 0;
  queryObject: QueryObject = new QueryObject({
    sortBy: 'parent',
    pageSize: 9999,
  });
  dataSource = new MatTableDataSource<Branch>();
  searchForm: FormGroup = Object.create(null);
  private subs = new Subscription();

  constructor(
    public branchService: BranchService,    
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService,
    private cdr: ChangeDetectorRef,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.searchForm = this.fb.group({
      searchText: [''],
    });
    this.populateBranch();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateBranch();
        }
      )
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateBranch() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.branchService
        .getWithQuery(this.queryObject)
        .subscribe(
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
    let searchText=this.searchForm.controls['searchText'].value.trim().toLowerCase()
    this.dataSource.filteredData=this.dataSource.data.filter(m=>m.sector?.name.includes(searchText)
    || m.name.includes(searchText)
    );
  }

  onPageChange(page: number) {
    this.applyFilter(page);
  }

  openModal(id: any): void {
    const dialogRef = this.dialog.open(BranchFormComponent, {
      width: '60em',
      data: { departmentId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateBranch();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(departmentId: number) {
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
          this.branchService.delete(departmentId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateBranch();
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
}
