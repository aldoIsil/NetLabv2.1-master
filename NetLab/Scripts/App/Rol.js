
function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Nuevo Rol",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 750,
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






//function createNewHandler() {

//    $(document).on("click", "#openDialog", function (e) {
//        e.preventDefault();
//        var url = $(this).attr("href");
//        $("#dialog-edit").dialog({
//            title: "Crear Nuevo Rol",
//            autoOpen: false,
//            resizable: false,
//            height: 300,
//            width: 500,
//            show: { effect: "drop", direction: "up" },
//            modal: true,
//            draggable: true,
//            open: function () {
//                $(this).load(url);
//            },
//            close: function () {
//                $(this).dialog("close");
//            }
//        });

//        $("#dialog-edit").dialog("open");
//        return false;
//    });
//}
















function eventosEdicion()
{
    $("#btnEditarRol").on("click", function ()
    {        
        var ok = true;
        var mensajeValidacionE = "Por favor ingresar el Nombre del Rol.";

        var nombreE = $("#txtNombreRolEditar").val();

        if (nombreE == "") {
            ok = false;
        }

        if (!ok) {
            jAlert(mensajeValidacionE, "Aviso");
            return false;
        }
        //else {
            
        //}

        
    });
}

function editExistingHandler() {

    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href")+"?_=" + (new Date()).getTime();
        $("#dialog-edit").dialog({
            title: "Editar Rol existente",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 760,
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
                    eventosEdicion();
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







//function editExistingHandler() {

//    $(document).on("click", ".editDialog", function () {
//        var url = $(this).attr("href")+"?_=" + (new Date()).getTime();
//        $("#dialog-edit").dialog({
//            title: "Editar Rol existente",
//            autoOpen: false,
//            resizable: false,
//            height: 300,
//            width: 500,
//            show: { effect: "drop", direction: "up" },
//            modal: true,
//            draggable: true,
//            open: function ()
//            {
//                $(this).load(url, function () {
//                    eventosEdicion();
//                });
//            },
//            close: function () {
//                $(this).dialog("close");
//            }
//        });

//        $("#dialog-edit").dialog("open");
//        return false;
//    });

    
//}










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

$(document).ready(function () {

    createNewHandler();

    editExistingHandler();

    confirmDeleteHandler();

    closeDialogHandler();


    //VALIDACION DE NUEVO ROL
    $('body').on('submit', '#frmNuevoRol', function (e)
    {
        e.preventDefault();

        var ok = true;
        var mensajeValidacionA = "Por favor ingresar el Nombre del Rol.";

        var mensajeValidacionB = "Ya existe el rol ingresado.";

        var nombreA = $("#txtNombreRolAgregar").val();
        var array = $("#nombreroles").val();

        var FLAT = 0

        var nomUpper=nombreA.toUpperCase();

        if (array.includes(nomUpper)) {
            ok = false
            FLAT = 1;
        }

        if (nombreA == "") {
            ok = false;
            FLAT = 2;
        }

        if (ok) {
            $.ajax({
                type: "POST",
                cache: false,
                url: $(this).attr('action'),
                data: $(this).serialize(),
                success: function (data) {

                    $("#BTNCREAR").addClass("hidden");
                    $("#btnCerrar").removeClass("hidden");
                    $("#btnCerrar").on("click", function () {
                        location.reload();
                        $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                    });
                    
                    jAlert('El Rol fue registrado.', 'Aviso');
                    //$("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                    
                }
            });
        }
        else {

            if (FLAT==1) {
                jAlert(mensajeValidacionB, "Aviso");
            } else {
                jAlert(mensajeValidacionA, "Aviso");
            }
            
        }

        return false;
    });



    //VALIDACION DE EDICION ROL
    //$('body').on('submit', '#frmEditarRol', function (e)
    $("#btnEditarRol").on("click", function ()
    {
        e.preventDefault();

        var ok = true;
        var mensajeValidacionE = "Por favor ingresar el Nombre del Rol.";

        var nombreE = $("#txtNombreRolEditar").val();

        if (nombreE == "") {
            ok = false;
        }

        if (ok) {
            $.ajax({
                type: "POST",
                cache: false,
                url: $(this).attr('action'),
                data: $(this).serialize(),
                success: function (data) {

                    jAlert('El Rol fue editado correctamente.', 'Aviso');
                    //$("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();

                }
            });
        }
        else {
            jAlert(mensajeValidacionE, "Aviso");
        }

        return false;
    });


});

