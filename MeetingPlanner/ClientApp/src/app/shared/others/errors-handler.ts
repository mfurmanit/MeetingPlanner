import { ErrorHandler, Injectable, NgZone } from '@angular/core';
import { SnackBarService } from '../services';
import { TranslateService } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';
import { isNullOrUndefined } from '../helpers/application.helper';

@Injectable()
export class ErrorsHandler implements ErrorHandler {
  constructor(private snackBar: SnackBarService,
              private translate: TranslateService,
              private zone: NgZone) {
    this.snackBar.translate(translate);
  }

  handleError(error: Error) {
    if (error instanceof HttpErrorResponse && !isNullOrUndefined(error?.error?.Message)) {
      this.zone.run(() => this.snackBar.openSnackBar(error.error.Message, true, false));
    } else if (error instanceof Error) {
      this.zone.run(() => this.snackBar.openSnackBar(error.message, true, false));
    }
  }
}
