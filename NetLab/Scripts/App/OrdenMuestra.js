// Descripción: Realiza validaciones al formulario del registro de recepcion, de busqueda y de referenciacion de muestras.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
jQuery(function ($) {

    var _oldShow = $.fn.show;

    $.fn.show = function (speed, oldCallback) {
        return $(this).each(function () {
            var obj = $(this),
                newCallback = function () {
                    if ($.isFunction(oldCallback)) {
                        oldCallback.apply(obj);
                    }
                    obj.trigger('afterShow');
                };

            // you can trigger a before show if you want
            obj.trigger('beforeShow');

            // now use the old function to show the element passing the new callback
            _oldShow.apply(obj, [speed, newCallback]);
        });
    }
});
function iBuscarClick() {
    document.getElementById("btnBuscar").click();
}
var myDate = new Date();

var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
var anio = myDate.getFullYear();

var closeEditToCharge = false;
var submitted = false;
var htmlToCharge = "";
var activeEdit;
var derivar;
var Token;
$('body').on('click', '#btnEditar', function (e) {
    e.preventDefault();
    $("#frmOrden").submit();
    setTimeout(function () {
        dpUI.loading.start("Editando Orden ...");
    }, 3000);
});


function agregarEventosMostrarReferencia() {
    $("#divDatosReferencia").on("change", ".chkReferenciar", function () {
        var datepicker = $(this).closest("tr").find('input.datepickerReferencia:text').data("kendoDatePicker");
        if ($(this).is(':checked')) {

            $(this).closest('tr').find('.chkDerivar').prop('checked', false);
            $(this).closest("tr").find('input[type="text"]').removeAttr('disabled');
            $(this).closest("tr").find('select').attr('disabled', false).trigger("chosen:updated");
            $(this).closest("tr").find('span').removeAttr('hidden');
            datepicker.enable();
        }
        else {

            $(this).closest("tr").find('input[type="text"]').attr('disabled', 'disabled');
            $(this).closest("tr").find('select').attr('disabled', true).trigger("chosen:updated");
            $(this).closest("tr").find('span').attr('hidden', true);
            datepicker.enable(false);
        }
    });
    //juan muga
    $("#divDatosReferencia").on("change", ".chkDerivar", function () {
        if ($(this).is(':checked')) {
            derivar = "1";
            $(this).closest('tr').find('.chkReferenciar').prop('checked', false);
            $(this).closest("tr").find('input[type="text"]').removeAttr('disabled');
            $(this).closest("tr").find('select').attr('disabled', false).trigger("chosen:updated");
            $(this).closest("tr").find('span').removeAttr('hidden');
        }
        else {
            derivar = "0";
            $(this).closest("tr").find('input[type="text"]').attr('disabled', 'disabled');
            $(this).closest("tr").find('select').attr('disabled', true).trigger("chosen:updated");
            $(this).closest("tr").find('span').attr('hidden', true);
            datepicker.enable(false);
        }
        //alert('Derivar1 : ' + derivar);
    });

    //$(document).ready(function (e) {

    //$("#tblDatosReferencia .inFechaRec").datepicker();
    $("#tblDatosReferencia .inHoraRec").timeEntry({ show24Hours: true });
    $("#tblDatosReferencia .inFechaRec").val(dia + "/" + mes + "/" + anio);

    var myDate = new Date();
    var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
    var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();

    $("#tblDatosReferencia .inHoraRec").val(hora + ":" + minutos);


    $(".tblDatosReferencia .idLaboratorioDestino").each(function () {

        $(this).ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 100,
            cache: false,
            url: URL_BASE + "OrdenMuestra/GetAllLaboratorios?derivar=" + $(this).closest('tr').find('.chkDerivar').val()
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, { placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias" }
       );
    });

    $(".chosen-container-single").css("width", "150px")

    $(".tblDatosReferencia .idLaboratorioDestino").val("0").trigger("liszt:updated");
    $(".tblDatosReferencia .idLaboratorioDestino").trigger("chosen:updated");

    $("#btnCancelarReferencia").on("click", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
        return false;
    });

    $("input.datepickerReferencia:text").kendoDatePicker({
        culture: "es-PE"
    });


    $('input.datepickerReferencia:text').each(function (i, obj) {
        var datepicker = $(this).data("kendoDatePicker");
        datepicker.enable(false);
    });


    $(".btnAlicuota").on("click", function (e) {
        e.preventDefault();

        var idBtn = $(this).attr("id");
        var idNroPresentacion = $(this).attr("title");
        var idMuestraReferencia = idBtn.substring(7);
        var idCant = "canRow_" + idMuestraReferencia;

        var codigoMuestraActual = $("#lblCodMuestra_" + idMuestraReferencia).text();

        var cantidadAlicuotas = parseInt($("#" + idCant).val());
        var nroAlicuotaCopiada = idNroPresentacion.split('#')[1];
        var cantAlicuotaCopiada = cantidadAlicuotas + 1;
        var lblNroAlicuota = "Alícuota #" + nroAlicuotaCopiada + "." + cantAlicuotaCopiada;

        var idMuestraReferenciaNueva = idMuestraReferencia + "_" + cantidadAlicuotas;

        var idAlicuotaActual = idBtn;
        var idAlicuotaNueva = "lnkDel_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevaLaboratorio = "cmbLab_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevoExamen = "cmbExa_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevaCheck = "chkNom_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevoDate = "txtFec_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevoTime = "txtHor_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevoLabel = "lblPre_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevoPadre = "lblPadre_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevoCodMuestra = "lblCodMuestra_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevoBtnCodMuestra = "btnShowCodigoTMP_" + idMuestraReferenciaNueva;
        //var idAlicuotaNuevoDel = "lblDel_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevaCheckDeriva = "chkDer_" + idMuestraReferenciaNueva;
        var idAlicuotaNuevoLabel2 = "TipoMuestra_" + idMuestraReferenciaNueva;

        chkNombre = "chkNom_" + idMuestraReferenciaNueva;
        if (cantidadAlicuotas > 0) {
            var cantidadAlicuotasTmp = cantidadAlicuotas - 1;
            var idAlicuotaActual = "lnkDel_" + idMuestraReferencia + "_" + cantidadAlicuotasTmp;
        }

        cantidadAlicuotas = cantidadAlicuotas + 1;
        $("#" + idCant).val(cantidadAlicuotas);

        var trHtml = $('#hideTRReferencia').html();

        trHtml = trHtml.replace('id="idOrdenRecepcion" type="hidden"', 'id="idOrdenRecepcion" type="hidden" value="' + idMuestraReferenciaNueva + '"');
        trHtml = trHtml.replace('id="idOrdenPadre" type="hidden"', 'id="' + idAlicuotaNuevoPadre + '" type="hidden" value="' + idMuestraReferencia + '"');

        trHtml = trHtml.replace('id="lblCodigoMuestra" name="lblCodigoMuestra"', 'id="' + idAlicuotaNuevoCodMuestra + '" name="' + idAlicuotaNuevoCodMuestra + '"');
        trHtml = trHtml.replace('href="/Orden/ShowPopupCodigoMuestra?codificacion=COD01"', 'href="/Orden/ShowPopupCodigoMuestra?codificacion=' + codigoMuestraActual + '"');
        trHtml = trHtml.replace('id="btnShowCodigoTMP"', 'id="' + idAlicuotaNuevoBtnCodMuestra + '"');

        trHtml = trHtml.replace('checkbox"', 'checkbox"  id="' + idAlicuotaNuevaCheck + '" name="' + idAlicuotaNuevaCheck + '"');
        trHtml = trHtml.replace('idLaboratorioDestino"', 'idLaboratorioDestino" id="' + idAlicuotaNuevaLaboratorio + '"');
        trHtml = trHtml.replace('name="idLaboratorioDestino"', 'name="' + idAlicuotaNuevaLaboratorio + '"');
        trHtml = trHtml.replace('id="idExamen"', 'id="' + idAlicuotaNuevoExamen + '"');

        trHtml = trHtml.replace('id="fechaEnvio1"', 'id="' + idAlicuotaNuevoDate + '"');
        trHtml = trHtml.replace('name="fechaEnvio"', 'name="' + idAlicuotaNuevoDate + '"');
        trHtml = trHtml.replace('id="horaEnvio1"', 'id="' + idAlicuotaNuevoTime + '"');
        trHtml = trHtml.replace('name="horaEnvio"', 'name="' + idAlicuotaNuevoTime + '"');
        trHtml = trHtml.replace('id="lblTmp"', 'id="' + idAlicuotaNuevoLabel + '"');
        trHtml = trHtml.replace('id="lnkDelete1"', 'id="' + idAlicuotaNueva + '"');

        trHtml = trHtml.replace('chkDerivar"', 'chkDerivar" id="' + idAlicuotaNuevaCheckDeriva + '" name="' + idAlicuotaNuevaCheckDeriva + '"');
        trHtml = trHtml.replace('id="idTipoMuestra" type="hidden"', 'id="idTipoMuestra" type="hidden" value="' + idMuestraReferenciaNueva + '"');

        //alert(trHtml);

        $(trHtml).insertAfter($("#" + idAlicuotaActual).closest('tr'));

        $("#" + idAlicuotaNuevoLabel).text(lblNroAlicuota);

        $("#" + idAlicuotaNuevoCodMuestra).text(codigoMuestraActual);

        $("#" + idAlicuotaNuevoDate).kendoDatePicker({
            culture: "es-PE"
        });

        $("#" + idAlicuotaNueva).on("click", function (e) {
            e.preventDefault();

            var idBtnTmp = $(this).attr("id");
            var idMuestraReferenciaTmp = idBtnTmp.substring(7);
            var lblPadre = "lblPadre_" + idMuestraReferenciaTmp;
            var idPadre = $("#" + lblPadre).val();
            var idCantTmp = "canRow_" + idPadre;
            var cantidadAlicuotasTmp = parseInt($("#" + idCantTmp).val());
            //alert("nueva:" + cantidadAlicuotasTmp);

            var cantNuevaTMP = cantidadAlicuotasTmp - 1;

            //alert("restada:"+cantNuevaTMP);

            $("#" + idCantTmp).val(cantNuevaTMP);
            //var idBtnDel = $(this).attr("id");
            //var idMuestraReferencia = idBtnDel.substring(7);
            //var idCant = "canRow_" + idMuestraReferencia;

            //var cantidadAlicuotas = parseInt($("#" + idCant).val());
            //var cantidadAlicuotasTmp = cantidadAlicuotas - 1;
            //var idAlicuotaActual = "lnkDel_" + idMuestraReferencia + "_" + cantidadAlicuotasTmp;

            $(this).closest('tr').remove();

            return false;
        });


        $("#" + idAlicuotaNuevoBtnCodMuestra).on("click", function (e) {
            e.preventDefault();
            var url = $(this).attr("href") + "&_=" + (new Date()).getTime();
            $("#dialog-open").html("");
            $("#dialog-open").dialog({
                title: "Código de Muestra",
                autoOpen: false,
                resizable: false,
                height: 150,
                width: 200,
                show: { effect: "drop", direction: "up" },
                modal: true,
                draggable: true,
                open: function () {
                    $(this).load(url);
                },
                close: function (event, ui) {
                    $(this).dialog('close');
                }
            });
            $("#dialog-open").dialog("open");
            return false;
        });




        //$("#idLaboratorio").ajaxChosen({
        $("#" + idAlicuotaNuevaLaboratorio).ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 100,
            cache: false,
            url: URL_BASE + "OrdenMuestra/GetAllLaboratorios?derivar=" + $(this).closest('tr').find('.chkDerivar').val()
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, {
            placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias"
        }).change(function () {
            limpiarCombo("#" + idAlicuotaNuevoExamen);

            var idLaboratorio = $("#" + idAlicuotaNuevaLaboratorio).val();
            var idexamen = $("#idExamen").val();
            var paciente_genero = $("#Paciente_Genero").val();
            $("#" + idAlicuotaNuevoExamen).ajaxChosen({
                dataType: "json",
                type: "POST",
                minTermLength: 3,
                afterTypeDelay: 100,
                cache: false,
                url: URL_BASE + "OrdenMuestra/GetExamenes?idLaboratorio=" + idLaboratorio + "&genero=" + paciente_genero + "&derivar=" + $(this).closest('tr').find('.chkDerivar').val()
            }, {
                loadingImg: URL_BASE + "Content/images/loading.gif"
            }, { placeholder_text_single: "Seleccione el Examen", no_results_text: "El Laboratorio no cuenta con el Examen que busca" });
        });

        setTimeout(function () {
            //$("#"+idAlicuotaNuevoDate).datepicker();
            $("#" + idAlicuotaNuevoDate).val(dia + "/" + mes + "/" + anio);
            //$("#" + idAlicuotaNuevoDate).attr('id', "txtFec_" + idMuestraReferencia + "_1");

            myDate = new Date();
            var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
            var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();

            $("#" + idAlicuotaNuevoTime).timeEntry({ show24Hours: true });
            $("#" + idAlicuotaNuevoTime).val(hora + ":" + minutos);

        }, 300);


        function limpiarCombo(id) {
            $(id).prop('disabled', false);
            $(id).empty();
            var newOption = $('<option value="0"></option>');
            $(id).append(newOption);
            $(id + "_chosen").removeClass("chosen-disabled");
            $(id).val("0").trigger("liszt:updated");
            $(id).trigger("chosen:updated");
        }

        return false;
    });


    $(".btnShowCodigoMuestra").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href") + "&_=" + (new Date()).getTime();
        $("#dialog-open").html("");
        $("#dialog-open").dialog({
            title: "Código de Muestra",
            autoOpen: false,
            resizable: false,
            height: 150,
            width: 200,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);

            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });
        $("#dialog-open").dialog("open");
        return false;
    });

}

