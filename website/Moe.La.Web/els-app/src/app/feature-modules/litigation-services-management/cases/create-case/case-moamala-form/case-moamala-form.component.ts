import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MainCaseDetails } from 'app/core/models/case';
import { CaseMoamalatDetails } from 'app/core/models/case-moamalat';
import { AlertService } from 'app/core/services/alert.service';
import { CaseService } from 'app/core/services/case.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { Subscription } from 'rxjs';
import { SearchMoamalaComponent } from './search-moamala/search-moamala.component';
import Swal from 'sweetalert2';
import { MoamalatListItem } from 'app/core/models/moamalat';

@Component({
  selector: 'app-case-moamala-form',
  templateUrl: './case-moamala-form.component.html',
  styleUrls: ['./case-moamala-form.component.css']
})
export class CaseMoamalaFormComponent implements OnInit, OnChanges, OnDestroy {
  @Input() case: MainCaseDetails;

  caseMoamalat: CaseMoamalatDetails[] = [];
  selectedMoamala: MoamalatListItem = null;

  displayedColumns: string[] = [
    'moamalaNumber',
    'createdOn',
    'subject',
    'delete',
  ];

  moamalatDataSource!: MatTableDataSource<CaseMoamalatDetails>;

  private subs = new Subscription();

  constructor(
    private caseService: CaseService,
    private alert: AlertService,
    private dialog: MatDialog,
    private loaderService: LoaderService) { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['case']) {
      this.case = changes['case'].currentValue;
      if (this.case != null) {
        this.caseMoamalat = this.case.caseMoamalat;
        this.moamalatDataSource = new MatTableDataSource(this.caseMoamalat);
      }
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateMoamalat() {
    if (this.case.id)  {
      this.subs.add(
        this.caseService.getCaseMoamalat(this.case.id).subscribe(
          (result: any) => {
            this.caseMoamalat = result.data;
            this.moamalatDataSource = new MatTableDataSource(result.data);
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
          }
        )
      );
    }
  }

  onSearchMoamala() {
    this.subs.add(
      this.dialog.open(SearchMoamalaComponent, {
        width: '90%',
        data: { caseMoamalat: this.caseMoamalat }
      }).afterClosed().subscribe(
        (res: any) => {
          if (res) {
            this.selectedMoamala = res;
            this.addCaseMoamala();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  addCaseMoamala() {
    let existMoamala = this.caseMoamalat.filter(t => t.moamalaId === this.selectedMoamala.id)[0];
    if (existMoamala == null) {
      this.loaderService.startLoading(LoaderComponent);
      let moamalaDetails = new CaseMoamalatDetails();
      moamalaDetails.caseId = this.case.id;
      moamalaDetails.moamalaId = this.selectedMoamala.id;

      this.subs.add(
        this.caseService.addCaseMoamalat(moamalaDetails).subscribe(
          () => {
            this.loaderService.stopLoading();
            this.populateMoamalat();

            this.alert.succuss('تم إضافة المعاملة بنجاح');
            this.selectedMoamala = null;
          },
          (error) => {
            console.error(error);
            this.alert.error('فشلت عملية جلب البيانات !');
            this.loaderService.stopLoading();
          }
        )
      );
    }
    else {
      this.alert.error('المعامله مرتبطة بالقضية');
    }
  }

  onDelete(caseMoamala) {
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
          this.caseService.deleteCaseMoamalat(caseMoamala.caseId, caseMoamala.moamalaId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              let existMoamala = this.caseMoamalat.filter(t => t.moamalaId === caseMoamala.moamalaId)[0];
              const index = this.caseMoamalat.indexOf(existMoamala, 0);
              if (index > -1) {
                this.caseMoamalat.splice(index, 1);
              }
              this.moamalatDataSource = new MatTableDataSource(this.caseMoamalat);

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

}
