/// <reference path="angular.js" />

var App = angular.module("MyApp", []);

var host = "http://localhost:62232/";

App.controller("ProductCtrl", function($scope,$http) {
    $scope.products = [];
    $scope.sepetList = [];
    $scope.ekleMi = false;
    $scope.sepetToplam = 0;
    $scope.categories = [];
    $scope.selectedCat = null;

    function init() {
        $http({
            method: 'GET',
            url: host + "api/category/getcategories"
        }).then(function successCallback(response) {
            console.log(response);
            var r = response.data;
            if (r.success) {
                $scope.categories = r.data;
                $scope.selectedCat = $scope.categories[0].CategoryID.toString();
            }
            }, function errorCallback(response) {
            console.log(response);
            })

        $http({
            method: 'GET',
            url: host + "api/products/getproducts"
        }).then(function successCallback(response) {
            console.log(response);
            var r = response.data;
            if (r.success) {
                $scope.products = r.data;
                $scope.selectedCat = $scope.categories[0].CategoryID.toString();
            }
        }, function errorCallback(response) {
            console.log(response);
        })
    };

    $scope.add = function() {
        $http({
            method: 'POST',
            url: host + "api/products/postproduct",
            data: {
                ProductID: Math.floor(Math.random() * 1001),
                ProductName: $scope.yeni.ProductName,
                UnitPrice: $scope.yeni.UnitPrice,
                CategoryID: $scope.selectedCat
            }
        }).then(function(rs) {
            if (rs.data.success) {
                alert(rs.data.message);
                init();
            } else {
                alert("bir hata oluştu " + rs.data.message);
            }
        })
    };

    $scope.update = function(id) {
        $http({
            method: 'POST',
            url: host + "api/products/putproduct",
            data: { id: id }
        }).then(function(rs) {
            if (rs.data.success) {
                alert(rs.data.message);
                init();
            } else {
                alert("bir hata oluştu " + rs.data.message);
            }
        })
    };

    init();
})