function agregarEventosEditarReferencia() {
    $(".idLaboratorioDestinoR").each(function () {
        $(this).ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "OrdenMuestra/GetAllLaboratorios?derivar=" + $(this).closest('tr').find('.chkDerivar').val()
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, { placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias" }
       );
    });


    $("#tblDatosRecepcionEdicion").on("change", ".chkReferenciar", function () {

        if ($(this).is(':checked')) {
            $(this).closest("tr").find('input[type="text"]').removeAttr('disabled');
            $(this).closest("tr").find('select').attr('disabled', false).trigger("chosen:updated");
        }
        else {
            $(this).closest("tr").find('input[type="text"]').attr('disabled', 'disabled');
            $(this).closest("tr").find('select').attr('disabled', true).trigger("chosen:updated");
        }
    });

    $("#btnCancelar").on("click", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    });

}

function agregarEventosRecepcionarOrden() {
    $("#btnCancelarRecepcion").on("click", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
        return false;
    });

    $('.selMotivoRechazoDiv').hide();

    $("input.datepickerRecepcion:text").kendoDatePicker({
        culture: "es-PE"
    });

}

function btnLevatnarObservacion() {
    //debugger;
    //las fechas las mandan unidefined
    var hddRECHAZODATOSFICHA = $("#hddRECHAZODATOSFICHA").val();
    var hddRECHAZOPACIENTE = $("#hddRECHAZOPACIENTE").val();
    var fechaIngresoINS = "";
    var fechaReevaluacionFicha = "";
    if ($("#fechaIngresoINS").val() === undefined) {
        fechaIngresoINS = "";
    }
    else {
        fechaIngresoINS = $("#fechaIngresoINS").val();
    }
    if ($("#fechaReevaluacionFicha").val() === undefined) {
        fechaReevaluacionFicha = "";
    }
    else {
        fechaReevaluacionFicha = $("#fechaReevaluacionFicha").val();
    }

    var tipoDocumento = $("#tipoDocumento").val();
    var nroDocumento = $("#nroDocumento").val();
    var idOrden = $("#idOrden").val();
    var datos = "?hddRECHAZODATOSFICHA=" + hddRECHAZODATOSFICHA + "&hddRECHAZOPACIENTE=" + hddRECHAZOPACIENTE + "&fechaIngresoINS=" + fechaIngresoINS + "&fechaReevaluacionFicha=" + fechaReevaluacionFicha + "&tipoDocumento=" + tipoDocumento + "&nroDocumento=" + nroDocumento + "&idOrden=" + idOrden;
    $.ajax({
        url: URL_BASE + "OrdenMuestra/ValidarDatos" + datos,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.length > 0) {
                alert(data);
            }
        }
    });
}

