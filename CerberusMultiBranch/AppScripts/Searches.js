const MinimumSearchLength = 3

/**Comienza una busqueda de cliente 
*@param  {string} input identificador del textbox que contiene el filtro
*@param  {function} customerSelected función a ejecutar al seleccionar un cliente 
*/
function SearchCustomer(input, customerSelected)
{
    if ($(input).val().length < MinimumSearchLength)
    {
        ShowNotify("Faltan datos", "warning", "La búsqueda requiere" + MinimumSearchLength + " caractéres por lo menos", 3000);
        return;
    }

    ShowLoading('static');

    GetAjax('/Selling/SearchCustomer', { filter: $(input).val() }, function (response)
    {
        HideLoading(function ()
        {
            //si encontre solo una coincidencia, la pongo de inmediato
            if ($.isPlainObject(response))
                customerSelected(response.JProperty);

            else
            {
                ShowModal(response, 'static', 'lg', null);

                OnCustumerSelected = customerSelected;
                $("#SearchCustomerFilter").val($(input).val());
            }
        });
    }); 
}


/**Comienza una busqueda de Producto 
*@param  {string} input identificador del textbox que contiene el filtro
*@param  {function} productSelected función a ejecutar al seleccionar un producto 
*@param  {function} productsShown función a ejecutar al mostrar los resultados de la búsqueda 
*/
function SearchProduct(input, productSelected, productsShown)
{
    if ($(input).val().length < MinimumSearchLength)
    {
        ShowNotify("Faltan datos", "warning", "La búsqueda requiere" + MinimumSearchLength + " caractéres por lo menos", 3000);
        return;
    }

    ShowLoading('static');

    GetAjax('/Selling/SearchProduct', { filter: $(input).val() }, function (response)
    {
        HideLoading(function ()
        {
            //si encontre solo una coincidencia, la pongo de inmediato
            if ($.isPlainObject(response))
                productSelected(response.JProperty);

            else
            {
                ShowModal(response, 'static', 'lg', null);

                OnProductsShown   = productsShown;
                OnProductSelected = productSelected;
                
                $("#SearchProductFilter").val($(input).val());
            }
        });
    });
}