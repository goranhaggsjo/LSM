$(document).ready(function () {
    $('#message2').fadeIn('slow', function () {
        $('#message2').delay(3000).fadeOut('slow');
    });
});




$("#buttonModules").on("click", function () {

    $("#tableModules").show();
    $("#tableStudents").hide();   

});

$("#buttonStudents").on("click", function () {

    $("#tableStudents").show();
    $("#tableModules").hide();
    $("#buttonModules").removeClass("active")

});