$(document).ready(function () {
    $.ajaxSetup({ cache: false });
    //debugger;



    $(".datepicker").kendoDatePicker({
        culture: "es-PE"
    });

    $(".datepickerDesde").kendoDatePicker({
        culture: "es-PE"
    });

    $(".datepickerHasta").kendoDatePicker({
        culture: "es-PE"
    });




    $(".editDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");

        $("#dialog-edit").dialog({
            title: "Recibir Muestras",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 1200,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            //fluid: true,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregarEventosRecepcionarOrden();
                    $("#CodigoUnicoEnvio").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: URL_BASE + "Orden/GetEstablecimientosNew",
                                type: "POST",
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                data: "{ 'prefix': '" + request.term + "'," + "  'Departamento': '" + $("#ActualDepartamentoEESS").val() + "'," + "  'Provincia': '" + $("#ActualProvinciaEESS").val() + "'," + "  'Distrito': '" + $("#ActualDistritoEESS").val() + "'" + "}",
                                success: function (data) {
                                    response($.map(data, function (item) {
                                        return { label: item.Nombre, value: item.IdEstablecimiento };
                                    }));
                                }
                            });
                        },
                        select: function (e, i) {
                            e.preventDefault();
                            $("#hddDatoEESSEnvio").val(i.item.value);
                            $("#CodigoUnicoEnvio").val(i.item.label);
                        },
                        minLength: 3
                    });
                });
            },
            close: function () {
                closeEditForm();
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });

    var idPopup = "";
    var generoPopup = "";
    var laboratorioPopup = "";

    $('body').on('submit', '#formRecepcionarMuestra', function (e) {
        e.preventDefault();
        //debugger;
        var criterioRechazo = "";
        var ok = true;
        var sms = false;
        var cfechas = document.getElementsByName("fechaRecepcion").length;
        var fechaRecepcion = null;
        for (var i = 0; i < cfechas; i++) {
            fechaRecepcion = document.getElementsByName("fechaRecepcion")[i].value;
            if (fechaRecepcion = undefined || fechaRecepcion == '' || fechaRecepcion == null) {
                jAlert('Ingrese todas las fechas de recepción de la muestra', 'Alerta');
                return false;
            }
        }

        

        $(".checkboxNoPrecisa2").each(function (index, item) {
            if (!$(item).is(':checked')) {
                //sms = true;
                criterioRechazo = $(this).closest("tr").find('.selMotivoRechazo').val();
                if (criterioRechazo === undefined || criterioRechazo === null) {
                    ok = false;
                }
            }
        });

        if (ok) {
            $.ajax({
                type: "POST",
                cache: false,
                url: $(this).attr('action'),
                data: $(this).serialize(),
                success: function (data) {
                    var splitData = data.split('_');
                    var cantidadMuestrasInvalidas = splitData[0];
                    idPopup = splitData[1];//idOrden
                    generoPopup = splitData[2];
                    laboratorioPopup = splitData[3];
                    var totalMuestrasxRecepcionar = splitData[4];
                    var totalMuestrasValidas = splitData[5];
                    var totalMuestras = splitData[6];
                    var ExamenError = splitData[7];
                    var TipoMuestraError = splitData[8];

                    // generoPopup = $('#generoOrden').val();
                    if (cantidadMuestrasInvalidas == 0 & totalMuestras == totalMuestrasValidas) {
                        jAlert('Se recibieron todas las muestras.', 'Alerta');
                        closeEditToCharge = false;
                        if (sms) {
                            var fonoUtm = '51' + $("#telefonoUTM").val();
                            var codigo = $("#codigo").val();
                            var msjUtm = 'Estimado(a) usuario, se rechazó la muestra con Codigo de Orden ' + codigo + ' revise su bandeja de Muestras Rechazas en el sistema Netlab v2.';
                            enviarSms(fonoUtm, msjUtm);
                        }
                    }
                    else {
                        if (cantidadMuestrasInvalidas > 0 & totalMuestras > 0) {
                            jAlert('No se recibieron ' + cantidadMuestrasInvalidas + ' muestras, porque el Examen al que pertenecen no se procesa en el Laboratorio a Recibir, por favor revisar el exámen: ' + ExamenError, 'Alerta');
                            closeEditToCharge = true;
                        }
                        else {
                            //jAlert('No se recepcionaron ' + cantidadMuestrasInvalidas + ' muestras, porque el Examen al que pertenecen no se procesa en el Laboratorio Recepción seleccionado.', 'Alerta Recepcion');
                            closeEditToCharge = true;
                        }
                    }
                    //Inicio Juan Muga
                    //EnviarRespuesta(idPopup);
                    //Fin Juan Muga
                    $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();

                    // jAlert('Si: ' + r, 'Confirmation Results');
                    // });
                    // htmlToCharge = data;
                }
            });
        }
        else {
            jAlert("Debe seleccionar los motivos de Rechazo de las muestras No Conformes.", "Aviso");
        }
    });


    $("body").on("submit", "#formReferenciarMuestra", function (e) {
        var ok = true;
        $(".chkReferenciar").each(function (index, item) {
            if ($(item).is(":checked")) {
                var valLabDestino = $(this).closest("tr").find(".idLaboratorioDestino").val();
                if (valLabDestino == undefined || valLabDestino == null || valLabDestino == 0) {
                    ok = false;
                }
            }
        });
        $(".chkDerivar").each(function (index, item) {
            if ($(item).is(":checked")) {
                var valLabDestino = $(this).closest("tr").find(".idLaboratorioDestino").val();
                if (valLabDestino == undefined || valLabDestino == null || valLabDestino == 0) {
                    ok = false;
                }
            }
        });

        if (ok) {
            $.ajax({
                type: "POST",
                cache: false,
                url: $(this).attr("action"),
                data: $(this).serialize(),
                success: function (data) {
                    //debugger;
                    var splitData = data.split("_");
                    var muestrasInvalidas = splitData[0];
                    var ExamenError = splitData[3];
                    closeEditToCharge = false;
                    submitted = true;

                    if (muestrasInvalidas > 0) {
                        jAlert("No se recibieron " + muestrasInvalidas + " muestras, no puede ser procesadas en el laboratorio. Por favor Referenciar y revisar el exámen: " + ExamenError, "Alerta");
                        return false;
                    }
                    else {
                        $("#btnCerrar").on("click", function () {
                            $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                        });
                        jAlert("Las muestras han sido recepcionada(s) correctamente", "Aviso", function () {
                            iBuscarClick();
                        });
                    }
                }
            });
        }
        else {
            jAlert("Debe seleccionar el Laboratorio para las muestras seleccionadas.", "Aviso");
        }

        e.preventDefault();
        return false;
    });

    function closeEditForm() {
        if (closeEditToCharge) {
            closeEditToCharge = false;
            $("#dialog-edit").dialog({
                title: "Derivar/Referir Muestras",
                autoOpen: false,
                resizable: false,
                height: 'auto',
                width: 1200,
                position: {
                    my: "center top",
                    at: ("center top++" + (window.innerHeight * .1)),
                    collision: "none"
                },
                show: { effect: "drop", direction: "up" },
                fluid: true,
                modal: true,
                draggable: true,
                closeOnEscape: true,
                open: function () {
                    $(this).load(URL_BASE + "OrdenMuestra/ReferenciarMuestra?idOrden=" + idPopup + "&genero=" + generoPopup + "&idLab=" + laboratorioPopup, function () {
                        agregarEventosMostrarReferencia();
                    });
                },
                beforeClose: function () {
                    //debugger;
                    if (!submitted) {
                        jAlert("Debe recibir o referenciar para salir del proceso.", "Aviso");
                        return false;
                    } else {
                        submitted = false;
                        iBuscarClick();
                        return false;
                    }
                },
                close: function () {
                    iBuscarClick();
                    return false;
                }
            });
            $("#dialog-edit").dialog("open");
        }
        //else {
        //    $("#btnBuscarOrdenes").click();
        //}
    };

    $('body').on('click', '#btnPreview', function (e) {
        e.preventDefault();
        var htmlString = $("#dialog-edit").html();
        $("#dialog-hidden").html(htmlString);

        var url = $(this).attr("href");
        $("#dialog-edit").load(url);

    });

    $('#dialog-edit').on('click', '.chkConformeMuestra', function (e) {
        debugger;
        if ($(this).is(':checked')) {
            $(this).closest("tr").find('.selMotivoRechazoDiv').hide();
        } else {
            var isFirst = $(this).attr("isFirst");
            if (isFirst == "1") {
                $(this).attr("isFirst", "0");
                var itemEvent = this;
                $(this).closest("tr").find('.selMotivoRechazoDiv').show();

            } else {

                $(this).closest("tr").find('.selMotivoRechazoDiv').show();
            }
        }
    });

    $(".recepcionadoDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");


        $("#dialog-edit").dialog({
            title: "Orden Recibida",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 1200,
            position: {
                my: "center top",
                at: ("center top+" + (window.innerHeight * .1)),
                collision: "none"
            },
            //fluid: true,
            //show: { effect: "drop", direction: "up" },
            modal: true,
            //draggable: true,
            open: function () {
                $(this).load(url, function () {
                });
            },
            close: function () {
                closeEditForm();
            }
        });

        $("#dialog-edit").dialog("open");
        return false;

        //$(".ui-dialog").draggable();
    });

    $(".referenciaDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");

        $("#dialog-edit").dialog({
            title: "Editar Referencia",
            autoOpen: false,
            resizable: false,
            height: 580,
            width: 950,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregarEventosEditarReferencia();
                });
            },
            close: function () {
                closeEditForm();
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });

    $('#estadoOrden').change(function (e) {
        iBuscarClick();
    });

    $(':radio[name=esFechaRegistro]').change(function () {
        iBuscarClick();
    });
    //Descripción:Envío de resultados a otros al EQhali 
    //Author: Juan Muga.
    function EnviarRespuesta(idPopup) {
        debugger;
        if (idPopup.length > 0) {
            var url = 'http://localhost:27110/Token';
            var body = {
                grant_type: 'password',
                username: 'wsnetlab2',
                password: '0'
            };
            //Get Token
            $.ajax({
                url: url,
                method: 'POST',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: body,
                success: function (result) {
                    debugger;
                    Token = result.access_token;
                    console.log(Token);
                    if (Token.length > 0) {
                        $.ajax({
                            url: URL_BASE + "OrdenMuestra/GetTramaResultadobyCodigoOrden?idPopup=" + idPopup,
                            cache: false,
                            method: "GET",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (result) {
                                debugger;
                                console.log(result);
                                if (result.length > 0) {
                                    $.ajax({
                                        url: 'http://localhost:27110/api/paciente/ReadResultado?Resultado=' + JSON.stringify(result),
                                        type: 'GET',
                                        data: { Resultado: JSON.stringify(result) },
                                        contentType: "application/json;chartset=utf-8",
                                        headers: { "Authorization": Token },
                                        success: function (response) {
                                            debugger;
                                            console.log(JSON.stringify(response));
                                            $.ajax({
                                                url: URL_BASE + "OrdenMuestra/UpdateTramaResultadobyCodigoOrden?idPopup=" + idPopup,
                                                cache: false,
                                                method: "GET"
                                            });
                                        },
                                        statusCode: {
                                            statusCode: {
                                                200: function () {
                                                    alert('200');
                                                },
                                                404: function () {
                                                    alert('404');
                                                },
                                                500: function () {
                                                    alert('500');
                                                }
                                            }
                                        },
                                        error: function (xhr) {
                                            alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
                                        }
                                    });
                                }
                            },
                            error: function (jqXHR) {
                                alert(jqXHR.responseText);
                            }
                        }).fail(function () {
                            //debugger;
                            jAlert("Se produjo un error de Conexión.", "Error")
                        })
                    }

                },
                error: function (jqXHR) {
                    alert(jqXHR.responseText);
                }
            });
        }
    }


    function split(val) {
        return val.split(/,\s*/);
    }
    function extractLast(term) {
        return split(term).pop();
    }
});

