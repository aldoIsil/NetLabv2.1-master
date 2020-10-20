$(document).ready(function () {

    establecimientoChosenConfig("cmbEstablecimiento");

    $("#btnShowPopupEnfermedadExamen").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href") + "?_=" + (new Date()).getTime();
        $("#dialog-open").html("");
        $("#dialog-open").dialog({
            title: "Agregar Examen",
            autoOpen: false,
            resizable: false,
            height: 380,
            width: 450,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    asignarEventosPopupEnfermedadExamen();
                });

            },
            close: function () {
                $(this).dialog("close");
            }
        });
        $("#dialog-open").dialog("open");
        return false;
    });
});


function asignarEventosPopupEnfermedadExamen() {

    $("#CerrarPopUp").on("click", function() {

        $(this).closest(".ui-dialog-content").dialog("close");
    });

    $("#dvTblExamen").on("click", ".eliminarExamen", function () {

        var idExamen = $(this).attr("id");
        var url = $(this).attr("data-url");

        $.ajax({
            url: url + "?idExamen=" + idExamen,
            cache: false,
            method: "GET"
            }).done(function (msg) {
                resultadoEliminarExamen(msg);
            }
        );
    });


    $("#idExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
    $("#idTipoMuestra").chosen({ placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias" });

    $("#idEnfermedad").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Orden/GetEnfermedades"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" });



    $("#idEnfermedad").on("change", function () {
        var idEnfermedad = $("#idEnfermedad").val();
        $.ajax(
           {
               url: URL_BASE + "Orden/GetExamenes?idEnfermedad=" + idEnfermedad,
               cache: false,
               method: "GET"
           }).done(function (msg) {
               $("#dvPopupExamen").html(msg);
               $("#idExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
           }
           );
    });


    $("#dvPopupExamen").on("change", "#idExamen", function () {
        var idExamen = "";
        $("#idExamen :selected").each(function (i, selected) {

            idExamen = idExamen + "idExamen=" + $(selected).val() + "&";
        });
        $.ajax(
               {
                   url: URL_BASE + "Orden/GetTiposMuestraByIdExamen?" + idExamen + "_=_",
                   cache: false,
                   method: "GET"
               }).done(function (msg) {
                   $("#dvPopupTipoMuestra").html(msg);
                   $("#idTipoMuestra").chosen({ placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias" });
               }
               );
    });


    $(".datepicker").datepicker();
    var myDate = new Date();
    var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
    var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
    var anio = myDate.getFullYear();
    var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
    var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();
    $(".datepicker").val(dia + "/" + mes + "/" + anio);
    $(".timepicker").timeEntry({ show24Hours: true, spinnerImage: "" });
    $(".timepicker").val(hora + ":" + minutos);


    $("#btnAgregarExamen").on("click", function () {
        var idEnfermedad = $("#idEnfermedad").val();
        var idExamen = $("#idExamen").val();
        var idTipoMuestra = $("#idTipoMuestra").val();
        var fechaColeccion = $("#fechaColeccion").val();
        var horaColeccion = $("#horaColeccion").val();
        var datos = "?idEnfermedad=" + idEnfermedad + "&" +
            "idExamen=" + idExamen + "&" +
            "idTipoMuestra=" + idTipoMuestra + "&" +
            "fechaColeccion=" + fechaColeccion + "&" +
            "horaColeccion=" + horaColeccion;
        $.ajax(
               {
                   url: URL_BASE + "Orden/AddExamen" + datos,
                   cache: false,
                   method: "GET"
               }).done(function (msg) {

                   $("#dialog-open").dialog("close");
                   $("#dvTblExamen").html(msg);
               }
               );
        $.ajax(
            {
                url: URL_BASE + "Orden/AddOrdenMuestra" + datos,
                cache: false,
                method: "GET"
            }).done(function (msg) {
                agregarOrdenMuestra(msg, fechaColeccion, horaColeccion);
            }
            );
    });
}

function agregarOrdenMuestra(msg, fechaColeccion, horaColeccion) {

    $("#dvTblOrdenMuestra").html(msg);

    //$(".datepicker").datepicker();
    $(".datepicker").val(fechaColeccion);
    //$(".timepicker").timeEntry({ show24Hours: true });
    $(".timepicker").val(horaColeccion);


    $(".btnShowPopupMaterial").on("click", function (e) {
        e.preventDefault();
        var idTipoMuestra = $(this).parent().attr("class");
        var url = $(this).attr("href") + "?idTipoMuestra=" + idTipoMuestra + "&_=" + (new Date()).getTime();
        $("#dialog-open").html("");
        $("#dialog-open").dialog({
            title: "Agregar Material",
            autoOpen: false,
            resizable: false,
            height: 380,
            width: 450,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    asignarEventosPopupMaterial();
                });

            },
            close: function () {
                $(this).dialog("close");
            }
        });
        $("#dialog-open").dialog("open");
        return false;
    });
}

function asignarEventosPopupMaterial() {

    $("#CerrarPopUp").on("click", function () {

        $(this).closest(".ui-dialog-content").dialog("close");
    });

    $("#idMaterial").chosen({ placeholder_text_single: "Seleccione el tipo de Material", no_results_text: "No existen coincidencias" });
    $("#btnAgregarMaterial").on("click", function () {
        var idTipoMuestra = $("#idTipoMuestraMaterial").val();
        var nombreTipoMuestra = $("#nombreTipoMuestra").val();
        var idMaterial = $("#idMaterial").val();
        var cantidad = $("#cantidad").val();
        var datos = "?idTipoMuestra=" + idTipoMuestra + "&" +
            "nombreTipoMuestra=" + nombreTipoMuestra + "&" +
            "idMaterial=" + idMaterial + "&" +
            "cantidad=" + cantidad;
        $.ajax(
               {
                   url: URL_BASE + "Orden/AddMaterial" + datos + "&_" + (new Date()).getTime(),
                   cache: false,
                   method: "GET"
               }).done(function (msg) {

                   $("#dialog-open").dialog("close");
                   $("#dvTblMaterial").html(msg);
                   agregarEventosTablaMaterial();
               }
               );
    });
}

function agregarEventosTablaMaterial() {
    $(".eliminarMaterial").on("click", function () {
        var idMaterial = $(this).parent().attr("class");
        $.ajax(
          {
              url: URL_BASE + "Orden/DelMaterial?idMaterial=" + idMaterial,
              cache: false,
              method: "GET"
          }).done(function (msg) {
              $("#dialog-open").dialog("close");
              $("#dvTblMaterial").html(msg);
          }
          );
    });
}