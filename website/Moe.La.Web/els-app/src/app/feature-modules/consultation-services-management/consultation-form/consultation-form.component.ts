import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Subscription } from 'rxjs/Rx';
import { RxwebValidators } from '@rxweb/reactive-form-validators';

import { ConsultationStatus } from 'app/core/enums/ConsultationStatus';
import { MoamalaDetails, MoamalatListItem } from 'app/core/models/moamalat';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MoamalatService } from 'app/core/services/moamalat.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { ConsultationService } from 'app/core/services/consultation.service';
import { ConsultationDetails } from 'app/core/models/consultation';
import { ConsultationSupportingDocument } from 'app/core/models/consultation-request';

@Component({
  selector: 'app-consultation-form',
  templateUrl: './consultation-form.component.html',
  styleUrls: ['./consultation-form.component.css']
})
export class ConsultationFormComponent implements OnInit, OnDestroy {

  moamalaId: number = 0;
  consultationId: number = 0;
  consultationDetails!: ConsultationDetails;
  moamala!: MoamalaDetails;
  relatedMoamalatDataSource!: MatTableDataSource<MoamalatListItem>;
  consultationSupportingDocument: ConsultationSupportingDocument = new ConsultationSupportingDocument();
  form: FormGroup = Object.create(null);
  private subs = new Subscription();

  consultationMeritsDisplayedColumns: string[] = ['position', 'text', 'actions'];
  consultationMeritsDataSource = new BehaviorSubject<AbstractControl[]>([]);

  consultationGroundsDisplayedColumns: string[] = ['position', 'text', 'actions'];
  consultationGroundsDataSource = new BehaviorSubject<AbstractControl[]>([]);

  public get ConsultationStatus(): typeof ConsultationStatus {
    return ConsultationStatus;
  }

  constructor(
    private consultationService: ConsultationService,
    private moamalatService: MoamalatService,
    private loaderService: LoaderService,
    private alert: AlertService,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public location: Location) {
    this.activatedRoute.paramMap.subscribe((params) => {
      // is new //
      var moamalaId = params.get('moamalaId');
      if (moamalaId != null) {
        this.moamalaId = +moamalaId;
      }
      // is update //
      var id = params.get('id');
      if (id != null) {
        this.consultationId = +id;
      }
    }
    );
  }

  ngOnInit(): void {
    this.init();
    ////

    this.loaderService.startLoading(LoaderComponent);
    // new consultation //
    if (this.moamalaId) {
      this.getMoamalaDetails(this.moamalaId);
    }
    // update consultation //
    if (this.consultationId) {
      this.subs.add(
        this.consultationService.get(this.consultationId).subscribe(
          (result: any) => {
            this.consultationDetails = result.data;
            this.getMoamalaDetails(this.consultationDetails.moamalaId);
            this.patchForm(this.consultationDetails);
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

  init() {
    this.form = this.fb.group({
      id: [this.consultationId],
      moamalaId: [this.moamalaId],
      subject: [null, Validators.compose([Validators.required])],
      legalAnalysis: [null, Validators.compose([Validators.required])],
      status: [ConsultationStatus.Draft],
      branchId: [null],
      departmentId: [null],
      consultationMerits: this.fb.array([this.consultationMerits]),
      consultationGrounds: this.fb.array([this.consultationGrounds]),
    });

    this.consultationMeritsDataSource.next(this.getConsultationMeritsControls());
    this.consultationGroundsDataSource.next(this.getConsultationGroundsControls());
  }

  patchForm(consultationDetails: ConsultationDetails) {

    this.form.patchValue({
      id: consultationDetails.id,
      moamalaId: consultationDetails.moamalaId,
      status: consultationDetails.status.id,
      subject: consultationDetails.subject,
      legalAnalysis: consultationDetails.legalAnalysis,
      branchId: consultationDetails.branch?.id,
      departmentId: consultationDetails.department?.id,
    });
    this.moamalaId = consultationDetails.moamalaId;

    //delete init empty of consultation merits
    this.deleteConsultationMerits(0);
    // to patch form array //   
    consultationDetails.consultationMerits.forEach((item) => {
      (this.form.get("consultationMerits") as FormArray).push(this.fb.group({
        text: item.text
      }));
    });
    this.consultationMeritsDataSource.next(this.getConsultationMeritsControls());

    //delete init empty of consultation grounds
    this.deleteConsultationGrounds(0);
    // to patch form array //   
    consultationDetails.consultationGrounds.forEach((item) => {
      (this.form.get("consultationGrounds") as FormArray).push(this.fb.group({
        text: item.text
      }));
    });
    this.consultationGroundsDataSource.next(this.getConsultationGroundsControls());

    this.consultationSupportingDocument = consultationDetails.consultationSupportingDocuments[0];
  }

  getMoamalaDetails(moamalaId: number) {
    this.subs.add(
      this.moamalatService.get(moamalaId).subscribe(
        (result: any) => {
          this.moamala = result.data;
          this.relatedMoamalatDataSource = new MatTableDataSource(result.data.relatedMoamalat);
          if (!this.consultationId) // new consultation
            this.form.patchValue({
              branchId: this.moamala?.branch ? this.moamala?.branch.id : 1,
              departmentId: this.moamala?.senderDepartment ? this.moamala?.senderDepartment?.id : null,
            });
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت  عملية جلب البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  get consultationMerits(): FormGroup {
    return this.fb.group({
      id: 0,
      text: ['', RxwebValidators.unique()]
    });
  }

  getConsultationMeritsControls() {
    return (this.form.get('consultationMerits') as FormArray).controls;
  }

  addConsultationMerits() {
    (this.form.get("consultationMerits") as FormArray).push(this.consultationMerits);
    this.consultationMeritsDataSource.next(this.getConsultationMeritsControls());
  }

  deleteConsultationMerits(index: any) {
    (this.form.get("consultationMerits") as FormArray).removeAt(index);
    this.consultationMeritsDataSource.next(this.getConsultationMeritsControls());
  }

  get consultationGrounds(): FormGroup {
    return this.fb.group({
      text: ['', RxwebValidators.unique()]
    });
  }

  getConsultationGroundsControls() {
    return (this.form.get('consultationGrounds') as FormArray).controls;
  }

  addConsultationGrounds() {
    (this.form.get("consultationGrounds") as FormArray).push(this.consultationGrounds);
    this.consultationGroundsDataSource.next(this.getConsultationGroundsControls());
  }

  deleteConsultationGrounds(index: any) {
    (this.form.get("consultationGrounds") as FormArray).removeAt(index);
    this.consultationGroundsDataSource.next(this.getConsultationGroundsControls());
  }

  onSubmitAsDraft() {
    // if new, save as draft
    if (!this.consultationId)
      this.form.controls['status'].setValue(ConsultationStatus.Draft);
    this.onSubmit();
  }

  onSubmitAsNew() {
    this.form.controls['status'].setValue(ConsultationStatus.New);
    this.onSubmit();
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    let result$ = this.consultationId
      ? this.consultationService.update(this.form.value)
      : this.consultationService.create(this.form.value);
    this.subs.add(
      result$.subscribe(
        () => {
          this.loaderService.stopLoading();
          let message = this.consultationId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
          //this.location.back();
          this.router.navigate(['/consultation']);
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          //
          let message = this.consultationId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
        }
      )
    );

  }

}
