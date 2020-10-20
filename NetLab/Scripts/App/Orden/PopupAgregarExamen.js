// Descripción: Funciones para agregar eventos y validciones correspondientes al popup de EnfermedadExamen.
//              Valída el formato de numeros enteros
//              Obtiene todos los labortorios, tipos de muestra de acuerdo al examen seleccionado
//              Inicaliza los combo box de los laboratorios.
//              Muestra el popup para seleccionar y agregar la enfermedad y examen.
//              Inicializa y valida los campos para el registro de un examen,tipo de muestra y materiales.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.

function agregarEventosPopupEnfermedadExamen(x) {
    //debugger;

    var paciente_genero = $("#Paciente_Genero").val();
    var idEnfermedad = '';
    if (x != '') {
        paciente_genero = 3;
    }
    $("#idEnfermedad").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Orden/GetEnfermedades"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias con el texto" }).
        change(function () {
            idEnfermedad = $("#idEnfermedad").val()
            if (idEnfermedad.length > 0) {
                $("#idExamen").ajaxChosen({
                    dataType: "json",
                    type: "GET",
                    minTermLength: 3,
                    afterTypeDelay: 300,
                    cache: false,
                    url: URL_BASE + "Orden/GetExamenes?genero=" + paciente_genero + "&idenfermedad=" + idEnfermedad
                }, {
                    loadingImg: URL_BASE + "Content/images/loading.gif"
                }, {
                    placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias con el texto"
                });
            }

        });
    if (idEnfermedad == '') {
        idEnfermedad = $("#idEnfermedad").val()
    }

    var idvalidador = $("#idPage").val();

    $("#idLaboratorio").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Orden/GetLaboratorios?ExamenVA=" + idvalidador
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, {
        placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias con el texto"
    }).change(function () {
        $("#idExamen").ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "Orden/GetExamenes?genero=" + paciente_genero + "&idenfermedad=" + idEnfermedad
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, {
            placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias con el texto"
        });
    });
    //Cambio Juan Muga

    $("#CodigoUnicoDestino").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Orden/GetLaboratoriosNew?prefix=" + request.term + "&ExamenVA=" + idvalidador,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Nombre, value: item.IdEstablecimiento };
                    }))
                }
            })
        },
        select: function (e, i) {
            e.preventDefault();
            $("#idEstablecimientoDestino").val(i.item.value);
            $("#CodigoUnicoDestino").val(i.item.label);
            $("#NombreEESSDestino").val(i.item.label);
            //
            if (idEnfermedad.length > 0) {
                $("#idExamen").ajaxChosen({
                    dataType: "json",
                    type: "POST",
                    minTermLength: 3,
                    afterTypeDelay: 300,
                    cache: false,
                    url: URL_BASE + "Orden/GetExamenes?genero=" + paciente_genero + "&idenfermedad=" + idEnfermedad
                }, {
                    loadingImg: URL_BASE + "Content/images/loading.gif"
                }, {
                    placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias con el texto"
                });
            }
        },
        minLength: 2
    });
    $("#CodigoUnicoDestino").click(function () {
        $("#CodigoUnicoDestino").val("");
        $("#idEstablecimientoDestino").val("");
        $("#NombreEESSDestino").val("");
    });

    if (idEnfermedad.length > 0) {
        $("#idExamen").ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "Orden/GetExamenes?genero=" + paciente_genero + "&idenfermedad=" + idEnfermedad
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, {
            placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias con el texto"
        });
    }
    //
    var idLaboratorio = $("#idLaboratorioMuestra").val();

    $("#idExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias con el texto" });
    $("#idTipoMuestra").chosen({ placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias con el texto" });
    //JRCR-REQ01
    //$("#idMaterial").chosen({ placeholder_text_single: "Seleccione el Material", no_results_text: "No existen coincidencias" });

    //dvPopupLaboratorio
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
                   $("#idTipoMuestra").chosen({ placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias con el texto" });
               }
               );
    });


    $("#dvPopupTipoMuestra").on("change", "#idTipoMuestra", function () {
        var idTipoMuestra = "";
        $("#idTipoMuestra :selected").each(function (i, selected) {
            idTipoMuestra = $(selected).val();
        });
        $.ajax(
               {
                   url: URL_BASE + "Orden/GetMaterialByTiposMuestra?idTipoMuestra=" + idTipoMuestra + "&_=_",
                   cache: false,
                   method: "GET"
               }).done(function (msg) {
                   //debugger;
                   //console.log("done Orden/GetMaterialByTiposMuestra");
                   //console.log(msg);
                   $("#dvPopupMaterial").html(msg);
                   $("#idMaterial").val($("#idMaterialDefecto").val());
                   //$("#idMaterial").chosen({ placeholder_text_single: "Seleccione el Material", no_results_text: "No existen coincidencias" });
               }
               );
    });


    //JRCR-REQ01
    //$("#tipoMaterialPopup").chosen({ placeholder_text_single: "Seleccione el tipo de Material", no_results_text: "No existen coincidencias" });
    //Juan Muga - configuracion estará solo en setup.js
    //$("input.datepickerTelerik:text").kendoDatePicker({
    //    culture: "es-PE"
    //});
    $("input.datepickerTelerik:text").setDefaultDatePicker();
    var myDate = new Date();
    myDate.setMinutes(myDate.getMinutes() + 10);

    var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
    var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
    var anio = myDate.getFullYear();
    var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
    var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();
    //Juan Muga
    //$("#fechaEnvioPopup").val(dia + "/" + mes + "/" + anio);
    //$("#horaEnvioPopup").timeEntry({ show24Hours: true });
    //$("#horaEnvioPopup").val(hora + ":" + minutos);
    var myDateColecion = new Date();
    var horaColeccion = (myDateColecion.getHours() >= 10) ? myDateColecion.getHours() : "0" + myDateColecion.getHours();
    var minColeccion = (myDateColecion.getMinutes() >= 10) ? myDateColecion.getMinutes() : "0" + myDateColecion.getMinutes();

    $("#fechaColeccionPopup").val(dia + "/" + mes + "/" + anio);
    $("#horaColeccionPopup").timeEntry({ show24Hours: true });
    $("#horaColeccionPopup").val(horaColeccion + ":" + minColeccion);
    //JRCR-REQ01
    //Control Numeric para cantidad y Volumen de Material
    //$("#cantidadPopup").kendoNumericTextBox({
    //    format: "0"
    //});

    //$("#volumenPopup").kendoNumericTextBox({
    //    format: "#.00 ml"
    //});
    //var numerictextbox = $("#volumenPopup").data("kendoNumericTextBox");
    //numerictextbox.enable(true);
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
    $("#idEstablecimientoMuestra").val("1").trigger("liszt:updated");
    //Agregar Examen a la Orden 
    $("#btnAgregarExamen").on("click", function () {
        //debugger;
        //var idLaboratorio = $("#idLaboratorio").val();
        $("#validacionexamenpacientedivid").hide();
        var idLaboratorio = $("#idEstablecimientoDestino").val();
        var idEnfermedad = $("#idEnfermedad").val();
        var textEnfermedad = $("#idEnfermedad option:selected").text();
        var idExamen = $("#idExamen").val();
        var idTipoMuestra = $("#idTipoMuestra").val();
        var fechaColeccion = $("#fechaColeccionPopup").val();
        var horaColeccion = $("#horaColeccionPopup").val();
        var idMaterial = $("#idMaterial").val();
        var volumenNoPrecisaPopup = $("#volumenNoPrecisaPopup").is(':checked');
        var volumenPopup = $("#volumenPopup").val();
        var cantidadPopup = $("#cantidadPopup").val();
        var tipoMaterialPopup = $("#tipoMaterialPopup").val();

        // SOTERO BUSTAMANTE
        var idOrden = x;
        var idProyecto = $("#Proyectoval").val();

        //muga
        //$("#idEnfermedad").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
        var fechaEnvio = '01/01/2018';
        var horaEnvio = '10:00';
        var tipoRegistro = $("#tipoRegistro").val();

        if (idLaboratorio == undefined || idLaboratorio == "" || idLaboratorio == 0) {
            jAlert("Seleccione un Laboratorio.", "Alerta!");
            return null;
        }
        else if (idEnfermedad == undefined || idEnfermedad == "" || idEnfermedad == 0) {
            jAlert("Seleccione una Enfermedad.", "Alerta!");
            return null;
        }
        else if (idEnfermedad == 1005644) {
            var sexo = $("#idSexo").val();
            if (sexo == 1) {
                jAlert("Enfermedad no relacionada con el sexo del Paciente", "Alerta!");
                return null;
            }
        }
        else if (idExamen == undefined || idExamen == "" || idExamen == 0 || idExamen == "00000000-0000-0000-0000-000000000000") {
            jAlert("Seleccione un Examen.", "Alerta!");
            return null;
        }
        else if (idTipoMuestra == undefined || idTipoMuestra == "" || idTipoMuestra == 0) {
            jAlert("Seleccione un Tipo de Muestra.", "Alerta!");
            return null;
        }
            //Juan Muga
            //else if (idMaterial == undefined || idMaterial == "" || idMaterial == 0) {
            //    jAlert("Seleccione un Material.", "Alerta!");
            //    return null;
            //}
        else if (horaColeccion == "") {
            jAlert("Ingrese una hora de obtención válida.", "Alerta!");
            return null;
        }

        var iHourDate = horaColeccion.split(":");

        if (iHourDate.length != 2) {
            jAlert("Ingrese una hora de obtención válida.", "Alerta!");
            return null;
        }

        if ((fechaColeccion.length) === 0) {
            jAlert("Ingrese una fecha de obtención válida", "¡Alerta!");
            return null;
        }

        var today = new Date().getTime();
        var idate = fechaColeccion.split("/");

        idate = new Date(idate[2], idate[1] - 1, idate[0], iHourDate[0], iHourDate[1], '00');
        if ((today - idate) < 0) {
            jAlert("La fecha y hora de obtención no puede ser mayor a la fecha y hora Actual", "¡Alerta!");
            return null;
        }
        //Juan Muga
        //if ((fechaEnvio.length) === 0) {
        //    jAlert("Ingrese una fecha de envío válida", "¡Alerta!");
        //    return null;
        //}

        //if (horaEnvio == "") {
        //    jAlert("Ingrese una hora de envío válida.", "Alerta!");
        //    return null;
        //}

        //iHourDate = horaEnvio.split(":");

        //if (iHourDate.length != 2) {
        //    jAlert("Ingrese una hora de envío válida.", "Alerta!");
        //    return null;
        //}

        //idate = fechaEnvio.split("/");
        //idate = new Date(idate[2], idate[1] - 1, idate[0], iHourDate[0], iHourDate[1], '00');
        //if ((idate - today) < 0) {
        //    jAlert("La fecha y hora de envío no puede ser menor a la fecha y hora ctual", "¡Alerta!");
        //    return null;
        //}
        //sotero cambio 11/2017
        var idPage = "";
        //debugger;
        if ($("#idPage").val() == undefined) {
            idPage = "";
        }
        else {
            idPage = $("#idPage").val();
        }
        
        if (idPage == "Validador"){

            var data = idOrden + '|' +
                        idLaboratorio + "|" +
                        idEnfermedad + "|" +
                        idExamen + "|" +
                        idTipoMuestra + "|" +
                        fechaColeccion + "|" +
                        horaColeccion + "|" +
                        idMaterial + "|" +
                        Number(volumenNoPrecisaPopup) + "|" +
                        volumenPopup + "|" +
                        cantidadPopup + "|" +
                        tipoMaterialPopup
            //Juan Muga
            //
            //"fechaEnvio=" + fechaEnvio + "&" +
            //"horaEnvio=" + horaEnvio  
            var datos = "?datos=" + data;

            $("#dialog-open").dialog("close");
            /*Agrega el examen*/
            $.ajax(
                {
                    url: URL_BASE + "Orden/AddExamenVerificador" + datos,
                    cache: false,
                    method: "GET"
                });
        }
        else if(idPage == "RomINS")
            {
            var data = idOrden + '|' +
                        idLaboratorio + "|" +
                        idEnfermedad + "|" +
                        idExamen + "|" +
                        idTipoMuestra + "|" +
                        fechaColeccion + "|" +
                        horaColeccion + "|" +
                        idMaterial + "|" +
                        Number(volumenNoPrecisaPopup) + "|" +
                        volumenPopup + "|" +
                        cantidadPopup + "|" +
                        tipoMaterialPopup
            //Juan Muga
            //
            //"fechaEnvio=" + fechaEnvio + "&" +
            //"horaEnvio=" + horaEnvio  
            var datos = "?datos=" + data;

            $("#dialog-open").dialog("close");
            /*Agrega el examen*/
            $.ajax(
                {
                    url: URL_BASE + "Orden/AddExamenRomINS" + datos,
                    cache: false,
                    method: "GET"
                });
            
        }
        else {
            var datos = "?idLaboratorio=" + idLaboratorio + "&" +
                "idEnfermedad=" + idEnfermedad + "&" +
                "idExamen=" + idExamen + "&" +
                "idTipoMuestra=" + idTipoMuestra + "&" +
                "fechaColeccion=" + fechaColeccion + "&" +
                "horaColeccion=" + horaColeccion + "&" +
                "idMaterial=" + idMaterial + "&" +
                "fechaEnvio=" + fechaEnvio + "&" +//juan muga
                //juan muga
                "fechaEnvio= &" +
                "noPrecisaVolumen=" + volumenNoPrecisaPopup + "&" +
                "volumen=" + volumenPopup + "&" +
                "cantidad=" + cantidadPopup + "&" +
                "tipoMaterial=" + tipoMaterialPopup + "&" +
                //Juan Muga
                //
                "fechaEnvio=" + fechaEnvio + "&" +
                "horaEnvio=" + horaEnvio + "&IdProyecto=" + idProyecto

            console.log(datos);

            //validar y en el success poner todo lo demas
            $.ajax({
                url: URL_BASE + "Orden/ValidarExamen",
                type: 'GET',
                contentType: 'application/json',
                dataType: "json",
                data: { idPaciente: $("#idPaciente").val(), idLaboratorio: idLaboratorio, idExamen: idExamen, fechaColeccion: fechaColeccion },
                success: function (respdatos) {
                    console.log("existe mismo examen de paciente: ", respdatos);
                    if (respdatos== '1') {
                        // alert
                        $("#validacionexamenpacientedivid").show();
                    }
                    else if (respdatos == '2') {
                        $("#validacionexamenpacientevih").show();
                    }
                    else {
                        //////
                    
                        dpUI.loading.start("Agregando ...");
                        $("#dialog-open").dialog("close");

                        /*Agrega el examen*/
                        $.ajax({
                            url: URL_BASE + "Orden/AddExamen" + datos,
                            cache: false,
                            method: "GET"
                        }).done(function (msg) {
                                if (msg.indexOf("<div>NoAgregado</div>") >= 0) {
                                    jAlert("Los datos seleccionados ya fueron agregados, por favor verificar...", "¡Información!");
                                }
                                else {
                                    //Juan Muga
                                    debugger;
                                    msg = "<div id='id-1'>" + msg + "</div>"

                                    var tbodyExamen = $(msg).find("#tbodyExamen").html();
                                    var tbodyOrdenMuestra = $(msg).find("#tbodyOrdenMuestra").html();

                                    var tbodyMaterial = "";
                                    //Juan Muga
                                    if (tipoRegistro == "0" || tipoRegistro == "2") //solo orden
                                    {
                                        var tbodyMaterial = $(msg).find("#tbodyMaterial").html();
                                    }
                                    else {
                                        var tbodyMaterial = $(msg).find("#idTableRecepcionar").html();
                                        $(".emptytablemr").remove();
                                        tbodyMaterial = "<table class='table'>" + tbodyMaterial + "</table>";
                                    }

                                    if (tbodyExamen.length > 20) {

                                        //Examen
                                        $('#tableExamen').append(tbodyExamen);
                                    }


                                    if (tbodyOrdenMuestra.length > 20) {
                                        //Orden Muestrawd
                                        $('#tableOrdenMuestra').append(tbodyOrdenMuestra);
                                        //Juan Muga
                                        var idMuestra = $(msg).find("#idMuestra").val();
                                        $("#fechaMuestra" + idMuestra).kendoDatePicker({ culture: "es-PE" });
                                        $("#horaMuestra" + idMuestra).timeEntry({ show24Hours: true });
                                        agregarEventosOrdenMuestra();
                                    }

                                    //Juan Muga - inicio
                                    if (tipoRegistro == "0" || tipoRegistro == "2") //solo orden
                                    {
                                        //    debugger;
                                        $("#idTableRecepcionar").remove();
                                        if (tbodyMaterial.length > 10) {
                                            //Material
                                            $('#tableMaterial').append(tbodyMaterial);
                                            var idOrdenMaterialTest = $(msg).find("#idOrdenMaterialTest").val();
                                            //$("#fechaEnvio" + idOrdenMaterialTest).kendoDatePicker({ culture: "es-PE" });
                                            //$("#horaEnvio" + idOrdenMaterialTest).timeEntry({ show24Hours: true });
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
                                                }, { placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias con el texto" }
                                                );
                                            });

                                            agregarEventosTablaMaterial();
                                        }
                                    }
                                    else {
                                        if (tbodyMaterial.length > 10) {
                                            $('#dvTblMaterial').append(tbodyMaterial);
                                            var idOrdenMaterialTest = $(msg).find("#idOrdenMaterialTest").val();
                                            $("#fechaEnvio" + idOrdenMaterialTest).kendoDatePicker({ culture: "es-PE" });
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
                                                }, { placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias con el texto" }
                                                );
                                            });


                                            agregarEventosTablaMaterial();
                                        }
                                    }
                                    //Juan Muga - fin
                                    jQuery('#tableExamen').trigger("create") //.addClass('table');
                                    jQuery('#tableOrdenMuestra').trigger("create") //.addClass('table');
                                    jQuery('#tableMaterial').trigger("create") //.addClass('table');


                                    //agregarExamen(msg);

                                    if ($("#tabs-" + idEnfermedad).length == 0) {
                                        $("#listaEnfermedad").append("<li><a id='link-" + idEnfermedad + "' href='#tabs-" + idEnfermedad + "'>" + textEnfermedad + "</a></li>")
                                        //Agregar Datos Clinico
                                        $.ajax(
                                            {
                                                url: URL_BASE + "Orden/GetDatosClinicos?idEnfermedad=" + idEnfermedad,
                                                cache: false,
                                                method: "GET"
                                            }).done(function (msg2) {

                                                $("#tabs").append("<div id='tabs-" + idEnfermedad + "'>" + msg2 + "</div>");
                                                //$("div#tabs").tabs("refresh");


                                                var tabId = $("#tabs").tabs("option", "active");
                                                $("#tabs").tabs("option", "active", tabId);
                                                //$("#tabs").tabs("load", tabId);
                                                $(".selector").tabs("enable");
                                                $("#tabs").tabs("refresh");

                                                //         $("#tabs").tabs("option", "active", tabId); with this $("#tabs").tabs("load", tabId);

                                                $("#link-" + idEnfermedad).trigger("click");


                                                AgregarEventosDatosClinicos();
                                                //$("#dvTblDatoClinico").html(msg2);
                                            });
                                    }
                                }
                        }).fail(function (ef) {
                                jAlert("Se produjo un error de Conexión.", "Error");
                                console.log(ef);
                        }).success(function () {
                                dpUI.loading.stop();
                        });

                        ////////
                    }
                },
                error: function (jqXHR, textStatus, exception) {
                    console.log(textStatus, exception);
                }
            });
        }
    });
}
