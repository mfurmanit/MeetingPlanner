import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { SharedModule } from './shared/shared.module';
import { PlannerModule } from './planner/planner.module';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { MissingTranslationHandler, TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { AppMissingTranslationHandler } from './shared/helpers/app-missing-translation-handler';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { ApplicationService } from './shared/services';
import { ErrorsHandler } from './shared/others/errors-handler';
import { CookieService } from 'ngx-cookie-service';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    SharedModule,
    PlannerModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'planner/global', pathMatch: 'full' },
      { path: 'planner', loadChildren: () => PlannerModule }
    ]),
    TranslateModule.forRoot({
      loader: { provide: TranslateLoader, useFactory: HttpLoaderFactory, deps: [HttpClient] },
      missingTranslationHandler: { provide: MissingTranslationHandler, useClass: AppMissingTranslationHandler }
    }),
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    BrowserAnimationsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: 'pl-PL' },
    { provide: ErrorHandler, useClass: ErrorsHandler },
    ApplicationService,
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
