
//global variables
selectedProducts = [];


//Inicia la acciones de revisión, autorización, recepción 
//segun el estado actual de la orden de compra
function BeginAction(changeRequested)
{
    ShowLoading('static');

    ExecuteAjax('/Purchasing/BeginAction', { id: $("#PurchaseOrderId").val(), changeRequested: changeRequested }, function (response)
    {
        HideLoading(function ()
        {
            if ($.isPlainObject(response))
            {
                ShowNotify(response.Header, response.Result, response.Body, 3000);
                Reload();
            }
            else
            {
                ShowModal(response, 'static');
            }
        });
    });
}


function SendPurchaseOrder()
{
    ShowLoading('static');

    ExecuteAjax('/Purchasing/SendPurchaseOrder', { id: $("#PurchaseOrderId").val() }, function (response)
    {
        HideLoading(function ()
        {
            ShowNotify(response.Header, response.Result, response.Body, 3000);
            Reload();

        });
    });
}

//recarga la vista parcial _PurchasOrder.cshtml
function Reload()
{
    ShowLoading();

    ExecuteAjax('/Purchasing/ReloadPurchaseOrder', { id: $("#PurchaseOrderId").val() }, function (response)
    {
        HideLoading(function () {
            $("#divPurchaseOrder").html(response);
        });
    });
}

//Inicia la captura de la fatura para una orden de compra
function BeginBilling()
{
    ShowLoading('static');

    ExecuteAjax('/Purchasing/BeginBilling', { id: $("#PurchaseOrderId").val() }, function (response)
    {
        HideLoading(function () {
            ShowModal(response);
        });
    });
}


/*****************PurchaseOrderDetails***************************************/

//acciones en la carga de la vista parcial _PurchaseOrderDetails.cshtml
function LoadDetails(pendingCount)
{
    if (pendingCount > 0)
    {
        $("#btnReceive").addClass("hidden");
        $("#btnChange").removeClass("hidden");

        $("#tbPurchaseDetails tr").each(function ()
        {
            $(this).find("#btnReceiveItem").addClass("hidden");
        });
    }

    else
    {
        $("#btnreceived").removeClass("hidden");
        $("#btnChange").addClass("hidden");

        $("#tbPurchaseDetails tr").each(function ()
        {
            $(this).find("#btnReceiveItem").removeClass("hidden");
        });
    }

    $('[data-toggle="tooltip"]').tooltip();
}

//Realiza la recepción temporal de la partida (hasta aqui los datos se cargan solo en memoria)
function Received(input)
{
    var row = $(input).parent().parent();

    var orderQty = parseFloat($(row).find("#item_OrderQty").val());

    var received = parseFloat($(input).val());

    if (received > orderQty)
        $(input).val(orderQty);

    if (received < 0)
        $(input).val(0);
}

function ReceiveProduct(id)
{
    var detail;

    $(ReceivedDetails).each(function (index, item)
    {
        if (item.DetailId == id)
        {
            detail = item;
        }
    });

    if (detail == null)
    {
        detail = { DetailId: parseInt(id) };
    }

    ShowLoading('static');

    ExecuteAjax('/Purchasing/ReceiveProduct', { item: detail }, function (response)
    {
        HideLoading(function ()
        {
            ShowModal(response, 'static', 'lg');
        });
    });
}

//Abre la ventana de detalle de recepcion de la partida (solo partidas recibidas cerradas)
function ViewDetail(id)
{
    ShowLoading('static');

    ExecuteAjax('/Purchasing/ViewDetail', { id: id }, function (response)
    {
        HideLoading(function ()
        {
            ShowModal(response, 'static', 'lg');
        });
    });
}

//Abre la ventana de búsqueda de productos
function OpenSearchProducts()
{
    ShowLoading('static');

    var filters = { providerId: $("#ProviderId").val(), branchId: $("#BranchId").val(), purchaseOrderId: $("#PurchaseOrderId").val() };

    ExecuteAjax('/Purchasing/OpenSearchProducts', filters, function (response)
    {
        HideLoading(function ()
        {
            ShowModal(response, 'static', 'ul');
        });
    });
}

//remueve una partida pendiente de guardar
function RemoveDetail(productId)
{
    var i = -1;

    $(selectedProducts).each(function (index, value)
    {
        if (value.ProductId == productId)
            i = index;
    });

    selectedProducts.splice(i, 1);;

    ShowLoading('static');

    ExecuteAjax('/Purchasing/BeginAddDetail', { items: selectedProducts, id: $("#PurchaseOrderId").val() }, function (response)
    {
        HideLoading(function ()
        {
            $("#divPurchaseDetails").html(response);
        });
    });
}


/*************************Search Product For Order*******************************************************/

