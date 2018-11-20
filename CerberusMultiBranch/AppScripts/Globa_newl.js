$(document).ready(function ()
{
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
});

function SetPointer(element)
{
    $(element).css('cursor', 'pointer');
}


function Compleate(textbox, list, url, onSelected)
{
    textbox.autocomplete(
      {
          source: function (request, response) {
              ExecuteAjax(url, { filter: request.term }, function (json)
              {
                  list.empty();
                  for (var i = 0; i < json.length; i++) {
                      list.append($('<option data-id=' + json[i].Id + '></option>').val(json[i].Label).html(json[i].Value));
                  }
              });
          },
          minLength: 4
      });
    
    //this is executed when an option from DataList is selected
    textbox.off('input').bind('input', function () {
        var val = this.value;

        if (list.find('option').filter(function () {
            return this.value.toUpperCase() === val.toUpperCase();
        }).length) {
            var option = list.find('option').filter(function () {
                return this.value.toUpperCase() === val.toUpperCase();
            });

            var value = option.text();
            var id = option.data("id");

            if (onSelected != null)
            {
                onSelected(id, value);
            }
        }
    });
}

//AJAX CALL
function ExecuteAjax(url, parameters, callback)
{
    $.ajax({
        url: url,
        type: "POST",
        data: parameters,
        success: function (response) {
            if ($.isPlainObject(response) && typeof (response.Code) != "undefined" && response.Code != 200)
            {
                ShowNotify(response.Header, response.Result, response.Body, 3500);

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
        error: function () {
            HideLoading();
            ShowNotify("Error Inesperado!", "dark", "ocurrio un error, quiza has perdido la conexion a internet!", 3500);
        }
    });
}

//Realiza paginación  sobre una tabla
function Paginate(table, iniRecords, responsive, filter, scrollX, buttonContainer)
{
    var searching = false;

    if (typeof (filter) != 'undefined')
        searching = true;

    var oTable = $(table).DataTable(
       {
           //destroy: true,
           keys: true,
           scrollX: scrollX,
           scrollCollapse: true,
           fixedHeader: true,
           responsive: responsive,
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
    if (typeof (filter) != 'undefined')
    {
        $(filter).keyup(function () {
            oTable.data().search(this.value).draw();
        });

        $(table + "_filter").addClass("hidden");
    }

    if (typeof (buttonContainer) != 'undefined') {

        $(buttonContainer).html("");

        new $.fn.dataTable.Buttons(oTable, {
            buttons: [
              
                {
                    extend: 'excelHtml5',
                    text: '<i class="fa fa-file-excel-o"></i> Excel',
                    titleAttr: 'Excel',
                    className: "btn btn-default"
                },
                  {
                      extend: 'copyHtml5',
                      text: '<i class="fa fa-files-o"></i> Copiar',
                      titleAttr: 'Copy',
                      className: "btn btn-default"
                  },
                 {
                     extend: 'pdfHtml5',
                     text: '<i class="fa fa-file-pdf-o"></i> PDF',
                     titleAttr: 'PDF',
                     className: "btn btn-default"
                 }
            ]
        }).container().appendTo($(buttonContainer));
    }
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



//CONFIRM CONTROL FUNCTIONS
function ShowMessage(textHeader, textBody, type, confirmCallBack, cancelCallBack, backdrop) {
    //header and body text
    $("#MessageHeader").text(textHeader);
    $("#MessageBody").text(textBody);

    $("#MessageGroup").children().hide();
    $("#MessageOk").show();

    //setting Image and header color
    if (type == 'success') {
        $("#MessageContent").attr("class", 'modal-content panel panel-success');
        $("#MessageImage").attr("src", '/Images/success.png');
        $("#MessageOk").attr('class', 'btn btn-success');
    }
    else if (type == 'warning') {
        $("#MessageContent").attr("class", 'modal-content panel panel-warning');
        $("#MessageImage").attr("src", '/Images/warning.png');
        $("#MessageOk").attr('class', 'btn btn-warning');
    }
    else if (type == 'confirm') {
        $("#MessageContent").attr("class", 'modal-content panel panel-info');
        $("#MessageImage").attr("src", '/Images/question.png');
        $("#MessageGroup").children().show();
        $("#MessageOk").hide();
    }


    //binding button acctions
    $("#MessageConfirm").unbind('click').click(function (e) {
        HideMessage(true, function () {
            ShowLoading('static');

            if (confirmCallBack != null)
                confirmCallBack();
        });
    });

    $("#MessageCancel").unbind('click').click(function (e) {
        HideMessage(true, cancelCallBack);
    });

    $("#MessageOk").unbind('click').click(function (e) {

        HideMessage(true, cancelCallBack);
    });

    //rise modal
    $("#ModalMessage").modal({ backdrop: backdrop });
}

function HideMessage(cleaUp, callback) {
    $('#ModalMessage').off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
        if (callback != null)
            callback();

        if (cleaUp) {
            $("#MessageHeader").text('');

            $("#MessageBody").html('');
            $("#MessageContent").attr("class", 'modal-content');
            $("#MessageImage").attr("src", '');
        }
    });
    $("#ModalMessage").modal('hide');
}

function ShowNotify(title, type, message, delay)
{
    var dly = 2500;

    if (isNaN("") || typeof (delay) != 'undefined')
        dly = delay;

    if (typeof (PNotify) === 'undefined')
    {
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



function GetCurrency(value)
{
    return "$" + value.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

//CONFIRM WINDOW FUNCTIONS
function ShowConfirm(header, text, backdrop, action) {
    $("#cnfHeader").text(header);
    $("#cnfText").text(text);
    $("#ModalConfirm").modal({ backdrop: backdrop });

    $("#btnConfirm").off('click').click(function () {
        $("#ModalConfirm").modal("hide");
        action();
    });
}



