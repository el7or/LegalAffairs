import { Injectable, Directive, Attribute } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl } from '@angular/forms';

@Directive({
  selector: '[mobileValidator2]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: MobileValidator2Directive,
    multi: true
  }]
})

export class MobileValidator2Directive implements Validator {

  //constructor(@Attribute('mobileValidator') public mobileValidator: string) { }

  validate(control: AbstractControl): { [key: string]: any } | null {

    if (!control.value || isNaN(control.value)) // will be checked by another validator
      return null;
    ///////
    let prefix: string = control.value.substr(0, 2);

    if (!prefix.startsWith("5"))
      return { mobileValidator2: true };

    if (prefix === "51" || prefix === "52") 
      return { mobileValidator2: true };

    if (control.value.length!=9)
      return { mobileValidator2: true };

    return null;
  }
}
