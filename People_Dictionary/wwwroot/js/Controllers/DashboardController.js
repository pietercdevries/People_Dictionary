// This controller is in charge of the data and the search.
var DashboardController = function ($scope, $http) {
    var availableTags = new Array(); // This is for the autocomplete list data source.
    var server = "https://people-dictionary.azurewebsites.net/api/people";

    // Declare our loading screen ui element.
    var $loading = $('#loadingDivOverlay');

    // This will load data in the table and the auto complete.
    $scope.loadData = function () {
        $loading.show();

        // When the search bar is empty it returned undefined so we will need to set it to an empty string.
        if ($scope.searchValue == null){
            $scope.searchValue = "";
        }

        // Get the data and add the search value and teh desired speed you wish to return the data.
        $http.get(server + "?name=" + $scope.searchValue + "&speed=" + $scope.speed)
            .then(onSuccess, onError);
    };

    // This will be called when the server returns are response.
    var onSuccess = function (response) {
        $loading.hide();

        // Set the people in the scope for the table.
        $scope.people = response.data;

        // Set the auto complete data. We do this every time in case a user was added by another application.
        addAutocompleteOptions(response.data);
    };

    // This is will be used for then the http web request fails.
    var onError = function(reason) {
        // Set the error and show an error on the view using ng-show="httpError"
        $scope.httpError = reason;
        $loading.hide();
    };

    // This will add the autocomplete data to the view and it will initialize it.
    var addAutocompleteOptions = function(people){
        // Go trough all people and add the first and last name to the list.
        $.each(people, function(key,value) {
            // Check fo duplicates.
            if(availableTags.indexOf(value.FirstName) < 0) {
                availableTags.push(value.FirstName);
            }

            // Check for duplicates.
            if(availableTags.indexOf(value.LastName) < 0) {
                availableTags.push(value.LastName);
            }
        });

        // Add the data source and initialize the autocomplete.
        $(".autocomplete").autocomplete({
            source: availableTags
        }).focus();
    }

    // Go load the data now on initial startup.
    $scope.loadData();

    // We saved the date of birth so we want to show the age and we calculate that here.
    // @params birthday This is the date you want to return as age from today.
    $scope.calculateAge = function calculateAge(birthday) {
        var ageDifMs = Date.now() - new Date(birthday).getTime();
        var ageDate = new Date(ageDifMs);

        return Math.abs(ageDate.getUTCFullYear() - 1970);
    };
};