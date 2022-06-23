import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-consultation-grounds-list',
  templateUrl: './consultation-grounds-list.component.html',
  styleUrls: ['./consultation-grounds-list.component.css']
})
export class ConsultationGroundsListComponent implements OnInit {
  @Input('consultation') consultation: any;

  ngOnInit(): void { }

  groundsDisplayedColumns: string[] = [
    'position',
    'text',
  ];

  constructor(private matDialog: MatDialog) { }

}
