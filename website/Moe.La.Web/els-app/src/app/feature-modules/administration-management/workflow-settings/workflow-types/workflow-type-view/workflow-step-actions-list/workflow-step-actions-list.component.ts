import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { WorkflowStepActionListItem } from 'app/core/models/workflow-step-action';

@Component({
  selector: 'app-workflow-steps-actions-list',
  templateUrl: './workflow-step-actions-list.component.html'
})
export class WorkflowStepActionsComponent implements OnInit, OnDestroy {
  @Input() workflowStepActions!: WorkflowStepActionListItem[];

  displayedColumns: string[] = ['position', 'workflowStepName', 'workflowActionName', 'nextStepName', 'nextStatusName', 'description'];
  dataSource = new MatTableDataSource<WorkflowStepActionListItem>();

  constructor() { }

  ngOnInit(): void {
    this.dataSource = [...this.workflowStepActions] as any;
  }

  ngOnDestroy(): void {
  }
}
