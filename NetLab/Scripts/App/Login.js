// Descripción: funciones de carga obtener el valor seleccionado de cada lista, realiza busqueda sensitiva de cada lista.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
$(document).ready(function () {
    $("#selEstablecimiento").chosen({ placeholder_text_single: "Seleccione un Establecimiento", no_results_text: "No existen coincidencias" });
    $("#seldisa").chosen({ placeholder_text_single: "Seleccione un elemento", no_results_text: "No existen coincidencias" });
    $("#selInstituciones").chosen({ placeholder_text_single: "Seleccione una Institucion", no_results_text: "No existen coincidencias" });
    $("#selRed").chosen({ placeholder_text_single: "Seleccione una Red", no_results_text: "No existen coincidencias" });
    $("#selmicrored").chosen({ placeholder_text_single: "Seleccione una Micro Red", no_results_text: "No existen coincidencias" });

    $('#btnEstablecimiento').click(function () {
        var selected = $('#selEstablecimiento').val();
        //var ok = true;
        $('#hdnEstablecimiento').val(selected);
        //if (selected == "0") {
        //    alert("Por favor, seleccionar un establecimiento.");
        //    ok = false;
        //}
        selected = $('#selRed').val();
        $('#hdnRed').val(selected);

        selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        selected = $('#selmicrored').val();
        $('#hdnMicroRed').val(selected);

        $('#hdnTipo').val('Establecimientos');

        $('#frmInstituciones').submit();
        //if (ok) {
            
        //}

    });

    $('#selInstituciones').change(function () {

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

});
