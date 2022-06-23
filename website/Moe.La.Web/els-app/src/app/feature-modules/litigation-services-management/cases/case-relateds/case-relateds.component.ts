import { Component, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';

import { CaseListItem } from 'app/core/models/case';

@Component({
  selector: 'app-case-relateds',
  templateUrl: './case-relateds.component.html',
  styleUrls: ['./case-relateds.component.css'],
})
export class CaseRelatedsComponent {
  @Input('caseRelateds') caseRelateds!: MatTableDataSource<CaseListItem>;
  columnsToDisplay = [
    'caseNumberInSource',
    'startDate',
    'caseSource',
    'court',
    'circleNumber',
    'status',
  ];
  expandedDetail = ['caseDetails'];

  expandedIndexes: any[] = [];
  //CaseListItem
  public get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }

  constructor() { }

  onClickRow(i: number) {
    if (!this.expandedIndexes.includes(i)) {
      this.expandedIndexes.push(i);

    }
    else {
      this.expandedIndexes.splice(this.expandedIndexes.indexOf(i), 1);

    }
  }
}
