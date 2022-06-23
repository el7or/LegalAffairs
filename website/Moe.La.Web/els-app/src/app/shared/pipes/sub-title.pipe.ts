import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'subTitle'
})
export class SubTitlePipe implements PipeTransform {

    transform(title: string, length: number): string {
        var text = '';
        var str = title.toString();
        if (str === '')
            text = '---------';
        else {
            if (str.length > length) {
                str = str.substring(0, length);

                if (str.lastIndexOf(' ') != -1)
                    str = str.substring(0, str.lastIndexOf(' ')) + '...';
            }
            text = str;
        }
        return text;
    }

}
