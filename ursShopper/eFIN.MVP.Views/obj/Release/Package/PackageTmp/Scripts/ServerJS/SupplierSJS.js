SupplierApp.factory('SupplierService', ['$http', function ($http) {
    var SupplierService = {};
    SupplierService.getSuppliers = function () {
        return $http.get('/Suppliers/GetSuppliers');
    };
    SupplierService.getSupplierProducts = function (supplier_name) {
        return $http.get('/Suppliers/GetSupplierProducts?SupplierName=' + supplier_name);
    };
    SupplierService.getSupplierOffers = function (supplier_id) {
        return $http.get('/Suppliers/GetSupplierOffers?supplierID=' + supplier_id);
    };

    return SupplierService;
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