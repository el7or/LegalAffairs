import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Province } from 'app/core/models/province';
import { City } from 'app/core/models/city';
import { CaseService } from 'app/core/services/case.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { CaseGround } from 'app/core/models/case-ground';

@Component({
  selector: 'app-case-ground-form',
  templateUrl: './case-ground-form.component.html',
  styleUrls: ['./case-ground-form.component.css'],
})
export class CaseGroundUpdateFormComponent implements OnInit {
  form: FormGroup = Object.create(null);
  subs = new Subscription();
  ground: CaseGround;
  groundsList: CaseGround[] = [];

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CaseGroundUpdateFormComponent>,
    private caseService: CaseService,
    private alert: AlertService,
    private loaderService: LoaderService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.ground) this.ground = this.data.ground;
    if (this.data.groundsList) this.groundsList = this.data.groundsList;
  }

  ngOnInit() {
    this.form = this.fb.group({
      id: [this.ground.id],
      caseId: [this.ground.caseId, Validators.required],
      text: [this.ground.text, Validators.required],
    });
  }

  onSubmit() {
    // check if ground already exists
    let groundExist = this.groundsList.find(
      (t) => t.text === this.form.value.text 
          && t.id != this.form.value.id
    );

    if (groundExist) {
      this.alert.error('السند موجود فى القضية');
      return;
    }

    this.loaderService.startLoading(LoaderComponent);

    let result$ = this.ground.id
      ? this.caseService.editGround(this.form.value)
      : this.caseService.addGround(this.form.value);
    this.subs.add(
      result$.subscribe(
        (result: any) => {
          this.loaderService.stopLoading();
          let msg = this.ground.id
            ? 'تم تعديل السند بنجاح'
            : 'تم إضافة السند بنجاح';
          this.alert.succuss(msg);
          this.ground.id
            ? (this.groundsList.find((n) => n.id == this.ground.id).text =
                this.form.value.text)
            : this.groundsList.push(result.data);

          this.dialogRef.close(this.groundsList);
        },
        (error) => {
          console.error(error);
          this.alert.error(
            this.ground.id ? 'فشلت عملية التعديل !' : 'فشلت عملية الإضافة !'
          );
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onCancel(result: any = null): void {
    this.dialogRef.close(result);
  }
}
