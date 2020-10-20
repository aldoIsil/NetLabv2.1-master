function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Agregar Metodo",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 500,
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



  $('#dialog-edit').on('click', '#btnGuardarMetodo', function (e) {
            $('#frmAgregarMetodo').submit();
        });



        $("body").on("submit", "#frmAgregarMetodo", function (e) {

            var mode = document.getElementById('mode').value;
        var ok=false;
        var ok2=false;
        var ok3=false;
        var ok4=false;

        var nombre = $.trim($('#Glosa').val());


        



        var nom=nombre.toUpperCase();
        if (mode.includes(nom)) {
        jAlert("Ya existe un Examen con ese nombre.", "¡Alerta!");
        }else {
        var ok=true;
        }

        if ( ok )
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
        jAlert("Los datos fueron registrados correctamente.", "Aviso");
        }
        });

        }

        e.preventDefault();
        return false;
        });