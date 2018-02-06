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