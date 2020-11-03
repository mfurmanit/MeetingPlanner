import { Injectable } from '@angular/core';
import { MissingTranslationHandler, MissingTranslationHandlerParams } from '@ngx-translate/core';

@Injectable()
export class AppMissingTranslationHandler extends MissingTranslationHandler {
  handle(params: MissingTranslationHandlerParams): string {
    console.error(`Missing translation key: ${params.key}`);
    return params.key;
  }
}
