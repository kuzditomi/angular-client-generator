
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

@Injectable()
export class ExampleApiService {
    apiUrl: string = API_BASE_URL;

    constructor(private http: HttpClient) {
    }
    public ExampleMethodConfig(id: number): HttpRequest {
        return {
            url: replaceUrl(API_BASE_URL + 'api/example/{id}', {
                id: id,
            }),
            method: 'GET',
        };
    }

    public ExampleMethod(id: number): Observable<ExampleWebAPI.Models.IExampleModel> {
        const config = this.ExampleMethodConfig(id);

        return this.httpClient.sendRequest(config);
    }
}

@NgModule({
    declarations: [
        ExampleApiService,
    ],
    imports: [
        HttpClientModule,
    ],
})
export class ExampleGeneratedModule {
}

export namespace GeneratedAngularClient {
    export namespace ExampleWebAPI.Models {
        export interface IExampleModel {
            Message: string;
            Id: number;
            Color: ExampleWebAPI.Models.Color;
        }

        export enum Color {
            Red = 0,
            Green = 1,
            Blue = 2,
        }
    }
}
