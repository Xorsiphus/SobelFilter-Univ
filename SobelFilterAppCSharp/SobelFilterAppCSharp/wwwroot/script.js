SaveFile = (file) => {
    let formData = new FormData();
    formData.append("formImage", file, file.name);

    $.ajax({
        type: "POST",
        url: "/Home/SendImage",
        success: (data) => {
            let imageContainer = $("#result-image-container");
            let image = document.createElement('img');
            image.setAttribute('src', 'data:image/jpeg;base64,' + data);
            while (!imageContainer.empty()) {
                imageContainer.remove(image.firstChild);
            }
            imageContainer.append(image);
            $("#spinner").hide();
        },
        async: true,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        timeout: 60000
    });
}

$("#send-button").on("click", function (_) {
    let input = $("#file-input")[0];
    let file = input.files[0];
    if (file) {
        $("#spinner").show();
        SaveFile(file);
    }
});

$("#file-input").on("change", function (e) {
    if (e.target.files.length === 0) return;
    let imageContainer = $("#preview-image-container");
    let image = document.createElement('img');
    image.setAttribute('id', 'image-preview');
    image.setAttribute('style', 'width: 100%');
    while (!imageContainer.empty()) {
        imageContainer.remove(image.firstChild);
    }
    imageContainer.append(image);

    let reader = new FileReader();
    reader.onload = function (event) {
        $('#image-preview').attr('src', event.target.result);
    }
    reader.readAsDataURL(e.target.files[0]);
});

$("#spinner").hide();