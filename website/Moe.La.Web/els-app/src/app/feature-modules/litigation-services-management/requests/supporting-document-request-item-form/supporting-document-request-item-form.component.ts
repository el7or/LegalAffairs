import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';

@Component({
  selector: 'app-document-request-item-form',
  templateUrl: './supporting-document-request-item-form.component.html',
  styleUrls: ['./supporting-document-request-item-form.component.css']
})
export class CaseSupportingDocumentRequestItemFormComponent implements OnInit {
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  name: string = "";
  edit = false;

  constructor(
    private fb: FormBuilder,
    private alert: AlertService,
    public dialogRef: MatDialogRef<CaseSupportingDocumentRequestItemFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (data.name != null) {
      this.name = data.name;
      this.edit = true;

    }
  }

  ngOnInit() {
    this.init();
  }

  private init(): void {
    this.form = this.fb.group({
      name: [this.name, Validators.compose([Validators.required, Validators.maxLength(100)])],
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSubmit() {
    this.onCancel(this.form.value.name);
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }

}
