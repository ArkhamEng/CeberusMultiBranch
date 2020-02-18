
var type = imageEditorType;

var aspect = NaN;

var uploadedImageName = 'cropped.jpg';
var uploadedImageType = 'image/jpeg';

var URL = window.URL || window.webkitURL;

var imgSource = $('#imgSource');
var inputImg = $('#inputImg');

imgSource.attr("src", currentImage.attr("src"));

var Sizes = {
    "default": [ { "width": 1000, "height": 1000 },
                 { "width": 500, "height": 500 } ,
                 { "width": 250, "height": 250 }],

    "vertical": [{ "width": 980, "height": 820 } ,
                 { "width": 480, "height": 320 } ,
                 { "width": 280, "height": 120 } ],

    "horizontal": [{ "width": 820, "height": 980 },
                   { "width": 320, "height": 480 },
                   { "width": 120, "height": 280 }]
}


var options =
{
    aspectRatio: aspect,
    preview: 'img-preview',
    crop: function (e) {
        $("#txtWidth").val(Math.round(e.detail.width));
        $("#txtHeight").val(Math.round(e.detail.height));
    }
};



$(document).ready(function ()
{
    SetCrop();
    SetLoadImage();
});



function SetCrop()
{
    switch (type)
    {
        case "vertical":
            aspect = 1 / 1.5;
            break;
        case "horizontal":
            aspect = 1.5 / 1;
            break;
        default:
            aspect = 1 / 1;
            break;
    }

    options.aspectRatio = aspect;
   
    imgSource.on({
        ready: function (e) {
            console.log("cropper ready");
            GetSamples();
        },
        cropstart: function (e) {
            //console.log("cropstart");
           // GetSamples();
        },
        cropmove: function (e) {
            console.log("cropper cropmove");
            //GetSamples();
        },
        cropend: function (e) {
            console.log("cropend");
            GetSamples();
        },
        crop: function (e) {
           // console.log("crop");
           // GetSamples();
        },
        zoom: function (e) {
            console.log("cropper zoom");
            GetSamples();
        }
    }).cropper(options);
}




function GetSamples() {

    var size = Sizes[type];

    var largeImg  = imgSource.cropper("getCroppedCanvas", { "width": size[0].width, "height": size[0].height });
   
    $("#largeImg").attr("src", largeImg.toDataURL());
}

function AceptImage()
{
    var images = [];

    var size = Sizes[type];

    for (var i = 2; i >-1; i--) {
        var canvas = imgSource.cropper("getCroppedCanvas", { "width": size[i].width, "height": size[i].height });
        images.push(canvas.toDataURL());
    }

    currentImage.attr("src", images[2]);
    OnImageSelected(images);
    HideModal(null, true);
}

function SetLoadImage() {
   
    var uploadedImageURL;

    options.aspectRatio = aspect;

    if (URL)
    {
        inputImg.change(function ()
        {
            var files = this.files;
            var file;

            if (!imgSource.data('cropper'))
            {
                return;
            }

            if (files && files.length)
            {
                file = files[0];

                if (/^image\/\w+$/.test(file.type))
                {
                    uploadedImageName = file.name;
                    uploadedImageType = file.type;

                    if (uploadedImageURL)
                    {
                        URL.revokeObjectURL(uploadedImageURL);
                    }

                    uploadedImageURL = URL.createObjectURL(file);
                    imgSource.cropper('destroy').attr('src', uploadedImageURL).cropper(options);
                    inputImg.val('');
                } else {
                    window.alert('El archivo debe ser una imagen válida');
                }
            }
        });
    } else {
        inputImg.prop('disabled', true).parent().addClass('disabled');
    }
}