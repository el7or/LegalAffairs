import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

import { WorkflowStepListItem } from 'app/core/models/workflow-step';

@Component({
  selector: 'app-workflow-steps-list',
  templateUrl: './workflow-steps-list.component.html'
})
export class WorkflowStepsListComponent implements OnInit, OnDestroy {
  @Input() workflowSteps!: WorkflowStepListItem[];

  displayedColumns: string[] = ['position', 'stepArName', 'workflowStepCategoryArName', 'id'];
  dataSource = new MatTableDataSource<WorkflowStepListItem>();

  constructor() { }

  ngOnInit(): void {
    this.dataSource = [...this.workflowSteps] as any;
  }

  ngOnDestroy(): void {
  }
}
