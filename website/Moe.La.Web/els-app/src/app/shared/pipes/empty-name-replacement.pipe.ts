import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'emptyNameReplacement'
})
export class EmptyNameReplacementPipe implements PipeTransform {

    transform(name: string, replacement: string): any {
        if (name.trim())
            return name;
        else if (replacement)
            return replacement;
        return '--------';
    }
}
