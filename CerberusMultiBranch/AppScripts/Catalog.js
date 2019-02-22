
//Muestra la ventada emergente de Edición /Captura de catalogos, dependiendo del 
//tipo dado en el parametro Entity (Client,Employee,Supplier, Product, etc), si se envía un disable call back
//se considerara que la ventana no esta en modo de edición
function ShowCatalogModal(OnCompleate, CloseCallBack, Entity, id, disableCallBack)
{
    ShowLoading('static');

    var param = {};
    var url = "";
    var unlockUrl = "";
    var idField = "";
    //si es para Editar
    
    if (!isNaN(id) && id > 0)
    {
        param = { id: id };

        switch (Entity)
        {
            case "Client":
                url = "/Clients/BeginAdd";
                unlockUrl = "/Clients/UnLock/";
                idField = "ClientId";
                break;
            case "Provider":
                url = "/Providers/BeginAdd";
                unlockUrl = "/Providers/UnLock/";
                idField = "ProviderId";
                break;
            case "Employee":
                url = "/Employees/BeginAdd";
                unlockUrl = "/Employees/UnLock/";
                idField = "EmployeeId";
                break;
            case "Product":
                url = "/Products/BeginAdd";
                unlockUrl = "/Products/UnLock/";
                idField = "ProductId";
                break;
        }
    }
    else if(id == 0)
    {
        switch (Entity)
        {
            case "Client":
                url = "/Clients/BeginAdd";
                break;
            case "Provider":
                url = "/Providers/BeginAdd";
                break;
            case "Employee":
                url = "/Employees/BeginAdd";
                break;
            case "Product":
                url = "/Products/BeginAdd";
                break;
        }
    }
    else
    {
        param = id;
        switch (Entity)
        {
            case "ProductCopy":
                url = "/Products/BeginCopy";
                break;
        }
    }
    var form = "#SaveForm";

    ExecuteAjax(url, param, function (response)
    {
        HideLoading(function ()
        {
            if (!$.isPlainObject(response))
            {
                //esto se debe hacer siempre que se oculte la ventana
                var onHidden = function ()
                {
                    //si el se tiene un id y no hay callback de deshabilitación remuevo el bloqueo
                    if (parseInt(param.id) > 0 && typeof (disableCallBack) == 'undefined')
                    {
                        ExecuteAjax(unlockUrl, { id: param.id }, function (response)
                        {
                            ShowNotify(response.Header, response.Result, response.Body, 3000);
                        });
                    }
                    //si hay callback al cerrar lo ejecuto
                    if (CloseCallBack != null)
                        CloseCallBack();
                };

                ShowModal(response, 'static', 'lg', onHidden);



                //si se recibe un id
                if (typeof (param.id) != 'undefined')
                {
                    //si se recibe un callback de deshabilitación.. entro a modo visualización
                    if (typeof (disableCallBack) != 'undefined')
                        disableCallBack();
                        //de lo contrario, entro a modo edición y bloqueo el registro
                    else
                    {
                        ShowNotify("Registro bloqueado!", "warning", "Dispones de 5 min para realizar cambios, sobre este registro", 4000);
                    }
                }

                if (Entity != "Product")
                    SubmitPerson(OnCompleate, form, idField);
                else
                    SubmitProduct(OnCompleate, form, idField);

                //evento del boton cancel
                $("#EditCancel").off('click').click(function (e)
                {
                    HideModal(function ()
                    {
                        onHidden();
                    }, true)
                });

                $("#btnCloseEdit").off('click').click(function (e)
                {
                    HideModal(function ()
                    {
                        onHidden();
                    }, true)
                });
            }
            else
                ShowNotify(response.Header, response.Result, response.Body, 4500);
        });
    });
}



