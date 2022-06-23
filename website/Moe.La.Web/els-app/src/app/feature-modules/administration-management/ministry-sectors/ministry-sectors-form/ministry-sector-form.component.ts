import { Component, OnInit, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { KeyValuePairs } from 'app/core/models/key-value-pairs';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { MinistrySectorService } from 'app/core/services/ministry-sectors.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-ministry-sector-form',
  templateUrl: './ministry-sector-form.component.html',
  styleUrls: ['./ministry-sector-form.component.css']
})
export class MinistrySectorFormComponent implements OnInit, AfterViewInit, OnDestroy {
  ministrySectorId: number = 0;
  ministrySector: KeyValuePairs = new KeyValuePairs();
  form: FormGroup = Object.create(null);
  subs = new Subscription();

  constructor(
    private fb: FormBuilder,
    public ministrySectorService: MinistrySectorService,
    private alert: AlertService,
    public dialogRef: MatDialogRef<MinistrySectorFormComponent>,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.ministrySectorId)
      this.ministrySectorId = this.data.ministrySectorId;
  }

  ngOnInit() {
    this.init();
    ///
    if (this.ministrySectorId) {
      this.loaderService.startLoading(LoaderComponent);
      this.subs.add(
        this.ministrySectorService.get(this.ministrySectorId).subscribe(
          (result: any) => {
            this.ministrySector = result.data;
            this.populateMinistrySectorForm(result.data);
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
    });
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateMinistrySectorForm(ministrySector: KeyValuePairs) {
    this.form.patchValue({
      id: ministrySector?.id,
      name: ministrySector?.name,
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.ministrySectorId
      ? this.ministrySectorService.update(this.form.value)
      : this.ministrySectorService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          this.onCancel(res);
          let message = this.ministrySectorId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
        },
        (error) => {
          this.loaderService.stopLoading();
          console.error(error);
          let message = this.ministrySectorId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
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
    if (nameCtrl.valid && this.ministrySector) {
      if (this.ministrySector.name === name) {
        nameCtrl.markAsPristine();
      }
      else {
        nameCtrl.markAsPending();
        this.ministrySectorService
          .isNameExists(name)
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


}
