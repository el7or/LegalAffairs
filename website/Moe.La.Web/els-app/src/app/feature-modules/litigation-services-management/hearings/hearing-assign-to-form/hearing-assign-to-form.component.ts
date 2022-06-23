import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import { AlertService } from 'app/core/services/alert.service';
import { HearingService } from 'app/core/services/hearing.service';
import { LoaderService } from 'app/core/services/loader.service';
import { UserService } from 'app/core/services/user.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { ResearcherQueryObject } from 'app/core/models/query-objects';
import { UserDetails } from 'app/core/models/user';
import { ResearcherConsultantService } from 'app/core/services/researcher-consultant.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';

@Component({
  selector: 'app-hearing-assign-to-form',
  templateUrl: './hearing-assign-to-form.component.html',
  styleUrls: ['./hearing-assign-to-form.component.css'],
})
export class HearingAssignToFormComponent implements OnInit {
  hearingId!: number;
  attendantId?: string;
 

  researchers: KeyValuePairs[] = [];

  allConsultantsAndResearchers!: UserDetails[];
  filteredConsultantsAndResearchers!: UserDetails[];

  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public userService: UserService,
    private hearingService: HearingService,
    private alert: AlertService,
    public researcherConsultantService: ResearcherConsultantService,
    public dialogRef: MatDialogRef<HearingAssignToFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.hearingId = this.data.hearingId;
    this.attendantId = this.data.attendantId;
  }

  ngOnInit(): void {
    this.initForm();
    this.populateConsultantsAndResearchers();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  private initForm(): void {
    this.form = this.fb.group({
      attendant: [null, Validators.compose([Validators.required])],
    });
  }

  populateConsultantsAndResearchers() {
    this.loaderService.startLoading(LoaderComponent);

    let queryObject: ResearcherQueryObject = new ResearcherQueryObject({
      sortBy: 'researcher',
      pageSize: 999,
      hasConsultant: true
    });

    this.subs.add(
      this.researcherConsultantService.getWithQuery(queryObject).subscribe(
        (result: any) => {

          this.researchers = result.data.items
            .filter((m: any) => m.researcherId != this.attendantId) // we will remove the current resarcher from the list
            .map((researcher: any) => {
              return {
                id: researcher.researcherId,
                name: researcher.researcher
              };
            });

          // let res: UserDetails[] = result.data.items;
          // if (this.attendantId) {
          //   res = res.filter((user) => user.id !== this.attendantId);
          // }
          // this.allConsultantsAndResearchers = this.filteredConsultantsAndResearchers = res;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  // filterConsultantsAndResearchers() {
  //   let filterValue = this.form.get('attendant')?.value?.toLowerCase();
  //   if (filterValue) {
  //     this.filteredConsultantsAndResearchers = this.allConsultantsAndResearchers.filter(
  //       (attendant) =>
  //         (attendant.firstName + ' ' + attendant.lastName)
  //           .toLowerCase()
  //           .includes(filterValue)
  //     );
  //   } else {
  //     this.filteredConsultantsAndResearchers = this.allConsultantsAndResearchers;
  //   }
  // }

  // displayFn(attendant?: UserDetails): string | undefined {
  //   return attendant ? attendant.firstName + ' ' + attendant.lastName : '';
  // }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.hearingService
        .assignHearingTo(this.hearingId, this.form.get('attendant')?.value)
        .subscribe(
          (res: any) => {
            this.loaderService.stopLoading();
            this.dialogRef.close(res.data);
            this.alert.succuss('تم إسناد التكليف بنجاح');
          },
          (error: any) => {
            console.error(error);
            this.loaderService.stopLoading();
            this.alert.error('فشلت عملية إسناد التكليف !');
          }
        )
    );
  }

  onCancel() {
    this.dialogRef.close();
  }
}
