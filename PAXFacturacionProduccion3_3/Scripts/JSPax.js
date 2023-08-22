function ShowAlertError(strTitle, strMessage, strTextOkButton, strSize, strImage) {
    bootbox.alert({
        title: '<img src="../img/' + strImage + ' "><strong>&nbsp;&nbsp;' + strTitle + '</strong>',
        message: '<p  class="text-center">' + strMessage + '</p>',
        size: strSize,
        buttons: {
            ok: {
                label: strTextOkButton,
                className: "btn"
            }
        }
    });
}



