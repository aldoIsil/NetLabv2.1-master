function createNewHandler() {
    
    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Nuevo Componente",
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


//function editExistingHandler() {

//    $(document).on("click", ".editDialog", function () {
//        var url = $(this).attr("href");
//        $("#dialog-edit").dialog({
//            title: "Editar Componente existente",
//            autoOpen: false,
//            resizable: false,
//            height: 360,
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





function editExistingHandler() {
    
    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Componente existente",
            autoOpen: false,
            resizable: false,
            height:'auto',
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

    PopUpNewAnalitoInterpretacion();

    PopUpEditarAnalitoInterpretacion();

    EditarAnalitoInterpretacion();
});

//SOTERO BUSTMANTE

$(".nvoOpcionResultado").on("click", function (e) {
    e.preventDefault();
    //var idOrden = e.target.nameProp;
    var url = $(this).attr("href") + "?_=" + (new Date()).getTime();
    var x = this.id;
    alert(x);
    $("#dialog-open").html("");
    $("#dialog-open").dialog({
        title: "Agregar Examen",
        autoOpen: false,
        resizable: false,
        draggable: true,
        height: 200,
        width: 500,
        show: { effect: "drop", direction: "up" },
        modal: true,
        responsive: true,

        fluid: true,
        helper: 'clone',
        open: function () {
            $(this).load(url, function () {
                agregarEventosPopupEnfermedadExamen(x);
            });
        },
        close: function (event, ui) {
            $(this).dialog('close');
        }
    });
    $("#dialog-open").dialog("open");
    return false;
});


/********Interpretacion de Resultados***********************/
function PopUpNewAnalitoInterpretacion() {
    $(document).on("click", "#openDialogInterpretacion", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-open").dialog({
            title: "Nueva Interpretación",
            autoOpen: false,
            resizable: false,
            height: 300,
            width: 800,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .2)),
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
        $("#dialog-open").dialog("open");
        return false;
    });
}

function PopUpEditarAnalitoInterpretacion() {
    $(document).on("click", "#editDialogInterpretacion", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Interpretación",
            autoOpen: false,
            resizable: false,
            height: 350,
            width: 800,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .2)),
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

function EditarAnalitoInterpretacion() {
    $('#dialog-edit').on('click', '#btnActualizar', function (e) {
        $('#formEditarAnalitoInterpretacion').submit();
    });

    $("body").on("submit", "#formEditarAnalitoInterpretacion", function (e) {
        var idInterpretacion = $("#idInterpretacion").val();
        var idAnalito = $("#idAnalito").val();
        var GlosaParent = $("#GlosaParent").val();
        var Glosa = $("#Glosa").val();
        var Interpretacion = $("#Interpretacion").val();
        var estado = $("#chkActivoAI").is(":checked") ? 1 : 0;
        if (GlosaParent == "" || Glosa == "" || Interpretacion == "") {
            jAlert("Debe registrar todos los campos");
            return false;
        }
        var datos = "?idInterpretacion=" + idInterpretacion + "&idAnalito=" + idAnalito + "&GlosaParent=" + GlosaParent + "&Glosa=" + Glosa + "&Interpretacion=" + Interpretacion + "&estado=" + estado;
        $.ajax(
            {
                url: URL_BASE + "Analito/EditarAnalitoInterpretacion" + datos,
                cache: false,
                method: "GET",
            }).success(function (response) {
                jAlert("Se actualizaron los datos correctamente.", "Alerta!");
            });
        e.preventDefault();
        return false;
    });
}