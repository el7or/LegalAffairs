import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arrayValues'
})
export class ArrayValuesPipe implements PipeTransform {

  transform(array:any[]): unknown {
    return array.map((item:any)=>item.value);
  }

}
