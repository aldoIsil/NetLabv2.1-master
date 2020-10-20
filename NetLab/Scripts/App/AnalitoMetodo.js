﻿function createNewHandler() {
    
    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Nuevo Método",
            autoOpen: false,
            resizable: false,
            height: 360,
            width: 500,
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

function editExistingHandler() {
    
    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Método existente",
            autoOpen: false,
            resizable: false,
            height: 360,
            width: 500,
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

    editExistingHandler();

    confirmDeleteHandler();
});

