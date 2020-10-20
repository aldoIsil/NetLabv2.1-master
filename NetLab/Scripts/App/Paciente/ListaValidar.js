$(document).ready(function () {

    $(".datepickerTelerik").kendoDatePicker({
        culture: "es-PE"
    });
    // Descripción: Realiza la validacion de las Fechas de busqueda.
    // Author: Terceros.
    // Fecha Creacion: 01/01/2017
    // Fecha Modificación: 02/02/2017.
    // Modificación: Se agregaron comentarios.
    $("#btnBuscar").on("click", function () {



        var fechaDesde = $.trim($("#datepickerDesde").val());
        var fechaHasta = $.trim($("#datepickerHasta").val());
        var fechaDesdeSplit = fechaDesde.split('/');
        var fechaHastaSplit = fechaHasta.split('/');

        var fechaFrom = new Date(fechaDesdeSplit[2], fechaDesdeSplit[1], fechaDesdeSplit[0],0,0,0);
        var fechaTo = new Date(fechaHastaSplit[2], fechaHastaSplit[1], fechaHastaSplit[0], 0, 0, 0);
        if (fechaFrom > fechaTo)
        {
            jAlert("La fecha Desde no puede ser superior a la fecha Hasta.", "Alerta!");
            return false;
        }

        var result = FechaValidaBusqueda('datepickerDesde', 'datepickerHasta');
        if (!result) return false;

        dpUI.loading.start("Buscando ...");
    });
    // Descripción: Muestra popup de espera para la validación del paciente.
    // Author: Terceros.
    // Fecha Creacion: 01/01/2017
    // Fecha Modificación: 02/02/2017.
    // Modificación: Se agregaron comentarios.
    $(".validarPaciente").on("click", function (e, params) {
        e.preventDefault();
        var url = $(this).attr("href") +"&_=" + (new Date()).getTime();
        $("#dialog-open").html("");
        $("#dialog-open").dialog({
            title: "Validar Paciente",
            autoOpen: false,
            resizable: false,
            height: 230,
            width: 580,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                dpUI.loading.start("Validando ...");

                $(this).load(url, function () {
                    dpUI.loading.stop();
                });

            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });
        $("#dialog-open").dialog("open");
        return false;
    });







   
});
