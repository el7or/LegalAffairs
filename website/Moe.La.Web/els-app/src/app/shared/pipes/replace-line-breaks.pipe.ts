import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'replaceLineBreaks',
  pure: false
})

export class ReplaceLineBreaksPipe implements PipeTransform {

  transform(value: string): string {
    if (value)
      return value.replace(/<br>/g, '\n');
    else 
      return "";
  }
}
