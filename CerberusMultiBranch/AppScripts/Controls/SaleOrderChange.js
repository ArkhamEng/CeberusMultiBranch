
var OnChangeCompleate = function () { };

$(document).ready(function ()
{
    var form = $("#SaleOrderChangeForm");
    form.validate();

    $("#SaleOrderChangeAccept").off("click").on("click", CancelSale);
    $("#SaleOrderChangeCancel").off("click").on("click", function () { HideModal(null, true); });
    $("#SaleOrderChangeClose").off("click").on("click", function () { HideModal(null, true); });    

    if ($("#IsCancelation").val() == 'true')
    {
        $("#SaleOrderChangePayment").removeClass("hidden");
        $("#SaleOrderChangeTitle").html('<i class="fa fa-ban"></i> Cancelación de venta')
    }
});


function CancelSale(e)
{
    var form = $("#SaleOrderChangeForm");

    if (!form.valid())
    {
        ShowNotify("Faltan datos!", "warning", "Debes agregar un comentario para cancelar o solicitar cambio", 4000);
        return;
    }

    HideModal(function ()
    {
        ShowLoading("static");

        var param = {
            saleId: $("#SaleCancelId").val(),
            comment: $("#CancelComment").val(),
            isCancelation: $("#IsCancelation").val()
        }

        ExecuteAjax("/Selling/RequestChange", param, function (response)
        {
            HideLoading(function ()
            {
                OnChangeCompleate(response);
            });
        });

    }, true);
}
