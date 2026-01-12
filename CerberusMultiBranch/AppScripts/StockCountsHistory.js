


$(document).ready(function () {


    $("#btnClear").click(function () {
        location.reload();
    });


    $("#btnSearch").click(function () {
        Search();
    });


});

function Search() {

    console.log($("#BeginDateTime").val());

    console.log($("#EndnDateTime").val());
    
    if (daysBetween($("#BeginDateTime").val(), $("#EndDateTime").val()) > 30) {
        ShowNotify("Rango de fechas", "warning", "La búsqueda supera el rango de fechas (Un mes)", 3000);
        return;
    }
    ShowLoading();

   var param = { branchId: $("#BranchId").val(), beginDate: $("#BeginDateTime").val(), endDate: $("#EndDateTime").val() }
       
    ExecuteAjax('/StockCountsHistory/Search', param, function (view) {
        $("#divStockCountsList").html(view);
         $("#DivGridHistory").removeClass('hidden');
        HideLoading();
    });

   
}


function daysBetween(sDate1, sDate2) {
    //Date.parse () analiza una cadena de fecha y hora y devuelve el número de milisegundos desde la medianoche del 1 de enero de 1970 hasta la fecha y la hora
    var time1 = Date.parse(new Date(sDate1));
    var time2 = Date.parse(new Date(sDate2));
    var nDays = Math.abs(parseInt((time2 - time1) / 1000 / 3600 / 24));
    return nDays;
};





