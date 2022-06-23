import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { WorkflowTypeListItem } from 'app/core/models/workflow-type';
import { WorkflowTypeListService } from './services/workflow-type-list-service';
import { LoaderService } from 'app/core/services/loader.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-workflow-type-list',
  templateUrl: './workflow-type-list.component.html',
  styleUrls: ['./workflow-type-list.component.css']
})
export class WorkflowTypeListComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['id', 'typeArName', 'lockPeriod', 'isActive', 'actions'];
  dataSource = new MatTableDataSource<WorkflowTypeListItem>();
  showFilter: boolean = false;

  @ViewChild(MatPaginator) paginator: MatPaginator = Object.create(null);
  @ViewChild(MatSort) sort: MatSort = Object.create(null);

  constructor(private workflowTypeListService: WorkflowTypeListService,
    private loaderService: LoaderService,
    private alert: AlertService) { }

  ngOnInit(): void {
    this.loaderService.startLoading(LoaderComponent);

    this.workflowTypeListService.GetAllWorkflowTypes().subscribe(
      (res: any) => {
        this.dataSource = res.data;
      },
      (err) => {
        this.alert.error('فشلت عملية جلب البيانات !');
      },
      () => this.loaderService.stopLoading()
    );
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
