import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { LegalMemoService } from 'app/core/services/legal-memo.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { LegalBoardMemo } from 'app/core/models/legal-board-memo';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';

@Component({
  selector: 'app-assign-memo-to-board-form',
  templateUrl: './assign-memo-to-board-form.component.html',
  styleUrls: ['./assign-memo-to-board-form.component.css']
})

export class AssignMemoToBoardFormComponent implements OnInit, OnDestroy {

  legalBoardMemo = new LegalBoardMemo();
  legalBoards: any;
  form: FormGroup = Object.create(null);
  subs = new Subscription();

  public get LegalMemoStatus(): typeof LegalMemoStatus {
    return LegalMemoStatus;
  }

  constructor(
    private fb: FormBuilder,
    public boardService: LegalBoardService,
    private legalMemoService: LegalMemoService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<AssignMemoToBoardFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.legalBoardMemo) {
      this.legalBoardMemo.id = this.data.legalBoardMemo.id;
      this.legalBoardMemo.legalMemoId = this.data.legalBoardMemo.legalMemoId;
      this.legalBoardMemo.legalBoardId = this.data.legalBoardMemo.legalBoardId;
    }

  }

  ngOnInit() {
    this.init();
    ///
    if (this.legalBoardMemo.legalMemoId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.boardService.getLegalBoard().subscribe(
          (result: any) => {
            this.legalBoards = result.data;
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
      id: this.legalBoardMemo.id,
      legalBoardId: ["", Validators.compose([Validators.required])],
      legalMemoId: this.legalBoardMemo.legalMemoId,
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = (this.legalBoardMemo.id) ? this.boardService.updateLegalBoardMemo(this.form.value) : this.boardService.createLegalBoardMemo(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.changeLgalMemoStatus(this.legalBoardMemo.legalMemoId, LegalMemoStatus.RaisingSubBoardHead);
          this.onCancel(res);

        },
        (error) => {
          this.loaderService.stopLoading();
          let message = 'فشلت عملية الإضافة !';
          this.alert.error(message);
          console.error(error);
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
  changeLgalMemoStatus(legalMemoId: number, legalMemoStatusId: number) {

    this.subs.add(this.legalMemoService.changeLegalMemoStatus(legalMemoId, legalMemoStatusId)
      .subscribe((res: any) => {
        this.alert.succuss("تمت إحالة المذكرة إلى لجنة فرعية.");
      }, (error) => {
        this.alert.error("فشلت عملية التغيير");
        this.loaderService.stopLoading();
        console.error(error);
      }))
  }
}
