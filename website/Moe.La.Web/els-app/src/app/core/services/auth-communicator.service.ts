import { Injectable } from '@angular/core';

@Injectable()
export class AuthCommunicatorService {
  constructor() {}

  init() {
    return new Promise<void>((resolve, reject) => {
      if (!window.sessionStorage.length) {
        if (
          window.navigator.userAgent.indexOf('Trident/') > -1 ||
          window.navigator.userAgent.indexOf('Edge') > -1
        ) {
          resolve();
        } else {
          setTimeout(() => {
            // If first tab and has no session data
            if (!window.sessionStorage.getItem('auth_token')) {
              resolve();
            }
          }, 1000);
        }
        // Check for storage events then intialize angular
        window.localStorage.setItem(
          'getSessionStorage',
          JSON.stringify(Date.now())
        );
      } else {
        // Initialize angular if session storage is already available
        resolve();
      }
      window.addEventListener('storage', (event) => {
        // tslint:disable-next-line:triple-equals
        if (event.key == 'getSessionStorage') {
          window.localStorage.setItem(
            'sessionStorage',
            JSON.stringify(window.sessionStorage)
          );
          window.localStorage.removeItem('sessionStorage');
        } else if (
          // tslint:disable-next-line:triple-equals
          event.key == 'sessionStorage' &&
          !window.sessionStorage.length
        ) {
          const data = JSON.parse(event.newValue || '{}');
          for (const key in data) {
            if (data[key]) {
              window.sessionStorage.setItem(key, data[key]);
            }
          }
          // Initilize Angular After the storage is retrieved from neighbour tabs
          resolve();
        } else if (
          // tslint:disable-next-line:triple-equals
          event.key == 'auth_token' &&
          window.sessionStorage.getItem('auth_token')
        ) {
          // tslint:disable-next-line:triple-equals
        } else if (event.key == 'clear') {
          window.sessionStorage.clear();
        }
      });

      window.onbeforeunload = function() {
        // sessionStorage.clear();
      };
    });
  }
}
