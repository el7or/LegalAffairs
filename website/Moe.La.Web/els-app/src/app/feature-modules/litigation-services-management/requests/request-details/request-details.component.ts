import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { RequestStatus } from 'app/core/enums/RequestStatus';
import { CompareRequestWithHistoryFormComponent } from '../compare-request-with-history/compare-request-with-history.component';

@Component({
  selector: 'app-request-details',
  templateUrl: './request-details.component.html',
  styleUrls: ['./request-details.component.css'],
})
export class RequestDetailsComponent implements OnInit {
  @Input('request') request: any;
  @Input('history') history: any;

  ngOnInit(): void { }
  transactionsDisplayedColumns: string[] = [
    'position',
    'createdOn',
    'createdOnTime',
    'transactionType',
    'createdBy',
    'description',
  ];
  historyDisplayedColumns: string[] = [
    'position',
    'requestStatus',
    'createdByFullName',
    'createdOn',
    'actions',
  ];
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  constructor(private matDialog: MatDialog) { }

  onCompare(requestId: Number, historyId: Number) {
    this.matDialog.open(CompareRequestWithHistoryFormComponent, {
      width: '70em',
      data: {
        requestId: requestId,
        historyId: historyId,
        requestType: this.request.requestType.id

      },
    });
  }
}
