function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Agregar Tipo de Muestra",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 600,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);
            },
            close: function () {
                $(this).dialog("close");
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });
}

function confirmDeleteHandler() {

    $(document).on("click", ".confirmDialog", function () {

        var url = $(this).attr("href");
        $("#dialog-confirm").dialog({
            autoOpen: false,
            resizable: false,
            height: 170,
            width: 350,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            buttons: {
                "OK": function () {
                    $(this).dialog("close");
                    window.location = url;

                },
                "Cancelar": function () {
                    $(this).dialog("close");

                }
            }
        });
        $("#dialog-confirm").dialog("open");
        return false;
    });
}

$(document).ready(function () {

    createNewHandler();

    confirmDeleteHandler();
});


$('#dialog-edit').on('click', '#btnGuardarTipoMuestra', function (e) {
    $('#frmAgregarTipoMuestra').submit();
});


$("body").on("submit", "#frmAgregarTipoMuestra", function (e) {


    var ok = false;


    var valor = $("#idTipoMuestra").val();
    var textocombo = $("#idTipoMuestra option:selected").text();

    var nombrecomboSeleccionado = textocombo.toUpperCase();

        var mode = document.getElementById('mode').value;

        if (mode.includes(nombrecomboSeleccionado)) {
            jAlert("Ya se Asigno este Tipo de Muestra.", "¡Alerta!");
        } else {
            var ok = true;
        }



    if (ok) {
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function (response) {

                var $resp = $(response);

                $("#btnCerrar").removeClass("hidden");
                $("#btnCerrar").on("click", function () {
                    location.reload();
                    $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                });

                jAlert("Tipo de Muestra Asignado Satisfactoriamente ", "Aviso");
            }
        });

    }

    e.preventDefault();
    return false;
});