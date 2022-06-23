import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arSatgeName'
})

export class ArSatgeNamePipe implements PipeTransform {

  transform(stage: string, service: any, against: boolean = false): string {
    return service.getArStageName(stage);
  }
}
