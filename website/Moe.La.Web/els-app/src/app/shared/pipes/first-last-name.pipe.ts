import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'firstLastName'
})
export class FirstLastNamePipe implements PipeTransform {

  transform(fullName: string, ...args: any[]): any {
    let names = fullName.trim().split(' ');
        return names[0] + ' ' + names[names.length-1];
  }
}
