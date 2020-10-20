$(document).ready(function () {
    var Enfermedad = $("#hdnEnfermedad").val();
    $("#idEnfermedad").chosen({ placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" });
    $("#idEnfermedad").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Orden/GetEnfermedades"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" });

    $("#CodigoUnicoEvaluado").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Catalogo/GetEESS",
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
            $("#hddDatoEvaluado").val(i.item.value);
            $("#CodigoUnicoEvaluado").val(i.item.label);
        },
        minLength: 1
    });
    $("#CodigoUnicoEvaluador").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Catalogo/GetEESS",
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
            $("#hddDatoEvaluador").val(i.item.value);
            $("#CodigoUnicoEvaluador").val(i.item.label);
        },
        minLength: 1
    });
    if(Enfermedad.length > 0)
    {
        $("#idEnfermedad").val(Enfermedad).trigger("chosen:updated");
    }
    
})
$('#dialog-edit').on('click', '#btnGuardarConfigEval', function (e) {
    $('#formAgregarEvaluacion').submit();
});
$("body").on("submit", "#formAgregarEvaluacion", function (e) {
    var ok = false;
    if ($("#idEnfermedad").val() == '') {
        alert('Enfermedad obligatoria.')
    }
    if ($("#CodigoUnicoEvaluador").val() == '') {
        alert('Establecimiento Evaluador obligatoria.')
    }
    if ($("#CodigoUnicoEvaluado").val() == '') {
        alert('Establecimiento Evaluado obligatoria.')
    }
    if ($("#CodigoUnicoEvaluado").val() == '') {
        alert('Establecimiento Evaluado obligatoria.')
    }
    if ($("#Nombre").val() == '') {
        alert('Nombre de la evaluación obligatoria.')
    }
    ok = true;
    var Evaluacion = new Object();
    Evaluacion.Nombre = $('#Nombre').val();
    Evaluacion.Descripcion = $('#Descripcion').val();
    Evaluacion.idEstablecimientoEvaluador = $('#hddDatoEvaluador').val();
    Evaluacion.idEstablecimientoEvaluado = $('#hddDatoEvaluado').val();
    Evaluacion.idEnfermedad = $('#idEnfermedad').val();

    if (ok) {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "ConfiguracionEvalControlCalidad/AddEvaluacion",
            data: { Nombre: Evaluacion.Nombre, Descripcion: Evaluacion.Descripcion, idEnfermedad: Evaluacion.idEnfermedad, idEESSEvaluador: Evaluacion.idEstablecimientoEvaluador, idEESSEvaluado: Evaluacion.idEstablecimientoEvaluado },
            success: function (response) {
                if (response == "1") {
                    alert("Evaluación Registrada Satisfactoriamente ", "Aviso");
                }
                else {
                    alert(response);
                }

            }
        });

    }

    e.preventDefault();
    return false;
});
//Edicion
$('#dialog-edit').on('click', '#btnEditarEval', function (e) {
    $('#formEditarEvaluacion').submit();
});
$("body").on("submit", "#formEditarEvaluacion", function (e) {
    var ok = false;
    if ($("#idEnfermedad").val() == '') {
        alert('Enfermedad obligatoria.')
    }
    if ($("#CodigoUnicoEvaluador").val() == '') {
        alert('Establecimiento Evaluador obligatoria.')
    }
    if ($("#CodigoUnicoEvaluado").val() == '') {
        alert('Establecimiento Evaluado obligatoria.')
    }
    if ($("#CodigoUnicoEvaluado").val() == '') {
        alert('Establecimiento Evaluado obligatoria.')
    }
    if ($("#Nombre").val() == '') {
        alert('Nombre de la evaluación obligatoria.')
    }
    ok = true;
    var Evaluacion = new Object();
    Evaluacion.idConfigEval =  $('#hdnIdEvaluacion').val();
    Evaluacion.Nombre = $('#Nombre').val();
    Evaluacion.Descripcion = $('#Descripcion').val();
    Evaluacion.idEstablecimientoEvaluador = $('#hddDatoEvaluador').val();
    Evaluacion.idEstablecimientoEvaluado = $('#hddDatoEvaluado').val();
    Evaluacion.idEnfermedad = $('#idEnfermedad').val();
    Evaluacion.estado = '1';
    if (ok) {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "ConfiguracionEvalControlCalidad/EditEvaluacion",
            data: { idConfigEval: Evaluacion.idConfigEval, Nombre: Evaluacion.Nombre, Descripcion: Evaluacion.Descripcion, idEnfermedad: Evaluacion.idEnfermedad, idEESSEvaluador: Evaluacion.idEstablecimientoEvaluador, idEESSEvaluado: Evaluacion.idEstablecimientoEvaluado, estado: Evaluacion.estado },
            success: function (response) {
                if (response == "1") {
                    alert("Evaluación Editado Satisfactoriamente ", "Aviso");
                }
                else {
                    alert(response);
                }

            }
        });

    }

    e.preventDefault();
    return false;
});