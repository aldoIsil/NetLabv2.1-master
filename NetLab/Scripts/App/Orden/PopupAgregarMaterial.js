// Descripción: Funciones para agregar eventos y validciones correspondientes al popup de EnfermedadExamen.
//              Valída el formato de fecha y convierte el valor con el formato estandar.
//              Carga todos los eventos para la ventana de materiales.
// Author            : Terceros.
// Fecha Creación    : 01/01/2017.
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.

function agregarEventosPopupMaterial() {
    $("#idEnfermedadExamen").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Orden/GetEnfermedades"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" }
    ).change(function () {
        var idEnfermedadExamen = $("#idEnfermedadExamen").val();
        var idTipoMuestraMaterial = $("#idTipoMuestraMaterial").val();
        $("#idLaboratorio").ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "Orden/GetLaboratoriosForMaterial?idEnfermedadExamen=" + idEnfermedadExamen + "&idTipoMuestraMaterial=" + idTipoMuestraMaterial
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, {
            placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias"
        }).change(function () {

            //Se debe borrar el material
        });

    });

    $("#idLaboratorio").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Orden/GetLaboratorios"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, {
        placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "El examen no se pueden procesar en el laboratorio buscado"
    }).change(function () {
    });

    //Juan Muga - configuracion estará solo en setup.js
    $("input.datepickerTelerik:text").setDefaultDatePicker();
    //$("input.datepickerTelerik:text").kendoDatePicker({
    //    culture: "es-PE"
    //});
    var myDate = new Date();
    myDate.setMinutes(myDate.getMinutes() + 10);

    var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
    var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
    var anio = myDate.getFullYear();
    var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
    var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();
    $("#fechaEnvioPopup").val(dia + "/" + mes + "/" + anio);
    $("#horaEnvioPopup").timeEntry({ show24Hours: true });
    $("#horaEnvioPopup").val(hora + ":" + minutos);

    //Control Numeric para cantidad y Volumen de Material
    $("#cantidadPopup").kendoNumericTextBox({
        format: "0"
    });

    $("#volumenPopup").kendoNumericTextBox({
        format: "#.00 ml"
    });

    var numerictextbox = $("#volumenPopup").data("kendoNumericTextBox");
    numerictextbox.enable(true);

    $('#volumenNoPrecisaPopup').change(function () {
        if ($("#volumenNoPrecisaPopup").is(":checked")) {
            //$("#volumenPopup").prop('disabled', false);

            var numerictextbox = $("#volumenPopup").data("kendoNumericTextBox");
            numerictextbox.enable(false);

        }
        else {
            var numerictextbox = $("#volumenPopup").data("kendoNumericTextBox");
            numerictextbox.enable(true);
        }
    });

    $("#tipoMaterialPopup").chosen({ placeholder_text_single: "Seleccione el tipo de Material", no_results_text: "No existen coincidencias" });


    $("#idMaterial").chosen({ placeholder_text_single: "Seleccione el Material", no_results_text: "No existen coincidencias" });


    $("#cantidadPopup").kendoNumericTextBox({
        format: "0"
    });

    $("#volumenPopup").kendoNumericTextBox({
        format: "#.00 ml"
    });

    var numerictextbox = $("#volumenPopup").data("kendoNumericTextBox");
    numerictextbox.enable(true);

    $('#volumenNoPrecisaPopup').change(function () {
        if ($("#volumenNoPrecisaPopup").is(":checked")) {
            //$("#volumenPopup").prop('disabled', false);

            var numerictextbox = $("#volumenPopup").data("kendoNumericTextBox");
            numerictextbox.enable(false);

        }
        else {
            var numerictextbox = $("#volumenPopup").data("kendoNumericTextBox");
            numerictextbox.enable(true);
        }
    });


    $("#CerrarPopUp").on("click", function () {
        $("#dialog-open").dialog("close");
    });


    $("#btnAgregarMaterial").on("click", function () {

        //var nombreLaboratorioMuestra = $("#nombreLaboratorioMuestra").val();


        var idLaboratorio = $("#idLaboratorio").val();
        var idEnfermedadExamen = $("#idEnfermedadExamen").val();
        var idTipoMuestra = $("#idTipoMuestraMaterial").val();
        var idMaterial = $("#idMaterial").val();
        var volumenNoPrecisaPopup = $("#volumenNoPrecisaPopup").is(':checked');
        var volumenPopup = $("#volumenPopup").val();
        var cantidadPopup = $("#cantidadPopup").val();
        var tipoMaterialPopup = $("#tipoMaterialPopup").val();
        var fechaEnvio = $("#fechaEnvioPopup").val();
        var horaEnvio = $("#horaEnvioPopup").val();
        var tipoRegistro = $("#tipoRegistro").val();



        if (idEnfermedadExamen == undefined || idEnfermedadExamen == "" || idEnfermedadExamen == 0) {
            jAlert("Seleccione un Examen.", "Alerta!");
            return null;
        }

        if (idLaboratorio == undefined || idLaboratorio == "" || idLaboratorio == 0) {
            jAlert("Seleccione un Laboratorio.", "Alerta!");
            return null;
        }

        //Validacion de fecha y hora
        var resultado = ValidacionDeHora(fechaEnvio, horaEnvio);

        if (resultado === 1) {
            jAlert("Ingrese la Fecha de Envío", "¡Alerta!");
            return false;
        }

        if (resultado === 2) {
            jAlert("Ingrese una hora de Envío válida.", "Alerta!");
            return false;
        }

        resultado = ValidacionDeFecha(fechaEnvio, horaEnvio, 2);

        if (resultado === 1) {
            jAlert("La Fecha y hora de Envío no puede ser mayor a la Fecha Actual", "¡Alerta!");
            return false;
        }

        if (resultado === 2) {
            jAlert("La Fecha y hora de Envío no puede ser menor a la Fecha Actual", "¡Alerta!");
            return false;
        }

        if (resultado === 3) {
            jAlert("Ingrese una fecha de " + tipoFecha + " válida", "¡Alerta!");
            return false;
        }

        //Validacion de fecha y hora

        var datos = "?idTipoMuestra=" + idTipoMuestra + "&" +
        "idMaterial=" + idMaterial + "&" +
        "idLaboratorio=" + idLaboratorio + "&" +
        "idTipoMuestra=" + idTipoMuestra + "&" +
        "volumen=" + volumenPopup + "&" +
        "cantidad=" + cantidadPopup + "&" +
        "noPrecisaVolumen=" + volumenNoPrecisaPopup + "&" +
        "tipoMaterial=" + tipoMaterialPopup + "&" +
        "fechaEnvio=" + fechaEnvio + "&" +
        "horaEnvio=" + horaEnvio + "&" +
        "idEnfermedadExamen=" + idEnfermedadExamen
        dpUI.loading.start("Agregando ...");
        $("#dialog-open").dialog("close");
        $.ajax(
               {
                   url: URL_BASE + "Orden/AddMaterial" + datos + "&_" + (new Date()).getTime(),
                   cache: false,
                   method: "GET"
               }).done(function (msg) {


                   msg = "<div id='id-1'>" + msg + "</div>"

                   var tbodyMaterial = "";
                   if (tipoRegistro == "0") //solo orden
                   {
                       var tbodyMaterial = $(msg).find("#tbodyMaterial").html();
                       //debugger;
                       if (tbodyMaterial.length > 10) {
                           //Material
                           $('#tableMaterial').append(tbodyMaterial);
                           var idOrdenMaterialTest = $(msg).find("#idOrdenMaterialTest").val();
                           //Juan Muga - configuracion estará solo en setup.js
                           $("#fechaEnvio" + idOrdenMaterialTest).setDatePicker();
                           //$("#fechaEnvio" + idOrdenMaterialTest).kendoDatePicker({ culture: "es-PE" });
                           $("#horaEnvio" + idOrdenMaterialTest).timeEntry({ show24Hours: true });
                           $("#cantidad" + idOrdenMaterialTest).kendoNumericTextBox({ format: "0" });
                           $("#volMuestra" + idOrdenMaterialTest).kendoNumericTextBox({ format: "#.00 ml" });
                           $("#laboratorio" + idOrdenMaterialTest).each(function () {
                               $(this).ajaxChosen({
                                   dataType: "json",
                                   type: "POST",
                                   minTermLength: 3,
                                   afterTypeDelay: 300,
                                   cache: false,
                                   url: URL_BASE + "OrdenMuestra/GetAllLaboratorios"
                               }, {
                                   loadingImg: URL_BASE + "Content/images/loading.gif"
                               }, { placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias" }
                              );
                           });

                           agregarEventosTablaMaterial();
                       }

                   }
                   else {
                       var tbodyMaterial = $(msg).find("#idTableRecepcionar").html();
                       tbodyMaterial = "<table class='table' width='1200'>" + tbodyMaterial + "</table>";
                       //debugger;
                       if (tbodyMaterial.length > 10) {
                           $('#dvTblMaterial').append(tbodyMaterial);
                           var idOrdenMaterialTest = $(msg).find("#idOrdenMaterialTest").val();
                           //Juan Muga - configuracion estará solo en setup.js
                           $("#fechaEnvio" + idOrdenMaterialTest).setDatePicker();
                           //$("#fechaEnvio" + idOrdenMaterialTest).kendoDatePicker({ culture: "es-PE" });
                           $("#horaEnvio" + idOrdenMaterialTest).timeEntry({ show24Hours: true });
                           $("#volMuestra" + idOrdenMaterialTest).kendoNumericTextBox({ format: "#.00 ml" });
                           $("#laboratorio" + idOrdenMaterialTest).each(function () {
                               $(this).ajaxChosen({
                                   dataType: "json",
                                   type: "POST",
                                   minTermLength: 3,
                                   afterTypeDelay: 300,
                                   cache: false,
                                   url: URL_BASE + "OrdenMuestra/GetAllLaboratorios"
                               }, {
                                   loadingImg: URL_BASE + "Content/images/loading.gif"
                               }, { placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias" }
                              );
                           });
                           agregarEventosTablaMaterial();
                       }
                   }
               }
               ).fail(function () {
                   jAlert("Se produjo un error de Conexión.", "Error")
               }).complete(function () {
                   dpUI.loading.stop();
               })
    });

}

