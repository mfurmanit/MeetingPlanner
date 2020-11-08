import * as _ from 'lodash-es';
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

export function isEmpty(value: any): boolean {
  return isNullOrUndefined(value) || _.isEmpty(value);
}

export function isNullOrUndefined(value: any): boolean {
  return _.isNil(value);
}

export const currentLang = (): string => {
  const language = localStorage.getItem('language');
  return !isNullOrUndefined(language) ? language : 'pl';
};

export function initTranslations(translate: TranslateService, cookieService: CookieService): void {
  if (!isNullOrUndefined(localStorage.getItem('language'))) {
    const lang = localStorage.getItem('language').toString();
    translate.setDefaultLang(lang);
    translate.use(lang);
    cookieService.set('language', lang, { path: '/' });
  } else {
    const lang = navigator.language && navigator.language.includes('pl') ? 'pl' : 'en';
    localStorage.setItem('language', lang);
    translate.setDefaultLang(lang);
    translate.use(lang);
    cookieService.set('language', lang, { path: '/' });
  }
}

export function parseDateParam(date: Date): string {
  return moment(date).format('YYYY-MM-DD');
}

export function handleGlobalFailure(message: string, router: Router): void {
  router.navigate(['/planner/global']);
  throw new Error(message);
}
