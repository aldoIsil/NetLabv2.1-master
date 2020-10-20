var IdOrden = $("#idOrden").val();
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

function mostrarConfirmacion() {
}

$("body #dialog-edit, body #IngresarResultados").on("click", ".irIngresarResultados", function (e) {
    e.preventDefault();
    var idOrden = $(this).attr('idorden');
    var idArea = $(this).attr('idarea');
    var idExamen = $(this).attr('idexamen');

    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/IngresarResultados",
        data: { id: idOrden, idArea: idArea, idExamen: idExamen },
        success: function (result) {
            htmlToCharge = result;
            closeToChargeResultados = true;
            $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
        }
    });

    return false;
});
function IngresarResultadoClick(idOrden, idOrdenExamen, idArea) {
    debugger;
    $('#SeleccionarAP').modal('hide');
    $.ajax({
        type: "GET",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/IngresarResultados",
        data: { id: idOrden, idArea: idArea, idOrdenExamen: idOrdenExamen },
        success: function (result) {
            $('#DataIngrersarResultado').html(result);
            $("#lnkRegistrarResultados").show();
            $("#lnkFinalizarResultados").hide();
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
    debugger;
    document.getElementById("divDetalle").innerHTML = "";
    var selectedElements = [];

    var idPlataforma = $('#ListaPlataforma').val();
    if (idPlataforma == -1) {
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

            if ($("name='metodo[]'").attr("disabled") == false) {
                $("#lnkRegistrarResultados").show();
            }

        }
    });
}
//***************************************************PASO 1
function getBool(val){ 
    var num = +val;
    return !isNaN(num) ? !!num : !!String(val).toLowerCase().replace(!!0,'');
}
$(document).ready(function () {
    debugger;
    var esPruebaRapida = $("#esPruebaRapida").val();
    if (!isNaN(esPruebaRapida) || esPruebaRapida != undefined) {
        if (getBool(esPruebaRapida)) {
            jConfirm('Existen exámenes seleccionados como Prueba Rápida, ¿Desea Ingresar Resultados ahora?',
                'Registro Prueba Rápida', function (r) {
                    debugger;
                    if (r) {
                        //$('#ListaUsuarios').modal('show');
                        //$.ajax({
                        //    url: URL_BASE + "Orden/UsuariosResponsablePruebaRapida?idOrden=" + IdOrden,
                        //    type: "POST",
                        //    dataType: "json",
                        //    contentType: "application/json; charset=utf-8",
                        //    success: function (response) {
                        //        var len = response.length;
                        //        $("#selUsuarios").empty();
                        //        for (var i = 0; i < len; i++) {
                        //            var id = response[i]['idUsuario'];
                        //            var name = response[i]['nombres'];
                        //            $("#selUsuarios").append("<option value='" + id + "'>" + name + "</option>");
                        //        }
                        //    },
                        //});
                        $.ajax({
                            url: URL_BASE + "Orden/RecepcionEESSandRecepcionLab",
                            cache: false,
                            method: "POST",
                            data: { idOrden: IdOrden },
                            success: function (response) {
                                var idOrdenExamen = response;
                                if (idOrdenExamen != null || idOrdenExamen != undefined) {
                                    $('#SeleccionarAP').modal('show');
                                    SeleccionarAPClick(idOrdenExamen, 1);
                                }

                            },
                        });
                    };
                });
        }
    }

    //$("#btnElegir").click(function () {
    //    debugger;
    //    var _idusuario = $("#selUsuarios").val();
    //    if (_idusuario != null) {
    //        $.ajax({
    //            url: URL_BASE + "Orden/RecepcionEESSandRecepcionLab",
    //            cache: false,
    //            method: "POST",
    //            data: { idOrden: IdOrden, idUsuario: _idusuario },
    //            success: function (response) {
    //                var idOrdenExamen = response;
    //                if (idOrdenExamen != null || idOrdenExamen != undefined) {
    //                    $('#SeleccionarAP').modal('show');
    //                    SeleccionarAPClick(idOrdenExamen, _idusuario);
    //                }

    //            },
    //        });
    //    }
    //});

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
                    if (valorRes.length === 0) {
                        ok = false;
                    }
                }
            });
            if (ok) {
                $("#formRegistroResultados").submit();
                debugger;
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
                            jAlert("Los resultados se han registrado y finalizado satisfactoriamante.", "Aviso", function () {
                                $.ajax({
                                    type: "POST",
                                    cache: false,
                                    url: URL_BASE + "ValidacionResultados/UpdatePruebaRapida",
                                    data: JSON.stringify(selectedElements),
                                    contentType: "application/json; charset=utf-8",
                                    success: function (resultLab) {
                                        //Impresión de resultados. 
                                        if (selectedElements.length > 0) {
                                            //$("#CerrarPopUpIngresoResultados").click();
                                            $(window).attr('location', URL_BASE + "ReporteResultados/ImprimirResultadosPruebaRapida?idOrden=" + IdOrden + "&idLaboratorio=" + resultLab +
                                            "&idexamen=" + selectedElements);
                                            $('#IngresarResultado').modal('hide');
                                        }
                                    },

                                    failure: function () {
                                        ok = false;
                                        return false;
                                    }
                                });
                            });
                        }
                    });
                }
            } else {
                jAlert("Por favor, llene todos los resultados.", "Alerta!");
            }
        } else {
            jAlert("Por favor, seleccione al menos un examen.", "Alerta!");
        }


    });

    $("body").on("submit", "#formRegistroResultados", function (e) {
        e.preventDefault();
        debugger;
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function () {
                if (!BoolFinalizar) {
                    jAlert("Los resultados se registraron satisfactoriamente.", "Registro Correcto", function () {
                        //$('#IngresarResultado').modal('show');                                   
                    });
                }
            }
        });
    });
    //$("body").on("submit", "#formRegistroResultados", function (e) {
    //    e.preventDefault();

    //    $.ajax({
    //        type: "POST",
    //        cache: false,
    //        url: $(this).attr("action"),
    //        data: $(this).serialize(),
    //        success: function () {
    //            //Finalizar Registro
    //            var selectedElements = [];

    //            $.each($("input[name='resultados']:checked"), function () {
    //                selectedElements.push($(this).val());
    //            });

    //            if (selectedElements.length > 0) {
    //                $.ajax({
    //                    type: "POST",
    //                    cache: false,
    //                    url: URL_BASE + "ResultadosAnalisis/Finalizar",
    //                    data: JSON.stringify(selectedElements),
    //                    contentType: "application/json; charset=utf-8",
    //                    success: function () {
    //                        actualizarCabecera();
    //                        verDetalle();

                            //jAlert("Los resultados se han registrado y finalizado satisfactoriamante.", "Aviso", function () {
                            //    $.ajax({
                            //        type: "POST",
                            //        cache: false,
                            //        url: URL_BASE + "ValidacionResultados/UpdatePruebaRapida",
                            //        data: JSON.stringify(selectedElements),
                            //        contentType: "application/json; charset=utf-8",
                            //        success: function (resultLab) {
                            //            //Impresión de resultados. 
                            //            if (selectedElements.length > 0) {
                            //                //$("#CerrarPopUpIngresoResultados").click();
                            //                //$(window).attr('location', URL_BASE + "ReporteResultados/ImprimirResultadosPruebaRapida?idOrden=" + IdOrden + "&idLaboratorio=" + resultLab +
                            //                //"&idexamen=" + selectedElements);
                            //                $('#IngresarResultado').modal('show');
                            //            }
                            //        },
                                    
                            //        failure: function () {
                            //            ok = false;
                            //            return false;
                            //        }
                            //    });
                            //});
    //                    }
    //                });
    //            };
    //        }
    //    });
    //});
    //$("#dialog-edit").on("click", "#CerrarPopUp", function () {
    //    $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    //});

    //$("#dialog-edit").on("click", "#CerrarPopUpIngresoResultados", function () {
    //    $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    //});   
});
//VerDetalle Resultados Seleccionados
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
        contenido += "<th class='rowExamen2'>";
        contenido += "Examen</th>";
        contenido += "<th class='rowExamen2'>Componente</th>";
        contenido += "<th class='rowExamen2'>Resultado</th>";
        contenido += "<th class='rowExamen2'>Observaciones</th>";
        contenido += "<th class='rowExamen2'>Unidad</th>";
        contenido += "<th class='rowExamen2'>Valor Normal / Referencia</th>";
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

function SeleccionarAPClick(idOrdenExamen,pr) {
    $.ajax({
        type: "GET",
        cache: false,
        url: URL_BASE + "ResultadosAnalisis/SeleccionarAP",
        data: { idOrdenExamen: idOrdenExamen, pr: pr },
        success: function (result) {
            $('#DataSeleccionarAP').html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
//
