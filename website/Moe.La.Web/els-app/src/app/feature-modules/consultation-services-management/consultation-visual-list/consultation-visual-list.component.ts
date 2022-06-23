import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs/Rx';
import Swal from 'sweetalert2';

import { AlertService } from 'app/core/services/alert.service';
import { ConsultationService } from 'app/core/services/consultation.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-consultation-visual-list',
  templateUrl: './consultation-visual-list.component.html',
  styleUrls: ['./consultation-visual-list.component.css']
})
export class ConsultationVisualListComponent implements OnInit {

  @Input('consultationVisuals') consultationVisuals: any;
  @Output('set-data-list') populateMoamalat = new EventEmitter<any>(); // value will returns to $event variable

  private subs = new Subscription();

  ngOnInit(): void {
  }
  visualsDisplayedColumns: string[] = [
    'position',
    'material',
    'visuals',
    'actions',
  ];

  constructor(
    private matDialog: MatDialog,
    private loaderService: LoaderService,
    private consultationService: ConsultationService,
    private alert: AlertService,
  ) { }

  onDelete(visulaId: number) {
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
          this.consultationService.deleteVisual(visulaId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateMoamalat.emit();
              this.resetControls();
            },
            (error: any) => {
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
    this.matDialog.closeAll();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['consultationVisuals']) {
      this.consultationVisuals = changes['consultationVisuals'].currentValue;
    }
  }

}
