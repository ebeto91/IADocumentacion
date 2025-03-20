function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}


function generateLink(filename, contentType, content) {
    // Create the URL
    const file = new File([content], filename, { type: contentType });
    const exportUrl = URL.createObjectURL(file);

    return exportUrl;
}

function toggleOptionsVisibility(show) {
    var optionsSection = document.querySelectorAll('#options-section');
    var allowOtherValueDiv = document.querySelectorAll('#allowOtherValueDiv');
    var allosJustify = document.getElementsByClassName("form-check-input");
    if (show) {
        optionsSection[optionsSection.length - 1].style.display = 'block';
        allowOtherValueDiv[allowOtherValueDiv.length - 1].style.display = 'block';
    } else {
        optionsSection[optionsSection.length - 1].style.display = 'none';
        allowOtherValueDiv[allowOtherValueDiv.length - 1].style.display = 'none';
        for (var i = 0; i < allosJustify.length; i++) {
            allosJustify[i].checked = true;
        }
    }
}
