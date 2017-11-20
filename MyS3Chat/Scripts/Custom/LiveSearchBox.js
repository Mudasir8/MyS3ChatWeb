

$(document).ready(function () {

    $("body").click(function () {
        $("ul#livesearchul").empty();
        $("input#searchtext").val("");

    });

    $("input#searchtext").keyup(function () {

        var searchText = $("input#searchtext").val();

        $("ul#livesearchul").empty();

        if (searchText == "" || searchText == " ") return false;

        var url = "/ajax/LiveSearch";

        $.post(url, { searchText: searchText }, function (data) {

            //if ($("ul#livesearchul li.close").length) return;

            if (data.length > 0) {

                $("ul#livesearchul").removeClass("hide");
            }

            for (var i = 0; i < data.length; i++) {
                var obj = data[i];

                $("ul#livesearchul").append('<li class="livesearchli"><a href="/Profile/' + obj.UserName + '"><img class="img-circle" src="/Uploads/' + obj.ImagePath + ' " />' + ' ' + obj.FirstName + ' ' + obj.LastName + '</a></li>');


            }

        });

    });

    $("body").on("click", "ul#livesearchul li.close", function () {
        $("ul#livesearchul").empty();
        $("input#searchtext").val("");
    });



}); // ready end