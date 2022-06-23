import { Validator, AbstractControl, NG_VALIDATORS } from "@angular/forms";
import { Directive } from "@angular/core";

@Directive({
  selector: '[validNameValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: ValidNameValidator,
    multi: true
  }]
})

export class ValidNameValidator implements Validator {

  validate(control: AbstractControl): { [key: string]:any} | null{

    var format = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;

    if (control)
      if (format.test(control.value))
        return {validNameValidator: true}

    return null;
  }

}
