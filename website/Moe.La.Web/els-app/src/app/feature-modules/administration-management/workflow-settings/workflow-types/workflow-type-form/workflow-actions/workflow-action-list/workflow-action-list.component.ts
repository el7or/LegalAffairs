import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatStepper } from '@angular/material/stepper';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { WorkflowActionListItem } from 'app/core/models/workflow-action';
import { AlertService } from 'app/core/services/alert.service';
import { WorkflowActionFormComponent } from '../workflow-action-form/workflow-action-form.component';
import { WorkflowTypeFormService } from '../../services/workflow-type-form.service';

@Component({
  selector: 'app-workflow-action-list',
  templateUrl: './workflow-action-list.component.html',
  styleUrls: ['./workflow-action-list.component.css']
})
export class WorkflowActionListComponent implements OnInit, OnDestroy {
  @Input() stepper: MatStepper = Object.create(null);

  displayedColumns: string[] = ['position', 'actionArName', 'id', 'actions'];
  dataSource = new MatTableDataSource<WorkflowActionListItem>();
  subs = new Subscription();

  constructor(private workflowTypeFormService: WorkflowTypeFormService,
    private dialog: MatDialog,
    private alert: AlertService) { }

  ngOnInit(): void {
    this.populateTable();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onNext(): void {
    if (!this.workflowTypeFormService.workflowTypeView.workflowActions.length) {
      this.alert.error('يرجى إدخال الإجراءات.');
      return;
    }

    this.stepper.next();
  }

  onPrevious(): void {
    this.stepper.previous();
  }

  onAdd(): void {
    const dialogRef = this.dialog.open(WorkflowActionFormComponent, {
      width: '30em'
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          this.populateTable();
        },
        (error) => {
          this.alert.error(error);
        }
      )
    );
  }

  onUpdate(workflowActionId: string): void {
    const dialogRef = this.dialog.open(WorkflowActionFormComponent, {
      width: '30em',
      data: { id: workflowActionId }
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          this.populateTable();
        },
        (error) => {
          this.alert.error(error);
        }
      )
    );
  }

  /**
   * Populate the Table
   */
  private populateTable(): void {
    if (this.workflowTypeFormService.workflowTypeView.workflowActions) {
      this.dataSource = [...this.workflowTypeFormService.workflowTypeView.workflowActions] as any;
    }
  }
}
