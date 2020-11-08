import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { currentLang, initTranslations } from './shared/helpers/application.helper';
import { ApplicationService } from './shared/services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  constructor(private translateService: TranslateService,
              private appService: ApplicationService,
              private cookieService: CookieService) {
    initTranslations(translateService, cookieService);
    this.initLang();
  }

  initLang() {
    this.appService.changeLanguage(currentLang());
  }
}
