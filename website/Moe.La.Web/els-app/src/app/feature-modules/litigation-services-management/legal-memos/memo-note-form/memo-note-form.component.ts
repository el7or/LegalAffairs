import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LegalMemoNote, LegalMemoNoteList } from 'app/core/models/legal-memo-note';
import { LegalMemoService } from 'app/core/services/legal-memo.service';

@Component({
  selector: 'app-memo-note-form',
  templateUrl: './memo-note-form.component.html',
  styleUrls: ['./memo-note-form.component.css']
})
export class MemoNoteFormComponent implements OnInit, OnDestroy {

  legalMemoNote: LegalMemoNote = new LegalMemoNote();

  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public legalMemoService: LegalMemoService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<MemoNoteFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.legalMemoNote) {
      this.legalMemoNote.id = this.data.legalMemoNote.id ?? 0;
      this.legalMemoNote.legalMemoId = this.data.legalMemoNote.legalMemoId;
      this.legalMemoNote.legalBoardId = this.data.legalMemoNote.legalBoardId;
      this.legalMemoNote.reviewNumber = this.data.legalMemoNote.reviewNumber;
    }
  }

  ngOnInit() {
    this.init();
    ///
    if (this.legalMemoNote.id) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.legalMemoService.getNote(this.legalMemoNote.id).subscribe(
          (result: any) => {
            const legalMemoNoteList: LegalMemoNoteList = result.data;
            this.form.patchValue({
              id: legalMemoNoteList?.id,
              legalMemoId: legalMemoNoteList?.legalMemoId,
              text: legalMemoNoteList?.text,
              reviewNumber: legalMemoNoteList?.reviewNumber
            });

            this.loaderService.stopLoading();
          },
          (error) => {
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
            console.error(error);
          }
        )
      );
    }
  }

  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      text: [null, Validators.compose([Validators.required])],
      legalMemoId: this.legalMemoNote.legalMemoId,
      legalBoardId: this.legalMemoNote.legalBoardId,
      reviewNumber: this.legalMemoNote.reviewNumber
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.legalMemoNote.id
      ? this.legalMemoService.updateNote(this.form.value)
      : this.legalMemoService.createNote(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.legalMemoNote.id
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          let message = this.legalMemoNote.id
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
          console.error(error);
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
