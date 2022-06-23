import { Component, OnInit, AfterViewInit, ViewChild, Input } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatStepper } from '@angular/material/stepper';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { WorkflowStepListItem } from 'app/core/models/workflow-step';
import { WorkflowStepFormComponent } from '../workflow-step-form/workflow-step-form.component';
import { WorkflowTypeFormService } from '../../services/workflow-type-form.service';

@Component({
  selector: 'app-workflow-step-list',
  templateUrl: './workflow-step-list.component.html',
  styleUrls: ['./workflow-step-list.component.css']
})
export class WorkflowStepListComponent implements OnInit, AfterViewInit {
  @Input() stepper: MatStepper = Object.create(null);

  displayedColumns: string[] = ['position', 'stepArName', 'workflowStepCategoryArName', 'id', 'actions'];
  dataSource = new MatTableDataSource<WorkflowStepListItem>();
  subs = new Subscription();

  @ViewChild(MatPaginator) paginator: MatPaginator = Object.create(null);
  @ViewChild(MatSort) sort: MatSort = Object.create(null);

  constructor(private workflowTypeFormService: WorkflowTypeFormService,
    private dialog: MatDialog,
    private alert: AlertService) { }

  ngOnInit(): void {
    this.populateTable();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onNext(): void {
    if (!this.workflowTypeFormService.workflowTypeView.workflowSteps.length) {
      this.alert.error('يرجى إدخال الخطوات.');
      return;
    }

    this.stepper.next();
  }

  onPrevious(): void {
    this.stepper.previous();
  }

  onAdd(): void {
    const dialogRef = this.dialog.open(WorkflowStepFormComponent, {
      width: '60em'
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

  onUpdate(workflowStepId: string): void {
    const dialogRef = this.dialog.open(WorkflowStepFormComponent, {
      width: '60em',
      data: { id: workflowStepId }
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
    if (this.workflowTypeFormService.workflowTypeView.workflowSteps) {
      this.dataSource = [...this.workflowTypeFormService.workflowTypeView.workflowSteps] as any;
    }
  }
}
