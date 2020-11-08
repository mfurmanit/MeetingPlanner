import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { SnackBarService } from './snack-bar.service';
import { CookieService } from 'ngx-cookie-service';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
  currentAppLang = new BehaviorSubject<string>('pl');
  appLangState = this.currentAppLang.asObservable();

  constructor(private snackBarService: SnackBarService,
              private translate: TranslateService,
              private cookieService: CookieService) {
    this.snackBarService.translate(translate);
  }

  changeLanguage(lang: string): void {
    this.currentAppLang.next(lang);
    this.cookieService.set('language', lang, { path: '/' });
    localStorage.setItem('language', lang);
  }
}
