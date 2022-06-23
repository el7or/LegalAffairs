import { BoardMeetingComponent } from './../board-meeting/board-meeting.component';
import { Component, OnInit, ViewChild, TemplateRef, ChangeDetectorRef } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Location } from '@angular/common';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatTabChangeEvent } from '@angular/material/tabs';
import Swal from 'sweetalert2';

import { LegalMemoNoteQueryObject } from 'app/core/models/query-objects';
import { LegalMemoService } from 'app/core/services/legal-memo.service';
import { LoaderService } from 'app/core/services/loader.service';
import { AlertService } from 'app/core/services/alert.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { MemoNoteFormComponent } from '../memo-note-form/memo-note-form.component';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { LegalMemoStatus } from 'app/core/enums/LegalMemoStatus';
import { LegalMemoNote, LegalMemoNoteList } from 'app/core/models/legal-memo-note';
import { CaseService } from 'app/core/services/case.service';
import { CaseDetails } from 'app/core/models/case';
import { AssignMemoToBoardFormComponent } from '../assign-memo-to-board-form/assign-memo-to-board-form.component';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { CaseStatus } from 'app/core/enums/CaseStatus';
import { MemoRejectFormComponent } from '../memo-reject-form/memo-reject-form.component';
import { LegalBoardMemo } from 'app/core/models/legal-board-memo';

@Component({
  selector: 'app-legal-memo-view',
  templateUrl: './legal-memo-view.component.html',
  styleUrls: ['./legal-memo-view.component.css'],
})
export class LegalMemoViewComponent implements OnInit {
  legalMemoId: number = 0;
  private subs = new Subscription();
  memoDetails: any;
  displayedColumns: string[] = [
    'position',
    'reviewNumber',
    'noteText',
    'createdBy',
    'boardName',
    'createdOn',
    'creationTime',
    'actions',
  ];

  isView: boolean = false;
  isReview: boolean = false;
  isBoardReview: boolean = false;

  queryObject: LegalMemoNoteQueryObject = new LegalMemoNoteQueryObject({
    sortBy: 'createdOn',
    pageSize: 999
  });
  @ViewChild(MatSort) sort!: MatSort;
  hasNewNotes: boolean = false;
  hasSecondaryLegalBoard: boolean = false;
  legalMemoNotes: any;
  legalBoards: any;
  case: CaseDetails | undefined;
  legalBoardMemo: LegalBoardMemo = new LegalBoardMemo();

  dataSource: MatTableDataSource<LegalMemoNoteList> = new MatTableDataSource<LegalMemoNoteList>();

  public get AppRole(): typeof AppRole {
    return AppRole;
  }

  public get LegalMemoStatus(): typeof LegalMemoStatus {
    return LegalMemoStatus;
  }
  public get CaseStatus(): typeof CaseStatus {
    return CaseStatus;
  }

