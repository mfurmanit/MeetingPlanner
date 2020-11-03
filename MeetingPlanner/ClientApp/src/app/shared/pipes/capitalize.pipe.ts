import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'capitalize'
})
export class CapitalizePipe implements PipeTransform {
  transform(value: string): string {
    if (!value) {
      return value;
    }

    if (typeof value !== 'string') {
      throw Error(`Invalid argument ( '${value}' ) for capitalize pipe.`);
    }

    return value[0].toUpperCase() + value.substr(1);
  }
}
