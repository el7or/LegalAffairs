import { Location } from '@angular/common';
import { Component, Input } from '@angular/core';

import { Attachment, GroupNames } from 'app/core/models/attachment';
import { HearingDetails } from 'app/core/models/hearing';

@Component({
  selector: 'app-hearing-summary-view',
  templateUrl: './hearing-summary-view.component.html',
  styleUrls: ['./hearing-summary-view.component.css'],
})
export class HearingSummaryViewComponent {
  @Input() caseSubject?: string;
  @Input() hearing?: HearingDetails;
  @Input() attachmentsToUpdate?: Attachment[] = [];

  public get GroupNames(): typeof GroupNames {
    return GroupNames;
  }

  constructor(public location: Location) { }
}
