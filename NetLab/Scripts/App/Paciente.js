// Descripción: funciones para mostrar popup y seleccionar una plantilla.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function selectorPlantilla(element) {
    //e.preventDefault();
    //var idTipoMuestra = $(this).parent().attr("class");
    var url = element.href + "&_=" + (new Date()).getTime();
    $("#dialog-edit").html("");
    $("#dialog-edit").dialog({
        title: "Seleccionar Plantilla",
        autoOpen: false,
        resizable: false,
        height: 220,
        width: 400,
        show: { effect: "drop", direction: "up" },
        modal: true,
        draggable: true,
        open: function () {
            $(this).load(url, function () {

            });

        },
        close: function (event, ui) {
            $(this).dialog('close');
        }
    });
    $("#dialog-edit").dialog("open");
    return false;
}



$(document).ready(function () {
    var textoRegistro = $("#textoRegistro").val();
    
    if (textoRegistro != undefined) {
        jAlert(textoRegistro, "¡Información!")
    }

    $("#btnBuscar").on("click", function () {

        var nroDocumento = $.trim($("#nroDocumento").val());
        var tipoDocumento = $("#tipoDocumento").val();

        if (nroDocumento == "" && tipoDocumento > 0) {
            jAlert("Ingrese Nro de Documento.", "¡Alerta!");
            return false;
        }

        if (tipoDocumento == 1 && nroDocumento.length != 8) {
            jAlert("Ingrese Nro de DNI válido, debe ser de 8 dígitos.", "¡Alerta!");
            return false;
        }

        if ((tipoDocumento == 2 || tipoDocumento == 3) && nroDocumento.length > 12) {
            jAlert("Nro de documento no puede tener más de 12 dígitos.", "¡Alerta!");
            return false;
        }
    });

    $("#HCPaciente").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr('href'),
            data: { page: 1, NroDocumento: $(this).data('nrodocumento') },
            success: function (result) {
                /*Cambio el valor del scroll X Y a 100% para que no se mueva el header*/
                $('#HisClinica').html(result);
                $('#dtPacienteBusqueda').DataTable({
                    "scrollY": "500px",
                    "scrollX": "100%",
                    "scrollCollapse": true
                });
                $('.dataTables_length').addClass('bs-select');
            }
        });
        console.log("entro");

        return false;
    });
    $("#HCPacienteNetLab1").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr('href'),
            data: { page: 1, NroDocumento: $(this).data('nrodocumento'), CodigoMuestra: '' },
            success: function (result) {
                $('#HisNetLab1').html(result);
            }
        });
        return false;
    });
    $("#btnBuscarHistorial").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "Paciente/ShowInformacionNetLabv1",
            data: { page: 1, NroDocumento: $('#nroDocumento').val(), CodigoMuestra: $('#CodigoMuestra').val() },
            success: function (result) {
                $('#HisNetLab1').html(result);
            }
        });
        return false;
    });

    $("#btnImprimir").on("click", function () {
        debugger;
        var data = '';
        var id = [];
        $('input[type=checkbox]').each(function () {
            if (this.checked) {
                data += $(this).val() + '","';    
            }
        });
        data = data.slice(0, -3);
        id = data.split(', ');
        for (var i = 0; i <= id.length; i++) {
            if (i > 0) {
                if (id[i] != id[i - 1]) {
                    jAlert("Solo puede imprimir exámenes de una solo Orden", "Alerta!")
                    return false;
                }
            }
        }

                
    });



    $("#btnBuscarPaciente").on("click", function () {
        $("#dvResultado").html('');
        var nroDocumento = $("#nroDocumento").val();
        var apellidoPaterno = $("#apellidoPaterno").val();
        var apellidoMaterno = $("#apellidoMaterno").val();
        var nombre = $("#nombre").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "Paciente/BuscarPacienteLaboratorio",
            data: {
                nroDocumento: nroDocumento,
                apellidoPaterno: apellidoPaterno,
                apellidoMaterno: apellidoMaterno,
                nombre: nombre
            },
            success: function (result) {
                var lista = [];
                var paciente = [];
                var contenido = "";
                if (result.length > 0) {
                    lista = result.split('|');
                    contenido += "<table id='dtPacienteLaboratorio' class='table - responsive'><thead><tr><th>Tipo de Documento</th><th>Número de Documento</th><th>Apellido Paterno</th><th>Apellido Materno</th><th>Nombres</th>";
                    contenido += "<th>Historial de Exámenes del Paciente</th></tr></thead>";
                    for (var i = 0; i < lista.length; i++) {
                        paciente = lista[i].split(',');
                        contenido += "<tr><td style='display:none'>" + paciente[0] + "</td><td>" + paciente[4] + "</td><td>" + paciente[5] + "</td><td>" + paciente[1] + "</td><td>" + paciente[2] + "</td><td>" + paciente[3] + "</td>";
                        contenido += "<td><button type='button' class='btn btn-default btn-block fa fa-user-md' onclick=verResultado(" + "'" + paciente[0] + "')></button></td></tr>";
                    }
                    contenido += "</table>";
                }
                else {
                    contenido = "<div class='alert alert-danger'>No se encontraron registros del paciente dentro de su Jurisdicción</div>";
                }
                
                $("#dvPaciente").html(contenido);
                
            }
        });
    });


    $("#btnConsultarPaciente").on("click", function () {
        debugger;
        $("#dvResultado").html('');
        $('#InfoPruebaRapida').html('');
        var nroDocumento = $("#nroDocumento").val();
        var apellidoPaterno = $("#apellidoPaterno").val();
        var apellidoMaterno = $("#apellidoMaterno").val();
        var nombre = $("#nombre").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "Paciente/ConsultaDatoPaciente",
            data: {
                nroDocumento: nroDocumento,
                apellidoPaterno: apellidoPaterno,
                apellidoMaterno: apellidoMaterno,
                nombre: nombre
            },
            success: function (result) {
                var lista = [];
                var paciente = [];
                var contenido = "";
                if (result.length > 0) {
                    lista = result.split('|');
                    contenido += "<table id='dtPacienteExterno' class='table - responsive'><thead><tr><th>Tipo de Documento</th><th>Número de Documento</th><th>Apellido Paterno</th><th>Apellido Materno</th><th>Nombres</th>";
                    contenido += "<th>Historial de Exámenes del Paciente</th><th>Información de Prueba Rápida</th></tr></thead>";
                    for (var i = 0; i < lista.length; i++) {
                        paciente = lista[i].split(',');
                        contenido += "<tr><td style='display:none'>" + paciente[0] + "</td><td>" + paciente[4] + "</td><td>" + paciente[5] + "</td><td>" + paciente[1] + "</td><td>" + paciente[2] + "</td><td>" + paciente[3] + "</td>";
                        contenido += "<td><button type='button' class='btn btn-default btn-block fa fa-user-md' onclick=verResultadoPacienteExterno(" + "'" + paciente[0] + "')></button></td>";
                        if (paciente[6] != '0') {
                            contenido += "<td><button type='button' class='btn btn-default btn-block fa fa-medkit' onclick=verResultadoPruebaRapidaCovid(" + "'" + paciente[5] + "')></button></td></tr>";
                        } else {
                            contenido += "<td>No registra Prueba Rápida SISCOVID</td></tr>";
                        }
                    }
                    contenido += "</table>";
                }
                else {
                    contenido = "<div class='alert alert-danger'>No se encontraron registros del paciente dentro de su Jurisdicción</div>";
                }

                $("#dvPaciente").html(contenido);

            }
        });
    });

});

