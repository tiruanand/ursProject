var ConsumerApp = angular.module("ConsumerApp", []); //'ui.bootstrap'

angular.module('ConsumerApp').filter('jsonDate', function () {
    return function (jsonDate) {
        if (!jsonDate) { return ''; }

        if (jsonDate == null || jsonDate.length == 0)
            return "";
        var startPos = jsonDate.indexOf("(");
        var lastPos = jsonDate.indexOf(")");
        return jsonDate.substring(startPos + 1, lastPos);
    };
});

ConsumerApp.controller('ConsumerController', function ($scope, ConsumerService) {
    getConsumers();
    function getConsumers() {
        ConsumerService.getConsumers()
            .success(function (studs) {
                $scope.Consumers = studs;
                console.log($scope.Consumers);
            })
            .error(function (error) {
                $scope.status = 'Unable to load consumer data: ' + error.message;
                alert($scope.status);
            });
    };

    $scope.getConsumerOffers = function (consumer) {
        ConsumerService.getConsumerOffers(consumer.MarketerId, consumer.ConsumerHandle)
              .success(function (conOffers) {
                  var offerCount = conOffers.length;
                  if (offerCount > 0) {
                      for (var loop = 0; loop < offerCount; loop++) {
                          alert('Offer Date:' + GetJSONDate(conOffers[loop].ConsumerOfferDate)
                          + ' \nQty:' + conOffers[loop].ConsumerOfferQty
                          + ' \nPrice:' + conOffers[loop].ConsumerOfferMaxPrice
                          + ' \nEnd Date:' + GetJSONDate(conOffers[loop].ConsumerOfferEndDate)
                          + ' \nIs Current:' + conOffers[loop].IsConsumerOfferCurrent);
                      };
                  }
                  else {
                      alert('No offer found for the customer ' + consumer.ConsumerHandle);
                  }
                  consumer.ConsumerOffers = conOffers;
              })
              .error(function (error) {
                  $scope.status = 'Unable to load consumer offers data: ' + error.message;
                  alert($scope.status);
                  console.log($scope.status);
              });
    };
});


function GetJSONDate(jsonDate) {
    if (!jsonDate) { return ''; }

    if (jsonDate == null || jsonDate.length == 0)
        return "";
    var startPos = jsonDate.indexOf("(");
    var lastPos = jsonDate.indexOf(")");
    var dateVal = Math.abs(parseInt(jsonDate.substring(startPos + 1, lastPos)));
    return (new Date(dateVal));
    //return new Date(jsonDate.substring(startPos + 1, lastPos));
};