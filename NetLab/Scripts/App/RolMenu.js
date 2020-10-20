var menuSeleccionados = 0;


function agregarEventosAgregarMenu() 
{
    $("#dialog-edit").on("change", ".chkMenuAgregar", function () {
        if ($(this).is(':checked')) {
            menuSeleccionados++;
        }
        else {
            menuSeleccionados--;
        }
    });


    $("#btnVolverRolMenu").on("click", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();

        return false;
    });

    $("#btnVolverRolMenu2").on("click", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();

        return false;
    });
}

function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Agregar Opciones de Menu",
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
            open: function () {
                //$(this).load(url);
                $(this).load(url, function () {
                    agregarEventosAgregarMenu();
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

    $('#dialog-edit').on('click', '#guardarMenu', function (e)
    {
        {
            e.preventDefault();
            var roles = "";

            if (menuSeleccionados < 1) {
                jAlert("Debe seleccione al menos una Opción de Menú.", "Aviso");
            }
            else {
                $('#formAgregarRolMenu').submit();
            }
        }
    });
});