function SubmitPerson(SuccessCallBack, form, idField) {
    $(form).off('submit').on('submit', function (e)
    {
        e.preventDefault();

        var $form = $(e.target),
        formData = new FormData(),
        params = $form.serializeArray(),
        files = [],
        addresses = [];

        var inputF = $(form).find('[type="file"]');

        if (inputF[0] != 'undefined' && inputF[0] != null) {
            var file = inputF[0].files[0];

            if (file != 'undefined' && file != null)
                formData.append('PostedFile', file);
        }


        if (!$form.valid()) {
            ShowNotify("Error de validación", "danger", "Existen errores en lo datos capturados, por favor verifica", 3500);
            return;
        }


        //agrego todos los campos del formulario
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        formData.append('Addresses[0].' + idField, $("#Address_" + idField).val());
        formData.append('Addresses[0].AddressId', $("#Address_AddressId").val());

        formData.append('Addresses[0].CityId', $("#Address_CityId").val());
        formData.append('Addresses[0].ZipCode', $("#Address_ZipCode").val());
        formData.append('Addresses[0].Location', $("#Address_Location").val());
        formData.append('Addresses[0].Street', $("#Address_Street").val());
        formData.append('Addresses[0].Reference', $("#Address_Reference").val());

        ShowModLoading();

        $.ajax({
            url: $form.attr('action'),
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (response) {
                HideModLoading();

                if ($.isPlainObject(response) && response.Code != 200) {
                    ShowNotify(response.Header, response.Result, response.Body, 3500);

                    switch (response.Code) {
                        case 401:
                            window.location = data.LogOnUrl;
                            break;
                    }
                }
                else {
                    HideModal(function () {
                        ShowNotify(response.Header, response.Result, response.Body);
                        SuccessCallBack(response.Id);
                    }, true);
                }
            },
            error: function () { HideModLoading(); }
        });
    });
}



function SubmitProduct(SuccessCallBack, form, idField)
{
    $(form).off('submit').on('submit', function (e)
    {
        e.preventDefault();

        var $form = $(e.target),
        formData = new FormData(),
        params = $form.serializeArray(),
        files = [],
        addresses = [];

       
        if (!$form.valid())
        {
            ShowNotify("Error de validación", "danger", "Existen errores en lo datos capturados, por favor verifica", 3500);
            return;
        }


        //agrego todos los campos del formulario
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

     
        ShowModLoading();

        $.ajax({
            url: $form.attr('action'),
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (response)
            {
                HideModLoading();

                if ($.isPlainObject(response) && response.Code != 200)
                {
                    ShowNotify(response.Header, response.Result, response.Body, 3500);

                    switch (response.Code) {
                        case 401:
                            window.location = data.LogOnUrl;
                            break;
                    }
                }
                else {
                    HideModal(function ()
                    {
                        ShowNotify(response.Header, response.Result, response.Body, 3500);
                        SuccessCallBack(response.Id);
                    }, true);
                }
            },
            error: function () { HideModLoading(); }
        });
    });
}


function ShowConfirm(text, url, id, CompleatedCallBack)
{

    $("#ConfirmText").html(text);

    $("#ConfirmAccept").off('click').click(function ()
    {
        HideConfirm(function ()
        {
            ShowLoading();

            console.log("url:" + url + "Id:" + id);

            ExecuteAjax(url, { id: id }, function (response)
            {
                HideLoading(function ()
                {
                    if (CompleatedCallBack != null && CompleatedCallBack != 'undefined' && response.Code == 200)
                    {
                        CompleatedCallBack();
                    }

                    ShowNotify(response.Header, response.Result, response.Body, 3000);
                });
            });
        });
    });

    $("#ConfirmCancel").off('click').click(function () {
        HideConfirm();
    });

    $("#ModalConfirm").modal({ backdrop: 'static' });
}

function HideConfirm(callback)
{
    $("#ModalConfirm").off("hidden.bs.modal").on("hidden.bs.modal", function (e)
    {
        if (callback != null)
            callback();
    });

    $("#ModalConfirm").modal('hide');
}


function ShowQuickSearch(url, onRecordSelected, onClosed, param)
{
    ShowLoading('static');
    

    if (param == null)
        param = { quickSearch: true }

    ExecuteAjax(url, param, function (response)
    {
        HideLoading(function ()
        {
            recordSelected = onRecordSelected;

            onCloseQuickSearch = onClosed;

            ShowModal(response, 'static', 'lg');
        });
    });
}