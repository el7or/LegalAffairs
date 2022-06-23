import { Directive, HostListener, ElementRef } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[numbersOnlyValidator]',
    providers: [{
    provide: NG_VALIDATORS,
    useExisting: NumbersOnlyValidatorDirective,
    multi: true
  }]
})

export class NumbersOnlyValidatorDirective implements Validator {

  validate(control: AbstractControl): { [key: string]: any } | null {

    if (control.value) {
      if (isNaN(control.value.replace(/[,]+/g, "")))
        return { numbersOnlyValidator: true };
    }
    return null;
   }

  //@HostListener('focus') onFocus() {
  //}
  //@HostListener('blur') onBlur() {
  //}

  //constructor(private el: ElementRef) {}
}
