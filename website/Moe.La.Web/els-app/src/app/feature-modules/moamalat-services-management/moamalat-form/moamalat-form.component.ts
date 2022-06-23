import { Location } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthService } from 'app/core/services/auth.service';
import { MoamalaStatuses } from 'app/core/enums/MoamalaStatuses';
import { Attachment, GroupNames } from 'app/core/models/attachment';
import { MoamalaDetails } from 'app/core/models/moamalat';
import { AlertService } from 'app/core/services/alert.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MoamalatService } from 'app/core/services/moamalat.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AppRole } from 'app/core/models/role';
import { MinistryDepartmentService } from 'app/core/services/ministry-departments.service';

@Component({
  selector: 'app-moamalat-form',
  templateUrl: './moamalat-form.component.html',
  styleUrls: ['./moamalat-form.component.css']
})
export class MoamalatFormComponent implements OnInit, OnDestroy {
  moamalaId: number = 0;
  form: FormGroup = Object.create(null);
  confidentialDegrees: any[];
  passTypes: any[];
  departments: any[];
  moamalaDetails!: MoamalaDetails;
  attachments: Attachment[] = [];
  filesCount: number = 0;

  private subs = new Subscription();

  public get MoamalaStatuses(): typeof MoamalaStatuses {
    return MoamalaStatuses;
  }
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }

  isUserConfidentialPermission: boolean = this.authService.checkRole(AppRole.GeneralSupervisor)
    || (this.authService.checkRole(AppRole.Distributor) && this.authService.currentUser.Permission === "ConfidentialMoamla");

  constructor(private route: ActivatedRoute,
    private fb: FormBuilder,
    private moamalatService: MoamalatService,
    public ministryDepartmentService: MinistryDepartmentService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private hijriConverter: HijriConverterService,
    private authService: AuthService,
    private router: Router,
    public location: Location) {
    this.route.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.moamalaId = +id;
      }
    });
  }

  ngOnInit(): void {
    this.loaderService.startLoading(LoaderComponent);

    this.initForm();
    this.populateConfidentialDegrees();
    this.populatePassTypes();
    this.populateMinistryDepartments();

    if (this.moamalaId) { //update moamala
      this.subs.add(
        this.moamalatService.get(this.moamalaId).subscribe(
          (result: any) => {
            this.moamalaDetails = result.data;
            this.onAttachFiles(this.moamalaDetails.attachments);
            this.patchForm(this.moamalaDetails);
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
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  initForm() {
    this.form = this.fb.group({
      id: [this.moamalaId],
      unifiedNo: [null, Validators.compose([Validators.maxLength(20)])],
      moamalaNumber: [null, Validators.compose([Validators.required, Validators.maxLength(20)])],
      confidentialDegree: [{
        value: (!this.isUserConfidentialPermission ? 1 : ""),
        disabled: (!this.isUserConfidentialPermission ? true : "")
      },
      Validators.compose([Validators.required])],
      subject: [null, Validators.compose([Validators.required, Validators.maxLength(100)])],
      description: [null, Validators.compose([Validators.required, Validators.maxLength(2000)])],
      passType: [{ value: 1, disabled: true }, Validators.compose([Validators.required])],
      passDate: [null, Validators.compose([Validators.required])],
      senderDepartmentId: [""],
      status: [MoamalaStatuses.New],
      attachments: [null],
      isManual: [true]
    });
  }
  patchForm(moamalaDetails: MoamalaDetails) {
    this.form.patchValue({
      unifiedNo: moamalaDetails.unifiedNo,
      moamalaNumber: moamalaDetails.moamalaNumber,
      confidentialDegree: moamalaDetails.confidentialDegree?.id,
      subject: moamalaDetails.subject,
      description: moamalaDetails.description.replace("<br>", "\n"),
      passType: moamalaDetails.passType?.id,
      passDate: moamalaDetails.passDate,
      senderDepartmentId: moamalaDetails.senderDepartment?.id,
      status: moamalaDetails.status?.id,
      attachments: this.attachments,
    });
  }

  populateConfidentialDegrees() {
    if (!this.confidentialDegrees) {
      this.subs.add(
        this.moamalatService.getConfidentialDegrees().subscribe(
          (result: any) => {
            this.confidentialDegrees = result;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  populatePassTypes() {
    if (!this.passTypes) {
      this.subs.add(
        this.moamalatService.getPassTypes().subscribe(
          (result: any) => {
            this.passTypes = result;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  populateMinistryDepartments() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.ministryDepartmentService.getWithQuery({
        sortBy: 'name',
        isSortAscending: true,
        page: 1,
        pageSize: 99,
      }).subscribe(
        (result: any) => {
          this.departments = result.data.items;
          if (!this.moamalaId) this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  onAttachFiles(list: any[]) {
    this.attachments = list;
        this.filesCount = this.attachments.filter(a => !a.isDeleted).length;
    this.form.controls['attachments'].setValue(this.attachments);
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    this.form.controls['confidentialDegree'].enable();
    this.form.controls['passType'].enable();
    this.form.value.passDate = this.hijriConverter.calendarDateToDate(
      this.form.get('passDate')?.value?.calendarStart);

    var result$ = (this.moamalaId) ? this.moamalatService.update(this.form.value) : this.moamalatService.create(this.form.value);
    this.subs.add(
      result$.subscribe(() => {
        this.loaderService.stopLoading();
        let message = this.moamalaId
          ? 'تمت عملية التعديل بنجاح'
          : 'تمت عملية الإضافة بنجاح';
        this.alert.succuss(message);
        this.router.navigateByUrl('/moamalat');
      },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          let message = this.moamalaId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
        }
      )
    );

  }
}
