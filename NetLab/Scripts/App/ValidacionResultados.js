var myDate = new Date();

var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
var anio = myDate.getFullYear();

var closeEditToCharge = false;
var idOrdenSel = "";


var closeToChargeResultados = false;
var htmlToCharge = "";
var activeEdit;

//var idOrdenSel = "";

var lista = [];
var campos = [];
var camposPadre = [];
var camposHijos = [];
var idCombo;
var auxHtmlContent = '';
var camposIdordenexamen = '';
var camposIdOrdenResultadoAnalito = '';
var todosResultados = [];
//
function verDetalleOpcion(x) {
    var idOpcion = x.trim();
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
    var contenido = "";
    var fireChangeEvent = false;
    var fireCheckEvent = false;
    var objId = '';
    var objValue = '';
    var triggerObjs = [];
    var triggerDatepicker = false;
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
                    var itemCampos = campos.filter(v => v.IdAnalitoOpcionResultado == value.IdAnalitoOpcionResultado && v.IdOrdenExamen == value.IdOrdenExamen && v.IdOrdenResultadoAnalito == value.IdOrdenResultadoAnalito);
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
                var valueInput = todosResultados.filter(v => v.Id == lista[i]["IdOrdenResultadoAnalito"]);
                contenido += "<input type='text'  class='form-control resultadoAnalito classInput" + inputType + "' data-idordenexamen='";
                contenido += lista[i]["IdOrdenExamen"] + "' data-idordenresultadoanalito='" + lista[i]["IdOrdenResultadoAnalito"] + " data-optid='";
                contenido += lista[i]["IdOrdenResultadoAnalito"];
                if (valueInput && valueInput.length > 0) {
                    contenido += "' value='";
                    contenido += valueInput[0].Value;
                }
                contenido += "' />";
            }
            if (tipo == 2) {
                triggerDatepicker = true;
                var dateValueInput = todosResultados.filter(v => v.Id == lista[i]["IdOrdenResultadoAnalito"]);
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
            }
            if (tipo == 3) {
                var checkboxItems = campos.filter(v => v.IdAnalitoOpcionParent.toString().trim() == lista[i]["IdAnalito"].trim().toUpperCase() && v.IdOrdenExamen == lista[i]["IdOrdenExamen"]);

                checkboxItems.forEach(function (item) {
                    if (todosResultados.filter(v => v.Id == item["IdAnalitoOpcionResultado"]) && todosResultados.filter(v => v.Id == item["IdAnalitoOpcionResultado"]).length > 0) {
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
                var valueInput = todosResultados.filter(v => v.Id == lista[i]["IdOrdenResultadoAnalito"]);
                contenido += "<textarea cols='10' rows='5' class='form-control resultadoAnalito classTextarea' data-idordenexamen='";
                contenido += lista[i]["IdOrdenExamen"] + "' data-idordenresultadoanalito='" + lista[i]["IdOrdenResultadoAnalito"] + " data-optid='";
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
                var camposPorExamen = campos.filter(v => v.IdOrdenExamen == lista[i]["IdOrdenExamen"]);
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

                camposPorExamen.forEach(function (item) {
                    if (item["IdAnalitoOpcionParent"].trim().toUpperCase() == lista[i]["IdAnalito"].trim().toUpperCase() && item.IdOrdenExamen == lista[i]["IdOrdenExamen"] && item.IdOrdenResultadoAnalito == lista[i]["IdOrdenResultadoAnalito"]) {
                        var selectedItemOpcion = "";
                        if (todosResultados.filter(v => v.Value == item["IdAnalitoOpcionResultado"]) && todosResultados.filter(v => v.Value == item["IdAnalitoOpcionResultado"]).length > 0) {
                            fireChangeEvent = true;
                            objId = lista[i]["IdAnalito"].trim().toUpperCase();
                            objValue = item["IdAnalitoOpcionResultado"];
                            triggerObjs.push({ "objId": objId, "objValue": objValue, "idordenresultadoanalito": lista[i]["IdOrdenResultadoAnalito"] });
                        }

                        contenido += "<option value='";
                        contenido += item["IdAnalitoOpcionResultado"];
                        contenido += "' "
                        contenido += selectedItemOpcion;
                        contenido += " >"
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
        contenido += "</br>";
    }

    document.getElementById("divDetalle").innerHTML = contenido;

    if (camposHijos.length > 0) {
        crearDetalles(camposPadre, camposHijos);
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
            $(selector).val(item.objValue).click();
        });
    }

    var showdatepicker = false;
    $.each($("tr.rowResultados"), function (key, item) {
        var trIdOrdenExamen = item.dataset.idordenexamen;
        if (estatusObj.filter(v => v.Id == trIdOrdenExamen) && estatusObj.filter(v => v.Id == trIdOrdenExamen).length > 0) {
            var trToDisable = estatusObj.filter(v => v.Id == trIdOrdenExamen && (v.status == 10 || v.status == 11));
            if (trToDisable && trToDisable.length > 0) {
                //$(item).find('.classInputtext, .classInputnumber, .classInputdate, .classCheckbox, .classTextarea, .classSelect, .observacion').prop('disabled', "disabled");
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
                parents.push(hijos[j]);
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

$("#dialog-edit").on("change", ".classSelect", function (test) {
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
    var campoSeleccionado = campos.filter(v => v.IdAnalitoOpcionResultado.toString().trim() == $(this).val().trim() && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito)[0];
    //console.log(campoSeleccionado);
    //debugger;
    //var campoSeleccionado = campos.filter(v=> v.IdAnalitoOpcionResultado.toString().trim() == $(this).val().trim() && v. == $(this).data("idordenexamen"))[0];
    var campoPadre = campos.filter(v => v.IdAnalitoOpcionResultado.toString().trim() == campoSeleccionado.IdAnalitoOpcionResultado.toString().trim() && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito)[0];

    var todosloshijos = campos.filter(v => v.IdAnalitoOpcionParent.toString().trim() == campoPadre.IdAnalitoOpcionParent.toString().trim() && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito);

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

$("#dialog-edit").on("click", ".classCheckbox", function () {
    var IdOrdenExamen = $(this).closest('.rowResultados').data("idordenexamen");
    var idOrdenResultadoAnalito = $(this).data("idordenresultadoanalito");
    var campoSeleccionado = campos.filter(v => v.IdAnalitoOpcionResultado.toString().trim() == $(this).data('optid') && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito)[0];
    var campoPadre = campos.filter(v => v.IdAnalitoOpcionResultado.toString().trim() == campoSeleccionado.IdAnalitoOpcionResultado.toString().trim() && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito)[0];

    if ($(this).is(':checked')) {
        $(this).attr("checked", "checked");

        CargarHijos(IdOrdenExamen, campoSeleccionado.IdAnalitoOpcionParent.trim(), campoSeleccionado.IdAnalitoOpcionResultado.toString().trim(), campoSeleccionado.Tipo, campoPadre.Glosa, campoPadre.Tipo, idOrdenResultadoAnalito);
    } else {
        var divid = IdOrdenExamen + "hijosde" + campoSeleccionado.IdAnalitoOpcionResultado.toString().trim();
        divid = divid.trim();
        document.getElementById(divid).innerHTML = "";
    }
});


//Autor : Juan Muga
//Fecha Creacion: 21/08/2018
//Descripcion: Carga y muestra todas las opciones tipo hijo configurados
function CargarHijos(IdOrdenExamen, idPadre, id, tipo, glosaPadre, tipoPadre, idOrdenResultadoAnalito) {
    var seleccionadoHijos = campos.filter(v => v.IdAnalitoOpcionParent.toString().trim() == id && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito);

    var nRegistros = lista.length;
    var dato = [];
    for (var i = 0; i < nRegistros; i++) {
        if (lista[i]["Resultado"]) {
            var iitem = JSON.parse(lista[i]["Resultado"]);
            iitem.forEach(function (auxitem) {
                if (auxitem.IdOrdenExamen == IdOrdenExamen) {
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
                var filteredItem = dato.filter(v => v.Id == item.IdAnalitoOpcionResultado && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito);
                if (filteredItem && filteredItem.length > 0) {
                    selectedValues.push({ "Id": filteredItem[0].Id, "Value": filteredItem[0].Value });
                }

                contenidoHtml += "<label>" + item.Glosa + "</label><br/>";
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
                var filteredItem = dato.filter(v => v.Id == item.IdAnalitoOpcionResultado.toString());
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
                if (dato.filter(v => v.Id == item.IdAnalitoOpcionResultado && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito) && dato.filter(v => v.Id == item.IdAnalitoOpcionResultado && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito).length > 0) {
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
                contenidoHtml += "<label>" + item.Glosa + "</label><br/>";
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
                var filteredItem = dato.filter(v => v.Id == item.IdAnalitoOpcionResultado);
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
                if (dato.filter(v => v.Value == item.IdAnalitoOpcionResultado) && dato.filter(v => v.Value == item.IdAnalitoOpcionResultado).length > 0) {
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
            var childElementsPrincipal = campos.filter(v => v.IdAnalitoOpcionParent.toString().trim() == idPadre && v.IdOrdenExamen == IdOrdenExamen && v.IdOrdenResultadoAnalito == idOrdenResultadoAnalito);

            $.each(childElementsPrincipal, function (i, item) {
                $("#" + IdOrdenExamen + "hijosde" + item.IdAnalitoOpcionResultado)[0].innerHTML = "";
            });
        } else {
            var childelements = $("#" + IdOrdenExamen + "hijosde" + id).closest("#" + IdOrdenExamen + "hijosde" + idPadre).find("[id*=" + IdOrdenExamen + "hijosde" + "]");

            $.each(childelements, function (i, item) {
                item.innerHTML = "";
            });
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
                var itemFiltered = selectedValues.filter(v => v.Id == item.dataset.optid);
                if (itemFiltered && itemFiltered.length > 0) {
                    $(this).val(itemFiltered[0].Value);
                }
            });
        }
        else if (tipo == 3) {
            var itemsParaClick = [];
            $.each(document.getElementsByClassName("classCheckbox " + idOrdenResultadoAnalito), function (i, item) {
                var itemFiltered = selectedValues.filter(v => v == item.dataset.optid && item.dataset.idordenresultadoanalito == idOrdenResultadoAnalito && item.dataset.idordenexamen == IdOrdenExamen);
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
function verDetalle(showOptions) {

    var selectedElements = [];

    $.each($("input[name='resultados']:checked"), function () {
        selectedElements.push($(this).val());
    });

    if (selectedElements.length > 0) {
        $("#lnkImprimir").removeAttr("hidden");
    }
    else {
        $("#lnkImprimir").attr("hidden", "hidden");
    }

    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ValidacionResultados/ListarDetalle",
        data: JSON.stringify(selectedElements),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#divDetalle").html(data);
            //alert($(this).val());
            if (showOptions) {
                $("#divDetalleUnico").removeClass("hidden");

                $("input[name=radiogroup]:radio").click(function () {

                    if ($(this).val() === "1") {
                        //$("#txtComentario").attr("hidden", "hidden");
                    }
                    else if ($(this).val() === "2") {
                        //$("#txtComentario").removeAttr("hidden");
                    }
                });
            }
        }
    });
}

$("#dialog-edit").on("click", "#lnkImprimir", function () {
    debugger;
    var tipoDocumento = $('#tipoDocumento').val();
    if (tipoDocumento == 4 || tipoDocumento == 7) {
        jAlert('El paciente presenta un tipo de Documento no válido para poder imprimir el resultado.', '¡Alerta!');
        return false;
    } else {
        $("#frmImprimirResultado").submit();
    }

});
//function PrintJSPDF(idOrdenPrint, idLaboratorioPrint) {
//    var selectedElements = [];

//    $.each($("input[name='resultados']:checked"), function () {
//        selectedElements.push($(this).val());
//    });
//    $.ajax({
//        type: "POST",
//        cache: false,
//        url: URL_BASE + "ReporteResultados/Print",
//        data: JSON.stringify({ idOrden: idOrdenPrint, idLaboratorio: idLaboratorioPrint, resultados: selectedElements }),
//        contentType: "application/json; charset=utf-8",
//        success: function (data) {
//            debugger;
//            var listaPrint = JSON.parse(data);

//            var objectOrden = listaPrint.Orden;
//            var objectLaboratorio = listaPrint.Laboratorio;
//            var objectMuestras = listaPrint.Muestras;
//            var objectExamenes = listaPrint.Examenes;

//            var m = objectMuestras.length;
//            var ex = objectExamenes.length;
//            if (m > 0) {

//            }
//            for (var i = 0; i < m; i++) {
//                console.log(objectMuestras[i]);
//            }
//            for (var x = 0; x < ex; x++) {
//                console.log(objectExamenes[x]);
//            }
//            var doc = new jsPDF();
//            var totalPagesExp = "{total_pages_count_string}";
//            //Cabecera
//            var pageContent = function (objectExamenes) {
//                // HEADER
//                debugger;
//                doc.setFontSize(20);
//                doc.setTextColor(40);
//                doc.setFontStyle('normal');
//                if (objectLaboratorio.LogoRegional) {
//                    doc.addImage(objectLaboratorio.LogoRegional, 'PNG', data.settings.margin.left, 15, 10, 10);
//                }
//                doc.text("Report Prueba", data.settings.margin.left + 15, 22);

//                // FOOTER
//                var str = "Page " + data.pageCount;
//                // Total page number plugin only available in jspdf v1.0+
//                if (typeof doc.putTotalPages === 'function') {
//                    str = str + " of " + totalPagesExp;
//                }
//                doc.setFontSize(10);
//                doc.text(str, data.settings.margin.left, doc.internal.pageSize.height - 10);
//            };
//            //doc.autoTable(getInfoCol(), objectExamenes, {
//            //    startY: 230,
//            //    theme: 'grid',
//            //    addPageContent: pageContent,
//            //    margin: { top: 30 }
//            //});

//            // Total page number plugin only available in jspdf v1.0+
//            if (typeof doc.putTotalPages === 'function') {
//                doc.putTotalPages(totalPagesExp);
//            }
//            doc.save("prueba.pdf");
//            //Cuerpo



//            //Pie Pagina
//        }

//    });
//}
//function getInfoCol() {
//    return [{ title: "Codigo", dataKey: "Codigo" },
//            { title: "Diagnóstico Laboratorio LRR", dataKey: "Descripcion" },
//            { title: "Control de Calidad \nLaboratorio Referencia de Enteroparásitos - (INS)", dataKey: "DescripcionINS" }];
//}

function Imprimir(idOrden, idLaboratorio) {
    var selectedElements = [];

    $.each($("input[name='resultados']:checked"), function () {
        selectedElements.push($(this).val());
    });

    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ReporteResultados",
        data: JSON.stringify({ idOrden: idOrden, idLaboratorio: idLaboratorio, idOrdenExamen: selectedElements }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
        }
    });
}

$(document).ready(function () {

    $.ajaxSetup({ cache: false });

    // This will make every element with the class "date-picker" into a DatePicker element
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


    $(".editDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        //idOrdenSel = $(this).attr('idOrden');
        $("#dialog-edit").dialog({
            title: "Validar Ordenes",
            autoOpen: false,
            resizable: true,
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
            },
            close: function () {
                closeEditForm();
            }
        });


        $("#dialog-edit").dialog("open");
        return false;
    });

    function closeEditForm() {
        if (closeEditToCharge) {
            closeEditToCharge = false;
            $("#dialog-edit").dialog({
                title: "Resultados Validados",
                autoOpen: false,
                resizable: false,
                height: 550,
                width: 1050,
                show: { effect: "drop", direction: "up" },
                modal: true,
                draggable: true,
                open: function () {
                    $(this).html(htmlToCharge);
                },
                close: function () {
                    iBuscarClick();
                }
            });
            $("#dialog-edit").dialog("open");
        }
        //else {
        //    iBuscarClick();
        //}

    };


    $("#dialog-edit").on("click", "#CerrarPopUp", function (e) {
        iBuscarClick();
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

    //$("#idEnfermedadFiltro").chosen({ placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" });
    //$("#idExamenFiltro").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });

    //$("#idEnfermedadFiltro").ajaxChosen({
    //    dataType: "json",
    //    type: "POST",
    //    minTermLength: 3,
    //    afterTypeDelay: 300,
    //    cache: false,
    //    url: URL_BASE + "Orden/GetEnfermedades"
    //}, {
    //    loadingImg: URL_BASE + "Content/images/loading.gif"
    //}, { placeholder_text_single: "Seleccionar la Enfermedad", no_results_text: "No existen coincidencias" }).change(function () {
    //    $("#idExamenFiltro").ajaxChosen({
    //        dataType: "json",
    //        type: "POST",
    //        minTermLength: 3,
    //        afterTypeDelay: 100,
    //        cache: false,
    //        url: URL_BASE + "Orden/GetExamenes?genero=3&idenfermedad=" + $("#idEnfermedadFiltro").val()
    //    }, {
    //        loadingImg: URL_BASE + "Content/images/loading.gif"
    //    }, { placeholder_text_single: "Seleccionar el Examen", no_results_text: "No existen coincidencias" });
    //});
    confirmDeleteHandler();
});

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

//nuevas funciones ingreso resultdos verificador 
//autor: SOTERO BUSTAMANTE
//INGRESADO SOTERO BUSTAMANTE 21/10/2017

$("#dialog-edit").on("click", ".irIngresarResultados", function (e) {
    e.preventDefault();
    var idOrden = $(this).attr('idOrden');
    var idArea = $(this).attr('idArea');
    var idOrdenExamen = $(this).attr('idOrdenExamen');
    $.ajax({
        type: "POST",
        cache: false,
        url: $(this).attr('href'),
        data: { id: idOrden, idOrdenExamen: idOrdenExamen, idArea: idArea },
        success: function (result) {
            htmlToCharge = result;
            closeToChargeResultados = true;
            $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
        }
    });

    return false;
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

function closeAPForm() {

    if (closeToChargeResultados) {
        closeToChargeResultados = false;
        $("#dialog-edit").dialog({
            title: "Registrar Resultados Verificador",
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
            },
            close: function () {
                $(this).dialog("close");
            }
        });
        $("#dialog-edit").dialog("open");
    }
};

function verDetalleVerificador() {
    debugger;
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
            //$("#divDetalle").html(data);
            crearVista(data);
            mostrarOpciones();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
$("#dialog-edit").on("click", "#CerrarPopUpIngresoResultados", function () {
    $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
});

$("#dialog-edit").on("click", "#lnkRegistrarResultados", function (e) {
    e.preventDefault();

    var jsonvalues = [];
    var selectItemValues = [];
    var observaciones = [];
    $.each(document.getElementsByClassName("classSelect"), function (i, item) {
        jsonvalues.push({ "Tipo": "combo", "Id": item.id, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
    });

    $.each(document.getElementsByClassName("classCheckbox"), function (i, item) {
        if (item.checked) {
            jsonvalues.push({ "Tipo": "checkbox", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
        }
    });

    $.each(document.getElementsByClassName("observacion"), function (i, item) {
        observaciones.push({ "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito, "ObservacionContent": item.value });
    });

    $.each(document.getElementsByClassName("classInputtext"), function (i, item) {
        jsonvalues.push({ "Tipo": "inputtext", "Id": item.dataset.optid, "Value": item.value, "AnalitoId": item.dataset.analitoid, "IdOrdenExamen": item.dataset.idordenexamen, "IdOrdenResultadoAnalito": item.dataset.idordenresultadoanalito });
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
            $("#formRegistroResultadosVerificador").submit();
        } else {
            jAlert("Por favor, llene todos los resultados.", "Alerta!");
        }
    } else {
        jAlert("Por favor, seleccione al menos un examen.", "Alerta!");
    }


});

$("body").on("submit", "#formRegistroResultadosVerificador", function (e) {
    e.preventDefault();
    $.ajax({
        type: "POST",
        cache: false,
        url: $(this).attr("action"),
        data: $(this).serialize(),
        success: function () {
            jAlert("Los resultados se registraron satisfactoriamente.", "Registro Correcto", function () {
                iBuscarClick();
            });
        }
    });
});

function SolicitudPendiente() {

    jAlert("La Solicitud aun esta Pendiente de su autorización", "Solicitud de Liberacion")
    return false;

}
function ConfirmaSolicitud() {
    debugger;
    jConfirm("Desea enviar una Solicitud para Liberar Resultados", "Solicita Liberacion", function () {
        window.location.reload();
    })

}
function confirmDeleteHandler() {

    $(document).on("click", ".confirmDialog", function () {
        debugger;
        var url = $(this).attr("href");
        var id = $('#id').val();
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
//SOTERO BUSTAMANTE
function NuevoExamenMuestra() {
    jConfirm("Desea ingresar Nuevo examen para esta muestra", "Nuevo Examen", function (r) {

        if (r) {

            $.ajax({
                url: URL_BASE + "Orden/ShowPopupNuevoEnfermedadExamen?tipo=" + 1,
                cache: false,
                method: "POST"
            });

        }

    });



}
//SOTERO BUSTAMANTE Muestra el popup para seleccionar y agregar la enfermedad y examen
//$("#btnShowPopupEnfermedadExamen").on("click", function (e) {
$(".btnShowPopupEnfermedadExamen").on("click", function (e) {
    e.preventDefault();
    //var idOrden = e.target.nameProp;
    var url = $(this).attr("href") + "?_=" + (new Date()).getTime();
    var x = this.id;
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
/*VALIDACION MASIVA*/

//$('#chkAll').click(function (e) {
//    var table = $(e.target).closest('table');
//    $('td input:checkbox', table).prop('checked', this.checked);

//});

//$('#chkSelect').click(function (e) {
//    $("#btnRecepcionMasiva").show();
//});

$("#btnValidacionMasiva").click(function (e) {
    e.preventDefault();
    debugger;

    var data = '';
    var comentarioList = [];
    $('input[type=radio]').each(function () {
        var txtcomentario = "txtComentario " + $(this).val();
        if (this.id == "rbConformeMasivo" && this.checked) {
            data = $(this).val();
            $.each(document.getElementsByClassName("comentario"), function (i, item) {
                if (txtcomentario == this.id) {
                    comentarioList.push({ "idOrdenExamen": data, "Conforme": "1", "Comentarios": item.value });
                }
            });
        }

        if (this.id == "rbNoConformeMasivo" && this.checked) {
            data = $(this).val();
            $.each(document.getElementsByClassName("comentario"), function (i, item) {
                if (txtcomentario == this.id) {
                    comentarioList.push({ "idOrdenExamen": data, "Conforme": "0", "Comentarios": item.value });
                }
            });
        }

    });
    if (comentarioList != '') {
        jConfirm("Desea validar los resultados de las Muestras seleccionadas?", "Validacion masiva", function (rpta) {
            if (rpta) {
                ValidacionMasivaMuestras(comentarioList);
                //iBuscarClick();
            }
        })
    }
    else {
        alert('Debe seleccionar al menos un resultado.');
        return false;
    }
    //var data = '';
    //$('input[type=checkbox]').each(function () {

    //    if (this.checked) {
    //        if (this.id != "chkAll") {
    //            data += $(this).val() + '|';
    //        }
    //    }

    //});

    //if (data != '') {
    //    jConfirm("Desea validar los resultados de las Muestras seleccionadas?", "Validacion masiva", function (rpta) {
    //        if (rpta) {
    //            data = data.slice(0, -1);
    //            ValidacionMasivaMuestras(data);
    //        }
    //    })
    //}
    //else {
    //    alert('Debes seleccionar al menos una muestra.');
    //    return false;
    //}

});

//function iBuscarClick() {
//    document.getElementById("btnBuscar").click();
//}

function ValidacionMasivaMuestras(comentarioList) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ValidacionResultados/ValidarMuestrasMasivo",
        data: JSON.stringify(comentarioList),
        contentType: "application/json; charset=utf-8",
        success: function () {
            jAlert("Los resultados fueron validados correctamente", "Aviso");
            iBuscarClick();
        }
    });

    //var datos = "?datos=" + data;
    //$.ajax(
    //       {
    //           url: URL_BASE + "ValidacionResultados/ValidarMuestrasMasivo" + datos,
    //           cache: false,
    //           method: "GET"
    //       }).done(function (msg) {
    //           if (msg == "ok") {
    //               jAlert("Los resultados fueron validados correctamente", "Aviso", function () {
    //                   iBuscarClick();
    //               });
    //           } else {
    //               jAlert("No se pudo validar los resultados, revise los datos.", "Error")
    //           }

    //       }).fail(function () {
    //           jAlert("Se produjo un error de Conexión.", "Error")
    //       })

}

function HPacienteClick(NroDocumento) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Paciente/ShowHistoriaClinicaPaciente",
        data: { page: 1, NroDocumento: NroDocumento },
        success: function (result) {
            $('#Dato').html(result);
            //alert(result);
            $('#dtPacienteBusqueda').DataTable({
                "scrollY": "600px",
                "scrollX": "200px",
                "scrollCollapse": true
            });
            $('.dataTables_length').addClass('bs-select');
        }
    });
}

function SolicitaAUVerificador(idOrdenExamen) {
    debugger;
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "ValidacionResultados/SolicitaAUVerificador",
        data: { id: idOrdenExamen, estatusSol: 1 },
        contentType: "application/json; charset=utf-8",
        success: function (data) {


        }
    });
}

function SolicitarLiberacionResultado(idOrdenExamen) {
    var cadena;
    debugger;
    jConfirm('¿Desea enviar una solicitud para corregir el informe de resultado?', 'Confirmar Solicitud', function (r) {
        if (r) {
            $.ajax({
                url: URL_BASE + "ValidacionResultados/SolicitaAUVerificador?id=" + idOrdenExamen + "&estatusSol=1",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    location.reload(true);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(XMLHttpRequest);
                }
            });
            location.reload(true);
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