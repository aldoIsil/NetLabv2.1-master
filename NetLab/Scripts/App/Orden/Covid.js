function btnBorrarClick(idOrden) {
    //$.ajax({
    //    url: URL_BASE + "RecepcionMuestraKobo/DeleteDatosPaciente?idOrden=" + idOrden,
    //    type: "POST",
    //    contentType: "application/json; charset=utf-8",
    //    success: function (response) {
    //        console.log(response);
    //    }
    //})
}
function VerSolicitanteClick() {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "RecepcionMuestraKobo/VerSolicitante",
        success: function (result) {
            $('#Dato').html(result);
            $("#CodigoUnicoSolicitante").autocomplete({
                appendTo: $("#Ver"),
                source: function (request, response) {
                    $.ajax({
                        url: "Orden/GetEstablecimientosNew",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'prefix': '" + request.term + "'}",
                        success: function (data) {
                            response($.map(data, function (item) {
                                //
                                return { label: item.Nombre, value: item.IdEstablecimiento };
                            }));
                        }
                    });
                },
                select: function (e, i) {
                    e.preventDefault();
                    $("#hddDatoSolicitante").val(i.item.value);
                    $("#CodigoUnicoSolicitante").val(i.item.label);
                    
                    $("#btnAgregarSolicitante").on("click", function () {
        debugger;
        var Dni = $("#Dni").val();
        var codigoColegio = $("#codigoColegio").val();
        var ApePat = $("#apellidoPaterno").val();
        var ApeMat = $("#apellidoMaterno").val();
        var Nombre = $("#Nombres").val();
        var Correo = $("#correo").val();
        var profesion = $("#cmbProfesion").val();
        var telefono = $("#telefono").val();
        var laboratorio = $("#hddDatoSolicitante").val();

        //if (codigoColegio == undefined || codigoColegio == "" || codigoColegio == 0) {
        //    jAlert("Ingrese el Código del Colegio Profesional.", "Alerta!");
        //    return null;
        //}
        if (ApePat == undefined || ApePat == "" || ApePat == 0) {
            jAlert("Ingrese el Apellido Paterno.", "Alerta!");
            return null;
        }
        if (Nombre == undefined || Nombre == "" || Nombre == 0) {
            jAlert("Ingrese el Nombres.", "Alerta!");
            return null;
        }
        if (laboratorio == undefined || laboratorio == "" || laboratorio == 0) {
            jAlert("Seleccione Centro de Trabajo", "Alerta!");
            return null;
        }
        $.get(URL_BASE + "Orden/ValidarCodigoColegio?codigoColegio=" + codigoColegio, function (data) {
            if (data == "True") {
                jAlert("Código Colegio ya está registrado", "Alerta!");
                return null;
            }
            else {
                var datos = "?Dni=" + Dni + "&" +
                "codigoColegio=" + codigoColegio + "&" +
                "ApePat=" + ApePat + "&" +
                "ApeMat=" + ApeMat + "&" +
                "Nombre=" + Nombre + "&" +
                "Correo=" + Correo + "&" +
                "profesion=" + profesion + "&" +
                "laboratorio=" + laboratorio + "&" +
                "telefono=" + telefono

                dpUI.loading.start("Creando ...");
                $('#Ver').modal('toggle');
                $.ajax(
                    {
                        url: URL_BASE + "Orden/AddSolicitante" + datos,
                        cache: false,
                        method: "GET"
                    }).done(function (msg) {
                        //en msg devolver el codigo de colegio de solicitante creado y setearlo en #idSolicitante
                        //LoadSolicitantes();
                        debugger;
                        $.get(URL_BASE + "Solicitante/LoadSolicitante", function (data) {
                            $("#divSolicitante").html(data);
                            LoadSolicitantes();
                        });
                    }).fail(function () {
                        jAlert("Error, por favor comunicarse con el Administrador.", "Error");
                        dpUI.loading.stop();
                    }).success(function () {
                        dpUI.loading.stop();
                    });
            }
        });
    });

                },
                minLength: 1
            });},
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}
function clicbtnGuardarDatosAdicionales() {
    var data = $('#frmPreSaveOrden').serialize();
    console.log($("#hddDatoEESSOrigen").val());
    //Agregar Validacion 
    //fecha , estabelcimeinto
    var valida = "0";
    if ($('#fechaObtencion').val().length < 10) {
        alert('Fecha de obtención inválida.');
        valida = "1";
        if ($('#fechaObtencion').val() == "01/01/0001") {
            alert('Fecha de obtención inválida.');
            valida = "1";
        }
    }
    if ($('#hddDatoEESSOrigenEdit').val().length == 0) {
        alert('Establecimiento de origen inválido.');
        valida = "1";
    }
    debugger;
    if (valida == "0") {
        $.ajax({
            type: "POST",
            cache: false,
            data: data,
            url: URL_BASE + "RecepcionMuestraKobo/PreSaveOrden",
            success: function (result) {
                console.log("success2");
                alert(result);
                $('#AddVariable').modal('toggle');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("textStatus: ", textStatus);
                console.log("errorThrown: ", errorThrown);
                console.log(XMLHttpRequest);
            }
        });
    }
    
}
function AddVariablesClick(idOrden) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "RecepcionMuestraKobo/AddVariables",
        data: { idOrden: idOrden },
        success: function (result) {
            console.log("success1");
            $('#DatoVariable').html(result);
            $("#CodigoUnicoOrigenEdit").autocomplete({
                appendTo: $("#AddVariable"),
                source: function (request, response) {
                    debugger;
                    $.ajax({
                        url: URL_BASE + "Orden/GetEstablecimientosNew",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: "{ 'prefix': '" + request.term + "'}",
                        success: function (data) {
                            response($.map(data, function (item) {

                                return { label: item.Nombre, value: item.IdEstablecimiento };
                            }))
                        }
                    })
                },
                select: function (e, i) {
                    e.preventDefault();
                    $("#hddDatoEESSOrigenEdit").val(i.item.value);
                    $("#CodigoUnicoOrigenEdit").val(i.item.label);
                },
                minLength: 3
            });

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("textStatus: ", textStatus);
            console.log("errorThrown: ", errorThrown);
            console.log(XMLHttpRequest);
        }
    });
}
function LoadSolicitantes() {
    //alert("load solicitantes");
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
}