//realiza búsqueda de productos por medio de los parametros
function SearchProduct()
{
    ShowModLoading();

    var filter = { filter: $("#FProductName").val(), providerId: $("#ProviderId").val(), branchId: $("#BranchId").val(), purchaseOrderId: $("#PurchaseOrderId").val() }

    ExecuteAjax('/Purchasing/SearchProducts', filter, function (response)
    {
        HideModLoading(function ()
        {
            $("#divProductList").html(response);
        });
    });
}


function CloseSearch()
{
    HideModal(null, true);
}

function CreateProduct()
{
    HideModal(function ()
    {
        ShowCatalogModal(null, null, 'Product', 0);

    }, true);
}

/******************************** SearchProductForOrderList**************************************/

function LoadSearchProductList()
{
    Paginate("#tbProducts", 10, false, "#FProductName");
    $('[data-toggle="tooltip"]').tooltip();

    $(selectedProducts).each(function (index, value)
    {
        $("#tbProducts tr").each(function (index, row)
        {
            var id = $(row).find("#sItem_ProductId").val();

            $(row).find("#sItem_Discount").val(value.Discount);
            $(row).find("#sItem_BuyPrice").val(value.BuyPrice);
            $(row).find("#sItem_AddQuantity").val(value.AddQuantity);

            if (id == value.ProductId)
            {
                $(row).find("td input").each(function ()
                {
                    $(this).prop("disabled", true);
                });

                $(row).find("td button").prop("disabled", true);

                $(row).addClass("alert alert-info");
            }
        });
    });
}

function AddProduct(button)
{
    var row = $(button).parent().parent();

    var model = {
        ProductId: row.find("#sItem_ProductId").val(),
        BuyPrice: parseFloat(row.find("#sItem_BuyPrice").val()),
        AddQuantity: parseFloat(row.find("#sItem_AddQuantity").val()),
        Discount: parseFloat(row.find("#sItem_Discount").val()),
        MaxQuantity: parseFloat(row.find("#sItem_MaxQuantity").val()),
        Quantity: parseFloat(row.find("#sItem_Quantity").val()),
        ProviderCode: row.find("#sItem_ProviderCode").val()
    };

    if (!ValidateDetails(model))
        return;

    row.find("td input").each(function ()
    {
        $(this).prop("disabled", true);
    });

    $(button).tooltip('hide');
    $(button).prop("disabled", true);

    row.addClass("alert alert-info");

    selectedProducts.push(model);

    ShowModLoading();

    ExecuteAjax('/Purchasing/BeginAddDetail', { items: selectedProducts, id: $("#PurchaseOrderId").val() }, function (response)
    {
        HideModLoading(function ()
        {
            $("#divPurchaseDetails").html(response);
        });
    });
}


function BeginProviderCode(productId)
{
    ShowProviderCode(productId, SearchProduct);
}

/************** Purchase Order Revision ****************************/

function SetAction(id, authorized, send)
{
    if (send && !authorized && $("#txtComment").val().length < 5 || !send && $("#txtComment").val().length < 5)
    {
        ShowNotify("Comentario requerido", "warning", "Se requiere un comentario mayor a 5 caracteres", 3000);
        return;
    }

    if (send && authorized && $("#Provider_Email").val().length < 5)
    {
        ShowNotify("Correo requerido", "warning", "Se requiere una dirección de correo válida", 3000);
        return;
    }

    var model = { id: id, comment: $("#txtComment").val(), authorized: authorized, to: $("#Provider_Email").val() };

    ShowModLoading();

    ExecuteAjax('/Purchasing/SetAction', model, function (response)
    {
        HideModLoading(function ()
        {
            ShowNotify(response.Header, response.Result, response.Body, 3000);

            HideModal(function () {
                Reload();
            }, true);

        });
    });
}

function Receive(id)
{
    if ($("#txtComment").val().length < 5)
    {
        ShowNotify("Comentario requerido", "warning", "Se requiere un comentario mayor a 5 caracteres", 3000);
        return;
    }

    if (ReceivedDetails.length < itemsCount)
    {
        ShowNotify("Revisión pendiente", "warning", "aun hay partidas pendientes de revisar", 3000);
        return;
    }

    ShowModLoading();

    var model = {
        items: ReceivedDetails,
        purchaseOrderId: id,
        comment: $("#txtComment").val(),
        shipMethodId: $("#ShipMethodId").val(),
        freight: $("#addFreight").val(),
        discount: $("#addDiscount").val(),
        insurance: $("#addInsurance").val(),
    };

    ExecuteAjax('/Purchasing/CompleateReception', model, function (response)
    {
        HideModLoading(function ()
        {
            HideModal(function () { Reload(); }, true);
        });
    });
}

function RequestChange(purchaseOrderId)
{
    ShowModLoading('static');

    ExecuteAjax('/Purchasing/RequestChange', { id: purchaseOrderId, items: selectedProducts, comment: $("#txtComment").val() }, function (response)
    {
        ShowNotify(response.Header, response.Result, response.Body, 3000);
        window.location = '/Purchasing/PurchaseOrder/' + purchaseOrderId;
    });
}