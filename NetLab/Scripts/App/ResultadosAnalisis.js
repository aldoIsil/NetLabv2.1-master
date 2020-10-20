/*
1    TEXTO SIMPLE
2    FECHA
3    SI O NO
4    CAMPO NUMERICO
5    TEXTO AMPLIADO
6    COMBO DE SELECCION
*/


var myDate = new Date();

var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
var anio = myDate.getFullYear();

var closeToChargeResultados = false;
var htmlToCharge = "";
var activeEdit;

var idOrdenSel = "";
var idExamenSel = "";

var lista = [];
var campos = [];
var camposPadre = [];
var camposHijos = [];
var idCombo;
var auxHtmlContent = '';
var camposIdordenexamen = '';
var camposIdOrdenResultadoAnalito = '';
var todosResultados = [];
var enableButtons = true;
var BoolFinalizar = false;


$('body').on('click', '#btnEditar', function (e) {
    e.preventDefault();
    setTimeout(function () {
        $("#frmOrden").submit();
        dpUI.loading.start("Editando Orden ...");
    }, 3000);
});
$(document).ready(function () {

    $.ajaxSetup({ cache: false });
    /*se cambio valor a 100% de los scroll 29-5-19*/
    // This will make every element with the class "date-picker" into a DatePicker element
   
    //$(document).on("click", "#HCPaciente", function (e) {
    //    e.preventDefault();
    //var url = $(this).attr("href");
    //$("#dialog-edit").dialog({
    //    title: "Historia Clinica del Paciente",
    //    css: "modal",
    //    autoOpen: false,
    //    resizable: false,
    //    show: { effect: "drop", direction: "up" },
    //    open: function () {
    //        $(this).load(url);
    //    },
    //    close: function () {
    //        $(this).dialog("close");
    //    }
    //});

    //$("#dialog-edit").dialog("open");
    //    return false;
    //});
    if ($("#datepickerDesde").val() == "") {
        var modDateYesterday = new Date(myDate - 86400000);
        var diaY = (modDateYesterday.getDate() >= 10) ? modDateYesterday.getDate() : "0" + modDateYesterday.getDate();
        var mesY = ((modDateYesterday.getMonth() + 1) >= 10) ? (modDateYesterday.getMonth() + 1) : "0" + (modDateYesterday.getMonth() + 1);
        var anioY = modDateYesterday.getFullYear();
        $("#datepickerDesde").val(diaY + "/" + mesY + "/" + anioY);

        $("#datepickerHasta").val(dia + "/" + mes + "/" + anio);
    }

    $("#datepickerDesde").kendoDatePicker({
        culture: "es-PE"
    });
    $("#datepickerHasta").kendoDatePicker({
        culture: "es-PE"
    });
    $("#btnCargaExcel").on("click", function (e) {
        e.preventDefault();
        var url = URL_BASE + "ResultadosAnalisis/CargaResultadoMasivo";
        $("#dialog-load").dialog({
            title: "Resultados",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 400,
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
                activeEdit = this;
            },
            close: function () {
            }
        });


        //$("#dialog-edit").on('dialogclose', function () { alert(4); });
        $("#dialog-load").dialog("open");
        return false;
    });
    $(".recepcionPendiente").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        idOrdenSel = $(this).attr('idOrden');
        idExamenSel = $(this).attr('idExamen');
        $("#dialog-edit").dialog({
            title: "Recepción Pendiente",
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
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);
                activeEdit = this;
            },
            close: function () {
            }
        });


        //$("#dialog-edit").on('dialogclose', function () { alert(4); });
        $("#dialog-edit").dialog("open");
        return false;
    });

    $(".validarMuestras").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        idOrdenSel = $(this).attr('idOrden');
        $("#dialog-edit").dialog({
            title: "Validar Muestras",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 1200,
            position: {
                my: "center top",
                at: ("center top+" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);
                activeEdit = this;
            },
            close: function () {
            }
        });


        //$("#dialog-edit").on('dialogclose', function () { alert(4); });
        $("#dialog-edit").dialog("open");
        return false;
    });

    $(".muestraValidada").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        idOrdenSel = $(this).attr('idOrden');
        idExamenSel = $(this).attr('idExamen');
        $("#dialog-edit").dialog({
            title: "Muestra Validada",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 1200,
            position: {
                my: "center top",
                at: ("center top+" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);
                activeEdit = this;
            },
            close: function () {
            }
        });


        //$("#dialog-edit").on('dialogclose', function () { alert(4); });
        $("#dialog-edit").dialog("open");
        return false;
    });


    $("#ValidarMuestra").on("click", ".conformeMuestra", function (e) {
        //alert('1');
        if ($(this).is(':checked')) {
            //alert('2');
            $(this).closest("tr").find('.criterioRechazo').attr('disabled', true);
            $(this).closest("tr").find('.SumoSelect').hide();
        }
        else {
            //alert('3');
            $(this).closest("tr").find('.criterioRechazo').removeAttr('disabled');
            $(this).closest("tr").find('.SumoSelect').show();
        }
    });

    $(".ingresarResultados").on("click", function (e) {

        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Seleccionar Área de Procesamiento",
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
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);
                activeEdit = this;
            },
            close: function () {
                closeAPForm();
            }
        });


        //$("#dialog-edit").on('dialogclose', function () { alert(4); });
        $("#dialog-edit").dialog("open");
        return false;
    });

    $("#dialog-edit").on("click", ".irIngresarResultados", function (e) {
        e.preventDefault();
        var idOrden = $(this).attr('idorden');
        var idArea = $(this).attr('idarea');
        var idExamen = $(this).attr('idexamen');
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr('href'),
            data: { id: idOrden, idArea: idArea, idExamen: idExamen },
            success: function (result) {
                htmlToCharge = result;
                closeToChargeResultados = true;
                $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
            }
        });

        return false;
    });

    function closeAPForm() {

        if (closeToChargeResultados) {
            closeToChargeResultados = false;
            $("#dialog-edit").dialog({
                title: "Registrar Resultados",
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
                modal: true,
                draggable: true,
                open: function () {
                    $(this).html(htmlToCharge);
                    activeEdit = this;
                    if ($("#EnabledButton").val() == '1') {
                        $("#lnkRegistrarResultados").show();
                        $("#lnkFinalizarResultados").hide();
                    }
                    else {
                        $("#lnkRegistrarResultados").hide();
                        $("#lnkFinalizarResultados").hide();
                    }
                },
                close: function () {
                    $(this).dialog("close");
                }
            });
            $("#dialog-edit").dialog("open");
        }
    };

    $("#dialog-edit").on("focusout", ".resultadoAnalito", function (e) {
        var elem = this;
        setTimeout(function () {
            validarResultado(elem);
        }, 300);

    });

    function validarResultado(itemElement) {
        //alert($(itemElement).closest("tr").attr("validacion"));
        var validacion = null;
        if ($(itemElement).closest("tr").attr("validacion")) {
            validacion = JSON.parse($(itemElement).closest("tr").attr("validacion"));
        }

        var valor = $(itemElement).val();
        //alert(validacion.Tipo);
        //alert(valor);
        if (valor != "" && validacion) {
            if (validacion.Tipo == 1) {
                var minimo = 0, maximo = 0, validar = 0;
                minimo = parseFloat(validacion.Minimo);
                maximo = parseFloat(validacion.Maximo);
                validar = parseFloat(valor);
                //if (validar < minimo || validar > maximo) {
                //    jAlert("El resultado esta fuera del rango normal.", "Alerta!");
                //}
            }

            if (validacion.Tipo == 3) {
                //validar opcion
                var valid = false;

                if (valor == validacion.ValorNormal) {
                    valid = true;
                }

                //if (!valid) {
                //    jAlert("El resultado no es normal.", "Alerta!");
                //}
                valid = true;
            }
        }
    };

    $("#dialog-edit").on("click", "#CerrarPopUp", function () {
        //location.reload();
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    });

    $("#dialog-edit").on("click", "#CerrarPopUpIngresoResultados", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    });
    //Autor : Juan Muga
    //Fecha Creacion: 21/08/2018
    //Descripcion: Creacion de los datos tipo json para el registro.
    $("body #dialog-edit, body #IngresarResultado").on("click", "#lnkRegistrarResultados", function (e) {
        e.preventDefault();
        var jsonvalues = [];
        var selectItemValues = [];
        var observaciones = [];
        var val1 = 0;
        var val2 = 0;
        var res = 0;
        debugger;
        $.each(document.getElementsByClassName("classSelect"), function (i, item) {
            if (!item.value) {
                jAlert("Por favor, llene todos los resultados.", "Alerta!");
            } else {
                jsonvalues.push({ "Tipo": "combo", "Id": item.id, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
        });

        $.each(document.getElementsByClassName("classCheckbox"), function (i, item) {
            if (item.checked) {
                jsonvalues.push({ "Tipo": "checkbox", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
            //else {
            //    jAlert("Por favor, llene todos los resultados.", "Alerta!");
            //}
        });

        $.each(document.getElementsByClassName("observacion"), function (i, item) {
            observaciones.push({ "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito, "ObservacionContent": item.value });
        });
        $.each(document.getElementsByClassName("classInputtext"), function (i, item) {
            if (item.dataset.optid == 4036 || item.dataset.optid == 4043 || item.dataset.optid == 4040) {
                if (Number.isNaN(Number.parseFloat(item.value))) {
                    val1 = 0;
                }
                else {
                    val1 = parseFloat(item.value);
                }
            }
            if (item.dataset.optid == 4038 || item.dataset.optid == 4044 || item.dataset.optid == 4041) {
                if (Number.isNaN(Number.parseFloat(item.value))) {
                    val2 = 0;
                }
                else {
                    val2 = parseFloat(item.value);
                }
            }
            if (item.dataset.optid == 4046 || item.dataset.optid == 4045 || item.dataset.optid == 4047) {
                if (Number.isNaN(Number.parseFloat(val1))) {
                    val1 = 0;
                }
                if (Number.isNaN(Number.parseFloat(val2))) {
                    val2 = 0;
                }
                res = val1 / val2;

                jsonvalues.push({ "Tipo": "inputtext", "Id": item.dataset.optid, "Value": res.toString(), "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
            else {
                jsonvalues.push({ "Tipo": "inputtext", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
            
        });

        $.each(document.getElementsByClassName("classInputnumber"), function (i, item) {
            if (item.value) {
                jsonvalues.push({ "Tipo": "inputnumber", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            } else {
                jAlert("Por favor, llene todos los resultados.", "Alerta!");
            }
        });

        $.each(document.getElementsByClassName("classInputdate"), function (i, item) {
            if (item.value) {
                if (item.className.indexOf("k-datepicker") < 0) {
                    jsonvalues.push({ "Tipo": "inputdate", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
                }
            } else {
                jAlert("Por favor, llene todos los resultados.", "Alerta!");
            }
        });

        $.each(document.getElementsByClassName("classTextarea"), function (i, item) {
            jsonvalues.push({ "Tipo": "textarea", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
        });

        document.getElementById("jsonvalues").value = JSON.stringify(jsonvalues);
        document.getElementById("observacionesjsonvalues").value = JSON.stringify(observaciones);
        console.log(observaciones);

        var ok = false;

        $.each($("input[name='resultados']:checked"), function () {
            ok = true;
        });

        if (ok) {
            $(".resultadoAnalito").each(function () {
                if ($(this)[0].className.indexOf("k-datepicker") < 0) {
                    var valorRes = $(this).val();
                    if (valorRes.length === 0 || valorRes == '4050') {
                        ok = false;
                    }
                }
            });
            if (ok) {
                $("#formRegistroResultados").submit();
            } else {
                jAlert("Por favor, llene todos los resultados.", "Alerta!");
            }
        } else {
            jAlert("Por favor, seleccione al menos un examen.", "Alerta!");
        }


    });

    $("body #dialog-edit, body #IngresarResultado").on("click", "#lnkFinalizarResultados", function () {
        //Registro de Resultados, modificación antes de finalizar
        debugger;
        var jsonvalues = [];
        var selectItemValues = [];
        var observaciones = [];
        $.each(document.getElementsByClassName("classSelect"), function (i, item) {
            if (!item.value) {
                jAlert("Por favor, llene todos los resultados.", "Alerta!");
            } else {
                jsonvalues.push({ "Tipo": "combo", "Id": item.id, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
        });

        $.each(document.getElementsByClassName("classCheckbox"), function (i, item) {
            if (item.checked) {
                jsonvalues.push({ "Tipo": "checkbox", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
            //else {
            //    jAlert("Por favor, llene todos los resultados.", "Alerta!");
            //}
        });

        $.each(document.getElementsByClassName("observacion"), function (i, item) {
            observaciones.push({ "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito, "ObservacionContent": item.value });
        });

        $.each(document.getElementsByClassName("classInputtext"), function (i, item) {
            if (item.dataset.optid == 4036 || item.dataset.optid == 4043 || item.dataset.optid == 4040) {
                if (Number.isNaN(Number.parseFloat(item.value))) {
                    val1 = 0;
                }
                else {
                    val1 = parseFloat(item.value);
                }
            }
            if (item.dataset.optid == 4038 || item.dataset.optid == 4044 || item.dataset.optid == 4041) {
                if (Number.isNaN(Number.parseFloat(item.value))) {
                    val2 = 0;
                }
                else {
                    val2 = parseFloat(item.value);
                }
            }
            if (item.dataset.optid == 4046 || item.dataset.optid == 4045 || item.dataset.optid == 4047) {
                if (Number.isNaN(Number.parseFloat(val1))) {
                    val1 = 0;
                }
                if (Number.isNaN(Number.parseFloat(val2))) {
                    val2 = 0;
                }
                res = val1 / val2;

                jsonvalues.push({ "Tipo": "inputtext", "Id": item.dataset.optid, "Value": res.toString(), "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
            else {
                jsonvalues.push({ "Tipo": "inputtext", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
//            jsonvalues.push({ "Tipo": "inputtext", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
        });

        $.each(document.getElementsByClassName("classInputnumber"), function (i, item) {
            jsonvalues.push({ "Tipo": "inputnumber", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
        });

        $.each(document.getElementsByClassName("classInputdate"), function (i, item) {
            if (item.className.indexOf("k-datepicker") < 0) {
                jsonvalues.push({ "Tipo": "inputdate", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
            }
        });

        $.each(document.getElementsByClassName("classTextarea"), function (i, item) {
            jsonvalues.push({ "Tipo": "textarea", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
        });

        document.getElementById("jsonvalues").value = JSON.stringify(jsonvalues);
        document.getElementById("observacionesjsonvalues").value = JSON.stringify(observaciones);
        console.log(observaciones);

        var ok = false;

        $.each($("input[name='resultados']:checked"), function () {
            ok = true;
        });
        //alert($(this).attr('idorden'));
        //alert($(this).attr('ordenExamen'));
        //alert($(this).attr('estatusOrdenExamen'));
        //alert($(this).attr('estatusOrdenExamen'));
        if (ok) {

            $(".resultadoAnalito").each(function () {
                if ($(this)[0].className.indexOf("k-datepicker") < 0) {
                    var valorRes = $(this).val();

                    if (valorRes.length === 0) {
                        ok = false;
                    }
                }
            });

            if (ok) {
                BoolFinalizar = true;
                $("#formRegistroResultados").submit();
                var selectedElements = [];

                $.each($("input[name='resultados']:checked"), function () {
                    selectedElements.push($(this).val());
                });

                if (selectedElements.length > 0) {
                    $.ajax({
                        type: "POST",
                        cache: false,
                        url: URL_BASE + "ResultadosAnalisis/Finalizar",
                        data: JSON.stringify(selectedElements),
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            actualizarCabecera();
                            verDetalle();
                            jAlert("Se ha finalizado el ingreso de resultados", "Aviso");
                            iBuscarClick();
                        }
                    });
                } else {
                    jAlert("Por favor, seleccione al menos un examen.", "Alerta!");
                }
            } else {
                jAlert("Por favor, llene todos los resultados.", "Alerta!");
            }
        } else {
            jAlert("Por favor, seleccione al menos un examen.", "Alerta!");
        }
        //

        //iBuscarClick();
    });

    $('#ValidarMuestra').on('click', '.conformeMuestra', function (e) {

        if ($(this).is(':checked')) {
            //alert("SIIIII");
            $(this).closest("tr").find('.selMotivoRechazoDiv').hide();
        } else {
            var isFirst = $(this).attr("isFirst");
            //alert("veee"+isFirst);
            if (isFirst == "1") {
                $(this).attr("isFirst", "0");
                var itemEvent = this;
                $(this).closest("tr").find('.selMotivoRechazoDiv').show();

                //setTimeout(function () {
                //    $(itemEvent).closest("tr").find('.selMotivoRechazo').SumoSelect({ placeholder: 'Seleccione Motivo de Rechazo' });
                //}, 300);
            } else {
                //   $(this).closest("tr").find('.selMotivoRechazo').removeAttr('disabled');
                $(this).closest("tr").find('.selMotivoRechazoDiv').show();
            }
        }
    });

    $('#dialog-edit').on('click', '#lnkPendienteRecepcion', function (e) {
        $('#formPendienteRecepcion').submit();
    });

    $('body').on('click', '#btnlnkPendienteRecepcion', function (e) {
        //alert(1)
        //console.log(this);
        //var formobj = $("body #formPendienteRecepcion");
        //console.log(formobj);
        $('body #formPendienteRecepcion').submit();
    });

    $('body').on('click', '#btnlnkValidarMuestras', function (e) {
        //$('#formValidarMuestras').submit();
        $('body #formValidarMuestras').submit();
    });

    $("body").on("submit", "#formPendienteRecepcion", function (e) {
        var i = 0;
        //alert($(this).serialize());
        //alert($(this).attr("action"));
        e.preventDefault();
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function (response) {
                //   $("#lnkVM_" + idOrdenSel).hide();
                //   $("#lnkIR_" + idOrdenSel).show();
                //alert('success');
                var $resp = $(response);
                $("#divFechaRecepcion").removeClass("hidden");
                $("#fechaRecepcion").text($resp.find("#fechaRecepcion").text());
                i = 1;
                jAlert("Recepción Realizada Satisfactoriamente ", "Aviso", function () {
                    iBuscarClick();
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest);
            }
        });
        if (i > 0) {

        }

        e.preventDefault();
        return false;
    });

    $("body").on("submit", "#formValidarMuestras", function (e) {
        e.preventDefault();
        debugger;
        var criterioRechazo = "";
        var ok = true;
        var bRechazo89 = false;
        var action = $(this).attr("action");
        var dataSerializer = $(this).serialize();
        $(".conformeMuestra").each(function (index, item) {
            if (!$(item).is(":checked")) {
                criterioRechazo = $(this).closest("tr").find(".selMotivoRechazo").val();
                if (criterioRechazo === undefined || criterioRechazo === null) {
                    ok = false;
                }
                else {
                    //Inicio Juan Muga
                    $.each(criterioRechazo, function (key, value) {  
                        if (value == "89") {
                            bRechazo89 = true;
                        }
                    });
                    //Fin Juan Muga
                }
            }
        });
        console.log($(this).attr("action"));
        console.log($(this).serialize());
        //if (ok && bRechazo89) {
        //    jConfirm('Ud. ha selccionado el rechazo: FECHA DE INGRESO AL ROM-INS INCORRECTA, SI CONTINUA SE RECHAZARÁ EL TOTAL DE MUESTRAS DEL OFICIO. ¿Desea Continuar?',
        //        'Confirmar Registro', function (r) {
        //            if (r) {
        //                $.ajax({
        //                    type: "POST",
        //                    cache: false,
        //                    url: action,
        //                    data: dataSerializer,
        //                    success: function (response) {
        //                        var $resp = $(response);
        //                        $("#fechaRecepcion").text($resp.find("#fechaRecepcion").text());
        //                        jAlert("Las muestras han sido procesadas correctamente", "Aviso", function () {
        //                            iBuscarClick();
        //                        });
        //                    },
        //                    error: function (XMLHttpRequest, textStatus, errorThrown) {
        //                        console.log(XMLHttpRequest);
        //                    }
        //                });
        //            }
        //            else {
        //                return false;
        //            }
        //        });            
        //}
        if (ok)
        {
            $.ajax({
                type: "POST",
                cache: false,
                url: $(this).attr("action"),
                data: $(this).serialize(),
                success: function (response) {
                    var $resp = $(response);
                    $("#fechaRecepcion").text($resp.find("#fechaRecepcion").text());
                    jAlert("Las muestras han sido registradas correctamente", "Aviso", function () {
                        iBuscarClick();
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(XMLHttpRequest);
                }
            });
        }
        else {
            jAlert("Por favor, seleccione los motivos de rechazo para todas las muestras no conformes.", "Alerta!");
        }


        return false;
    });

    $("body").on("submit", "#formRegistroResultados", function (e) {
        e.preventDefault();

        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function () {
                if (!BoolFinalizar) {
                    jAlert("Los resultados se registraron satisfactoriamente.", "Registro Correcto", function () {
                        //Habilitar Finalizar y deshabilitar Guardar
                        $("#lnkFinalizarResultados").show();
                        $("#lnkRegistrarResultados").hide();
                    });
                }
            }
        });
    });

    $('#tipoOrden').change(function (e) {
        iBuscarClick();
    });

    $(':radio[name=esFechaRegistro]').change(function () {
        iBuscarClick();
    });

    $('#estadoResultado').change(function (e) {
        iBuscarClick();
    });
    if (event.keyCode === 13) {
        iBuscarClick();
    }
    //Descripción:Envío de resultados a otros al EQhali 
    //Author: Juan Muga.
    //function EnviarRespuesta(idPopup) {
    //    debugger;
    //    if (idPopup.length > 0) {
    //        var url = 'http://localhost:27110/Token';
    //        var body = {
    //            grant_type: 'password',
    //            username: 'wsnetlab2',
    //            password: '0'
    //        };
    //        //Get Token
    //        $.ajax({
    //            url: url,
    //            method: 'POST',
    //            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
    //            data: body,
    //            success: function (result) {
    //                debugger;
    //                Token = result.access_token;
    //                console.log(Token);
    //                if (Token.length > 0) {
    //                    $.ajax({
    //                        url: URL_BASE + "OrdenMuestra/GetTramaResultadobyCodigoOrden?idPopup=" + idPopup,
    //                        cache: false,
    //                        method: "GET",
    //                        dataType: "json",
    //                        contentType: "application/json; charset=utf-8",
    //                        success: function (result) {
    //                            debugger;
    //                            console.log(result);
    //                            if (result.length > 0) {
    //                                $.ajax({
    //                                    url: 'http://localhost:27110/api/paciente/ReadResultado?Resultado=' + JSON.stringify(result),
    //                                    type: 'GET',
    //                                    data: { Resultado: JSON.stringify(result) },
    //                                    contentType: "application/json;chartset=utf-8",
    //                                    headers: { "Authorization": Token },
    //                                    success: function (response) {
    //                                        debugger;
    //                                        console.log(JSON.stringify(response));
    //                                        $.ajax({
    //                                            url: URL_BASE + "OrdenMuestra/UpdateTramaResultadobyCodigoOrden?idPopup=" + idPopup,
    //                                            cache: false,
    //                                            method: "GET"
    //                                        });
    //                                    },
    //                                    statusCode: {
    //                                        statusCode: {
    //                                            200: function () {
    //                                                alert('200');
    //                                            },
    //                                            404: function () {
    //                                                alert('404');
    //                                            },
    //                                            500: function () {
    //                                                alert('500');
    //                                            }
    //                                        }
    //                                    },
    //                                    error: function (xhr) {
    //                                        alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
    //                                    }
    //                                });
    //                            }
    //                        },
    //                        error: function (jqXHR) {
    //                            alert(jqXHR.responseText);
    //                        }
    //                    }).fail(function () {
    //                        //debugger;
    //                        jAlert("Se produjo un error de Conexión.", "Error")
    //                    })
    //                }

    //            },
    //            error: function (jqXHR) {
    //                alert(jqXHR.responseText);
    //            }
    //        });
    //    }
    //}
});
function HPacienteClick(NroDocumento) {
    debugger;
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Paciente/ShowHistoriaClinicaPaciente",
        data: { page: 1, NroDocumento: NroDocumento },
        success: function (result) {

            $('#Dato').html(result);
            $('#dtPacienteBusqueda').DataTable({
                "scrollY": "560px",
                "scrollX": "100%",
                "scrollCollapse": true
            });
            $('.dataTables_length').addClass('bs-select');
        }
    });
}
function ShowPLantillaSelector(idOrden) {
    //alert(idOrden);
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/ShowSelectorPlantilla",
        data: { idOrden: idOrden },
        success: function (result) {
            $('#DatoPlantilla').html(result);
        }
    });
}
function btnSelectPlantilla() {
    var idPlantilla = $("#idPlantilla").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/SelectPlantilla",
        data: { idplantilla: idPlantilla },
        success: function (result) {
            iBuscarClick();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

function RecepcionPendienteClick(idOrden, idExamen) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/RecepcionPendiente2",
        data: { id: idOrden, idExamen: idExamen },
        success: function (result) {
            $('#Dato').html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

function ValidarMuestrasClick(idOrden, idExamen) {
    $.ajax({
        type: "GET",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/ValidarMuestras2",
        data: { id: idOrden, idExamen: idExamen },
        success: function (result) {
            $('#DatoValidarMuestra').html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

function MuestraValidadaClick(idOrden, idExamen) {
    $.ajax({
        type: "GET",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/MuestrasValidadas",
        data: { id: idOrden, idExamen: idExamen },
        success: function (result) {
            $('#DatoMuestraValidada').html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

function SeleccionarAPClick(idOrdenExamen) {
    $.ajax({
        type: "GET",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/SeleccionarAP",
        data: { idOrdenExamen: idOrdenExamen },
        success: function (result) {
            $('#DataSeleccionarAP').html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

function VerOrden(idOrden, Origen, Controlador,codigo) {
    console.log(codigo);
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
            //$('#codigoOrden').val();
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
                //console.log(idOrden);
                //console.log(Origen);
                //console.log(codigo);
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
//IngresarResultadoClick
function IngresarResultadoClick(idOrden, idOrdenExamen, idArea) {
    debugger;
    $.ajax({
        type: "GET",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/IngresarResultados",
        data: { id: idOrden, idArea: idArea, idOrdenExamen: idOrdenExamen},
        success: function (result) {
            $('#DataIngrersarResultado').html(result);
            if ($("#EnabledButton").val() == '1') {
                $("#lnkRegistrarResultados").show();
                $("#lnkFinalizarResultados").hide();
            }
            else {
                $("#lnkRegistrarResultados").hide();
                $("#lnkFinalizarResultados").hide();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
function actualizarCabecera() {
    $.each($("input[name='resultados']:checked"), function () {
        var idOrdenExamen = $(this).val();
        $("select[id='" + idOrdenExamen + "']").attr("disabled", "disabled");
    });
}

function verDetalle() {
    //alert('verDetalle');
    debugger;
    document.getElementById("divDetalle").innerHTML = "";
    var selectedElements = [];
    var idPlataforma = $('#ListaPlataforma').val();

    var nombreLocal = $('#nombreLocal').val();
    var validaLabo = false;
    if (nombreLocal == 'INS VIRUS RESPIRATORIO' || nombreLocal == 'HOSPITAL NACIONAL ALBERTO SABOGAL SOLOGUREN DE LA RED ASISTENCIAL SABOGAL') {
        validaLabo = true;
    }

    if (idPlataforma == -1 && validaLabo == true) {
        jAlert('Seleccione Plataforma de procesamiento', 'Alerta!');
        $("#resultados").attr('checked', false);
        return false;
    }

    $.each($("input[name='resultados']:checked"), function () {
        selectedElements.push($(this).val());
    });

    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/ListarDetalle",
        data: JSON.stringify({ idOrdenExamen: selectedElements, idPlataforma: idPlataforma }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            crearVista(data);
            mostrarOpciones();
            //document.getElementById("ListaPlataforma").disabled = true;

            if ($("input[name='metodo[]']").attr("disabled") == false) {
                $("#lnkRegistrarResultados").show();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
//Autor : Juan Muga
//Fecha Creacion: 21/08/2018
//Descripcion: Carga y muestra todas las opciones configurados para un componente
function verDetalleOpcion(x) {
    var idOpcion = x.trim();
    //alert(idOpcion);
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/DetalleOpcion?idOpcion=" + idOpcion,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            crearVista(data);
            crearCombo();
            mostrarOpciones();

            if ($("input[name='metodo[]']").attr("disabled") == false) {
                $("#lnkRegistrarResultados").show();
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
//Autor : Juan Muga
//Fecha Creacion: 21/08/2018
//Descripcion: Carga y muestra la vista para la visualizacion del ingreso de resultados
function crearVista(rpta) {
    enableButtons = $("#EnabledButton").val();
    lista = JSON.parse(rpta);
    debugger;
    var contenido = "";
    var fireChangeEvent = false;
    var fireCheckEvent = false;
    var objId = '';
    var objValue = '';
    var triggerObjs = [];
    var triggerDatepicker = false;
    var triggernumber = false;
    var estatusObj = [];
    if (rpta != "") {
        pos = 0;
        var nRegistros = lista.length;
        contenido += "<table class='table'><thead><tr>";
        contenido += "<th class='rowExamen2' style='width:30%;'>";
        contenido += "Examen</th>";
        contenido += "<th class='rowExamen2' style='width:15%;'>Componente</th>";
        contenido += "<th class='rowExamen2' style='width:20%;'>Resultado</th>";
        contenido += "<th class='rowExamen2' style='width:15%;'>Observaciones</th>";
        contenido += "<th class='rowExamen2' style='width:10%;'>Unidad</th>";
        contenido += "<th class='rowExamen2' style='width:10%;'>Valor Normal / Referencia</th>";
        contenido += "</tr></thead>";
        contenido += "<tbody>";
        for (var i = 0; i < nRegistros; i++) {
            todosResultados = lista[i]["Resultado"] ? JSON.parse(lista[i]["Resultado"]) : [];
            var iMetodos = lista[i]["Metodos"];
            var estatusE = lista[i]["estatusE"];
            estatusObj.push({ "Id": lista[i]["IdOrdenExamen"], "status": lista[i]["estatusE"] });
            if (iMetodos) {
                $.each(iMetodos, function (key, value) {
                    var itemCampos = campos.filter(v=> v.IdAnalitoOpcionResultado == value.IdAnalitoOpcionResultado && v.IdOrdenExamen == value.IdOrdenExamen && v.IdOrdenResultadoAnalito == value.IdOrdenResultadoAnalito);
                    if (itemCampos && itemCampos.length == 0) {
                        campos.push(value);
                    }
                });
            }

            camposIdordenexamen = lista[i]["IdOrdenExamen"];
            camposIdOrdenResultadoAnalito = lista[i]["IdOrdenResultadoAnalito"];
            contenido += "<tr class='rowResultados' id='" + lista[i]["IdOrdenExamen"] + "' data-idordenexamen='" + lista[i]["IdOrdenExamen"] + "'>";
            contenido += "<td>";
            contenido += lista[i]["Examen"];
            contenido += "</td>";
            contenido += "<td>";
            contenido += lista[i]["Analito"];
            contenido += "</td>";

            contenido += "<td>";
            var tipo = lista[i]["tipo"];
            if (tipo == 1 || tipo == 4) {
                var inputType = tipo == 1 ? "text" : "number";
                var valueInput = todosResultados.filter(v=> v.Id == lista[i]["IdOrdenResultadoAnalito"]);
                contenido += "<input type='" + inputType + "'  class='form-control resultadoAnalito classInput" + inputType + "' data-idordenexamen='";
                contenido += lista[i]["IdOrdenExamen"] + "' data-idordenresultadoanalito='" + lista[i]["IdOrdenResultadoAnalito"] + "' data-optid='";
                contenido += lista[i]["IdOrdenResultadoAnalito"];
                console.log(lista[i]["IdOrdenResultadoAnalito"]);
                contenido += "' data-analitoid='";
                contenido += lista[i]["IdAnalito"];
                if (valueInput && valueInput.length > 0) {
                    contenido += "' value='";
                    contenido += valueInput[0].Value;
                }
                contenido += "' />";
                console.log(contenido);
                if (inputType == "number") {
                    triggernumber = true;
                }
            }
            if (tipo == 2) {
                triggerDatepicker = true;
                var dateValueInput = todosResultados.filter(v=> v.Id == lista[i]["IdOrdenResultadoAnalito"]);
                contenido += "<input type='text' class='form-control resultadoAnalito classInputdate' data-idordenexamen='";
                contenido += lista[i]["IdOrdenExamen"] + "' data-idordenresultadoanalito='" + lista[i]["IdOrdenResultadoAnalito"] + "' data-optid='";
                contenido += lista[i]["IdOrdenResultadoAnalito"];
                contenido += "' data-analitoid='";
                contenido += lista[i]["IdAnalito"];
                if (dateValueInput && dateValueInput.length > 0) {
                    contenido += "' value='";
                    contenido += dateValueInput[0].Value;
                }
                contenido += "' />";
                //console.log("IdAnalitoTipoFecha");
                //console.log(lista[i]["IdAnalito"]);
                //console.log(lista[i]["IdOrdenResultadoAnalito"]);
                console.log(contenido);
            }
            debugger;
            if (tipo == 3) {
                var checkboxItems = campos.filter(v=> v.IdAnalitoOpcionParent.toString().trim() == lista[i]["IdAnalito"].trim().toUpperCase() && v.IdOrdenExamen == lista[i]["IdOrdenExamen"]);
                fireCheckEvent = true;
                checkboxItems.forEach(function (item) {
                    if (todosResultados.filter(v=> v.Id == item["IdAnalitoOpcionResultado"]) && todosResultados.filter(v=> v.Id == item["IdAnalitoOpcionResultado"]).length > 0) {
                        fireCheckEvent = true;
                        objId = lista[i]["IdAnalito"].trim().toUpperCase();
                        objValue = item["IdAnalitoOpcionResultado"];
                        triggerObjs.push({ "objId": objId, "objValue": objValue, "idordenresultadoanalito": lista[i]["IdOrdenResultadoAnalito"] });
                    }

                    contenido += "<input type='checkbox' name='chkbx" + item.IdAnalitoOpcionResultado + "' style='padding-left:5px;' class='form-control classCheckbox " + lista[i]["IdOrdenResultadoAnalito"] + "' data-optid='";
                    contenido += item.IdAnalitoOpcionResultado;
                    contenido += "' data-analitoid='";
                    contenido += item.IdAnalito;
                    contenido += "' data-idordenexamen='";
                    contenido += camposIdordenexamen + "' data-idordenresultadoanalito='" + lista[i]["IdOrdenResultadoAnalito"];
                    contenido += "' id='" + lista[i]["IdAnalito"].trim().toUpperCase();
                    contenido += "'  />";
                    contenido += "<label>" + item.Glosa + "</label><br/>";
                });
            }
            if (tipo == 5) {
                var valueInput = todosResultados.filter(v=> v.Id == lista[i]["IdOrdenResultadoAnalito"]);
                contenido += "<textarea cols='10' rows='5' class='form-control resultadoAnalito classTextarea' data-idordenexamen='";
                contenido += lista[i]["IdOrdenExamen"] + "' data-idordenresultadoanalito='" + lista[i]["IdOrdenResultadoAnalito"] + "' data-optid='";
                contenido += lista[i]["IdOrdenResultadoAnalito"];
                if (valueInput && valueInput.length > 0) {
                    contenido += "' value='";
                    contenido += valueInput[0].Value;
                }
                contenido += "'></textarea>";
            }
            if (tipo == 6) {
                contenido += "<select class='divPadre form-control resultadoAnalito classSelect " + lista[i]["IdOrdenResultadoAnalito"] + "' data-padre='1' id='";
                contenido += lista[i]["IdAnalito"].trim().toUpperCase();
                contenido += "' data-analitoid='";
                var comboidanalito = '';
                var camposPorExamen = campos.filter(v=> v.IdOrdenExamen == lista[i]["IdOrdenExamen"]);
                camposPorExamen.forEach(function (item) {
                    if (item["IdAnalitoOpcionParent"].trim().toUpperCase() == lista[i]["IdAnalito"].trim().toUpperCase() && item.IdOrdenExamen == lista[i]["IdOrdenExamen"] && item.IdOrdenResultadoAnalito == lista[i]["IdOrdenResultadoAnalito"]) {
                        comboidanalito = item.IdAnalito;
                        return false;
                    }
                });
                contenido += comboidanalito;

                contenido += "' data-idordenexamen='";
                contenido += lista[i]["IdOrdenExamen"] + "' data-idordenresultadoanalito='" + lista[i]["IdOrdenResultadoAnalito"];
                contenido += "'>";
                contenido += "<option value=''>Seleccione</option>";
                if (camposPorExamen.filter(x=> x.IdAnalitoOpcionParent.trim().toUpperCase() == lista[i]["IdAnalito"].trim().toUpperCase()).length > 0) {
                    var firstelement = camposPorExamen.filter(x=> x.IdAnalitoOpcionParent.trim().toUpperCase() == lista[i]["IdAnalito"].trim().toUpperCase());
                    fireChangeEvent = true;
                    triggerObjs.push({ "objId": lista[i]["IdAnalito"].trim().toUpperCase(), "objValue": firstelement[0]["IdAnalitoOpcionResultado"], "idordenresultadoanalito": lista[i]["IdOrdenResultadoAnalito"] });
                    //contenido += "<option value='";
                    //contenido += firstelement[0]["IdAnalitoOpcionResultado"];
                    //contenido += "' "
                    ////contenido += selectedItemOpcion;
                    //contenido += " >"
                    //contenido += firstelement[0]["Glosa"];
                    //contenido += "</option>";
                }
                camposPorExamen.forEach(function (item) {
                    if (item["IdAnalitoOpcionParent"].trim().toUpperCase() == lista[i]["IdAnalito"].trim().toUpperCase() && item.IdOrdenExamen == lista[i]["IdOrdenExamen"] && item.IdOrdenResultadoAnalito == lista[i]["IdOrdenResultadoAnalito"]) {
                        var selectedItemOpcion = "";
                        if (todosResultados.filter(v=> v.Value == item["IdAnalitoOpcionResultado"]) && todosResultados.filter(v=> v.Value == item["IdAnalitoOpcionResultado"]).length > 0) {
                            fireChangeEvent = true;
                            objId = lista[i]["IdAnalito"].trim().toUpperCase();
                            objValue = item["IdAnalitoOpcionResultado"];
                            triggerObjs.push({ "objId": objId, "objValue": objValue, "idordenresultadoanalito": lista[i]["IdOrdenResultadoAnalito"] });
                        }

                        contenido += "<option value='";
                        contenido += item["IdAnalitoOpcionResultado"];
                        contenido += "' ";
                        contenido += selectedItemOpcion;

                        var firstelement = camposPorExamen.filter(x=> x.IdAnalitoOpcionParent.trim().toUpperCase() == lista[i]["IdAnalito"].trim().toUpperCase());
                        if (firstelement[0]["IdAnalitoOpcionResultado"] == item["IdAnalitoOpcionResultado"]) {
                            contenido += " selected>";
                        } else {
                            contenido += " >";
                        }
                        contenido += item["Glosa"];
                        contenido += "</option>";
                        camposPadre.push(item);

                    } else {
                        camposHijos.push(item)
                    }
                });

                contenido += "</select>";
            }

            contenido += "</br>";
            contenido += "<div id='idOpciones'>";
            contenido += "</div>";
            campos.forEach(function (item) {
                if (item["IdAnalitoOpcionParent"].trim().toUpperCase() == lista[i]["IdAnalito"].trim().toUpperCase()) {
                    contenido += "<div id='" + camposIdordenexamen + "hijosde" + item["IdAnalitoOpcionResultado"] + "'></div>";
                }
            });
            contenido += "</td>";

            contenido += "<td>";
            contenido += "<input type='text' name='observacion[]' class='form-control input observacion' value= '";
            contenido += lista[i]["Observacion"] == '' ? "" : lista[i]["Observacion"];
            contenido += "' data-idordenresultadoanalito='" + lista[i]["IdOrdenResultadoAnalito"] + "'></td>";
            contenido += "<td>";
            contenido += lista[i]["Unidad"];
            contenido += "</td>";
            contenido += "<td>";
            contenido += lista[i]["ValorReferencia"];
            contenido += "</td>";
            contenido += "</tr>";
            //}
        }
        contenido += "</tbody>";
        contenido += "</table>";
        contenido += "</br>"
    }

    document.getElementById("divDetalle").innerHTML = contenido;

    if (camposHijos.length > 0) {
        crearDetalles(camposPadre, camposHijos)
    }

    if (fireChangeEvent) {
        $.each(triggerObjs, function (key, item) {
            var selector = "#" + item.objId + ".classSelect." + item.idordenresultadoanalito;
            $(selector).val(item.objValue).change();
        });
    }

    if (fireCheckEvent) {
        $.each(triggerObjs, function (key, item) {
            var selector = "#" + item.objId + ".classCheckbox." + item.idordenresultadoanalito;
            console.log("firecheckEvent select:");
            console.log(selector);
            $(selector).val(item.objValue).click();
        });
    }

    var showdatepicker = false;
    $.each($("tr.rowResultados"), function (key, item) {
        var trIdOrdenExamen = item.dataset.idordenexamen;
        if (estatusObj.filter(v=> v.Id == trIdOrdenExamen) && estatusObj.filter(v=> v.Id == trIdOrdenExamen).length > 0) {
            var trToDisable = estatusObj.filter(v=> v.Id == trIdOrdenExamen && (v.status == 10 || v.status == 11));
            if (trToDisable && trToDisable.length > 0) {
                $(item).find('.classInputtext, .classInputnumber, .classInputdate, .classCheckbox, .classTextarea, .classSelect, .observacion').prop('disabled', "disabled");
            }
            else {
                showdatepicker = true;
            }
        }
    });

    if (showdatepicker) {
        if (triggerDatepicker) {
            $(".classInputdate").setDatePickerWithMaxValue();
        }
    }
}
//Autor : Juan Muga
//Fecha Creacion: 21/08/2018
//Descripcion: Carga y muestra todos los detalles por componente.
function crearDetalles(padres, hijos) {
    var canP = padres.length;
    var canH = hijos.length;
    var parents = [];
    var childs = [];

    for (var i = 0; i < canP; i++) {
        childs.push(padres[i].IdAnalitoOpcionResultado.toString().trim())
        for (var j = 0; j < canH; j++) {
            var parent = padres[i].IdAnalitoOpcionResultado.toString().trim();
            var hijo = hijos[j].IdAnalitoOpcionParent.trim();
            var tipo = hijos[j].Tipo;
            if (parent == hijo) {
                parents.push(hijos[j])
            }
        }
    }
}
//Autor : Juan Muga
//Fecha Creacion: 21/08/2018
//Descripcion: Carga y muestra todas las opciones configurados para un componente
function mostrarOpciones() {
    debugger;
    var controles = document.getElementsByName("resultado");
    var nControles = controles.length;
    var control = 0;
    for (var j = 0; j < nControles; j++) {
        control = controles[j];
        if (control.className == "form-control resultadoAnalito combo-padre") {
            control.onchange = function (selected) {
            }
        }
        if (control.className == "form-control resultadoAnalito combo-hijo") {
            control.onchange = function () {
                MostrarDetalleOpcionesHijos(control.value);
            }
        }
    }
}

function iBuscarClick() {
    document.getElementById("btnBuscar").click();
}

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

    var lengthToCompare;

    switch (controlId) {
        case "nroOficio":
        case "codigoOrden":
            lengthToCompare = 20;
            break;
        default:
            lengthToCompare = 11;
    }

    if (textbox.length === lengthToCompare || newTextValue.length > lengthToCompare)
        return false;

    return true;
}

function getDate() {
    var myDate = new Date();

    var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
    var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
    var anio = myDate.getFullYear();

    return [dia, mes, anio].join("/");
}

function getTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    return [h, m].join(":");
}

//SOTERO BUSTAMANTE Muestra el popup para seleccionar y agregar la enfermedad y examen
//$("#btnShowPopupEnfermedadExamen").on("click", function (e) {
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
        height: 200,
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

    $("#idExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
    $("#idTipoMuestra").chosen({ placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias" });


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
    $("#horaColeccionPopup").timeEntry({ show24Hours: true });
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

        var data = idOrden + '|' +
                    idLaboratorio + "|" +
                    idEnfermedad + "|" +
                    idExamen + "|" +
                    idTipoMuestra + "|" +
                    fechaColeccion + "|" +
                    horaColeccion + "|" +
                    idMaterial + "|" +
                                        //"fechaEnvio=" + fechaEnvio + "&" +//juan muga
                                        //juan muga
                    Number(volumenNoPrecisaPopup) + "|" +
                    volumenPopup + "|" +
                    cantidadPopup + "|" +
                    tipoMaterialPopup
        console.log(data);
        //Juan Muga
        //
        //"fechaEnvio=" + fechaEnvio + "&" +
        //"horaEnvio=" + horaEnvio  
        var datos = "?datos=" + data;

        $("#dialog-open").dialog("close");

        /*Agrega el examen*/
        $.ajax(
               {
                   url: URL_BASE + "Orden/AddExamenAnalista" + datos,
                   cache: false,
                   method: "GET"
               }).fail(function () {
                   jAlert("Se produjo un error de Conexión.", "Error")
               })


    });
}

//Abril04

$("body #dialog-edit, body #IngresarResultado").on("change", ".classSelect", function (test) {

    var IdOrdenExamen = $(this).closest('.rowResultados').data("idordenexamen");
    //var x = $(this).find("option[value=" + $(this).val() + "]").attr('selected', true);

    //if ($(this).val() == "") {
    var childelements = [];
    if ($(this).data("padre") && $(this).data("padre") == "1") {//padre
        childelements = $("[id*=" + IdOrdenExamen + "hijosde" + "]");
    }
    else {
        childelements = $(this).closest("#" + IdOrdenExamen + "hijosde" + $(this).attr("id")).find("[id*=" + IdOrdenExamen + "hijosde" + "]");
    }

    $.each(childelements, function (i, item) {
        item.innerHTML = "";
    });
    //}
    //else {
    $(this).find("option[value!=" + $(this).val() + "]").removeAttr('selected');
    $(this).find("option[value=" + $(this).val() + "]").attr('selected', true);
    var idOrdenResultadoAnalito = $(this).data("idordenresultadoanalito");
    var campoSeleccionado = campos.filter(v=> v.IdAnalitoOpcionResultado.toString().trim() == $(this).val().trim() && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito)[0];
    //console.log(campoSeleccionado);
    //debugger;
    //var campoSeleccionado = campos.filter(v=> v.IdAnalitoOpcionResultado.toString().trim() == $(this).val().trim() && v. == $(this).data("idordenexamen"))[0];
    var campoPadre = campos.filter(v=> v.IdAnalitoOpcionResultado.toString().trim() == campoSeleccionado.IdAnalitoOpcionResultado.toString().trim() && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito)[0];

    var todosloshijos = campos.filter(v=> v.IdAnalitoOpcionParent.toString().trim() == campoPadre.IdAnalitoOpcionParent.toString().trim() && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito);

    $.each(todosloshijos, function (i, item) {
        if (item.IdAnalitoOpcionResultado.toString().trim() != campoSeleccionado.IdAnalitoOpcionResultado.toString().trim()) {
            var divId = "hijosde" + item.IdAnalitoOpcionResultado.toString().trim();
            divId = divId.trim();
            if (document.getElementById(divId)) {
                document.getElementById(divId).innerHTML = "";
            }
        }
    });

    CargarHijos(IdOrdenExamen, campoSeleccionado.IdAnalitoOpcionParent.trim(), campoSeleccionado.IdAnalitoOpcionResultado.toString().trim(),
                campoSeleccionado.Tipo, campoPadre.Glosa, campoPadre.Tipo,
                idOrdenResultadoAnalito);
    //}
});

$("body #dialog-edit, body #IngresarResultado").on("click", ".classCheckbox", function () {
    var IdOrdenExamen = $(this).closest('.rowResultados').data("idordenexamen");
    var idOrdenResultadoAnalito = $(this).data("idordenresultadoanalito");
    var campoSeleccionado = campos.filter(v=> v.IdAnalitoOpcionResultado.toString().trim() == $(this).data('optid') && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito)[0];
    var campoPadre = campos.filter(v=> v.IdAnalitoOpcionResultado.toString().trim() == campoSeleccionado.IdAnalitoOpcionResultado.toString().trim() && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito)[0];
    debugger;
    if ($(this).is(':checked')) {
        $(this).attr("checked", "checked");

        CargarHijos(IdOrdenExamen, campoSeleccionado.IdAnalitoOpcionParent.trim(), campoSeleccionado.IdAnalitoOpcionResultado.toString().trim(), campoSeleccionado.Tipo, campoPadre.Glosa, campoPadre.Tipo, idOrdenResultadoAnalito);
    } else {
        var divid = IdOrdenExamen + "hijosde" + campoSeleccionado.IdAnalitoOpcionResultado.toString().trim();
        divid = divid.trim();
        document.getElementById(divid).innerHTML = "";
    }
});
function CallbackFunctionToFindTaskById(item) {

    return item.Id === '1137';
}
//Autor : Juan Muga
//Fecha Creacion: 21/08/2018
//Descripcion: Carga y muestra todas las opciones tipo hijo configurados
function CargarHijos(IdOrdenExamen, idPadre, id, tipo, glosaPadre, tipoPadre, idOrdenResultadoAnalito) {
    var seleccionadoHijos = campos.filter(v=> v.IdAnalitoOpcionParent.toString().trim() == id && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito);

    var nRegistros = lista.length;
    var dato = [];
    for (var i = 0; i < nRegistros; i++) {
        if (lista[i]["Resultado"]) {
            var iitem = JSON.parse(lista[i]["Resultado"]);
            iitem.forEach(function (auxitem) {
                if (auxitem.IdOrdenExamen.toLowerCase() == IdOrdenExamen.toLowerCase()) {
                    dato.push(auxitem);
                }
            });
        }
    }
    var nuevoDivId = "hijosde" + id;
    nuevoDivId = nuevoDivId.trim();
    var fireChangeEvent = false;
    var selectedValues = [];
    var selectedIds = [];
    var contenidoHtml = "";
    var triggerDatepicker = false;
    contenidoHtml += "<div ";
    contenidoHtml += "id='contenedorHijos";
    contenidoHtml += id;
    contenidoHtml += "'>";
    //alert("cargas hijos");
    if (seleccionadoHijos.length > 0) {
        //if (tipo != 6) {//Para que no aparezca la glosa cuando la opcion es combo seleccion
        //    contenidoHtml += "<h5>" + glosaPadre + "</h5><br/>";
        //}
        contenidoHtml += "<div style='padding-left:20px;'>";
        if (tipo == 1 || tipo == 4) {
            var inputType = tipo == 1 ? "text" : "number";
            seleccionadoHijos.forEach(function (item) {
                var filteredItem = dato.filter(v=> v.Id == item.IdAnalitoOpcionResultado && v.IdOrdenExamen.toLowerCase() == IdOrdenExamen.toLowerCase() && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito);
                if (filteredItem && filteredItem.length > 0) {
                    selectedValues.push({ "Id": filteredItem[0].Id, "Value": filteredItem[0].Value });
                }
                debugger;
                if (item.Glosa.trim().length > 0) {
                    contenidoHtml += "<label>" + item.Glosa + "</label><br/>";
                }
                //contenidoHtml += "<label>" + item.Glosa + "</label><br/>";
                contenidoHtml += "<input type='" + inputType + "' name='text" + item.IdAnalitoOpcionResultado + "' class='classInput" + inputType + "' data-optid='";
                contenidoHtml += item.IdAnalitoOpcionResultado;
                contenidoHtml += "' data-analitoid='";
                contenidoHtml += item.IdAnalito;
                contenidoHtml += "' data-idordenexamen='";
                contenidoHtml += IdOrdenExamen + "' data-idordenresultadoanalito='" + idOrdenResultadoAnalito;
                if (filteredItem && filteredItem.length > 0) {
                    contenidoHtml += "' value='";
                    contenidoHtml += filteredItem[0].Value;
                }
                contenidoHtml += "' /><br/>";
                contenidoHtml += "<div id='" + IdOrdenExamen + "hijosde" + item.IdAnalitoOpcionResultado + "'></div>";
            });
        }
        else if (tipo == 2) {
            triggerDatepicker = true;
            seleccionadoHijos.forEach(function (item) {
                var filteredItem = dato.filter(v=> v.Id == item.IdAnalitoOpcionResultado.toString());
                if (filteredItem && filteredItem.length > 0) {
                    selectedValues.push({ "Id": filteredItem[0].Id, "Value": filteredItem[0].Value });
                }
                contenidoHtml += "<label>" + item.Glosa + "</label><br/>";
                contenidoHtml += "<input type='input' name='textboxdate" + item.IdAnalitoOpcionResultado + "' class='classInputdate' data-optid='";
                contenidoHtml += item.IdAnalitoOpcionResultado;
                contenidoHtml += "' data-analitoid='";
                contenidoHtml += item.IdAnalito;
                contenidoHtml += "' data-idordenexamen='";
                contenidoHtml += IdOrdenExamen + "' data-idordenresultadoanalito='" + idOrdenResultadoAnalito;
                if (filteredItem && filteredItem.length > 0) {
                    contenidoHtml += "' value='";
                    contenidoHtml += filteredItem[0].Value;
                }
                contenidoHtml += "' /><br/>";
                contenidoHtml += "<div id='" + IdOrdenExamen + "hijosde" + item.IdAnalitoOpcionResultado + "'></div>";
            });
        }
        else if (tipo == 3) {
            debugger;
            fireChangeEvent = true;
            seleccionadoHijos.forEach(function (item) {
                //var x = item.IdAnalitoOpcionResultado;
                if (dato.filter(v=> v.Id == item.IdAnalitoOpcionResultado && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito) && dato.filter(v=> v.Id == item.IdAnalitoOpcionResultado && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito).length > 0) {
                    fireChangeEvent = true;
                    selectedValues.push(item.IdAnalitoOpcionResultado);
                    //selectedValues.push({ "objId": id, "objValue": item.IdAnalitoOpcionResultado, "idordenresultadoanalito": idOrdenResultadoAnalito });
                }

                contenidoHtml += "<input type='checkbox' name='chkbx" + item.IdAnalitoOpcionResultado + "' style='padding-left:5px;' class='classCheckbox " + idOrdenResultadoAnalito + "' data-optid='";
                contenidoHtml += item.IdAnalitoOpcionResultado;
                contenidoHtml += "' data-analitoid='";
                contenidoHtml += item.IdAnalito;
                contenidoHtml += "' data-idordenexamen='";
                contenidoHtml += IdOrdenExamen + "' data-idordenresultadoanalito='" + idOrdenResultadoAnalito;
                contenidoHtml += "'  />";
                if (item.Glosa.trim().length > 0) {
                    contenidoHtml += "<label>" + item.Glosa + "</label><br/>";
                }
               
                //agregar el div de los hijos debajo por cada padre y no todos a la vez al final
                contenidoHtml += "<div id='" + IdOrdenExamen + "hijosde" + item.IdAnalitoOpcionResultado + "'></div>";
            });

            //seleccionadoHijos.forEach(function (item) {
            //    //contenidoHtml += "<div id='" + IdOrdenExamen + "chckbxcontenedorHijos" + item.IdAnalitoOpcionResultado + "'></div>";
            //    contenidoHtml += "<div id='" + IdOrdenExamen + "hijosde" + item.IdAnalitoOpcionResultado + "'></div>";
            //});
        }
        else if (tipo == 5) {
            seleccionadoHijos.forEach(function (item) {
                var filteredItem = dato.filter(v=> v.Id == item.IdAnalitoOpcionResultado);
                if (filteredItem && filteredItem.length > 0) {
                    selectedValues.push({ "Id": filteredItem[0].Id, "Value": filteredItem[0].Value });
                }
                contenidoHtml += "<label>" + item.Glosa + "</label><br/>";
                contenidoHtml += "<textarea cols='10' rows='5' name='textbox" + item.IdAnalitoOpcionResultado + "' class='classTextarea' data-optid='";
                contenidoHtml += item.IdAnalitoOpcionResultado;
                contenidoHtml += "' data-analitoid='";
                contenidoHtml += item.IdAnalito;
                contenidoHtml += "' data-idordenexamen='";
                contenidoHtml += IdOrdenExamen + "' data-idordenresultadoanalito='" + idOrdenResultadoAnalito;
                if (filteredItem && filteredItem.length > 0) {
                    contenidoHtml += "' value='";
                    contenidoHtml += filteredItem[0].Value;
                }
                contenidoHtml += "' ></textarea><br/>";
                contenidoHtml += "<div id='" + IdOrdenExamen + "hijosde" + item.IdAnalitoOpcionResultado + "'></div>";
            });

        }
        else if (tipo == 6) {
            contenidoHtml += "<select name='resultado' class='form-control resultadoAnalito classSelect " + idOrdenResultadoAnalito + "' id='";
            contenidoHtml += id;
            contenidoHtml += "' data-analitoid='";
            var comboidanalito = '';
            if (seleccionadoHijos.length > 0) {
                comboidanalito = seleccionadoHijos[0].IdAnalito;
            }
            contenidoHtml += comboidanalito;
            contenidoHtml += "' data-idordenexamen='";
            contenidoHtml += IdOrdenExamen + "' data-idordenresultadoanalito='" + idOrdenResultadoAnalito;
            contenidoHtml += "'>";
            contenidoHtml += "<option value=''>Seleccione</option>";
            if (seleccionadoHijos.length > 0) {
                fireChangeEvent = true;
                selectedValues.push({ "objId": id, "objValue": seleccionadoHijos[0].IdAnalitoOpcionResultado, "idordenresultadoanalito": idOrdenResultadoAnalito });
                //contenidoHtml += "<option value='";
                //contenidoHtml += seleccionadoHijos[0].IdAnalitoOpcionResultado;
                //contenidoHtml += "' selected>";
                //contenidoHtml += seleccionadoHijos[0].Glosa;
                //contenidoHtml += "</option>";
                //} else {
            }
            seleccionadoHijos.forEach(function (item) {
                if (dato.filter(v=> v.Value == item.IdAnalitoOpcionResultado) && dato.filter(v=> v.Value == item.IdAnalitoOpcionResultado).length > 0) {
                    fireChangeEvent = true;
                    selectedIds.push(id);
                    selectedValues.push({ "objId": id, "objValue": item.IdAnalitoOpcionResultado, "idordenresultadoanalito": idOrdenResultadoAnalito });
                }
                contenidoHtml += "<option value='";
                contenidoHtml += item.IdAnalitoOpcionResultado;
                if (seleccionadoHijos[0].IdAnalitoOpcionResultado == item.IdAnalitoOpcionResultado) {
                    contenidoHtml += "' selected>";
                } else {
                    contenidoHtml += "'>";
                }

                contenidoHtml += item.Glosa;
                contenidoHtml += "</option>";
            });
            //}
            contenidoHtml += "</select>";
            contenidoHtml += "</br>";
            seleccionadoHijos.forEach(function (item) {
                contenidoHtml += "<div id='" + IdOrdenExamen + "hijosde" + item.IdAnalitoOpcionResultado + "'></div>";
            });
        }

        contenidoHtml += "</div>";
    } else {
        if ($("#" + idPadre).hasClass("divPadre")) {
            var childElementsPrincipal = campos.filter(v=> v.IdAnalitoOpcionParent.toString().trim() == idPadre && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito);

            $.each(childElementsPrincipal, function (i, item) {
                $("#" + IdOrdenExamen + "hijosde" + item.IdAnalitoOpcionResultado)[0].innerHTML = "";
            });
        } else {
            var childelements = $("#" + IdOrdenExamen + "hijosde" + id).closest("#" + IdOrdenExamen + "hijosde" + idPadre).find("[id*=" + IdOrdenExamen + "hijosde" + "]");
            //Juan Muga prueba checkbox quita las demas opciones seleccionadas
            //$.each(childelements, function (i, item) {
            //    item.innerHTML = "";
            //});
        }
    }

    contenidoHtml += "</div>";
    //Revisar para que solo cargue el ultimo seleccionados
    //seleccionadoHijos.forEach(function (item) {
    //    contenidoHtml += "<div id='" + IdOrdenExamen + "hijosde";
    //    contenidoHtml += item.IdAnalitoOpcionResultado;
    //    contenidoHtml += "'></div>";
    //});

    var selectHijos = IdOrdenExamen + "hijosde" + id;

    document.getElementById(selectHijos).innerHTML = contenidoHtml;

    if (triggerDatepicker) {
        $(".classInputdate").setDefaultDatePicker();
    }

    if (fireChangeEvent) {
        if (tipo == 1 || tipo == 4) {
            var classname = tipo == 1 ? "classInputtext" : "classInputnumber";
            $.each(document.getElementsByClassName(classname), function (i, item) {
                var itemFiltered = selectedValues.filter(v=> v.Id == item.dataset.optid);
                if (itemFiltered && itemFiltered.length > 0) {
                    $(this).val(itemFiltered[0].Value);
                }
            });
        }
        else if (tipo == 3) {
            var itemsParaClick = [];
            $.each(document.getElementsByClassName("classCheckbox " + idOrdenResultadoAnalito), function (i, item) {
                var itemFiltered = selectedValues.filter(v=> v == item.dataset.optid && item.dataset.idordenresultadoanalito == idOrdenResultadoAnalito && item.dataset.idordenexamen == IdOrdenExamen);
                if (itemFiltered && itemFiltered.length > 0) {
                    itemsParaClick.push(item);
                    //$(item).trigger("click");
                }
            });
            if (itemsParaClick.length > 0) {
                $.each(itemsParaClick, function (i, item) {
                    $(item).trigger("click"); 
                });
            }
        }
        else if (tipo == 6) {
            $.each(selectedValues, function (key, item) {
                var selector = "#" + item.objId + ".classSelect." + idOrdenResultadoAnalito;
                $(selector).val(item.objValue).change();
            });
        }
    }
}

function ProcesoMasivoLabCodigoMuestra(TipoProcesoMasivo) {
    if ($("#CodigoMuestra").val().length > 0 || $("#NroSolicitud").val().length > 0) {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "ResultadosAnalisis/ProcesoMasivoLabCodigoMuestra?TipoProcesoMasivo=" + TipoProcesoMasivo + "&codigoMuestra=" + $("#CodigoMuestra").val() + "&UsuarioRom=" + $("#UsuarioRom").val() + "&metodoKit=" + $("#ddMetodoInterfazMASED").val() + "&nroSolicitud=" + $("#NroSolicitud").val(),
            success: function (result) {
                debugger;
                var data = result;
                console.log(data);
                var rows = [];
                rows = data.split("|");
                for (var i = 0; i < rows.length; i++) {
                    var recepcion = [];
                    recepcion = data.split(",");
                    var fila = "";
                    fila += "<tr>";
                    fila += "<td>" + recepcion[0] + "</td>" + "<td>" + recepcion[1] + "</td>" + "<td>" + recepcion[2] + "</td>" + "<td>" + recepcion[3] + "</td>" + "<td>" + recepcion[4] + "</td>" + "<td>" + recepcion[5] + "</td>" + "<td>" + recepcion[6] + "</td>";
                    fila += "</tr>";
                    $(fila).appendTo($("#filasMuestra"));
                }
                
                $("#CodigoMuestra").val("");
                $("#CodigoMuestra").focus();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest);
            }
        });
    }
}
function RecepcionMasiva(TipoProcesoMasivo) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/RecepcionMasiva?TipoProcesoMasivo=" + TipoProcesoMasivo,
        success: function (result) {
            $('#DatoMuestra').html(result);

            $("#CodigoMuestra").keyup(function (event) {
                if (event.keyCode == 13) {
                    console.log('x');
                    $("#btnAddMuestra").click();
                    $("#btnValMuestra").click();
                    $("#btnRecValMuestra").click();
                }
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
function PrintRecepcionLab() {
   
    $('#DatoMuestra').printThis();
}
/*
    Descripcion:Obtiene Fecha y hora actual del sistema
    Autor: Marcos Mejia
    Fecha: 24/10/2019
*/
function getFechaActual(){
    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth()+1; //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var anio = fecha.getFullYear(); //obteniendo año
    if(dia<10)
        dia='0'+dia; //agrega cero si el menor de 10
    if(mes<10)
        mes = '0' + mes //agrega cero si el menor de 10
    var hora = fecha.getHours();
    var minuto = fecha.getMinutes();
    var segungo = fecha.getSeconds();
    var fechaActual = dia + "/" + mes + "/" + anio + " " + hora + ":" + minuto + ":" + segungo;
    return fechaActual;
}

/*
    Descripcion:Envía mensaje de para registro de fecha de siembra al Modal
    Autor: Marcos Mejia
    Fecha: 24/10/2019
*/
function IngresarFechaSiembra(idOrdenExamen,examen,codigo) {
    debugger;
    var cadena = "";
    cadena += "<center>¿Desea Registrar la fecha de siembra del cultivo para el Examen <b>" + examen + "</b> de código de Orden <b>" + codigo + "</b>?</center>";
    cadena += "<br /><center>Fecha y hora de Siembra: " + getFechaActual() + "</center>";
    cadena += "<br /><input type=hidden value=" + idOrdenExamen + " id=idOrdenExamen />";
    $('#RegFechaSiembra').html(cadena);
}

/*
    Descripcion:Envía mensaje de confirmacion de registro de fecha de siembra al Modal
    Autor: Marcos Mejia
    Fecha: 24/10/2019
*/
function MostrarFechaSiembra(examen, codigo,fecha) {
    debugger;
    var cadena = "";
    cadena += "<center>La fecha del registro de la siembra del cultivo del <b>" + examen + "</b> de código de Orden <b>" + codigo + "</b> es:</center>";
    cadena += "<br /><center>Fecha y hora de Siembra: " + fecha + "</center>";
    $('#RegFechaSiembra').html(cadena);
}

/*
    Descripción: Registra la fecha de siembra del cultivo
    Autor: Marcos Mejia
    Fecha: 24/10/2018
*/
$("#btnGrabar").click(function () {
    debugger;
    var idOrdenExamen = $('#idOrdenExamen').val();

    $.ajax(
               {
                   type: "GET",
                   cache: false,
                   url: URL_BASE + "ResultadosAnalisis/RegistrarFechaSiembra",
                   data: { idOrdenExamen: idOrdenExamen },
                   success: function (valor) {
                       if (valor == 1) {
                           jAlert("La fecha y hora de la siembra se registró exitósamente", "¡Alerta!", function () {
                               location.reload(true);
                           });
                       } else {
                           $('#RegistrarFechaSiembra').modal('hide');
                       }
                   },
                   error: function (XMLHttpRequest, textStatus, errorThrown) {
                       console.log(XMLHttpRequest);
                   }
               });
});

function IngresarCodigos() {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/AsignarMetodoProtocolo",
        success: function (result) {
            $('#DatoMuestra2').html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
$(".ficha").click(function (e) {
    e.preventDefault();
    var ficha = $(this).data('file');
    var clase = '.DescargarFicha' + ficha;
    debugger;
    $.ajax(
        {
            type: "GET",
            cache: false,
            url: URL_BASE + "ResultadosAnalisis/ValidaFicha",
            data: { ficha: ficha },
            success: function (x) {
                debugger;
                if (x == 'True') {
                    //$("#DescargarFicha").click();
                    var href = $(clase).attr('href');
                    console.log(href);
                    window.location.href = href;
                } else {
                    jAlert("La ficha no se encuentra escaneada.", "¡Alerta!");
                    return false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest);
            }
        });
});
