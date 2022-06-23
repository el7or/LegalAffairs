import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';

import { CaseQueryObject } from 'app/core/models/query-objects';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { AuthService } from 'app/core/services/auth.service';
import { CaseService } from 'app/core/services/case.service';
import { LoaderService } from 'app/core/services/loader.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CaseListItem } from 'app/core/models/case';
import { LegalMemoTypes } from 'app/core/enums/LegalMemoTypes';

@Component({
  selector: 'app-hearing-search-cases',
  templateUrl: './hearing-search-cases.component.html',
  styleUrls: ['./hearing-search-cases.component.css']
})
export class HearingSearchCasesComponent implements OnInit, OnDestroy {
  columnsToDisplay = [
    'position',
    'id',
    'startDate',
    'caseSource',
    'court',
    'circleNumber',
    'hearingsCount',
    'status',
    'actions',
  ];
  queryObject: CaseQueryObject = new CaseQueryObject({
    sortBy: 'startDate',
    isSortAscending: true
  });

  totalItems!: number;
  searchForm: FormGroup = Object.create(null);
  dataSource!: MatTableDataSource<CaseListItem>;

  private subs = new Subscription();
  errorMsg!: string;

  public get LegalMemoTypes(): typeof LegalMemoTypes {
    return LegalMemoTypes;
  }

  constructor(
    private fb: FormBuilder,
    public authService: AuthService,
    private caseService: CaseService,
    public dialogRef: MatDialogRef<HearingSearchCasesComponent>,
    private hijriConverter: HijriConverterService,
    private loaderService: LoaderService,
    private alert: AlertService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.queryObject = new CaseQueryObject(this.data);
    this.errorMsg = this.data.errorMsg;
  }

  ngOnInit(): void {
    this.init();
    this.populateCases();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  init() {
    this.searchForm = this.fb.group({
      startDateFrom: [],
      startDateTo: [],
      legalStatus: [""],
      partyName: []
    });
  }

  onFilter() {
    this.queryObject = new CaseQueryObject(this.data);
    this.queryObject.startDateFrom = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('startDateFrom')?.value?.calendarStart
    );

    this.queryObject.startDateTo = this.hijriConverter.calendarDateToDate(
      this.searchForm.get('startDateTo')?.value?.calendarStart
    );
    this.queryObject.legalStatus = this.searchForm.controls['legalStatus'].value;
    this.queryObject.partyName = this.searchForm.controls['partyName'].value;
    this.populateCases();
  }

  onClearFilter() {
    this.queryObject = new CaseQueryObject(this.data);

    this.searchForm.reset();
    this.populateCases();
  }

  populateCases() {

    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.caseService.getWithQuery(this.queryObject).subscribe(
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
    this.populateCases();
  }

  onSubmit(selectedCase: CaseListItem) {
    this.onCancel(selectedCase);
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
