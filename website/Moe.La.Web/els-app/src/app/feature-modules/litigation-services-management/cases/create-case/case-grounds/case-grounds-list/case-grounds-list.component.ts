import { Component, Input, OnDestroy, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { ResearcherConsultantService } from 'app/core/services/researcher-consultant.service';
import { CaseService } from 'app/core/services/case.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { MainCaseDetails } from 'app/core/models/case';
import { MatTableDataSource } from '@angular/material/table';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { CaseGroundUpdateFormComponent } from '../case-ground-form/case-ground-form.component';
import { CaseGround } from 'app/core/models/case-ground';

@Component({
  selector: 'app-case-grounds-list',
  templateUrl: './case-grounds-list.component.html',
  styleUrls: ['./case-grounds-list.component.css']
})
export class CaseGroundsFormComponent implements OnDestroy {

  // groundForm: FormGroup = Object.create(null);
  private subs = new Subscription();
  groundsList: any[] = [];
  @Input() case: MainCaseDetails;
  displayedGroundsColumns: any[] = [
    'text',
    'actions',
  ];
  groundsDataSource!: MatTableDataSource<any>;

  constructor(
    private fb: FormBuilder,
    private caseService: CaseService,
    public researcherConsultantService: ResearcherConsultantService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private dialog: MatDialog

  ) {

  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  ngOnInit(): void {
    //this.init();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['case']) {
      this.case = changes['case'].currentValue;
      this.groundsList = this.case?.caseGrounds;
      this.groundsDataSource = new MatTableDataSource(this.groundsList);
    }

  }

  editGround(caseGround:CaseGround) {

    if(caseGround==null){
      caseGround=new CaseGround({
        id:0,
        caseId:this.case.id,
        text:''
      });
    }

    const dialogRef = this.dialog.open(CaseGroundUpdateFormComponent, {
      width: '30em',
      data: {
        ground: caseGround,
        groundsList: this.groundsList },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.groundsDataSource = new MatTableDataSource(this.groundsList);
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }
  removeGround(id: number, index: number) {
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
          this.caseService.deleteGround(id).subscribe(
            (result: any) => {
              this.loaderService.stopLoading();
              this.groundsList.splice(index, 1);
              this.groundsDataSource = new MatTableDataSource(this.groundsList);
              this.alert.succuss('تم حذف السند بنجاح');
            },
            (error) => {
              console.error(error);
              this.alert.error('فشلت عملية الحذف !');
              this.loaderService.stopLoading();
            }
          )
        );
      }
    });
  }

}

