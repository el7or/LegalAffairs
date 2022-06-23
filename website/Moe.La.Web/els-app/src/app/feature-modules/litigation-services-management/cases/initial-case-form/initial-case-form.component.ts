import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseCategoryQueryObject } from 'app/core/models/query-objects';
import { MatDialogRef } from '@angular/material/dialog';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Subscription } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { ENTER } from '@angular/cdk/keycodes';
import { startWith } from 'rxjs/internal/operators/startWith';
import { map } from 'rxjs/operators';

import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CaseSources } from 'app/core/enums/CaseSources';
import { SecondSubCategoryService } from 'app/core/services/second-sub-category.service';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';

@Component({
  selector: 'app-initial-case-form',
  templateUrl: './initial-case-form.component.html',
  styleUrls: ['./initial-case-form.component.css']
})
export class InitialCaseFormComponent implements OnInit {
  caseSources: any;
  form: FormGroup = Object.create(null);
  //queryObject = new QueryObject({ pageSize: 99999 });
  caseSourceNumberPlaceholder: string = 'رقم القضية';
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER];
  // categoryCtrl = new FormControl();
  filteredCategories$!: Observable<KeyValuePairs<number>[]>;
  categories: KeyValuePairs<number>[] = [];
  allCategories: KeyValuePairs<number>[] = [];
  @ViewChild('categoryInput') categoryInput!: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete!: MatAutocomplete;

  private subs = new Subscription();

  public get CaseSource(): typeof CaseSources {
    return CaseSources;
  }

  constructor(private route: ActivatedRoute,
    private fb: FormBuilder,
    private caseService: CaseService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private router: Router,
    private caseCategoryService: SecondSubCategoryService,
    public dialogRef: MatDialogRef<InitialCaseFormComponent>,
    private hijriConverter: HijriConverterService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.init();
    this.populateCaseSources();
  }
  private _filter(value: string): KeyValuePairs[] {
    if (typeof value == "string") {
      const filterValue = value.toLowerCase();

      return this.allCategories.filter(category => category.name.toLowerCase().indexOf(filterValue) === 0);
    }
    return [];
  }
  populateCategories() {
    let queryObject: CaseCategoryQueryObject = new CaseCategoryQueryObject({
      sortBy: 'name',
      pageSize: 999,
      caseSource: this.form.value.caseSource,
    });
    this.subs.add(
      this.caseCategoryService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.allCategories = result.data.items.map((i: any) => new KeyValuePairs({ name: i.name, id: i.id }));

          this.filteredCategories$ = this.form.controls['caseCategories'].valueChanges.pipe(
            startWith(null),
            map((category: string | null) => category ? this._filter(category) : this.allCategories.slice()));
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
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
    this.form.controls['caseCategories'].setValue(null);
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
    this.form.controls['caseCategories'].setValue(null);
  }

  init() {
    this.form = this.fb.group({
      id: [0],
      caseSourceNumber: [null, Validators.compose([Validators.required])],
      caseSource: [3, Validators.compose([Validators.required])],
      caseCategories: [null],
      startDate:[null, Validators.compose([Validators.required])]
    });

    this.onChangeCaseSource();
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

  onChangeCaseSource() {
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

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.controls['caseSource'].enable();
    this.form.controls['caseSourceNumber'].enable();
    this.form.controls['caseCategories'].setValue(this.categories);

    this.form.patchValue({
      startDate: this.hijriConverter.calendarDateToDate(
        this.form.get('startDate')?.value?.calendarStart
      )})
    this.subs.add(this.caseService.createInitialCase(this.form.value).subscribe(res => {
      this.alert.succuss("تمت عملية الإضافة بنجاح");
      this.loaderService.stopLoading();
      this.onCancel();
    }, (error) => {
      this.alert.error("فشلت  عملية إضافة قضية!");
      this.loaderService.stopLoading();
      console.error(error);
    }))



  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }

  ngOnDestroy() {
    this.subs.closed;
  }
}
