(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var module = angular.module("example", ["example-generated"]);
var ExampleController = /** @class */ (function () {
    function ExampleController(scope, apiExampleService) {
        apiExampleService.ExampleMethod(1).then(function (model) {
            scope.model = model;
        });
    }
    ExampleController.$inject = ["$scope", "ApiExampleService"];
    return ExampleController;
}());
module.controller("ExampleController", ExampleController);
},{}]},{},[1])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJhcHAvZXhhbXBsZS1tb2R1bGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7OztBQ0VBLElBQUksTUFBTSxHQUFHLE9BQU8sQ0FBQyxNQUFNLENBQUMsU0FBUyxFQUFFLENBQUMsbUJBQW1CLENBQUMsQ0FBQyxDQUFDO0FBTTlEO0lBR0ksMkJBQVksS0FBOEIsRUFBRSxpQkFBb0Q7UUFDNUYsaUJBQWlCLENBQUMsYUFBYSxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxVQUFBLEtBQUs7WUFDekMsS0FBSyxDQUFDLEtBQUssR0FBRyxLQUFLLENBQUM7UUFDeEIsQ0FBQyxDQUFDLENBQUM7SUFDUCxDQUFDO0lBTk0seUJBQU8sR0FBRyxDQUFDLFFBQVEsRUFBRSxtQkFBbUIsQ0FBQyxDQUFDO0lBT3JELHdCQUFDO0NBUkQsQUFRQyxJQUFBO0FBRUQsTUFBTSxDQUFDLFVBQVUsQ0FBQyxtQkFBbUIsRUFBRSxpQkFBaUIsQ0FBQyxDQUFDIiwiZmlsZSI6ImdlbmVyYXRlZC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzQ29udGVudCI6WyIoZnVuY3Rpb24oKXtmdW5jdGlvbiByKGUsbix0KXtmdW5jdGlvbiBvKGksZil7aWYoIW5baV0pe2lmKCFlW2ldKXt2YXIgYz1cImZ1bmN0aW9uXCI9PXR5cGVvZiByZXF1aXJlJiZyZXF1aXJlO2lmKCFmJiZjKXJldHVybiBjKGksITApO2lmKHUpcmV0dXJuIHUoaSwhMCk7dmFyIGE9bmV3IEVycm9yKFwiQ2Fubm90IGZpbmQgbW9kdWxlICdcIitpK1wiJ1wiKTt0aHJvdyBhLmNvZGU9XCJNT0RVTEVfTk9UX0ZPVU5EXCIsYX12YXIgcD1uW2ldPXtleHBvcnRzOnt9fTtlW2ldWzBdLmNhbGwocC5leHBvcnRzLGZ1bmN0aW9uKHIpe3ZhciBuPWVbaV1bMV1bcl07cmV0dXJuIG8obnx8cil9LHAscC5leHBvcnRzLHIsZSxuLHQpfXJldHVybiBuW2ldLmV4cG9ydHN9Zm9yKHZhciB1PVwiZnVuY3Rpb25cIj09dHlwZW9mIHJlcXVpcmUmJnJlcXVpcmUsaT0wO2k8dC5sZW5ndGg7aSsrKW8odFtpXSk7cmV0dXJuIG99cmV0dXJuIHJ9KSgpIiwi77u/aW1wb3J0IHsgR2VuZXJhdGVkQ2xpZW50IH0gZnJvbSBcIi4vZ2VuZXJhdGVkXCJcclxuXHJcbmxldCBtb2R1bGUgPSBhbmd1bGFyLm1vZHVsZShcImV4YW1wbGVcIiwgW1wiZXhhbXBsZS1nZW5lcmF0ZWRcIl0pO1xyXG5cclxuaW50ZXJmYWNlIElFeGFtcGxlQ29udHJvbGxlclNjb3BlIGV4dGVuZHMgbmcuSVNjb3BlIHtcclxuICAgIG1vZGVsOiBHZW5lcmF0ZWRDbGllbnQuRXhhbXBsZVdlYkFQSS5Nb2RlbHMuSUV4YW1wbGVNb2RlbDtcclxufVxyXG5cclxuY2xhc3MgRXhhbXBsZUNvbnRyb2xsZXIge1xyXG4gICAgc3RhdGljICRpbmplY3QgPSBbXCIkc2NvcGVcIiwgXCJBcGlFeGFtcGxlU2VydmljZVwiXTtcclxuXHJcbiAgICBjb25zdHJ1Y3RvcihzY29wZTogSUV4YW1wbGVDb250cm9sbGVyU2NvcGUsIGFwaUV4YW1wbGVTZXJ2aWNlOiBHZW5lcmF0ZWRDbGllbnQuQXBpRXhhbXBsZVNlcnZpY2UpIHtcclxuICAgICAgICBhcGlFeGFtcGxlU2VydmljZS5FeGFtcGxlTWV0aG9kKDEpLnRoZW4obW9kZWwgPT4ge1xyXG4gICAgICAgICAgICBzY29wZS5tb2RlbCA9IG1vZGVsO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG59XHJcblxyXG5tb2R1bGUuY29udHJvbGxlcihcIkV4YW1wbGVDb250cm9sbGVyXCIsIEV4YW1wbGVDb250cm9sbGVyKTsiXX0=
