$(document).ready(function () {

    $.ajaxSetup({ cache: false });

    $("#openDialog").on("click", null, function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        $("#dialog-edit").dialog({
            title: 'Crear Nueva Presentacion',
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 500,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url);
            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });

        $("#dialog-edit").dialog('open');
        return false;
    });

    $(".editDialog").on("click", null, function (e) {
        var url = $(this).attr('href');
        $("#dialog-edit").dialog({
            title: 'Editar Presentacion',
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 500,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url);

            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });

        $("#dialog-edit").dialog('open');
        return false;
    });

    $(".confirmDialog").on("click", null, function (e) {

        var url = $(this).attr('href');
        $("#dialog-confirm").dialog({
            autoOpen: false,
            resizable: false,
            height: 170,
            width: 350,
            show: { effect: 'drop', direction: "up" },
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
        $("#dialog-confirm").dialog('open');
        return false;
    });
});


function ValidaCampoRequeridoPresentacion() {
    var nombre = $.trim($('#glosa').val());

    if (nombre === '') {
        jAlert("Ingrese el nombre de la presentación", "¡Alerta!");
        return false;
    }

    return true;
}

function ValidaCampoRequeridoPresentacionEditar() {
    var nombre = $.trim($('#glosa').val());

    if (nombre === '') {
        jAlert("Ingrese el nombre de la presentación", "¡Alerta!");
        return false;
    }

    return true;
}
