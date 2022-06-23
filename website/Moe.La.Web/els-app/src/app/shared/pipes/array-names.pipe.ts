import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arrayNames'
})
export class ArrayNamesPipe implements PipeTransform {

  transform(array:any[]): unknown {
    return array.map((item:any)=>item.name);
  }

}
