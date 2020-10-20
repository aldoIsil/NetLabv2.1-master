
var rolesSeleccionados = 0;

function AgregarRol(idUsuario) {
    debugger;
    $.ajax({
        type: "GET",
        cache: false,
        url: URL_BASE + "Usuario/AgregarRol?idUsuario=" + idUsuario,

        success: function (data) {
            $('#DatoRol').html(data);
        }
    });
}

//function agregarEventosAgregarRol()
//{
   

//    $("#dialog-edit").on("change", ".chkRolAgregar", function () {
//        if ($(this).is(':checked'))
//        {
//            rolesSeleccionados++;
//        }
//        else {
//            rolesSeleccionados--;
//        }
//    });


//    $("#btnVolverRolUsuario").on("click", function ()
//    {
//        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();

//        return false;
//    });

//    $("#btnVolverRolUsuario2").on("click", function () {
//        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();

//        return false;
//    });

//}

function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Agregar Roles",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 700,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },

            //height: 590,
            //width: 700,


            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                //$(this).load(url);
                $(this).load(url, function () {
                 //   agregarEventosAgregarRol();
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


    //$('body').on('submit', '#formAgregarRolUsuario', function (e)
    //$('#dialog-edit').on('click', '#guardarRoles', function (e)
    //{
    //    {
    //        e.preventDefault();
    //        var roles = "";

    //        if (rolesSeleccionados < 1) {
    //            jAlert("Debe seleccione al menos un Rol.", "Aviso");
    //        }
    //        else
    //        {
    //            $('#formAgregarRolUsuario').submit();
    //        }
    //    }
    //});



});

$('#dialog-edit').on('click', '#guardarRoles', function (e) {
    $('#formAgregarRolUsuario').submit();
});




$("body").on("submit", "#formAgregarRolUsuario", function (e) {

    var ok = false;
    $('#TableRol tr')
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

                jAlert("Rol(es) Asignados/Actualizados Satisfactoriamente ", "Aviso");
            }
        });

    } else {
        jAlert("Debe seleccionar un Rol", "Aviso");
    }

    e.preventDefault();
    return false;
});

