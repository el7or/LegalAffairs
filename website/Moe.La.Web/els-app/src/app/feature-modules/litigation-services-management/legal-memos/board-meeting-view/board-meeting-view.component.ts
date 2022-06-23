import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from 'app/core/services/alert.service';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-board-meeting-view',
  templateUrl: './board-meeting-view.component.html',
  styleUrls: ['./board-meeting-view.component.css']
})
export class BoardMeetingViewComponent implements OnInit {
  boardMeetingId: number;
  meetingDetails: any;

  subs = new Subscription();

  constructor(
    private loaderService: LoaderService,
    private alert: AlertService,
    private activatedRouter: ActivatedRoute,
    private boardService: LegalBoardService,
    public location: Location) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.boardMeetingId = +id;
      }
    });
  }

  ngOnInit(): void {
    this.populateMeetingDetails();
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateMeetingDetails() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.boardService.getMeeting(this.boardMeetingId).subscribe(
        (result: any) => {
          this.meetingDetails = result.data;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }));
  }

}
