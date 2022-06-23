import { Component, OnInit} from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute} from '@angular/router';
import { Location } from '@angular/common';
import { LoaderService } from 'app/core/services/loader.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { AuthService } from 'app/core/services/auth.service';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { LegalBoardMemberType } from 'app/core/enums/LegalBoardMemberType';

@Component({
  selector: 'app-legal-board-view',
  templateUrl: './legal-board-view.component.html',
  styleUrls: ['./legal-board-view.component.css']
})
export class LegalBoardViewComponent implements OnInit {

  legalBoardId: number = 0;
  private subs = new Subscription();
  boardDetails: any;
  displayedColumns: string[] = ['name', 'adjective', 'status', 'startDate', 'boardHead'];

  boardHead:string='';

  public get LegalBoardMemberType(): typeof LegalBoardMemberType {
    return LegalBoardMemberType;
  }

  constructor(
    private activatedRouter: ActivatedRoute,
    private boardService: LegalBoardService,
    private loaderService: LoaderService,
    private alert: AlertService,
    public authService: AuthService,
    private location: Location
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.legalBoardId = +id;
      }
    });
  }

  ngOnInit(): void {
    this.populateLegalBoardDetails();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  populateLegalBoardDetails() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.boardService.get(this.legalBoardId).subscribe(
        (result: any) => {
          this.boardDetails = result.data;
          this.boardHead=this.boardDetails.legalBoardMembers.find(m=>m.boardHead)?.userName;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }));
  }
  onBack(): void {
    this.location.back();
  }
}
