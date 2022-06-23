import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, AfterContentChecked, ViewChild, ElementRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { RxwebValidators } from '@rxweb/reactive-form-validators';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { map, startWith } from 'rxjs/operators';
import { ENTER } from '@angular/cdk/keycodes';
import Swal from 'sweetalert2';

import { CaseService } from 'app/core/services/case.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CaseCategoryQueryObject, CaseQueryObject, CourtQueryObject, QueryObject } from 'app/core/models/query-objects';
import { CaseSources } from 'app/core/enums/CaseSources';
import { GroupNames } from 'app/core/models/attachment';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { SecondSubCategoryService } from 'app/core/services/second-sub-category.service';
import { CaseDetails, SaveCase } from 'app/core/models/case';
import { CourtService } from 'app/core/services/court.service';
import { BranchService } from 'app/core/services/branch.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { LitigationTypes } from 'app/core/enums/LitigationTypes';

@Component({
  selector: 'app-case-form',
  templateUrl: './case-form.component.html',
  styleUrls: ['./case-form.component.css'],
})

export class CaseFormComponent implements OnInit, AfterContentChecked, OnDestroy {
  caseDetails!: CaseDetails;
  caseId: number = 0;
  caseStatus: any;
  caseSources: any;
  litigationTypes: any;
  ministryLegalStatus: any;
  courts: any;
  caseTypes: any;
  branches: any;
  saveCase: SaveCase = new SaveCase();
  filesCount: number = 0;
  attachments: any = [];
  parties: any = [];
  caseSourceNumberPlaceholder: string = 'رقم القضية';
  //category addition
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER];
  // categoryCtrl = new FormControl();
  filteredCategories$!: Observable<KeyValuePairs<number>[]>;
  categories: KeyValuePairs<number>[] = [];
  allCategories: KeyValuePairs<number>[] = [];
  @ViewChild('categoryInput') categoryInput!: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete!: MatAutocomplete;
  //
  form: FormGroup = Object.create(null);
  queryObject = new QueryObject({ pageSize: 99999 });
  caseQueryObject = new CaseQueryObject({ pageSize: 99999 });

  caseGroundsDisplayedColumns: string[] = ['text', 'actions',];
  caseGroundsDataSource = new BehaviorSubject<AbstractControl[]>([]);

  private subs = new Subscription();

  public get CaseSource(): typeof CaseSources {
    return CaseSources;
  }
  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }
  public get LitigationTypes(): typeof LitigationTypes {
    return LitigationTypes;
  }
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private caseService: CaseService,
    private courtService: CourtService,
    private branchService: BranchService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private hijriConverter: HijriConverterService,
    private caseCategoryService: SecondSubCategoryService,
    public location: Location,
    private cdr: ChangeDetectorRef
  ) {
    this.route.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.caseId = +id;
      }
    });
  }

  ngOnInit() {

    this.init();

    this.populateCaseStatus();
    this.populateCaseSources();
    this.populateLitigationTypes();
    this.populateMinistryLegalStatus();
    this.populateBranches();

    if (this.caseId) { //update case
      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.caseService.get(this.caseId).subscribe(
          (result: any) => {
            this.caseDetails = result.data;

            this.patchForm(this.caseDetails);
            this.validateEditCaseForm();
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

  onReferenceCaseNoBlure() {
    let litigationType = this.form.controls["litigationType"].value;
    let referenceCaseNo = this.form.controls["referenceCaseNo"].value;

    if (litigationType && referenceCaseNo && litigationType != LitigationTypes.FirstInstance)
      this.populateCases(litigationType, referenceCaseNo);
  }


  onlitigationTypeChange(category, litigationType) {
    let referenceCaseNo = this.form.controls["referenceCaseNo"].value;
    if (litigationType && referenceCaseNo && litigationType != LitigationTypes.FirstInstance)
      this.populateCases(litigationType, referenceCaseNo);
    else if (litigationType && litigationType != LitigationTypes.FirstInstance && !referenceCaseNo)
      Swal.fire({
        text: 'لم يتم ادخال رقم القضية المرجعية',
        confirmButtonText: 'حسناً',
      });

    this.populateCourts(category, litigationType);
  }

  populateCases(litigationTypeValue: any, referenceCaseNoValue: any) {
    this.loaderService.startLoading(LoaderComponent);
    this.caseQueryObject.litigationType = litigationTypeValue - 1;
    this.caseQueryObject.referenceCaseNo = referenceCaseNoValue;

    this.subs.add(
      this.caseService.getWithQuery(this.caseQueryObject).subscribe(
        (result: any) => {
          this.loaderService.stopLoading();

          if (result.data.items.length == 0)
            Swal.fire({
              text: 'رقم القضية المرجعية غير موجود مع قضية سابقة',
              confirmButtonText: 'حسناً',
            });
        },
        (error) => {
          console.error(error);
          this.alert.error(error);
          this.loaderService.stopLoading();
        }
      )
    );
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  init() {
    this.form = this.fb.group({
      id: [this.caseId],
      isManual: [null],
      raselRef: [null, Validators.compose([Validators.required])],
      raselUnifiedNo: [null, Validators.compose([Validators.required])],
      caseSourceNumber: [null, Validators.compose([Validators.required])],
      najizRef: [null],
      najizId: [null],
      moeenRef: [null],
      caseSource: ["", Validators.compose([Validators.required])],
      status: [{ value: "", disabled: true }],
      litigationType: ["", Validators.compose([Validators.required])],
      legalStatus: ["", Validators.compose([Validators.required])],
      startDate: [null],
      courtId: [""],
      //caseTypeId: [null, Validators.compose([Validators.required])],
      referenceCaseNo: [null],
      //mainNo: [null],
      circleNumber: [null, Validators.compose([Validators.required])],
      subject: [null, Validators.compose([Validators.required])],
      caseDescription: [null, Validators.compose([Validators.required])],
      //orderDescription: [null],
      branchId: [{ value: "", disabled: true }],
      //recordDate: [null],
      //fileNo: [null],
      judgeName: [null],
      notes: [null],
      //pronouncingJudgmentDate: [null],
      receivingJudgmentDate: [null],
      caseGrounds: this.fb.array([this.caseGrounds]),
      attachments: [null],
      parties: [null]
    });
    this.caseGroundsDataSource.next(this.getCaseGroundsControls());
  }

  get caseGrounds(): FormGroup {
    return this.fb.group({
      text: ['', RxwebValidators.unique()]
    });
  }

  getCaseGroundsControls() {
    return (this.form.get('caseGrounds') as FormArray).controls;
  }

  updateCaseGrounds() {
    (this.form.get("caseGrounds") as FormArray).push(this.caseGrounds);
    this.caseGroundsDataSource.next(this.getCaseGroundsControls());
  }

  deleteCaseGrounds(index: any) {
    (this.form.get("caseGrounds") as FormArray).removeAt(index);
    this.caseGroundsDataSource.next(this.getCaseGroundsControls());
  }

  patchForm(caseDetails: CaseDetails) {

    if (caseDetails.caseSource?.id) { this.onChangeCaseSource(caseDetails.caseSource?.id); }

    this.onCaseParties(caseDetails.parties);
    this.onCaseAttachment(caseDetails.attachments)

    this.form.patchValue({
      isManual: caseDetails.isManual,
      raselRef: caseDetails.raselRef,
      raselUnifiedNo: caseDetails.raselUnifiedNo,
      caseSourceNumber: caseDetails.caseNumberInSource,
      najizRef: caseDetails.najizRef,
      najizId: caseDetails.najizId,
      moeenRef: caseDetails.moeenRef,
      caseSource: caseDetails.caseSource?.id,
      status: caseDetails.status?.id,
      litigationType: caseDetails.litigationType?.id,
      legalStatus: caseDetails.legalStatus?.id,
      startDate: caseDetails.startDate,
      courtId: caseDetails.court?.id,
      //caseTypeId: caseDetails.caseType?.id,
      referenceCaseNo: caseDetails.referenceCaseNo,
      //mainNo: caseDetails.mainNo,
      circleNumber: caseDetails.circleNumber,
      subject: caseDetails.subject,
      caseDescription: caseDetails.caseDescription,
      //orderDescription: caseDetails.orderDescription,
      branchId: caseDetails.branchId,
      // recordDate: caseDetails.recordDate,
      // fileNo: caseDetails.fileNo,
      judgeName: caseDetails.judgeName,
      closeDate: caseDetails.closeDate,
      pronouncingJudgmentDate: caseDetails.pronouncingJudgmentDate,
      receivingJudgmentDate: caseDetails.receivingJudgmentDate,
      notes:caseDetails.notes
    });

    //delete init empty
    this.deleteCaseGrounds(0);
    // to patch form array //
    caseDetails.caseGrounds.forEach((item) => {
      (this.form.get("caseGrounds") as FormArray).push(this.fb.group({
        text: item.text
      }));
    });
    this.caseGroundsDataSource.next(this.getCaseGroundsControls());
    ///
    this.ChangePlaceholder();
  }

  ChangePlaceholder() {
    if (this.form.get('caseSource')?.value == this.CaseSource.Najiz) {
      this.caseSourceNumberPlaceholder = "رقم القضية في ناجز";
    }
    else if (this.form.get('caseSource')?.value == this.CaseSource.Moeen) {
      this.caseSourceNumberPlaceholder = "رقم الدعوى في معين";
    }
    else if (this.form.get('caseSource')?.value == this.CaseSource.QuasiJudicialCommittee) {
      this.caseSourceNumberPlaceholder = "رقم القضية في اللجنة";
    }
  }

  onChangeCaseSource(value: any) {
    this.form.controls["courtId"].setValue(null);
    if (value)
      this.populateCourts(value, this.caseDetails?.litigationType?.id);

    if (value == this.CaseSource.Najiz) {
      this.form.controls['najizRef']?.setValidators(Validators.required);
      this.form.controls['najizId']?.setValidators(Validators.required);
      ///
      this.form.get('moeenRef')?.clearValidators();
    }
    else if (value == this.CaseSource.Moeen) {
      this.form.controls['moeenRef']?.setValidators(Validators.required);
      ///
      this.form.get('najizRef')?.clearValidators();
      this.form.get('najizId')?.clearValidators();
    }
    else if (value == this.CaseSource.QuasiJudicialCommittee) {
      ///
      this.form.get('najizRef')?.clearValidators();
      this.form.get('najizId')?.clearValidators();
      this.form.get('moeenRef')?.clearValidators();
    }

    this.form.controls['najizRef']?.updateValueAndValidity();
    this.form.controls['najizId']?.updateValueAndValidity();
    this.form.controls['moeenRef']?.updateValueAndValidity();
    ///
    this.ChangePlaceholder();
  }

  validateEditCaseForm() {
    if (this.caseId) {
      if (this.form.get('raselRef')?.value == null) {
        this.form.get('raselRef')?.clearValidators();
        this.form.get('raselUnifiedNo')?.clearValidators();
        this.form.controls['raselRef']?.updateValueAndValidity();
        this.form.controls['raselUnifiedNo']?.updateValueAndValidity();
      }
      ///
      this.form.controls['caseSource']?.disable();
      this.form.controls['caseSourceNumber'].disable();
      ///
      if (this.form.get('isManual')?.value == false) {
        this.form.controls['najizRef'].disable();
        this.form.controls['najizId'].disable();
        this.form.controls['moeenRef'].disable();
        this.form.controls['raselRef'].disable();
        this.form.controls['raselUnifiedNo'].disable();
      }
    }
  }

  populateCaseStatus() {
    if (!this.caseStatus) {
      this.subs.add(
        this.caseService.getCaseStatus().subscribe(
          (result: any) => {
            this.caseStatus = result;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
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


  populateCourts(category, litigationType) {

    let queryObject: CourtQueryObject = new CourtQueryObject({
      pageSize: 999,
      courtCategory: category,
      litigationType: litigationType
    });
    this.subs.add(
      this.courtService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.courts = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateBranches() {
    if (!this.caseTypes) {
      this.subs.add(
        this.branchService
          .getWithQuery(this.queryObject)
          .subscribe(
            (result: any) => {
              this.branches = result.data.items;
            },
            (error) => {
              console.error(error);
              this.alert.error('فشلت عملية جلب البيانات !');
            }
          )
      );
    }
  }

  onCaseAttachment(list: any) {
    this.attachments = list;
        this.filesCount = this.attachments.filter(a => !a.isDeleted).length;
    this.form.controls['attachments'].setValue(this.attachments);
  }

  onSubmit() {

    this.loaderService.startLoading(LoaderComponent);
    this.form.controls['status'].enable();
    this.form.controls['caseSource'].enable();
    this.form.controls['caseSourceNumber'].enable();
    this.form.controls['najizRef'].enable();
    this.form.controls['najizId'].enable();
    this.form.controls['moeenRef'].enable();
    this.form.controls['raselRef'].enable();
    this.form.controls['raselUnifiedNo'].enable();
    this.form.controls['branchId'].enable();

    this.form.patchValue({
      startDate: this.hijriConverter.calendarDateToDate(
        this.form.get('startDate')?.value?.calendarStart
      ),
      recordDate: this.hijriConverter.calendarDateToDate(
        this.form.get('recordDate')?.value?.calendarStart
      ),
      closeDate: this.hijriConverter.calendarDateToDate(
        this.form.get('closeDate')?.value?.calendarStart
      ),
      pronouncingJudgmentDate: this.hijriConverter.calendarDateToDate(
        this.form.get('pronouncingJudgmentDate')?.value?.calendarStart
      ),
      receivingJudgmentDate: this.hijriConverter.calendarDateToDate(
        this.form.get('receivingJudgmentDate')?.value?.calendarStart
      ),
    });

    let result$ = this.caseId
      ? this.caseService.update(this.form.value)
      : this.caseService.create(this.form.value);
    this.subs.add(
      result$.subscribe(
        () => {
          this.loaderService.stopLoading();
          let message = this.caseId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
          this.location.back();
          //this.router.navigate(['/cases']);
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.validateEditCaseForm();
          this.form.controls['status'].disable();
          this.form.controls['branchId'].disable();
          //
          let message = this.caseId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
        }
      )
    );
  }
  onCaseParties(list: any) {
    this.parties = list;
    this.form.controls['parties'].setValue(this.parties);
  }

  //categories addition
  private _filter(value: string): KeyValuePairs[] {
    if (typeof value == "string") {
      const filterValue = value.toLowerCase();

      return this.allCategories.filter(category => category.name.toLowerCase().indexOf(filterValue) === 0);
    }
    return [];
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    //Add category
    if ((value || '').trim()) {
      // if added category already exists in selected categories return
      if (this.categories.map(c => c.name).indexOf(value.trim()) > -1) {
        return;
      }

      // if added category exists in allcategories
      var existedCategoryInAllCategoriesIndex = this.allCategories.map(c => c.name).indexOf(value.trim());
      if (existedCategoryInAllCategoriesIndex > -1) {
        this.categories.push(this.allCategories[existedCategoryInAllCategoriesIndex]);
      }


      // if added category not exist in allcategories add it with id = 0
      else { this.categories.push(new KeyValuePairs({ name: value.trim(), id: 0 })); }
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }
  }

  remove(index: any): void {
    if (index >= 0) {
      this.categories.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    if (this.categories.map(c => c.name).indexOf(event.option.value.name.trim()) > -1) {
      return;
    }
    this.categories.push(event.option.value);
    this.categoryInput.nativeElement.value = '';
   }
  //
}
