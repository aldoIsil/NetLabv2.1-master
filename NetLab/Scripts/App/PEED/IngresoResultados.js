jQuery(function ($) {

    var _oldShow = $.fn.show;

    $.fn.show = function (speed, oldCallback) {
        return $(this).each(function () {
            var obj = $(this),
                newCallback = function () {
                    if ($.isFunction(oldCallback)) {
                        oldCallback.apply(obj);
                    }
                    obj.trigger('afterShow');
                };

            // you can trigger a before show if you want
            obj.trigger('beforeShow');

            // now use the old function to show the element passing the new callback
            _oldShow.apply(obj, [speed, newCallback]);
        });
    }
});
$(document).ready(function () {
    $.ajaxSetup({ cache: false });

    $("#btnRegistrarResultado").click(function (e) {
        e.preventDefault();
        $('#frmResultadoPEEC').submit();
        return false;
    });
    $("#btnRefresh").click(function (e) {
        e.preventDefault();
        dpUI.loading.start("Cargando ...");
        
        var idEval = $('#idConfigEvaluacionHdd').val();
        var idPanel = $('#idConfiguracionPanelHdd').val();
        var idEEVal = $('#idEstablecimientoEvaluadorHdd').val();
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "ResultadoPEEC/Refresh",
            data: { idConfigEvaluacion: idEval, idConfiguracionPanel: idPanel, idEstablecimientoEvaluador: idEEVal },      
            success: function (res) {
                $("#Resultados").html(res);
                dpUI.loading.stop();
            }
        });
        return false;
    });
    $('body').on('submit', '#frmResultadoPEEC', function (e) {
        e.preventDefault();
        var ok = true;
        dpUI.loading.start("Agregando ...");
        if (ok) {
            $.ajax({
                type: "POST",
                cache: false,
                url: $(this).attr("action"),
                data: $(this).serialize(),
                success: function (data) {
                    $("#Resultados").html(data);
                    dpUI.loading.stop();
                }
            });
        }
        return false;
    });
});