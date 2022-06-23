import { HttpClient, HttpHeaders } from '@angular/common/http';;
import { Injectable } from '@angular/core';
import { forkJoin } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/observable/throw';
import { BadInput } from '../../shared/errors/bad-input';
import { NotFoundError } from '../../shared/errors/not-found-error';
import { AppError } from '../../shared/errors/app-error';
import { LoadingIndicatorService } from './loading-indicator.service';
import { Observable } from 'rxjs/Rx';
import { QueryObject } from '../models/query-objects';
import { Guid } from 'guid-typescript';

export class DataService {
  /* protected httpOptions = {
    headers: new HttpHeaders({
      Accept: 'application/json'
    })
  }; */

  constructor(protected url: string, protected http: HttpClient) { }

  getAll(url?: string | null) {

    if (url == null)
      url = this.url;

    return this.http.get(url);
  }

  getWithQuery(queryObject: any) {
    return this.http.get(this.url + '?' + this.toQueryString(queryObject));
  }

  toQueryString(obj: { [x: string]: any; }) {
    var parts = [];

    for (var property in obj) {
      var value = obj[property]

      if (value != null && value != undefined)
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value))
    }

    return parts.join('&');
  }


  get(id: number | string) {
    return this.http.get(this.url + '/' + id);
  }

  create(resource: any) {
    return this.http.post(this.url, resource) //JSON.stringify(resource) ;
  }

  update(resource: any) {
    // const headers = new HttpHeaders;
    // headers.append('Access-Control-Allow-Origin', '*');
    // return this.http.put(this.url, resource,{ headers: headers }) //JSON.stringify({ isRead: true }) ;
    return this.http.put(this.url, resource) //JSON.stringify({ isRead: true }) ;
  }

  delete(id: any) {
    return this.http.delete(this.url + '/' + id);
  }

  protected handleError(error: Response) {

    //new LoadingIndicatorService().onRequestFinished();

    if (error.status === 400)
      return Observable.throw(new BadInput(error.json()));

    if (error.status === 404)
      return Observable.throw(new NotFoundError());

    return Observable.throw(new AppError(error));
  }
}
