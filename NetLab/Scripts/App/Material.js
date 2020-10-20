function onChangeTipoMuestra(urlPresentacionPorTipoMuestra, urlReactivoPorPresentacion) {

    var id = $("#TipoMuestra_IdTipoMuestra").val();

    $("#dvPresentacion").load(urlPresentacionPorTipoMuestra, { idTipoMuestra: id }, function () {
        onChangePresentacion(urlReactivoPorPresentacion);
    });
}

function onChangePresentacion(urlReactivoPorPresentacion) {

    var id = $("#Presentacion_IdPresentacion").val();

    $("#dvReactivo").load(urlReactivoPorPresentacion, { idPresentacion: id });
}

function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Nuevo Material",
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
                    agregarEventosPopupMaterial();
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
            title: "Editar Material existente",
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

    editExistingHandler();

    confirmDeleteHandler();
});






function agregarEventosPopupMaterial() {
    $("#idExamen").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Material/getTiposMuestras"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione un Tipo de Muestra", no_results_text: "No existen coincidencias" });

    $("#idPresentacion").chosen({ placeholder_text_single: "Seleccione Presentacion", no_results_text: "No existen coincidencias" });

    $("#idReactivo").chosen({ placeholder_text_single: "Seleccione Reactivo", no_results_text: "No existen coincidencias" });




    $("#dvPopupExamen").on("change", "#idExamen", function () {
        var idExamen = "";
        $("#idExamen :selected").each(function (i, selected) {

            idExamen = idExamen + "idExamen=" + $(selected).val() + "&";
        });
        $.ajax(
               {
                   url: URL_BASE + "Material/GetPresentacionesByMuestra?" + idExamen + "_=_",
                   cache: false,
                   method: "GET"
               }).done(function (msg) {
                   $("#dvPopupPresentacion").html(msg);
                   $("#idPresentacion").chosen({ placeholder_text_single: "Seleccione Presentacion", no_results_text: "No existen coincidencias" });
               }
               );

        $("#idPresentacion").chosen({ placeholder_text_single: "Seleccione Presentacion", no_results_text: "No existen coincidencias" });
    });


    $("#dvPopupPresentacion").on("change", "#idPresentacion", function () {
        var idTipoMuestra = "";
        $("#idPresentacion :selected").each(function (i, selected) {
           // idTipoMuestra = $(selected).val();
            idTipoMuestra = idTipoMuestra + "idTipoMuestra=" + $(selected).val() + "&";
        });
        $.ajax(
               {
                   url: URL_BASE + "Material/GetReactivosbyPresentacion?" + idTipoMuestra + "&_=_",
                   cache: false,
                   method: "GET"
               }).done(function (msg) {
                   $("#dvPopupReactivo").html(msg);
                   $("#idReactivo").chosen({ placeholder_text_single: "Seleccione el reactivo", no_results_text: "No existen coincidencias" });
               }
               );
        $("#idReactivo").chosen({ placeholder_text_single: "Seleccione el reactivo", no_results_text: "No existen coincidencias" });
    });


}





