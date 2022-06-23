
import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ResearcherConsultantService } from 'app/core/services/researcher-consultant.service';
import { UserService } from 'app/core/services/user.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { UserQueryObject } from 'app/core/models/query-objects';
import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { ResearcherConsultant, UserDetails } from 'app/core/models/user';

@Component({
  selector: 'app-researcher-map',
  templateUrl: './researcher-form.component.html',
  styleUrls: ['./researcher-form.component.css'],
})
export class ResearcherFormComponent implements OnInit, OnDestroy {
  researcherId: string = '';
  consultantId: string[] = [];

  researcherDetails!: ResearcherConsultant;

  allConsultants!: UserDetails[];
  filteredConsultants!: UserDetails[] | undefined;

  queryObject: UserQueryObject = new UserQueryObject({roles:['LegalConsultant'],enabled:true});


  form: FormGroup = Object.create(null);
  subs = new Subscription();
  id: any;
  oldConsultant!: KeyValuePairs<string>;

  constructor(
    private fb: FormBuilder,
    public userService: UserService,
    public researcherConsultantService: ResearcherConsultantService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<ResearcherFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.id = this.data.id;
    this.researcherId = this.data.researcherId;

  }

  ngOnInit() {
    this.init();
    this.getSelectedUser();
  }

  private init(): void {
    this.form = this.fb.group({
      consultant: [],
    });
  }

  getSelectedUser() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.userService.getUserDetails(this.researcherId).subscribe(
        (result: any) => {
          this.queryObject.branchId = result.data.branch.id;
          this.loaderService.stopLoading();
          this.populateConsultants();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }
  populateConsultants() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.userService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.allConsultants = result.data.items;
          this.filteredConsultants = result.data.items;

          if (this.id)
            this.populateResearcher();
          else
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
  filterConsultants() {
    let filterValue = this.consultant?.value?.toLowerCase();
    if (filterValue) {
      this.filteredConsultants = this.allConsultants.filter(consultants => (consultants.firstName + ' ' + consultants.lastName).toLowerCase().includes(filterValue));
    } else {
      this.filteredConsultants = this.allConsultants;
    }
  }

  displayFn(consultant?: UserDetails): string | undefined {
    return consultant ? consultant.firstName + ' ' + consultant.lastName : 'لا يوجد';
  }
  populateResearcher() {
    this.subs.add(
      this.researcherConsultantService.get(this.id).subscribe(
        (result: any) => {
          this.researcherDetails = result.data;
          this.populateResearcherForm(this.researcherDetails);
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
  populateResearcherForm(researcher: ResearcherConsultant) {
    let currentConsultant =  this.allConsultants.filter(c => c.id == researcher.consultantId)[0];
    this.form = this.fb.group({
      consultant: [currentConsultant],
    });
    this.oldConsultant = {id :currentConsultant?.id , name: currentConsultant?.firstName + ' ' + currentConsultant?.lastName};
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  checkSubmit() {
    if (this.oldConsultant.id != this.form.value.consultant?.id) {
      Swal.fire({
        title: 'تأكيد',
        text: 'الباحث مرتبط حالياً بالمستشار  ('+ this.oldConsultant.name +') ، هل تريد إتمام العملية؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        confirmButtonText: 'نعم',
        cancelButtonText: 'إلغاء',
      }).then((result: any) => {
        if (result.value) {
          this.onSubmit();

        }
      });
    }
    else {
      this.onCancel(null);

    }
  }
  onSubmit() {
    if (!this.oldConsultant && !this.form.value.consultant?.id) {
      this.onCancel(null);
      return;
    }

    this.loaderService.startLoading(LoaderComponent);
    let researcherConsultant = {researcherId: this.researcherId,consultantId: this.form.value.consultant?.id};
    let result = this.researcherConsultantService.create(researcherConsultant);
    this.subs.add(
      result.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.form.value.consultant?.id? 'تم ربط الباحث بالمستشار بنجاح' : 'تم إلغاء الارتباط بنجاح'
          this.alert.succuss(message);
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية ربط الباحث بالمستشار !');
        }
      )
    );


  }
  get consultant() {
    return this.form.get('consultant');
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
