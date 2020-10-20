function agregarEventosOrdenMuestra() {
    $(".btnShowPopupMaterial").on("click", function (e, params) {
        e.preventDefault();
        var idTipoMuestra = $(this).parent().attr("class");
        var url = $(this).attr("href") + "?idTipoMuestra=" + idTipoMuestra + "&_=" + (new Date()).getTime();
        $("#dialog-open").html("");
        $("#dialog-open").dialog({
            title: "Agregar Material",
            autoOpen: false,
            resizable: false,
            height: 340,
            width: 500,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregarEventosPopupMaterial();
                });

            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });
        $("#dialog-open").dialog("open");
        return false;
    });
}

function agregarEventosOrdenSolicitante() {
    $("#btnShowPopupSolicitante").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-open").dialog({
            title: "Nuevo Solicitante",
            autoOpen: false,
            resizable: true,
            height: 360,
            width: 500,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregarEventosPopupEnfermedadExamen();
                });
            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });
        $("#dialog-open").dialog("open");
        return false;
    });
}
/*Se dispara cuando se hace clic en el boton eliminar en la tabla examen*/
function definirEventoEliminarExamen() {
    $("#dvTblExamen").on("click", ".eliminarExamen", function () {
        //debugger;
        var idExamen = $(this).attr("id");
        var idEnfermedad = $(this).parent().attr("class");
        var idTipoMuestra = $(this).parent().attr("id");
        $.ajax(
            {
                url: URL_BASE + "Orden/DelExamen?idExamen=" + idExamen + "&idEnfermedad=" + idEnfermedad + "&idTipoMuestra=" + idTipoMuestra,
                cache: false,
                method: "GET"
            }).done(function (msg) {

                $("#dvTblExamen").html("");
                $("#dvTblOrdenMuestra").html("");
                $("#dvTblMaterial").html("");

                $("#divTblMultiple").html(msg);

                //Se agrega Examen
                $("#dvTblExamen").html($("#dvTblExamenSubMultiple").html());

                //Se agrega Orden Muestra
                $("#dvTblOrdenMuestra").html($("#dvTblOrdenMuestraSubMultiple").html());
                agregarEventosOrdenMuestra();



                //Se Agrega Material
                $("#dvTblMaterial").html($("#dvTblMaterialSubMultiple").html());

                //Juan Muga - configuracion estará solo en setup.js
                //$(".datepickerMaxValue").setDefaultDatePicker();
                //$("input.datepickerTelerik:text").kendoDatePicker({ culture: "es-PE" });
                $(".timepicker").timeEntry({ show24Hours: true });
                $(".cantidad").kendoNumericTextBox({ format: "0" });
                $(".volumen").kendoNumericTextBox({ format: "#.00 ml" });

                agregarEventosTablaMaterial();

                agregarEventosOrdenSolicitante()
                //Se agrega eventos para todos

                //volMuestra




                $("#divTblMultiple").html("");


                var eliminarEnfermedad = $("<div>" + msg + "</div>").find("#eliminarEnfermedad").val();
                if (eliminarEnfermedad == 0) {
                    $("#link-" + idEnfermedad).parent().remove().attr("aria-controls");
                    $("#tabs-" + idEnfermedad).remove();
                    $("#tabs").tabs("refresh");
                }
            }
            );
        return false;
    });
}
function getBool(val) {
    var num = +val;
    return !isNaN(num) ? !!num : !!String(val).toLowerCase().replace(!!0, '');
}
$(document).ready(function () {
   
    var textoRegistro = $("#textoRegistro").val();
    //var existeError = $("#existeError").val();
    var esPruebaRapida = $("#esPruebaRapida").val();

    //if (existeError == "S") {
    //    if (textoRegistro != "")
    //    jAlert(textoRegistro, "¡Información!")
    //}
    if (isNaN(esPruebaRapida) || esPruebaRapida == undefined) {
        if (textoRegistro != "")
            jAlert(textoRegistro, "¡Información!");
    }
    if (textoRegistro != "")
        jAlert(textoRegistro, "¡Información!")
    /*Se agregan eventos a todos los controles*/
    //Juan Muga - configuracion estará solo en setup.js
    //debugger;
    $(".datepickerMaxValue").setDatePickerWithMaxValue();

    //$("input.datepickerTelerik:text").kendoDatePicker({
    //    culture: "es-PE"
    //});
    $(".timepicker").timeEntry({ show24Hours: true });


    //Se agregan eventos para materiales
    $(".cantidad").kendoNumericTextBox({
        format: "0"
    });

    $(".volumen").kendoNumericTextBox({
        format: "#.00 ml"
    });

    //mostrarConfirmacion();

    agregarEventosOrdenMuestra();
    definirEventoEliminarExamen();
    agregarEventosTablaMaterial();

    $("#idEstablecimientoFrecuente").on("change", function (e) {
        //$('#idEstablecimiento').find('option:first-child').prop('selected', true).end().trigger('chosen:updated');
        $('#hddDatoEESSOrigen').val("");
        $('#CodigoUnicoOrigen').val("");
    });

    $("#idEstablecimiento").on("change", function (e) {
        $("#idEstablecimientoFrecuente option:selected").prop("selected", false);
    });
    $("#idEstablecimiento").chosen({ placeholder_text_single: "Seleccione el Establecimiento", no_results_text: "No existen coincidencias" });

    $("#idEstablecimiento").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 1000,
        cache: false,
        url: URL_BASE + "Orden/GetEstablecimientos?dato=0"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" });

    //$(document).tooltip();
    SeguroSaludSetUp();
    AgregarEventosDatosClinicos();


    //Se agrega tabs  para las enfermedades
    $(function () {
        $("#tabs").tabs();
    });


    //INICIO Control para modificar datos de Paciente
    //Se oculta la seccion de ubigeoActualEditable
    //$("#_ubigeoActualEditable").hide();

    $('#chkActualizarUbicacion').val($(this).is(':checked'));

    $('#chkActualizarUbicacion').change(function () {
        if ($(this).is(":checked")) {
            $("#_ubigeoActualNoEditable").hide();
            $("#_ubigeoActualEditable").show();
        }
        else {
            $("#_ubigeoActualNoEditable").show();
            $("#_ubigeoActualEditable").hide();
        }
        $('#chkActualizarUbicacion').val($(this).is(':checked'));
    });

    //FIN Control para modificar datos de Paciente




    ///INICIO Evento Guardar Orden
    $("#btnGuardar").on("click", function (e) {
        //debugger;
        e.preventDefault();
        var procedencia =[];
        //$(".id").parent("tr").find(".id").each(function () {
        //    procedencia += $(this).html() + '|';
        //});
        //console.log("procedencia:", procedencia);
        //console.log("JSON.stringify(procedencia):", JSON.stringify(procedencia));
        //$('#procPaciente').val(JSON.stringify(procedencia));

        var ok = true;
        //Juan Muga - inicio - validar fecha manual
        var val_date = $('#fechaSolicitud').val();
        ok = ValidarFecha(val_date);
        //Juan Muga - fin

        var idProyecto = $("#IdProyecto").val();
        //var idEstablecimiento = $("#idEstablecimiento").val();
        var idEstablecimiento = $("#hddDatoEESSOrigen").val();
        var valcodigoOrden = $('#codigoOrden').val();

        var textoEstablecimiento = "";

        if (valcodigoOrden == "" && (idEstablecimiento == undefined || idEstablecimiento == "")) {
            idEstablecimiento = $("#idEstablecimientoFrecuente").val();

            if (idEstablecimiento == undefined || idEstablecimiento == "" || idEstablecimiento == 0) {
                jAlert("Seleccione un establecimiento.", "¡Alerta!");
                ok = false;
            }
            else {
                //textoEstablecimiento = $("#idEstablecimientoFrecuente").text();
                textoEstablecimiento = $("#idEstablecimientoFrecuente option[value='" + idEstablecimiento + "']").text()
            }
        }
        else {
            textoEstablecimiento = $("#idEstablecimiento option[value='" + idEstablecimiento + "']").text()
            //textoEstablecimiento = $("#idEstablecimiento").text();
        }

        var examenes = $("#dvTblExamen table tr").length;
        if (ok && examenes <= 1) {
            jAlert("Agregue exámenes a la orden.", "¡Alerta!");
            ok = false;
        }

        var muestras = $("#dvTblOrdenMuestra table tr").length;
        if (ok && muestras <= 1) {
            jAlert("Agregue muestras a la orden.", "¡Alerta!");
            ok = false;
        }

        var muestras = $("#dvTblMaterial table tr").length;
        if (ok && muestras <= 1) {
            jAlert("Agregue materiales a la orden.", "¡Alerta!");
            ok = false;
        }

        if (ok) ok = EvaluarTablaMuestra();
        //if (ok) ok = EvaluarTablaMaterial(); //Juan Muga

        //Si el check no esta seleccionado
        //if (!($("#noPrecisaNroOficio").is(":checked"))) {
        //    var nroOficio = $.trim($("#nroOficio").val());
        //    if (ok && nroOficio == "") {
        //        jAlert("Ingrese el nro de Oficio / Documento.", "¡Alerta!");
        //        ok = false;
        //    }
        //}

        //Si no se ingreso solicitante
        var solicitante = $.trim($("#solicitante").val());
        if (ok && solicitante === "") {
            jAlert("Ingrese el Código de Colegio Profesional del Solicitante.", "¡Alerta!");
            ok = false;
        }

        //Si no se ingreso fecha de solicitud
        var fechaSolicitud = $.trim($("#fechaSolicitud").val());
        if (ok && !ValidaFecha(fechaSolicitud, null, "Solicitud", 1))
            return false;

        /*  var muestras = $("#dvTblDatoClinico table tr").length;
          if (ok && muestras <= 1) {
              jAlert("Agregue datos clínicos a la orden.", "Alerta!");
              ok = false;
          }        */

        $(".idLaboratorioDestinoMaterial").each(function (index, item) {
            var valLab = $(item).val();

            if (valLab === undefined || valLab === null || valLab == 0 || valLab == "") {
                $(item).focus();

                jAlert("Ingrese el Laboratorio de Destino en todos los materiales", "¡Alerta!");
                ok = false;
            }
        });

        $(".codigoMuestraRecepcion").each(function (index, item) {
            var valLab = $(item).val();

            if (valLab === undefined || valLab === null || valLab == "") {
                $(item).focus();

                jAlert("Ingrese el código de muestra en todos los tipos de muestra", "¡Alerta!");
                ok = false;
            }
            else if ($.trim(valLab) == "") {
                jAlert("Ingrese el código de muestra en todos los tipos de muestra", "¡Alerta!");
                ok = false;
            }
        });
        if ($("#CodigoUnicoOrigen").val() != "") {
            textoEstablecimiento = $("#CodigoUnicoOrigen").val();
        }
        else {
            textoEstablecimiento = $("#idEstablecimientoFrecuente option[value='" + idEstablecimiento + "']").text();
        }
        /*
        debugger;
        var xarchivo = $('#Archivo').val();
        if (xarchivo == null || xarchivo == undefined) {
            jAlert("Debe adjuntar la Ficha Epidemiológica de la Orden", "¡Alerta!");
            ok = false;
        }
        var size = $('#sizeFile').val();
        if (size > 1024) {
            jAlert('El peso del archivo no debe exceder los 1Mb', 'Alerta')
            return false;
        }
        */
        var _dniPr = $('#dniPr').val();
        $('#dniEjecutorPr').val(_dniPr);
        var _ejecutor = $('#ejecutor').val();
        $('#ejecutorPr').val(_ejecutor);

        var PruebaR = $('#pr').val();
        if (PruebaR == '1') {
            if (_dniPr == '' && _ejecutor == '') {
                jAlert('Ingrese los datos del encargado de la prueba rápida', 'Alerta');
                return false;
            } else if (_dniPr != '' && _ejecutor == '') {
                jAlert('Ingrese los datos del encargado de la prueba rápida', 'Alerta');
                return false;
            }
        }
        
        //if (ok) {
        //    if (valcodigoOrden == "") {
        //        jConfirm('¿Está seguro de guardar la orden con el establecimineto seleccionado: ' + textoEstablecimiento + '?', 'Confirmar Registro', function (r) {
        //            if (r) {
        //                dpUI.loading.start("Creando Orden ...");
        //                $("#frmOrden").submit();
        //            }
        //        });
        //    }
        //    if (valcodigoOrden != "") {
        //        dpUI.loading.start("Creando Orden ...");
        //        $("#frmOrden").submit();
        //    }
        //}
        // Cambiando mensaje de salida al guardar.

        if (ok) {
            //var xarchivo = $('#Archivo').val();
            if (valcodigoOrden == "") {
                
                jConfirm('¿Está seguro de guardar la orden con el establecimineto seleccionado: ' + textoEstablecimiento + '?', 'Confirmar Registro', function (r) {
                    if (r) {
                        jConfirm('¿Desea guardar y finalizar la orden?', 'Confirmar Registro', function (q) {
                            if (q) {
                                $("#GuardarFinalizar").val('1');
                                dpUI.loading.start("Registrando Orden ...");

                                $("#frmOrden").submit();
                            } else {
                                dpUI.loading.start("Creando Orden ...");

                                $("#frmOrden").submit();
                            }
                        });
                    }

                    if (valcodigoOrden != "") {
                        dpUI.loading.start("Creando Orden ...");
                        $("#frmOrden").submit();
                    }
                });
            }
        }
                
        return ok;
    });
    ///Fin Evento Guardar Orden


    ///INICIO Evento Finalizar Orden
    $("#btnFinalizar").on("click", function (e) {
        e.preventDefault();
        //debugger;
        //alert($("#hddDatoEESSOrigen").val());
        //alert($("#CodigoUnicoOrigen").val());
        //Juan Muga - Inicio
        var xarchivo = $('#Archivo').val();
        $("#codifFinalizar").val($("input[id^='codificacion']").val());
        //Juan Muga - Fin
        var ok = true;
        var examenes = $("#dvTblExamen table tr").length;
        if (ok && examenes <= 1) {
            jAlert("Agregue exámenes a la orden.", "Alerta!");
            ok = false;
        }

        var muestras = $("#dvTblOrdenMuestra table tr").length;
        if (ok && muestras <= 1) {
            jAlert("Agregue muestras a la orden.", "Alerta!");
            ok = false;
        }

        var muestras = $("#dvTblMaterial table tr").length;
        if (ok && muestras <= 1) {
            jAlert("Agregue materiales a la orden.", "Alerta!");
            ok = false;
        }

        /*
        var muestras = $("#dvTblDatoClinico table tr").length;
        if (ok && muestras <= 1) {
            jAlert("Agregue datos clínicos a la orden.", "Alerta!");
            ok = false;
        }*/

        //Si el check no esta seleccionado
        //if (!($("#noPrecisaNroOficio").is(":checked"))) {
        //    var nroOficio = $.trim($("#nroOficio").val());
        //    if (ok && nroOficio == "") {
        //        jAlert("Ingrese el nro de Oficio / Documento.", "¡Alerta!");
        //        ok = false;
        //    }
        //}

        //Si no se ingreso fecha de solicitud
        var fechaSolicitud = $.trim($("#fechaSolicitud").val());
        if (ok && fechaSolicitud == "") {
            jAlert("Ingrese la Fecha de Solicitud", "¡Alerta!");
            ok = false;
        }
        //var fechaIngresoINS = $.trim($("#fechaIngresoINS").val());
        //if (ok && fechaIngresoINS == "") {
        //    jAlert("Ingrese la Fecha de Ingreso al INS", "¡Alerta!");
        //    ok = false;
        //}
        var today = new Date().getTime();
        var idate = fechaSolicitud.split("/");

        idate = new Date(idate[2], idate[1] - 1, idate[0]).getTime();
        if ((today - idate) < 0) {
            jAlert("La Fecha de Solicitud no puede ser mayor a la Fecha Actual", "¡Alerta!");
            ok = false;
        }


        if (ok) {
            dpUI.loading.start("Finalizando Orden ...");
            $("#frmOrden").submit();
        }

        return ok;
    });
    ///Fin Evento Finalizar Orden
    $("#btnShowCodigoMuestra").on("click", function (e) {
        e.preventDefault();

        var url = $(this).attr("href");

        $("#dialog-open").dialog({
            title: "Código de Muestra",
            autoOpen: false,
            resizable: false,
            height: 120,
            width: 200,
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
});




function agregarEventosTablaMaterial() {
    $('.selMotivoRechazo').each(function (i, elm) {
        if ($(this).attr("id")) {
            $(this).change(function () {
                //console.log($(this).val());
            }).multipleSelect({
                width: '100%',
                multiple: true,
                multipleWidth: 300
            });
        }
    });

    $('.chkConformeMuestra').each(function (i, obj) {
        var divselMotivoRechazo = "div" + $(this).attr("id");
        if ($(this).is(":checked")) {
            $("#" + divselMotivoRechazo).hide();
        }
        else {
            $("#" + divselMotivoRechazo).show();
        }
        $(this).on('click', function (e) {
            if ($(this).is(":checked")) {
                $("#" + divselMotivoRechazo).hide();
            }
            else {
                $("#" + divselMotivoRechazo).show();
            }

        });

    });

    $(".eliminarMaterial").on("click", function () {
        var idOrdenMaterial = $(this).parent().attr("class");
        $.ajax(
          {
              url: URL_BASE + "Orden/DelMaterial?idOrdenMaterial=" + idOrdenMaterial,
              cache: false,
              method: "GET"
          }).done(function (msg) {

              //Se Agrega Material
              $("#dvTblMaterial").html(msg);
              agregarEventosTablaMaterial();

              //Se agrega eventos para todos
              //Juan Muga - configuracion estará solo en setup.js
              $("input.datepickerTelerik:text").setDefaultDatePicker();
              //$("input.datepickerTelerik:text").kendoDatePicker({ culture: "es-PE" });
              $(".timepicker").timeEntry({ show24Hours: true });
              $(".cantidad").kendoNumericTextBox({ format: "0" });
              $(".volumen").kendoNumericTextBox({ format: "#.00 ml" });

          }
          );
    });



    $('.noPrecisaVolumen').each(function (i, obj) {
        var idVolumenMuestra = $(this).attr("id");
        var idDato = idVolumenMuestra.substr(5, idVolumenMuestra.length);
        var idNoPrecisaVolumen = "volMuestra" + idDato;
        if ($(this).is(":checked")) {
            var numerictextbox = $("#" + idNoPrecisaVolumen).data("kendoNumericTextBox");
            if (numerictextbox != undefined)
                numerictextbox.enable(false);
        }
        else {
            var numerictextbox = $("#" + idNoPrecisaVolumen).data("kendoNumericTextBox");
            if (numerictextbox != undefined)
                numerictextbox.enable(true);
        }
    });

    $('.noPrecisaVolumen').change(function () {
        var idVolumenMuestra = $(this).attr("id");
        var idDato = idVolumenMuestra.substr(5, idVolumenMuestra.length);
        var idNoPrecisaVolumen = "volMuestra" + idDato;

        if ($(this).is(":checked")) {
            var numerictextbox = $("#" + idNoPrecisaVolumen).data("kendoNumericTextBox");
            numerictextbox.enable(false);

        }
        else {
            var numerictextbox = $("#" + idNoPrecisaVolumen).data("kendoNumericTextBox");
            numerictextbox.enable(true);
        }
    });



    /*
    $("input.datepickerTelerik:text").kendoDatePicker({
        culture: "es-PE"
    });
    var myDate = new Date();
    var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
    var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
    var anio = myDate.getFullYear();
    var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
    var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();
    $("#fechaEnvioPopup").val(dia + "/" + mes + "/" + anio);
    $("#horaEnvioPopup").timeEntry({ show24Hours: true });
    $("#horaEnvioPopup").val(hora + ":" + minutos);*/
    $(".idLaboratorioDestinoMaterial").each(function () {
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
}



function AgregarEventosDatosClinicos() {
    //Juan Muga - configuracion estará solo en setup.js
    //debugger;
    $("input.datepickerTelerik:text").setDefaultDatePicker();
    //$("input.datepickerTelerik:text").kendoDatePicker({
    //    culture: "es-PE"
    //});

    $('.checkboxNoPrecisa').change(function () {
        if ($(this).is(":checked")) {
            var idCheckedNoPrecisa = $(this).attr("id");
            var idDato = "value" + idCheckedNoPrecisa.substr(8, idCheckedNoPrecisa.length);
            var idCheck = "check" + idCheckedNoPrecisa.substr(8, idCheckedNoPrecisa.length);

            $("#" + idCheck).val("true");
            $("#" + idDato).prop('disabled', true);
            $.trim($("#" + idDato).val(""));
        }
        else {
            var idCheckedNoPrecisa = $(this).attr("id");
            var idDato = "value" + idCheckedNoPrecisa.substr(8, idCheckedNoPrecisa.length);
            var idCheck = "check" + idCheckedNoPrecisa.substr(8, idCheckedNoPrecisa.length);

            $("#" + idCheck).val("false");
            $("#" + idDato).prop('disabled', false);
        }
        $('.checkboxNoPrecisa').val($(this).is(':checked'));
    });
}





function onChangenoPrecisaNroOficio() {
    if ($("#noPrecisaNroOficio").is(":checked")) {
        $("#nroOficio").val("");
        $("#nroOficio").prop('disabled', true);
    }
    else {
        $("#nroOficio").prop('disabled', false);
    }

}



function limpiarCombo(id) {
    $(id).prop('disabled', false);
    $(id).empty();
    var newOption = $('<option value="0"></option>');
    $(id).append(newOption);
    $(id + "_chosen").removeClass("chosen-disabled");
    $(id).val("0").trigger("liszt:updated");
    $(id).trigger("chosen:updated");
}


function seguroSaludChangeHandler() {

    $("#Paciente_tipoSeguroSalud").change(function () {

        SeleccionarTipoSeguro();
    });
}

function SeguroSaludSetUp() {

    SeleccionarTipoSeguro();
    seguroSaludChangeHandler();
}

function SeleccionarTipoSeguro() {

    var tipoSeguroSalud = $("#Paciente_tipoSeguroSalud").val();
    if (tipoSeguroSalud == 9) {
        $("#Paciente_otroSeguroSalud").prop("disabled", false);
    }
    else {
        $("#Paciente_otroSeguroSalud").prop("disabled", true);
    }
}

function OnFechaInicioSintomasChange(control) {
    var fechaSintomas = $.trim($("#" + control.id).val());

    if ($("#" + control.id)[0].disabled) {
        $.trim($("#" + control.id).val(""));
        return false;
    }

    if (fechaSintomas != "") {
        var today = new Date().getTime();
        var idate = fechaSintomas.split("/");

        idate = new Date(idate[2], idate[1] - 1, idate[0]).getTime();
        if ((today - idate) < 0) {
            $.trim($("#" + control.id).val(""));
            jAlert("La Fecha de Inicio de Síntomas no puede ser mayor a la Fecha Actual", "¡Alerta!");
            return false;
        }
    }

    return true;
}

function EvaluarTablaMuestra() {
    var existeFechaInvalida = false;

    $('#tableOrdenMuestra tr')
        .each(function () {
            var rowId = $(this).find('input[type="hidden"]').val();

            if (rowId != undefined) {
                //hora obtencion
                var horaObtencion = $("#horaMuestra" + rowId).val();

                //Fecha de obtencion
                var fechaObtencion = $("#fechaMuestra" + rowId).val();

                if (!ValidaFecha(fechaObtencion, horaObtencion, "Obtención", 1)) {
                    existeFechaInvalida = true;
                }
                //if ($("#horaMuestra" + rowId).val();) Revisar si cuando finalizas valida que  ingreses todas las muestras
            }
        });

    var resultado = !existeFechaInvalida; // && campo2 && campo3...

    return resultado;
}

function EvaluarTablaMaterial() {
    var existeFechaInvalida = false;

    $('#tableMaterial tr')
        .each(function () {
            var rowId = $(this).find('input[type="hidden"]').val();

            if (rowId != undefined) {
                //hora de envio
                var horaEnvio = $("#horaEnvio" + rowId).val();


                //fecha de envio
                var fechaEnvio = $("#fechaEnvio" + rowId).val();

                if (!ValidaFecha(fechaEnvio, horaEnvio, "Envío", 2)) {
                    existeFechaInvalida = true;
                }
            }
        });

    var resultado = !existeFechaInvalida; // && campo2 && campo3...

    return resultado;
}

function ValidaFecha(fecha, hora, tipoFecha, tipoValidacion) {
    var resultado = ValidacionDeHora(fecha, hora);

    if (resultado === 1) {
        jAlert("Ingrese la Fecha de " + tipoFecha, "¡Alerta!");
        return false;
    }

    if (resultado === 2) {
        jAlert("Ingrese una hora de " + tipoFecha + " válida.", "Alerta!");
        return false;
    }

    resultado = ValidacionDeFecha(fecha, hora, tipoValidacion);

    if (resultado === 1) {
        jAlert("La Fecha y hora de " + tipoFecha + " no puede ser mayor a la Fecha Actual", "¡Alerta!");
        return false;
    }

    if (resultado === 2) {
        jAlert("La Fecha y hora de " + tipoFecha + " no puede ser menor a la Fecha Actual", "¡Alerta!");
        return false;
    }

    if (resultado === 3) {
        jAlert("Ingrese una fecha de " + tipoFecha + " válida", "¡Alerta!");
        return false;
    }

    return true;
}

function ValidacionDeHora(fecha, hora) {
    if (fecha === "") {
        return 1;
    }

    var ihoraFecha = null;

    //Valida hora
    if (hora != null) {
        if (hora === "") {
            return 2;
        }

        ihoraFecha = hora.split(":");

        if (ihoraFecha.length !== 2) {
            return 2;
        }
    }

    return 0;
}

function ValidacionDeFecha(fecha, hora, tipoValidacion) {
    var today = new Date().getTime();
    var idate = fecha.split("/");
    var ihoraFecha = null;

    if (idate[2] < 1900)
        return 3;

    if (hora == null)
        idate = new Date(idate[2], idate[1] - 1, idate[0]).getTime();
    else {
        ihoraFecha = hora.split(":");
        idate = new Date(idate[2], idate[1] - 1, idate[0], ihoraFecha[0], ihoraFecha[1], '00');
    }

    //Valida fecha tomando en cuenta la hora
    if (tipoValidacion === 1) {
        if ((today - idate) < 0) {
            return 1;
        }
    } else {
        if ((idate - today) < 0) {
            return 2;
        }
    }

    return 0;
}

function DatoClinicoAno(event) {
    var controlId = event.target.id;
    var textbox = $.trim($('#' + controlId).val());

    if (ValidKeyOnKeyPress(event))
        return true;

    if (textbox.length == 4)
        return false;

    return isOnlyNumber(event);
}

function ValidKeyOnKeyPress(event) {
    switch (event.keyCode) {
        case 39:
        case 37:
        case 8:
        case 46:
            return true;
        default:
            return false;
    }
}

function DatoClinicoGlasgow(event) {
    var controlId = event.target.id;
    var textbox = $.trim($('#' + controlId).val());

    if (textbox.length == 10)
        return false;

    return isOnlyNumber(event);
}

function ValidateTextboxLength(event, newvalue) {
    if (EsCombinacionEspecial(event))
        return true;

    var controlId = event.target.id;
    var textbox = $.trim($('#' + controlId).val());
    var newTextValue = textbox + newvalue;

    var lengthToCompare;

    switch (controlId) {
        case "Paciente_ApellidoPaterno":
        case "Paciente_ApellidoMaterno":
        case "Paciente_Nombres":
        case "Paciente_otroSeguroSalud":
            lengthToCompare = 20;
            break;
        case "Paciente_correoElectronico":
        case "Paciente_ocupacion":
        case "solicitante":
        case "observacion":
            lengthToCompare = 35;
            break;
        case "Paciente_DireccionActual":
            lengthToCompare = 50;
            break;
        case "nroOficio":
            lengthToCompare = 50;
            break;
        default:
            lengthToCompare = 100;
    }

    if (textbox.length === lengthToCompare || newTextValue.length > lengthToCompare)
        return false;

    return true;
}

//Este metodo solo funciona con el keypress por lo que solo se necesita usar el event.
function NewInputHasValidCharacters(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

$(function () {
    $(".telefonoFijoValido").bind('keypress', function (e) {
        if (EsCombinacionEspecial(e))
            return true;

        return isOnlyNumber(e) && NewPhoneNumberIsValid(e, '#Paciente_TelefonoFijo', '');
    });
    $(".telefonoFijoValido").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);

        if (!NewInputHasValidPastedCharacters(newAdditionalValue) || !NewPhoneNumberIsValid(event, '#Paciente_TelefonoFijo', newAdditionalValue)) {
            event.preventDefault();
        }
    });
});

$(function () {
    $(".celular1Valido").bind('keypress', function (e) {
        if (EsCombinacionEspecial(e))
            return true;

        return isOnlyNumber(e) && NewPhoneNumberIsValid(e, '#Paciente_Celular1', '');
    });
    $(".celular1Valido").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);

        if (!NewInputHasValidPastedCharacters(newAdditionalValue) || !NewPhoneNumberIsValid(event, '#Paciente_Celular1', newAdditionalValue)) {
            event.preventDefault();
        }
    });
});

$(function () {
    $(".celular2Valido").bind('keypress', function (e) {
        if (EsCombinacionEspecial(e))
            return true;

        return isOnlyNumber(e) && NewPhoneNumberIsValid(e, '#Paciente_Celular2', '');
    });
    $(".celular2Valido").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);

        if (!NewInputHasValidPastedCharacters(newAdditionalValue) || !NewPhoneNumberIsValid(event, '#Paciente_Celular2', newAdditionalValue)) {
            event.preventDefault();
        }
    });
});

function isOnlyNumber(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

//Evalua la cadeana pegada de acuerdo a la logica del metodo. Solo para el 'paste' event.
function NewInputHasValidPastedCharacters(value) {
    if (value.match(/^[0-9]+$/) == null)
        return false;

    return true;
}

//Se creo para que estas acciones funcionen en firefox. IE y Chrome no tienen problemas.
function EsCombinacionEspecial(event) {
    return event.charCode === 0 ||  //Esto es para firefox: delete,supr,arrow keys.
           (event.ctrlKey && event.key === "x") ||
           (event.ctrlKey && event.key === "c") ||
           (event.ctrlKey && event.key === "v");
}

$('body').on('click', '#btnImprimir', function (e) {
    if ($.browser.msie || !!navigator.userAgent.match(/Trident.*rv[ :]?[1-9]{2}\./)) {
        var printContent = document.getElementById('imprimirCodificacion');
        var windowUrl = 'Job Receipt';
        var uniqueName = new Date();
        var windowName = 'Print' + uniqueName.getTime();
        var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');
        printWindow.document.write(printContent.innerHTML);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();

    }
    else {
        $("#imprimirCodificacion").printArea();
        return false;
    }
});