function createNewHandler() {
    
    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Nuevo Examen",
            autoOpen: false,
            resizable: false,
             height:'auto',
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

function editExistingHandler() {
    
    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Examen existente",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 580,
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
//            title: "Crear Nuevo Examen",
//            autoOpen: false,
//            resizable: false,
//            height: 400,
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

//function editExistingHandler() {
    
//    $(document).on("click", ".editDialog", function () {
//        var url = $(this).attr("href");
//        $("#dialog-edit").dialog({
//            title: "Editar Examen existente",
//            autoOpen: false,
//            resizable: false,
//            height: 400,
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

/***************************PLATAFORMA DE EXAMEN ************************************ */
function VerListaPlataforma() {
    $.ajax({
        url: URL_BASE + "Examen/VerListaPlataforma",
        cache: false,
        method: "POST",
        success: function (result) {
            var lista = [];
            var plataforma = [];
            var contenido = "";
            if (result.length > 0) {
                lista = result.split('|');
                contenido += "<table id='dtPlataforma' class='table - responsive'><thead><tr><th></th><th>Plataforma</th></tr></thead>";
                for (var i = 0; i < lista.length; i++) {
                    plataforma = lista[i].split(',');
                    contenido += "<tr><td><input class='form - check - input' type='checkbox' value=" + plataforma[0] + " id=" + plataforma[0] + "></td><td>" + plataforma[1] + "</td></tr>";
                }
                contenido += "</table>";
            }
            $("#dvPlataforma").html(contenido);
        },
    });
}

function RegistrarExamenPlataforma(idExamen) {
    debugger;
    var id = [];
    $('#dtPlataforma input:checked').each(function () {
        id.push($(this).attr('id'));
    });
    var examen = idExamen;
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Examen/RegistrarExamenPlataforma",
        data: { idPlataforma: id, idExamen: idExamen },
        success: function (result) {
            if (result = '1') {
                $('#exampleModalCenter').modal('hide');
                jAlert("Plataforma(s) registradas correctamente", "Alerta", function () {
                    location.reload(true);
                });    
            }
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

function EliminarExamenPlataforma(idExamen,idPlataforma) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Examen/EliminarExamenPlataforma",
        data: { idPlataforma: idPlataforma, idExamen: idExamen },
        success: function (result) {
            if (result = '1') {
                jAlert("Plataforma(s) eliminada(s) correctamente", "Alerta", function () {
                    location.reload(true);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
