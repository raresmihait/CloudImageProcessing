// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
var openFile = function (event) {
    var input = event.target;

    var urlReader = new FileReader();
    urlReader.onload = function () {
        var url = urlReader.result;
        var node = document.getElementById('inputImg');
        node.src = url;
        node.text = input.files[0];
    };

    if (input.files.length > 0) {
        toggleButtonDisable(false);
        urlReader.readAsDataURL(input.files[0]);
    } else {
        toggleButtonDisable(true);
    }
};

var applyEffectA = function () {
    applyEffect(1);
};

var applyEffectB = function () {
    applyEffect(2);
};

var applyEffectC = function () {
    applyEffect(3);
};

var applyEffect = function (effectCode) {

    toggleLoadingDisplay(true);

    var effectUrl = "ApplyGrayscale";

    if (effectCode === 1) {
        effectUrl = "ApplyGrayscale";
    } else if (effectCode === 2) {
        effectUrl = "ApplyInvert";
    } else if (effectCode === 3) {
        effectUrl = "ApplyContrast";
    }

    var input = $('#uploadFile')[0].files[0];

    var formData = new FormData();
    formData.append('file', input);

    $.ajax({
        url: 'api/ImageProcessing/' + effectUrl,
        type: 'POST',
        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        success: function (response) {//make sure the server sends only the required data
            document.getElementById("outputImg").src = "data:image/png;base64," + response;

            toggleLoadingDisplay(false);
        },
        error: function (response) {//make sure the server sends only the required data
            toggleLoadingDisplay(false);
        }
    });
};

var toggleLoadingDisplay = function (active) {
    if (active === true) {
        $("#outputImg").hide();
        $("#spinner").show();
    } else {
        $("#outputImg").show();
        $("#spinner").hide();
    }

    $("#uploadFile").attr("disabled", active);
    toggleButtonDisable(active);
};

var toggleButtonDisable = function (active) {
    $("#btnEffectA").attr("disabled", active);
    $("#btnEffectB").attr("disabled", active);
    $("#btnEffectC").attr("disabled", active);
};