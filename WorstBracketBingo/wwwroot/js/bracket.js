$(document).ready(function () {
    $("#currentRound").find(".thumbnail").each(function () {

        $(this).addClass("contenderContainer");

        $(this).click(function () {
            var thumbDiv = $(this);
            $.ajax({
                url: '/Contenders/UpdateEliminationStatus/' + $(this).attr("data-contenderId"),
                type: 'POST',
                success: function (obj) {
                    var div = thumbDiv.find("#contenderOverlay");

                    if (obj.eliminated == true)
                    {
                        div.removeClass("contenderOverlay");
                        div.addClass("contenderEliminated");
                        
                    }
                    else
                    {
                        div.addClass("contenderOverlay");
                        div.removeClass("contenderEliminated");
                    }
                }
            });
        });
    });
});
