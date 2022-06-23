import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'getPageData'
})
export class GetPageDataPipe implements PipeTransform {

  transform(array: any, pageSize:number, currentPage:number): any[] {
    return array.slice(currentPage * pageSize, (currentPage + 1) * pageSize);
  }

}
