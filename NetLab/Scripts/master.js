$(document).ready(function () {
    function ValidarFecha(input) {
        var result = true;
        var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
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
});