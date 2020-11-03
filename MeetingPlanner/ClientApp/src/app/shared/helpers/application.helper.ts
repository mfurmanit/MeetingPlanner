import * as _ from 'lodash-es';
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';

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

export function initTranslations(translate: TranslateService) {
  if (!isNullOrUndefined(localStorage.getItem('language'))) {
    translate.setDefaultLang(localStorage.getItem('language').toString());
    translate.use(localStorage.getItem('language').toString());
  } else {
    const lang = navigator.language && navigator.language.includes('pl') ? 'pl' : 'en';
    translate.setDefaultLang(lang);
    translate.use(lang);
    localStorage.setItem('language', lang);
  }
}

export function parseDateParam(date: Date) {
  return moment(date).format('YYYY-MM-DD');
}