  constructor(
    private activatedRouter: ActivatedRoute,
    private router: Router,
    private legalMemoService: LegalMemoService,
    private boardService: LegalBoardService,
    private caseService: CaseService,
    private loaderService: LoaderService,
    private alert: AlertService,
    private dialog: MatDialog,
    public authService: AuthService,
    private cdr: ChangeDetectorRef,
    private location: Location
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.legalMemoId = +id;
      }
    });

    /// change content of page from routing///
    if (this.router.url.includes('/view/'))
      this.isView = true;
    else if (this.router.url.includes('/review/'))
      this.isReview = true;
    else if (this.router.url.includes('/board-review/'))
      this.isBoardReview = true;

  }

  ngOnInit(): void {
    this.populateMemoDetails();
    this.populatelegalBoards();
  }


  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort?.sortChange.subscribe(
        (result: any) => {
          this.queryObject.sortBy = result.active;
          this.queryObject.isSortAscending = result.direction == 'asc';
          this.populateLegalMemoNotes();
        },
        (error: any) => {
          console.error(error);
          this.alert.error(error);
        }
      )
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  tabClick(data: MatTabChangeEvent) {
    if (data.index == 1) {
      this.populateCaseDetails();
    }
  }

  populateMemoDetails() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalMemoService.get(this.legalMemoId).subscribe(
        (result: any) => {
          this.memoDetails = result.data;
          this.populateLegalMemoNotes();
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }));
  }

  populatelegalBoards() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.boardService.getLegalBoard().subscribe(
        (result: any) => {
          this.legalBoards = result.data;
          this.hasSecondaryLegalBoard = this.legalBoards.length > 0 ? true : false;
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }));
  }
  populateCaseDetails() {
    if (this.memoDetails.initialCaseId) {
      this.subs.add(
        this.caseService.get(this.memoDetails.initialCaseId).subscribe((res: any) => {
          this.case = res.data;
        }, (error) => {
          this.alert.error("فشلت  عملية جلب البيانات");
          console.error(error);
        })
      )
    }
  }

  populateLegalMemoNotes() {
    this.queryObject.legalMemoId = this.legalMemoId;

    this.subs.add(
      this.legalMemoService.getNotesWithQuery(this.queryObject).subscribe((result: any) => {
        this.loaderService.stopLoading();
        this.dataSource = new MatTableDataSource(result.data.items);

        // we will check if the current user has added notes in the current review number.
        this.hasNewNotes = result.data.items.filter((m: any) =>
          m.reviewNumber == this.memoDetails.reviewNumber &&
          this.authService.currentUser?.id == m.createdBy.id).length > 0;
      },
        (error) => {
          console.error(error);
          this.alert.error('فشلت عملية جلب البيانات !');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onDelete(legalMemoNoteId: number) {
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
          this.legalMemoService.deleteNote(legalMemoNoteId).subscribe(
            () => {
              this.loaderService.stopLoading();
              this.alert.succuss('تمت عملية الحذف  بنجاح');
              this.populateLegalMemoNotes();
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
  changeLgalMemoStatus(legalMemoStatusId: number) {
    let message: string = "";
    let confirmButtonText: string = "حفظ";
    let confirmButtonColor = "";
    if (legalMemoStatusId == LegalMemoStatus.Accepted) {
      message = "هل أنت متأكد من إتمام عملية قبول المذكرة؟";
      confirmButtonText = "قبول";
      confirmButtonColor = "#28a745";
    }
    else if (legalMemoStatusId == LegalMemoStatus.Returned && this.isReview) {
      message = "هل أنت متأكد من إتمام عملية إعادة المذكرة للباحث لإعادة للصياغة؟";
      confirmButtonText = "إعادة";
      confirmButtonColor = "#ff3d71";

    }
    else if (legalMemoStatusId == LegalMemoStatus.Approved && this.isReview) {
      message = "هل أنت متأكد من إتمام عملية اعتماد المذكرة؟";
      confirmButtonText = "اعتمد";
      confirmButtonColor = "#28a745";

    }
    else if (legalMemoStatusId == LegalMemoStatus.Approved && this.isBoardReview) {
      message = "هل أنت متأكد من إتمام عملية اعتماد المذكرة من  اللجنة  الرئيسية؟";
      confirmButtonText = "اعتمد";
      confirmButtonColor = "#28a745";

    }
    else if (legalMemoStatusId == LegalMemoStatus.Returned && this.isBoardReview) {
      message = "هل أنت متأكد من إتمام عملية إعادة المذكرة إلى الصياغة؟";
      confirmButtonText = "إعادة";
      confirmButtonColor = "#ff3d71";

    }

    Swal.fire({
      title: 'تأكيد',
      text: message,
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: confirmButtonColor,
      confirmButtonText: confirmButtonText,
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(this.legalMemoService.changeLegalMemoStatus(this.legalMemoId, legalMemoStatusId)
          .subscribe((res: any) => {
            this.alert.succuss("تمت عملية التغيير بنجاح");
            this.loaderService.stopLoading();
            this.location.back();
            //this.router.navigateByUrl('memos/review-list');
            // this.router.navigateByUrl('memos/board-review-list');
          }, (error) => {
            this.alert.error("فشلت عملية التغيير");
            this.loaderService.stopLoading();
            console.error(error);
          }))
      }
    });
  }

  assignMemoToBoard(LegalBoardType: string) { //legalMemoId: number

    this.legalBoardMemo = {
      id: this.memoDetails.legalBoardMemos[0]?.id ?? 0,
      legalMemoId: this.legalMemoId,
      legalBoardId: this.memoDetails.legalBoardMemos[0]?.legalBoardId ?? 0
    };

    if (LegalBoardType == "Major") {
      let message: string = "";
      let confirmButtonText: string = "إحالة";
      if (this.isReview && this.memoDetails.status.id == LegalMemoStatus.RaisingSubBoardHead) {
        message = "هل أنت متأكد من إتمام عملية إعادة المذكرة إلى اللجنة الرئيسية؟";
        confirmButtonText = "إعادة";
      }
      else if (this.isBoardReview)
        message = "هل أنت متأكد من تحويل المذكرة إلى اللجنة الرئيسية؟";
      else if (this.isReview && this.memoDetails.status.id == LegalMemoStatus.RaisingConsultant)
        message = "هل أنت متأكد من تحويل المذكرة إلى اللجنة الرئيسية؟";
      Swal.fire({
        title: 'تأكيد',
        text: message,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        confirmButtonText: confirmButtonText,
        cancelButtonText: 'إلغاء',
      }).then((result: any) => {
        if (result.value) {
          this.loaderService.startLoading(LoaderComponent);

          let result$ = (this.legalBoardMemo.id) ? this.boardService.updateLegalBoardMemo(this.legalBoardMemo) : this.boardService.createLegalBoardMemo(this.legalBoardMemo);
          this.subs.add(
            result$.subscribe(
              (res: any) => {
                if (res.isSuccess) {
                  this.subs.add(this.legalMemoService.changeLegalMemoStatus(this.legalMemoId, LegalMemoStatus.RaisingMainBoardHead)
                    .subscribe((res: any) => {
                      this.alert.succuss("تمت عملية التغيير بنجاح");
                      this.loaderService.stopLoading();
                      if (this.isReview)
                        this.router.navigateByUrl('memos/review-list');
                      else if (this.isBoardReview)
                        this.populateMemoDetails();

                    }, (error) => {
                      this.alert.error("فشلت عملية التغيير");
                      this.loaderService.stopLoading();
                      console.error(error);
                    }));
                }
                else {
                  this.loaderService.stopLoading();
                }
              }, (error) => {
                this.loaderService.stopLoading();
                this.alert.error("فشلت عملية التغيير");
                console.error(error);
              }));
        }
      });
    }
    else {
      const dialogRef = this.dialog.open(AssignMemoToBoardFormComponent, {
        width: '30em',
        data: { legalBoardMemo: this.legalBoardMemo },
      });
      this.subs.add(
        dialogRef.afterClosed().subscribe(
          (res) => {
            if (res) {
              this.router.navigateByUrl('memos/board-review-list');
            }
          },
          (error) => {
            console.error(error);
          }
        )
      );
    }
  }

  sortData(sort: Sort) {
    const data = this.dataSource.data.slice();
    if (!sort.active || sort.direction === '') {
      this.dataSource.data = data;
      return;
    }

    this.dataSource.data = data.sort((a: LegalMemoNoteList, b: LegalMemoNoteList) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'createdOn': return this.compare(a.createdOn, b.createdOn, isAsc);
        case 'reviewNumber': return this.compare(a.reviewNumber, b.reviewNumber, isAsc);
        case 'creationTime': return this.compare(a.creationTime, b.creationTime, isAsc);
        case 'text': return this.compare(a.text, b.text, isAsc);
        default: return 0;
      }
    });
  }

  compare(a: number | string, b: number | string, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

  openModal(id: number): void {

    const legalMemoNote: LegalMemoNote = {
      id: id,
      legalMemoId: this.legalMemoId,
      reviewNumber: this.memoDetails.reviewNumber
    };

    if (this.memoDetails.status.id == LegalMemoStatus.RaisingMainBoardHead || this.memoDetails.status.id == LegalMemoStatus.RaisingSubBoardHead
      || this.memoDetails.status.id == LegalMemoStatus.Rejected) {
      legalMemoNote.legalBoardId = this.memoDetails.legalBoardMemos[0]?.legalBoardId;
    }

    const dialogRef = this.dialog.open(MemoNoteFormComponent, {
      width: '75%',
      data: { legalMemoNote: legalMemoNote },
    });

    this.subs.add(
      dialogRef.afterClosed().subscribe(
        (res) => {
          if (res) {
            this.populateLegalMemoNotes();
            this.populateMemoDetails();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onPrint() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.legalMemoService.printLegalMemo(this.memoDetails).subscribe(
        (data) => {
          var downloadURL = window.URL.createObjectURL(data);
          var link = document.createElement('a');
          link.href = downloadURL;
          link.target = '_blank';
          link.download = `${this.memoDetails.name}`;
          link.click();
          this.loaderService.stopLoading();
        },
        (error) => {
          console.error(error);
          this.alert.error('فشل طباعة المذكرة');
          this.loaderService.stopLoading();
        }
      )
    );
  }

  onChooseBoardMembers() {
    const boardMeeting = {
      // boardMeetingId: this.memoDetails.boardMeetingId,
      legalMemoId: this.legalMemoId,
      legalBoardId: this.memoDetails.legalBoardMemos[0].legalBoardId
    };
    this.dialog.open(BoardMeetingComponent, {
      width: '75%',
      data: { boardMeeting: boardMeeting },
    }).afterClosed().subscribe(
      (res) => {
        if (res) {
          this.populateMemoDetails();
        }
      },
      (error) => {
        console.error(error);
      }
    )
  }




  raisToAllBoardsHead() {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متاكد من عملية قبول ورفع المذكرة لرئيس اللجان للاعتماد؟',
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#28a745',
      confirmButtonText: 'قبول',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.legalMemoService.raisToAllBoardsHead(this.legalMemoId).subscribe(
            (result: any) => {

              this.loaderService.stopLoading();
              if (this.isBoardReview)
                this.router.navigateByUrl('memos/board-review-list');
              else
                this.router.navigateByUrl('memos/review-list');

            },
            (error) => {
              console.error(error);
              this.alert.error('فشلت عملية قبول المذكرة !');
              this.loaderService.stopLoading();
            }));
      }
    });

  }

  approve() {
    Swal.fire({
      title: 'تأكيد',
      text: 'هل أنت متأكد من عملية اعتماد المذكرة؟',
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#28a745',
      confirmButtonText: 'اعتماد',
      cancelButtonText: 'إلغاء',
    }).then((result: any) => {
      if (result.value) {
        this.loaderService.startLoading(LoaderComponent);
        this.subs.add(
          this.legalMemoService.approve(this.legalMemoId).subscribe(
            (result: any) => {

              this.loaderService.stopLoading();

              this.router.navigateByUrl('memos/list');

            },
            (error) => {
              console.error(error);
              this.alert.error('فشلت عملية اعتماد المذكرة !');
              this.loaderService.stopLoading();
            }));

      }
    });
  }

  reject(status: LegalMemoStatus) {
    this.subs.add(
      this.dialog.open(MemoRejectFormComponent, {
        width: '30em',
        data: {
          id: this.legalMemoId,
          status
        }
      }).afterClosed().subscribe(
        (result) => {
          if (result) {
            if (this.isBoardReview)
              this.router.navigateByUrl('memos/board-review-list');
            else if (this.isReview)
              this.router.navigateByUrl('memos/review-list');
            else
              this.router.navigateByUrl('memos/list');

          }
        }
      )
    );
  }

  onBack(): void {
    this.location.back();
  }
}
