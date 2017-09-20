$(document).ready(function () {
    $("button#eliminateBtn").on('click', function () {
        var btn = $(this);
        var eliminatedDiv = $(this).parent().parent();
        var eliminatedName = eliminatedDiv.find("#entrantName")
        var eliminatedTag = eliminatedDiv.find("#entrantTag");
        var eliminatedThumb = eliminatedDiv.find("#entrantThumb");

        var editModal = $('#eliminateModal');
        editModal.find('.modal-title').text("Eliminate" + eliminatedName.text());
        editModal.find('.modal-body').text("Are you sure you want to eliminate this contender?");
        editModal.modal();

        editModal.find('button#confirm').click(function () {
            editModal.modal('hide');

            editModal.off('hidden.bs.modal');
            editModal.on('hidden.bs.modal', function (e) {
                $.ajax({
                    url: '/Contenders/UpdateEliminationStatus/' + btn.attr("data-contenderId"),
                    type: 'POST',
                    success: function (obj) {
                        if (obj.eliminated == true) {
                            eliminatedDiv.addClass("table-danger");
                            eliminatedThumb.addClass("eliminatedThumb");
                            eliminatedName.html("<s>" + eliminatedName.text() + "</s>");
                            eliminatedTag.html("<s>" + eliminatedTag.text() + "</s>");
                            btn.prop('disabled', true);
                        }
                    }
                });
            });
        });
    });
});
