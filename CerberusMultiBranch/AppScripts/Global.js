

/***************************************************
************AUTOCOMPLEATE USING JQUERY UI***********
***************************************************/
function Compleate(textbox, list, url, onSelected) {
    $(textbox).off('autocomplete').autocomplete(
      {
          source: function (request, response) {
              ExecuteAjax(url, { filter: request.term }, function (json) {
                  $(list).empty();
                  for (var i = 0; i < json.length; i++) {
                      $(list).append($('<option data-id=' + json[i].Id + '></option>').val(json[i].Label).html(json[i].Value));
                  }
              });
          },
          minLength: 4
      });

    //this is executed when an option from DataList is selected
    $(textbox).off('input').bind('input', function () {
        var val = this.value;
        if ($(list).find('option').filter(function ()
        {
            return this.value.toUpperCase() === val.toUpperCase();
        }).length)
        {
            var option = $(list).find('option').filter(function ()
            {
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

function LoadPopOver(button,callback)
{
    $("#ConfirmPopYes").click(function ()
    {
        console.log("Yes Clicked");
        callback();
    });

    $("#ConfirmPopNo").click(function ()
    {
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

/**************************************************
************PAGINATION USING DATA TABLE************
***************************************************/

function Paginate(table, records)
{
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