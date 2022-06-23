import { Component, OnDestroy, OnInit } from '@angular/core';
import { NotificationSystemQueryObject } from 'app/core/models/query-objects';
import { AuthService } from 'app/core/services/auth.service';
import { LoaderService } from 'app/core/services/loader.service';
import { LoaderComponent } from 'app/shared/components/loader/loader.component';
//import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../../core/services/notification.service';
import { NotificationListItem } from '../../../core/models/notification';
import { AlertService } from 'app/core/services/alert.service';
import { Router } from '@angular/router';



@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: []
})
export class AppHeaderComponent implements OnInit, OnDestroy {
  // public config: PerfectScrollbarConfigInterface = {};

  notifications: NotificationListItem[] = [];
  PAGE_SIZE: number = 20;

  queryObject: NotificationSystemQueryObject = {
    isForCurrentUser: true,
    sortBy: 'createdOn',
    isSortAscending: true,
    page: 1,
    pageSize: this.PAGE_SIZE,
  };

  private subs = new Subscription();

  constructor(private notificationService: NotificationService,
    public authService: AuthService,
    public alert: AlertService,
    private loaderService: LoaderService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.populateNotifications();
  }

  populateNotifications() {
    this.loaderService.startLoading(LoaderComponent);
    this.queryObject.isRead = false;

    this.subs.add(
      this.notificationService.getWithQuery(this.queryObject).subscribe(
        (result: any) => {
          this.loaderService.stopLoading()
          this.notifications = result.data.items;
          this.notifications.forEach(m => { m.currentDay = new Date(m.createdOn).getDay() == new Date().getDay() });
        },
        (error) => {
          console.error(error);
          this.loaderService.stopLoading()
        }
      )
    );
  }

  logout(): void {
    this.authService.logout();
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  readNotification(notificationId: number, url: string) {
    this.loaderService.startLoading(LoaderComponent);
    this.subs.add(this.notificationService.readNotification(notificationId)
      .subscribe((res: any) => {
        this.alert.succuss("تمت قراءة الاشعار  بنجاح");
        this.router.navigateByUrl(url);
        this.loaderService.stopLoading();
      }, (error) => {
        this.alert.error("فشلت عملية قراءة الاشعار ");
        this.loaderService.stopLoading();
        console.error(error);
      }))
  }

}

  // // This is for Notifications
  // notifications: any[] = [
  //   {
  //     round: 'round-danger',
  //     icon: 'ti-blot',
  //     title: 'اشعار 1',
  //     subject: 'طلب قضية جديد',
  //     time: '9:30 AM'
  //   },
  //   {
  //     round: 'round-success',
  //     icon: 'ti-bang',
  //     title: 'اشعار 2',
  //     subject: 'رسالة من المستشار',
  //     time: '9:10 AM'
  //   },
  //   {
  //     round: 'round-warning',
  //     icon: 'ti-bang',
  //     title: 'اشعار 2',
  //     subject: '11رسالة من المستشار',
  //     time: '9:15 AM'
  //   },
  //   {
  //     round: 'round-info',
  //     icon: 'ti-bill',
  //     title: 'اشعار 3',
  //     subject: 'تم تغيير الإعدادات',
  //     time: '9:08 AM'
  //   }
  // ];

//   // This is for Mymessages
//   mymessages: any[] = [
//     {
//       useravatar: 'assets/images/users/avatar.jpg',
//       status: 'online',
//       from: 'Pavan kumar',
//       subject: 'Just see the my admin!',
//       time: '9:30 AM'
//     },
//     {
//       useravatar: 'assets/images/users/2.jpg',
//       status: 'busy',
//       from: 'Sonu Nigam',
//       subject: 'I have sung a song! See you at',
//       time: '9:10 AM'
//     },
//     {
//       useravatar: 'assets/images/users/2.jpg',
//       status: 'away',
//       from: 'Arijit Sinh',
//       subject: 'I am a singer!',
//       time: '9:08 AM'
//     },
//     {
//       useravatar: 'assets/images/users/4.jpg',
//       status: 'offline',
//       from: 'Pavan kumar',
//       subject: 'Just see the my admin!',
//       time: '9:00 AM'
//     }
//   ];

