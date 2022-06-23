import { MeetingQueryObject } from './../../../../core/models/query-objects';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BoardMeetingListItem } from 'app/core/models/board-meeting';
import { Subscription } from 'rxjs';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LegalBoardService } from 'app/core/services/legal-board.service';

@Component({
  selector: 'app-board-meeting-list',
  templateUrl: './board-meeting-list.component.html',
  styleUrls: ['./board-meeting-list.component.css']
})
export class BoardMeetingListComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'meetingPlace',
    'meetingDate',
    'meetingTime',
    'board',
    'memo',
    'actions',
  ];

  legalMemoStatus: any;
  totalItems!: number;
  queryObject: MeetingQueryObject = new MeetingQueryObject({
    sortBy: 'meetingDate',
    isSortAscending: false
  });
  searchText: string = '';
  showFilter: boolean = false;
  secondSubCategories: any = [];
  searchForm: FormGroup = Object.create(null);

  @ViewChild(MatSort) sort!: MatSort;
  dataSource!: MatTableDataSource<BoardMeetingListItem>;

  private subs = new Subscription();

  constructor(
    private legalBoardService: LegalBoardService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private fb: FormBuilder,
    private hijriConverter: HijriConverterService,
  ) { }

  ngOnInit() {
    this.initSearchForm();
    this.populateMeetings();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  initSearchForm() {
    this.searchForm = this.fb.group({
      meetingPlace: [],
      meetingDateFrom: [],
      meetingDateTo: [],
    });
  }
  ngAfterViewInit() {
    this.subs.add(
      this.sort.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateMeetings();
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  populateMeetings() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalBoardService.getMeetingsWithQuery(this.queryObject).subscribe(
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
    this.queryObject.meetingPlace = this.searchForm.get('meetingPlace')?.value;
    this.queryObject.meetingDateFrom = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('meetingDateFrom')?.value?.calendarStart
    );
    this.queryObject.meetingDateTo = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('meetingDateTo')?.value?.calendarStart
    );

    this.populateMeetings()
  }

  onSearch() {
    this.queryObject.searchText = this.searchText?.trim();
    this.populateMeetings()
  }


  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMeetings()
  }

  advancedFilter() {
    this.showFilter = !this.showFilter;
    if (!this.showFilter) {
      this.queryObject = new MeetingQueryObject();
      this.populateMeetings()
      this.searchForm.reset();
    }
  }
}
