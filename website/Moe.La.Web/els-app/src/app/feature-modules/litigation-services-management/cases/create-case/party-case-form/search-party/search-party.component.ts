import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';

import { PartyQueryObject } from 'app/core/models/query-objects';
import { PartyService } from 'app/core/services/party.service';
import { LoaderService } from 'app/core/services/loader.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { PartyDetails } from 'app/core/models/party';
import { even } from '@rxweb/reactive-form-validators';

@Component({
  selector: 'app-search-party',
  templateUrl: './search-party.component.html',
  styleUrls: ['./search-party.component.css']
})
export class SearchPartyComponent implements OnInit {

  columnsToDisplay = [
    'name',
    'partyTypeName',
    'actions',
  ];
  queryObject: PartyQueryObject = new PartyQueryObject({
    sortBy: 'name',
    isSortAscending: true
  });

  totalItems!: number;
  searchForm: FormGroup = Object.create(null);
  dataSource!: MatTableDataSource<PartyDetails>;
  orgPlaceHolder: string = 'رقم الهوية' ;

  private subs = new Subscription();
  constructor( private fb: FormBuilder,
    private partyService: PartyService,
    public dialogRef: MatDialogRef<SearchPartyComponent>,
    private loaderService: LoaderService,
    private alert: AlertService
    ) { }

    ngOnInit(): void {
      this.init();
      this.populateParties();
    }
    ngOnDestroy() {
      this.subs.unsubscribe();
    }

    init() {
      this.searchForm = this.fb.group({
        name: [],
        partyType: [],
        identityValue: []
      });
    }

    onFilter() {
      this.queryObject = new PartyQueryObject();
      this.queryObject.name = this.searchForm.controls['name'].value;
      this.queryObject.partyType = this.searchForm.controls['partyType'].value;
      this.queryObject.identityValue = this.searchForm.controls['identityValue'].value;
      this.populateParties();
    }

    onClearFilter() {
      this.queryObject = new PartyQueryObject();
      this.searchForm.reset();
      this.populateParties();
    }

    populateParties() {

      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.partyService.getWithQuery(this.queryObject).subscribe(
          (result: any) => {
            this.totalItems = result.data.totalItems;
            this.dataSource = new MatTableDataSource(result.data.items);
            this.loaderService.stopLoading();
          },
          (error) => {
            console.error(error);
            this.alert.error(error);
            this.loaderService.stopLoading();
          }
        )
      );
    }

    onPageChange(page: number) {
      this.queryObject.page = page + 1;
      this.populateParties();
    }

    onSubmit(selectedParty: PartyDetails) {
      this.onCancel(selectedParty);
    }

    onCancel(result: any = null): void {
      this.dialogRef.close(result);
    }
    isOrg = false;
    onChangeSource(event : any){
      if(event == 3){
        this.orgPlaceHolder = 'رقم التسجيل';
        this.isOrg = false;
      }
      else if(event == 1) {
        this.orgPlaceHolder = 'رقم الهوية';
        this.isOrg = false;
      }
      else
      {
        this.isOrg = true;
      }
    }
}
