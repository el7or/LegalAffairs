import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { HearingUpdateListItem } from 'app/core/models/hearing';
import { HearingUpdateQueryObject } from 'app/core/models/query-objects';
import { AppRole } from 'app/core/models/role';
import { AlertService } from 'app/core/services/alert.service';
import { AttacmentService } from 'app/core/services/attacment.service';
import { AuthService } from 'app/core/services/auth.service';
import { HearingService } from 'app/core/services/hearing.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { HearingUpdateFormComponent } from '../hearing-update-form/hearing-update-form.component';
import { HearingStatus } from 'app/core/enums/HearingStatus';

@Component({
  selector: 'app-hearing-updates-list',
  templateUrl: './hearing-updates-list.component.html',
  styleUrls: ['./hearing-updates-list.component.css']
})

export class HearingUpdatesListComponent implements OnInit {

  @Input() hearingId!: any;
  @Input() HearingAssignedTo!: any;
  @Input() Editable!: any;

  displayedColumns: string[] = [
    'position',
    'text',
    'updatedDate',
    'updateTime',
    'createdByUser',
    'attachments',
    'actions'
  ];

  queryObject: HearingUpdateQueryObject = new HearingUpdateQueryObject({
    sortBy: 'updateDate',
    pageSize: 999,
    hearingId: this.hearingId
  });

  searchForm: FormGroup = Object.create(null);
  currentPage: number = 0;
  PAGE_SIZE: number = 20;
  totalItems!: number;
  showFilter: boolean = false;
  searchText: string = '';
  hearingUpdate: any;
  hearing: any;


  dataSource!: MatTableDataSource<HearingUpdateListItem>;
  @ViewChild(MatSort) sort!: MatSort;

  public get AppRole(): typeof AppRole {
    return AppRole;
  }

  public get HearingStatus(): typeof HearingStatus {
    return HearingStatus;
  }


  private subs = new Subscription();

  constructor(
    private dialog: MatDialog,
    private alert: AlertService,
    private loaderService: LoaderService,
    private cdr: ChangeDetectorRef,
    public authService: AuthService,
    public hearingService: HearingService,
    private attachmentService: AttacmentService
  ) { }

  ngOnInit() {
    this.populateHearing();
    this.populateHearingUpdates();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateHearingUpdates();
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

  populateHearingUpdates() {
    this.queryObject.hearingId = this.hearingId;
    this.subs.add(
      this.hearingService.getAllHearingUpdates(this.queryObject).subscribe(
        (result: any) => {
          this.totalItems = result.data.totalItems;
          this.dataSource = new MatTableDataSource(result.data.items);
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  populateHearing() {
    this.subs.add(
      this.hearingService.get(this.hearingId).subscribe(
        (result: any) => {
          this.hearing = result.data;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }
  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateHearingUpdates();
  }
  resetControls() {
    this.dialog.closeAll();
  }

  OpenDialogue(hearingUpdateId?: number) {
    let dialogRef = this.dialog.open(HearingUpdateFormComponent, {
      width: '60em',
      data: { HearingUpdateId: hearingUpdateId, hearingId: this.hearingId }
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (result) => {
          if (result) {
            this.populateHearingUpdates();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDownload(id: string, name: string) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.attachmentService.downloadFile(id, name).subscribe(
        (data: any) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.download = name;
          link.click();
          this.loaderService.stopLoading();
          link.remove();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }
}
