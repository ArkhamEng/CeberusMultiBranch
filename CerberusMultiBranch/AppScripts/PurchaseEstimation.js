var creditType = 1;

function ShowProviderQuickSearch()
{
    ShowQuickSearch('/Providers/ShowQuickSearch', function (id, name, ftr)
    {
        $("#ProviderId").val(id);
        $("#ProviderName").val(name);

        $("#btnBeginAddProduct").removeAttr("disabled");
    });
}

function RemoveAll()
{
    ShowLoading('static');

    ExecuteAjax('/PurchaseEstimation/RemoveAll', { providerId: $("#ProviderId").val() }, function (response)
    {
        ShowNotify(response.Header, response.Result, response.Body, 3000);
        window.location = '/Purchasing/PurchaseEstimation';
    });
}

function ShowProductQuickSearch()
{
    ShowLoading('static');

    var filters = { providerId: $("#ProviderId").val(), branchId: $("#BranchId").val() };

    ExecuteAjax('/PurchaseEstimation/OpenSearchProducts', filters, function (response)
    {
        HideLoading(function ()
        {
            ShowModal(response, 'static', 'ul');
        });
    });
}

function GenerateOrders()
{

    var model = {
        ProviderId: $("#ProviderId").val(),
        Comment: $("#Comment").val(),
        PurchaseType: $("#PurchaseType").val(),
        DaysToPay: $("#DaysToPay").val(),
        Insurance: $("#Insurance").val(),
        Freight: $("#Freight").val(),
        Discount: $("#GlobalDiscount").val(),
        ShipmentMethodId: $("#ShipmentMethodId").val(),
    };

    if (model.Comment.length < 5)
    {
        ShowNotify("Datos incorrectos", "warning", "Necesitas ingresar un comentario de 5 caracteres como mínimo", 3000);
        return;
    }

    if (model.PurchaseType == creditType && (!$.isNumeric(model.DaysToPay) || parseInt(model.DaysToPay) <= 0))
    {
        ShowNotify("Datos incorrectos", "warning", "Necesitas ingresar los dias de crédito", 3000);
        return;
    }

    ShowLoading('static');

    ExecuteAjax('/PurchaseEstimation/CreateOrders', { model: model }, function (response)
    {
        window.location = '/Purchasing/PurchaseOrders';
    });
}

/********************* Edit ESTIMATION DETAIL*************************/

function SetDiscount()
{
    var add = parseFloat($("#AddQuantity").val());
    var price = parseFloat($("#BuyPrice").val());
    var discount = parseFloat($("#Discount").val());


    if (!$.isNumeric(discount) || discount < 0 || discount > 100)
    {
        discount = 0;
        $("#Discount").val(discount);
    }
    var total = 0;

    if (discount > 0)
        total = (price * add) - ((price * add) * (discount / 100));
    else
        total = (price * add);

    $("#divTotal").text("Total Partida " + GetCurrency(total));
}

function SetTotal()
{
    var stock = parseFloat($("#Quantity").val());
    var max = parseFloat($("#MaxQuantity").val());
    var add = parseFloat($("#AddQuantity").val());
    var price = parseFloat($("#BuyPrice").val());

    if (!$.isNumeric(add) || add < 0)
    {
        add = 0;
        $("#AddQuantity").val(add);
    }

    var quantity = add + stock;

    if (quantity > max) {
        add = max - stock;

        $("#AddQuantity").val(add);
    }

    SetDiscount();
}

function SetDetailChange(productId, branchId)
{
    ShowModLoading();

    var model = { productId: productId, branchId: branchId, quantity: $("#AddQuantity").val(), discount: $("#Discount").val() };

   
    ExecuteAjax('/PurchaseEstimation/SetDatailChange', model, function (response)
    {
        HideModLoading(function ()
        {
            $("#divPurchaseDetails").html(response);

            HideModal(null, true);
        });
    });
}

/********************* Purchase Estimation Details **************************/

function RemoveFromEstimation(branchId, productId)
{
    ShowLoading('static');

    ExecuteAjax('/PurchaseEstimation/RemoveFromEstimation', { branchId: branchId, productId: productId }, function (response)
    {
        HideLoading(function ()
        {
            $("#divPurchaseDetails").html(response);
        });
    });
}

