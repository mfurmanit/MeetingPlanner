import { Injectable } from '@angular/core';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class SnackBarService {
  translateService: TranslateService;

  constructor(private snackBar: MatSnackBar,
              private liveAnnouncer: LiveAnnouncer) {
  }

  translate(translate) {
    this.translateService = translate;
  }

  openSnackBar(message: string, autoClose: boolean, translate: boolean): void {
    const snackBarRef = this.snackBar.open(
      translate ? this.translateService.instant(message) : message,
      !autoClose ? this.translateService.instant('common.close') : null,
      autoClose ? { duration: 5000 } : null
    );
    this.liveAnnouncer.announce(translate ? this.translateService.instant(message) : message).then(() => null);
    snackBarRef.onAction().subscribe(() => snackBarRef.dismiss());
  }
}
