import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';
import {
  trigger,
  style,
  animate,
  state,
  transition,
} from '@angular/animations';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Subscription } from 'rxjs';

import { MoamalaTransactionTypes } from 'app/core/enums/MoamalaTransactionTypes';
import { Department } from 'app/core/enums/Department';
import { MoamalaTransactionList } from 'app/core/models/moamala-transaction';
import { MoamalaTransactionQueryObject } from 'app/core/models/query-objects';
import { AppRole } from 'app/core/models/role';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MoamalatTransactionService } from 'app/core/services/moamalat-transaction.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';


@Component({
  selector: 'app-moamalat-inbound-list',
  templateUrl: './moamalat-inbound-list.component.html',
  styleUrls: ['./moamalat-inbound-list.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class MoamalatInboundListComponent implements OnInit, AfterViewInit, OnDestroy {
  columnsToDisplay = [
    'position',
    'subject',
    'referenceNo',
    'unifiedNo',
    'sendingDepartment',
    'actions',
  ];
  expandedDetail = ['moamalatDetails'];

  expandedIndexes: any[] = [];

  queryObject: MoamalaTransactionQueryObject = new MoamalaTransactionQueryObject({
    sortBy: 'unifiedNo'
  });

  totalItems!: number;

  private subs = new Subscription();

  showFilter: boolean = false;

  dataSource!: MatTableDataSource<MoamalaTransactionList>;
  @ViewChild(MatSort) sort!: MatSort;

  searchForm: FormGroup = Object.create(null);

  searchText?: string;

  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);

  constructor(
    private fb: FormBuilder,
    public authService: AuthService,
    private moamalatTransactionService: MoamalatTransactionService,
    private loaderService: LoaderService,
    private alert: AlertService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.init();
    this.populateMoamalatTransactions();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateMoamalatTransactions();
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
      subject: [],
      referenceNo: [],
      unifiedNo: [],
    });
  }

  populateMoamalatTransactions() {
    this.loaderService.startLoading(LoaderComponent);
    this.queryObject.transactionType = MoamalaTransactionTypes.inbound;
    this.subs.add(
      this.moamalatTransactionService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.totalItems = result.data.totalItems;
          this.dataSource = new MatTableDataSource(result.data.items);
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMoamalatTransactions();
  }

  onShowFilter() {
    this.showFilter = !this.showFilter;
    this.onClearFilter();
  }

  onFilter() {
    this.queryObject = new MoamalaTransactionQueryObject({
      sortBy: 'unifiedNo',
    });

    this.queryObject.subject = this.searchForm.controls['subject'].value;

    this.queryObject.unifiedNo = this.searchForm.controls['unifiedNo'].value;

    this.queryObject.referenceNo = this.searchForm.controls['referenceNo'].value;

    this.populateMoamalatTransactions();
  }

  onClearFilter() {
    this.queryObject = new MoamalaTransactionQueryObject({
      sortBy: 'unifiedNo',
    });
    this.searchForm.reset();
    this.populateMoamalatTransactions();
  }

  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateMoamalatTransactions();
  }

  onClickRow(i: number) {
    if (!this.expandedIndexes.includes(i)) {
      this.expandedIndexes.push(i);

    }
    else {
      this.expandedIndexes.splice(this.expandedIndexes.indexOf(i), 1);

    }
  }
}
