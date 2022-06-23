import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import * as CustomEditor from 'app/shared/ckeditor-build/ckeditor';
import { Router } from '@angular/router';

import { AlertService } from 'app/core/services/alert.service';
import { RequestService } from 'app/core/services/request.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { RequestStatus } from 'app/core/enums/RequestStatus';
import { SuggestedOpinon } from 'app/core/enums/SuggestedOpinon';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-objection-permit-request-form',
  templateUrl: './objection-permit-request-form.component.html',
  styleUrls: ['./objection-permit-request-form.component.css']
})
export class ObjectionPermitRequestFormComponent implements OnInit {
  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  caseId: any;
  requestStatus: any;
  public Editor = CustomEditor;
  public config = {
    language: 'ar'
  };
  public get SuggestedOpinon(): typeof SuggestedOpinon {
    return SuggestedOpinon;
  }
  public get RequestStatus(): typeof RequestStatus {
    return RequestStatus;
  }
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ObjectionPermitRequestFormComponent>,
    private authService: AuthService,
    private requestService: RequestService,
    private loaderService: LoaderService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (this.data.caseId) this.caseId = this.data.caseId;
  }

  ngOnInit(): void {
    this.init();
  }

  /**
* Component init.
*/
  private init(): void {
    this.form = this.fb.group({
      id: 0,
      caseId: this.caseId,
      suggestedOpinon: [null, Validators.compose([Validators.required, Validators.maxLength(4000)])],
      note: ['', Validators.compose([Validators.required, Validators.maxLength(4000)])],
      requestStatus: [RequestStatus.New],
      researcherId: [this.authService.currentUser?.id]
    });
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.requestService.createObjectionPermitRequest(this.form.value).subscribe(result => {
        this.loaderService.stopLoading();
        this.onCancel(result);
      }, error => {
        this.loaderService.stopLoading();
      }));
  }

  onCancel(result: any = null) {
    this.dialogRef.close(result);
  }

}

