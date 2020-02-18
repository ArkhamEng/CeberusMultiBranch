$(document).ready(function () {
   
    $("#btnCreate").off('click').on('click', function ()
    {
        ShowLoading('static');
        window.location = '/Offers/Editor/';
    });

    $("#btnSearch").click(function ()
    {
        ShowLoading('static');
        var data = { description: $("#txtName").val() };

        GetAjax('/Offers/Search/', data, function (view)
        {
            HideLoading(function () {
                $("#divOffers").html(view);
                SetPagination();
            });
           
        });
    });

    SetPagination();
});


function Edit(id)
{

    ShowLoading('static');
    window.location = '/Offers/Editor/' + id;
}

function Delete(id)
{

    ShowConfirm('Eliminar promoción!', "Al eliminar una promoción la imagen realcionada sera borrada, ¿estas seguro de continuar?",
        function ()
        {
            ShowLoading('static');
            ExecuteAjax('/Offers/Delete/', { id: id }, function (response) {
                HideLoading(function ()
                {
                    $("#divOffers").html(response);
                    SetPagination();
                });

            });
        }, null);
}

function SetPagination()
{
    Paginate("#tbOffers", 10, true, "#txtName", false, "#tbButtons", null);

}
