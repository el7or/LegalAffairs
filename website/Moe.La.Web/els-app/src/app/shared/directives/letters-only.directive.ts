import { Directive, HostBinding, HostListener, ElementRef } from "@angular/core";

@Directive({
  selector: '[lettersOnly]'
})

export class LettersOnlyDirective {

  constructor(
    private el: ElementRef) { }

  @HostListener('input', ['$event']) onInputChange(event) {
    // const initalValue = this.el.nativeElement.value;
    // //this.el.nativeElement.value = initalValue.replace(/[^a-zA-Z]/g, '');
    // this.el.nativeElement.value = initalValue.replace(/[^\u0600-\u06FF ]/, '');
  }
}
