export enum NotificationUnit {
  Minutes = 'Minutes',
  Hours = 'Hours',
  Days = 'Days',
  Weeks = 'Weeks'
}

export interface NotificationUnitLabel {
  value: NotificationUnit;
  key: string;
}

export const Quantities = new Map<string, number>([
  [NotificationUnit.Minutes, 59],
  [NotificationUnit.Hours, 23],
  [NotificationUnit.Days, 31],
  [NotificationUnit.Weeks, 16]
]);
