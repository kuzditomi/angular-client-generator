import { Injectable, NgModule } from '@angular/core';
import { HttpClientModule, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

type RequestOptions = Parameters<HttpClient["request"]>["2"] & { method: string, url: string };

let addr = window['ApiHost'];
if (addr.indexOf('ApiHost') !== -1) {
    addr = 'http://localhost:1337/';
}

export const BASE_URL = addr;
export const API_SUFFIX = '';
export const API_BASE_URL = BASE_URL + API_SUFFIX;

function replaceUrl(url: string, params: any) {
    let replaced = url;
    for (let key in params) {
        if (params.hasOwnProperty(key)) {
            const param = params[key];
            replaced = replaced.replace('{' + key + '}', param);
        }
    }
    return replaced;
}

export namespace GeneratedAngularClient {
    @Injectable({
        providedIn: 'root'
    })
    export class ExampleApiService {
        apiUrl: string = API_BASE_URL;

        constructor(private httpClient: HttpClient) {
        }

        public ExampleMethodConfig(id: number): RequestOptions {
            return {
                url: replaceUrl(API_BASE_URL + 'api/example/{id}', {
                    id: id,
                }),
                method: 'GET',
            } as RequestOptions;
        }

        public ExampleMethod(id: number): Observable<ExampleWeb.Models.IExampleModel> {
            const config = this.ExampleMethodConfig(id);

            return this.httpClient.request(config.method, config.url, config);
        }
    }

    @NgModule({
        providers: [
            ExampleApiService,
        ],
        imports: [
            HttpClientModule,
        ],
    })
    export class ExampleGeneratedModule {
    }

    export namespace ExampleWeb.Models {
        export interface IExampleModel {
            Message: string;
            Id: number;
            Color: ExampleWeb.Models.Color;
        }

        export enum Color {
            Red = 0,
            Green = 1,
            Blue = 2,
        }
    }
}
