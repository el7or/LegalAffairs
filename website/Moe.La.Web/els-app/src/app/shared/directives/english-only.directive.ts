import { Directive, HostBinding, HostListener, ElementRef } from "@angular/core";

@Directive({
  selector: '[englishOnly]'
})

export class EnglishOnlyDirective {

  constructor(
    private el: ElementRef) { }

    @HostListener('input', ['$event']) onInputChange(event) {

      var ew = event.which;
    if(ew == 32)
        return true;
    if(48 <= ew && ew <= 57)
        return true;
    if(65 <= ew && ew <= 90)
        return true;
    if(97 <= ew && ew <= 122)
        return true;
    return false;

      const initalValue = this.el.nativeElement.value;
      //this.el.nativeElement.value = initalValue.replace(/[^a-zA-Z]/g, '');
      this.el.nativeElement.value = initalValue.replace(/[^\u0600-\u06FF ]/, '');
  }
}
