var MarketerApp = angular.module("MarketerApp", []); //'ui.bootstrap'

MarketerApp.controller('MarketersController', function ($scope, MarketerService) {
    getMarketers();
    function getMarketers() {
        MarketerService.getMarketers()
            .success(function (studs) {
                $scope.Marketers = studs;
                console.log($scope.Marketers);
            })
            .error(function (error) {
                $scope.status = 'Unable to load marketer data: ' + error.message;
                alert($scope.status);
            });
    };

    $scope.getMarketerProducts = function(marketer) {
        MarketerService.getMarketerProducts(marketer.Name)
              .success(function (mktrProducts) {
                  marketer.Products = mktrProducts;
              })
              .error(function (error) {
                  $scope.status = 'Unable to load marketer product data: ' + error.message;
                  console.log($scope.status);
              });
    };


    // to be moved to common JS
    $scope.convertToDateTime = function (dbDateValue) {
        alert(dbDateValue);
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
