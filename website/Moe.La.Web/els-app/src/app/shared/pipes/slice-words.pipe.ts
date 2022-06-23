import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sliceWords',
  pure: false
})
export class SliceWordsPipe implements PipeTransform {
  transform(value: string, wordsCount: number, item?: any): string {
    if (value.split(/\s+/).length > wordsCount && (item.isWithViewMore != true || item.showViewMore != false)) {
      item.isWithViewMore = true;
      item.showViewMore = true;
      return value.split(/\s+/).slice(0, wordsCount).join(" ").concat(" ...");
    }
    else { return value }
  }
}
