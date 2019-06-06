﻿var Details = [];

var totalAmount = 0;
var totalItems  = 0;

var popOverTitle = '<button id="btnClosePop" type="button"  class="close"><i class="fa fa-times"></i> </button>' +
                   '<i class="fa fa-user"></i> Detalle del cliente'

const PresaleDays    = 30;
const ReservationDays = 30;

$(document).ready(function () {
    //sumo totales y cargo el detalle inicial en memoria
    SumAmount(true);

    //se puede modificar el cliente si el status actual  InProcess
    if ($("#Status").val() == TranStatus.InProcess.Name)
    {
        $("#btnBegingSearchCustomer").off("click").on("click", BegingSearchCustomer);

        //al dar enter en el filtro se dispára la búsqueda
        $("#Client_Name").off("keyup").on("keyup", function (e) { if (e.keyCode == 13) $("#btnBegingSearchCustomer").click(); });

        $("#TransactionType").off("change").on("change", SetExpirationDate);
    }
    else //si la venta despachada, no se puede cambiar el cliente ni el tipo de venta o despacho
    {
        $("#btnBegingSearchCustomer").attr("disabled", true);

        $("#Expiration").attr("readonly",      true);
        $("#SendingType").attr("disabled",     true);
        $("#TransactionType").attr("disabled", true);
        $(".txtQuantity").attr("disabled",     true);

        //si se encuentra en modo cambio, se puede registrar devoliciones
        if ($("#Status").val() == TranStatus.OnChange.Name)
            $(".btnDelete").attr("disabled", true);
        else
            $(".btnDelete").hide();
    }

    //se puede agregar partidas si la venta sigue en proceso o fue enviada a modificación
    if ($("#Status").val() == TranStatus.InProcess.Name || $("#Status").val() == TranStatus.OnChange.Name)
    {
        $("#btnBegingSearchProduct").off("click").on("click", BeginSearchProduct);
        $("#ProductFilter").off("keyup").on("keyup", function (e) { if (e.keyCode == 13) $("#btnBegingSearchProduct").click(); });
    }
    else //es imposile modificar las partidas en cualquier otro status
        $("#AddProductZone").hide();

    //siempre se tiene un cliente por lo que es necesario removerlo con el proceso de asignación
    $("#Client_Name").attr("readonly", true);

    //colo los popover y tooltips
    SetPopOver();
});

//Cambia la fecha de expiración en función del tipo de venta y tipo de cliente
function SetExpirationDate(e)
{
    var creditDays = parseInt($("#Client_CreditDays").val());

    var exp = new Date($("#TransactionDate").val());

    //si la venta es a crédito
    if ($("#TransactionType").val() == TranType.Credit.Value)
        exp.setDate(exp.getDate() + parseInt(creditDays));

    //si es preventa
    if ($("#TransactionType").val() == TranType.Presale.Value)
        exp.setDate(exp.getDate() + parseInt(PresaleDays));

    //si es preventa
    if ($("#TransactionType").val() == TranType.Reservation.Value)
        exp.setDate(exp.getDate() + parseInt(ReservationDays));


    $("#Expiration").val(exp.toISOString().split('T')[0]);
}

//comienza búsqueda de cliente
function BegingSearchCustomer(e)
{
    var clientId = parseInt($("#ClientId").val());

    if (isNaN(clientId))
        SearchCustomer("#Client_Name", SetCustomer);
    else {
        $("#iSearch").removeClass("fa-refresh").addClass("fa-search");

        $("#Client_Name").attr("readonly", false);

        $("#Client_Name").val("");
        $("#ClientId").val("");

        $("#Expiration").val("");

        $("#Client_ClientType").val("");
        $("#Client_CreditLimit").val("");
        $("#Client_CreditAvailable").val("");
        $("#Client_CreditDays").val("");
    }
}

//se ejecuta al encontrar una coincidencia de cliente o bien al seleccionar uno del listado
function SetCustomer(customer)
{
    $("#ClientId").val(customer.ClientId);
    $("#Client_Name").val(customer.Name);

    $("#Client_ClientType").val(customer.Type);
    $("#Client_CreditAvailable").val(customer.CreditAvailable);
    $("#Client_CreditDays").val(customer.CreditDays);
    $("#Client_CreditLimit").val(customer.CreditLimit);

    SetExpirationDate();

    $("#iSearch").removeClass("fa-search").addClass("fa-refresh");

    $("#Client_Name").attr("readonly", true);
}


