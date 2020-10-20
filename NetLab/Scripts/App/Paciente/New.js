   function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }


function seguroSaludChangeHandler() {

    $("#tipoSeguroSalud").change(function () {

        var tipoSeguroSalud = $("#tipoSeguroSalud").val();

        if (tipoSeguroSalud == 9) {
            $("#otroSeguroSalud").prop("disabled", false);
        }
        else {
            $("#otroSeguroSalud").prop("disabled", true);
        }
    });
}

function TipoDocumentoChange()
{
    debugger;
    var tipo = $("#TipoDocumento").val();
    if ($("#TipoDocumento").val() == 1 || $("#TipoDocumento").val() == 2
              || $("#TipoDocumento").val() == 3) {
        $('#NroDocumento').attr('readonly', false);
        $("#NroDocumento").val("");
        $("#divSinDocumento").hide();
        $("#DivTutorsinDatos").hide();
        /*
        if($("#NroDocumento").val() == "00000000" )
        {
             $("#NroDocumento").val("");
        }*/

    }
    else if ($("#TipoDocumento").val() == 4) {
        $('#NroDocumento').attr('readonly', false);
        $("#NroDocumento").val("");

        $("#spanSinDocumento").text("El número de documento para un Menor sin identificar debe tener 11 dígitos, el primer dígito debe ser el tipo de documento del tutor, " +
            "los valores válidos son: (1=DNI, 2=Carnet de extranjería, 3=Pasaporte, 4=Documento de identidad extranjero), los siguientes 8 dígitos deben ser el número de documento de identidad del tutor, " +
            "los últimos 2 dígitos deben indicar el número de hijo.");
        $("#divSinDocumento").show();
        $("#DivTutorsinDatos").show();
        
    }
    else if ($("#TipoDocumento").val() == 7) {
        $("#spanSinDocumento").text("El número de documento para un Adulto sin identificar se genera automáticamente, con la fecha actual.");
        $("#divSinDocumento").show();
        $("#DivTutorsinDatos").hide();


        var d = new Date();

        var month = d.getMonth() + 1;
        var day = d.getDate();
        var hour = d.getHours();
        var minute = d.getMinutes();
        var second = d.getSeconds();
        // getFullYear() 
        var output = d.getFullYear().toString().substring(2, 4) + '' +
            (('' + month).length < 2 ? '0' : '') + month + '' +
            (('' + day).length < 2 ? '0' : '') + day + '' +
            (('' + hour).length < 2 ? '0' : '') + hour + '' +
            (('' + minute).length < 2 ? '0' : '') + minute + '' +
            (('' + second).length < 2 ? '0' : '') + second;


        $('#NroDocumento').attr('readonly', true);
        //    var nroDocumento = (new Date()).getTime();
        //   jAlert("Se agregará el siguiente código como Nro de Documento: " + nroDocumento +", por favor tomar nota.");
        $("#NroDocumento").val(output);
    } else if ($("#TipoDocumento").val() > 7) {
        $('#NroDocumento').attr('readonly', false);
        $("#NroDocumento").val("");
        $("#divSinDocumento").hide();
        $("#DivTutorsinDatos").hide()
    } else {
        $('#NroDocumento').attr('readonly', true);
        $("#divSinDocumento").hide();
        $("#DivTutorsinDatos").hide();
    }


}

function TipoDocumentoSetUp()
{
    $("#TipoDocumento").change(function () {
        TipoDocumentoChange();
    });

}

function SeguroSaludSetUp() {
    
    var tipoSeguroSalud = $("#tipoSeguroSalud").val();

    if (tipoSeguroSalud == 9) {
        $("#otroSeguroSalud").prop("disabled", false);
    }
    else {
        $("#otroSeguroSalud").prop("disabled", true);
    }

    seguroSaludChangeHandler();
}

function ObtenerDifAnios(nacimiento) {
    debugger;
    var parts = nacimiento.split('/');
    var today = new Date();

    var birthdayString = parts[2] + parts[1] + parts[0];
    var todayString = getTodayStringDate(today);

    if (birthdayString > todayString)
        return -1;

    var birthday = new Date(parts[2], parts[1]-1, parts[0]);
    var years = today.getFullYear() - birthday.getFullYear();

    return years;
}

function getTodayStringDate(today) {
    debugger;
    var year = today.getFullYear().toString();
    var month = (today.getMonth()+1).toString();
    var day = (new Date()).getDate();

    if (month < 10)
        month = "0" + month;

    if (day < 10)
        day = "0" + day;

    return year + month + day;
}

