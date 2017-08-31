$(document).ready(function () {

    $('#sidebarCollapse').on('click', function ()
    {
        $('#sidebar').toggleClass('active');
    });

});

function SetCascade(ddlParent, ddlChild, action) {
    console.log("Setting Cascade Controls State/Cities");
    console.log("Child Value " + $(ddlChild).val());

    if ($(ddlChild).val() != '')
        $(ddlChild).attr("readonly", false);
    else
        $(ddlChild).attr("readonly", true);

    $(ddlParent).change(function ()
    {
        console.log("Populating Cascade");
        if ($(ddlParent).val() != '') {
            var parentId = $(ddlParent).val();
            console.log("Searching for " + parentId);

            var AObj =
                {
                    url: "/Cascade/" + action,
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