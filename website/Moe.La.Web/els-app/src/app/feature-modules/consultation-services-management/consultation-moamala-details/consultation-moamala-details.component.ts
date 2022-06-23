import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Rx';

import { GroupNames } from 'app/core/models/attachment';
import { MoamalaDetails } from 'app/core/models/moamalat';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MoamalatService } from 'app/core/services/moamalat.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';

@Component({
  selector: 'app-consultation-moamala-details',
  templateUrl: './consultation-moamala-details.component.html',
  styleUrls: ['./consultation-moamala-details.component.css']
})
export class ConsultationMoamalaDetailsComponent implements OnInit, OnDestroy {
  @Input('moamalaId') moamalaId!: number;
  @Input('showAttachments') showAttachments: boolean = false;

  moamalaDetails!: MoamalaDetails;

  subs = new Subscription();

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }

  constructor(private moamalatService: MoamalatService,
    private loaderService: LoaderService,
    private alert: AlertService,) { }

  ngOnInit(): void {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.moamalatService.get(this.moamalaId).subscribe(
        (result: any) => {
          this.moamalaDetails = result.data;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت  عملية جلب البيانات');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

}
