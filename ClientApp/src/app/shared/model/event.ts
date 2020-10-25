export class Event {
  id?: string;
  date?: string | Date;
  hourFrom?: string;
  hourTo?: string;
  withTime?: boolean;
  recurring?: boolean;
  global?: boolean;
  title?: string;
  description?: string;
  notifications?: any[];

  constructor(obj) {
    for (const prop in obj) {
      if (this[prop]) {
        this[prop] = obj[prop];
      }
    }
  }
}