//comienza búsqueda de producto
function BeginSearchProduct(e)
{
    SearchProduct("#ProductFilter", SetProduct, function ()
    {
        $("#tbSearchProductResults tbody tr").each(function (index, row)
        {
            var pId = $(row).find("#product_ProductId").val();

            $(Details).each(function (index, detail)
            {
                if (parseInt(pId) == detail.ProductId)
                {
                    $(row).addClass("alert alert-info");
                    $(row).find("#btnSelectProduct").attr("disabled", true);
                }
            });
        });
    });
}

//se ejecuta al encontrar una coincidencia de producto o bien al seleccionar uno del listado
function SetProduct(product)
{
    //este código se ejecuta al agregar un producto
    for (var i = 0; i < Details.length; i++)
    {
        if (Details[i].ProductId == parseInt(product.ProductId))
        {
            ShowNotify("Producto repetido", "info", "Este producto ya se encuentra en el detalle de venta", 3000);
            return;
        }
    }

    var row = $("#rowTemplate").clone();

    row.removeAttr("id");

    if (product.Stock <= 0)
    {
        ShowNotify("Producto Agotado", "danger",
            "Este producto se encuentra agotado, solo podra ser agregado a una prevena o cotización", 4000);

        row.attr("style", "background-color:lightgray");

        if ($("#Status").val() != TranStatus.InProcess.Name)
            return;
    }

    try {

        var saleId = 0;
        var price = parseFloat($("#Client_ClientType").val() == ClientType.Store ? product.StorePrice :
                    $("#Client_ClientType").val() == ClientType.Dealer ? product.DealerPrice : product.WholesalerPrice);

        row.find("#item_ProductId").val(product.ProductId);
        row.find("#item_SaleId").val(saleId);
        row.find("#tdCode").text(product.Code);
        row.find("#tdName").text(product.Name);

        row.find("#imgProduct").attr("src", product.Image);

        row.find("#item_Quantity").val(product.SellQty);

        row.find("#item_TaxedPrice").val(price);

        var amount = GetCurrency(price * parseFloat(product.SellQty));

        row.find("#tdRowAmount").text(amount);

        $("#tbSaleDetails tbody").append(row);

        SumAmount();

    } catch (e) {
        console.log(e);
    }
}


//suma totales y carga en memoria las partidas de la venta
function SumAmount(isFirstLoad)
{
    totalAmount = 0;
    totalItems = 0;

    $("#tbSaleDetails tbody tr").each(function (index, row)
    {
        var quantityTxt = $(row).find("#item_Quantity");
        var priceTxt = $(row).find("#item_TaxedPrice");
        var refundTxt = $(row).find("#item_Refund");
        var productIdTxt = $(row).find("#item_ProductId");
        var saleIdTxt = $(row).find("#item_SaleId");
        var amountCell = $(row).find("#tdRowAmount");

        var deleteDetailBtn = $(row).find("#btnDeleteDetail");


        $(quantityTxt).off("blur").on("blur", function (e) { SetQuantity(this); });

        $(priceTxt).off("blur").on("blur", function (e) { SetPrice(this); });

        $(deleteDetailBtn).off('click').on('click', function (e) { DeleteRow(this); });

        $(refundTxt).off("blur").on("blur", function (e) { SetRefund(this); });

        var detail = {
            ProductId: parseInt(productIdTxt.val()),
            Amount: CurrencyToNumber(amountCell.text()),
            Price: CurrencyToNumber(priceTxt.val()),
            Quantity: parseFloat(quantityTxt.val()),
            SaleId: parseInt(saleIdTxt.val()),
            Refund: parseFloat(refundTxt.val()),
            IsModified: false
        }

        var exist = false;

        $(Details).each(function (index, product)
        {
            if (product.ProductId == detail.ProductId)
                exist = true;
        })

        if (!exist)
            Details.push(detail);

        //si es la primera carga de una venta en cambio y no hay devoluciones en la partida, aun se puede recibir
        if (isFirstLoad && $("#Status").val() == TranStatus.OnChange.Name && detail.Refund == 0)
        {
            console.log($("#Status").val());
            console.log(detail.Refund);
            $(refundTxt).attr("disabled", false);
        }

        totalAmount += detail.Amount;
        totalItems  += sale.Quantity - sale.Refund;
    });

    $("#tdAmount").text(GetCurrency(totalAmount));
}

