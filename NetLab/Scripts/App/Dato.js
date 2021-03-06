﻿function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();

        var url = $(this).attr("href");

        $("#dialog-edit").dialog({
            title: "Crear Nuevo Dato",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 450,
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
                checkTipoDato();
            },
            close: function () {
                $(this).dialog("close");
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });
}

function editExistingHandler() {

    $(document).on("click", ".editDialog", function () {

        var url = $(this).attr("href");

        $("#dialog-edit").dialog({
            title: "Editar Dato existente",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 450,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function() {
                    toggleTipoDato();
                });
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

function checkTipoDato() {

    var selected = $("#Tipo_IdTipoDato option:selected").text().toUpperCase();

    var combo = "combo".toUpperCase();

    if (selected === combo) {
        $("#dvLista").show();
    } else {
        $("#dvLista").hide();
    }
}

function toggleTipoDato() {

    checkTipoDato();

    $(document).on("change", "#Tipo_IdTipoDato", function () {

        checkTipoDato();
    });
}

$(document).ready(function () {

    createNewHandler();

    editExistingHandler();

    confirmDeleteHandler();

    closeDialogHandler();

    toggleTipoDato();
});