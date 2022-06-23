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
import { Observable } from 'rxjs';
import { FirstSubCategoryService } from 'app/core/services/first-sub-category.service';
import { MainCategoryService } from 'app/core/services/main-category.service';
import { CaseService } from 'app/core/services/case.service';
import { CaseSources } from 'app/core/enums/CaseSources';
import { ValueConverter } from '@angular/compiler/src/render3/view/template';

@Component({
  selector: 'app-case-category-form',
  templateUrl: './case-category-form.component.html',
  styleUrls: ['./case-category-form.component.css']
})

export class CaseCategoryFormComponent implements OnInit, AfterViewInit, OnDestroy {
  caseCategoryId: number = 0;
  saveCategory: any = {};
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
  mainCategories: any[] = [];
  firstSubCategories: any[] = [];
  filteredOptions: Observable<string[]> = new Observable<string[]>();
  firstSubFilteredOptions: Observable<string[]> = new Observable<string[]>();

  public get CaseSource(): typeof CaseSources {
    return CaseSources;
  }
  constructor(
    private fb: FormBuilder,
    private alert: AlertService,
    public dialogRef: MatDialogRef<CaseCategoryFormComponent>,
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
            this.caseCategory = result.data;
            this.populateCategoryForm(result.data);
            this.populateMainCategories(result.data.caseSource);
            this.populateFirstSubCategories(result.data?.mainCategory?.id);
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
      mainCategory: [null],
      firstSubCategory: [null],
      isActive: true
    });
  }


  populateCategoryForm(caseCategory: CaseCategory) {
    this.form.patchValue({
      id: caseCategory?.id,
      name: caseCategory?.name,
      caseSource: caseCategory?.caseSource,
      mainCategory: caseCategory?.mainCategory,
      firstSubCategory: caseCategory?.firstSubCategory,
      isActive: caseCategory?.isActive
    });
  }

  onSubmit() {
    Object.assign(this.saveCategory, this.form.value);
    if (!this.saveCategory.firstSubCategory?.id) {
      this.saveCategory.firstSubCategory = {
        name: this.saveCategory.firstSubCategory,
        id: this.caseCategory?.firstSubCategory?.id,
        //mainCategoryId:this.caseCategory?.firstSubCategory?.mainCategoryId
      }
    }

    if (!this.saveCategory.mainCategory?.id) {
      this.saveCategory.mainCategory = {
        name: this.saveCategory.mainCategory,
        // id:this.caseCategory?.mainCategory?.id
      }
    }
    this.loaderService.startLoading(LoaderComponent);
    let result$ = this.caseCategoryId
      ? this.secondSubCategoryService.update(this.saveCategory)
      : this.secondSubCategoryService.create(this.saveCategory);

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
          this.alert.error("فشلت عملية الحفظ !");
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

        if (!this.form.value.firstSubCategory?.id) {
          this.form.value.firstSubCategory = {
            name: this.form.value.firstSubCategory,
            // id:this.form.value?.firstSubCategory?.id

          }
        }

        if (!this.form.value.mainCategory?.id) {
          this.form.value.mainCategory = {
            name: this.form.value.mainCategory,
            // id:this.caseCategory?.mainCategory?.id
          }
        }
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

  populateMainCategories(caseSource?: any) {
    if (caseSource != this.caseCategory?.caseSource && (this.caseCategoryId != undefined || this.caseCategoryId != 0)) {
      this.form.controls['mainCategory'].setValue(null);
      this.form.controls['firstSubCategory'].setValue(null);
    }
    if (this.caseCategoryId == undefined || this.caseCategoryId == 0) {
      this.form.controls['mainCategory'].setValue(null);
      this.form.controls['firstSubCategory'].setValue(null);
    }
    let queryObject: CaseCategoryQueryObject = new CaseCategoryQueryObject({
      sortBy: 'name',
      pageSize: 999,
      caseSource: caseSource == null ? this.form.value.caseSource : caseSource,
    });
    this.subs.add(
      this.mainCategoryService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.mainCategories = result.data.items;
          this.filteredOptions = this.form.controls['mainCategory'].valueChanges.pipe(
            startWith(''),
            map(value => typeof value === 'string' ? value : value?.name),
            map(name => name ? this._filter(name) : this.mainCategories.slice())
          );
          this.form.controls['mainCategory'].valueChanges.subscribe(
            value => {
              this.form.controls['firstSubCategory'].setValue(null);
              this.firstSubFilteredOptions = null;
              let mainCategory = this.mainCategories.includes(value);
              if (mainCategory) {
                this.populateFirstSubCategories();
              }
            }
          );
        },
        (error) => {
          console.error(error);
          this.alert.error("فشلت عملية جلب البيانات !");
        }
      )
    );
  }

  populateFirstSubCategories(mainCategory?: any) {
    let queryObject: FirstSubCategoryQueryObject = new FirstSubCategoryQueryObject({
      sortBy: 'name',
      pageSize: 999,
      mainCategoryId: mainCategory == null ? this.form.controls['mainCategory'].value?.id : mainCategory
    });
    this.subs.add(
      this.firstSubCategoryService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.firstSubCategories = result.data.items;
          this.firstSubFilteredOptions = this.form.controls['firstSubCategory'].valueChanges
            .pipe(
              startWith(''),
              map(value => typeof value === 'string' ? value : value?.name),
              map(name => name ? this._filterFirstSubCategory(name) : this.firstSubCategories.slice()),
            );
        },
        (error) => {
          console.error(error);
          this.alert.error("فشلت عملية جلب البيانات !");
        }
      )
    );
  }

  displayFn(mainCategory: any): string {
    return mainCategory && mainCategory?.name ? mainCategory?.name : '';
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.mainCategories.filter((option: any) => option?.name.includes(filterValue));
  }

  displayFirstSubCategoryFn(firstSubCategory: any): string {
    return firstSubCategory && firstSubCategory?.name ? firstSubCategory?.name : '';
  }

  private _filterFirstSubCategory(value: any): any[] {
    const filterValue = value.toLowerCase();
    return this.firstSubCategories.filter((option: any) => option?.name.includes(filterValue));
  }

}