function verResultado(idPaciente) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Paciente/ConsultaExternaPaciente",
        data: {
            idPaciente: idPaciente
        },
        success: function (result) {
            var lista = [];
            var resultado = [];

            if (result.length > 0) {
                lista = result.split('$');
                var contenido = "";
                contenido += "<table id='dtResultado' class='table - responsive'><thead><tr><th></th><th>Nro Oficio</th><th>Código Orden</th><th>Fecha Obtención</th><th>Enfermedad</th><th>Examen</th>";
                contenido += "<th>Componente</th><th>Fecha Publicacion Resultado</th><th>EESS-Origen</th><th>EESS-Destino</th><th>Estado</th><th>Criterio de Rechazo</th></tr></thead>";
                for (var i = 0; i < lista.length; i++) {
                    resultado = lista[i].split('|');
                  
                    contenido += "<tr><td>";
                    console.log(resultado[12]);
                    if (resultado[12]=='11') {
                        contenido += '<a href="/ReporteResultados/ImprimirResultadosBusqueda?idOrden=' + resultado[13] + '&idLaboratorio=' + resultado[14] + '&idExamen=' + resultado[15] +'" target="_blank" class="lnkForm"><img src="/img/imgPdf.gif" /></a>';
                    }
                    contenido +="</td><td>" + resultado[0] + "</td><td>" + resultado[1] + "</td><td>" + resultado[2] + "</td><td>" + resultado[3] + "</td><td>" + resultado[4] + "</td>";
                    contenido += "<td>" + resultado[5] + "</td><td>" + resultado[7] + "</td><td>" + resultado[8] + "</td><td>" + resultado[9] + "</td>";
                    contenido += "<td>" + resultado[10] + "</td><td>" + resultado[11] + "</td></tr>";
                }
                contenido += "</table>";
            }
            else {
                contenido = "<div class='alert alert-danger'>No se encontraron resultados del paciente</div>";
            }
            
            $("#dvResultado").html(contenido);

        }
    });
}


