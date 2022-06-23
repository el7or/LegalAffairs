import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arrayKeyValue'
})
export class ArrayKeyValuePipe implements PipeTransform {

    transform(id: any, array: any): string {
        if (id && array)
            if (array.find((s: { id: any; }) => s.id == id))
                return array.find((s: { id: any; }) => s.id == id).name;
        return '';
    }
}
