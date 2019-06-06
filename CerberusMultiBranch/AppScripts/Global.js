$(document).ready(function ()
{
    $('#sidebarCollapse').on('click', function ()
    {
        $('#sidebar').toggleClass('active');
    });
});

const TranStatus = {
    OnChange:  { Name: "OnChange",  Value: -3, Display:  "En Modificación" },
    PreCancel: { Name: "PreCancel", Value: -2, Display:  "En Cancelación" },
    Canceled:  { Name: "Canceled",  Value: -1, Display:  "Cancelado" },
    InProcess: { Name: "InProcess", Value:  0,  Display: "En Proceso" },
    Reserved:  { Name: "Reserved",  Value:  1,  Display: "Reservador" },
    Revision:  { Name: "Revision",  Value:  2,  Display: "En Seguimieto" },
    Compleated:{ Name: "Compleated", Value: 3,  Display: "Completado" }
}

const TranType =
 {
     Cash: { Name: "Cash", Value: 0, Display: "Efectivo" },
     Credit: { Name: "Credit", Value: 1, Display: "Crédito" },
     Presale: { Name: "Presale", Value: 2, Display: "Preventa" },
     Reservation: { Name: "Reservation", Value: 3, Display: "Apartado" }
}

const ClientType =
{
    Store: { Name: "Store", Value: 0, Display: "Mostrador" },
    Dealer: { Name: "Dealer", Value: 1, Display: "Distribuidor" },
    Wholesaler: { Name: "Wholesaler", Value: 2, Display: "Mayorista" }
}

const PopOverTemplatePrimary = '<div class="popover panel panel-primary fade" role="tooltip">' +
           '<div class="arrow"></div>' +
           '<div class="popover-title"></div>' +
           '<div class="panel-body popover-content"></div></div>'


/**
*convierte un string a fecha
*@param {string} value fecha en formado dd/MM/yyyy
*/
function StrgToDate(value) {

    var date = new Date(value.substring(6, 10), value.substring(3, 5), value.substring(0, 2));
    return date;
}

/**
*convierte un string a fecha
*@param {string} value fecha en formado dd/MM/yyyy
*/
function ToDate(value) {
    var date = new Date(value.substring(6, 10), value.substring(3, 5), value.substring(0, 2));
    return date;
}

function DateToString(date) {
    return date.getDay() + "/" + date.getMonth() + "/" + date.getYear();
}