function verResultadoPacienteExterno(idPaciente) {
    debugger;
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Paciente/ResultadosConsultaExternaPaciente",
        data: {
            idPaciente: idPaciente
        },
        success: function (result) {
            var lista = [];
            var resultado = [];

            if (result.length > 0) {
                lista = result.split('$');
                var contenido = "";
                contenido += "<table id='dtResultadoExterno' class='table - responsive'><thead><tr><th></th><th>Nro Oficio</th><th>Código Orden</th><th>Fecha Obtención</th><th>Enfermedad</th><th>Examen</th>";
                contenido += "<th>Componente</th><th>Resultado</th><th>Fecha Publicacion Resultado</th><th>EESS-Origen</th><th>EESS-Destino</th><th>Estado</th><th>Criterio de Rechazo</th></tr></thead>";
                for (var i = 0; i < lista.length; i++) {
                    resultado = lista[i].split('|');

                    contenido += "<tr><td>";
                    console.log(resultado[12]);
                    if (resultado[12] == '11') {
                        contenido += '<a href="/ReporteResultados/ImprimirResultadosBusqueda?idOrden=' + resultado[13] + '&idLaboratorio=' + resultado[14] + '&idExamen=' + resultado[15] + '" target="_blank" class="lnkForm"><img src="/img/imgPdf.gif" /></a>';
                    }
                    contenido += "</td><td>" + resultado[0] + "</td><td>" + resultado[1] + "</td><td>" + resultado[2] + "</td><td>" + resultado[3] + "</td><td>" + resultado[4] + "</td>";
                    contenido += "<td>" + resultado[5] + "</td><td>" + resultado[6] + "</td><td>" + resultado[7] + "</td><td>" + resultado[8] + "</td><td>" + resultado[9] + "</td>";
                    contenido += "<td>" + resultado[10] + "</td><td>" + resultado[11] + "</td></tr>";
                }
                contenido += "</table>";
            }
            else {
                contenido = "<div class='alert alert-danger'>No se encontraron resultados del paciente</div>";
            }

            $("#dvResultado").html(contenido);

        }
    });
}

