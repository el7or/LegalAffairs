import { AuthService } from 'app/core/services/auth.service';
import { SecondSubCategoryService } from './../../../../../core/services/second-sub-category.service';
import { FirstSubCategoryService } from './../../../../../core/services/first-sub-category.service';
import { MainCategoryService } from './../../../../../core/services/main-category.service';
import { Location } from '@angular/common';
import { Component, OnInit, OnDestroy, Output, EventEmitter, Input, SimpleChanges } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { CaseSources } from 'app/core/enums/CaseSources';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';
import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { CourtService } from 'app/core/services/court.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { Subscription } from 'rxjs';
import { CaseQueryObject, FirstSubCategoryQueryObject, MainCategoryQueryObject, SecondSubCategoryQueryObject } from 'app/core/models/query-objects';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { CaseListItem, MainCaseDetails } from 'app/core/models/case';
import { MatDialog } from '@angular/material/dialog';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { AppRole } from 'app/core/models/role';
import { Department } from 'app/core/enums/Department';
import Swal from 'sweetalert2';
import { HearingSearchCasesComponent } from 'app/feature-modules/litigation-services-management/hearings/hearing-search-cases/hearing-search-cases.component';

@Component({
  selector: 'app-basic-case-form',
  templateUrl: './basic-case-form.component.html',
  styleUrls: ['./basic-case-form.component.css']
})
export class BasicCaseFormComponent implements OnInit, OnDestroy {
  caseSources: any;
  mainCategories: any;
  firstSubCategories: any;
  secondSubCategories: any;
  litigationTypes: any;
  ministryLegalStatus: any;
  courts: any;
  caseStatus: any;

  @Input() case: MainCaseDetails;
  @Output() onAddCase = new EventEmitter<number>();
  @Input() form: FormGroup = Object.create(null);

  isLitigationManager: boolean = this.authService.checkRole(AppRole.DepartmentManager, Department.Litigation);
  isResearcher: boolean = this.authService.checkRole(AppRole.LegalResearcher, Department.Litigation);

  private subs = new Subscription();

