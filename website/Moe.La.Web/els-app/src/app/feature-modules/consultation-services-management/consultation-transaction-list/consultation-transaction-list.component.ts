import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { ConsultationStatus } from 'app/core/enums/ConsultationStatus';

@Component({
  selector: 'app-consultation-transaction-list',
  templateUrl: './consultation-transaction-list.component.html',
  styleUrls: ['./consultation-transaction-list.component.css']
})
export class ConsultationTransactionListComponent implements OnInit {

  @Input('consultation') consultation: any;

  ngOnInit(): void { }
  transactionsDisplayedColumns: string[] = [
    'position',
    'createdOn',
    'createdOnTime',
    'transactionType',
    'createdBy',
    'note',
  ];
  public get ConsultationStatus(): typeof ConsultationStatus {
    return ConsultationStatus;
  }

  constructor(private matDialog: MatDialog) { }

}