function EditDetail(branchId, productId)
{
    ShowLoading('static');

    ExecuteAjax('/PurchaseEstimation/EditDetail', { branchId: branchId, productId: productId }, function (response)
    {
        HideLoading(function ()
        {
            ShowModal(response, 'static');
        });
    });
}

function LoadEstimationDetails(hasItems)
{
    $("#PuschaseType").change(function ()
    {
        var value = this.value;

        if ($("#PuschaseType").val() != creditType)
        {
            $("#DaysToPay").attr("disabled", true);
            $("#DaysToPay").val(0);
        }
        else
            $("#DaysToPay").removeAttr("disabled");
    });

    if (hasItems)
    {
        $("#btnGenerate").removeAttr("disabled");
        $("#btnCancelAll").removeAttr("disabled");
        $("#btnQuickSearchProvider").attr("disabled", true);
    }
    else
    {
        $("#btnGenerate").attr("disabled", true);
        $("#btnCancelAll").attr("disabled", true);
        $("#btnQuickSearchProvider").removeAttr("disabled");
    }

    if ($("#ProviderId").val() == '' || $("#ProviderId").val() == 0)
        $("#btnBeginAddProduct").attr("disabled", true);

    else
        $("#btnBeginAddProduct").removeAttr("disabled");

    $('[data-toggle="tooltip"]').tooltip();
}


/***************** Product Search *******************************/


function LoadSearchProductList()
{
    Paginate("#tbProducts", 10, true, "#FProductName");
    $('[data-toggle="tooltip"]').tooltip();
}

function SearchProduct()
{
    ShowModLoading();

    var filter = { filter: $("#FProductName").val(), providerId: $("#ProviderId").val(), branchId: $("#BranchId").val() }

    ExecuteAjax('/PurchaseEstimation/SearchProducts', filter, function (response)
    {
        HideModLoading(function () {
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

function BeginProviderCode(productId)
{
    ShowProviderCode(productId, SearchProduct);
}

function AddProduct(button)
{
    var row = $(button).parent().parent();

    var model = {
        ProductId: row.find("#sItem_ProductId").val(),
        Price: parseFloat(row.find("#sItem_BuyPrice").val()),
        Quantity: parseFloat(row.find("#sItem_AddQuantity").val()),
        Discount: parseFloat(row.find("#sItem_Discount").val()),
        BranchId: $("#BranchId").val(),
        ProviderId: $("#ProviderId").val(),
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

    ShowModLoading();

    ExecuteAjax('/PurchaseEstimation/AddToEstimation', { item: model }, function (response) {
        HideModLoading(function ()
        {
            $("#divPurchaseDetails").html(response);
        });
    });
}

/******************************* Required Product Search List ******************************/

function ProductSelected(btn)
{
    var row = $(btn).parent().parent()[0];
    var txt = $(row).find("#item_AddQuantity");

    var toAdd = $(txt).val();
    var productId = $(row).find("#item_ProductId").val();

    var branchId = $(row).find("#item_BranchId").val();
    var code = $(row).find("#item_ProviderCode").val();
    var price = parseFloat($(row).find("#item_BuyPrice").val());

    if (code == 'No asignado')
    {
        ShowNotify("Sin código de proveedor", "warning", "Este producto no tiene relación con el proveedor seleccionado", 3000);
        return;
    }


    var param =
       {
           BranchId: branchId,
           ProviderId: $("#ProviderId").val(),
           ProductId: productId,
           Quantity: toAdd,
           Price: price
       };

    ShowModLoading();

    ExecuteAjax('/PurchaseEstimation/AddToEstimation', { item: param }, function (response)
    {
        HideModLoading(function ()
        {
            $(btn).attr('disabled', true);
            $(txt).attr('disabled', true);
            $(row).addClass('bgDataTable-info');
            ShowNotify("Partida agregada", "info", "Partida agregada a la estimación de compra", 3000);
            $("#divPurchaseDetails").html(response);
        });
    });
}

//evalua la cantidad de produto ingresado (no puede superar al maximo configurado)
function CheckQuantity(input)
{
    var row = $(input).parent().parent()[0];

    var stock = parseFloat($(row).find("#item_Quantity").val());
    var max = parseFloat($(row).find("#item_MaxQuantity").val());

    var quantity = $(input).val();

    if (isNaN(quantity) || quantity == '')
        $(input).val(max - stock);

    quantity = parseFloat(quantity);

    var total = stock + quantity;

    if (total > max || quantity <= 0)
        $(input).val(max - stock);
}

