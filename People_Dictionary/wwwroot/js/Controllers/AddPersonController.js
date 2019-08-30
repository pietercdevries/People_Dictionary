// This controller is responsible for creating a new user.
var AddPersonController = function ($scope, $http, $timeout) {
    // Set the server for connecting to get the people.
    var server = "https://people-dictionary.azurewebsites.net/api/people";

    $scope.getNewUser = function(){
        // Angular has issue with date picker because it has it's own scope. We need to set the date here.
        $scope.DateOfBirth = $(".datepicker").val();

        // Get the date of birth as a date.
        var dateParts = $scope.DateOfBirth.split('/');
        var dateOfBirth = new Date(dateParts[2], dateParts[0] -1, dateParts[1]);

        // Create a new person object to be send to the api.
        $scope.newPerson = {
            FirstName : $scope.FirstName,
            LastName : $scope.LastName,
            StreetAddress : $scope.StreetAddress,
            StreetAddressAdditional : $scope.StreetAddressAdditional,
            City : $scope.City,
            State : $scope.State,
            ZipCode : $scope.ZipCode,
            DateOfBirth : dateOfBirth,
            Interests : $scope.Interests,
            AvatarUrl : $scope.AvatarUrl,
            Active : true,
        };

        // Convert object to Json and don't make it look pretty.
        var newPersonJson = angular.toJson($scope.newPerson, false);

        // Post the new user to api and handle the response appropriately.
        $http.post(server, newPersonJson, {headers: {'Content-Type': 'application/json; charset=utf-8', 'responseType': 'text', 'Access-Control-Allow-Origin': '*', 'Access-Control-Allow-Methods' : 'GET,PUT,POST,DELETE'}})
            .then(onSuccess, onError);
    }

    // If the web request is successful this method will be called.
    var onSuccess = function(response) {
        // We need to know what response we received.
        if (response.data == "success"){
            // Let the user know a new person has been added.
            $scope.newUserAlert = "The new user has been added! Reloading Page...";
            $scope.addingUserAlertClass = "alert-success";

            // Show the user that the we added a new person and wait so they can see the message.
            $timeout(function(){location.reload();}, 3000, false);
        }
        else {
            // Show the user the action failed and append the reason.
            $scope.newUserAlert = "Error! " + response.data;
            $scope.addingUserAlertClass = "alert-danger";
        }
    };

    // This will be called when the request failed.
    var onError = function(reason) {
        // We got an http request error let the user know.
        $scope.newUserAlert = "Error! Could not contact server.";
        $scope.addingUserAlertClass = "alert-danger";
    };
}
