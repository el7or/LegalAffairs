import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { WorkflowTypeView } from 'app/core/models/workflow-type';

@Component({
  selector: 'app-workflow-type-view',
  templateUrl: './workflow-type-view.component.html',
  styleUrls: ['./workflow-type-view.component.css']
})
export class WorkflowTypeViewComponent implements OnInit, OnDestroy {
  workflowTypeView!: WorkflowTypeView;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.workflowTypeView = <WorkflowTypeView>this.route.snapshot.data.workflowTypeView;
  }

  ngOnDestroy(): void {
  }
}
