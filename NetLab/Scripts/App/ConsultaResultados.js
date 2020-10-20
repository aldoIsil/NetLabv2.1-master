var myDate = new Date();
var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
var anio = myDate.getFullYear();

var idOrdenSel = "";

restaFechas = function (f1, f2) {
    var aFecha1 = f1.split('/');
    var aFecha2 = f2.split('/');
    var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
    var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
    var dif = fFecha2 - fFecha1;
    var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
    return dias;
}

function BuscarResultados() {
    debugger;
    var fechaDesde = $('#datepickerDesde').val();
    var fechaHasta = $('#datepickerHasta').val();
    var idEnfermedad = $('#idEnfermedad').val();
    var dFechas = restaFechas(fechaDesde, fechaHasta);

    if (idEnfermedad == 0) {
        jAlert("Debe seleccionar la enfermedad", "Alerta");
        return false;
    }
    if (dFechas >= 31) {
        jAlert("La diferencia entre fechas no deben ser mayor a un mes", "Alerta");
        return false;
    }
    window.location = URL_BASE + "ConsultaResultados/GetConsultaResultadosRecepcionINS?idEnfermedad=" + idEnfermedad + "&fechaDesde=" + fechaDesde + "&fechaHasta=" + fechaHasta;
}

function BuscarEstablecimiento() {
    $("#CodigoUnico").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Muestra/GetEstablecimientos?prefix=" + request.term,
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
            $("#idEstablecimiento").val(i.item.value);
            $("#CodigoUnico").val(i.item.label);
        },
        minLength: 2
    });
    $("#CodigoUnico").click(function () {
        $("#idEstablecimiento").val("");
        $("#CodigoUnico").val("");
    });
}

