function closeEditDialog(msg) {

    $("#dialog-edit").dialog("close");
    $("#UserMessage").html("<h4 class=\"msj-error\">" + msg + "</h4>");
}

function enfermedadHandler() {

    $("#idEnfermedad").on("change", function () {

        var idEnfermedad = $("#idEnfermedad").val();
        var nombreEnfermedad = $("#idEnfermedad_chosen a span").html();

        $.ajax({
            url: $(this).attr("data-categoria-url") + "?idEnfermedad=" + idEnfermedad + "&nombreEnfermedad=" + nombreEnfermedad,
            cache: false,
            method: "GET"
        }).done(function (msg) {
            $("#dvCategorias").html(msg);
        });
    });
}

function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();

        var url = $(this).attr("href");

        $("#dialog-edit").dialog({
            title: "Crear Nueva Categoría",
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

function editExistingHandler() {

    $(document).on("click", ".editDialog", function () {

        var url = $(this).attr("href");

        $("#dialog-edit").dialog({
            title: "Editar Categoría existente",
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

                    $.ajax({
                        url: url,
                        cache: false,
                        method: "GET"
                    }).done(function (msg) {
                        $("#dvCategorias").html(msg);
                    });

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

        closeEditDialog("");

        return false;
    });
}

function editViewHandler() {

    var isEdit = $("#IsEdit").val();

    if (isEdit === "True") {

        var idEnfermedad = $("#CodigoEnfermedad").val();
        var nombreEnfermedad = $("#NombreEnfermedad").val();

        $("#idEnfermedad_chosen a").removeClass("chosen-default");
        $("#idEnfermedad_chosen a span").html(nombreEnfermedad);

        enfermedadHandler();

        $("#idEnfermedad").append("<option value=\""+ idEnfermedad + "\" selected=\"selected\">"+ nombreEnfermedad +"</option>");
        $("#idEnfermedad").val(idEnfermedad);
        $("#idEnfermedad").trigger("liszt:updated");
        $("#idEnfermedad").change();
    }
}

$(document).ready(function () {

    selectChosenConfig("idEnfermedad", "Seleccione la Enfermedad");

    enfermedadHandler();

    createNewHandler();

    editExistingHandler();

    confirmDeleteHandler();

    closeDialogHandler();

    editViewHandler();
});