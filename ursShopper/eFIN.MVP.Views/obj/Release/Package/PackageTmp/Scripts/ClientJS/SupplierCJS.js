var SupplierApp = angular.module("SupplierApp", []); //'ui.bootstrap'

angular.module('SupplierApp').filter('jsonDate', function () {
    return function (jsonDate) {
        if (!jsonDate) { return ''; }

        if (jsonDate == null || jsonDate.length == 0)
            return "";
        var startPos = jsonDate.indexOf("(");
        var lastPos = jsonDate.indexOf(")");
        return jsonDate.substring(startPos + 1, lastPos);
    };
});

SupplierApp.controller('SupplierController', function ($scope, SupplierService) {
    getSuppliers();
    function getSuppliers() {
        SupplierService.getSuppliers()
            .success(function (studs) {
                $scope.Suppliers = studs;
                console.log($scope.Suppliers);
            })
            .error(function (error) {
                $scope.status = 'Unable to load supplier data: ' + error.message;
                alert($scope.status);
            });
    };
    $scope.FunCall = function (sup) {
        alert(sup.supplier_id);
    };

    $scope.getSupplierProducts = function(supplier) {
        SupplierService.getSupplierProducts(supplier.Name)
              .success(function (supProducts) {
                  supplier.Products = supProducts.Products;
              })
              .error(function (error) {
                  $scope.status = 'Unable to load supplier product data: ' + error.message;
                  console.log($scope.status);
              });
    };

    $scope.convertToDateTime = function (dbDateValue) {
        if (dbDateValue == null || dbDateValue.length == 0)
            return "";

        var startPos = dbDateValue.indexOf("(");
        var lastPos = dbDateValue.indexOf(")");
        alert(dbDateValue.subString(startPos + 1, dbDate.length - lastPos));
        return dbDateValue.subString(startPos + 1, dbDate.length - lastPos);
    };

    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };

    $scope.pageChanged = function () {
        console.log('Page changed to: ' + $scope.currentPage);
    };

    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first paghe
    }

});