function EsCombinacionEspecial(event) {
    return event.charCode === 0 ||  //Esto es para firefox: delete,supr,arrow keys.
           (event.ctrlKey && event.key === "x") ||
           (event.ctrlKey && event.key === "c") ||
           (event.ctrlKey && event.key === "v");
}

function ValidateTextboxLength(event, newvalue) {
    if (EsCombinacionEspecial(event))
        return true;

    var controlId = event.target.id;
    var textbox = $.trim($('#' + controlId).val());
    var newTextValue = textbox + newvalue;
    var lengthToCompare = 20;

    if (textbox.length === lengthToCompare || newTextValue.length > lengthToCompare)
        return false;

    return true;
}

//SOTERO BUSTAMANTE
$(".btnShowPopupEnfermedadExamen").on("click", function (e) {
    e.preventDefault();
    var x = this.id;
    var url = $(this).attr("href") + "?idOrden=" + x;
    $("#dialog-open").html("");
    $("#dialog-open").dialog({
        title: "Agregar Examen",
        autoOpen: false,
        resizable: false,
        draggable: true,
        height: 300,
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
function agregarEventosPopupEnfermedadExamen(x) {
    var paciente_genero = 3;
    var idEnfermedad = '';
    $("#idEnfermedad").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Orden/GetEnfermedadesAnalista?idOrden=" + x
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, {
        placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias"
    }).change(function () {
        idEnfermedad = $("#idEnfermedad").val();
        console.log(idEnfermedad);
        $("#idExamen").ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "Orden/GetExamenesByTipoMuestra?idOrden=" + x + "&idEnfermedad=" + idEnfermedad
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, {
            placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias"

        });
    });

    if (idEnfermedad == '') {
        idEnfermedad = $("#idEnfermedad").val();
        console.log(idEnfermedad);
    }
    //alert(idEnfermedad);

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
        placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias"
    }).change(function () {
        idLaboratorio = $("#idLaboratorio").val();
    });
    console.log(idEnfermedad);
    console.log('value');
    console.log($("#idEnfermedad").val());
    console.log('-------');
    $("#CodigoUnicoDestino").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Orden/GetLaboratoriosNew?prefix=" + request.term + "&ExamenVA=RomINS",
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
            url: URL_BASE + "Orden/GetExamenesByTipoMuestra?idOrden=" + x + "&idEnfermedad=" + idEnfermedad
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, {
            placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias"

        });
    }

    $("#dvPopupExamen").on("change", "#idExamen", function () {
        var idExamen = "";
        $("#idExamen :selected").each(function (i, selected) {

            idExamen = idExamen + "idExamen=" + $(selected).val() + "&";
        });
        $.ajax({
            url: URL_BASE + "Orden/GetTiposMuestraByIdExamen?" + idExamen + "_=_",
            cache: false,
            method: "GET"
        }).done(function (msg) {
            $("#dvPopupTipoMuestra").html(msg);
            $("#idTipoMuestra").chosen({ placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias con el texto" });
        });
    });

    //ENFERMEDAD
    var idLaboratorio = $("#idLaboratorioMuestra").val();

    $("#idExamen").chosen({
        placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias"
    });
    $("#idTipoMuestra").chosen({
        placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias"
    });


    $("input.datepickerTelerik:text").setDefaultDatePicker();
    var myDate = new Date();
    myDate.setMinutes(myDate.getMinutes() + 10);

    var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
    var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
    var anio = myDate.getFullYear();
    var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
    var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();

    var myDateColecion = new Date();
    var horaColeccion = (myDateColecion.getHours() >= 10) ? myDateColecion.getHours() : "0" + myDateColecion.getHours();
    var minColeccion = (myDateColecion.getMinutes() >= 10) ? myDateColecion.getMinutes() : "0" + myDateColecion.getMinutes();

    $("#fechaColeccionPopup").val(dia + "/" + mes + "/" + anio);
    $("#horaColeccionPopup").timeEntry({
        show24Hours: true
    });
    $("#horaColeccionPopup").val(horaColeccion + ":" + minColeccion);

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
        var idLaboratorio = $("#idLaboratorio").val();
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
        var idProyecto = $("#idProyecto").val();
        //Juan Muga
        //var fechaEnvio = $("#fechaEnvioPopup").val();
        //var horaEnvio = $("#horaEnvioPopup").val();
        var tipoRegistro = $("#tipoRegistro").val();

        if (idLaboratorio == undefined || idLaboratorio == "" || idLaboratorio == 0) {
            jAlert("Seleccione un Laboratorio.", "Alerta!");
            return null;
        }
        else if (idEnfermedad == undefined || idEnfermedad == "" || idEnfermedad == 0) {
            jAlert("Seleccione una Enfermedad.", "Alerta!");
            return null;
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

        $("#dialog-open").dialog("close");

        /*Agrega el examen*/
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
        var datos = "?datos=" + data;

        $("#dialog-open").dialog("close");
        /*Agrega el examen*/
        $.ajax(
               {
                   url: URL_BASE + "Orden/AddExamenRomINS" + datos,
                   cache: false,
                   method: "GET"
               })


    });
}

