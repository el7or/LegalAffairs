import { HijriConverterService } from './../../core/services/hijri-converter.service';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'hijriConverter',
})
export class HijriConverterPipe implements PipeTransform {
  constructor(private hijriConverter: HijriConverterService) {}

  transform(date: string, toType: string) {
    if (!date) return '';
    if (toType == 'toHijri') return this.hijriConverter.gregorianToHijri(date) + " هـ";
    if (toType == 'toGregorian') return this.hijriConverter.hijriToGregorian(date)+ " هـ";
  }
}
