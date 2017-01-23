export namespace GeneratedClient {
    export let Module = angular.module('example-generated', []);
    
    function replaceUrl(url: string, params: { }) {
        let replaced = url;
        for (let key in params) {
            if (params.hasOwnProperty(key)) {
                const param = params[key];
                replaced = replaced.replace('{' + key + '}', param);
            }
        }
        return replaced;
    }
    
    export class ApiExampleService {
        static $inject = ['$http', '$q'];
        constructor(private http, private q){ }
        
        public ExampleMethodConfig() : ng.IRequestConfig {
            return {
                url: 'api/example/example',
                method: 'GET',
            };
        }
        public ExampleMethod = () : ng.IPromise<ExampleWebAPI.Models.IExampleModel> => {
            return this.http(this.ExampleMethodConfig())
                .then(resp => {
                    return resp.data;
                }, resp => {
                    return this.q.reject({
                        Status: resp.status,
                        Message: (resp.data && resp.data.Message) || resp.statusText
                    });
                });
        }
    }
    
    Module.service('ApiExampleService', ApiExampleService);
    
    export namespace ExampleWebAPI.Models {
        export interface IExampleModel {
            Message: string;
        }
        
    }
    export class EnumHelperService {
        constructor() {
        }
        
    	public Register = (name: string, enumtype: any, titles?: { [key: string]: string }) => {
            this.RegisterArray(name, enumtype, titles);
            this.RegisterHash(name, enumtype, titles);
        }
    
    	private RegisterArray = (enumname: string, enumtype: any, titles?: { [key: string]: string }) => {
            var enumArray = [];
            for (var enumMember in enumtype) {
                var isValueProperty = parseInt(enumMember, 10) >= 0;
                if (isValueProperty) {
                    var name = enumtype[enumMember];
                    var value = parseInt(enumMember);
                    var title = (titles && titles[name]) || name;
        
                    enumArray.push({ Name: name, Value: value, Title: title});
                }
            }
        
            this[enumname] = enumArray;
        }
        
        private RegisterHash(enumname: string, enumtype: any, titles?: { [key: string]: string }) {
            var enumObj = {};
            for (var enumMember in enumtype) {
                var isValueProperty = parseInt(enumMember, 10) >= 0;
                var name = isValueProperty? enumtype[enumMember] : enumMember;
                var value = isValueProperty ? parseInt(enumMember) : parseInt(enumtype[enumMember]);
                var title = titles ? titles[name] : name; 
    
                enumObj[enumMember] = ({ Name: name, Value: value, Title: title });
            }
        
            this[enumname + 'Obj'] = enumObj;
        }
    }
    
    Module.service('Enums', EnumHelperService);
}
