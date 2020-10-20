
function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Nuevo ",
            autoOpen: false,
            resizable: false,
            height: 300,
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

//CATALOGO DE SERVICIOS MODAL
function editExistingHandler() {

    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href");
        $(".editDialog").draggable();
        $("#dialog-edit").dialog({
            title: "Detalle de Enfermedad",
            autoOpen: false,
            resizable: false,
            position: 'center',
            height: 'auto',
            width: 'auto',
            fluid: true,
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
//            title: "Detalle de Enfermedad",
//            autoOpen: false,
//            resizable: false,
//            height: 500,
//            width: 1300,
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







function confirmDeleteHandler() {

    $(document).on("click", ".confirmDialog", function () {

        var url = $(this).attr("href");
        $("#dialog-confirm").dialog({
            autoOpen: false,
            resizable: false,
            height: 170,
            width: 350,
            fluid: true,
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


$("#dialog-edit").on("click", "#CerrarPopUp", function (e) {
    $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
});


function ValidateTextboxLength(event, newvalue) {
    //if (EsCombinacionEspecial(event))
    //    return true;

    var controlId = event.target.id;
    var textbox = $.trim($('#' + controlId).val());
    var newTextValue = textbox + newvalue;

    var lengthToCompare;

    switch (controlId) {
        case "search":
            lengthToCompare = 40;
            break;
        default:
            lengthToCompare = 40;
    }

    if (textbox.length === lengthToCompare || newTextValue.length > lengthToCompare)
        return false;

    return true;
}

//Se creo para que estas acciones funcionen en firefox. IE y Chrome no tienen problemas.
function EsCombinacionEspecial(event) {
    return event.charCode === 0 ||  //Esto es para firefox: delete,supr,arrow keys.
            (event.ctrlKey && event.key === "x") ||
            (event.ctrlKey && event.key === "c") ||
            (event.ctrlKey && event.key === "v") ||
            (event.key === "Enter");
}