//Aplica formato de moneda a un valor de punto flotante
function GetCurrency(value) {
    return "$" + value.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

//convierte un texto en formato moneda a un valor de punto flotante
function CurrencyToNumber(value) {
    return parseFloat(value.trim().replace("$", "").replace(",", ""));
}

function SetPointer(element) {
    $(element).css('cursor', 'pointer');
}


function ShowNotify(title, type, message, delay) {
    var dly = 4500;

    if (isNaN(delay) || typeof (delay) != 'undefined')
        dly = delay;

    if (typeof (PNotify) === 'undefined') {
        return;
    }

    if (type == 'danger')
        type = "error";

    new PNotify({
        title: title,
        type: type,
        text: message,
        styling: 'bootstrap3',
        delay: dly,
        hide: true,
    });
};




/***************************************************
************AUTOCOMPLEATE USING JQUERY UI***********
***************************************************/
function Compleate(textbox, list, url, onSelected, entityId) {
    $(textbox).off('autocomplete').autocomplete(
      {
          source: function (request, response) {
              if (entityId == null) {
                  ExecuteAjax(url, { filter: request.term }, function (json) {
                      $(list).empty();
                      for (var i = 0; i < json.length; i++) {
                          $(list).append($('<option data-id=' + json[i].Id + '></option>').val(json[i].Label).html(json[i].Value));
                      }
                  });
              }
              else {
                  ExecuteAjax(url, { filter: request.term, entityId: entityId }, function (json) {
                      $(list).empty();
                      for (var i = 0; i < json.length; i++) {
                          $(list).append($('<option data-id=' + json[i].Id + '></option>').val(json[i].Label).html(json[i].Value));
                      }
                  });
              }

          },
          minLength: 3
      });

    //this is executed when an option from DataList is selected
    $(textbox).off('input').bind('input', function () {
        var val = this.value;
        if ($(list).find('option').filter(function () {
            return this.value.toUpperCase() === val.toUpperCase();
        }).length) {
            var option = $(list).find('option').filter(function () {
                return this.value.toUpperCase() === val.toUpperCase();
            });


            var value = option.text();
            var id = option.data("id");

            if (onSelected != null) {
                onSelected(id, value);
            }
        }
    });
}

function LoadPopOver(button, callback) {
    $("#ConfirmPopYes").click(function () {
        console.log("Yes Clicked");
        callback();
    });

    $("#ConfirmPopNo").click(function () {
        console.log("No Clicked");
        $(button).popover('hide');
        $(button).data("bs.popover").inState.click = false;
    });

    $(button).popover({
        html: true,
        content: function () { return $("#ConfirmPopOver").html(); },
        container: 'body'
    });

}



//AJAX CALL
function ExecuteAjax(url, parameters, callback, errorCallBack) {
    $.ajax({
        url: url,
        type: "POST",
        data: parameters,
        success: function (response) {
            if ($.isPlainObject(response) && typeof (response.Code) != "undefined" && response.Code != 200) {
                HideLoading();
                HideModLoading();
                ShowNotify(response.Header, response.Result, response.Body, 3500);

                if (errorCallBack != null)
                    errorCallBack(response);

                switch (response.Code) {
                    case 401:
                        window.location = response.Extra;
                        break;
                }

            }
            else {
                callback(response);
            }
        },
        error: function (err) {
            HideLoading();
            HideModLoading();
            ShowNotify("Sin respuesta del servidor!", "danger", "El servidor no respondio en el tiempo esperado, revisa tu conexión a internet", 3500);
        }
    });
}

//sends a Get Ajax request
function GetAjax(url, parameters, callback, errorCallBack) {
    $.ajax({
        url: url,
        type: "GET",
        data: parameters,
        success: function (response) {
            if ($.isPlainObject(response) && typeof (response.Code) != "undefined" && response.Code != 200) {
                HideLoading();
                HideModLoading();
                ShowNotify(response.Header, response.Result, response.Body, 3500);

                if (errorCallBack != null)
                    errorCallBack(response);

                switch (response.Code) {
                    case 401:
                        window.location = response.Extra;
                        break;
                }

            }
            else {
                callback(response);
            }
        },
        error: function (err) {
            HideLoading();
            HideModLoading();
            ShowNotify("Sin respuesta del servidor!", "danger", "El servidor no respondio en el tiempo esperado, revisa tu conexión a internet", 3500);
        }
    });
}

function SubmitAjax(url, formData, callback, errorCallBack) {
    $.ajax({
        url: url,
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        cache: false,
        success: function (response) {
            if ($.isPlainObject(response) && typeof (response.Code) != "undefined" && response.Code != 200) {
                HideLoading();
                HideModLoading();
                ShowNotify(response.Header, response.Result, response.Body, 3500);

                switch (response.Code) {
                    case 401:
                        window.location = response.Extra;
                        break;
                }
            }
            else {
                HideLoading();
                HideModLoading();
                callback(response);
            }
        },
        error: function () {
            HideLoading();
            ShowNotify("Error Inesperado!", "danger", "ocurrio un error, quiza has perdido la conexion a internet!", 3500);

            if (errorCallBack != null)
                errorCallBack();
        }
    });
}


//DROP DOWN CASCADE
function SetCascade(ddlParent, ddlChild, url) {
    $(ddlParent).unbind('change').change(function (e) {
        if ($(ddlParent).val() != '') {
            var parentId = $(ddlParent).val();

            ExecuteAjax(url, { id: parentId }, function (data) {
                $(ddlChild).empty();
                $(ddlChild).append($('<option></option>').val("").html(""));

                for (var i = 0; i < data.length; i++) {
                    $(ddlChild).append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }
                if (data.length > 0)
                    $(ddlChild).attr("readonly", false);
                else
                    $(ddlChild).attr("readonly", true);
            });
        }
        else {
            $(ddlChild).empty();
            $(ddlChild).attr("readonly", true);
        }
    });
}


//Realiza paginación  sobre una tabla
function Paginate(table, iniRecords, responsive, filter, scrollX, buttonContainer, printOp, rows) {
    var searching = false;

    if (typeof (filter) != 'undefined')
        searching = true;

    var oTable = $(table).DataTable(
       {
           //destroy: true,
           //keys: true,
           //scrollX: scrollX,
           //scrollCollapse: true,
           fixedHeader: true,
           responsive: responsive,
           "aaSorting": [],
           "lengthChange": false,
           "searching": searching,
           "order": [],
           "lengthMenu": [[5, 10, 20, 50, 100, -1], [5, 10, 20, 50, 100, "All"]],
           "pageLength": iniRecords,
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
           },
       });
    if (typeof (filter) != 'undefined') {
        $(filter).keyup(function () {
            oTable.data().search(this.value).draw();
        });

        $(table + "_filter").addClass("hidden");
    }

    if (typeof (buttonContainer) != 'undefined') {

        $(buttonContainer).html("");

        var copyButton = {
            extend: 'copyHtml5',
            text: '<i class="fa fa-files-o"></i> Copiar',
            titleAttr: 'Copy',
            className: "btn btn-default"
        };

        var printButton = "";

        if (printOp != null && printOp != 'undefined') {

            var printButton = {
                extend: 'print',
                text: '<i class="fa fa-print"></i> Imprimir',
                titleAttr: 'print',
                className: "btn btn-default",
                exportOptions: { columns: printOp.Columns },
                title: printOp.Title
            };
        }

        var excelButton =
        {
            extend: 'excelHtml5',
            text: '<i class="fa fa-file-excel-o"></i> Excel',
            titleAttr: 'Excel',
            className: "btn btn-default"
        };

        if (printButton != '') {
            new $.fn.dataTable.Buttons(oTable,
         { buttons: [copyButton, printButton, excelButton] }).container().appendTo($(buttonContainer));
        }
        else {
            new $.fn.dataTable.Buttons(oTable,
            { buttons: [copyButton, excelButton] }).container().appendTo($(buttonContainer));
        }
    }
}

