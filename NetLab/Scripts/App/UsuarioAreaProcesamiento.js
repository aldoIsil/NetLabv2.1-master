







//var areasSeleccionadas = 0;

//function agregarEventosAgregarAreasProcesamiento()
//{
//    $("#dialog-edit").on("change", ".chkAreaAgregar", function () {
//        if ($(this).is(':checked')) {
//            areasSeleccionadas++;
//        }
//        else {
//            areasSeleccionadas--;
//        }
//    });


//    $("#btnVolverRolArea").on("click", function ()
//    {
//        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
//        return false;
//    });

//    $("#btnVolverRolArea2").on("click", function () {
//        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
//        return false;
//    });
//}

function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Agregar Área de Procesamiento",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 700,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function ()
            {
                $(this).load(url, function () {
                    agregarEventosAgregarAreasProcesamiento();
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

$(document).ready(function () {

    createNewHandler();

    confirmDeleteHandler();

    $('#dialog-edit').on('click', '#guardarAreas', function (e) {
        {
            e.preventDefault();
            var areas = "";

            if (areasSeleccionadas < 1)
            {
                jAlert("Debe seleccione al menos una Área de Procemiento.", "Aviso");
            }
            else {
                $('#formAgregarAreaUsuario').submit();
            }
        }
    });

});




$('#dialog-edit').on('click', '#guardarAreas', function (e) {
    $('#formAgregarAreaUsuario').submit();
});




$("body").on("submit", "#formAgregarAreaUsuario", function (e) {

    var ok = false;
    $('#TableAreaProcesamiento tr')
        .each(function () {
            var checkboxElement = $(this).find('input[type="checkbox"]');

            if (checkboxElement != undefined && checkboxElement.length > 0) {
                var checkValue = checkboxElement[0].checked;


                if (checkValue) ok = true;
            }

        });

    if (ok) {
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function (response) {

                var $resp = $(response);

               
                $("#btnCerrar").on("click", function () {
                    location.reload();
                    $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                });

                jAlert("Area de Procesamiento(s) Asignados/Actualizados Satisfactoriamente ", "Aviso");
            }
        });

    } else {
        jAlert("Debe seleccionar un Area de Procesamiento(s)", "Aviso");
    }

    e.preventDefault();
    return false;
});
