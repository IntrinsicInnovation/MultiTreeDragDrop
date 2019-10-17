$(document).ready(function () {

    // Add the value of "Search..." to the input field
    $("#searchinput").val("Enter Criteria...");

    // When you click on #search
    $("#searchinput").focus(function () {

        // If the value is equal to "Search..."
        if ($(this).val() == "Enter Criteria...") {
            // remove all the text
            $(this).val("");
        }
    });

    // When the focus on #search is lost
    $("#searchinput").blur(function () {

        // If the input field is empty
        if ($(this).val() == "") {
            // Add the text "Search..."
            $(this).val("Enter Criteria...");
        }

    });

    //Todo:  Need to change hover class - or switch image for button.
    //    $("#search-submit").hover(function () {
    //        $(this).addClass("hover");
    //    });

    $("#search-submit").live("click", function () {
        var searchstring = document.getElementById("searchinput");

        //var searchstring = $("select[id$='searchinput']").val();
        window.location = "/Pages/Search/SearchResults.aspx?stxt=" + searchstring.value;
        return false;
    });

    $("#search-submit").attr("disabled", "disabled");
    $('#search-submit').fadeTo('fast', 0.3);

   
    $("form input").keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#search-submit').click();
            return false;         
            } 
       else {    
            return true;
            }
     }); 


    $('#searchinput').keydown(function () {
        setTimeout(function () {
            var mylength = $('#searchinput').val().length;
            if (mylength >= 2) {
                $("#search-submit").removeAttr("disabled");
                $('#search-submit').fadeTo('medium', 1);
            }
            else {
                $("#search-submit").attr("disabled", "disabled");
                $('#search-submit').fadeTo('medium', 0.3);
            }
        }, 50);
    });
});
