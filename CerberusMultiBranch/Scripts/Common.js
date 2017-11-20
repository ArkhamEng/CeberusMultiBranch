$(document).ready(function ()
{
    $('#sidebarCollapse').on('click', function ()
    {
        $('#sidebar').toggleClass('active');
    });
});



function GetCurrency(value)
{
    return "$" + value.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

function ShowMessage(title, text, cls)
{
   
    if (cls == 'success')
        $("#MessageContent").attr("class", "modal-content panel-success");

    $("#MessageTitle").html(title);
    $("#MessageText").html(text);
    $("#MessageImage").attr("src", "/Content/Images/" + cls + ".png");
    $("#MessageBox").modal();
}

function ShowLoading()
{
    $("#ModalProgress").modal();
}


function HideLoading()
{
    $("#ModalProgress").modal('hide');
}


function SetPointer(element)
{
    $(element).css('cursor', 'pointer');
}


function ExecuteAjax(url, parameters, callback) {
    console.log("Executing Ajax..");

    $.ajax({
        url: url,
        type: "POST",
        data: parameters,
        error: function (data) {
            callback("Error al ejecutar Ajax")
        },
        statusCode:
        {
            200: function (data) {
                callback(data);
            },
            401: function (data) {
                callback(data);
            }
        }
    });
}

function SetCascade(ddlParent, ddlChild, action)
{
   
    if ($(ddlChild).val() == null)
        $(ddlChild).attr("readonly", false);
    else
        $(ddlChild).attr("readonly", true);

    $(ddlParent).change(function () {
        console.log("Populating Cascade");
        if ($(ddlParent).val() != '') {
            var parentId = $(ddlParent).val();
        
            var AObj =
                {
                    url: "/Json/" + action,
                    type: "POST",
                    data: { parentId: parentId },

                    success: function (data) {
                        console.log("cascade data adquired");
                        $(ddlChild).empty();
                        $(ddlChild).append($('<option></option>').val("").html(""));
                        for (var i = 0; i < data.length; i++) {
                            $(ddlChild).append($('<option></option>').val(data[i].Value).html(data[i].Text));
                        }
                        if (data.length > 0)
                            $(ddlChild).attr("readonly", false);
                        else
                            $(ddlChild).attr("readonly", true);
                    },
                    error: function () {
                        console.log("Error on pupulating cascade");
                    }
                };
            $.ajax(AObj);
        }
        else {
            $(ddlChild).empty();
            $(ddlChild).attr("readonly", true);
        }
    });
}

function Search(url, data, target) {
 
    $.ajax({
        url: url,
        type: "POST",
        data: data,

        sucess: function (view) {
            console.log("Success");
            $(target).html(view);
        },

        error: function (data) { console.log("Error Executing"); console.log(data); },
        statusCode:
        {
            200: function (view) {
                console.log("200: Authenticated ");
                $(target).html(view);
            },
            401: function (data) {
                alert('401: Unauthenticated');
            }
        }
    });
}

function Paginate(table, records) {
    var oTable = $(table).DataTable(
       {
           destroy: true,
           "lengthChange": false,
           "searching": true,
           "lengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
           "pageLength": records,
           "language": {
               "search": "filtrar resultados",
               "lengthMenu": "mostrar  _MENU_ ",
               "zeroRecords": "no hay datos disponibles",
               "info": "página _PAGE_ de _PAGES_",
               "infoEmpty": "",
               "infoFiltered": "(filtrado de _MAX_ total registros)",
               "paginate": {
                   "previous": "Anterior",
                   "next": "Siguiente"
               }
           }
       });
}

function SetDataTable(table, filter) {
    var oTable = $(table).DataTable(
       {
           destroy: true,
           "lengthChange": false,
           "searching": true,
           "lengthMenu": [[3, 5, 10, -1], [3, 5, 10, "All"]],
           "pageLength": 10,
           "language": {
               "search": "filtrar resultados",
               "lengthMenu": "mostrar  _MENU_ ",
               "zeroRecords": "no hay datos disponibles",
               "info": "página _PAGE_ de _PAGES_",
               "infoEmpty": "",
               "infoFiltered": "(filtrado de _MAX_ total registros)",
               "paginate": {
                   "previous": "Anterior",
                   "next": "Siguiente"
               }
           }
       });


    $(filter).keyup(function () {
        oTable.data().search(this.value).draw();
    });
}

function SetFilterControls(filters, button, target) {
    $(filters).on("hide.bs.collapse", function ()
    {
        $(button).attr("class", "btn btn-warning");
        $(button).html('<span class="glyphicon glyphicon-eye-open"></span>');
        $(button).prop("title", "Mostrar filtros");

        
        $(target).attr("class", "col-md-12");
    });

    $(filters).on("show.bs.collapse", function ()
    {
        $(button).attr("class", "btn btn-default");
        $(button).html('<span class="glyphicon glyphicon-eye-close"></span>');
        $(button).prop("title", "Ocultar filtros"); 
        
        $(target).attr("class", "col-md-8");
    });
}

