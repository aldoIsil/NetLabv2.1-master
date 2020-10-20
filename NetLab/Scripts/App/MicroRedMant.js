function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Nueva MICRORED",
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
                $(this).load(url, function () {
                    agregarEventosPopupEnfermedadExamen();

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

function editExistingHandler() {

    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar MICRORED existente",
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
                $(this).load(url, function () {
                    agregarEventosPopupEnfermedadExamen();

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

    editExistingHandler();

    confirmDeleteHandler();
});


function agregarEventosPopupEnfermedadExamen() {
    $("#idEnfermedad").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "MicroRed/GetGetDisas"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione DISA/DIR/GER", no_results_text: "No existen coincidencias" });

    $("#idRed").chosen({ placeholder_text_single: "Seleccione Red", no_results_text: "No existen coincidencias" });


    
    
$("#dvPopupDisa").on("change", "#idEnfermedad", function () {
    var idDisa = "";
    $("#idEnfermedad :selected").each(function (i, selected) {

        idDisa = idDisa + "idDisa=" + $(selected).val() + "&";
    });
    $.ajax(
           {
               url: URL_BASE + "MicroRed/GetredesById?" + idDisa + "_=_",
               cache: false,
               method: "GET"
           }).done(function (msg) {
               $("#dvPopupRedes").html(msg);
               $("#idRed").chosen({ placeholder_text_single: "Seleccione la red", no_results_text: "No existen coincidencias" });
           }
           );
});


}


$("#idEnfermedad").change(function () {
    alert("Handler for .change() called.");
});


$(document).ready(function () {
    $('#idEnfermedad').bind('change', function (e) {
        //if ($('#status').val() == 'Rejected') {
        //    $('#reason').show();
        //    $("#reason").css({ display: "inline-block" });
        //    $('#decision').hide();
        //}
        //else if ($('#status').val() == 'Accepted') {
        //    $('#reason').hide();
        //    $('#decision').show();
        //}
    }).trigger('change');
});