function SetQuantity(input)
{
    row = $(input).parent().parent();

    var quantity = parseFloat($(input).val());

    if (isNaN(quantity) || quantity <= 0)
    {
        ShowNotify("Cantidad Inválida", "danger", "La cantidad debe ser un número positivo", 3000);
        $(input).off("blur");
        $(input).focus();
        $(input).on("blur", function () { SetQuantity(this); });

        return;
    }

    pId = row.find("#item_ProductId").val();

    ShowLoading("static");

    GetAjax('/Selling/GetProductsInfo', { "productIds[0]": pId }, function (response)
    {
        HideLoading(function ()
        {
            var idx = $("#tbSaleDetails tbody tr").index(row);

            if (quantity > parseFloat(response.JProperty[0].Stock))
            {
                ShowNotify("Cantidad Incorrecta", "danger",
                    "la cantidad requerida, supera la disponible en inventario! Cantidad actual: "
                    + GetCurrency(response.JProperty[0].Stock), 4000);

                //coloco la cantidad que hay en memoria
                $(input).val(Details[idx].Quantity);
                $(input).off("blur");
                $(input).focus();
                $(input).on("blur", function () { SetQuantity(this); });

                return;
            }

            var textPrice  = $(row).find("#item_TaxedPrice").val();
            var amountCell = $(row).find("#tdRowAmount");

            var newAmount = parseFloat(textPrice) * quantity;

            $(amountCell).html(GetCurrency(newAmount));

            Details[idx].Quantity = quantity;
            Details[idx].Amount = newAmount;
            Details[idx].IsModified = true;


            SumAmount(false);
        });
    });
}

function SetPrice(input)
{
    row = $(input).parent().parent();

    var idx = $("#tbSaleDetails tbody tr").index(row);

    var price = parseFloat($(input).val());

    if (isNaN(price) || price <= 0)
    {
        ShowNotify("Precio Incorrecto", "warning",
                   "El precio debe ser un número positivo " + GetCurrency(response.JProperty[0].StorePrice), 4000);

        $(input).val(Details[idx].Price);
        $(input).off("blur");
        $(input).focus();
        $(input).on("blur", function () { SetPrice(this); });
        return;
    }

    pId = row.find("#item_ProductId").val();

    //validación de precio
    ShowLoading('static');

    GetAjax('/Selling/GetProductsInfo', { "productIds[0]": pId }, function (response)
    {
        HideLoading(function ()
        {
            if (price > parseFloat(response.JProperty[0].StorePrice))
            {
                ShowNotify("Precio Incorrecto", "warning",
                    "El precio no puede ser mayor a " + GetCurrency(response.JProperty[0].StorePrice), 4000);

                //coloco el precio que tengo en memoria
                $(input).val(Details[idx].Price);
                $(input).off("blur");
                $(input).focus();
                $(input).on("blur", function () { SetPrice(this); });

                return;
            }

            if (price < parseFloat(response.JProperty[0].WholesalerPrice))
            {
                ShowNotify("Precio Incorrecto", "warning",
                    "El precio no puede ser menor a " + GetCurrency(response.JProperty[0].WholesalerPrice), 4000);

                $(input).val(Details[idx].Price);

                $(input).off("blur");
                $(input).focus();
                $(input).on("blur", function () { SetPrice(this); });

                return;
            }

            var textQuantity = $(row).find("#item_Quantity").val();
            var amountCell = $(row).find("#tdRowAmount");

            var newAmount = parseFloat(textQuantity) * price;

            $(amountCell).html(GetCurrency(newAmount));

            Details[idx].Price = price;
            Details[idx].Amount = newAmount;
            Details[idx].IsModified = true;

            SumAmount(false);
        });
    });
}


function SetRefund(input)
{
    row = $(input).parent().parent();

    var idx = $("#tbSaleDetails tbody tr").index(row);

    var refund = parseFloat($(input).val());

    if (isNaN(refund) || refund < 0)
    {
        ShowNotify("Devolución Inválida", "danger", "La devolución debe ser un número positivo", 3000);
        //coloco la cantidad que hay en memoria
        $(input).val(Details[idx].Refund);

        $(input).off("blur");
        $(input).focus();
        $(input).on("blur", function () { SetRefund(this); });

        return;
    }

    
    pId = row.find("#item_ProductId").val();


    var price = parseFloat($(row).find("#item_TaxedPrice").val());
    var quantity = $(row).find("#item_Quantity").val();
    var amountCell = $(row).find("#tdRowAmount");

    if (refund > quantity)
    {
        ShowNotify("Devolución Inválida", "danger", "La devolución no puede ser mayor a la cantidad de productos comprados", 3000);
        //coloco la cantidad que hay en memoria
        $(input).val(Details[idx].Refund);

        $(input).off("blur");
        $(input).focus();
        $(input).on("blur", function () { SetRefund(this); });

        return;
    }


    var newAmount = parseFloat(price) * (quantity - refund);

    $(amountCell).html(GetCurrency(newAmount));

    Details[idx].Refund = refund;
    Details[idx].Amount = newAmount;
    Details[idx].IsModified = true;

    SumAmount(false);
}

