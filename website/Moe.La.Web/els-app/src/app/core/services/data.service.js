"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("rxjs/add/operator/catch");
require("rxjs/add/operator/map");
require("rxjs/add/operator/toPromise");
require("rxjs/add/observable/throw");
var bad_input_1 = require("../shared/errors/bad-input");
var not_found_error_1 = require("../shared/errors/not-found-error");
var app_error_1 = require("../shared/errors/app-error");
var Rx_1 = require("rxjs/Rx");
var DataService = /** @class */ (function () {
    function DataService(url, http) {
        this.url = url;
        this.http = http;
    }
    DataService.prototype.getAll = function (url) {
        if (url == null)
            url = this.url;
        return this.http.get(url)
            .map(function (response) { return response; })
            .catch(this.handleError);
    };
    DataService.prototype.getWithQuery = function (queryObject) {
        return this.http.get(this.url + '?' + this.toQueryString(queryObject))
            .map(function (response) { return response; })
            .catch(this.handleError);
    };
    DataService.prototype.toQueryString = function (obj) {
        var parts = [];
        for (var property in obj) {
            var value = obj[property];
            if (value != null && value != undefined)
                parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
        }
        return parts.join('&');
    };
    DataService.prototype.get = function (id) {
        return this.http.get(this.url + '/' + id)
            .map(function (response) { return response; })
            .catch(this.handleError);
    };
    DataService.prototype.create = function (resource) {
        return this.http.post(this.url, resource) //JSON.stringify(resource)
            .map(function (response) { return response; })
            .catch(this.handleError);
    };
    DataService.prototype.update = function (resource) {
        return this.http.put(this.url + '/' + resource.id, resource) //JSON.stringify({ isRead: true })
            .map(function (response) { return response; })
            .catch(this.handleError);
    };
    DataService.prototype.delete = function (id) {
        return this.http.delete(this.url + '/' + id)
            //  
            //  
            .catch(this.handleError);
    };
    DataService.prototype.handleError = function (error) {
        //new LoadingIndicatorService().onRequestFinished();
        if (error.status === 400)
            return Rx_1.Observable.throw(new bad_input_1.BadInput(error.json()));
        if (error.status === 404)
            return Rx_1.Observable.throw(new not_found_error_1.NotFoundError());
        return Rx_1.Observable.throw(new app_error_1.AppError(error));
    };
    return DataService;
}());
exports.DataService = DataService;
//# sourceMappingURL=data.service.js.map