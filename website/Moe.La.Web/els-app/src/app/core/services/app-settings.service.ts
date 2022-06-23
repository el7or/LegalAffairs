import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';;
import { AppSettings } from '../models/app-settings';
import { environment } from '../../../environments/environment';

@Injectable({providedIn: "root"})
export class AppSettingsService {

constructor(private http: HttpClient) { }

  getAppSettings() {
    this.http.get(environment.API_URL + "app-settings")
        
      .subscribe((result:any) => {
        localStorage.setItem('app_settings', JSON.stringify (result));
      });
  }

  get appSettings(): AppSettings {
    return JSON.parse(localStorage.getItem('app_settings')||'');
  }

  get isLawFirmOffice(): boolean {
    return this.appSettings.isLawFirmOffice == "true";
  }
}
