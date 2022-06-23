import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Guid } from 'guid-typescript';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { WorkflowAction, WorkflowActionListItem } from 'app/core/models/workflow-action';
import { WorkflowActionService } from './services/workflow-action.service';
import { WorkflowTypeFormService } from '../../services/workflow-type-form.service';

@Component({
  selector: 'app-workflow-action-form',
  templateUrl: './workflow-action-form.component.html',
  styleUrls: ['./workflow-action-form.component.css']
})
export class WorkflowActionFormComponent implements OnInit, OnDestroy {
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  workflowAction = new WorkflowAction();

  constructor(private fb: FormBuilder,
    public dialogRef: MatDialogRef<WorkflowActionFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WorkflowAction,
    private workflowActionService: WorkflowActionService,
    private workflowTypeFormService: WorkflowTypeFormService,
    private loaderService: LoaderService,
    private alert: AlertService) {

  }

  ngOnInit(): void {
    if (this.data && this.data.id) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(this.workflowActionService.getWorkflowAction(this.data.id).subscribe(
        (res: any) => {
          this.populateForm(res.data);
        },
        (err) => {
          this.alert.error('فشلت عملية جلب بيانات الإجراء');
        },
        () => this.loaderService.stopLoading()
      ));
    }

    this.init();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  public onSave(): void {
    if (!this.form.valid) {
      return;
    }

    this.loaderService.startLoading(LoaderComponent);
    this.workflowAction = { ...this.workflowAction, ...this.form.value };
    this.workflowAction.workflowTypeId = this.workflowTypeFormService.workflowTypeView.workflowType.id!;

    let result$ = this.workflowAction.id === Guid.EMPTY
      ? this.workflowActionService.createWorkflowAction(this.workflowAction)
      : this.workflowActionService.updateWorkflowAction(this.workflowAction);

    this.subs.add(
      result$.subscribe(
        (res: any) => {
          this.loaderService.stopLoading();
          var msg = 'تم الحفظ بنجاح';

          if (res.statusCode === 201) {
            this.addWorkflowAction(res.data);
          }
          else {
            this.updateWorkflowAction(res.data);
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
        actionArName: ['', Validators.required]
      });
  }

  /**
   * Update the form values.
   * @param workflowAction workflow action used to populate the form.
   */
  private populateForm(workflowAction: WorkflowAction) {
    this.form.patchValue({
      id: workflowAction.id,
      actionArName: workflowAction.actionArName
    });
  }

  /**
   * Add workflow action to workflowActions array.
   * @param workflowAction workflow action to be added.
   */
  private addWorkflowAction(workflowAction: WorkflowAction): void {
    var newWorkflowAction = new WorkflowActionListItem();
    newWorkflowAction.id = workflowAction.id!;
    newWorkflowAction.actionArName = workflowAction.actionArName;
    newWorkflowAction.workflowTypeArName = this.workflowTypeFormService.workflowTypeView.workflowType.typeArName || '';
    this.workflowTypeFormService.workflowTypeView.workflowActions.push(newWorkflowAction);
  }

  /**
   * Update workflow action in workflowActions array.
   * @param workflowAction workflow action to be updated.
   */
  private updateWorkflowAction(workflowAction: WorkflowAction): void {
    let itemToUpdate = this.workflowTypeFormService.workflowTypeView.workflowActions.find(m => m.id === workflowAction.id);

    if (itemToUpdate) {
      itemToUpdate.actionArName = workflowAction.actionArName;
      let index = this.workflowTypeFormService.workflowTypeView.workflowActions.indexOf(itemToUpdate);
      this.workflowTypeFormService.workflowTypeView.workflowActions[index] = itemToUpdate;
    }
  }
}
