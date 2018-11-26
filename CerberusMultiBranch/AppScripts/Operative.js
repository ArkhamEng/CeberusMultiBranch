

//Muestra ventana de cancelación
function BeginCancelSale(id, onCancelCompleated)
{
    ShowLoading('static');

    ExecuteAjax("/Sales/BeginCancel", { id: id }, function (response)
    {
        HideLoading(function ()
        {
            ShowModal(response, 'static');

            OnCancelCompleated = onCancelCompleated;
        });
    });
}

function BeginCancelPurchase(id, onCancelCompleated)
{
    ShowLoading('static');

    console.log(id);

    ExecuteAjax("/Purchases/BeginCancel", { id: id }, function (response)
    {
        HideLoading(function () {
            ShowModal(response, 'static');

            OnCancelCompleated = onCancelCompleated;
        });
    });
}