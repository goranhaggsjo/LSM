$(document).ready(function () {
    $('#message2').fadeIn('slow', function () {
        $('#message2').delay(5000).fadeOut();
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
