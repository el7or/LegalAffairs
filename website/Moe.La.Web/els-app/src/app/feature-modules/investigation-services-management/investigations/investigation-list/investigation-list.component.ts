import { Subscription } from 'rxjs';
import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
  ChangeDetectorRef,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

import { InvestigationService } from 'app/core/services/investigation.service';
import { InvestigationQueryObject } from 'app/core/models/query-objects';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AlertService } from 'app/core/services/alert.service';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
import { InvestigationListItem } from 'app/core/models/investigation';

@Component({
  selector: 'app-investigation-list',
  templateUrl: './investigation-list.component.html',
  styleUrls: ['./investigation-list.component.css'],
})
export class InvestigationListComponent implements OnInit, OnDestroy {
  columnsToDisplay = [
    'position',
    'investigationNumber',
    'startDate',
    'subject',
    'investigator',
    'investigationStatus',
    'isHasCriminalSide',
    'actions',
  ];
  queryObject: InvestigationQueryObject = new InvestigationQueryObject({
    sortBy: 'investigationNumber',
  });
  totalItems!: number;

  dataSource!: MatTableDataSource<InvestigationListItem>;
  @ViewChild(MatSort) sort!: MatSort;

  searchText?: string;

  private subs = new Subscription();

  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  isInvestigatorManager = this.authService.checkRole(AppRole.DepartmentManager, Department.Investiation);//AppRole.InvestigationManager
  constructor(
    private investigationService: InvestigationService,
    private loaderService: LoaderService,
    private alert: AlertService,
    private cdr: ChangeDetectorRef,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    this.populateInvestigations();
  }
  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateInvestigations();
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

  populateInvestigations() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.investigationService.getWithQuery(this.queryObject).subscribe(
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
    this.populateInvestigations();
  }

  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateInvestigations();
  }

  onExportExcel() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.investigationService.exportExcel(this.queryObject).subscribe(
        (data: any) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.download = 'investigation-list.xlsx';
          link.click();
          this.loaderService.stopLoading();
        },
        (error: any) => {
          console.error(error);
          this.alert.error('فشل تصدير البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onExportPdf() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.investigationService.exportPdf(this.queryObject).subscribe(
        (data: any) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.target = '_blank';
          link.click();
          this.loaderService.stopLoading();
        },
        (error: any) => {
          console.error(error);
          this.alert.error('فشل تصدير البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }
}