$(document).ready(function () {
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

    $("#selEstablecimiento").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#seldisa").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#selInstituciones").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#selRed").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#selmicrored").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#idEnfermedad").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#idExamen").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });

    $('#btnEstablecimiento').click(function () {

        debugger;
        //se agrego para mostar mensaje de cargando y bloquear la pagina al dar un click y asi evitar que el usuario de multiples clicks    
       // dpUI.loading.start("Cargando ...");

        var selected = $('#selEstablecimiento').val();
        $('#hdnEstablecimiento').val(selected);

        var selected2 = $('#selRed').val();
        $('#hdnRed').val(selected2);

        var selected3 = $('#seldisa').val();
        $('#hdnDisa').val(selected3);

        var selected4 = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected4);

        var selected5 = $('#selmicrored').val();
        $('#hdnMicroRed').val(selected5);

        $('#hdnTipo').val('Establecimientos');

        var selectedenf = $('#idEnfermedad').val();
        $('#hdnEnfermedad').val(selectedenf);

        var selectedexa = $('#idExamen').val();
        $('#hdnExamen').val(selectedexa);

        console.log(selected);
        console.log(selected2);
        console.log(selected3);
        console.log(selected4);
        console.log(selected5);

        var rbnDatoClinico = $("#rbnDatoClinico").is(":checked");
        if (rbnDatoClinico && selectedenf == '') {
            jAlert('Debe ingresar la Enfermedad para consultar datos clínicos', '¡Alerta!');
            return false;
        }        
        //setTimeout(function () {
        //   // $('#frmInstituciones').submit();
        //    dpUI.loading.start("Cargando ...");
        //}, 6000);

    });

    $('#selInstituciones').change(function () {
        debugger;
        var selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        $('#hdnTipo').val('Instituciones');
        $('#frmInstituciones').submit();
    });

    $('#seldisa').change(function () {

        var selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        $('#hdnTipo').val('Disa');
        $('#frmInstituciones').submit();
    });

    $('#selRed').change(function () {

        var selected = $('#selRed').val();
        $('#hdnRed').val(selected);

        selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        $('#hdnTipo').val('Red');
        $('#frmInstituciones').submit();
    });

    $('#selmicrored').change(function () {

        var selected = $('#selRed').val();
        $('#hdnRed').val(selected);

        selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        selected = $('#selmicrored').val();
        $('#hdnMicroRed').val(selected);

        $('#hdnTipo').val('MicroRed');
        $('#frmInstituciones').submit();
    });

    $('#selEstablecimiento').change(function () {

        var selected = $('#selEstablecimiento').val();
        $('#hdnEstablecimiento').val(selected);

        selected = $('#selRed').val();
        $('#hdnRed').val(selected);

        selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        selected = $('#selmicrored').val();
        $('#hdnMicroRed').val(selected);

        $('#frmEstablecimiento').submit();
    });

    
    $('#frmInstituciones input').on('change', function () {
        debugger;
        var value = ($('input[name=esTipoEstablecimiento]:checked', '#frmInstituciones').val());
        //alert($('input[name=esTipoEstablecimiento]:checked', '#frmInstituciones').val());
        if (value == "2") {

            $("#selInstituciones").attr('disabled', 'disabled');
            $("#selInstituciones").attr('disabled', true).trigger("chosen:updated");

            $("#seldisa").attr('disabled', 'disabled');
            $("#seldisa").attr('disabled', true).trigger("chosen:updated");

            $("#selRed").attr('disabled', 'disabled');
            $("#selRed").attr('disabled', true).trigger("chosen:updated");

            $("#selmicrored").attr('disabled', 'disabled');
            $("#selmicrored").attr('disabled', true).trigger("chosen:updated");

            $("#selEstablecimiento").attr('disabled', 'disabled');
            $("#selEstablecimiento").attr('disabled', true).trigger("chosen:updated");

            $("#txtEstablecimiento").removeAttr('disabled');
            $("#txtEstablecimiento").attr('disabled', false).trigger("chosen:updated");

            $("#CodigoUnico").removeAttr('disabled');
            $("#CodigoUnico").attr('disabled', false).trigger("chosen:updated");

            BuscarEstablecimiento();

        } else if (value == "1") {

            $("#selInstituciones").removeAttr('disabled');
            $("#selInstituciones").attr('disabled', false).trigger("chosen:updated");

            $("#seldisa").removeAttr('disabled');
            $("#seldisa").attr('disabled', false).trigger("chosen:updated");

            $("#selRed").removeAttr('disabled');
            $("#selRed").attr('disabled', false).trigger("chosen:updated");

            $("#selmicrored").removeAttr('disabled');
            $("#selmicrored").attr('disabled', false).trigger("chosen:updated");

            $("#selEstablecimiento").removeAttr('disabled');
            $("#selEstablecimiento").attr('disabled', false).trigger("chosen:updated");

            $("#txtEstablecimiento").attr('disabled', 'disabled');
            $("#txtEstablecimiento").attr('disabled', true).trigger("chosen:updated");

            $("#CodigoUnico").attr('disabled', 'disabled');
            $("#CodigoUnico").attr('disabled', true).trigger("chosen:updated");

        } else if (value == "3") {

            $("#selInstituciones").removeAttr('disabled');
            $("#selInstituciones").attr('disabled', false).trigger("chosen:updated");

            $("#seldisa").removeAttr('disabled');
            $("#seldisa").attr('disabled', false).trigger("chosen:updated");

            $("#selRed").removeAttr('disabled');
            $("#selRed").attr('disabled', false).trigger("chosen:updated");

            $("#selmicrored").removeAttr('disabled');
            $("#selmicrored").attr('disabled', false).trigger("chosen:updated");


            $("#selEstablecimiento").removeAttr('disabled');
            $("#selEstablecimiento").attr('disabled', false).trigger("chosen:updated");

            $("#txtEstablecimiento").attr('disabled', 'disabled');
            $("#txtEstablecimiento").attr('disabled', true).trigger("chosen:updated");

            $("#CodigoUnico").attr('disabled', 'disabled');
            $("#CodigoUnico").attr('disabled', true).trigger("chosen:updated");

        }
        else if (value == "4") {

            $("#selInstituciones").attr('disabled', 'disabled');
            $("#selInstituciones").attr('disabled', true).trigger("chosen:updated");

            $("#seldisa").attr('disabled', 'disabled');
            $("#seldisa").attr('disabled', true).trigger("chosen:updated");

            $("#selRed").attr('disabled', 'disabled');
            $("#selRed").attr('disabled', true).trigger("chosen:updated");

            $("#selmicrored").attr('disabled', 'disabled');
            $("#selmicrored").attr('disabled', true).trigger("chosen:updated");

            $("#selEstablecimiento").attr('disabled', 'disabled');
            $("#selEstablecimiento").attr('disabled', true).trigger("chosen:updated");

            $("#txtEstablecimiento").removeAttr('disabled');
            $("#txtEstablecimiento").attr('disabled', false).trigger("chosen:updated");

            $("#CodigoUnico").removeAttr('disabled');
            $("#CodigoUnico").attr('disabled', false).trigger("chosen:updated");

            BuscarEstablecimiento();
        }
    });

    $("#idEnfermedad").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 1,
        afterTypeDelay: 100,
        cache: false,
        url: URL_BASE + "Orden/GetEnfermedades"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" }).change(function () {
        $("#idExamen").ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 100,
            cache: false,
            url: URL_BASE + "Orden/GetExamenes?genero=3&idenfermedad=" + $("#idEnfermedad").val()
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, { placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
    });

});