$(document).ready(function () {
    $(".datepickerMaxValue").setDatePickerWithMaxValue();

    LoadSolicitantes();

    $("#CodigoUnicoOrigen").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Orden/GetEstablecimientosNew",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ 'prefix': '" + request.term + "'}",
                success: function (data) {
                    response($.map(data, function (item) {

                        return { label: item.Nombre, value: item.IdEstablecimiento };
                    }))
                }
            })
        },
        select: function (e, i) {
            e.preventDefault();
            $("#hddDatoEESSOrigen").val(i.item.value);
            $("#CodigoUnicoOrigen").val(i.item.label);
        },
        minLength: 3
    });

    $("#CodigoUnicoEnvio").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Orden/GetEstablecimientosNew",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ 'prefix': '" + request.term + "'}",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Nombre, value: item.IdEstablecimiento };
                    }))
                }
            })
        },
        select: function (e, i) {
            e.preventDefault();
            $("#hddDatoEESSEnvio").val(i.item.value);
            $("#CodigoUnicoEnvio").val(i.item.label);
        },
        minLength: 3
    });

    $("#btnAgregar").click(function () {
        debugger;
        var Nombres = $('#Nombres').val();
        var apellidoPaterno = $('#apellidoPaterno').val();
        var apellidoMaterno = $('#apellidoMaterno').val();
        var nroDocumento = $('#nroDocumento').val();
        var TipoDocumento = $('#tipoDocumento').val();
        var contenido = '';
        var validaOrden = 0;


        //$(".nroDocumento").parent("tr").find(".nroDocumento").each(function () {
        //    pacientes.push($(this).html());
        //});

        if (pacientes.length == 0) {
            alert('No existen pacientes a registrar.');
            validaOrden = "1";
        }
        if ($('#tipoDocumento').val() == "0") {
            alert('Tipo de Documento inválido.');
            validaOrden = "1";
        }
        if ($('#nroDocumento').val() == "") {
            alert('Debe ingresar número de documento');
            validaOrden = "1";
        }
        if ($('#FechaObtencion').val().length < 10) {
            alert('Fecha de obtención inválida.');
            validaOrden = "1";
        }
        if ($('#fechaIngresoINS').val().length < 10) {
            alert('Fecha de ingreso al INS inválida.');
            validaOrden = "1";
        }
        if (!ValidacionDeFecha($('#FechaObtencion').val(), $('#fechaIngresoINS').val())) {
            jAlert("La Fecha de Obtención no puede ser mayor a la Fecha de Ingreso al ROM", "¡Alerta!");
            validaOrden = "1";
        }
        if ($('#CodigoUnicoOrigen').val().length == 0) {
            alert('Establecimiento de origen inválido.');
            validaOrden = "1";
        }
        if ($('#CodigoUnicoEnvio').val().length == 0) {
            alert('Establecimiento de destino inválido.');
            validaOrden = "1";
        }
        if ($('#solicitante').val().length == 0) {
            alert('Solicitante inválido.');
            validaOrden = "1";
        }
        $(".nroDocumento").parent("tr").find(".nroDocumento").each(function () {
            if (nroDocumento == $(this).html()) {
                jAlert('Nro de Documento existente.', 'Error');
                validaOrden = 1;
            }
            else {
                validaOrden = 0;
            }
        });
        debugger;
        if (validaOrden == "0") {
            var datoPaciente = "";
            $.ajax({
                url: URL_BASE + "Paciente/BusquedaPacienteCovid",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: "{ 'TipoDocumento': '" + TipoDocumento + "'," + "'nroDocumento': '" + nroDocumento + "'}",
                success: function (datos) {
                    debugger;
                    console.log(datos);
                    datoPaciente = datos;
                    if (datoPaciente.length == 0) {
                        jAlert('Nro de Documento no válido.', 'Error');
                    }
                    else {

                        $.ajax({
                            url: URL_BASE + "RecepcionMuestraKobo/PreRegistro?TipoDocumento=" + TipoDocumento + "&NroDocumento=" + nroDocumento + "&EstablecimientoOrigen=" + $('#hddDatoEESSOrigen').val() + "&EstablecimientoEnvio=" + $('#hddDatoEESSEnvio').val() + "&FechaObtencion=" + $('#FechaObtencion').val() + "&solicitante=" + $('#solicitante').val() + "&fechaIngresoINS=" + $('#fechaIngresoINS').val() + "&nroOficio=" + $('#nroOficio').val() + "&idOrdenPadreExportado=" + $('#idOrdenPadreExportado').val(),
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                debugger;
                                console.log(response);
                                var lst = response.split('|');
                                var idordenpadre = lst[2];
                                console.log("$(#idOrdenPadreExportado) : ", $("#idOrdenPadreExportado"));
                                $("#idOrdenPadreExportado").val(idordenpadre);
                                var idPaciente = response;
                                contenido += "<tr><td><button class='btn btn-default' id='btnAddVariable' onclick='AddVariablesClick(\"" + lst[1] + "\")' data-toggle='modal' data-idPaciente='" + idPaciente + "' data-target='#AddVariable' ><span class='fa fa-edit'></span></button></td><td class='nroDocumento' >" + nroDocumento + "</td><td>" + datoPaciente + "</td><td><button class='borrar btn btn-danger' id='btnBorrar' onclick='btnBorrarClick(\"" + lst[1] + "\")'> <span class='fa fa-trash-o fa-lg'></span></button></td></tr>";
                                console.log(idPaciente);
                                console.log(contenido);
                                $("#dtPaciente").css("display", "block");
                                $('#TbPaciente').append(contenido);
                            }
                        })
                    }
                }
            })
        }

    });

    $(document).on('click', '.borrar', function (event) {
        event.preventDefault();
        $(this).closest('tr').remove();
    });
    
    $("#btnGuardar").on("click", function (e) {
        debugger;
        e.preventDefault();
        var pacientes = [];
        var valida = 0;
        $(".nroDocumento").parent("tr").find(".nroDocumento").each(function () {
            pacientes.push($(this).html());
        });

        if (pacientes.length == 0) {
            alert('No existen pacientes a registrar.');
            valida = 1;
        }
        if ($('#tipoDocumento').val() == "0") {
            alert('Tipo de Documento inválido.');
            valida = 1;
        }
        if ($('#nroDocumento').val() == "") {
            alert('Debe ingresar número de documento');
            valida = 1;
        }
        if ($('#FechaObtencion').val().length < 10) {
            alert('Fecha de obtención inválida.');
            valida = 1;
        }
        if ($('#fechaIngresoINS').val().length < 10) {
            alert('Fecha de ingreso al INS inválida.');
            valida = 1;
        }
        if (!ValidacionDeFecha($('#FechaObtencion').val(), $('#fechaIngresoINS').val())) {
            jAlert("La Fecha de Obtención no puede ser mayor a la Fecha de Ingreso al ROM", "¡Alerta!");
            valida = 1;
        }
        if ($('#CodigoUnicoOrigen').val().length == 0) {
            alert('Establecimiento de origen inválido.');
            valida = 1;
        }
        if ($('#CodigoUnicoEnvio').val().length == 0) {
            alert('Establecimiento de destino inválido.');
            valida = 1;
        }
        if ($('#solicitante').val().length == 0) {
            alert('Solicitante inválido.');
            valida = 1;
        }

        document.getElementById("pacientes").value = pacientes; // JSON.stringify(pacientes);
        if (valida == 0) {
            $('#frmOrdenCovid')[0].submit();
            //Limpida datos Paciente
            document.getElementById("nroDocumento").value = '';
            $("#TbPaciente").find("tr").remove();
            $("#dtPaciente").css("display", "none");
            $('#idOrdenPadreExportado').val("");
        }
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
            jAlert("La Fecha de " + tipoFecha + " no puede ser mayor a la Fecha Actual", "¡Alerta!");
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
        //if (fecha === "") {
        //    return 1;
        //}

        //var ihoraFecha = null;

        ////Valida hora
        //if (hora != null) {
        //    if (hora === "") {
        //        return 2;
        //    }

        //    ihoraFecha = hora.split(":");

        //    if (ihoraFecha.length !== 2) {
        //        return 2;
        //    }
        //}

        return 0;
    }
    function ValidacionDeFecha(fecha1, fecha2) {
        //Split de las fechas recibidas para separarlas
        var x = fecha1.split('/');
        var z = fecha2.split('/');

        //Cambiamos el orden al formato americano, de esto dd/mm/yyyy a esto mm/dd/yyyy
        fecha1 = x[1] + '-' + x[0] + '-' + x[2];
        fecha2 = z[1] + '-' + z[0] + '-' + z[2];

        //Comparamos las fechas
        if (Date.parse(fecha1) > Date.parse(fecha2)) {
            return false;
        } else {
            return true;
        }
    }


    //$("#btnAddVariable").click(function (e) {
    //    AddVariablesClick();
    //});
    $(document).on("change", ".checkboxNoPrecisa", function (e) {
        console.log("$(this).data(inputid)", $(this).data("inputid"));
        var inputid = $(this).attr("data-inputid");
        if (!this.checked) {
            $(this).parents(".trpadre").find("input[name*='" + inputid + "']:not('.checkboxNoPrecisa')").prop("disabled", false);
        } else {
            $(this).parents(".trpadre").find("input[name*='" + inputid + "']:not('.checkboxNoPrecisa')").prop("disabled", true);
            $(this).parents(".trpadre").find("input[name*='" + inputid + "']:not('.checkboxNoPrecisa')").attr("value", "");
        }
    });
});