

$(document).ready(function () {
    $('#message2').fadeIn('slow', function () {
        $('#message2').delay(3000).fadeOut('slow');
    });
    
$("#buttonSchedule").on("click", function () {

    $("#tableSchedule").show();
    $("#tableCourseInfo").hide();   

});

$("#buttonCourseInfo").on("click", function () {

    $("#tableCourseInfo").show();
    $("#tableSchedule").hide();
    $("#buttonSchedule").removeClass("active")

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


});