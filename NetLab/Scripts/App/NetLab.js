// Descripción: funcione para evitar la edicion.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function NoEdicion(event) {

    return false;
}

function FechaValidaBusqueda(fechaDesde, fechaHasta) {

    var desde = $("#" + fechaDesde).val();
    var hasta = $("#" + fechaHasta).val();

    if (desde.trim() == "" || hasta.trim() == "") {
        jAlert("Ingrese un rango de fecha válido");
        return false;
    }

    if (desde.length < 10 || hasta.length < 10) {
        jAlert("Ingrese un rango de fecha válido");
        return false;
    }

    var fechaDesdeEsCorrecto = ValidarFechaSinMax(desde);
    if (fechaDesdeEsCorrecto == false) {
        $("#" + fechaDesde).focus();
        return false;
    }

    var fechaHastaEsCorrecto = ValidarFechaSinMax(hasta);
    if (fechaHastaEsCorrecto == false) {
        $("#" + fechaHasta).focus();
        return false;
    }

    return true;
}

function dateControlConfig() {
    //Juan Muga - configuracion estará solo en setup.js
    $(".datepicker").setDefaultDatePicker();
    //$(".datepicker").kendoDatePicker({
    //    culture: "es-PE"
    //});


    $.validator.addMethod("date",
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }

        var ok = true;
        try {
            $.datepicker.parseDate("dd/mm/yyyy", value);
        }
        catch (err) {
            ok = false;
        }
        return ok;
    });
}

function establecimientoChosenConfig(ddlEstablecimiento) {

    var establecimientoSelector = "#" + ddlEstablecimiento;

    $(establecimientoSelector).ajaxChosen({
        dataType: "json",
        type: "GET",
        minTermLength: 5,
        afterTypeDelay: 500,
        cache: true,
        url: $(establecimientoSelector).attr("data-url")
    }, {
        loadingImg: $(establecimientoSelector).attr("data-loading-image")
    }, { placeholder_text_single: "Seleccione el Establecimiento", no_results_text: "No existen coincidencias" });
}

function selectChosenConfig(idControl, text) {

    var selector = "#" + idControl;

    $(selector).ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 3,
        afterTypeDelay: 300,
        cache: true,
        url: $(selector).attr("data-url")
    }, {
        loadingImg: $(selector).attr("data-loading-image")
    }, { placeholder_text_single: text, no_results_text: "No existen coincidencias" });
}

$(function () {
    $(".numericOnly").bind('keypress', function (e) {
        if (EsCombinacionEspecial(e))
            return true;

        return NewInputHasValidCharacters(e) && NewInputIsValid('');
    });
    $(".numericOnly").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);

        if (!NewInputHasValidPastedCharacters(newAdditionalValue) || !NewInputIsValid(newAdditionalValue)) {
            event.preventDefault();
            return false;
        }

        return true;
    });
});

$(function () {
    $(".simpleNumbersOnly").bind('keypress', function (e) {
        return ValidateNumbers(e.key);
    });
    $(".simpleNumbersOnly").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);
        if (!ValidateNumbers(newAdditionalValue)) {
            event.preventDefault();
            return false;
        }
        return true;
    });
});

function ValidateNumbers(value) {
    if (value.match(/^[0-9]+$/) == null)
        return false;

    return true;
}

$(function () {
    $(".lettersOnly").bind('keypress', function (e) {
        if (e.charCode === 32)
            return true;

        return OnlyLettersEvaluation(e.key);
    });
    $(".lettersOnly").bind('paste', function (event) {
        if (event.charCode === 32)
            return true;

        var newAdditionalValue = GetDataFromClipBoard(event);
        if (!OnlyLettersEvaluation(newAdditionalValue)) {
            event.preventDefault();
            return false;
        }
        return true;
    });
});

function OnlyLettersEvaluation(name) {
    var alphaExp = /^[áéíóúña-zÑÁÉÍÓÚA-Z]+$/;
    return name.match(alphaExp) != null;
}

//Juan Muga - crear funcion y llamarlo en el document ready
function SetDateOnly() {
    $(".dateOnly").bind('keypress', function (e) {
        var value = e.key;

        //if (value === "/")
        //    value = e.char;

        return DateValidation(value);
    });
    $(".dateOnly").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);
        if (!DateValidation(newAdditionalValue)) {
            event.preventDefault();
            return false;
        }
        return true;
    });
}

$(function () {
    SetDateOnly();
});


function DateValidation(value) {
    if (value === "/")
        return true;

    var regex = /^[0-9]+$/;
    if (value.match(regex) == null)
        return false;

    return true;
}

$(function () {
    $(".ValidateLength").bind('keypress', function (e) {
        return ValidateTextboxLength(e, '');
    });
    $(".ValidateLength").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);

        if (!ValidateTextboxLength(event, newAdditionalValue)) {
            event.preventDefault();
        }
    });
});

$(function () {
    $(".BloqueoFecha").bind('keypress', function (e) {
        return false;
    });
    $(".BloqueoFecha").bind('paste', function (event) {
        event.preventDefault();
    });
});

function BloqueoFechaOnKeyPress() {
    return false;
}

function BloqueoFechaOnPaste(event) {
    event.preventDefault();
    return false;
}

function NewPhoneNumberIsValid(evt, controlId, nuevoValor) {
    var telefono = $.trim($(controlId).val());
    var nuevoTelefono = telefono + nuevoValor;

    if (telefono.length === 9 || nuevoTelefono.length > 9) {
        return false;
    }

    return true;
}

function GetDataFromClipBoard(event) {
    try {
        return (event.originalEvent || event).clipboardData.getData('Text');
    } catch (e) {
        return event.originalEvent.view.clipboardData.getData('Text');
    }
}