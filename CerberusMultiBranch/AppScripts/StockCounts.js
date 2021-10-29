
const MinimumSearchLength = 3;

var rows = [];
let products = [];
let SystemsIndex = 0;
let BranchIndex = 0;
let beginDate = null;
let linesCounted = 0;
let correctLines = 0;
let linesAccurancy = 0.00;

$(document).ready(function () {

    
    GetSession();

    $("#btnBegingSearchProduct").off("click").on("click", BeginSearchProduct);

    $("#ProductFilter").off("keyup").on("keyup", function (e) { if (e.keyCode == 13) $("#btnBegingSearchProduct").click(); });

    $('#ProductId').keyup(function () {
        ValidateButtonAdd();
    });

    $('#Observations').keyup(function () {
        ValidateButtonAdd();
    });

    $("#Name").keyup(function () {
        ValidateButtonAdd();
    });

    SetProductSearch();

    $("#btnCancel").click(function () {
        CancelButton();
    });

    $("#btnAccept").click(function () {
        Accept();
    });

    TotalRows();

    

    beginDate = $("#BeginDate").text();

    

});

function TotalRows() {

    let rowCount = rows.length;
    if (rowCount > 0) {
        $("#Branches").prop('disabled', true);
        $("#btnAccept").prop('disabled', false);
        $("#Systems").prop('disabled', true);
        sessionStorage.setItem("BranchSelected", $("#Branches").prop('selectedIndex'));
        sessionStorage.setItem("SystemsSelected", $("#Systems").prop('selectedIndex'));

    }

    else {
        $("#Branches").prop('disabled', false);
        $("#btnAccept").prop('disabled', true);
        $("#Systems").prop('disabled', false);
        sessionStorage.setItem("BranchSelected", $("#Branches").prop('selectedIndex'));
        sessionStorage.setItem("SystemsSelected", $("#Systems").prop('selectedIndex'));
    }
    
}




function GetSession() {

    let dBranchSelected = sessionStorage.getItem("BranchSelected");
    if (dBranchSelected !== null) {
        $("#Branches").prop('selectedIndex', dBranchSelected);
    }
    let dSystemsSelected = sessionStorage.getItem("SystemsSelected");
    if (dSystemsSelected !== null) {
        $("#Systems").prop('selectedIndex', dSystemsSelected);
    }

    let dproducts = sessionStorage.getItem('products');
    if (dproducts !== null) {
        products = JSON.parse(dproducts);
    }

    let drows = sessionStorage.getItem('rows');
    if ( drows !== null) {
        rows = JSON.parse(drows);
        UpdateTable();
    }
}

function Accept() {
           
    if ($("#Name").val().length >= 5 && $("#Observations").val().length >= 20) {
        

        linesCounted = rows.length;
        correctLines = 0;
        rows.forEach(function (r) {
            if (r.VarianceQty === 0) {
                correctLines++;
            }
        });
        if (correctLines !== 0) {
            linesAccurancy = (correctLines * 100) / linesCounted;
        }
        else {
            linesAccurancy = 0;
        }
       
        
        var filter = {
            "IdBranch": $("#Branches option:selected").val(),
            "IdPartSystem": $("#Systems option:selected").val(),
            "Observations": $('#Observations').val(),
            "Name": $('#Name').val(),
            "Products": products,
            "StockCountDetails": rows,
            "TotalCost": $('#TVar').text(),
            "TotalCostVariance": $('#TCosVar').text(),
            "LinesCounted": linesCounted,
            "CorrectLines": correctLines,
            "LinesAccurancy": linesAccurancy.toFixed(2),
            "BeginDate": beginDate
        }

        $.ajax({
            url: "/StockCounts/CreateRegister",
            type: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(filter),
            success: function (result) {
                if (result) {
                    CancelButton();
                }
                else {
                    alert(result);
                }
                
            },
            error: function (result) {
                alert(result);
            }
        });
    }
    else {
        ShowNotify("Faltan datos", "warning", "Se deben llenar los campos Nombre con al menos 5 caracteres y Observación con al menos 20 caracteres", 3000);
    }



  
}

function ButtonAdd()
{
    var filter = {
        "IdBranch": $("#Branches option:selected").val(),
        "IdPartSystem": $("#Systems option:selected").val(),
        "Observations": $('#Observations').val(),
        "Name": $('#Name').val(),
        "Products": products,
        "StockCountDetails": rows
    }


    $.ajax({
        url: "/StockCounts/AddStockCount",
        type: "POST",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(filter),
        success: function (result) {
            FillTable(result);
        },
        error: function (result) {
            alert(result);
        }
    });
}

