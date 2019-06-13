

function SearchProviderProduct(filter, fromModal)
{
    var filtr = $(filter).val();

 
    if (filtr.length < 4)
    {
        ShowNotify("Datos insuficientes!", "info", "Para realizar una busqueda en el catálogo de proveedores"
            + "es necesario ingresar por lo menos 4 caracteres en el filtro de descripción / código", 4000);
        return;
    }

    if (fromModal)
        ShowModLoading();
    else
        ShowLoading('static');

    ExecuteAjax('../../Products/ExternalSearch', { filter: filtr }, function (response)
    {
        if (fromModal)
        {
            HideModLoading(function ()
            {
                HideModal(function ()
                {
                    ShowModal(response, "static", "lg", null);
                });
            });
        }
        else
        {
            HideLoading(function ()
            {
                ShowModal(response, "static", "lg", null);
            });
        }
    });
}

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

    ExecuteAjax("/Purchases/BeginCancel", { id: id }, function (response) {
        HideLoading(function () {
            ShowModal(response, 'static');

            OnCancelCompleated = onCancelCompleated;
        });
    });
}


function ShowProviderCode(productId, compleateCallBack) {
    ShowModLoading();

    ExecuteAjax('/PurchaseEstimation/BeginSetCode', { productId: productId, providerId: $("#ProviderId").val() }, function (response) {
        HideModLoading(function () {
            ShowChildModal(response, null, 'sm');

            $("#btnApplyProviderCode").off('click').on('click', function () {
                var price = $("#Price").val();
                var providerCode = $("#ProviderCode").val();
                var providerId = $("#ProviderId").val();

                if (isNaN(price) || price < 0) {
                    ShowNotify("Precio Incorrecto", "warning", "El precio proveedor debe ser un número positivo", 3000);
                    return;
                }

                if (providerCode.lenght <= 5) {
                    ShowNotify("Código incorrecto", "warning", "El código proveedor, requiere una longitud mayos a 5 caracteres", 3000);
                    return;
                }

                var model = {
                    ProductId: productId,
                    ProviderId: providerId,
                    ProviderCode: providerCode,
                    Price: price
                };

                HideChildModal(function () {
                    ShowModLoading();

                    ExecuteAjax('/PurchaseEstimation/SetProviderCode', { model: model }, function (response) {
                        HideModLoading(function () {
                            ShowNotify(response.Header, response.Result, response.Body, 3000);
                            compleateCallBack();
                        });
                    });
                });
            });
        });
    });
}

//Validata la partida que agregara a la orden de compra
function ValidateDetails(model) {
    if ((model.MaxQuantity - model.Quantity) < model.AddQuantity) {
        ShowNotify("Cantidad excedente", "danger", "estas intentando comprar mas producto del requerido", 3000);
        return false;
    }

    if (model.ProviderCode == "No asignado") {
        ShowNotify("Si Código de proveedor", "danger", "Imposible agregar el producto sin un código de proveedor", 3000);
        return false;
    }
    if (model.AddQuantity <= 0) {
        ShowNotify("Cantidad incorrecta", "warning", "la cantidad ordenada debe ser mayor a 0", 3000);
        return false;
    }

    if (model.Discount < 0 || model.Discount > 100) {
        ShowNotify("Descuento incorrecto", "warning", "El procentaje de descuento debe ser un valor entre 0 y 100", 3000);
        return false;
    }

    if (model.BuyPrice <= 0) {
        ShowNotify("Precio de compra incorrecto", "danger", "El precio de compra debe ser mayor a $0.00", 3000);
        return false;
    }

    return true;
}
