import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { InvestigationRecordAttendant } from 'app/core/models/investigation-record';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';
import { AlertService } from 'app/core/services/alert.service';
import { FaresService } from 'app/core/services/fares.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';

@Component({
  selector: 'app-attendants-form',
  templateUrl: './attendants-form.component.html',
  styleUrls: ['./attendants-form.component.css']
})
export class AttendantFormComponent implements OnInit {

  form: FormGroup = Object.create(null);
  subs = new Subscription();

  attendant: InvestigationRecordAttendant = new InvestigationRecordAttendant();
  editMode: boolean = false;

  ministryDepartments: KeyValuePairs[] = [];
  filteredMinistryDepartments!: KeyValuePairs[] | undefined;
  department: number | undefined;

  constructor(
    private fb: FormBuilder,
    private ministryDepartmentService: MinistryDepartmentService,
    private faresService: FaresService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<AttendantFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data?.attendant) {
      Object.assign(this.attendant, this.data?.attendant);

      this.editMode = true;
    }
  }

  ngOnInit() {
    this.init();
    ///
    this.populateMoeDepartments();
    ///
    if (this.editMode) {
      this.patchForm(this.attendant)
    }

    ///
    this.form.controls['representativeOf'].valueChanges.subscribe(
      value => {
        if (!value?.id)
          this.form.get('representativeOf')?.setErrors({ selectedValue: true });
      }
    );
    ///
    this.form.controls['identityNumber'].valueChanges.subscribe(
      value => {
        this.attendant = new InvestigationRecordAttendant();
        if (this.form.controls['identityNumber'].value.length == 10) { this.onSearch(); }
      }
    );

  }
  onSearch() {
    let identityNumber = this.form.get('identityNumber')?.value;
    this.attendant = new InvestigationRecordAttendant();
    this.getUserData(identityNumber);

  }
  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      identityNumber: [{ value: null, disabled: this.editMode }, Validators.compose([Validators.required, Validators.minLength(10), Validators.pattern("^[0-9]*$")])],
      representativeOf: [null, Validators.compose([Validators.required])],
      details: [null]
    });
  }

  private patchForm(attendant: InvestigationRecordAttendant): void {
    this.form.patchValue({
      identityNumber: attendant.identityNumber,
      representativeOf: attendant.representativeOf,
      details: attendant.details
    });

  }

  populateMoeDepartments() {
    this.subs.add(
      this.ministryDepartmentService.getAll().subscribe((result: any) => {
        if (result.isSuccess) {
          this.ministryDepartments = result.data.items;
          this.filteredMinistryDepartments = result.data.items;
        }
      }, (error) => {
        this.alert.error("فشلت  عملية جلب البيانات");
        console.error(error);
      }));
  }
  filterMinistryDepartments() {
    let filterValue = this.form.get('representativeOf')?.value?.toLowerCase();
    if (filterValue) {
      this.filteredMinistryDepartments = this.ministryDepartments.filter(department => (department.name).toLowerCase().includes(filterValue));
    } else {
      this.filteredMinistryDepartments = this.ministryDepartments;
    }
  }

  displayFn(ministryDepartment?: KeyValuePairs): string | undefined {
    return ministryDepartment ? ministryDepartment.name : '';
  }

  getUserData(identityNumber: string) {
    this.subs.add(
      this.faresService.getUser(identityNumber).subscribe((result: any) => {
        if (result.data) {
          this.attendant.assignedWork = result.data.assignedWork;
          this.attendant.fullName = result.data.name;
          this.attendant.workLocation = result.data.workLocation;
        }
        else {
          // add error to identityNumber if user not exists
          this.form.get('identityNumber')?.setErrors({ notExists: true });
        }
      }, (error) => {
        this.alert.error("فشلت  عملية جلب البيانات");
        console.error(error);
      }));
  }
  onSubmit() {
    this.attendant.identityNumber = this.form.value.identityNumber;
    this.attendant.representativeOf = this.form.value.representativeOf;
    this.attendant.representativeOfId = this.form.value.representativeOf.id;

    this.attendant.details = this.form.value.details;

    this.onCancel(this.attendant)

  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
