$(document).ready(function ()
{

    $('#sidebarCollapse').on('click', function ()
    {
        $('#sidebar').toggleClass('active');
    });

});

function BeginProgress()
{
    $("#ModalProgress").modal();
    $("#ProgressValue").width("33%");
}

function HalfProgress()
{
    $("#ModalProgress").modal();
    $("#ProgressValue").width("66%");
}

function CompleateProgress()
{
    $("#ProgressValue").width("100%");
    $("#ModalProgress").modal('hide');
    $("#ProgressValue").width("0%");
}



function SetPointer(element)
{
    $(element).css('cursor', 'pointer');
}

function SetCascade(ddlParent, ddlChild, action) {
    console.log("Setting Cascade Controls State/Cities");
    console.log("Child Value " + $(ddlChild).val());

    if ($(ddlChild).val() == null)
        $(ddlChild).attr("readonly", false);
    else
        $(ddlChild).attr("readonly", true);

    $(ddlParent).change(function () {
        console.log("Populating Cascade");
        if ($(ddlParent).val() != '') {
            var parentId = $(ddlParent).val();
            console.log("Searching for " + parentId);

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
            console.log("executing ajax");
            $.ajax(AObj);
        }
        else {
            $(ddlChild).empty();
            $(ddlChild).attr("readonly", true);
        }


    });
}

function Search(url, data, target) {
    console.log("Executing Ajax..");

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
            200: function (view){
                console.log("200: Authenticated ");
                $(target).html(view);
            },
            401: function (data) {
                alert('401: Unauthenticated');
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
               "infoFiltered": "(filtered from _MAX_ total records)",
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
    $(filters).on("hide.bs.collapse", function () {
        $(button).attr("class", "btn btn-warning");
        $(button).html('<span class="glyphicon glyphicon-eye-open"></span>');
        $(target).attr("class", "col-md-12");
    });
    $(filters).on("show.bs.collapse", function () {
        $(button).attr("class", "btn btn-default");
        $(button).html('<span class="glyphicon glyphicon-eye-close"></span>');
        $(target).attr("class", "col-md-8");
    });
}

