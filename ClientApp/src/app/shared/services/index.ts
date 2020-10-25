import { ApplicationService } from './app.service';
import { SnackBarService } from './snack-bar.service';

export const services = [
  ApplicationService,
  SnackBarService
];

export * from './app.service';
export * from './snack-bar.service';
