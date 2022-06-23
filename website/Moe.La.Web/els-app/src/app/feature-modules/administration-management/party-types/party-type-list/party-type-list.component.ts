import { Location } from '@angular/common';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';

import { PartyTypeFormComponent } from '../party-type-form/party-type-form.component';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LoaderService } from 'app/core/services/loader.service';
import { QueryObject } from 'app/core/models/query-objects';
import { AlertService } from 'app/core/services/alert.service';
import { PartyTypeService } from 'app/core/services/party-type.service';

@Component({
  selector: 'app-party-type-list',
  templateUrl: './party-type-list.component.html',
  styleUrls: ['./party-type-list.component.css']
})
export class PartyTypeListComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'actions'];

  partyTypes: any[] = [];
  allPartyTypes: any[] = [];

  searchText: string = '';
  showFilter: boolean = false;

  form: FormGroup = Object.create(null);

  queryResult: any = {
    totalItems: 0,
    items: [],
  };

  PAGE_SIZE: number = 20;

  queryObject: QueryObject = {
    sortBy: 'name',
    isSortAscending: true,
    page: 1,
    pageSize: this.PAGE_SIZE,
  };

  @ViewChild('template', { static: true }) template:
    | TemplateRef<any>
    | undefined;

  private subs = new Subscription();

  constructor(
    public partyTypeService: PartyTypeService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private alert: AlertService,
    private location: Location,
    private loaderService: LoaderService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: [''],
    });
    this.populatePartyTypes();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populatePartyTypes() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.partyTypeService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.queryResult.totalItems = result.data.totalItems;
          this.allPartyTypes = result.data.items;
          this.partyTypes = this.allPartyTypes;
          this.loaderService.stopLoading()
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading()
        }
      )
    );
  }

  onSearch() {
    let nameText = this.form.controls['name'].value;
    if (this.searchText.length > 0)
      this.partyTypes = this.allPartyTypes.filter(
        (item) =>
          item.name.includes(this.searchText)
      );
    else if (nameText.length > 0)
      this.partyTypes = this.allPartyTypes.filter(
        (item) =>
          item.name.includes(nameText)
      );
    else this.partyTypes = this.allPartyTypes;
  }

  onClearFilter() {
    this.form.reset();
    this.populatePartyTypes();
  }

  sortBy(column: string) {
    this.queryObject.sortBy = column;
    this.queryObject.isSortAscending = !this.queryObject.isSortAscending;
    this.populatePartyTypes();
  }

  onPageChange(page: number) {
    this.queryObject.page = page + 1;
    this.populatePartyTypes();
  }

  openModal(id: any): void {

    const dialogRef = this.dialog.open(PartyTypeFormComponent, {
      width: '30em',
      data: { partyTypeId: id },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populatePartyTypes();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onDelete(courtId: number) {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من إتمام عملية الحذف؟',
      icon: 'error',
      showCancelButton: true,
      confirmButtonColor: '#ff3d71',
      confirmButtonText: 'حذف',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.partyTypeService.delete(courtId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populatePartyTypes();
              this.resetControls();
            },
            (error) => {
              console.error(error);
              this.loaderService.stopLoading();
              this.alert.error('فشلت عملية الحذف !');
            }
          )
        );
      }
    });
  }

  resetControls() {
    this.dialog.closeAll();
  }
}
