$.ajaxSetup({
    statusCode: {
        401: function (result) {
            window.location.href = jQuery.parseJSON(result.responseText).Url;
        }
    }
});


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isDecimalKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46)
        return false;
    return true;
}

//Juan Muga - metodo global para validar fecha ingresada manualmente
//$(document).ready(function () {
function ValidarFecha(input) {
    var result = true;
    var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    //debugger;
    if (input.match(dateformat)) {
        var seperator1 = input.split('/');
        var seperator2 = input.split('-');

        if (seperator1.length > 1) {
            var splitdate = input.split('/');
        }
        else if (seperator2.length > 1) {
            var splitdate = input.split('-');
        }
        var dd = parseInt(splitdate[0]);
        var mm = parseInt(splitdate[1]);
        var yy = parseInt(splitdate[2]);
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        var currentYear = (new Date).getFullYear();
        //if (yy < currentYear) {
        //    jAlert("Fecha incorrecta. Año ingresado es menor al actual", "¡Alerta!");
        //    result = false;
        //}
        //debugger;
        //if (new Date(yy, mm-1, dd).toLocaleDateString() < new Date().toLocaleDateString()) {
        if (new Date(yy, mm - 1, dd) > new Date()) {
            jAlert("Fecha incorrecta. No se puede ingresar fecha futura", "¡Alerta!");
            result = false;
        }
        else {
            if (mm == 1 || mm > 2) {
                if (dd > ListofDays[mm - 1]) {
                    jAlert("Fecha incorrecta. Formato permitido (en números): día/mes/año", "¡Alerta!");
                    result = false;
                }
            }
            if (mm == 2) {
                var lyear = false;
                if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                    lyear = true;
                }
                if ((lyear == false) && (dd >= 29)) {
                    jAlert("Fecha incorrecta. Este año Febrero no tiene más de 28 días", "¡Alerta!");
                    result = false;
                }
                if ((lyear == true) && (dd > 29)) {
                    jAlert("Fecha incorrecta. Este año Febrero no tiene más de 29 días", "¡Alerta!");
                    result = false;
                }
            }
        }
    }
    else {
        jAlert("Fecha incorrecta. Formato permitido (en números): día/mes/año", "¡Alerta!");
        result = false;
    }

    return result;
}

function ValidarFechaSinMax(input) {
    var result = true;
    var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    //debugger;
    if (input.match(dateformat)) {
        var seperator1 = input.split('/');
        var seperator2 = input.split('-');

        if (seperator1.length > 1) {
            var splitdate = input.split('/');
        }
        else if (seperator2.length > 1) {
            var splitdate = input.split('-');
        }
        var dd = parseInt(splitdate[0]);
        var mm = parseInt(splitdate[1]);
        var yy = parseInt(splitdate[2]);
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        var currentYear = (new Date).getFullYear();
        //if (yy < currentYear) {
        //    jAlert("Fecha incorrecta. Año ingresado es menor al actual", "¡Alerta!");
        //    result = false;
        //}
        //debugger;
        //if (new Date(yy, mm-1, dd).toLocaleDateString() < new Date().toLocaleDateString()) {

        if (mm == 1 || mm > 2) {
            if (dd > ListofDays[mm - 1]) {
                jAlert("Fecha incorrecta. Formato permitido (en números): día/mes/año", "¡Alerta!");
                result = false;
            }
        }
        if (mm == 2) {
            var lyear = false;
            if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                lyear = true;
            }
            if ((lyear == false) && (dd >= 29)) {
                jAlert("Fecha incorrecta. Este año Febrero no tiene más de 28 días", "¡Alerta!");
                result = false;
            }
            if ((lyear == true) && (dd > 29)) {
                jAlert("Fecha incorrecta. Este año Febrero no tiene más de 29 días", "¡Alerta!");
                result = false;
            }
        }
    }
    else {
        jAlert("Fecha incorrecta. Formato permitido (en números): día/mes/año", "¡Alerta!");
        result = false;
    }

    return result;
}
//});
//Juan Muga - creacion de funciones para datepickers
$(document).ready(function (e) {
    $.fn.setDefaultDatePicker = function () {
        //$("input.datepickerTelerik:text").kendoDatePicker({ culture: "es-PE" });
        var today = (new Date);
        //var dateparts = today.toLocaleDateString().split('/');
        //$(this).kendoDatePicker({
        //    culture: "es-PE"
        //});
        var dateparts = today.toLocaleDateString().split('/');
        $(this).kendoDatePicker({
            culture: "es-PE",
            max: new Date(dateparts[2], dateparts[1] - 1, dateparts[0])
        });
        $(this).attr("readonly", "readonly");
    };

    //$(".datepickerMaxValue").setDatePickerWithMaxValue();
    $.fn.setDatePickerWithMaxValue = function () {
        var today = (new Date);
        var dateparts = today.toLocaleDateString().split('/');
        $(this).kendoDatePicker({
            culture: "es-PE",
            max: new Date(dateparts[2], dateparts[1] - 1, dateparts[0])
        });
        $(this).attr("readonly", "readonly");
    };

    $("#ExamenFiltro").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Home/GetExamenUsuario",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ 'prefix': '" + request.term + "'}",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.nombre, value: item.idExamen };
                    }))
                }
            })
        },
        select: function (e, i) {
            e.preventDefault();
            $("#hdnExamen").val(i.item.value);
            $("#ExamenFiltro").val(i.item.label);
        },
        minLength: 3
    });

    if (document.getElementById("nroOficio") != null) {
        document.getElementById("nroOficio").addEventListener("keyup", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                document.getElementById("btnBuscar").click();
            }
        });
    }
    if (document.getElementById("nroDocumento") != null) {
        document.getElementById("nroDocumento").addEventListener("keyup", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                document.getElementById("btnBuscar").click();
            }
        });
    }
    if (document.getElementById("codigoOrden") != null) {
        document.getElementById("codigoOrden").addEventListener("keyup", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                document.getElementById("btnBuscar").click();
            }
        });
    }
    if (document.getElementById("ExamenFiltro") != null) {
        document.getElementById("ExamenFiltro").addEventListener("keyup", function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();
                document.getElementById("btnBuscar").click();
            }
        });
    }

});

//// on window resize run function
//$(window).resize(function () {
//    fluidDialog();
//});

//// catch dialog if opened within a viewport smaller than the dialog width
//$(document).on("dialogopen", ".ui-dialog", function (event, ui) {
//    fluidDialog();
//});

//function fluidDialog() {
//    var $visible = $(".ui-dialog:visible");
//    // each open dialog
//    $visible.each(function () {
//        var $this = $(this);
//        console.log("$this", $this);
//        var dialog = $this.find(".ui-dialog-content").data("ui-dialog");
//        // if fluid option == true
//        if (dialog.options.fluid) {
//            var wWidth = $(window).width();
//            // check window width against dialog width
//            if (wWidth < (parseInt(dialog.options.maxWidth) + 50)) {
//                // keep dialog from filling entire screen
//                $this.css("max-width", "90%");
//            } else {
//                // fix maxWidth bug
//                $this.css("max-width", dialog.options.maxWidth + "px");
//            }
//            //reposition dialog
//            dialog.option("position", dialog.options.position);
//        }
//    });

//}