import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';

import { IdentityTypeService } from 'app/core/services/identity-type.service';
import { QueryObject } from 'app/core/models/query-objects';
import { GovernmentOrganizationService } from 'app/core/services/governmentOrganization.service';
import { DistrictService } from 'app/core/services/district.service';
import { PartyTypes } from 'app/core/enums/PartyTypes';
import { ProvinceService } from 'app/core/services/province.service';
import { CityService } from 'app/core/services/city.service';
import { CountryService } from 'app/core/services/country.service';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';
import { CaseService } from 'app/core/services/case.service';
import { AlertService } from 'app/core/services/alert.service';
import { PartyTypeService } from 'app/core/services/party-type.service';

@Component({
  selector: 'app-add-manual-party-form',
  templateUrl: './add-manual-party-form.component.html',
  styleUrls: ['./add-manual-party-form.component.css']
})
export class AddManualPartyFormComponent implements OnInit {

  partyType: number = 0;
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  partyTypes: any;
  party: any;
  identityTypes: any[] = [];

  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: 999,
  };
  governmentOrganizations: any[] = [];
  districts: any[] = [];
  provinces: any;
  cities: any;
  countries: any;
  filteredidentityTypes: any;
  identityValueError: string = '';
  identityTypeMaxValue = 20;
  parties: any = [];


  public get PartyTypes(): typeof PartyTypes {
    return PartyTypes;
  }
  constructor(
    public countryService: CountryService,
    public provinceService: ProvinceService,
    public districtService: DistrictService,
    public cityService: CityService,
    public governmentOrganizationService: GovernmentOrganizationService,
    public identityTypeService: IdentityTypeService,
    private fb: FormBuilder,
    public partyTypeService: PartyTypeService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<AddManualPartyFormComponent>,
    private hijriConverter: HijriConverterService,
    private caseService: CaseService,

    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (data) {
      this.party = data.party;
      this.parties = data.parties;
    }
  }

  ngOnInit() {
    this.init();

    this.populateGovernmentOrganizations();
    this.populateIdentityTypes();
    this.populatePartyTypes();
    this.populateProvinces();
    this.populateCountries();

    if (this.party)
      this.populateForm(this.party);

    this.form.controls['identityStartDate'].valueChanges.subscribe(
      value => {
        this.validateIdentityDates();
      }
    );
    this.form.controls['identityExpireDate'].valueChanges.subscribe(
      value => {
        this.validateIdentityDates();
      }
    );

    this.form.controls['identityValue'].valueChanges.subscribe(
      value => {
        if (this.form.controls['identityTypeId'].value)
          this.validateIdentityValue();
      }
    );
    this.form.controls['identityTypeId'].valueChanges.subscribe(
      value => {
        if (value)
          this.validateIdentityValue();

      }
    );

  }
  validateIdentityValue() {
    let identityValue = this.form.controls['identityValue'];
    let identityTypeId = this.form.controls['identityTypeId'];
    if (identityTypeId.value == 1 && identityValue.value && identityValue.value[0] != 1) {
      identityValue.setErrors({ notValid: true });
      this.identityValueError = 'يجب ان يبدا رقم الهوية الوطنية ب 1';
      this.identityTypeMaxValue = 10;
    }

    else if (identityTypeId.value == 2 && identityValue.value && identityValue.value[0] != 2) {
      identityValue.setErrors({ notValid: true });
      this.identityValueError = 'يجب ان يبدا رقم هوية المقيم ب 2';
      this.identityTypeMaxValue = 10;
    }

    else if (identityValue.value?.length != 10 && (identityTypeId.value == 2 || identityTypeId.value == 1)) {
      identityValue.setErrors({ maxLength: true });
      this.identityValueError = '';
      this.identityTypeMaxValue = 10;
    }
    else if (identityValue.value?.length != 20 && !(identityTypeId.value == 2 || identityTypeId.value == 1)) {
      identityValue.setErrors({ maxLength: true });
      this.identityValueError = '';
      this.identityTypeMaxValue = 20;
    }
    else if (!identityValue.value) {
      identityValue.setErrors({ required: true });
      this.identityValueError = '';
    }

    else {
      identityValue.setErrors(null);
      this.identityValueError = '';
      this.identityTypeMaxValue = 20;
    }
  }
  validateIdentityDates() {

    let identityStartDate = this.form.controls['identityStartDate'].value;
    let identityExpireDate = this.form.controls['identityExpireDate'].value;

    if (identityExpireDate?.calendarStart && identityStartDate?.calendarStart && (new Date(identityStartDate?.calendarStart.year, identityStartDate?.calendarStart.month - 1, identityStartDate?.calendarStart.day) >
      new Date(identityExpireDate?.calendarStart.year, identityExpireDate?.calendarStart.month - 1, identityExpireDate?.calendarStart.day))) {
      this.form.controls['identityExpireDate'].setErrors({ notValidDate: true });
    }
    else {
      this.form.controls['identityExpireDate'].setErrors(null);

    }

  }

  populateGovernmentOrganizations() {

    this.subs.add(
      this.governmentOrganizationService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.governmentOrganizations = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }
  populatePartyTypes() {
    this.subs.add(
      this.caseService.getPartyTypes().subscribe(
        (result: any) => {
          this.partyTypes = result;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateProvinces() {
    this.subs.add(
      this.provinceService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.provinces = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }
  populateCities(provinceId) {
    this.subs.add(
      this.cityService.getWithQuery({ ...this.queryObject, provinceId }).subscribe(
        (result: any) => {
          this.cities = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateDistricts(cityId) {

    this.subs.add(
      this.districtService.getWithQuery({ ...this.queryObject, cityId }).subscribe(
        (result: any) => {
          this.districts = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  populateCountries() {
    this.subs.add(
      this.countryService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.countries = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }
  /**
     * Component init.
     */
  private init(): void {
    this.form = this.fb.group({
      id: [0],
      idInRelatedCase: [0],
      name: [null, Validators.compose([Validators.required])],
      partyType: ["", Validators.compose([Validators.required])],
      identityValue: [null],
      identityTypeId: [""],
      organizationId: [""],
      mobile: [null],
      provinceId: [""],
      cityId: [""],
      districtId: [""],
      street: [null],
      buidlingNumber: [null],
      postalCode: [null],
      addressDetails: [null],
      telephoneNumber: [
        null,
        [Validators.minLength(9), Validators.maxLength(9), Validators.pattern("^[0-9]*$")],
      ],
      nationalityId: [""],
      gender: [""],
      identitySource: [null],
      identityStartDate: [null],
      identityExpireDate: [null],
      commertialRegistrationNumber: [null, Validators.compose([Validators.minLength(6), Validators.maxLength(10), Validators.pattern("^[0-9]*$")])],
      companyName: [null]
    });
  }

  populateForm(party) {
    if (party.provinceId)
      this.populateCities(party.provinceId);
    if (party.cityId)
      this.populateDistricts(party.cityId);

    this.changePartyType(party.partyType);

    this.form.patchValue({
      id: party.id,
      idInRelatedCase: party.idInRelatedCase,
      name: party.name,
      partyType: party.partyType,
      identityValue: party.identityValue,
      identityTypeId: party.identityTypeId,
      organizationId: party.organizationId,
      mobile: party.mobile?.replace('0966', ''),
      provinceId: party.provinceId, //
      cityId: party.cityId,
      districtId: party.districtId,
      street: party.street,
      buidlingNumber: party.buidlingNumber,
      postalCode: party.postalCode,
      addressDetails: party.addressDetails,
      telephoneNumber: party.telephoneNumber,
      nationalityId: party.nationalityId,
      gender: party.gender,
      identitySource: party.identitySource,
      identityStartDate: party.identityStartDate,
      identityExpireDate: party.identityExpireDate,
      commertialRegistrationNumber: party.commertialRegistrationNumber,
      companyName: party.companyName
    });

    this.validateIdentityValue();

  }

  onSubmit() {
    let partiesExists = this.parties.filter(p =>
      (!this.form.value.id || p.id != this.form.value.id)
      && p.identityTypeId == this.form.value.identityTypeId
      && p.identityValue == this.form.value.identityValue
      && p.partyType == this.form.value.partyType
      && p.organizationId == this.form.value.organizationId);

    if (!partiesExists || partiesExists?.length == 0) {
      this.form.patchValue({
        identityStartDate: this.hijriConverter.calendarDateToDate(
          this.form.get('identityStartDate')?.value?.calendarStart
        ),
        identityExpireDate: this.hijriConverter.calendarDateToDate(
          this.form.get('identityExpireDate')?.value?.calendarStart
        )
      });
      this.dialogRef.close({
        ...this.form.value, partyTypeName: this.partyTypes.find(p => p.value == this.form.value.partyType).nameAr,
        organization: this.governmentOrganizations.find(p => p.id == this.form.value.organizationId)?.name
      });
    } else {
      Swal.fire({
        text: 'تم تسجيل الطرف مسبقا',
        confirmButtonText: 'حسناً',
      });
    }
  }


  populateIdentityTypes() {
    let queryObject: QueryObject = {
      sortBy: 'name',
      isSortAscending: true,
      page: 1,
      pageSize: 999,
    };
    this.subs.add(
      this.identityTypeService.getWithQuery(queryObject).subscribe(
        (result: any) => {
          this.identityTypes = result.data.items;
          this.filteredidentityTypes = result.data.items;
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
        }
      )
    );
  }

  changePartyType(partyType) {

    this.form.patchValue({
      identityValue: null,
      identityTypeId: "",
      organizationId: "",
      mobile: null,
      provinceId: "", //
      cityId: "",
      districtId: "",
      street: null,
      buidlingNumber: null,
      postalCode: null,
      addressDetails: null,
      telephoneNumber: null,
      nationalityId: "",
      gender: "",
      identitySource: null,
      identityStartDate: null,
      identityExpireDate: null,
      commertialRegistrationNumber: null,
      companyName: null,
      name: null
    });

    if (partyType == this.PartyTypes.Person) {
      this.filteredidentityTypes = this.identityTypes.filter(t => t.id != 6);
      // this.form.controls['identityValue'].setValidators(Validators.compose([Validators.compose([Validators.minLength(10), Validators.maxLength(10)])]));
      this.form.controls['identityTypeId'].setValidators(Validators.compose([Validators.required]));
      this.form.controls['name'].setValidators(Validators.compose([Validators.required]));

      this.form.controls['commertialRegistrationNumber'].setValidators(null);
      this.form.controls['organizationId'].setValidators(null);
      this.form.controls['companyName'].setValidators(null);

    }
    else if (partyType == this.PartyTypes.GovernmentalEntity) {
      this.form.controls['organizationId'].setValidators(Validators.compose([Validators.required]));

      this.form.controls['identityTypeId'].setValidators(null);
      this.form.controls['identityValue'].setValidators(null);
      this.form.controls['commertialRegistrationNumber'].setValidators(null);
      this.form.controls['companyName'].setValidators(null);
      this.form.controls['name'].setValidators(null);
    }
    else if (partyType == this.PartyTypes.CompanyOrInstitution) {
      this.form.controls['commertialRegistrationNumber'].setValidators(Validators.compose([Validators.required]));
      this.form.controls['companyName'].setValidators(Validators.compose([Validators.required]));
      this.form.controls['identityTypeId'].setValidators(null);
      this.form.controls['identityValue'].setValidators(null);
      this.form.controls['organizationId'].setValidators(null);
      this.form.controls['name'].setValidators(null);
    }

    this.form.controls['organizationId'].updateValueAndValidity();
    this.form.controls['identityTypeId'].updateValueAndValidity();
    this.form.controls['identityValue'].updateValueAndValidity();
    this.form.controls['commertialRegistrationNumber'].updateValueAndValidity();
    this.form.controls['companyName'].updateValueAndValidity();
    this.form.controls['name'].updateValueAndValidity();
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}

