import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'daysDiff'
})
export class DaysDiffPipe implements PipeTransform {

  transform(firstDate: any, ...args: any[]): number {

    var m = moment(new Date());
    return m.diff(firstDate, 'days');
  }

}
