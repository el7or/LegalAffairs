import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-consultation-merits-list',
  templateUrl: './consultation-merits-list.component.html',
  styleUrls: ['./consultation-merits-list.component.css']
})
export class ConsultationMeritsListComponent implements OnInit {
  @Input('consultation') consultation: any;

  ngOnInit(): void { }

  meritsDisplayedColumns: string[] = [
    'position',
    'text',
  ];

  constructor(private matDialog: MatDialog) { }

}
