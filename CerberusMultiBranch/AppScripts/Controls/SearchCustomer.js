
var OnCustumerSelected = null;


$(document).ready(function ()
{
    SetCustomerSearch();

    LoadCustomerResults();

    $("#btnCloseSearchCustomer").off("click").on("click", function (e) { HideModal(null, true); });
});


function LoadCustomerResults()
{
    SetSelectCustomer();
    Paginate("#tbSearchCustomerResults", 8, true, "#SearchCustomerFilter", false);
}


function SetSelectCustomer()
{
    $("#tbSearchCustomerResults tbody tr").each(function (index, row)
    {
        var btnSelect = $(row).find("#btnSelectCustomer");

        btnSelect.off("click").on("click", function ()
        {
            var customer =
                {
                    Name: $(row).find("#customer_Name").val(),
                    Type: $(row).find("#customer_Type").val(),
                    CreditAvailable: $(row).find("#customer_CreditAvailable").val(),
                    CreditDays: $(row).find("#customer_CreditDays").val()
                }

            HideModal(function () { OnCustumerSelected(customer); }, true);
        });
    });
}

function SetCustomerSearch()
{
    $("#btnSearchCustomer").off("click").on("click", function (e)
    {
        ShowModLoading();

        GetAjax('/Selling/SearchCustomer', { filter: $("#SearchCustomerFilter").val(), fromModal: true },
            function (response)
            {
                HideModLoading(function ()
                {
                    if ($.isPlainObject(response)) {
                        OnCustumerSelected(response.JProperty);

                        HideModal(null, true);
                    }
                    else
                    {
                        $("#divSearchCustomerResults").html(response);
                        LoadCustomerResults();
                    }
                });
            });
    });
}

