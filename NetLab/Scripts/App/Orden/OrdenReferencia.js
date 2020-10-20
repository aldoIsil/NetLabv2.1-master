// Descripción: Realiza la refernciación a otros laboratorios.
//              Obtiene todos los laboratorios para mostrarlos en el popup de la referenciación.
//              Ejecuta la transacicon para el registro de la referneciación.
// Author            : Terceros.
// Fecha Creación    : 01/01/2017.
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
$(document).ready(function () {

    $("input.datepickerReferencia:text").kendoDatePicker({
        culture: "es-PE"
    });
    
    $('input.datepickerReferencia:text').each(function (i, obj) {
        var datepicker = $(this).data("kendoDatePicker");
        datepicker.enable(false);
    });

    $(".chkReferenciar").on("change", function () {
        var datepicker = $(this).closest("tr").find('input.datepickerReferencia:text').data("kendoDatePicker");
        if ($(this).is(':checked')) {
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


    $("#tblDatosReferencia .inHoraRec").timeEntry({ show24Hours: true });
    /*$("#tblDatosReferencia .inFechaRec").val(dia + "/" + mes + "/" + anio);*/

    var myDate = new Date();
    var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
    var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();

    $("#tblDatosReferencia .inHoraRec").val(hora + ":" + minutos);

    $("#tblDatosReferencia .idLaboratorioDestino").each(function () {
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

    $("#btnReferenciar").on("click", function (e) {
    
        var ok = true;

        return ok;
    });

});