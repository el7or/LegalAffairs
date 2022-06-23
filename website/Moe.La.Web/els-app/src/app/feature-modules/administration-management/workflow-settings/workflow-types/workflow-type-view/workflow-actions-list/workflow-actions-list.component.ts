import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { WorkflowActionListItem } from 'app/core/models/workflow-action';

@Component({
  selector: 'app-workflow-actions-list',
  templateUrl: './workflow-actions-list.component.html'
})
export class WorkflowActionsListComponent implements OnInit, OnDestroy {
  @Input() workflowActions!: WorkflowActionListItem[];

  displayedColumns: string[] = ['position', 'actionArName', 'id'];
  dataSource = new MatTableDataSource<WorkflowActionListItem>();

  constructor() { }

  ngOnInit(): void {
    this.dataSource = [...this.workflowActions] as any;
  }

  ngOnDestroy(): void {
  }
}
