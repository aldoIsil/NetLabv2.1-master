﻿function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Agregar Criterios de Rechazo",
            autoOpen: false,
            resizable: false,
            height: 680,
            width: 700,

            //height: 480,
            //width: 700,

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



$('#dialog-edit').on('click', '#btnGuardarCriterio', function (e) {
    $('#formAgregarCriterio').submit();
});




$("body").on("submit", "#formAgregarCriterio", function (e) {

    var ok = false;
        $('#TableCriterioRechazo tr')
            .each(function () {
                var checkboxElement = $(this).find('input[type="checkbox"]');

                if (checkboxElement != undefined && checkboxElement.length > 0) {
                    var checkValue = checkboxElement[0].checked;

                 
                    if (checkValue) ok = true;
                }

            });

        if (ok)
        {
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

                    jAlert("Criterio(s) de rechazo(s) Asignados/Actualizados Satisfactoriamente ", "Aviso");
                }
            });

        } else {
            jAlert("Debe seleccionar un criterio de rechazo", "Aviso");
        }

    e.preventDefault();
    return false;
});



//function ExisteElementoSeleccionado() {
//    var existe = false;
//    $('#TableCriterioRechazo tr')
//        .each(function () {
//            var checkboxElement = $(this).find('input[type="checkbox"]');
//
//            if (checkboxElement != undefined && checkboxElement.length > 0) {
//                var checkValue = checkboxElement[0].checked;
//
//               //  jAlert("Recepcion Realizada Satisfactoriamente ", "Aviso");
//                if (checkValue) existe = true;
//            }
//
//        });
//
//    if (!existe)
//        jAlert("Seleccione por lo menos un elemento.", "¡Alerta!");
//
//    return false;
//}