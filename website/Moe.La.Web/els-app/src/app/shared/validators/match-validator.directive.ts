import { Injectable, Directive, ElementRef, HostListener, Attribute } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl } from '@angular/forms';

@Directive({
  selector: '[matchValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: MatchValidatorDirective,
    multi: true
  }]
})

export class MatchValidatorDirective implements Validator {

  constructor(@Attribute('matchValidator') public matchValidator: string) { }

  validate(control: AbstractControl): { [key: string]: any } | null {

    let controlToCompare = control.root.get(this.matchValidator);

    if (controlToCompare && control)
      if (controlToCompare.value != control.value)
        return { matchValidator: true };

    return null;
  }
}
