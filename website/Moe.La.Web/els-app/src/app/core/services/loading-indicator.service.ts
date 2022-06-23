import { Injectable } from '@angular/core';

@Injectable()
export class LoadingIndicatorService {

  // ExpressionChangedAfterItHasBeenCheckedError
  // Move onRequestStarted() from ngOnInit() to constructor()

  private _loading: boolean = false;
  public loadingId: number | undefined;
  public loadingText:string | undefined;

  get loading(): boolean {
    return this._loading;
  }


  onRequestStarted(): void {
    this._loading = true;
  }

  onRequestFinished(): void {
    this._loading = false;
    this.loadingText="";
  }

}
