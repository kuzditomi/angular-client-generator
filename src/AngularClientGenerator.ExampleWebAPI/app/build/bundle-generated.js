(function e(t,n,r){function s(o,u){if(!n[o]){if(!t[o]){var a=typeof require=="function"&&require;if(!u&&a)return a(o,!0);if(i)return i(o,!0);var f=new Error("Cannot find module '"+o+"'");throw f.code="MODULE_NOT_FOUND",f}var l=n[o]={exports:{}};t[o][0].call(l.exports,function(e){var n=t[o][1][e];return s(n?n:e)},l,l.exports,e,t,n,r)}return n[o].exports}var i=typeof require=="function"&&require;for(var o=0;o<r.length;o++)s(r[o]);return s})({1:[function(require,module,exports){
"use strict";
var GeneratedClient;
(function (GeneratedClient) {
    GeneratedClient.Module = angular.module('example-generated', []);
    function replaceUrl(url, params) {
        var replaced = url;
        for (var key in params) {
            if (params.hasOwnProperty(key)) {
                var param = params[key];
                replaced = replaced.replace('{' + key + '}', param);
            }
        }
        return replaced;
    }
    var ApiExampleService = (function () {
        function ApiExampleService(http, q) {
            var _this = this;
            this.http = http;
            this.q = q;
            this.ExampleMethod = function () {
                return _this.http(_this.ExampleMethodConfig())
                    .then(function (resp) {
                    return resp.data;
                }, function (resp) {
                    return _this.q.reject({
                        Status: resp.status,
                        Message: (resp.data && resp.data.Message) || resp.statusText
                    });
                });
            };
        }
        ApiExampleService.prototype.ExampleMethodConfig = function () {
            return {
                url: 'api/example/example',
                method: 'GET',
            };
        };
        return ApiExampleService;
    }());
    ApiExampleService.$inject = ['$http', '$q'];
    GeneratedClient.ApiExampleService = ApiExampleService;
    GeneratedClient.Module.service('ApiExampleService', ApiExampleService);
    var EnumHelperService = (function () {
        function EnumHelperService() {
            var _this = this;
            this.Register = function (name, enumtype, titles) {
                _this.RegisterArray(name, enumtype, titles);
                _this.RegisterHash(name, enumtype, titles);
            };
            this.RegisterArray = function (enumname, enumtype, titles) {
                var enumArray = [];
                for (var enumMember in enumtype) {
                    var isValueProperty = parseInt(enumMember, 10) >= 0;
                    if (isValueProperty) {
                        var name = enumtype[enumMember];
                        var value = parseInt(enumMember);
                        var title = (titles && titles[name]) || name;
                        enumArray.push({ Name: name, Value: value, Title: title });
                    }
                }
                _this[enumname] = enumArray;
            };
        }
        EnumHelperService.prototype.RegisterHash = function (enumname, enumtype, titles) {
            var enumObj = {};
            for (var enumMember in enumtype) {
                var isValueProperty = parseInt(enumMember, 10) >= 0;
                var name = isValueProperty ? enumtype[enumMember] : enumMember;
                var value = isValueProperty ? parseInt(enumMember) : parseInt(enumtype[enumMember]);
                var title = titles ? titles[name] : name;
                enumObj[enumMember] = ({ Name: name, Value: value, Title: title });
            }
            this[enumname + 'Obj'] = enumObj;
        };
        return EnumHelperService;
    }());
    GeneratedClient.EnumHelperService = EnumHelperService;
    GeneratedClient.Module.service('Enums', EnumHelperService);
})(GeneratedClient = exports.GeneratedClient || (exports.GeneratedClient = {}));
},{}]},{},[1])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyaWZ5L25vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJhcHAvZ2VuZXJhdGVkLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBOztBQ0FBLElBQWlCLGVBQWUsQ0FzRi9CO0FBdEZELFdBQWlCLGVBQWU7SUFDakIsc0JBQU0sR0FBRyxPQUFPLENBQUMsTUFBTSxDQUFDLG1CQUFtQixFQUFFLEVBQUUsQ0FBQyxDQUFDO0lBRTVELG9CQUFvQixHQUFXLEVBQUUsTUFBVztRQUN4QyxJQUFJLFFBQVEsR0FBRyxHQUFHLENBQUM7UUFDbkIsR0FBRyxDQUFDLENBQUMsSUFBSSxHQUFHLElBQUksTUFBTSxDQUFDLENBQUMsQ0FBQztZQUNyQixFQUFFLENBQUMsQ0FBQyxNQUFNLENBQUMsY0FBYyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQztnQkFDN0IsSUFBTSxLQUFLLEdBQUcsTUFBTSxDQUFDLEdBQUcsQ0FBQyxDQUFDO2dCQUMxQixRQUFRLEdBQUcsUUFBUSxDQUFDLE9BQU8sQ0FBQyxHQUFHLEdBQUcsR0FBRyxHQUFHLEdBQUcsRUFBRSxLQUFLLENBQUMsQ0FBQztZQUN4RCxDQUFDO1FBQ0wsQ0FBQztRQUNELE1BQU0sQ0FBQyxRQUFRLENBQUM7SUFDcEIsQ0FBQztJQUVEO1FBRUksMkJBQW9CLElBQUksRUFBVSxDQUFDO1lBQW5DLGlCQUF1QztZQUFuQixTQUFJLEdBQUosSUFBSSxDQUFBO1lBQVUsTUFBQyxHQUFELENBQUMsQ0FBQTtZQVE1QixrQkFBYSxHQUFHO2dCQUNuQixNQUFNLENBQUMsS0FBSSxDQUFDLElBQUksQ0FBQyxLQUFJLENBQUMsbUJBQW1CLEVBQUUsQ0FBQztxQkFDdkMsSUFBSSxDQUFDLFVBQUEsSUFBSTtvQkFDTixNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQztnQkFDckIsQ0FBQyxFQUFFLFVBQUEsSUFBSTtvQkFDSCxNQUFNLENBQUMsS0FBSSxDQUFD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SyxFQUFFLEtBQUssRUFBRSxLQUFLLEVBQUUsQ0FBQyxDQUFDO1lBQ3ZFLENBQUM7WUFFRCxJQUFJLENBQUMsUUFBUSxHQUFHLEtBQUssQ0FBQyxHQUFHLE9BQU8sQ0FBQztRQUNyQyxDQUFDO1FBQ0wsd0JBQUM7SUFBRCxDQXRDQSxBQXNDQyxJQUFBO0lBdENZLGlDQUFpQixvQkFzQzdCLENBQUE7SUFFRCxnQkFBQSxNQUFNLENBQUMsT0FBTyxDQUFDLE9BQU8sRUFBRSxpQkFBaUIsQ0FBQyxDQUFDO0FBQy9DLENBQUMsRUF0RmdCLGVBQWUsR0FBZix1QkFBZSxLQUFmLHVCQUFlLFFBc0YvQiIsImZpbGUiOiJnZW5lcmF0ZWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uIGUodCxuLHIpe2Z1bmN0aW9uIHMobyx1KXtpZighbltvXSl7aWYoIXRbb10pe3ZhciBhPXR5cGVvZiByZXF1aXJlPT1cImZ1bmN0aW9uXCImJnJlcXVpcmU7aWYoIXUmJmEpcmV0dXJuIGEobywhMCk7aWYoaSlyZXR1cm4gaShvLCEwKTt2YXIgZj1uZXcgRXJyb3IoXCJDYW5ub3QgZmluZCBtb2R1bGUgJ1wiK28rXCInXCIpO3Rocm93IGYuY29kZT1cIk1PRFVMRV9OT1RfRk9VTkRcIixmfXZhciBsPW5bb109e2V4cG9ydHM6e319O3Rbb11bMF0uY2FsbChsLmV4cG9ydHMsZnVuY3Rpb24oZSl7dmFyIG49dFtvXVsxXVtlXTtyZXR1cm4gcyhuP246ZSl9LGwsbC5leHBvcnRzLGUsdCxuLHIpfXJldHVybiBuW29dLmV4cG9ydHN9dmFyIGk9dHlwZW9mIHJlcXVpcmU9PVwiZnVuY3Rpb25cIiYmcmVxdWlyZTtmb3IodmFyIG89MDtvPHIubGVuZ3RoO28rKylzKHJbb10pO3JldHVybiBzfSkiLCJleHBvcnQgbmFtZXNwYWNlIEdlbmVyYXRlZENsaWVudCB7XHJcbiAgICBleHBvcnQgbGV0IE1vZHVsZSA9IGFuZ3VsYXIubW9kdWxlKCdleGFtcGxlLWdlbmVyYXRlZCcsIFtdKTtcclxuICAgIFxyXG4gICAgZnVuY3Rpb24gcmVwbGFjZVVybCh1cmw6IHN0cmluZywgcGFyYW1zOiB7IH0pIHtcclxuICAgICAgICBsZXQgcmVwbGFjZWQgPSB1cmw7XHJcbiAgICAgICAgZm9yIChsZXQga2V5IGluIHBhcmFtcykge1xyXG4gICAgICAgICAgICBpZiAocGFyYW1zLmhhc093blByb3BlcnR5KGtleSkpIHtcclxuICAgICAgICAgICAgICAgIGNvbnN0IHBhcmFtID0gcGFyYW1zW2tleV07XHJcbiAgICAgICAgICAgICAgICByZXBsYWNlZCA9IHJlcGxhY2VkLnJlcGxhY2UoJ3snICsga2V5ICsgJ30nLCBwYXJhbSk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIHJlcGxhY2VkO1xyXG4gICAgfVxyXG4gICAgXHJcbiAgICBleHBvcnQgY2xhc3MgQXBpRXhhbXBsZVNlcnZpY2Uge1xyXG4gICAgICAgIHN0YXRpYyAkaW5qZWN0ID0gWyckaHR0cCcsICckcSddO1xyXG4gICAgICAgIGNvbnN0cnVjdG9yKHByaXZhdGUgaHR0cCwgcHJpdmF0ZSBxKXsgfVxyXG4gICAgICAgIFxyXG4gICAgICAgIHB1YmxpYyBFeGFtcGxlTWV0aG9kQ29uZmlnKCkgOiBuZy5JUmVxdWVzdENvbmZpZyB7XHJcbiAgICAgICAgICAgIHJldHVybiB7XHJcbiAgICAgICAgICAgICAgICB1cmw6ICdhcGkvZXhhbXBsZS9leGFtcGxlJyxcclxuICAgICAgICAgICAgICAgIG1ldGhvZDogJ0dFVCcsXHJcbiAgICAgICAgICAgIH07XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHB1YmxpYyBFeGFtcGxlTWV0aG9kID0gKCkgOiBuZy5JUHJvbWlzZTxFeGFtcGxlV2ViQVBJLk1vZGVscy5JRXhhbXBsZU1vZGVsPiA9PiB7XHJcbiAgICAgICAgICAgIHJldHVybiB0aGlzLmh0dHAodGhpcy5FeGFtcGxlTWV0aG9kQ29uZmlnKCkpXHJcbiAgICAgICAgICAgICAgICAudGhlbihyZXNwID0+IHtcclxuICAgICAgICAgICAgICAgICAgICByZXR1cm4gcmVzcC5kYXRhO1xyXG4gICAgICAgICAgICAgICAgfSwgcmVzcCA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgcmV0dXJuIHRoaXMucS5yZWplY3Qoe1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBTdGF0dXM6IHJlc3Auc3RhdHVzLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICBNZXNzYWdlOiAocmVzcC5kYXRhICYmIHJlc3AuZGF0YS5NZXNzYWdlKSB8fCByZXNwLnN0YXR1c1RleHRcclxuICAgICAgICAgICAgICAgICAgICB9KTtcclxuICAgICAgICAgICAgICAgIH0pO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxuICAgIFxyXG4gICAgTW9kdWxlLnNlcnZpY2UoJ0FwaUV4YW1wbGVTZXJ2aWNlJywgQXBpRXhhbXBsZVNlcnZpY2UpO1xyXG4gICAgXHJcbiAgICBleHBvcnQgbmFtZXNwYWNlIEV4YW1wbGVXZWJBUEkuTW9kZWxzIHtcclxuICAgICAgICBleHBvcnQgaW50ZXJmYWNlIElFeGFtcGxlTW9kZWwge1xyXG4gICAgICAgICAgICBNZXNzYWdlOiBzdHJpbmc7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIFxyXG4gICAgfVxyXG4gICAgZXhwb3J0IGNsYXNzIEVudW1IZWxwZXJTZXJ2aWNlIHtcclxuICAgICAgICBjb25zdHJ1Y3RvcigpIHtcclxuICAgICAgICB9XHJcbiAgICAgICAgXHJcbiAgICBcdHB1YmxpYyBSZWdpc3RlciA9IChuYW1lOiBzdHJpbmcsIGVudW10eXBlOiBhbnksIHRpdGxlcz86IHsgW2tleTogc3RyaW5nXTogc3RyaW5nIH0pID0+IHtcclxuICAgICAgICAgICAgdGhpcy5SZWdpc3RlckFycmF5KG5hbWUsIGVudW10eXBlLCB0aXRsZXMpO1xyXG4gICAgICAgICAgICB0aGlzLlJlZ2lzdGVySGFzaChuYW1lLCBlbnVtdHlwZSwgdGl0bGVzKTtcclxuICAgICAgICB9XHJcbiAgICBcclxuICAgIFx0cHJpdmF0ZSBSZWdpc3RlckFycmF5ID0gKGVudW1uYW1lOiBzdHJpbmcsIGVudW10eXBlOiBhbnksIHRpdGxlcz86IHsgW2tleTogc3RyaW5nXTogc3RyaW5nIH0pID0+IHtcclxuICAgICAgICAgICAgdmFyIGVudW1BcnJheSA9IFtdO1xyXG4gICAgICAgICAgICBmb3IgKHZhciBlbnVtTWVtYmVyIGluIGVudW10eXBlKSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgaXNWYWx1ZVByb3BlcnR5ID0gcGFyc2VJbnQoZW51bU1lbWJlciwgMTApID49IDA7XHJcbiAgICAgICAgICAgICAgICBpZiAoaXNWYWx1ZVByb3BlcnR5KSB7XHJcbiAgICAgICAgICAgICAgICAgICAgdmFyIG5hbWUgPSBlbnVtdHlwZVtlbnVtTWVtYmVyXTtcclxuICAgICAgICAgICAgICAgICAgICB2YXIgdmFsdWUgPSBwYXJzZUludChlbnVtTWVtYmVyKTtcclxuICAgICAgICAgICAgICAgICAgICB2YXIgdGl0bGUgPSAodGl0bGVzICYmIHRpdGxlc1tuYW1lXSkgfHwgbmFtZTtcclxuICAgICAgICBcclxuICAgICAgICAgICAgICAgICAgICBlbnVtQXJyYXkucHVzaCh7IE5hbWU6IG5hbWUsIFZhbHVlOiB2YWx1ZSwgVGl0bGU6IHRpdGxlfSk7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICBcclxuICAgICAgICAgICAgdGhpc1tlbnVtbmFtZV0gPSBlbnVtQXJyYXk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIFxyXG4gICAgICAgIHByaXZhdGUgUmVnaXN0ZXJIYXNoKGVudW1uYW1lOiBzdHJpbmcsIGVudW10eXBlOiBhbnksIHRpdGxlcz86IHsgW2tleTogc3RyaW5nXTogc3RyaW5nIH0pIHtcclxuICAgICAgICAgICAgdmFyIGVudW1PYmogPSB7fTtcclxuICAgICAgICAgICAgZm9yICh2YXIgZW51bU1lbWJlciBpbiBlbnVtdHlwZSkge1xyXG4gICAgICAgICAgICAgICAgdmFyIGlzVmFsdWVQcm9wZXJ0eSA9IHBhcnNlSW50KGVudW1NZW1iZXIsIDEwKSA+PSAwO1xyXG4gICAgICAgICAgICAgICAgdmFyIG5hbWUgPSBpc1ZhbHVlUHJvcGVydHk/IGVudW10eXBlW2VudW1NZW1iZXJdIDogZW51bU1lbWJlcjtcclxuICAgICAgICAgICAgICAgIHZhciB2YWx1ZSA9IGlzVmFsdWVQcm9wZXJ0eSA/IHBhcnNlSW50KGVudW1NZW1iZXIpIDogcGFyc2VJbnQoZW51bXR5cGVbZW51bU1lbWJlcl0pO1xyXG4gICAgICAgICAgICAgICAgdmFyIHRpdGxlID0gdGl0bGVzID8gdGl0bGVzW25hbWVdIDogbmFtZTsgXHJcbiAgICBcclxuICAgICAgICAgICAgICAgIGVudW1PYmpbZW51bU1lbWJlcl0gPSAoeyBOYW1lOiBuYW1lLCBWYWx1ZTogdmFsdWUsIFRpdGxlOiB0aXRsZSB9KTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIFxyXG4gICAgICAgICAgICB0aGlzW2VudW1uYW1lICsgJ09iaiddID0gZW51bU9iajtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbiAgICBcclxuICAgIE1vZHVsZS5zZXJ2aWNlKCdFbnVtcycsIEVudW1IZWxwZXJTZXJ2aWNlKTtcclxufVxyXG4iXX0=
