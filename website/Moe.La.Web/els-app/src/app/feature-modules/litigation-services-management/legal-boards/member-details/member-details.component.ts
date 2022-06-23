import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { LegalBoardMemberQueryObject, QueryObject } from 'app/core/models/query-objects';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {

  displayedColumns: string[] = ['startDate', 'endDate', 'adjective'];
  PAGE_SIZE: number = 10;
  queryResult: any = {
    totalItems: 0,
    items: [],
  };

  queryObject: LegalBoardMemberQueryObject = {
    sortBy: 'startDate',
    isSortAscending: true,
    page: 1,
    pageSize: this.PAGE_SIZE,
  };
  userId: string = '';
  legalBoardId: number = 0;
  private subs = new Subscription();
  LegalBoardMemberHistory: any[] = [];
  constructor(
    private legalBoardService: LegalBoardService,
    private alert: AlertService,
    private loaderService: LoaderService,
    public dialogRef: MatDialogRef<MemberDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (data) {
      this.userId = data.userId;
      this.legalBoardId = data.legalBoardId;
    }
  }

  ngOnInit(): void {
    this.populateMemberDetails();
  }
  populateMemberDetails() {
    this.loaderService.startLoading(LoaderComponent);
    this.queryObject.userId=this.userId;
    this.queryObject.legalBoardId=this.legalBoardId;
    this.subs.add(
      this.legalBoardService.getLegalBoardMemberHistory(this.queryObject)
        .subscribe((res: any) => {
          this.queryResult.items = res.data.items;
          this.queryResult.totalItems = res.data.totalItems;
          this.loaderService.stopLoading();
        }, (error) => {
          this.alert.error("فشلت  عملية جلب البيانات");
          this.loaderService.stopLoading();
          console.error(error);
        })
    )
  }
  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populateMemberDetails();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
