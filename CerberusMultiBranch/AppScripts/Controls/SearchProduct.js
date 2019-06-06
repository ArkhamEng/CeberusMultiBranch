
var OnProductSelected = null;

var OnProductsShown = null;


$(document).ready(function ()
{
    SetProductSearch();

    LoadProductResults();

    $("#btnCloseSearchProduct").off("click").on("click", function (e) { HideModal(null, true); });
});


function LoadProductResults()
{
    SetSelectProduct();
    Paginate("#tbSearchProductResults", 8, true, "#SearchProductFilter", false);
    
    if (OnProductsShown != null)
        OnProductsShown();
}


function SetSelectProduct()
{
    $("#tbSearchProductResults tbody tr").each(function (index, row)
    {
        var btnSelect = $(row).find("#btnSelectProduct");

        btnSelect.off("click").on("click", function ()
        {
            var Product =
                {
                    ProductId: $(row).find("#product_ProductId").val(),
                    Code: $(row).find("#product_Code").val(),
                    Name: $(row).find("#product_Name").val(),
                    Stock: $(row).find("#product_Stock").val(),
                    Image: $(row).find("#product_Image").val(),
                    StorePrice: $(row).find("#product_StorePrice").val(),
                    DealerPrice: $(row).find("#product_DealerPrice").val(),
                    WholesalerPrice: $(row).find("#product_WholesalerPrice").val(),
                    OrderQty: $(row).find("#product_OrderQty").val(),
                    SellQty: $(row).find("#product_SellQty").val()
                }

            HideModal(function () { OnProductSelected(Product); }, true);
        });
    });
}

function SetProductSearch()
{
    $("#btnSearchProduct").off("click").on("click", function (e)
    {
        ShowModLoading();

        GetAjax('/Selling/SearchProduct', { filter: $("#SearchProductFilter").val(), fromModal: true },
            function (response)
            {
                HideModLoading(function ()
                {
                    if ($.isPlainObject(response))
                    {
                        OnProductSelected(response.JProperty);

                        HideModal(null, true);
                    }
                    else
                    {
                        $("#divSearchProductResults").html(response);
                        LoadProductResults();
                    }
                });
            });
    });
}

