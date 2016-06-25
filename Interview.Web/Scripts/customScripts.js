function DeleteBusinessPartner() {
    var partner = $('.glyphicon-remove');
    var id = 0;
    for (var i = 0; i < partner.length; i++) {
        partner[i].addEventListener("click", function (data) {
            //id = ($(this).attr('id'));
            id = partner.parent().parent().attr('id');
            $.ajax({
                url: 'BusinessPartner/DeletePartner',
                data: { id: id },
                method: 'POST',
                success: function (result) {
                    $('#' + id).remove();
                }
            })
        })
    }
}
DeleteBusinessPartner();