function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Agregar Panel",
            autoOpen: false,
            resizable: false,
            height: 500,
            width: 600,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .2)),
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
function editNewHandler() {

    $(document).on("click", "#editDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Panel",
            autoOpen: false,
            resizable: false,
            height: 500,
            width: 600,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .2)),
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
    editNewHandler();

    confirmDeleteHandler();
});



$('#dialog-edit').on('click', '#btnGuardarPanel', function (e) {
    $('#frmAgregarPanel').submit();
});



$("body").on("submit", "#frmAgregarPanel", function (e) {

    //var mode = document.getElementById('mode').value;
    var ok = true;
    //var ok2 = false;
    //var ok3 = false;
    //var ok4 = false;

    //var nombre = $.trim($('#Glosa').val());
    //var nom = nombre.toUpperCase();
    //if (mode.includes(nom)) {
    //    jAlert("Ya existe un Examen con ese nombre.", "¡Alerta!");
    //} else {
    //    var ok = true;
    //}
    if (ok) {
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function (response) {

                var $resp = $(response);
                jAlert("Los datos fueron registrados correctamente.", "Aviso");
                $("#btnCerrar").removeClass("hidden");
                $("#btnCerrar").on("click", function () {
                    location.reload();
                    $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                });
               
            }
        });
    }

    e.preventDefault();
    return false;
});

$('#dialog-edit').on('click', '#btnEditarPanel', function (e) {
    $('#frmEditarPanel').submit();
});


$("body").on("submit", "#frmEditarPanel", function (e) {
    $.ajax({
        type: "POST",
        cache: false,
        url: $(this).attr("action"),
        data: $(this).serialize(),
        success: function (response) {

            var $resp = $(response);
            jAlert("Los datos fueron registrados correctamente.", "Aviso");
            $("#btnCerrar").removeClass("hidden");
            $("#btnCerrar").on("click", function () {
                location.reload();
                $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
            });

        }
    });
});