import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';

import { CaseCategoryQueryObject, FirstSubCategoryQueryObject, BranchQueryObject } from 'app/core/models/query-objects';
import { SecondSubCategoryService } from 'app/core/services/second-sub-category.service';
import { CaseCategory } from 'app/core/models/case-category';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AfterViewInit } from '@angular/core';
import { OnDestroy } from '@angular/core';
import { debounceTime, map, startWith } from 'rxjs/operators';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { ENTER } from '@angular/cdk/keycodes';
import { ElementRef } from '@angular/core';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { FirstSubCategoryService } from 'app/core/services/first-sub-category.service';
import { MainCategoryService } from 'app/core/services/main-category.service';
import { CaseService } from 'app/core/services/case.service';
import { CaseSources } from 'app/core/enums/CaseSources';

@Component({
  selector: 'app-case-category-form',
  templateUrl: './case-second-sub-category-form.component.html',
  styleUrls: ['./case-second-sub-category-form.component.css']
})

export class CaseSecondSubCategoryFormComponent implements OnInit, AfterViewInit, OnDestroy {
  caseCategoryId: number = 0;
  caseCategory: CaseCategory = null;
  parents: CaseCategory[] = [];
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  queryObject: BranchQueryObject = {
    sortBy: 'id',
    isSortAscending: true,
    page: 1,
    pageSize: 100,
    isParent: true,
  };
  caseSources: any;
  caseSourceNumberPlaceholder: string = 'رقم القضية';
  //category addition
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER];
  // categoryCtrl = new FormControl();
  filteredCategories$!: Observable<KeyValuePairs<number>[]>;
  categories: KeyValuePairs<number>[] = [];
  mainCategories: KeyValuePairs<number>[] = [];
  firstSubCategories: KeyValuePairs<number>[] = [];
  @ViewChild('categoryInput') categoryInput!: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete!: MatAutocomplete;
  //
  public get CaseSource(): typeof CaseSources {
    return CaseSources;
  }
  constructor(
    private fb: FormBuilder,
    private alert: AlertService,
    public dialogRef: MatDialogRef<CaseSecondSubCategoryFormComponent>,
    private loaderService: LoaderService,
    private secondSubCategoryService: SecondSubCategoryService,
    private firstSubCategoryService: FirstSubCategoryService,
    private mainCategoryService: MainCategoryService,
    private caseService: CaseService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.caseCategoryId) this.caseCategoryId = this.data.caseCategoryId;
  }

  ngOnInit() {
    this.init();
    ///
    this.populateCaseSources();
    if (this.caseCategoryId) {
      this.loaderService.startLoading(LoaderComponent);

      this.subs.add(
        this.secondSubCategoryService.get(this.caseCategoryId).subscribe(
          (result: any) => {
            debugger
            this.caseCategory = result.data;
            this.populateForm();
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
  }

  /**
   * Component init.
   */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      name: [null, Validators.compose([Validators.required])],
      caseSource: ["", Validators.compose([Validators.required])],
      mainCategoryId: [null, Validators.compose([Validators.required])],
      firstSubCategoryId: [null, Validators.compose([Validators.required])],
    });
  }


  populateForm() {
    this.form.patchValue({
      id: this.caseCategory?.id,
      name: this.caseCategory?.name,
      caseSource: this.caseCategory?.caseSource.id,
    });

    this.populateMainCategories();

    this.form.patchValue({
      mainCategoryId: this.caseCategory?.mainCategory.id
    });

    this.populateFirstSubCategories()

    this.form.patchValue({
      firstSubCategoryId: this.caseCategory?.firstSubCategory.id
    });
  }

  onSubmit() {
    debugger
    this.loaderService.startLoading(LoaderComponent);
    let result$ = this.caseCategoryId
      ? this.secondSubCategoryService.update(this.form.value)
      : this.secondSubCategoryService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.caseCategoryId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          this.alert.error("فشلت عمليةالحفظ !");
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }

  /**
   * Check whether the name field value is unique or not.
   * @param name The name value.
   */
  private checkName(name: string): void {
    const nameCtrl = this.form.get('name');
    if (nameCtrl.valid && this.caseCategory) {
      if (this.caseCategory.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.secondSubCategoryService
          .isNameExists(this.form.value)
          .subscribe((result: any) => {
            if (result.data) {
              nameCtrl.setErrors({ uniqueName: true });
              nameCtrl.markAsTouched();
            } else {
              nameCtrl.updateValueAndValidity({ emitEvent: false });
            }
          });
      }
    }
  }

  ngAfterViewInit() {
    // Watch for name changes.
    this.form
      .get('name')
      .valueChanges.pipe(
        debounceTime(1000),
        map((value: string) => value.trim())
      )
      .subscribe((value: string) => {
        this.checkName(value);
      });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  //categories addition
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

  onChangeCaseSource() {
    this.populateMainCategories();
    this.firstSubCategories = [];
    //
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



  populateMainCategories() {

    let queryObject: CaseCategoryQueryObject = new CaseCategoryQueryObject({
      sortBy: 'name',
      pageSize: 999,
      caseSource: this.form.get('caseSource')?.value
    });

    this.subs.add(
      this.mainCategoryService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.mainCategories = result.data.items.map((i: any) => new KeyValuePairs({ name: i.name, id: i.id }));
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateFirstSubCategories() {
    let queryObject: FirstSubCategoryQueryObject = new FirstSubCategoryQueryObject({
      sortBy: 'name',
      pageSize: 999,
      mainCategoryId: this.form.value.caseSource,
    });
    this.subs.add(
      this.firstSubCategoryService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.firstSubCategories = result.data.items.map((i: any) => new KeyValuePairs({ name: i.name, id: i.id }));
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
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
    this.form.controls['mainCategory'].setValue(null);
  }
  //

}
