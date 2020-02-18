
$(document).ready(function () {

    SetColor();

    $("#btnNew").off('click').on('click', function () {
        ShowLoading('static');
        window.location = '/Offers/Editor/';
    });

    $("#btnGoToSearch").off('click').on('click', function () {
        ShowLoading('static');
        window.location = '/Offers/';
    });

    $("#btnShowEditor").off('click').on('click', function (e) {
        ShowImageEditor('horizontal', $('#detLg'), function (images) {
            $("#Base64").val(images[2]);
            $("#HasImage").val(true);

        });
    });

    $("#Description").off('keyup').on('keyup', function (e) {
        $(".ImageText").text(this.value);
    });

    $("#TextColor").off('change').on('change', SetColor);

    $("#ShadowColor").off('change').on('change', SetColor);

    $("#SaveForm").off('submit').on('submit', function (e) { Submit(e); });
});


function SetColor() {
    var textColor = $("#TextColor :selected").val();
    var shadowColor = $("#ShadowColor :selected").val();

    var shadow = '-1px 0 ' + shadowColor + ', 0 1px ' + shadowColor + ', 1px 0 ' + shadowColor + ', 0 -1px ' + shadowColor;

    $(".ImageText").css('textShadow', shadow);
    $(".ImageText").css('color', textColor);
}


function Submit(e) {
    e.preventDefault();

    var $form = $(e.target),
        formData = new FormData(),
        params = $form.serializeArray();

    //agrego todos los campos del formulario
    $.each(params, function (i, val) {
        formData.append(val.name, val.value);
    });

    var hasImage = formData.get("HasImage");

    if (hasImage == 'False' || hasImage == 'false') {
        ShowNotify('Oferta sin Imagen', 'warning', "Debe cargar una imagen para poder guardar la oferta", 4000);
        return;
    }

    ShowLoading('static');

    SubmitAjax($form.attr('action'), formData, OnDataSaved, OnError);
}

function OnDataSaved(response) {

    console.log(response);
    if (response.Id > 0) {
        ShowLoading('static');

        window.location = '/Offers/Editor/' + response.Id;
    }

}

function OnError(response) {
    HideLoading(function () {
    });
}

