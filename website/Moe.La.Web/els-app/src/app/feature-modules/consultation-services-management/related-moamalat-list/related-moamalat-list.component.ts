import { Component, ViewChild, Input, SimpleChanges, OnChanges } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

import { MoamalatQueryObject } from 'app/core/models/query-objects';
import { MoamalatListItem } from 'app/core/models/moamalat';

@Component({
  selector: 'app-related-moamalat-list',
  templateUrl: './related-moamalat-list.component.html',
  styleUrls: ['./related-moamalat-list.component.css']
})
export class RelatedMoamalatListComponent implements OnChanges {
  displayedColumns: string[] = [
    'moamalaNumber',
    'createdOn',
    'subject',
    'workItemType',
    'createdBy',
    'actions',
  ];

  totalItems!: number;
  queryObject: MoamalatQueryObject = new MoamalatQueryObject();

  @ViewChild(MatSort) sort!: MatSort;

  @Input('dataSource') dataSource!: MatTableDataSource<MoamalatListItem>;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['dataSource']) {
      this.dataSource = changes['dataSource'].currentValue;
    }
  }

}
