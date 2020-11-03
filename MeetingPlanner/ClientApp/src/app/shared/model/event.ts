import { Notification } from './notification';

export class Event {
  id?: string;
  date?: string | Date;
  hourFrom?: string;
  hourTo?: string;
  global?: boolean;
  title?: string;
  description?: string;
  notifications?: Notification[];

  constructor(obj) {
    // tslint:disable-next-line:forin
    for (const prop in obj) {
      this[prop] = obj[prop];
    }
  }
}
