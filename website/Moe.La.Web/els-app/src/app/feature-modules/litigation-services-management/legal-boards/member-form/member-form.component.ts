import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription, Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

import { CourtService } from 'app/core/services/court.service';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { CourFormComponent } from 'app/feature-modules/administration-management/courts/court-form/court-form.component';
import { LegalBoardMemberType } from 'app/core/enums/LegalBoardMemberType';

@Component({
  selector: 'app-member-form',
  templateUrl: './member-form.component.html',
  styleUrls: ['./member-form.component.css']
})
export class MemberFormComponent implements OnInit {
  legalBoardId: any;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  consultants: any[] = [];
  filteredOptions: Observable<string[]> = new Observable<string[]>();
  isMember: boolean = true;
  isAdmin = this.authService.checkRole(AppRole.Admin);
  isHeadMemberExist: boolean = false;
  members: any[] = [];
  constructor(
    private fb: FormBuilder,
    public courtService: CourtService,
    public authService: AuthService,
    public dialogRef: MatDialogRef<CourFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.legalBoardId)
      this.legalBoardId = this.data.legalBoardId;
    if (this.data.consultants)
      this.consultants = this.data.consultants;
    this.isHeadMemberExist = this.data.isHeadMemberExist;
  }
  member = new FormControl();
  ngOnInit(): void {
    this.init();
    this.filteredOptions = this.form.controls['member'].valueChanges.pipe(
      startWith(''),
      map(value => typeof value === 'string' ? value : value.name),
      map(name => name ? this._filter(name) : this.consultants.slice())
    );
    //set error if member not selected.
    this.form.controls['member'].valueChanges.subscribe(
      value => {
        this.isMember = this.consultants.includes(value);
        const memberCtrl = this.form.get('member');
        if (!this.isMember)
          memberCtrl?.setErrors({ selectedMember: true });
      }
    );

  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  private init(): void {
    this.form = this.fb.group({
      member: ['', Validators.compose([Validators.required])],
      memberType: ["", Validators.compose([Validators.required])],

    });
  }


  displayFn(user: any): string {
    return user && user.name ? user.name : '';
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.consultants.filter((option: any) => option.name.includes(filterValue) || option.employeeNo.includes(filterValue));
  }
  onSubmit() {
    this.onCancel(this.form.value);
  }
  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
