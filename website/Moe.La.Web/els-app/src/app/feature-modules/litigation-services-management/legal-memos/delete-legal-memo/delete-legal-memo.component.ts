import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';

import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LegalMemoService } from 'app/core/services/legal-memo.service';

@Component({
  selector: 'app-delete-legal-memo',
  templateUrl: './delete-legal-memo.component.html',
  styleUrls: ['./delete-legal-memo.component.css']
})
export class DeleteLegalMemoComponent implements OnInit {
  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  memoId: number = 0;
  public Editor = CustomEditor;
  public config = {
    language: 'ar'
  };
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<DeleteLegalMemoComponent>,
    private alert: AlertService,
    private legalMemoService: LegalMemoService,
    private loaderService: LoaderService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.memoId) this.memoId = this.data.memoId;

  }

  ngOnInit(): void {
    this.init();
  }

  private init(): void {
    this.form = this.fb.group({
      id: [this.memoId],
      deletionReason: ['', Validators.compose([Validators.required])],
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalMemoService.deleteMemo(this.form.value).subscribe(result => {
        this.alert.succuss("تم حذف المذكرة بنجاح");
        this.loaderService.stopLoading();
        this.onCancel(result);
        this.router.navigate(['/memos/list']);
      }, error => {
        this.loaderService.stopLoading();
        this.alert.error("فشلت عملية حذف المذكرة");
        this.onCancel();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

  ngDestroy() {
    this.subs.unsubscribe();
  }

}

