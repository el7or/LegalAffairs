import { Injectable, Directive, Attribute } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl } from '@angular/forms';

@Directive({
  selector: '[mobileValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: MobileValidatorDirective,
    multi: true
  }]
})

export class MobileValidatorDirective implements Validator {

  //constructor(@Attribute('mobileValidator') public mobileValidator: string) { }

  validate(control: AbstractControl): { [key: string]: any } | null {

    if (!control.value || isNaN(control.value)) // will be checked by another validator
      return null;
    ///////
    let prefix: string = control.value.substr(0, 3);

    if (!prefix.startsWith("05"))
      return { mobileValidator: true };

    if (prefix === "051" || prefix === "052") 
      return { mobileValidator: true };

    if (control.value.length!=10)
      return { mobileValidator: true };

    return null;
  }
}
