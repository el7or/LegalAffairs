import { Component } from '@angular/core';

import { AuthCommunicatorService } from 'app/core/services/auth-communicator.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private authCommunicator: AuthCommunicatorService) {

  }
}
