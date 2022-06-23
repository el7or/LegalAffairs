import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Rx';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import Swal from 'sweetalert2';

import { ConsultationStatus, ReturnedConsultationTypes } from 'app/core/enums/ConsultationStatus';
import { MoamalaDetails, MoamalatListItem } from 'app/core/models/moamalat';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MoamalatService } from 'app/core/services/moamalat.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { ConsultationService } from 'app/core/services/consultation.service';
import { ConsultationReviewFormComponent } from '../consultation-review-form/consultation-review-form.component';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { ConsultationSupportingDocument } from 'app/core/models/consultation-request';
import { MoamalatQueryObject } from 'app/core/models/query-objects';
import { Department } from 'app/core/enums/Department';

@Component({
  selector: 'app-consultation-review-view',
  templateUrl: './consultation-review-view.component.html',
  styleUrls: ['./consultation-review-view.component.css']
})
export class ConsultationReviewViewComponent implements OnInit, OnDestroy {

  consultationId: number = 0;
  moamala!: MoamalaDetails;
  consultation!: any;
  relatedMoamalatDataSource!: MatTableDataSource<MoamalatListItem>;
  private subs = new Subscription();
  isDepartmentManager: boolean;
  isGeneralSupervisor: boolean;
  isRegulationsAndLawsDepartment: boolean;
  accepted: boolean;
  returned: boolean;
  approved: boolean;
  consultationSupportingDocument: ConsultationSupportingDocument = new ConsultationSupportingDocument();
  queryObject: MoamalatQueryObject = new MoamalatQueryObject();

  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private consultationService: ConsultationService,
    private moamalatService: MoamalatService,
    private router: Router,
    private loaderService: LoaderService,
    private alert: AlertService,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    public location: Location,
    private dialog: MatDialog,
    public authService: AuthService,
  ) {
    this.activatedRoute.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.consultationId = +id;
      }
    }
    );
  }

  public get ConsultationStatus(): typeof ConsultationStatus {
    return ConsultationStatus;
  }
  public get ReturnedConsultationTypes(): typeof ReturnedConsultationTypes {
    return ReturnedConsultationTypes;
  }
  public get Department(): typeof Department {
    return Department;
  }

  populateConsultation() {
    this.subs.add(
      this.consultationService.get(this.consultationId).subscribe((res: any) => {
        this.consultation = res.data;
        this.populateMoamala(this.consultation.moamalaId);
        this.populateRelatedMoamalat(this.consultation.moamalaId);
        this.isDepartmentManager = this.authService.checkRole(AppRole.DepartmentManager, this.consultation.department.id);
        this.isGeneralSupervisor = this.authService.currentUser.roles.includes(AppRole.GeneralSupervisor);
        this.accepted = this.isDepartmentManager && (this.consultation.status.id == ConsultationStatus.New || this.consultation.status.id == ConsultationStatus.Returned || this.consultation.status.id == ConsultationStatus.Modified)
        this.returned = (this.isDepartmentManager && (this.consultation.status.id == ConsultationStatus.New || this.consultation.status.id == ConsultationStatus.Modified))
          || (this.isGeneralSupervisor && (this.consultation.status.id == ConsultationStatus.Accepted || this.consultation.status.id == ConsultationStatus.Modified));
        this.isRegulationsAndLawsDepartment = this.consultation.department.id == Department.RegulationsAndLaws;
        this.approved = this.isGeneralSupervisor && (this.consultation.status.id == ConsultationStatus.Accepted || this.consultation.status.id == ConsultationStatus.Modified)
        if (this.consultation.consultationSupportingDocuments?.length > 0) { this.consultationSupportingDocument = this.consultation.consultationSupportingDocuments[0]; }

      }, (error) => {
        this.alert.error("فشلت عملية جلب البيانات.");
        console.error(error);
      })
    )
  }
  populateMoamala(moamalaId: number) {
    this.subs.add(
      this.moamalatService.get(moamalaId).subscribe(
        (result: any) => {
          this.moamala = result.data;
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

  ngOnInit(): void {
    this.populateConsultation();

  }

  populateRelatedMoamalat(moamalaId) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.moamalatService.get(moamalaId).subscribe(
        (result: any) => {
          this.relatedMoamalatDataSource = new MatTableDataSource(result.data.relatedMoamalat);
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

  changeConsultationStatus(consultationStatus: ConsultationStatus) {
    if (consultationStatus == ConsultationStatus.Returned) {
      let dialogRef = this.dialog.open(ConsultationReviewFormComponent, {
        width: '30em',
        data: { consultationId: this.consultation.id, consultationStatus: ConsultationStatus.Returned, returnedType: this.isGeneralSupervisor ? ReturnedConsultationTypes.FromGeneralSupervisor : ReturnedConsultationTypes.FromDepartmentManager }
      });
    }
    else if (consultationStatus == ConsultationStatus.Approved || consultationStatus == ConsultationStatus.Accepted) {
      let message = '';
      let successMessage = '';
      let errorMessage = '';
      if (consultationStatus == ConsultationStatus.Accepted) {
        message = 'هل أنت متأكد من إتمام عملية قبول النموذج؟';
        successMessage = 'تمت عملية قبول النموذج بنجاح';
        errorMessage = 'فشلت عملية قبول النموذج !';
      }
      else if (consultationStatus == ConsultationStatus.Approved) {
        message = 'هل أنت متأكد من إتمام عملية  اعتماد النموذج؟';
        successMessage = 'تمت عملية  اعتماد النموذج بنجاح';
        errorMessage = 'فشلت عملية اعتماد النموذج !';
      }
      Swal.fire({
        title: 'تأكيد',
        text: message,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#ff3d71',
        confirmButtonText: (consultationStatus == ConsultationStatus.Approved) ? 'اعتماد' : 'قبول',
        cancelButtonText: 'إلغاء',
      }).then((result: any) => {
        if (result.value) {
          this.loaderService.startLoading(LoaderComponent);
          this.subs.add(
            this.consultationService.consultationReview({ Id: this.consultation.id, DepartmentVision: '', ConsultationStatus: consultationStatus }).subscribe(
              () => {
                this.loaderService.stopLoading();
                this.alert.succuss(successMessage);
                this.router.navigateByUrl('/consultation');
              },
              (error) => {
                this.loaderService.stopLoading();
                console.error(error);
                this.alert.error(errorMessage);
              }
            )
          );
        }
      });
    }
  }


  onBack(): void {
    this.location.back();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateVisuals($event) {
    this.populateConsultation();
  }
}