$(document).ready(function () {

    $("#btnGuardar").click(function () {

        var telefonoFijoValido = $.trim($("#TelefonoFijo").val());

        if (telefonoFijoValido.length !== 0 && telefonoFijoValido.length !== 9) {
            jAlert("El teléfono fijo es inválido. La longitud debe ser de 9 dígitos.");
            return false;
        }

        var celular1Valido = $.trim($("#Celular1").val());

        if (celular1Valido.length !== 0 && celular1Valido.length !== 9) {
            jAlert("El celular 1 es inválido. La longitud debe ser de 9 dígitos.");
            return false;
        }

        var celular2Valido = $.trim($("#Celular2").val());

        if (celular2Valido.length !== 0 && celular2Valido.length !== 9) {
            jAlert("El celular 2 es inválido. La longitud debe ser de 9 dígitos.");
            return false;
        }

        var dni = $("#NroDocumento").val();
        if (!ValidateOnlyNumbers(dni)) {
            jAlert("El número de documento no es válido.");
            return false;
        }
        debugger;
        var tipoDocumento = $("#TipoDocumento").val();
        var nacimiento = $("#FechaNacimiento").val();
        //Juan Muga - validar fecha de nacimiento

        if (tipoDocumento == 7 && nacimiento == "") {
            return true;
        } else {
            var validarNacimiento = ValidarFecha(nacimiento);

            if (validarNacimiento == false) {
                $("#FechaNacimiento").focus();
                return false;
            }

            var years = ObtenerDifAnios(nacimiento);

            if (years < 0 && tipoDocumento != 4) {
                jAlert("La fecha de nacimiento no es válida.");
                return false;
            }

            if (tipoDocumento == 4) {
                if (years > 17) {
                    jAlert("La fecha de nacimiento no es válida para un menor sin identificar.");
                    return false;
                }
            }

            if (tipoDocumento == 7) {
                if (years < 18) {
                    jAlert("La fecha de nacimiento no es válida para un adulto sin identificar.");
                    return false;
                }
            }
        }
        
        //Captura los datos del paciente para enviarlo mediante un ajax al controlador PacienteController
        var ApellidoPaterno = $("#ApellidoPaterno").val();
        var ApellidoMaterno = $("#ApellidoMaterno").val();

        //if (tipoDocumento == 1) {
        //    if (ApellidoMaterno == "" && $("#ApeMatHdd").val() == "") {
        //        jAlert("El apellido Materno es obligatorio.");
        //        return false;
        //    }
        //}
        if (ApellidoPaterno == "" && ApellidoMaterno == "") {
            ApellidoPaterno = $("#ApePatHdd").val();
            ApellidoMaterno = $("#ApeMatHdd").val();

            var datos = "?ApellidoPaterno=" + ApellidoPaterno + "&" + 
            "ApellidoMaterno=" + ApellidoMaterno

            $.ajax(
                       {
                           url: URL_BASE + "Paciente/GetApellidos" + datos,
                           cache: false,
                           method: "GET"
                       })
        }

        return true;
    });


    SeguroSaludSetUp();

    dateControlConfig();

    TipoDocumentoSetUp();

    //  TipoDocumentoChange();


     if ($("#TipoDocumento").val() == 1 || $("#TipoDocumento").val() == 2
              || $("#TipoDocumento").val() == 3) {
        $('#NroDocumento').attr('readonly', false);
        $("#divSinDocumento").hide();
               
    }
    else if ($("#TipoDocumento").val() == 4) {
        $('#NroDocumento').attr('readonly', false);
        $("#spanSinDocumento").text("El número de documento para un Menor sin identificar debe tener 11 dígitos, el primer dígito debe ser el tipo de documento del tutor, " +
            "los valores válidos son: (1=DNI, 2=Carnet de extranjería, 3=Pasaporte, 4=Documento de identidad extranjero), los siguientes 8 dígitos deben ser el número de documento de identidad del tutor, " +
            "los últimos 2 dígitos deben indicar el número de hijo.");
        $("#divSinDocumento").show();
        $("#DivTutorsinDatos").show();
    }
    else if ($("#TipoDocumento").val() == 7) {
        $("#spanSinDocumento").text("El número de documento para un Adulto sin identificar se genera automáticamente, con valor es la fecha actual.");
        $("#divSinDocumento").show();
        $('#NroDocumento').attr('readonly', true);
 
    }

     $('#chckDireccionActual').change(function () {
         
         if ($(this).is(":checked")) {
            $("#direccionActualFieldSet1").hide();
            $("#direccionActualFieldSet2").hide();
        }
        else {
            $("#direccionActualFieldSet1").show();
            $("#direccionActualFieldSet2").show();
        }
    });

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
            ok = true;
        }
        return ok;
    });

    $('#chkSinDatosTutor').change(function () {
        var d = new Date();

        var month = d.getMonth() + 1;
        var day = d.getDate();
        var hour = d.getHours();
        var minute = d.getMinutes();
        var second = d.getSeconds();
        // getFullYear() 
        var output = d.getFullYear().toString().substring(2, 4) + '' +
            (('' + month).length < 2 ? '0' : '') + month + '' +
            (('' + day).length < 2 ? '0' : '') + day + '' +
            (('' + hour).length < 2 ? '0' : '') + hour + '' +
            (('' + minute).length < 2 ? '0' : '') + minute + '' +
            (('' + second).length < 2 ? '0' : '') + second;


        if ($(this).is(":checked")) {
            $('#NroDocumento').attr('readonly', true);
            $("#NroDocumento").val(output);
        }
        else {
            $('#NroDocumento').attr('readonly', false);
            $("#NroDocumento").val('');
        }
    });

});

