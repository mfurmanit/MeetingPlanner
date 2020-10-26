import { ApplicationService } from './app.service';
import { SnackBarService } from './snack-bar.service';
import { EventService } from './event.service';

export const services = [
  ApplicationService,
  SnackBarService,
  EventService
];

export * from './app.service';
export * from './snack-bar.service';
