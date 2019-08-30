(function()
{
    // This function is to be used to initialize components.
    function init(){
        // For the sidebar icons to be loaded.
        feather.replace();

        // Enable date picker.
        $('.datepicker').datepicker("setDate", new Date());

        // Set a click event handler to export the csv file.
        $(".export").on('click', function(event) {
            var args = [$('#table-container>table'), 'export.csv'];

            exportTableToCSV.apply(this, args);
        });
    }

    // Initialize components on load.
    init();
}());

