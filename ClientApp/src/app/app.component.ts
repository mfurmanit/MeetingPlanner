import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  constructor(private translateService: TranslateService) {
    this.translateService.setDefaultLang('pl');
    this.translateService.use('pl');
  }
}
