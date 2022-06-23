import { Directive, HostBinding, HostListener, ElementRef } from "@angular/core";

@Directive({
  selector: '[arabic-only]'
})

export class ArabicOnlyDirective {

  constructor(
    private el: ElementRef) { }

  @HostListener('input', ['$event']) onInputChange(event) {
    const initalValue = this.el.nativeElement.value;
    this.el.nativeElement.value = initalValue.replace(/[^\u0600-\u06FF ]/, '');
  }
}
