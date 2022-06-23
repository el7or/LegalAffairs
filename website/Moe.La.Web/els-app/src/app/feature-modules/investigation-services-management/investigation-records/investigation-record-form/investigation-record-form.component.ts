import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { InvestigationService } from 'app/core/services/investigation.service';
import { InvestigationRecordService } from 'app/core/services/investigation-record.service';
import { InvestigationRecordAttendant, InvestigationRecordDetails, InvestigationRecordParty, InvestigationRecordQuestion, SaveInvestigationRecord } from '../../../../core/models/investigation-record';
import { Attachment, GroupNames } from 'app/core/models/attachment';
import { QueryObject } from 'app/core/models/query-objects';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RecordStatuses } from 'app/core/enums/RecordStatuses';
import { UserService } from 'app/core/services/user.service';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-investigation-record-form',
  templateUrl: './investigation-record-form.component.html',
  styleUrls: ['./investigation-record-form.component.css']
})
export class InvestigationRecordFormComponent implements OnInit {

  private subs = new Subscription();
  queryObject = new QueryObject({ pageSize: 99999 });

  //
  form: FormGroup = Object.create(null);
  saveInvestigationRecord: SaveInvestigationRecord = new SaveInvestigationRecord();

  //routing parameters
  recordId!: number;
  investigationId!: number;

  // investigation record number
  investigation: any;
  investigationRecordNumber: string = '';

  // external components lists 
  attendants: InvestigationRecordAttendant[] = [];
  parties: InvestigationRecordParty[] = [];
  attachments: Attachment[] = [];
  questions: InvestigationRecordQuestion[] = [];

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  constructor(
    private activatedRouter: ActivatedRoute,
    private fb: FormBuilder,
    private alert: AlertService,
    private router: Router,
    private loaderService: LoaderService,
    public userService: UserService,
    public authService: AuthService,
    public location: Location,
    private investigationService: InvestigationService,
    private investigationRecordService: InvestigationRecordService
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var investigationId = params.get('investigationId');
      if (investigationId != null) {
        this.investigationId = +investigationId;
      }
      var id = params.get('id');
      if (id != null) {
        this.recordId = +id;
      }
    });
  }

  ngOnInit() {

    this.init();
    /// to set new investigation record number 
    if (this.investigationId) {
      this.populateInvestigation();
    }

    /// get investigation record for edit
    if (this.recordId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.investigationRecordService.get(this.recordId).subscribe((result: any) => {
          this.investigationRecordNumber = result.data.recordNumber;
          this.patchForm(result.data);
          this.loaderService.stopLoading();
        })
      );
    }
  }
  private patchForm(record: InvestigationRecordDetails): void {
    // patch list components values
    this.attachments = record.attachments;
    this.attendants = record.attendants;
    this.parties = record.investigationRecordParties;
    this.questions = record.investigationRecordQuestions;

    this.form.patchValue({
      id: record.id,
      investigationId: record.investigationId,
      recordDate: record.startDate,
      startTime: record.startDate,
      endTime: record.endDate,
      visuals: record.visuals,
      recordStatus: record.recordStatus,
      isRemote: record.isRemote,
      attachments: record.attachments,
      investigationRecordParties: record.investigationRecordParties,
      investigationRecordQuestions: record.investigationRecordQuestions,
      investigationRecordInvestigators: record.investigationRecordInvestigators.map(i => i.investigatorId),
      attendants: record.attendants
    });
  }
  private init() {
    this.form = this.fb.group({
      id: [0],
      investigationId: [this.investigationId, Validators.compose([Validators.required])],
      recordDate: [new Date(), Validators.compose([Validators.required])],
      startTime: [new Date(), Validators.compose([Validators.required])],
      endTime: [new Date(), Validators.compose([Validators.required])],
      visuals: [null],
      recordStatus: [RecordStatuses.Draft, Validators.compose([Validators.required])],
      isRemote: [false, Validators.compose([Validators.required])],
      attachments: [null],
      investigationRecordParties: [null, Validators.compose([Validators.required])],
      investigationRecordQuestions: [null, Validators.compose([Validators.required])],
      investigationRecordInvestigators: [[this.authService.currentUser?.id], Validators.compose([Validators.required])],
      attendants: [null, Validators.compose([Validators.required])]
    });
  }

  populateInvestigation() {
    this.subs.add(
      this.investigationService.get(this.investigationId).subscribe(
        (result: any) => {
          this.investigation = result.data;
          if (this.investigation?.investigationNumber)
            this.investigationRecordNumber = `${this.investigation.investigationNumber}${this.setInvestigationRecordNumber(this.investigation.investigationRecords.length + 1, 3)}`;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );

  }
  setInvestigationRecordNumber(investigationRecords: number, length: number) {
    var len = length - ('' + investigationRecords).length;
    return (len > 0 ? new Array(++len).join('0') : '') + investigationRecords
  }

  // attachments actions
  onRecordAttachment(list: Attachment[]) {
    this.attachments = list;
    this.form.controls['attachments'].setValue(this.attachments);
  }

  // attendants actions
  onRecordAttendant(list: InvestigationRecordAttendant[]) {
    this.attendants = list;
    this.form.controls['attendants'].setValue(list);
  }

  // parties actions
  onUpdateParties(list: InvestigationRecordParty[]) {
    this.parties = list;
    this.form.controls['investigationRecordParties'].setValue(list);
  }

  // questions actions
  onUpdateQuestions(list: InvestigationRecordQuestion[]) {
    this.questions = list;
    this.form.controls['investigationRecordQuestions'].setValue(list);
  }

  // submit form
  onSubmit() {

    Object.assign(this.saveInvestigationRecord, this.form.value);
    // set start & end dates
    this.saveInvestigationRecord.startDate = this.createDate(this.form.get('recordDate')?.value?.calendarStart, this.form.value.startTime);
    this.saveInvestigationRecord.endDate = this.createDate(this.form.get('recordDate')?.value?.calendarStart, this.form.value.endTime);

    this.saveInvestigationRecord.recordNumber = this.investigationRecordNumber;

    this.loaderService.startLoading(LoaderComponent);
    if (this.recordId) {
      this.subs.add(
        this.investigationRecordService.update(this.saveInvestigationRecord).subscribe(
          (result: any) => {
            this.loaderService.stopLoading();
            this.router.navigateByUrl('/investigation-records');
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }
    else {
      this.subs.add(
        this.investigationRecordService.create(this.saveInvestigationRecord).subscribe(
          (result: any) => {
            this.loaderService.stopLoading();
            this.router.navigateByUrl('/investigation-records');
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }
  }

  createDate(date: any, dateTime: Date) {
    return new Date(date.year, date.month, date.day, dateTime.getHours(), dateTime.getMinutes(), dateTime.getSeconds());
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }


}
