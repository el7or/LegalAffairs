import { Component, OnInit, ViewChild, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';

import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { AuthService } from 'app/core/services/auth.service';
import { MoamalatListItem } from 'app/core/models/moamalat';
import { MoamalatRaselInboxService } from 'app/core/services/moamalatRaselInbox.service';
import { MoamalatQueryObject, MoamalatRaselQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { MoamalaRaselStatuses } from 'app/core/enums/MoamalaRaselStatuses';
import { ConfidentialDegrees } from 'app/core/enums/ConfidentialDegrees';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { UserList } from 'app/core/models/user';

@Component({
  selector: 'app-moamalat-rasel-inbox-list',
  templateUrl: './moamalat-rasel-inbox-list.component.html',
  styleUrls: ['./moamalat-rasel-inbox-list.component.css']
})
export class MoamalatRaselInboxListComponent implements OnInit {
  displayedColumns: string[] = [
    'unifiedNumber',
    'itemNumber',
    'gregorianCreatedDate',
    'subject',
    'comments',
    'itemPrivacy',
    'status',
    'actions',
  ];

  totalItems!: number;
  queryObject: MoamalatRaselQueryObject = new MoamalatRaselQueryObject();
  searchText: string = '';
  showFilter: boolean = false;
  searchForm: FormGroup = Object.create(null);

  @ViewChild(MatSort) sort!: MatSort;
  dataSource!: MatTableDataSource<MoamalatListItem>;


  //lists
  ministryDepartments: KeyValuePairs[] = [];
  departments: KeyValuePairs[] = [];
  usersList: UserList[] = [];

  private subs = new Subscription();


  public get MoamalaRaselStatuses(): typeof MoamalaRaselStatuses {
    return MoamalaRaselStatuses;
  }
  public get ConfidentialDegrees(): typeof ConfidentialDegrees {
    return ConfidentialDegrees;
  }
  constructor(
    private moamalatRaselInboxService: MoamalatRaselInboxService,
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private hijriConverter: HijriConverterService,
    private dialog: MatDialog,

  ) { }

  ngOnInit() {
    this.init();
    this.populateMoamalat();
  }

  init() {
    this.searchForm = this.fb.group({
      searchText: [],
      status: [0],
      itemPrivacy: [0],
      createdOnFrom: [],
      createdOnTo: []
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateMoamalat(tableData: any = null) {
    // populate table component data
    if (tableData?.page)
      this.queryObject.page = tableData.page;
    if (tableData?.sortBy)
      this.queryObject.sortBy = tableData.sortBy;
    if (tableData?.isSortAscending)
      this.queryObject.isSortAscending = tableData.isSortAscending == 'true';

    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.moamalatRaselInboxService.getWithQuery(this.queryObject).subscribe(
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
    this.queryObject.searchText = this.searchForm.get('searchText')?.value;
    this.queryObject.status = this.searchForm.get('status')?.value;
    this.queryObject.itemPrivacy = this.searchForm.get('itemPrivacy')?.value;
    // convert createdOn from date from hijri to miladi
    this.queryObject.createdOnFrom = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('createdOnFrom')?.value?.calendarStart
    );
    // convert createdOn to date hijri to miladi
    this.queryObject.createdOnTo = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('createdOnTo')?.value?.calendarStart
    );
    this.populateMoamalat()
  }

  onClearFilter() {
    this.queryObject = new MoamalatQueryObject();
    this.searchForm.reset();
    this.populateMoamalat();
  }

  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateMoamalat()
  }

  advancedFilter() {
    this.showFilter = !this.showFilter;

    if (!this.showFilter) {

      this.queryObject = new MoamalatQueryObject();
      this.populateMoamalat()
      this.searchForm.reset();
    }
  }

  resetControls() {
    this.dialog.closeAll();
  }
  ngAfterViewInit() {
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.populateMoamalat();
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['dataSource']) {
      this.dataSource = changes['dataSource'].currentValue;
    }
  }

  onPageChange(page: number) {
    this.populateMoamalat();
  }

  onDelete(moamalaId: number) {
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
          this.moamalatRaselInboxService.delete(moamalaId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateMoamalat()
              this.resetControls();
            },
            (error: any) => {
              console.error(error);
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الحذف !');
            }
          )
        );
      }
    });
  }

  receiveMoamalaRasel(moamalaId: any) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام عملية استلام المعاملة؟',
      icon: 'info',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'استلام',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.moamalatRaselInboxService.receiveMoamalaRasel(moamalaId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية استلام المعاملة  بنجاح');
              this.populateMoamalat()
              this.resetControls();
            },
            (error: any) => {
              console.error(error);
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية استلام المعاملة !');
            }
          )
        );
      }
    });
  }

}
