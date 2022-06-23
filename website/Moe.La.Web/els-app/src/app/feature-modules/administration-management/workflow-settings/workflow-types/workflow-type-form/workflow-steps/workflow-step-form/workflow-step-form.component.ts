import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription, forkJoin } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { WorkflowStepService } from './services/workflow-step.service';
import { WorkflowStep, WorkflowStepListItem } from 'app/core/models/workflow-step';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Guid } from 'guid-typescript';
import { RoleService } from 'app/core/services/role.service';
import { WorkflowTypeFormService } from '../../services/workflow-type-form.service';

@Component({
  selector: 'app-workflow-step-form',
  templateUrl: './workflow-step-form.component.html',
  styleUrls: ['./workflow-step-form.component.css']
})
export class WorkflowStepFormComponent implements OnInit, OnDestroy {
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  workflowStep = new WorkflowStep();
  workflowStepCategories: KeyValuePairs[] = [];
  allRoles: any[] = [];

  constructor(private fb: FormBuilder,
    public dialogRef: MatDialogRef<WorkflowStepFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WorkflowStep,
    private workflowStepService: WorkflowStepService,
    private workflowTypeFormService: WorkflowTypeFormService,
    private roleService: RoleService,
    private loaderService: LoaderService,
    private alert: AlertService) { }

  ngOnInit(): void {
    if (this.data && this.data.id) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(this.workflowStepService.getWorkflowStep(this.data.id).subscribe(
        (res: any) => {
          this.loaderService.stopLoading();
          this.populateForm(res.data);
        },
        (err) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب بيانات الإجراء');
        }
      ));
    }

    this.init();

    var sources = [
      this.workflowStepService.getWorkflowStepCategories(),
      this.roleService.getAll()
    ];

    this.subs.add(
      forkJoin(sources).subscribe(
        (res: any) => {
          this.loaderService.stopLoading();
          this.workflowStepCategories = res[0];
          this.allRoles = res[1].data.items;
        },
        (err) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  public onSave(): void {
    if (!this.form.valid) {
      return;
    }

    this.loaderService.startLoading(LoaderComponent);
    this.workflowStep = { ...this.workflowStep, ...this.form.value };
    this.workflowStep.workflowTypeId = this.workflowTypeFormService.workflowTypeView.workflowType.id!;
    let result$ = this.workflowStep.id === Guid.EMPTY
      ? this.workflowStepService.createWorkflowStep(this.workflowStep)
      : this.workflowStepService.updateWorkflowStep(this.workflowStep);

    this.subs.add(
      result$.subscribe(
        (res: any) => {
          this.loaderService.stopLoading();
          var msg = 'تم الحفظ بنجاح';

          if (res.statusCode === 201) {
            this.addWorkflowStep(res.data);
          }
          else {
            this.updateWorkflowStep(res.data);
            msg = 'تم التعديل بنجاح';
          }

          this.alert.succuss(msg);
          this.dialogRef.close();
        },
        (err) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية الحفظ');
        })
    );
  }

  /**
   * Closes the dialog.
   */
  onCancel(): void {
    this.dialogRef.close();
  }

  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group(
      {
        id: [Guid.createEmpty().toString()],
        workflowStepCategoryId: ["", Validators.required],
        stepArName: ['', Validators.required],
        roles: [],
      });
  }

  /**
   * Update the form values.
   * @param workflowStep workflow step used to populate the form.
   */
  private populateForm(workflowStep: WorkflowStep) {
    this.form.patchValue({
      id: workflowStep.id,
      workflowStepCategoryId: workflowStep.workflowStepCategoryId,
      stepArName: workflowStep.stepArName
    });
  }

  /**
   * Add workflow step to workflowSteps array.
   * @param workflowStep 
   */
  private addWorkflowStep(workflowStep: WorkflowStep): void {
    var newWorkflowStep = new WorkflowStepListItem();
    newWorkflowStep.id = workflowStep.id!;
    newWorkflowStep.stepArName = workflowStep.stepArName;
    newWorkflowStep.workflowTypeId = workflowStep.workflowTypeId;
    newWorkflowStep.workflowStepCategoryId = workflowStep.workflowStepCategoryId;
    newWorkflowStep.workflowStepCategoryArName = this.workflowStepCategories.find(m => m.id === workflowStep.workflowStepCategoryId)?.name!;
    this.workflowTypeFormService.workflowTypeView.workflowSteps.push(newWorkflowStep);
  }

  /**
   * Update workflow step in workflowSteps array.
   * @param workflowStep workflow step to be updated.
   */
  private updateWorkflowStep(workflowStep: WorkflowStep): void {
    let itemToUpdate = this.workflowTypeFormService.workflowTypeView.workflowSteps.find(m => m.id === workflowStep.id);

    if (itemToUpdate) {
      itemToUpdate.workflowTypeId = workflowStep.workflowTypeId;
      itemToUpdate.workflowStepCategoryId = workflowStep.workflowStepCategoryId;
      itemToUpdate.workflowStepCategoryArName = this.workflowStepCategories.find(m => m.id === workflowStep.workflowStepCategoryId)?.name!;
      itemToUpdate.stepArName = workflowStep.stepArName;
      let index = this.workflowTypeFormService.workflowTypeView.workflowSteps.indexOf(itemToUpdate);
      this.workflowTypeFormService.workflowTypeView.workflowSteps[index] = itemToUpdate;
    }
  }
}