function DeleteRow(button)
{
    var row = $(button).parent().parent();

    var inx = $("#tbSaleDetails tbody tr").index(row);

    $(row).remove();

    Details.splice(inx, 1);

    SumAmount();
}



function SetPopOver() {
    $("#spanPopOver").popover({
        html: true,
        container: '.customerInfo',
        trigger: 'click',
        placement: 'bottom auto',
        content: $("#spanPopOver").attr("popover-content"),
        title: popOverTitle,
        template: PopOverTemplatePrimary
    }).off('shown.bs.popover').on('shown.bs.popover', function () {
        $("#btnClosePop").off("click").on("click", function () { $("#spanPopOver").click() });

        var cLimit = parseFloat($("#Client_CreditLimit").val());
        var cAvailable = parseFloat($("#Client_CreditAvailable").val());

        $("#divCreditLimit").text(GetCurrency(cLimit));
        $("#divCreditAvailable").text(GetCurrency(cAvailable));

        $("#divClientType").text($("#Client_ClientType").val());
        $("#divCreditDays").text($("#Client_CreditDays").val());
    });

    $('[data-toggle="tooltip"]').tooltip();

    $("#spanPopOver").tooltip({
        placement: 'bottom',
        title: $("#spanPopOver").attr("tooltip-title")
    })

    $("#btnBegingSearchCustomer").tooltip({
        placement: 'bottom',
        title: $("#btnBegingSearchCustomer").attr("tooltip-title")
    })


    $("#btnBegingSearchProduct").tooltip({
        placement: 'rigth',
        title: $("#btnBegingSearchProduct").attr("tooltip-title")
    })

}



function SendOrder()
{
    var sale =
        {
            SaleId: $("#SaleId").val(),
            ClientId: $("#ClientId").val(),
            UserId: $("#UserId").val(),
            BranchId: $("#BranchId").val(),
            Folio: $("#Folio").val(),
            TransactionDate: $("#TransactionDate").val(),
            Expiration: $("#TransactionDate").val(),
            TransactionType: $("#TransactionType").val(),
            SendingType: $("#SendingType").val(),
            LastStatus: $("#LastStatus").val(),
            Status: $("#Status").val(),
            SaleDetails: Details,
            TotalTaxedAmount: totalAmount
        }

    if (totalItems == 0)
    {
        ShowNotify("Venta sin artículos", "danger",
            "No puedes guardar datos de una venta sin articulos, si estas haciendo devolución de todos los articulos realiza una cancelación total", 4000);
        return;
    }

    if (sale.TransactionType != TranType.Cash.Value && sale.ClientId == 0)
    {
        ShowNotify("Se requiere un cliente", "warning",
            "Las ventas de tipo, crédito, preventa y apartado requiere un cliente asignado", 4000);
        return;
    }

    if (sale.ClientId == "")
    {
        ShowNotify("Se requiere un cliente", "warning",
            "Debes asignarnad un cliente a la venta", 4000);
        return;
    }

    ShowConfirm("Envío de Venta", "Estas a punto de envíar las modificaciones de la venta <br/> ¿Deseas continuar?",
        function () { CompleateSendOrder(sale); });
   
}

function CompleateSendOrder(sale)
{
    ShowLoading('static');

    ExecuteAjax('/Selling/SendSaleOrder', { sale: sale }, function (response)
    {
        HideLoading(function ()
        {
            console.log(window.location.pathname);
            console.log(window.location.pathname);

            ShowConfirm(response.Header, "<p>" + response.Body + "<br/> ¿Deseas agregar otra venta? <p/>",
                function () { ShowLoading('static'); window.location.replace('/Selling/SaleOrder'); },
                function () { ShowLoading('static'); window.location.replace("/Selling/SaleOrder/" + response.Id) });
        });
    });
}