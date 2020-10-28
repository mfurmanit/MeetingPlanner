import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { SnackBarService } from './snack-bar.service';

@Injectable({providedIn: 'root'})
export class ApplicationService {
  currentAppLang = new BehaviorSubject<string>('pl');
  appLangState = this.currentAppLang.asObservable();

  constructor(private snackBarService: SnackBarService,
              private translate: TranslateService) {
    this.snackBarService.translate(translate);
  }

  changeLanguage(lang: string): void {
    this.currentAppLang.next(lang);
    localStorage.setItem('language', lang);
  }
}
