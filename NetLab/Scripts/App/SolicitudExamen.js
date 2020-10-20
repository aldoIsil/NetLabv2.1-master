$(document).ready(function () {
    debugger;
    $("#divCargaViral").css("display", "none");
    $("#divCD4").css("display", "none");
    $(".criterio").click(function (evento) {
        var valor = $(this).val();
        if (valor == '2') {
            $("#divCargaViral").css("display", "block");
            $("#divCD4").css("display", "none");
        } else if (valor == '3') {
            $("#divCargaViral").css("display", "none");
            $("#divCD4").css("display", "block");
        } else {
            $("#divCargaViral").css("display", "none");
            $("#divCD4").css("display", "none");
        }
    });
    $(".resultado").click(function (evento) {
        var valor = $(this).val();
        if (valor == 'arn') {
            $("#divCargaViral").css("display", "block");
            $("#divCD4").css("display", "none");
        } else {
            $("#divCargaViral").css("display", "none");
            $("#divCD4").css("display", "block");
        }
    });

    //function imprSelec(nombre) {
    //    debugger;
    //    var ficha = document.getElementById(nombre);
    //    var ventimp = window.open(' ', 'popimpr');
    //    ventimp.document.write(ficha.innerHTML);
    //    ventimp.document.close();
    //    ventimp.print();
    //    ventimp.close();
    //}

    restaFechas = function (f1, f2) {
        var aFecha1 = f1.split('/');
        var aFecha2 = f2.split('/');
        var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
        var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
        var dif = fFecha2 - fFecha1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        return dias;
    }

    $("#btnEnviarGeno").on("click", function () {
        debugger;
        var criterio = $(':radio[name=criterio]:checked').val();

        var myDate = new Date();
        var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
        var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
        var anio = myDate.getFullYear();
        var fechaHoy = dia + '/' + mes + '/' + anio;

        if (criterio == 0 || criterio == undefined || criterio == '') {
            jAlert("Seleccione algún criterio para uso de la prueba de genotipificación", "Alerta");
            return false;
        }

        if (criterio == 2) {
            var resulCv = $('#resultadoCV').val();
            var FechaHoraEmisionCV = $('#FechaHoraEmisionCV').val();
            //var resulCv = resultado.replace(/[<,>.' 'A-Z/-]/gi, '');
            if (resulCv < 1000) {
                jAlert("La CV debe ser ≥1000 para solicitar la genotipificación a partir de ARN, pase al criterio 3", "Alerta");
                return false;
            }
            var dFechas = restaFechas(FechaHoraEmisionCV, fechaHoy);
            if (dFechas < 30) {
                jAlert("La fecha de la ultima Carga Viral es menor a 30", "Alerta");
                return false;
            } else if (dFechas > 90) {
                jAlert("El tiempo entre la última CV o CD4, y la solicitud de genotipificación debe ser menor a 3 meses. Se debe solicitar nueva CV o CD4, si el paciente tiene buena adherencia", "Alerta");
                return false;
            }
        }
        else if (criterio == 3) {
            var ResultadoCD4 = $('#ResultadoCD4').val();
            var FechaResultadoCD4 = $('#FechaResultadoCD4').val();
            if (ResultadoCD4 < 100) {
                jAlert("El CD4 debe ser ≥100 para solicitar la genotipificación a partir de ADN", "Alerta");
                return false;
            }
            var dFechas = restaFechas(FechaResultadoCD4, fechaHoy);
            if (dFechas < 30) {
                jAlert("La fecha del ultimo CD4 es menor a 30", "Alerta");
                return false;
            } else if (dFechas > 90) {
                jAlert("El tiempo entre la última CV o CD4, y la solicitud de genotipificación debe ser menor a 3 meses. Se debe solicitar nueva CV o CD4, si el paciente tiene buena adherencia", "Alerta");
                return false;
            }
        }

        jAlert("La solicitud para el examen de Genotipificación se realizó satisfactoriamente", "Alerta");
        $("#btnEnviarGeno").css("display", "none");
    });

    $("#btnEnviarTropismo").on("click", function () {
        debugger;
        //var codigoOrden = $("#CodigoOrden").val();
        //var idEstablecimiento = $("#idEstablecimiento").val();
        //var idCriterio = $(':radio[name=rbCriterio]:checked').val();
        var idCriterio = $(':radio[name=criterio]:checked').val();
        //var idResultado = $(':input[name=resultado]:checked').val();

        if (idCriterio == "" || idCriterio == undefined) {
            jAlert("Debe elegir un criterio de uso para la solicitud de Tropismo", "¡Alerta!");
            return false;
        }

        //var fechaTropismo = $("#datepickerfechaTropismo").val();
        //if (fechaTropismo == "" || fechaTropismo == undefined) {
        //    jAlert("Debe ingresar fecha de solicitud de Tropismo", "¡Alerta!");
        //    return false;
        //}
        //var fechaObtencion = $("#datepickerfechaObtencion").val();
        //if (fechaObtencion == "" || fechaObtencion == undefined) {
        //    jAlert("Debe ingresar fecha de obtención de Muestra", "¡Alerta!");
        //    return false;
        //}

        //if (idResultado == 'arn') {
        //    var fechacv = $("#FechaHoraEmisionCV").val();
        //    if (fechacv == "" || fechacv == undefined || fechacv == "No presenta") {
        //        jAlert("Solicitud no presenta fecha de resultado de la Carga Viral", "¡Alerta!");
        //        return false;
        //    }
        //    var resultadoCv = $("#resultadoCV").val().replace(/[<,>.' 'A-Z/-]/gi, '');
        //    if (resultadoCv < 1000) {
        //        jAlert("La CV debe ser ≥1000 para solicitar la prueba de Tropismo a partir de plasma.  Si el paciente presenta menos de 1000 copias, deberá solicitar la prueba a partir de ADN proviral", "¡Alerta!");
        //        return false;
        //    }
        //    var diascv = restaFechas(fechacv, fechaTropismo);
        //    if (diascv < 30) {
        //        jAlert("La fecha de solicitud de Tropismo debe ser mayor a 30 días con respecto a la fecha de la Carga Viral", "¡Alerta!");
        //        return false;
        //    } else if (diascv > 90) {
        //        alert("El tiempo entre la última CV y la solicitud de Tropismo debe ser menor a 3 meses. Se debe solicitar nueva CV si el paciente tiene buena adherencia", "¡Alerta!");
        //        return false;
        //    }
        //}
        //else if (idResultado == "adn") {
        //    var fechaCd4 = $("#FechaResultadoCD4").val();
        //    if (fechaCd4 == "" || fechaCd4 == undefined || fechaCd4 == "No presenta") {
        //        jAlert("Solicitud no presenta fecha de resultado de CD4", "¡Alerta!");
        //        return false;
        //    }

        //    var resultadoCD4 = $("#ResultadoCD4").val().replace(/[<,>.' 'A-Z/-]/gi, '');
        //    if (resultadoCv < 50) {
        //        jAlert("El CD4 debe ser ≥50 para solicitar la prueba de Tropismo a partir de sangre total.  Si el paciente presenta menos de 50 células deberá solicitar la prueba a partir de carga viral.", "¡Alerta!");
        //        return false;
        //    }
        //}
        var myDate = new Date();
        var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
        var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
        var anio = myDate.getFullYear();
        var fechaHoy = dia + '/' + mes + '/' + anio;

        if (idCriterio == '3') {
            var ResultadoCD4 = $('#ResultadoCD4').val();
            var FechaResultadoCD4 = $('#FechaResultadoCD4').val();
            if (ResultadoCD4 < 50) {
                jAlert("El CD4 debe ser ≥50 para solicitar la prueba de Tropismo a partir de sangre total. Si el paciente presenta menos de 50 células deberá solicitar la prueba a partir de carga viral.", "Alerta");
                return false;
            }
            var dFechas = restaFechas(FechaResultadoCD4, fechaHoy);
            if (dFechas < 30) {
                jAlert("La fecha del ultimo CD4 es menor a 30", "Alerta");
                return false;
            }
            //else if (dFechas > 90) {
            //    jAlert("El tiempo entre la última CV o CD4, y la solicitud de genotipificación debe ser menor a 3 meses. Se debe solicitar nueva CV o CD4, si el paciente tiene buena adherencia", "Alerta");
            //    return false;
            //}
        }
        //else {
        //    jAlert("Debe indicar el tipo de modalidad", "¡Alerta!");
        //    return false;
        //}

        var tipoSolicitud = $('#tipoSolicitud').val();
        if (tipoSolicitud == 2) {
            jAlert("La solicitud para el examen de Tropismo se realizó satisfactoriamente", "Alerta");
        }
        else {
            jAlert("La solicitud para el examen de HLA B*5701 se realizó satisfactoriamente", "Alerta");
        }
        $("#btnEnviarTropismo").css("display", "none");

    });
});