$('#chkAll').click(function (e) {
    var table = $(e.target).closest('table');
    $('td input:checkbox', table).prop('checked', this.checked);
});

$('#chkSelect').click(function (e) {
    $("#btnRecepcionMasiva").show();
});

$("#btnRecepcionMasiva").click(function (e) {
    e.preventDefault();

    var data = '';
    $('input[type=checkbox]').each(function () {
        //debugger;
        if (this.checked) {
            if (this.id != "chkAll") {
                data += $(this).val() + '|';
            }
        }

    });

    if (data != '') {
        jConfirm("Desea Recepcionar las Muestras seleccionadas?", "Recepcion masiva", function (rpta) {
            if (rpta) {
                data = data.slice(0, -1);
                RecepcionMasivaMuestras(data)
            }
        })
    }
    else {
        alert('Debes seleccionar al menos una muestra.');
        return false;
    }


});


function RecepcionMasivaMuestras(data) {
    //debugger;
    var datos = "?datos=" + data;
    $.ajax(
           {
               url: URL_BASE + "OrdenMuestra/RecepcionMuestrasMasivo" + datos,
               cache: false,
               method: "GET"
           }).done(function (msg) {
               //debugger;
               if (msg == "ok") {
                   jAlert("Las ordenes fueron recepcionadas correctamente", "Aviso", function () {
                       iBuscarClick();
                   });
               } else {
                   jAlert("No se pudo recepcionar las muestras, revise los datos.", "Error")
               }

           }).fail(function () {
               //debugger;
               jAlert("Se produjo un error de Conexión.", "Error")
           })

}
function LevantarObservacionClick(idOrden, tipoMuestra, idOrdenMuestraRecepcion, idOrdenMaterial) {
    debugger;
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "OrdenMuestra/ShowLevantarObservaciones?idOrden=" + idOrden + "&tipoMuestra=" + tipoMuestra + "&idOrdenMuestraRecepcion=" + idOrdenMuestraRecepcion + "&idOrdenMaterial=" + idOrdenMaterial,
        // data: { codigoOrden: idOrden },
        success: function (result) {
            debugger;
            $('#DataLevantarObservacion').html(result);
            //$("input.datepickerTelerik:text").setDefaultDatePicker();
            $(".datepickerMaxValue").setDatePickerWithMaxValue();
            $(".timepicker").timeEntry({ show24Hours: true });
            $(function () {
                $("#tabs").tabs();
            });
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
            $("#solicitante").ajaxChosen({
                dataType: "json",
                type: "GET",
                //minTermLength: 3,
                afterTypeDelay: 300,
                cache: false,
                url: URL_BASE + "Solicitante/GetSolicitantes"
            }, {
                loadingImg: URL_BASE + "Content/images/loading.gif"
            }, {
                placeholder_text_single: "Seleccione el Solicitante", no_results_text: "No existen coincidencias"
            }).change(function () {
                //alert($("#solicitante").val());
            });
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
            $('body').on('click', '#btnEditarObservacion', function (e) {
                e.preventDefault();
                var horaObtencion = "";
                var fechaObtencion = "";
                if ($("#hddRECHAZOFECHAOBTENCIONMUESTRA").val() == "True") {
                    var existeFechaInvalida = false;

                    $('#tableOrdenMuestra tr')
                        .each(function () {
                            var rowId = $(this).find('input[type="hidden"]').val();

                            if (rowId != undefined) {
                                //hora obtencion
                                horaObtencion = $("#horaMuestra" + rowId).val();

                                //Fecha de obtencion
                                fechaObtencion = $("#fechaMuestra" + rowId).val();

                                if (!ValidaFecha(fechaObtencion, horaObtencion, "Obtención", 1)) {
                                    existeFechaInvalida = true;
                                }
                                //if ($("#horaMuestra" + rowId).val();) Revisar si cuando finalizas valida que  ingreses todas las muestras
                            }
                        });

                    $("#hddFechaObtencion").val(fechaObtencion);
                    $("#hddHoraObtencion").val(horaObtencion);
                }
                //if ($("#hddRECHAZOEESSDESTINO").val() == "True") {
                //    $(".idLaboratorioDestinoMaterial").each(function (index, item) {
                //        var valLab = $(item).val();

                //        if (valLab === undefined || valLab === null || valLab == 0 || valLab == "") {
                //            $(item).focus();

                //            jAlert("Ingrese el nuevo Laboratorio de Destino.", "¡Alerta!");
                //            return;
                //        }
                //    });
                //}
                var datos = "?hddRECHAZOFECHAINGRESOROM=" + $("#hddRECHAZOFECHAINGRESOROM").val() + "&hddRECHAZOPACIENTE=" + $("#hddRECHAZOPACIENTE").val() + "&hddRECHAZODATOCLINICO=" + $("#hddRECHAZODATOCLINICO").val()
                    + "&hddRECHAZOCODIFICACION=" + $("#hddRECHAZOCODIFICACION").val() + "&hddRECHAZOFECHAEVALFICHA=" + $("#hddRECHAZOFECHAEVALFICHA").val() + "&hddRECHAZOFECHASOL=" + $("#hddRECHAZOFECHASOL").val()
                            + "&hddRECHAZOFECHAOBTENCIONMUESTRA=" + $("#hddRECHAZOFECHAOBTENCIONMUESTRA").val() + "&hddRECHAZOSOLICITANTE=" + $("#hddRECHAZOSOLICITANTE").val()
                            + "&hddRECHAZONROOFICIO=" + $("#hddRECHAZONROOFICIO").val() + "&hddRECHAZOEESSDESTINO=" + $("#hddRECHAZOEESSDESTINO").val() + "&fechaIngresoINS=" + $("#fechaIngresoINS").val() + "&fechaReevaluacionFicha=" + $("#fechaReevaluacionFicha").val()
                            + "&nroOficio=" + $("#nroOficioNew").val() + "&fechaSolicitud=" + $("#fechaSolicitud").val()
                + "&tipoDocumento=" + $("#tipoDocumento").val() + "&nroDocumento=" + $("#nroDocumento").val() + "&idOrden=" + $("#idOrden").val() + "&Solicitante=" + $("#solicitante").val()
                + "&fechaObtencion=" + fechaObtencion + "&horaObtencion=" + horaObtencion;
                $.ajax(
                        {
                            url: URL_BASE + "OrdenMuestra/ValidarRegistro" + datos,
                            cache: false,
                            method: "POST"
                        }).done(function (msg) {
                            if (msg.length > 1) {
                                jAlert(msg, "¡Alerta!");
                            }
                            else {
                                $("#frmOrdenMuestraObservacion").submit();
                                setTimeout(function () {
                                    dpUI.loading.start("Editando Orden ...");
                                }, 3000);
                                return true;
                            }
                        }).fail(function () {
                            //debugger;
                            jAlert("Se produjo un error de Conexión.", "Error")
                        })
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
function VerOrden(idOrden, Origen, Controlador, codigo) {
    console.log(codigo);
    debugger;
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Orden/ShowEditRom",
        data: { idOrden: idOrden, Origen: Origen, Controlador: Controlador },
        success: function (result) {
            $('#DataVerOrden2').html(result);

            $(".datepickerMaxValue").setDatePickerWithMaxValue();
            $(function () {
                $("#tabs").tabs();
            });
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
            $("#solicitante").ajaxChosen({
                dataType: "json",
                type: "GET",
                //minTermLength: 3,
                afterTypeDelay: 300,
                cache: false,
                url: URL_BASE + "Solicitante/GetSolicitantes"
            }, {
                loadingImg: URL_BASE + "Content/images/loading.gif"
            }, {
                placeholder_text_single: "Seleccione el Solicitante", no_results_text: "No existen coincidencias"
            }).change(function () {
                //alert($("#solicitante").val());
            });
            var DatoRechazo = [];
            var availableTags = [{ "value": "Error en Registro de Paciente", "data": "1" }, { "value": "Error en Registro de Establecimiento Origen", "data": "2" }];
            function split(val) {
                return val.split(/,\s*/);
            }
            function extractLast(term) {
                return split(term).pop();
            }
            $("#DataVerOrden2 #MotivoRechazoList").autocomplete({
                source: function (request, response) {
                    response($.ui.autocomplete.filter(
                      availableTags, extractLast(request.term)));
                },
                appendTo: "#autocompletedivid",
                focus: function () {
                    return false;
                },
                select: function (event, ui) {
                    var terms = split(this.value);
                    terms.pop();
                    terms.push(ui.item.value);
                    terms.push("");
                    this.value = terms.join(", ");
                    DatoRechazo.push(ui.item.data);
                    return false;
                }
            });
            $('body').on('click', '#btnRechazarOrden', function (e) {
                e.preventDefault();
                console.log(DatoRechazo);
                console.log(idOrden);
                console.log(Controlador);
                console.log(codigo);
                $.ajax(
                        {
                            url: URL_BASE + "Orden/RechazarOrden?idRechazo=" + DatoRechazo + "&idOrden=" + idOrden + "&Controlador=" + Controlador + "&codigoOrden=" + codigo,
                            cache: false,
                            method: "POST"
                        }).done(function (msg) {
                            iBuscarClick();
                        }).fail(function () {
                            //debugger;
                            jAlert("Se produjo un error de Conexión.", "Error")
                        })
            });

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

$("#btnBuscarSolicitud").on("click", function () {
    var fechaDesde = $('#datepickerDesde').val();
    var fechaHasta = $('#datepickerHasta').val();
    var codigoMuestra = $('#codigoMuestra').val();
    var formulario = $('#formulario').val();
    //debugger;
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "OrdenMuestra/ListarSolicitudSalidaMuestraRom?fechaDesde=" + fechaDesde + "&fechaHasta=" + fechaHasta + "&codigoMuestra=" + codigoMuestra + "&formulario=" + formulario,
        success: function (result) {
            var contenido = null;
            if (result.length > 0) {
                var lista = [];
                var resultado = [];
                if (result.length > 0) {
                    lista = result.split('|');
                    var contenido = "";
                    contenido += "<table id='dtMuestras' class='table - responsive'><thead><tr><th>Formulario</th><th>Nro Oficio</th><th>Código Lineal</th><th>Código de Muestra</th>";
                    contenido += "<th>Fecha Envío de Muestra</th><th>Establecimiento</th><th>Paciente</th><th>Documento de Identidad</th><th></th><th></th></tr></thead>";
                    for (var i = 0; i < lista.length; i++) {
                        resultado = lista[i].split(',');
                        contenido += "<tr><td>" + resultado[1] + "</td><td>" + resultado[2] + "</td><td>" + resultado[3] + "</td><td>" + resultado[4] + "</td><td>" + resultado[5] + "</td>";
                        contenido += "<td>" + resultado[6] + "</td><td>" + resultado[7] + "</td><td>" + resultado[8] + "</td>";
                        contenido += "<td><a href='/OrdenMuestra/ExportarSolicitudExcel?formato=" + resultado[1] + "' class='lnkForm'><img src='/img/excel_download.gif' /></a></td>";
                        contenido += "<td><a href='/OrdenMuestra/ExportarSolicitudPDF?formato=" + resultado[1] + "' target='_blank' class='lnkForm'><img src='/img/imgPdf.gif' /></a></td></tr >";
                    }
                    contenido += "</table>";
                }
            }
            else {
                contenido = "<div class='alert alert-danger'>No se encontraron registros de muestras enviadas a Laboratorio</div>";
            }
            $("#LisMx").html(contenido);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
});


function EnvioMuestraROM() {
    debugger;
    if ($("#CodigoMuestra").val().length > 0 || $("#Oficio").val().length > 0) {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "OrdenMuestra/EnvioMuestraRom?codigoMuestra=" + $("#CodigoMuestra").val() + "&nroOficio=" + $("#Oficio").val(),
            success: function (result) {
                var data = result;
                if (data == '0') {
                    jAlert('Muestra no pertenece a su Establecimiento', 'Alerta!');
                    return false;
                }
                else if (data == '1') {
                    jAlert('Muestra ya fue enviada a Laboratorio', 'Alerta!');
                    return false;
                }
                else
                {
                    console.log(data);
                    var rows = [];
                    rows = data.split("|");
                    for (var i = 0; i < rows.length; i++) {
                        var recepcion = [];
                        recepcion = rows[i].split(",");
                        var fila = "";
                        fila += "<tr>";
                        fila += "<td>" + recepcion[1] + "</td>" + "<td>" + recepcion[2] + "</td>" + "<td class='id'>" + recepcion[3] + "</td>" + "<td>" + recepcion[4] + "</td>" + "<td>" + recepcion[5] + "</td>" + "<td>" + recepcion[6] + "</td>";
                        fila += "</tr>";
                        $(fila).appendTo($("#filasMuestra"));
                    }
                    
                    $("#CodigoMuestra").val("");
                    $("#Oficio").val("");
                    $("#CodigoMuestra").focus();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest);
            }
        });
    }
}


function GenerarSolicitudSalidaMuestra() {
    debugger;
    var codigoMuestra = '';
    $(".id").parent("tr").find(".id").each(function () {
        codigoMuestra += $(this).html() + '|';
    }); 

    if (codigoMuestra.length > 0) {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "OrdenMuestra/GeneraSolicitudSalidaMuestrasRom?codigoMuestra=" + codigoMuestra,
            success: function (result) {
                var data = result;
                if (data.length > 0) {
                    jAlert('Se generó la Solicitud de Salida de Muestra con el Número ' + data, 'Alerta', function () {
                        debugger;
                        fnExcelReport(data);
                        //ExportPDF(data);
                        $('#filasMuestra').html('');
                        $("#CodigoMuestra").val("");
                    });                    
                }
            },
        });
    }
}

//function ExportRecepcionMuestras() {
//    debugger;
//    var tab_text = "<table border='2px'><tr><b>Lista de Muestras recibidas o por recibir</b></tr><tr bgcolor='#87AFC6'>";
//    var tab = document.getElementById('dtorden'); // id of table

//    for (var j = 0; j < tab.rows.length; j++) {
//        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
//    }
//    tab_text = tab_text + "</table>";
//    var ua = window.navigator.userAgent;
//    var msie = ua.indexOf("MSIE ");

//    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
//    {
//        txtArea1.document.open("txt/html", "replace");
//        txtArea1.document.write(tab_text);
//        txtArea1.document.close();
//        txtArea1.focus();
//        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
//    }
//    else                 //other browser not tested on IE 11
//        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

//    return (sa);
//}

function fnExcelReport(formato) {
    debugger;
    var tab_text = "<table border='2px'><tr><b>Formato Salida de Muestras: " + formato + "</b></tr><tr bgcolor='#87AFC6'>";
    var tab = document.getElementById('ForMx'); // id of table

    for (var j = 0; j < tab.rows.length; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    }
    tab_text = tab_text + "</table>";
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    }
    else                 //other browser not tested on IE 11
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}

function ExportPDF(formato) {
    debugger;
    //var pdf = new jsPDF('p', 'pt', 'letter');
    //// source can be HTML-formatted string, or a reference
    //// to an actual DOM element from which the text will be scraped.
    //source = $('#ForMx')[0];

    //// we support special element handlers. Register them with jQuery-style 
    //// ID selector for either ID or node name. ("#iAmID", "div", "span" etc.)
    //// There is no support for any other type of selectors 
    //// (class, of compound) at this time.
    //specialElementHandlers = {
    //    // element with id of "bypass" - jQuery style selector
    //    '#bypassme': function (element, renderer) {
    //        // true = "handled elsewhere, bypass text extraction"
    //        return true
    //    }
    //};
    //margins = {
    //    top: 80,
    //    bottom: 60,
    //    left: 40,
    //    width: 522
    //};
    //// all coords and widths are in jsPDF instance's declared units
    //// 'inches' in this case
    //pdf.fromHTML(
    //    source, // HTML string or DOM elem ref.
    //    margins.left, // x coord
    //    margins.top, {// y coord
    //    'width': margins.width, // max width of content on PDF
    //    'elementHandlers': specialElementHandlers
    //},
    //    function (dispose) {
    //        // dispose: object with X, Y of the last line add to the PDF 
    //        //          this allow the insertion of new lines after html
    //        pdf.save(formato + '.pdf');
    //    }
    //    , margins);



    //var pdf = new jsPDF('p', 'pt', 'letter');

    //pdf.cellInitialize();
    //pdf.setFontSize(10);
    //$.each($('#ForMx tr'), function (i, row) {
    //    $.each($(row).find("td, th"), function (j, cell) {
    //        var txt = $(cell).text().trim() || " ";
    //        var width = (j == 4) ? 40 : 70; //make 4th column smaller
    //        pdf.cell(10, 50, width, 30, txt, i);
    //    });
    //});

    //pdf.save('sample-file.pdf');


    
}
