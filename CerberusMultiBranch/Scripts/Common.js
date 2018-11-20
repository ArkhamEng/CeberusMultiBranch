



function Search(url, data, target)
{
 
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


function SetFilterControls(filters, button, target)
{
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

