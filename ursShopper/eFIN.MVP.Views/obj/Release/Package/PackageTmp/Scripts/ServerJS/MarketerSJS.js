MarketerApp.factory('MarketerService', ['$http', function ($http) {
    var MarketerService = {};
    MarketerService.getMarketers = function () {
        return $http.get('/Marketers/GetMarketers');
    };
    MarketerService.getMarketerProducts = function (marketer_name) {
        return $http.get('/Marketers/GetMarketerProducts?MarketerName=' + marketer_name);
    };
    return MarketerService;
}]);
/*
ProductsApp.factory('ProductsService', ['$http', function ($http) {
    var ProductsService = {};   
    ProductsService.getProducts = function () {
        return $http.get('/ProductEntities/GetProduct');
    };
    return ProductsService;
}]);
*/