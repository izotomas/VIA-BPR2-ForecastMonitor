import { Component } from '@angular/core';
import { ToasterConfig } from 'angular2-toaster';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  toasterConfig: ToasterConfig;

  constructor() {
    this.toasterConfig = new ToasterConfig({
      mouseoverTimerStop: true,
      positionClass: 'toast-bottom-right',
      animation: 'flyRight'
    });
  }
}