function FillTable(result) {
    
    rows.push(result);
    
    sessionStorage.setItem('rows', JSON.stringify(rows));
    //Calculate(result.ProductId, 0);
    $("#contenido").append("<tr class='QtyValid'><td style='display: none;'>"+ result.ProductId + "</td><td class='QtyValidValue'  style='width:200px;text-align:center;font-weight:bold;'>" + result.Product.Code + "</td><td  style='width:500px;text-align:left'>" + result.Product.Name + "</td><td style='width:200px;text-align:center'>" + result.UnitCost +  "</td><td style='width:200px;text-align:center'>" + result.CurrentQty
        + "</td> <td  style='width:200px;text-align:center'> <input type='number' onmouseup = 'Calculate(" + result.ProductId + ",value)' onkeyup = 'Calculate(" + result.ProductId + ",value)' class='Qty' placeholder='0' value =" + result.CountQty + "></td><td  style='width:200px;text-align:center;font-weight:bold'><font color='#DCDADA'>"
        + result.VarianceQty + "</font><td style='width:200px;text-align:center;font-weight:bold;'><font color='#DCDADA'>" + result.VarianceCost.toFixed(2) + "</font></td><td  style='width:50px;text-align:center'><button id='btnDeleteDetail' class='delete btn btn-sm btn-danger pull-right' type='button' title='Eliminar partida'><i class='fa fa-trash'></i ></button ></td></tr>");

    $('.delete').off().click(function (e) {
        $(this).parent('td').parent('tr').remove();
        DeleteRow(e);
    });

    $('.Qty').focusout(function () {
        UpdateTable();
    });

    TotalRows();
}


function UpdateTable() {
    $("#contenido").html('');
    let tVar = 0;
    let tCoVar = 0;
    rows.forEach(function (r) {

        let colorQuantity = Colors(r.VarianceQty);
        let colorVarCost = Colors(r.VarianceCost);

        $("#contenido").append("<tr class='QtyValid'><td style='display: none;'>" + r.ProductId + "</td><td class='QtyValidValue'  style='width:200px;text-align:center;font-weight:bold;'>" + r.Product.Code + "</td><td  style='width:500px;text-align:left'>" + r.Product.Name + "</td> <td style='width:200px;text-align:center'>" + r.UnitCost + "</td><td style='width:200px;text-align:center'>" + r.CurrentQty
            + "</td> <td  style='width:200px;text-align:center'> <input type='number' onmouseup = 'Calculate(" + r.ProductId + ",value)' onkeyup = 'Calculate(" + r.ProductId + ",value)' class='Qty' placeholder='0' value =" + r.CountQty + "></td><td  style='width:200px;text-align:center;font-weight:bold;' bgcolor='" + colorQuantity.backGroundColor + "'><font color='" + colorQuantity.fontColor + "' >"
            + r.VarianceQty + "</font></td><td style='width:200px;text-align:center;font-weight:bold;' bgcolor='" + colorVarCost.backGroundColor + "'><font color='" + colorVarCost.fontColor +"' > $" + r.VarianceCost + "</font></td><td  style='width:50px;text-align:center'><button id='btnDeleteDetail' class='delete btn btn-sm btn-danger pull-right' type='button' title='Eliminar partida'><i class='fa fa-trash'></i ></button ></td></tr>");
        tVar = tVar + r.VarianceQty;
        tCoVar = parseFloat(tCoVar) + parseFloat(r.VarianceCost);
    });

    $('.delete').off().click(function (e) {
        $(this).parent('td').parent('tr').remove();
        DeleteRow(e);
    });

    $('.Qty').focusout(function () {
        UpdateTable();
    });

    sessionStorage.setItem('rows', JSON.stringify(rows));


    let colors = Colors(tVar)

    document.getElementById("TVar").innerHTML = "Total: " + tVar;
    document.getElementById("TVar").style.color = colors.backGroundColor;
    //document.getElementById("TVar").style.background = colors.backGroundColor;

    colors = Colors(tCoVar);

    document.getElementById("TCosVar").innerHTML = "Total: $" + tCoVar.toFixed(2);
    document.getElementById("TCosVar").style.color = colors.backGroundColor;
    //document.getElementById("TCosVar").style.background = colors.backGroundColor;

    linesCounted = rows.length;
    correctLines = 0;
    rows.forEach(function (r) {
        if (r.VarianceQty === 0) {
            correctLines++;
        }
    });
    if (correctLines !== 0)
    {
        linesAccurancy = (correctLines * 100) / linesCounted;
    }
    else
    {
        linesAccurancy = 0;
    }


    document.getElementById("lineCounted").innerHTML = "Líneas contadas: " + linesCounted;
    document.getElementById("correctLines").innerHTML = "Líneas correctas: " + correctLines;
    document.getElementById("linesAccurancy").innerHTML = "% de líneas: " + linesAccurancy.toFixed(2);

}


function CancelButton()
{
    var rows = [];
    let products = [];

    var dproducts = sessionStorage.getItem('products');
    if (dproducts !== null) {
        sessionStorage.removeItem('products');
    }

    let drows = sessionStorage.getItem('rows');
    if (drows !== null) {
        sessionStorage.removeItem('rows');
    }

    let dBranchSelected = sessionStorage.getItem("BranchSelected");
    if (dBranchSelected !== null) {
        sessionStorage.removeItem('BranchSelected');
    }
    let dSystemsSelected = sessionStorage.getItem("SystemsSelected");
    if (dSystemsSelected !== null) {
        sessionStorage.removeItem('SystemsSelected');
    }


    location.reload();

}

