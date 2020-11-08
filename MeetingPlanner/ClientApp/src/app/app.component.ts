import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { currentLang, initTranslations } from './shared/helpers/application.helper';
import { ApplicationService } from './shared/services';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnDestroy, AfterViewInit {
  title = 'app';
  viewInitialized: boolean = false;
  showNavbar: boolean = true;
  private readonly subscription: Subscription = new Subscription();

  constructor(private translateService: TranslateService,
              private appService: ApplicationService,
              private router: Router,
              private cdr: ChangeDetectorRef,
              private cookieService: CookieService) {
    initTranslations(translateService, cookieService);
    this.initLang();
    this.checkActiveRoute();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  ngAfterViewInit() {
    this.checkViewInitialization();
  }

  initLang() {
    this.appService.changeLanguage(currentLang());
  }

  private checkViewInitialization(): void {
    if (this.translateService.store.translations[this.translateService.currentLang]) {
      this.viewInitialized = true;
    } else {
      this.translateService.onLangChange.subscribe(() => this.viewInitialized = true);
    }
  }

  private checkActiveRoute(): void {
    this.subscription.add(
      this.router.events.subscribe(() =>
        this.showNavbar = !this.router.url.includes('/authentication')));
  }
}
