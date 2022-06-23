import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MinistrySectorService } from 'app/core/services/ministry-sectors.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { MinistrySectorFormComponent } from '../ministry-sectors-form/ministry-sector-form.component';

@Component({
  selector: 'app-ministry-sectors-list',
  templateUrl: './ministry-sectors-list.component.html',
  styleUrls: ['./ministry-sectors-list.component.css']
})
export class MinistrySectorListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'actions'];

  ministrySectors: any[] = [];
  allMinistrySectors: any[] = [];

  searchText: string = '';
  showFilter: boolean = false;

  form: FormGroup = Object.create(null);

  queryResult: any = {
    totalItems: 0,
    items: [],
  };

  PAGE_SIZE: number = 20;

  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: this.PAGE_SIZE,
  };

  @ViewChild('template', { static: true }) template:
    | TemplateRef<any>
    | undefined;

  private subs = new Subscription();

  constructor(
    public ministrySectorService: MinistrySectorService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [],
    });
    this.populateMinistrySectors();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateMinistrySectors() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.ministrySectorService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allMinistrySectors = result.data.items;
          this.ministrySectors = this.allMinistrySectors;
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
      this.ministrySectors = this.allMinistrySectors.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else this.ministrySectors = this.allMinistrySectors;
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populateMinistrySectors();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMinistrySectors();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(MinistrySectorFormComponent, {
      width: '30em',
      data: { ministrySectorId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateMinistrySectors();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(ministrySectorId: number) {
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
          this.ministrySectorService.delete(ministrySectorId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateMinistrySectors();
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