function verResultadoPruebaRapidaCovid(NroDocumento) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Paciente/MostrarPruebaRapidaSiscovidPaciente",
        data: { nroDocumento: NroDocumento },
        success: function (result) {
            /*Cambio el valor del scroll X Y a 100% para que no se mueva el header*/
            $('#InfoPruebaRapida').html(result);
            //$('#dtPRSiscovidPacienteResultados').DataTable({
            //    "scrollY": "500px",
            //    "scrollX": "100%",
            //    "scrollCollapse": true
            //});
            $('.dataTables_length').addClass('bs-select');
        }
    });
    console.log("entro");

    return false;
}

// Descripción: Ejecuta la busqueda y realiza postback.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function iBuscarClick() {
    document.getElementById("btnBuscar").click();
}

function Referenciar(sIdPaciente) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Paciente/ReferenciarPaciente",
        data: { idPaciente: sIdPaciente },
        success: function (result) {
            $('#RefPaciente').html(result);
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
                            }));
                        }
                    });
                },
                select: function (e, i) {
                    e.preventDefault();
                    $("#hddDatoEESSOrigen").val(i.item.value);
                    $("#CodigoUnicoOrigen").val(i.item.label);
                },
                minLength: 3
            });
        }
    });
}
function btnReferenciar(Observaciones, sidPaciente) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Paciente/ReferenciarPaciente2",
        data: { observaciones: $("#observaciones").val(), idPaciente: sidPaciente, idEstablecimiento: $("#hddDatoEESSOrigen").val() },
        success: function (result) {
            document.getElementById("btnBuscar").click();
        }
    });
}
// Descripción: Valida el tipo y nro de documento, dependiendo del tipo de documento valida el nro de documento.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;


    var tipoDocumento = $("#tipoDocumento").val();
    var nroDocumento = $.trim($("#nroDocumento").val());
    if (tipoDocumento == 1 && nroDocumento.length == 8) {
        return false;
    }

    if ((tipoDocumento == 4 && nroDocumento.length == 11)) {
        return false;
    }

    if ((tipoDocumento == 2 || tipoDocumento == 3) && nroDocumento.length > 12) {
        return false;
    }
    return true;
}

//
// Descripción: Se creo para que estas acciones funcionen en firefox. IE y Chrome no tienen problemas.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function EsCombinacionEspecial(event) {
    return event.charCode === 0 ||  //Esto es para firefox: delete,supr,arrow keys.
           (event.ctrlKey && event.key === "x") ||
           (event.ctrlKey && event.key === "c") ||
           (event.ctrlKey && event.key === "v") ||
           (event.key === "Enter");
}

//
// Descripción: Este metodo solo funciona con el keypress por lo que solo se necesita usar el event.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function NewInputHasValidCharacters(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

//
// Descripción: Este metodo solo funciona al momento de hacer 'paste' por lo que no se usa el event.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function NewInputIsValid(value) {
    var tipoDocumento = $("#tipoDocumento").val();
    var nroDocumento = $.trim($("#nroDocumento").val());

    var newValue = nroDocumento + value;

    if (tipoDocumento == 1 && (nroDocumento.length == 8 || newValue.length > 8)) {
        return false;
    }

    if ((tipoDocumento == 4) && (nroDocumento.length == 11 || newValue.length > 11)) {
        return false;
    }

    if ((tipoDocumento == 2 || tipoDocumento == 3) && (newValue.length > 12)) {
        return false;
    }

    if ((tipoDocumento == 7) && (nroDocumento.length == 20 || newValue.length > 20))
        return false;

    return true;
}

//
// Descripción: Evalua la cadeana pegada de acuerdo a la logica del metodo. Solo para el 'paste' event.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function NewInputHasValidPastedCharacters(value) {
    if (value.match(/^[0-9]+$/) == null)
        return false;

    return true;
}