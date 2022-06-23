import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { MatStepper } from '@angular/material/stepper';

import { WorkflowTypeFormService } from './services/workflow-type-form.service'
import { WorkflowTypeView } from 'app/core/models/workflow-type';

@Component({
  selector: 'app-workflow-type-form',
  templateUrl: './workflow-type-form.component.html',
  styleUrls: ['./workflow-type-form.component.css']
})
export class WorkflowTypeFormComponent implements OnInit, OnDestroy {
  @ViewChild(MatStepper, { static: false }) matStepper: MatStepper = Object.create(null);

  constructor(private workflowTypeFormService: WorkflowTypeFormService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    if (this.route.snapshot.data.workflowTypeView) {
      this.workflowTypeFormService.workflowTypeView = <WorkflowTypeView>this.route.snapshot.data.workflowTypeView;
    }
  }

  ngOnDestroy(): void {
    this.workflowTypeFormService.reset();
  }

  /**
   * Handler when stepper is clicked
   * @param stepperSelectionEvent retrived from mat stepper
   */
  OnSelectionChange(stepperSelectionEvent: StepperSelectionEvent) {
  }
}
