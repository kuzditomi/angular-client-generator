import { GeneratedClient } from "./generated"

let module = angular.module("example", ["example-generated"]);

interface IExampleControllerScope extends ng.IScope {
    model: GeneratedClient.ExampleWebAPI.Models.IExampleModel;
}

class ExampleController {
    static $inject = ["$scope", "ApiExampleService"];

    constructor(scope: IExampleControllerScope, apiExampleService: GeneratedClient.ApiExampleService) {
        apiExampleService.ExampleMethod(1).then(model => {
            scope.model = model;
        });
    }
}

module.controller("ExampleController", ExampleController);