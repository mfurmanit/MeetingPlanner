import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ApplicationService } from '../shared/services';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private translateService: TranslateService,
              private appService: ApplicationService) {
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  changeLanguage(language: string): void {
    this.translateService.use(language);
    this.appService.changeLanguage(language);
  }
}