function ValidateOnlyNumbers(value) {
    //if (value.match(/^[0-9]+$/) == null)
        //return false;

    return true;
}

//Se creo para que estas acciones funcionen en firefox. IE y Chrome no tienen problemas.
function EsCombinacionEspecial(event) {
    return event.charCode === 0 ||  //Esto es para firefox: delete,supr,arrow keys.
           (event.ctrlKey && event.key === "x") ||
           (event.ctrlKey && event.key === "c") ||
           (event.ctrlKey && event.key === "v");
}

function ValidateTextboxLength(event, newvalue) {
    if (EsCombinacionEspecial(event))
        return true;

    var controlId = event.target.id;
    var textbox = $.trim($('#'+controlId).val());
    var newTextValue = textbox + newvalue;

    var lengthToCompare;

    switch (controlId) {
        case "ApellidoPaterno":        
            lengthToCompare = 20;
            break;
        case "Nombres":
            lengthToCompare = 30;
            break;
        case "otroSeguroSalud":
            lengthToCompare = 30;
            break;
        case "correoElectronico":
            lengthToCompare = 50;
            break;
        case "ocupacion":
            lengthToCompare = 40;
            break;
        case "DireccionActual":
            lengthToCompare = 50;
            break;
        case "FechaNacimiento":
            lengthToCompare = 10;
            break;
        default:
            lengthToCompare = 100;
    }

    if (textbox.length === lengthToCompare || newTextValue.length > lengthToCompare)
        return false;

    return true;
}

function ValidateTextBoxDefaultLength(evt, textboxID) {
    var textbox = $.trim($(textboxID).val());

    if (textbox.length == 35) {
        return false;
    }

    return true;
}

function ValidateTextBoxCustomtLength(evt, textboxID, txtLength) {
    var textbox = $.trim($(textboxID).val());

    if (textbox.length == txtLength) {
        return false;
    }

    return true;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    var tipoDocumento = $("#TipoDocumento").val();
    var nroDocumento = $.trim($("#NroDocumento").val());

    if (tipoDocumento == 1 && nroDocumento.length == 28) {
        return false;
    }

    if ((tipoDocumento == 4) && nroDocumento.length == 11) {
        return false;
    }

    if ((tipoDocumento == 2 || tipoDocumento == 3) && nroDocumento.length == 12) {
        return false;
    }
    return true;
}

$(function () {
    $(".telefonoFijoValido").bind('keypress', function (e) {
        if (EsCombinacionEspecial(e))
            return true;

        return isOnlyNumber(e) && NewPhoneNumberIsValid(e, '#TelefonoFijo', '');
    });
    $(".telefonoFijoValido").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);

        if (!NewInputHasValidPastedCharacters(newAdditionalValue) || !NewPhoneNumberIsValid(event, '#TelefonoFijo', newAdditionalValue)) {
            event.preventDefault();
        }
    });
});

$(function () {
    $(".celular1Valido").bind('keypress', function (e) {
        if (EsCombinacionEspecial(e))
            return true;

        return isOnlyNumber(e) && NewPhoneNumberIsValid(e, '#Celular1', '');
    });
    $(".celular1Valido").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);

        if (!NewInputHasValidPastedCharacters(newAdditionalValue) || !NewPhoneNumberIsValid(event, '#Celular1', newAdditionalValue)) {
            event.preventDefault();
        }
    });
});

$(function () {
    $(".celular2Valido").bind('keypress', function (e) {
        if (EsCombinacionEspecial(e))
            return true;

        return isOnlyNumber(e) && NewPhoneNumberIsValid(e, '#Celular2', '');
    });
    $(".celular2Valido").bind('paste', function (event) {
        var newAdditionalValue = GetDataFromClipBoard(event);

        if (!NewInputHasValidPastedCharacters(newAdditionalValue) || !NewPhoneNumberIsValid(event, '#Celular2', newAdditionalValue)) {
            event.preventDefault();
        }
    });
});

function isOnlyNumber(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

//Evalua la cadeana pegada de acuerdo a la logica del metodo. Solo para el 'paste' event.
function NewInputHasValidPastedCharacters(value) {
    if (value.match(/^[0-9]+$/) == null)
        return false;

    return true;
}

//Este metodo solo funciona con el keypress por lo que solo se necesita usar el event.
function NewInputHasValidCharacters(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

//Este metodo solo funciona al momento de hacer 'paste' por lo que no se usa el event.
function NewInputIsValid(value) {
    var tipoDocumento = $("#TipoDocumento").val();
    var nroDocumento = $.trim($("#NroDocumento").val());
    var sinTutor =  $.trim($("#chkSinDatosTutor").val());

    var newValue = nroDocumento + value;

    if (tipoDocumento == 1 && (nroDocumento.length == 8 || newValue.length > 8)) {
        return false;
    }

    if ((tipoDocumento == 4) && (nroDocumento.length == 11 || newValue.length > 11)) {
        return false;
    }
    if ((tipoDocumento == 2 || tipoDocumento == 3) && (nroDocumento.length == 12 || newValue.length > 12)) {
        return false;
    }
    //if (sinTutor.length == 0) {
        
    //}
    
    return true;
}
