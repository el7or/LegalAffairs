import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { WorkflowStatusService } from './services/workflow-status-service';
import { WorkflowStatus } from 'app/core/models/workflow-status';

@Component({
  selector: 'app-workflow-status-form',
  templateUrl: './workflow-status-form.component.html',
  styleUrls: ['./workflow-status-form.component.css']
})
export class WorkflowStatusFormComponent implements OnInit, OnDestroy {
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  workflowStatus = new WorkflowStatus();

  constructor(private fb: FormBuilder,
    private workflowStatusesService: WorkflowStatusService,
    private loaderService: LoaderService,
    private router: Router,
    private alert: AlertService) { }

  ngOnInit(): void {
    this.init();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  public onSave(): void {
    this.loaderService.startLoading(LoaderComponent);

    if (!this.form.valid) {
      return;
    }

    this.workflowStatus = { ...this.workflowStatus, ...this.form.value };

    this.subs.add(
      this.workflowStatusesService.createWorkflowStatus(this.workflowStatus).subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.alert.succuss('تم الحفظ بنجاح');
          this.router.navigateByUrl('/workflow-settings/workflow-types');
        },
        (err) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية الحفظ');
        })
    );
  }

  private init(): void {
    this.form = this.fb.group(
      {
        statusArName: ['', Validators.required],
        statusEnName: ['', Validators.required]
      });
  }
}
