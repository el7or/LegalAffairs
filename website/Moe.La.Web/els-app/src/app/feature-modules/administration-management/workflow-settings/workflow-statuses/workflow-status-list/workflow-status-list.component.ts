import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { WorkflowStatusListItem } from 'app/core/models/workflow-status';
import { WorkflowStatusListService } from './services/workflow-status-list-service';
import { LoaderService } from 'app/core/services/loader.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-workflow-status-list',
  templateUrl: './workflow-status-list.component.html',
  styleUrls: ['./workflow-status-list.component.css']
})
export class WorkflowStatusListComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['id', 'statusArName', 'statusEnName', 'actions'];
  dataSource = new MatTableDataSource<WorkflowStatusListItem>();
  showFilter: boolean = false;

  @ViewChild(MatPaginator) paginator: MatPaginator = Object.create(null);
  @ViewChild(MatSort) sort: MatSort = Object.create(null);

  constructor(private workflowStatusListService: WorkflowStatusListService,
    private loaderService: LoaderService,
    private alert: AlertService) { }

  ngOnInit(): void {
    this.loaderService.startLoading(LoaderComponent);

    this.workflowStatusListService.GetAllWorkflowStatuses().subscribe(
      (res) => {
        var result: any = res;
        this.dataSource = result.data;
      },
      (err) => {
        this.alert.error('فشلت عملية جلب البيانات !');
        this.loaderService.stopLoading();
      });

    this.loaderService.stopLoading();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  init(): void {

  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  syncPaginator(event: PageEvent) {
    this.paginator.pageIndex = event.pageIndex;
    this.paginator.pageSize = event.pageSize;
    this.paginator.page.emit(event);
  }
}
