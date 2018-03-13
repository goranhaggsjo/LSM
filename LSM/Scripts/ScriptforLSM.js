
$(document).ready(function () {
        
        


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
