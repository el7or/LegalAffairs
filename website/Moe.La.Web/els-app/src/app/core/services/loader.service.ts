import { Overlay } from '@angular/cdk/overlay';
import { Injectable } from '@angular/core';
import { ComponentPortal } from '@angular/cdk/portal';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  overlayRef = this.overlay.create({
    positionStrategy: this.overlay
      .position()
      .global()
      .centerHorizontally()
      .centerVertically(),
    hasBackdrop: true,
  });

  constructor(private overlay: Overlay) {}

  startLoading(loaderComponent: any) {
    if (this.overlayRef && !this.overlayRef.hasAttached()) {
      this.overlayRef.attach(new ComponentPortal(loaderComponent));
    }
  }

  stopLoading() {
    this.overlayRef.detach();
  }
}
