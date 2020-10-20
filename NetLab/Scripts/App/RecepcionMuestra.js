// Descripción: Configura el popup para mostrar infomación de la orden y sus muestras, no editable.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
$(document).ready(function () {

    $.ajaxSetup({ cache: false });

    $(".editDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr('href'); 

        $("#dialog-edit").dialog({
            title: 'Orden Recibida',
            autoOpen: false,
            resizable: false,
            height: 300,
            width: 450,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url, function () {
                });
            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });

        $("#dialog-edit").dialog('open');
        return false;
    });

    $("#btnCancelar").on("click", function ()
    {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    });


});