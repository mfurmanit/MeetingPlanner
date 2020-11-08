import * as _ from 'lodash-es';
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';

export function isEmpty(value: any) {
  return isNullOrUndefined(value) || _.isEmpty(value);
}

export function isNullOrUndefined(value: any) {
  return _.isNil(value);
}

export const currentLang = (): string => {
  const language = localStorage.getItem('language');
  return !isNullOrUndefined(language) ? language : 'pl';
};

export function initTranslations(translate: TranslateService, cookieService: CookieService) {
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

export function parseDateParam(date: Date) {
  return moment(date).format('YYYY-MM-DD');
}
