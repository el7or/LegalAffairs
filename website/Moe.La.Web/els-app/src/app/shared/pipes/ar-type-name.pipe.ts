import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arTypeName'
})
export class ArTypeNamePipe implements PipeTransform {

  transform(type: string, service: any): string {
    return service.getArTypeName(type);
  }
}
