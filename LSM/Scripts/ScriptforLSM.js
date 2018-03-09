
    $(document).ready(function () {
        
   


        alert("Test")
        

        $("#button").on("click", function () {
            alert("hej")
            $("hej").html('@{ Html.RenderPartial("ViewPage1");}');
            
           


        });


    });