//SHOWS MODAL WITH CUSTON FUNCTIONS AND CONTENT
function ShowModal(html, backdrop, size, closeCallback)
{
    $("#ModalDialog").removeClass('modal-sm');
    $("#ModalDialog").removeClass('modal-lg');
    $("#ModalDialog").removeClass('modal-ul');

    if (size == 'lg')
        $("#ModalDialog").addClass('modal-lg');

    if (size == 'ul')
        $("#ModalDialog").addClass('modal-ul');

    if (size == 'sm')
        $("#ModalDialog").addClass('modal-sm');

    $("#ModalLoading").children().hide();

    $("#ModalContent").html(html);

    $("#SiteModal").modal({ backdrop: backdrop });

    //si la modal se sin invocar el método hide
    $('#SiteModal').off('hidden.bs.modal').on('hidden.bs.modal', function (e)
    {
        if (closeCallback != null)
            closeCallback();

        $("#SiteModalContent").html('');
    });
}

//Hide Main Modal
function HideModal(callback, removeContent)
{
    $('#SiteModal').off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
        if (callback != null)
            callback();

        if (removeContent)
            $("#SiteModalContent").html('');
    });

    $("#SiteModal").modal('hide');

}

//Show Child Modal 
function ShowChildModal(content, openCallBack, size) {
    $("#ChildModalDialog").removeClass('modal-sm');
    $("#ChildModalDialog").removeClass('modal-lg');

    $("#ChildModal").off("shown.bs.modal").on('shown.bs.modal', function ()
    {
        $('#SiteModal').css('opacity', .7);
        $('#SiteModal').unbind();

        if (openCallBack != null && openCallBack != 'undefined')
            openCallBack();
    });

    if (size == 'lg')
        $("#ChildModalDialog").addClass('modal-lg');

    if (size == 'sm')
        $("#ChildModalDialog").addClass('modal-sm');

    $("#ChildModalContent").html(content);
    $("#ChildModal").css("margin-top", "100px");
    $("#ChildModal").modal({ backdrop: 'static' });
}

//Hide Child Modal 
function HideChildModal(closCallBack) {
    $('#ChildModal').off('hidden.bs.modal').on('hidden.bs.modal', function () {
        $('#SiteModal').css('opacity', 1);
        $('#SiteModal').removeData("modal").modal({});
        $('body').addClass("modal-open");
        $("#ChildModalContent").html("");

        if (closCallBack != null)
            closCallBack();
    });

    $('#ChildModal').modal("hide");
}


