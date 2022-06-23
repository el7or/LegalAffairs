import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';

import { InvestigationRecordQueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { InvestigationRecordService } from 'app/core/services/investigation-record.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-investigation-record-list',
  templateUrl: './investigation-record-list.component.html',
  styleUrls: ['./investigation-record-list.component.css']
})
export class InvestigationRecordListComponent implements OnInit {
  displayedColumns: string[] = [
    'position',
    'recordNumber',
    'startDate',
    'startTime',
    'endDate',
    'endTime',
    'recordStatus',
    'createdBy',
    'actions',
  ];
  @ViewChild(MatSort) sort!: MatSort;
  dataSource!: MatTableDataSource<any>;
  totalItems!: number;
  queryObject: InvestigationRecordQueryObject = new InvestigationRecordQueryObject();
  searchText: string = '';
  private subs = new Subscription();
  investigationId?: number;
  constructor(
    private investiationRecordService: InvestigationRecordService,
    public authService: AuthService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private activatedRouter: ActivatedRoute,
    public location: Location
  ) {
    this.activatedRouter.queryParams.subscribe((params) => {
      this.investigationId = params.investigationId;
    });
  }

  ngOnInit(): void {
    this.populateInvestigationRecords();
  }
  ngAfterViewInit() {
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateInvestigationRecords();
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
  populateInvestigationRecords() {
    this.loaderService.startLoading(LoaderComponent);
    if (this.investigationId) {
      this.queryObject.investigationId = this.investigationId;
    }
    this.subs.add(
      this.investiationRecordService.getWithQuery(this.queryObject).subscribe(
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
  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    // this.populateMemos()
  }


  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    //this.populateMemos()
  }

}