//function SolicitudExamenVih(idPaciente, tipo) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        cache: false,
//        url: URL_BASE + "SolicitudExamen/SolicitudExamenVih",
//        data: {
//            idPaciente: idPaciente,
//            tipoSolicitud: tipo
//        }
//    });
//}

function HistorialPacienteVIH(idPaciente) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "SolicitudExamen/ConsultaHistorialPacienteVIH",
        data: {
            idPaciente: idPaciente
        },
        success: function (result) {
            var lista = [];
            var resultado = [];

            if (result.length > 0) {
                lista = result.split('$');
                var contenido = "";
                contenido += "<table id='dtResultado' class='table - responsive'><thead><tr><th>Nro Oficio</th><th>Código Orden</th><th>Fecha Obtención</th><th>Examen</th>";
                contenido += "<th>Componente</th><th>Resultado</th><th>Fecha Publicacion Resultado</th><th>EESS-Origen</th><th>EESS-Destino</th></tr></thead>";
                for (var i = 0; i < lista.length; i++) {
                    resultado = lista[i].split('|');
                    contenido += "<tr><td>" + resultado[0] + "</td><td>" + resultado[1] + "</td><td>" + resultado[2] + "</td><td>" + resultado[3] + "</td><td>" + resultado[4] + "</td>";
                    contenido += "<td>" + resultado[5] + "</td><td>" + resultado[6] + "</td><td>" + resultado[7] + "</td><td>" + resultado[8] + "</td></tr>";
                }
                contenido += "</table>";
            }

            $("#HistorialVIH").html(contenido);

        }
    });
}

function ValidaDatosExamen(idPaciente, tipo) {
    var examen = '';
    switch (tipo) {
        case '1':
            examen = 'Genotipificación';
            break;
        case '2':
            examen = 'Tropismo';
            break;
        default:
            examen = 'HLA B*5701';
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "SolicitudExamen/ValidaResultadoVih",
        data: {
            idPaciente: idPaciente
        },
        success: function (valor) {
            debugger;
            if (valor == '1') {
                window.location.href = 'SolicitudExamen/SolicitudExamenVih?idPaciente=' + idPaciente + '&tipoSolicitud=' + tipo;
                return false;
            }
            else {
                jAlert('Los resultados de Carga Viral o CD4 no cumplen los requisitos para generar una solicitud de '+ examen + '. Revise el historial del paciente');
            }
        }
    });
}