function Colors(value)
{
    let fontColor;
    let backGroundColor;

    if (value > 0) {
        backGroundColor = "#1BBD1B"; //Verde
        fontColor = "#ffffff";
    }
    else if (value === 0) {
        backGroundColor = "";
        fontColor = "#000000";
    }
    else if (value < 0) {
        backGroundColor = "#d60000";  //Rojo
        fontColor = "#ffffff";    }
    else {
        backGroundColor = "";
        fontColor = "#000000";
    }

    return {
        backGroundColor,
        fontColor
    }
}


function Calculate(ProductId, value) {

    if (value !== '') {

        let valueInt = parseInt(value);
        rows.forEach(function (r) {
            if (r.ProductId === ProductId) {
                r.CountQty = valueInt;
                //Reglas de negocio
                if (r.CurrentQty < 0) {
                    let current = (-1) * r.CurrentQty;
                    
                    r.VarianceQty = current + r.CountQty;

                    console.log(`${r.VarianceQty} = ${ current } + ${ r.CountQty }`);
                }
                else if (r.CurrentQty === 0)
                {
                    r.VarianceQty = r.CurrentQty + r.CountQty;
                }
                else if (r.CurrentQty > 0) {

                    if (r.CurrentQty > r.CountQty) {
                        let current = r.CurrentQty - r.CountQty;
                        r.VarianceQty = current * (-1);
                    }
                    else if (r.CurrentQty === r.CountQty) {
                        r.VarianceQty = 0;
                    }
                    else if (r.CurrentQty < r.CountQty) {
                        let current = r.CurrentQty - r.CountQty;
                        r.VarianceQty = current * (-1);
                    }

                }

                r.VarianceCost = r.VarianceQty * r.UnitCost;
                r.VarianceCost = r.VarianceCost.toFixed(2);
                sessionStorage.setItem('rows', JSON.stringify(rows));
            }
        });

    }

}

function DeleteRow(button) {
    let row = $(button).parent().parent();

    let inx = $("#DivGrid tbody tr").index(row);

    $(row).remove();

    rows.splice(inx, 1);
    products.splice(inx, 1);
    sessionStorage.setItem('rows', JSON.stringify(rows));
    sessionStorage.setItem('products', JSON.stringify(products));

    UpdateTable();

    TotalRows();

}


//BEGIN Control para busquedas de producto

function SearchProduct(input, productSelected, productsShown) {
    if ($(input).val().length < MinimumSearchLength) {
        ShowNotify("Faltan datos", "warning", "La búsqueda requiere" + MinimumSearchLength + " caractéres por lo menos", 3000);
        return;
    }

    ShowLoading('static');

    GetAjax('/StockCounts/SearchProduct', { filter: $(input).val(), idBranch: $("#Branches option:selected").val(), idPartSystem: $("#Systems option:selected").val()  }, function (response) {
        HideLoading(function () {
            //si encontre solo una coincidencia, la pongo de inmediato
            if ($.isPlainObject(response))
                productSelected(response.JProperty);

            else {
                ShowModal(response, 'static', 'lg', null);

                OnProductsShown = productsShown;
                OnProductSelected = productSelected;

                $("#SearchProductFilter").val($(input).val());
            }
        });
    });
}

//comienza búsqueda de producto
function BeginSearchProduct(e) {
    SearchProduct("#ProductFilter", SetProduct, function () {
        $("#tbSearchProductResults tbody tr").each(function (index, row) {
            var pId = $(row).find("#product_ProductId").val();

            $(products).each(function (index, detail) {
                if (parseInt(pId) == detail.ProductId) {
                    $(row).addClass("alert alert-info");
                    $(row).find("#btnSelectProduct").attr("disabled", true);
                }
            });
        });
    });
}

//se ejecuta al encontrar una coincidencia de producto o bien al seleccionar uno del listado
function SetProduct(product) {
    //este código se ejecuta al agregar un producto
    for (let i = 0; i < products.length; i++) {
        if (products[i].ProductId == parseInt(product.ProductId)) {
            ShowNotify("Producto repetido", "info", "Este producto ya se encuentra en el detalle de venta", 3000);
            return;
        }
    }
   
    // Usar producto
    let exist = false;

    products.forEach(function (p) {
        if (product.ProductId == p.ProductId) {
            exist = true;
        }
    });


    if (!exist) {
        products.push(product);
        sessionStorage.setItem('products', JSON.stringify(products));
        ButtonAdd();
    }
    else {
        alert("Producto ya agregado!! " + product.Name);
    }

}

function SetProductSearch() {
    $("#btnSearchProduct").off("click").on("click", function (e) {
        ShowModLoading();

        GetAjax('/StockCounts/SearchProduct', { filter: $("#SearchProductFilter").val(), fromModal: true },
            function (response) {
                HideModLoading(function () {
                    if ($.isPlainObject(response)) {
                        OnProductSelected(response.JProperty);

                        HideModal(null, true);
                    }
                    else {
                        $("#divSearchProductResults").html(response);
                        LoadProductResults();
                    }
                });
            });
    });
}

//END Control para busquedas de producto