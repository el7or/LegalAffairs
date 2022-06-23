import {
  Component,
  OnInit,
  OnDestroy,
  Injectable,
  ViewChild,
  ChangeDetectorRef,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Location, formatDate } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MemberFormComponent } from '../member-form/member-form.component';
import { AuthService } from 'app/core/services/auth.service';
import { AppRole } from 'app/core/models/role';
import { LegalBoardMemberType } from 'app/core/enums/LegalBoardMemberType';
import { UserService } from 'app/core/services/user.service';
import {
  LegalBoardMembersListItem,
  LegalBoardListItem,
  LegalBoardMembers,
} from 'app/core/models/legal-board';
import { AlertService } from 'app/core/services/alert.service';
import { LegalBoardService } from 'app/core/services/legal-board.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
import { LegalBoardType } from 'app/core/enums/LegalBoardType';
import { MemberDetailsComponent } from '../member-details/member-details.component';
import { MatSort, Sort } from '@angular/material/sort';
import { HijriConverterService } from 'app/core/services/hijri-converter.service';

@Component({
  selector: 'app-legal-board-form',
  templateUrl: './legal-board-form.component.html',
  styleUrls: ['./legal-board-form.component.css'],
})
@Injectable()
export class LegalBoardFormComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'position',
    'name',
    'adjective',
    'startDate',
    'status',
    'membershipType',
    'actions',
  ];

  legalBoardId: number;

  form: FormGroup = Object.create(null);
  private subs = new Subscription();
  boardTypes: any[] = [];
  boardMemberTypes: any[] = [];

  dataSource = new MatTableDataSource<LegalBoardMembersListItem>();
  @ViewChild(MatSort) sort: MatSort;


  public get AppRole(): typeof AppRole {
    return AppRole;
  }
  public get LegalBoardMemberType(): typeof LegalBoardMemberType {
    return LegalBoardMemberType;
  }
  public get LegalBoardType(): typeof LegalBoardType {
    return LegalBoardType;
  }

  isAdmin = this.authService.checkRole(AppRole.Admin);
  consultants: any[] = [];

  constructor(
    private fb: FormBuilder,
    private legalBoardService: LegalBoardService,
    private alert: AlertService,
    private loaderService: LoaderService,
    private activatedRouter: ActivatedRoute,
    public location: Location,
    private router: Router,
    private dialog: MatDialog,
    public authService: AuthService,
    private userService: UserService,
    private hijriConverter: HijriConverterService,
    private cdr: ChangeDetectorRef
  ) {
    this.activatedRouter.paramMap.subscribe((params) => {
      var id = params.get('id');
      if (id != null) {
        this.legalBoardId = +id;
      }
    });
  }

  ngOnInit() {

    this.dataSource.sort = this.sort;

    this.init();
    this.populateLegalBoardTypes();
    this.populateLegalBoardMemberType();
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
    this.subs.add(
      this.sort.sortChange.subscribe(
        (sort: any) => {
          const data = this.dataSource.data.slice();
          const isAsc = sort.direction === 'asc';
          this.dataSource.data = data.sort((a: any, b: any) => {
            switch (sort.active) {
              case 'name': return this.compare(a.name, b.name);
              case 'startDate': return this.compare(a.startDate, b.startDate, isAsc);
              default: return 0;
            }
          });
        }
      )
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  private init(): void {
    this.form = this.fb.group({
      id: [0],
      name: [null, Validators.compose([Validators.required])],
      typeId: ['', Validators.compose([Validators.required])],
      legalBoardMembers: [null],
      createdBy: [''],
    });
  }

  populateLegalBoardTypes() {
    this.subs.add(
      this.legalBoardService.getLegalBoardType().subscribe(
        (data: any) => {
          this.boardTypes = data;
          if (this.legalBoardId) {
            this.populateLegalBoardForm();
          }
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          console.error(error);
        }
      )
    );
  }

  populateLegalBoardMemberType() {
    this.subs.add(
      this.legalBoardService.getLegalBoardMemberType().subscribe(
        (data: any) => {
          this.boardMemberTypes = data;
        },
        (error) => {
          this.alert.error('فشلت عملية جلب البيانات !');
          console.error(error);
        }
      )
    );
  }

  populateLegalBoardForm() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.controls['typeId'].disable();
    this.subs.add(
      this.legalBoardService.get(this.legalBoardId).subscribe(
        (data: any) => {
          let members = data.data.legalBoardMembers;
          this.dataSource = new MatTableDataSource(members);
          const legalBoard: LegalBoardListItem = data.data;

          this.form.patchValue({
            id: legalBoard?.id,
            name: legalBoard?.name,
            typeId: this.boardTypes.find((t) => t.nameAr == legalBoard?.type)?.value || 0,
            legalBoardMembers: legalBoard.legalBoardMembers.map((member): LegalBoardMembers => {
              return {
                id: member.id,
                userId: member.userId,
                membershipType: member.membershipType.id,
                startDate: member.startDate,
                isActive: member.isActive
              }
            }),
            createdBy: legalBoard.createdBy,
          });
          this.loaderService.stopLoading();
        },
        (error) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب البيانات !');
          console.error(error);
        }
      )
    );

  }

  populateConsultants() {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(
      this.userService.getConsultants('', null).subscribe(
        (result: any) => {
          this.consultants = result.data;
          this.loaderService.stopLoading();
          this.openAddMemberModal();
        },
        (error) => {
          this.loaderService.stopLoading();
          this.alert.error('فشلت عملية جلب البيانات !');
          console.error(error);

        }
      )
    );
  }
  setMainBoardHead() {
    // check if the selected board type is Major
    if (this.form.get('typeId').value == LegalBoardType.Major) {
      // check if we do not have a head member
      // let headMember = this.dataSource.data.find(
      //   (m) => m.membershipType.id == LegalBoardMemberType.Head && m.isActive
      // );

      // if (headMember == null) {
      // add the head member
      const today = formatDate(new Date(), 'yyyy-MM-dd', 'en');
      let newMember: LegalBoardMembersListItem = {
        id: 0,
        userName: this.authService.currentUser.given_name,
        userId: this.authService.currentUser.id,
        membershipType: { id: LegalBoardMemberType.Head, name: 'أمين لجنة' },
        isActive: true,
        startDate: today,
        startDateHigri: this.hijriConverter.gregorianToHijri(today)

      };

      this.dataSource.data = [newMember];

      this.dataSource._updateChangeSubscription();
      // }
    }
    // if the selected board type is Secondary
    else {
      // // get the index of the current user if he is a head
      // let headMemberIndex = this.dataSource.data.findIndex(
      //   (m) =>
      //     m.membershipType.id == LegalBoardMemberType.Head &&
      //     m.userId == this.authService.currentUser.id &&
      //     m.isActive
      // );
      this.dataSource.data = [];
      this.dataSource._updateChangeSubscription();
    }
  }

  get isHeadMemberExist(): boolean {
    return this.dataSource.data.find((m) => m.membershipType.id == LegalBoardMemberType.Head && m.isActive) != null;
  }

  onSubmit() {
    this.loaderService.startLoading(LoaderComponent);
    this.form.controls['typeId'].enable();
    this.form.controls['legalBoardMembers'].setValue(this.dataSource.data.map((member): LegalBoardMembers => {
      return {
        id: member.id,
        userId: member.userId,
        membershipType: member.membershipType.id,
        startDate: member.startDate,
        isActive: member.isActive
      }
    }));

    let result$ = this.legalBoardId
      ? this.legalBoardService.update(this.form.value)
      : this.legalBoardService.create(this.form.value);

    this.subs.add(
      result$.subscribe(
        (res) => {
          this.loaderService.stopLoading();
          let message = this.legalBoardId
            ? 'تمت عملية التعديل بنجاح'
            : 'تمت عملية الإضافة بنجاح';
          this.alert.succuss(message);
          this.router.navigate(['/legal-boards/list']);
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading();
          let message = this.legalBoardId
            ? 'فشلت عملية التعديل !'
            : 'فشلت عملية الإضافة !';
          this.alert.error(message);
        }
      )
    );
  }

  openMemberDetailsModal(userId: string, legalBoardId: number): void {
    this.dialog.open(MemberDetailsComponent, {
      width: '35em',
      disableClose: false,
      data: { userId: userId, legalBoardId: legalBoardId },
    });
  }

  checkOpenAddMemberModal(): void {
    if (this.consultants.length == 0) {
      this.populateConsultants();
    }
    else {
      this.openAddMemberModal();
    }
  }

  openAddMemberModal() {

    this.subs.add(
      this.dialog.open(MemberFormComponent, {
        width: '30em',
        data: {
          legalBoardId: this.legalBoardId,
          consultants: this.consultants.filter((c) => this.dataSource.data.map(m => m.userId).indexOf(c.consultantId) == -1),
          isHeadMemberExist: this.isHeadMemberExist
        },
      }).afterClosed().subscribe(
        (res) => {
          if (res) {
            let today = formatDate(new Date(), 'yyyy-MM-dd', 'en');
            let newMember: LegalBoardMembersListItem = {
              id: 0,
              userName: res.member.name,
              userId: res.member.consultantId,
              membershipType: { id: res.memberType, name: res.memberType == LegalBoardMemberType.Head ? 'أمين لجنة' : 'عضو لجنة' },
              isActive: true,
              startDate: today,
              startDateHigri: this.hijriConverter.gregorianToHijri(today)
            };
            this.dataSource.data.push(newMember);
            this.dataSource._updateChangeSubscription();
          }
        },
        (error) => {
          console.error(error);
        }
      )
    );
  }

  onChangeMemberType(member: LegalBoardMembersListItem) {
    this.dataSource.data.forEach(
      (m) => (m.membershipType = { id: LegalBoardMemberType.Member, name: "عضو لجنة" })
    );
    this.dataSource.data.find((m) => m.userId == member.userId).membershipType = {
      id: LegalBoardMemberType.Head,
      name: "أمين لجنة"
    }
  }

  onChangeActivation(member: LegalBoardMembersListItem) {
    if (member.membershipType.id == LegalBoardMemberType.Head && member.isActive == false) {
      this.dataSource.data.find((m) => m.userId == member.userId).membershipType = {
        id: LegalBoardMemberType.Member,
        name: "عضو لجنة"
      }
    }
  }

  compare(a: number | string, b: number | string, isAsc=false) {
    return (a < b ? -1 : 1)  * (isAsc ? 1 : -1);
  }

}