//LOADING CONTROL FUNCTIONS
function ShowLoading(backdrop) {
    $("#Loading").modal({ backdrop: backdrop });
}

function HideLoading(callback) {
    $("#Loading").off("hidden.bs.modal").on("hidden.bs.modal", function (e) {
        if (callback != null)
            callback();
    });

    $("#Loading").modal('hide');
}


//Loading In Modal
function ShowModLoading() {
    $("#ChildLoading").off("shown.bs.modal").on('shown.bs.modal', function () {
        $('#SiteModal').css('opacity', .7);
        $('#SiteModal').unbind();
    });


    $("#ChildLoading").css("margin-top", "100px");
    $("#ChildLoading").modal({ backdrop: 'static' });
}

function HideModLoading(callback) {
    $('#ChildLoading').off('hidden.bs.modal').on('hidden.bs.modal', function () {
        $('#SiteModal').css('opacity', 1);
        $('#SiteModal').removeData("modal").modal({});
        $('body').addClass("modal-open");

        if (callback != null)
            callback();
    });

    $('#ChildLoading').modal("hide");
}




//CONFIRM CONTROL FUNCTIONS
function ShowMessage(textHeader, textBody, type, confirmCallBack, cancelCallBack, backdrop)
{
    //header and body text
    $("#MessageHeader").text(textHeader);
    $("#MessageBody").text(textBody);

    $("#MessageGroup").children().hide();
    $("#MessageOk").show();

    //setting Image and header color
    if (type == 'success')
    {
        $("#MessageContent").attr("class", 'modal-content panel panel-success');
        $("#MessageImage").attr("src", '/Images/success.png');
        $("#MessageOk").attr('class', 'btn btn-success');
    }
    else if (type == 'warning')
    {
        $("#MessageContent").attr("class", 'modal-content panel panel-warning');
        $("#MessageImage").attr("src", '/Images/warning.png');
        $("#MessageOk").attr('class', 'btn btn-warning');
    }
    else if (type == 'confirm')
    {
        $("#MessageContent").attr("class", 'modal-content panel panel-info');
        $("#MessageImage").attr("src", '/Images/question.png');
        $("#MessageGroup").children().show();
        $("#MessageOk").hide();
    }


    //binding button acctions
    $("#MessageConfirm").off('click').on("click", function (e)
    {
        HideMessage(true, function ()
        {
            ShowLoading('static');

            if (confirmCallBack != null)
                confirmCallBack();
        });
    });

    $("#MessageCancel").unbind('click').click(function (e)
    {
        HideMessage(true, cancelCallBack);
    });

    $("#MessageOk").unbind('click').click(function (e) {

        HideMessage(true, cancelCallBack);
    });

    //rise modal
    $("#ModalMessage").modal({ backdrop: backdrop });
}




//Pagina agrupando filas
function GroupRow(table, iniRecords, filter, index, colspan) {
    var searching = false;

    if (typeof (filter) != 'undefined')
        searching = true;

    var oTable = $(table).DataTable(
       {
           destroy: true,
           fixedHeader: true,
           responsive: false,
           "aaSorting": [],
           "lengthChange": false,
           "searching": searching,
           "order": [],
           "lengthMenu": [[5, 10, 20, 50, 100, -1], [5, 10, 20, 50, 100, "All"]],
           "pageLength": iniRecords,
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
           },
           "columnDefs": [
               { "visible": false, "targets": index }
           ],

           "displayLength": 25,
           "drawCallback": function (settings) {
               var api = this.api();
               var rows = api.rows({ page: 'current' }).nodes();
               var last = null;

               apCd = api.column(index, { page: 'current' }).data();

               if (apCd != 'undefined') {
                   apCd.each(function (group, i) {
                       if (last !== group) {
                           $(rows).eq(i).before(
                               '<tr class="group bgDataTable-dark"><td colspan="' + colspan + '">' + group + '</td></tr>'
                           );

                           last = group;
                       }
                   });
               }
           }
       });
    if (typeof (filter) != 'undefined') {
        $(filter).keyup(function () {
            oTable.data().search(this.value).draw();
        });

        $(table + "_filter").addClass("hidden");
    }

}