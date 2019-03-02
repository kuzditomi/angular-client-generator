export namespace GeneratedClient {
    export let Module = angular.module('example-generated', []);

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


    export type EnumArr = ;
    export type EnumObj = ;
    export type EnumValue = { Name: string, Value: number, Title: string };
    export type EnumArrayServiceType = { [K in EnumArr]?: Array<EnumValue>; };
    export type EnumObjServiceType = { [K in EnumObj]?: Record<string, EnumValue>; };
    export interface IEnumService extends EnumArrayServiceType, EnumObjServiceType, GeneratedClient.EnumHelperService { }

    export class EnumHelperService {
        [index: string]: any;
        constructor() {
        }

        public Register = (name: string, enumtype: any, titles?: { [key: string]: string }) => {
            this.RegisterArray(name, enumtype, titles);
            this.RegisterHash(name, enumtype, titles);
        }

        private RegisterArray = (enumname: string, enumtype: any, titles?: { [key: string]: string }) => {
            const enumArray = [];
            for (const enumMember in enumtype) {
                const isValueProperty = parseInt(enumMember, 10) >= 0;
                if (isValueProperty) {
                    const name = enumtype[enumMember];
                    const value = parseInt(enumMember);
                    const title = (titles && titles[name]) || name;

                    enumArray.push({ Name: name, Value: value, Title: title });
                }
            }

            this[enumname] = enumArray;
        }

        private RegisterHash(enumname: string, enumtype: any, titles?: { [key: string]: string }) {
            const enumObj: Record<string, EnumValue> = {};
            for (const enumMember in enumtype) {
                const isValueProperty = parseInt(enumMember, 10) >= 0;
                const name = isValueProperty ? enumtype[enumMember] : enumMember;
                const value = isValueProperty ? parseInt(enumMember) : parseInt(enumtype[enumMember]);
                const title = titles ? titles[name] : name;

                enumObj[enumMember] = ({ Name: name, Value: value, Title: title });
            }

            this[enumname + 'Obj'] = enumObj;
        }
    }

    Module.service('Enums', EnumHelperService);
}
