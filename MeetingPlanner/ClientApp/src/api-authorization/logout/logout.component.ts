import { Component, OnInit } from '@angular/core';
import { AuthenticationResultStatus, AuthorizeService } from '../authorize.service';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { ApplicationPaths, LogoutActions, ReturnUrlType } from '../api-authorization.constants';
import { SnackBarService } from '../../app/shared/services';
import { TranslateService } from '@ngx-translate/core';
import { handleGlobalFailure } from '../../app/shared/helpers/application.helper';

// The main responsibility of this component is to handle the user's logout process.
// This is the starting point for the logout process, which is usually initiated when a
// user clicks on the logout button on the LoginMenu component.
@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {

  constructor(private authorizeService: AuthorizeService,
              private activatedRoute: ActivatedRoute,
              private snackBarService: SnackBarService,
              private translateService: TranslateService,
              private router: Router) {
    this.snackBarService.translate(translateService);
  }

  async ngOnInit() {
    const action = this.activatedRoute.snapshot.url[1];
    switch (action.path) {
      case LogoutActions.Logout:
        if (!!window.history.state.local) {
          await this.logout(this.getReturnUrl());
        } else {
          // This prevents regular links to <app>/authentication/logout from triggering a logout
          handleGlobalFailure('The logout was not initiated from within the page.', this.router);
        }

        break;
      case LogoutActions.LogoutCallback:
        await this.processLogoutCallback();
        break;
      case LogoutActions.LoggedOut:
        this.redirectAfterLogout();
        break;
      default:
        throw new Error(`Invalid action '${action}'`);
    }
  }

  private async logout(returnUrl: string): Promise<void> {
    const state: INavigationState = { returnUrl };
    const isAuthenticated = await this.authorizeService.isAuthenticated().pipe(
      take(1)
    ).toPromise();
    if (isAuthenticated) {
      const result = await this.authorizeService.signOut(state);
      switch (result.status) {
        case AuthenticationResultStatus.Redirect:
          break;
        case AuthenticationResultStatus.Success:
          await this.navigateToReturnUrl(returnUrl);
          break;
        case AuthenticationResultStatus.Fail:
          handleGlobalFailure(result.message, this.router);
          break;
        default:
          throw new Error('Invalid authentication result status.');
      }
    }
  }

  private async processLogoutCallback(): Promise<void> {
    const url = window.location.href;
    const result = await this.authorizeService.completeSignOut(url);
    switch (result.status) {
      case AuthenticationResultStatus.Redirect:
        // There should not be any redirects as the only time completeAuthentication finishes
        // is when we are doing a redirect sign in flow.
        handleGlobalFailure('Should not redirect.', this.router);
        break;
      case AuthenticationResultStatus.Success:
        await this.navigateToReturnUrl(this.getReturnUrl(result.state));
        break;
      case AuthenticationResultStatus.Fail:
        handleGlobalFailure(result.message, this.router);
        break;
      default:
        throw new Error('Invalid authentication result status.');
    }
  }

  private async navigateToReturnUrl(returnUrl: string) {
    await this.router.navigateByUrl(returnUrl, {
      replaceUrl: true
    });
  }

  private getReturnUrl(state?: INavigationState): string {
    const fromQuery = (this.activatedRoute.snapshot.queryParams as INavigationState).returnUrl;
    // If the url is comming from the query string, check that is either
    // a relative url or an absolute url
    if (fromQuery &&
      !(fromQuery.startsWith(`${window.location.origin}/`) ||
        /\/[^\/].*/.test(fromQuery))) {
      // This is an extra check to prevent open redirects.
      handleGlobalFailure('Invalid return url. The return url needs to have the same origin as the current page.', this.router);
    }
    return (state && state.returnUrl) ||
      fromQuery ||
      ApplicationPaths.LoggedOut;
  }

  private redirectAfterLogout(): void {
    this.router.navigate(['/planner/global']).then(() => {
      this.translateService.get('messages.logoutSuccessful').subscribe(translation => {
        this.snackBarService.openSnackBar(translation, true, false);
      });
    });
  }
}

interface INavigationState {
  [ReturnUrlType]: string;
}
