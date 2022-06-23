import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { LegalBoardMembersListItem } from 'app/core/models/legal-board';
import { AlertService } from 'app/core/services/alert.service';
import { DateTimeService } from 'app/core/services/date-time.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { BoardMeeting } from 'app/core/models/board-meeting';

@Component({
  selector: 'app-board-meeting',
  templateUrl: './board-meeting.component.html',
  styleUrls: ['./board-meeting.component.css']
})
export class BoardMeetingComponent implements OnInit {
  boardMeeting = new BoardMeeting();
  form: FormGroup = Object.create(null);
  legalBoardMembers!: LegalBoardMembersListItem[];
  isSelectedAll: boolean = false;

  subs = new Subscription();

  constructor(private fb: FormBuilder,
    public legalBoardService: LegalBoardService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<BoardMeetingComponent>,
    private loaderService: LoaderService,
    private hijriConverter: HijriConverterService,
    private dateTimeService: DateTimeService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    if (this.data.boardMeeting) {
      this.boardMeeting.legalMemoId = this.data.boardMeeting.legalMemoId;
      this.boardMeeting.legalBoardId = this.data.boardMeeting.legalBoardId;
    }
  }

  ngOnInit(): void {
    this.initForm();
    this.populateMembers();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  private initForm(): void {
    this.form = this.fb.group({
      id: [null],
      memoId: [this.boardMeeting.legalMemoId],
      BoardId: [this.boardMeeting.legalBoardId],
      meetingPlace: [null, Validators.compose([Validators.required])],
      meetingDate: [null, Validators.compose([Validators.required])],
      meetingTime: [null, Validators.compose([Validators.required])],
      boardMeetingMembers: [null, Validators.compose([Validators.required])]
    });
  }

  populateMembers() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalBoardService.getLegalBoardMembers(this.boardMeeting.legalBoardId).subscribe(
        (res: any) => {
          this.legalBoardMembers = res.data;
          this.loaderService.stopLoading();
          this.populateBoardMeetingDetails();

        },
        (error) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب البيانات !');
          console.error(error);
        }
      )
    );
  }

  populateBoardMeetingDetails() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalBoardService.getMeetingByBoardAndMemo(this.boardMeeting.legalBoardId, this.boardMeeting.legalMemoId).subscribe(
        (res: any) => {
          this.loaderService.stopLoading();
          if (res.data) {
            let meetingDetails = res.data;
            this.boardMeeting.boardMeetingId = meetingDetails.id;

            this.form.patchValue({
              id: meetingDetails.id,
              meetingPlace: meetingDetails.meetingPlace,
              meetingDate: meetingDetails.meetingDate,
              meetingTime: meetingDetails.meetingDate
            });
            this.legalBoardMembers.map(member => {
              if (meetingDetails.boardMeetingMembersIds.find(m => m == member.id)) {
                member.isSelected = true;
              }
            });
            this.onSelectMember();
          }
        },
        (error) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب البيانات !');
          console.error(error);
        }
      )
    );
  }

  onSelectAll(isChecked: boolean) {
    this.legalBoardMembers.forEach(member => member.isSelected = isChecked);
    this.onSelectMember();
  }

  onSelectMember() {
    if (this.legalBoardMembers.find(m => m.isSelected == true)) {
      this.form.get('boardMeetingMembers')?.clearValidators();
      this.form.get('boardMeetingMembers')?.updateValueAndValidity();
    } else {
      this.form.get('boardMeetingMembers')?.setValidators(Validators.required);
      this.form.get('boardMeetingMembers')?.updateValueAndValidity();
    }
    this.checkSelectAll();
  }

  checkSelectAll() {
    this.isSelectedAll = !this.legalBoardMembers.find(m => m.isSelected != true) ?
      true : false;
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    const legalBoardMembersIds: number[] = this.legalBoardMembers
      .filter(member => member.isSelected == true)
      .map(member => { return member.id });
    const meetingDate = new Date(this.hijriConverter.calendarDateToDate(this.form.get('meetingDate')?.value?.calendarStart));
    this.form.patchValue({
      meetingDate: this.dateTimeService.mergDateTime(meetingDate, this.form.get('meetingTime')?.value),
      boardMeetingMembers: legalBoardMembersIds,
    });
    let result$ = this.boardMeeting.boardMeetingId ? this.legalBoardService.updateMeeting(this.form.value) : this.legalBoardService.createMeeting(this.form.value);
    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          this.alert.succuss('تم اختيار الأعضاء بنجاح');
        },
        (error) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية اختيار الأعضاء !');
          console.error(error);
          this.onCancel();
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
