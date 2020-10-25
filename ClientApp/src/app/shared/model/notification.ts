import { NotificationUnit } from './notification-unit';

export class Notification {
  id?: string;
  quantity?: number;
  unit?: NotificationUnit;

  constructor(obj) {
    for (const prop in obj) {
      if (this[prop]) {
        this[prop] = obj[prop];
      }
    }
  }
}
