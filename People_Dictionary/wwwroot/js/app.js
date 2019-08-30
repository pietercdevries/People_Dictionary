(function() {
    // The main angular app for people search dashboard.
    var app = angular.module('searchPeopleApp', []);

    // Add controllers to the dashboard.
    app.controller('DashboardController', ['$scope', '$http', DashboardController]);
    app.controller('AddPersonController', ['$scope', '$http', '$timeout', AddPersonController]);
}());