ConsumerApp.factory('ConsumerService', ['$http', function ($http) {
    var ConsumerService = {};
    ConsumerService.getConsumers = function () {
        return $http.get('/Consumers/GetConsumers');
    };
    //ConsumerService.getConsumerProducts = function (consumer_id) {
    //    return $http.get('/Consumers/GetConsumerProducts?consumerID=' + consumer_id);
    //};
    ConsumerService.getConsumerOffers = function (marketerId, consumerHandle) {
        return $http.get('/Consumers/GetConsumerOffers?ConsumerHandle=' + consumerHandle + '&MarketerId=' + marketerId);
    };
    return ConsumerService;
}]);