  public get CaseSource(): typeof CaseSources {
    return CaseSources;
  }
  public get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }

  constructor(
    private caseService: CaseService,
    private mainCategoryService: MainCategoryService,
    private firstSubCategoryService: FirstSubCategoryService,
    private secondSubCategoryService: SecondSubCategoryService,
    private courtService: CourtService,
    private authService: AuthService,
    private hijriConverter: HijriConverterService,
    private alert: AlertService,
    public location: Location,
    private loaderService: LoaderService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.populateCaseSources();
    this.populateLitigationTypes();
    this.populateMinistryLegalStatus();

  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['case']) {
      this.case = changes['case'].currentValue;
      if (this.case) {
        this.populateMainCategories(this.case.caseSource.id);
        this.populateFirstSubCategories(this.case.subCategory.mainCategory.id);
        this.populateSecondSubCategories(this.case.subCategory.firstSubCategory.id);
        this.patchForm(this.case);
        this.populateCourts();
      }
    }
  }

  populateCaseSources() {
    if (!this.caseSources) {
      this.subs.add(
        this.caseService.getCaseSources().subscribe(
          (result: any) => {
            this.caseSources = result;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  populateMainCategories(caseSource: number) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.mainCategoryService
        .getWithQuery(new MainCategoryQueryObject({
          sortBy: 'name',
          pageSize: 9999,
          caseSource
        }))
        .subscribe(
          (result: any) => {
            this.mainCategories = result.data.items;
            this.loaderService.stopLoading();
          },
          (error: any) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
    );
  }


  patchForm(caseDetails: MainCaseDetails) {
    this.form.patchValue({
      id: caseDetails.id,
      caseSource: caseDetails.caseSource.id,
      mainCategoryId: caseDetails.subCategory?.mainCategory.id,
      firstSubCategoryId: caseDetails.subCategory?.firstSubCategory.id,
      secondSubCategoryId: caseDetails.subCategory?.id,
      caseNumberInSource: caseDetails.caseNumberInSource,
      litigationType: caseDetails.litigationType.id,
      legalStatus: caseDetails.legalStatus.id,
      startDate: caseDetails.startDate,
      courtId: caseDetails.court?.id,
      circleNumber: caseDetails.circleNumber,
      subject: caseDetails.subject,
      caseDescription: caseDetails?.caseDescription?.replace('<br>', '\n'),
      judgeName: caseDetails.judgeName,
      relatedCaseId: caseDetails.relatedCaseId,
      relatedCaseNumber: caseDetails.relatedCase?.caseNumberInSource,
      status: caseDetails.status.id,
      notes: caseDetails.notes
    });
    this.caseStatus = caseDetails.status.id;
    this.validateBasicCaseForm();
  }

  validateBasicCaseForm() {
    if (this.case?.id) {
      if (!this.isLitigationManager && this.caseStatus != CaseStatus.Draft) {
        this.form.controls['mainCategoryId'].disable();
        this.form.controls['firstSubCategoryId'].disable();
        this.form.controls['secondSubCategoryId'].disable();
      }
      if (this.isResearcher) {
        this.form.controls['caseSource'].disable();
        this.form.controls['caseNumberInSource'].disable();
        this.form.controls['litigationType'].disable();
      }
      if (this.isLitigationManager) {
        this.form.controls['caseNumberInSource'].disable();
      }
    }
  }

  populateFirstSubCategories(mainCategoryId: number) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.firstSubCategoryService
        .getWithQuery(new FirstSubCategoryQueryObject({
          sortBy: 'name',
          pageSize: 9999,
          mainCategoryId: mainCategoryId
        }))
        .subscribe(
          (result: any) => {
            this.firstSubCategories = result.data.items;
            this.loaderService.stopLoading();
          },
          (error: any) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
    );
  }

  populateSecondSubCategories(firstSubCategoryId: number) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.secondSubCategoryService
        .getWithQuery(new SecondSubCategoryQueryObject({
          sortBy: 'name',
          pageSize: 9999,
          firstSubCategoryId: firstSubCategoryId,
          isActive: true
        }))
        .subscribe(
          (result: any) => {
            this.secondSubCategories = result.data.items;
            this.loaderService.stopLoading();
          },
          (error: any) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
    );
  }

  populateCourts() {
    var litigationType = this.form.controls['litigationType'].value;
    var courtCategory = this.form.controls['caseSource'].value;

    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.courtService.getWithQuery({
        sortBy: 'name',
        isSortAscending: true,
        page: 1,
        pageSize: 99,
        litigationType,
        courtCategory
      }).subscribe(
        (result: any) => {
          this.courts = result.data.items;
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

  populateLitigationTypes() {
    if (!this.litigationTypes) {
      this.subs.add(
        this.caseService.getLitigationTypes().subscribe(
          (result: any) => {
            this.litigationTypes = result;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  populateMinistryLegalStatus() {
    if (!this.ministryLegalStatus) {
      this.subs.add(
        this.caseService.getMinistryLegalStatus().subscribe(
          (result: any) => {
            this.ministryLegalStatus = result;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  onChangeCaseSource(value: any) {
    if (value) {
      this.form.controls["mainCategoryId"].setValue(null);
      this.form.controls["firstSubCategoryId"].setValue(null);
      this.form.controls["secondSubCategoryId"].setValue(null);
      this.form.controls["courtId"].setValue(null);

      // get casesource maincategories
      this.populateMainCategories(value);

      // get courts with casesource && litigationtype
      this.populateCourts();
    }
  }

  onChangeMainCategory(value: any) {
    if (value) {
      this.form.controls["firstSubCategoryId"].setValue(null);
      this.populateFirstSubCategories(value);
    }
    if (this.isLitigationManager && this.case) {
      Swal.fire({
        title: 'تحذير',
        text: 'تعديل التصنيف قد يؤثر في تصنيف المذكرات المرتبطة بجلسات القضية !',
        icon: 'warning',
        showCancelButton: false,
        confirmButtonText: 'حسناً',
      })
    }
  }

  onChangeFirstSubCategory(value: any) {
    if (value) {
      this.form.controls["secondSubCategoryId"].setValue(null);
      this.populateSecondSubCategories(value);
    }
    if (this.isLitigationManager && this.case) {
      Swal.fire({
        title: 'تحذير',
        text: 'تعديل التصنيف قد يؤثر في تصنيف المذكرات المرتبطة بجلسات القضية !',
        icon: 'warning',
        showCancelButton: false,
        confirmButtonText: 'حسناً',
      })
    }
  }

  onChangeSecondSubCategory() {
    if (this.isLitigationManager && this.case) {
      Swal.fire({
        title: 'تحذير',
        text: 'تعديل التصنيف قد يؤثر في تصنيف المذكرات المرتبطة بجلسات القضية !',
        icon: 'warning',
        showCancelButton: false,
        confirmButtonText: 'حسناً',
      })
    }
  }

  onChangeLitigationType(value: any) {
    if (value == LitigationTypes.FirstInstance) {
      this.form.get('relatedCaseId')?.clearValidators();
      this.form.get('relatedCaseId')?.updateValueAndValidity();
      this.form.controls['relatedCaseId'].setValue(null);
    } else {
      this.form.get('relatedCaseId')?.setValidators(Validators.required);
      this.form.get('relatedCaseId')?.updateValueAndValidity();
    }

    // get litigationType courts
    this.form.controls['courtId'].setValue(null);
    this.populateCourts();
  }

  onChooseCase() {
    let caseQueryObject: CaseQueryObject = new CaseQueryObject();
    caseQueryObject.isForChooseRelatedCase = true;
    // filter prev case with litigationType
    caseQueryObject.litigationType = this.form.controls['litigationType'].value - 1;

    const dialogRef = this.dialog.open(HearingSearchCasesComponent, {
      width: '90%',
      height: '90%',
      data: caseQueryObject
    });
    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res: CaseListItem) => {
          if (res) {
            this.form.patchValue({
              relatedCaseId: res.id,
              relatedCaseNumber: res.caseNumberInSource
            });
          }
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.patchValue({
      startDate: this.hijriConverter.calendarDateToDate(
        this.form.get('startDate')?.value?.calendarStart
      )
    });
    this.form.controls['relatedCaseId'].enable();
    this.form.controls['relatedCaseNumber'].disable();
    this.form.controls['mainCategoryId'].enable();
    this.form.controls['firstSubCategoryId'].enable();
    this.form.controls['secondSubCategoryId'].enable();
    this.form.controls['caseSource'].enable();
    this.form.controls['caseNumberInSource'].enable();
    this.form.controls['litigationType'].enable();

    // update case
    if (this.form.controls["id"].value) {
      this.subs.add(
        this.caseService.update(this.form.value).subscribe(
          () => {
            this.loaderService.stopLoading();
            this.onAddCase.emit(this.form.controls["id"].value);
            this.alert.succuss('تمت عملية التعديل بنجاح');
          },
          (error) => {
            this.validateBasicCaseForm();
            console.error(error);
            this.loaderService.stopLoading();
            this.alert.error('فشلت عملية التعديل !');
          }
        )
      );
    }
    // add new case
    else {
      this.subs.add(
        this.caseService.create(this.form.value).subscribe(
          (result: any) => {
            this.loaderService.stopLoading();
            this.onAddCase.emit(result.data.id);
            this.form.controls["id"].setValue(result.data.id);
            this.alert.succuss('تمت عملية الحفظ بنجاح');
          },
          (error) => {
            this.validateBasicCaseForm();
            console.error(error);
            this.loaderService.stopLoading();
            this.alert.error('فشلت عملية الحفظ !');
          }
        )
      );
    }
  }
}
