import * as _ from 'lodash-es';

export function isNullOrUndefined(value: any) {
  return _.isNil(value);
}
