function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Agregar Muestras",
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

function closeDialogHandler() {

    $(document).on("click", "#closeDialog", function (e) {
        e.preventDefault();

        $("#dialog-edit").dialog("close");

        return false;
    });
}

function enfermedadHandler() {

    $(document).on("change", "#idEnfermedad", function () {
        enfermedadTrigger();
    });
}

function enfermedadTrigger() {
    
    var idEnfermedad = $("#idEnfermedad").val();

    $.ajax({
        url: $("#idEnfermedad").attr("data-examen-url") + "?idEnfermedad=" + idEnfermedad,
        cache: false,
        method: "GET"
    }).done(function (msg) {
        $("#dvExamen").html(msg);
        selectChosenConfig("idExamen", "Seleccione el Examen");
        examenHandler();
        examenTrigger();
    });
}

function examenHandler() {

    $(document).on("change", "#idExamen", function () {
        examenTrigger();
    });
}

function examenTrigger() {

    var idExamen = $("#idExamen").val();

    $.ajax({
        url: $("#idExamen").attr("data-url") + "?idExamen=" + idExamen,
        cache: false,
        method: "GET"
    }).done(function (msg) {
        $("#dvMuestra").html(msg);
        selectChosenConfig("idTipoMuestra", "Seleccione el tipo de Muestra");
        muestraHandler();
        muestraTrigger();
    });
}

function muestraHandler() {

    $(document).on("change", "#idTipoMuestra", function () {
        muestraTrigger();
    });
}

function muestraTrigger() {

    var idTipoMuestra = $("#idTipoMuestra").val();

    $.ajax({
        url: $("#idTipoMuestra").attr("data-url") + "?idTipoMuestra=" + idTipoMuestra,
        cache: false,
        method: "GET"
    }).done(function (msg) {
        $("#dvMaterial").html(msg);
        selectChosenConfig("idMaterial", "Seleccione el Material");
    });
}

$(document).ready(function () {

    createNewHandler();

    confirmDeleteHandler();

    closeDialogHandler();
});