import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Guid } from "guid-typescript";

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { WorkflowTypeService } from './services/workflow-type-service';
import { WorkflowType, WorkflowTypeListItem } from 'app/core/models/workflow-type';
import { MatStepper } from '@angular/material/stepper';
import { WorkflowTypeFormService } from '../services/workflow-type-form.service';

@Component({
  selector: 'app-workflow-type',
  templateUrl: './workflow-type.component.html'
})
export class WorkflowTypeComponent implements OnInit, OnDestroy {
  workflowType: WorkflowType = new WorkflowType();
  @Input() stepper: MatStepper = Object.create(null);

  workflowTypeForm: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(private fb: FormBuilder,
    private workflowTypesService: WorkflowTypeService,
    private workflowTypeFormService: WorkflowTypeFormService,
    private loaderService: LoaderService,
    private alert: AlertService) { }

  ngOnInit(): void {
    this.init();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  public onSave(): void {
    if (!this.workflowTypeForm.valid) {
      return;
    }

    this.loaderService.startLoading(LoaderComponent);
    this.workflowType = { ...this.workflowType, ...this.workflowTypeForm.value };
    let result$ = this.workflowType.id === Guid.EMPTY
      ? this.workflowTypesService.createWorkflowType(this.workflowType)
      : this.workflowTypesService.updateWorkflowType(this.workflowType);

    this.subs.add(
      result$.subscribe(
        (res: any) => {
          this.loaderService.stopLoading();
          var msg = 'تم الحفظ بنجاح';

          if (res.statusCode === 200) {
            msg = 'تم التعديل بنجاح';
          }

          this.workflowTypeFormService.workflowTypeView.workflowType = res.data;

          this.alert.succuss(msg);
          this.populateForm(res.data);
          this.stepper.next();
        },
        (err) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية الحفظ');
        })
    );
  }

  /**
   * Init component.
   */
  private init(): void {
    this.workflowTypeForm = this.fb.group(
      {
        id: [Guid.createEmpty().toString()],
        typeArName: ['', Validators.required],
        lockPeriod: ['', Validators.required],
        isActive: false
      });

    if (this.workflowTypeFormService.workflowTypeView.workflowType) {
      this.workflowType = this.workflowTypeFormService.workflowTypeView.workflowType;
      this.populateForm(this.workflowType);
    }
  }

  /**
   * Update the form values.
   * @param workflowType workflow type used to populate the form.
   */
  private populateForm(workflowType: WorkflowType) {
    this.workflowTypeForm.patchValue({
      id: workflowType.id,
      typeArName: workflowType.typeArName,
      lockPeriod: workflowType.lockPeriod,
      isActive: workflowType.isActive
    });
  }
}
