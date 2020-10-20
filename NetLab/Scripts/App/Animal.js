function onChangeEspecie(urlRazaPorEspecie) {

    var codigoEspecie = $("#Especie_IdEspecie").val();
    $("#dvRaza").load(urlRazaPorEspecie + "?codigoEspecie=" + codigoEspecie);
}

function checkNoPrecisaFecha() {
    
    if ($("#noPrecisaFecha").is(":checked")) {

        $("#fechaNacimiento").val("");
        $("#fechaNacimiento").attr("disabled", "disabled");

    } else {
        $("#fechaNacimiento").removeAttr("disabled");
    }
}

function noPrecisaFechaHandler() {

    checkNoPrecisaFecha();

    $("#noPrecisaFecha").change(function () {

        checkNoPrecisaFecha();
    });
}

function checkNoPrecisaUbigeo() {

    if ($("#noPrecisaUbigeoAnimal").is(":checked")) {

        $("#IdDepartamento").attr("disabled", "disabled");
        $("#IdProvincia").attr("disabled", "disabled");
        $("#IdDistrito").attr("disabled", "disabled");
        $("#Animal_Direccion").val("");
        $("#Animal_Direccion").attr("disabled", "disabled");
    } else {

        $("#IdDepartamento").removeAttr("disabled");
        $("#IdProvincia").removeAttr("disabled");
        $("#IdDistrito").removeAttr("disabled");
        $("#Animal_Direccion").removeAttr("disabled");
    }
}

function noPrecisaUbigeoAnimalHandler() {

    checkNoPrecisaUbigeo();

    $("#noPrecisaUbigeoAnimal").change(function () {

        checkNoPrecisaUbigeo();
    });
}

function checkNoPrecisaUbigeoPropietario() {
    
    if ($("#noPrecisaPropietario").is(":checked")) {

        $("#TipoDocumento_IdTipoDocumento").attr("disabled", "disabled");
        $("#Animal_Propietario_NroDocumento").val("");
        $("#Animal_Propietario_NroDocumento").attr("disabled", "disabled");
        $("#Animal_Propietario_ApellidoPaterno").val("");
        $("#Animal_Propietario_ApellidoPaterno").attr("disabled", "disabled");
        $("#Animal_Propietario_ApellidoMaterno").val("");
        $("#Animal_Propietario_ApellidoMaterno").attr("disabled", "disabled");
        $("#Animal_Propietario_Nombres").val("");
        $("#Animal_Propietario_Nombres").attr("disabled", "disabled");
        $("#Genero_IdGenero").attr("disabled", "disabled");
        $("#Animal_Propietario_TelefonoFijo").val("");
        $("#Animal_Propietario_TelefonoFijo").attr("disabled", "disabled");
        $("#Animal_Propietario_Celular1").val("");
        $("#Animal_Propietario_Celular1").attr("disabled", "disabled");
        $("#DepartamentoProp").attr("disabled", "disabled");
        $("#ProvinciaProp").attr("disabled", "disabled");
        $("#DistritoProp").attr("disabled", "disabled");
        $("#Animal_Propietario_DireccionActual").val("");
        $("#Animal_Propietario_DireccionActual").attr("disabled", "disabled");
    } else {

        $("#TipoDocumento_IdTipoDocumento").removeAttr("disabled");
        $("#Animal_Propietario_NroDocumento").removeAttr("disabled");
        $("#Animal_Propietario_ApellidoPaterno").removeAttr("disabled");
        $("#Animal_Propietario_ApellidoMaterno").removeAttr("disabled");
        $("#Animal_Propietario_Nombres").removeAttr("disabled");
        $("#Genero_IdGenero").removeAttr("disabled");
        $("#Animal_Propietario_TelefonoFijo").removeAttr("disabled");
        $("#Animal_Propietario_Celular1").removeAttr("disabled");
        $("#DepartamentoProp").removeAttr("disabled");
        $("#ProvinciaProp").removeAttr("disabled");
        $("#DistritoProp").removeAttr("disabled");
        $("#Animal_Propietario_DireccionActual").removeAttr("disabled");

        checkTipoDocumento();
    }
}

function noPrecisaPropietarioHandler() {

    checkNoPrecisaUbigeoPropietario();

    $("#noPrecisaPropietario").change(function () {

        checkNoPrecisaUbigeoPropietario();
    });
}

function toggleRefugio() {

    $("#dvRefugio").hide();

    $("#Origen_IdOrigen").change(function () {

        var selected = $("#Origen_IdOrigen option:selected").text().toUpperCase();
        var refugio = "refugio".toUpperCase();

        if (selected === refugio) {
            $("#dvRefugio").show();
        } else {
            $("#Animal_Refugio").val("");
            $("#dvRefugio").hide();
        }
    });
}

function checkTipoDocumento() {

    var selected = $("#TipoDocumento_IdTipoDocumento option:selected").text().toUpperCase();

    var sinDocumento = "sin documento".toUpperCase();

    if (selected === sinDocumento) {
        $("#Animal_Propietario_NroDocumento").val("");
        $("#Animal_Propietario_NroDocumento").attr("disabled", "disabled");
    } else {
        $("#Animal_Propietario_NroDocumento").removeAttr("disabled");
    }

    var dni = "le / dni".toUpperCase();

    if (selected === dni) {
        $("#dvValidarDocumento").show();
    } else {
        $("#dvValidarDocumento").hide();
    }
}

function toggleTipoDocumento() {

    checkTipoDocumento();

    $("#TipoDocumento_IdTipoDocumento").change(function () {

        checkTipoDocumento();
    });
}

function validarDocumentoHandler() {

    $("#validarDocumento").click(function (event) {
        event.preventDefault();

        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: $(this).attr("data-url"),
            data: {
                nroDocumento: $("#Animal_Propietario_NroDocumento").val()
            },
            success: function (result) {

                if (result) {
                    $("#Animal_Propietario_ApellidoPaterno").val(result.ApellidoPaterno);
                    $("#Animal_Propietario_ApellidoMaterno").val(result.ApellidoMaterno);
                    $("#Animal_Propietario_Nombres").val(result.Nombres);
                    $("#Genero_IdGenero").val(result.Genero);
                    $("#Animal_Propietario_TelefonoFijo").val(result.TelefonoFijo);
                    $("#Animal_Propietario_Celular1").val(result.Celular1);

                    var idUbigeoActual = result.UbigeoActual.Id;

                    if (idUbigeoActual) {
                        $("#DepartamentoProp").val(idUbigeoActual.substr(0, 2)).change();
                        $("#ProvinciaProp").val(idUbigeoActual.substr(2, 2)).change();
                        $("#DistritoProp").val(idUbigeoActual.substr(4, 2));
                    }

                    $("#Animal_Propietario_DireccionActual").val(result.DireccionActual);
                }
            },
            error: function (errorData) {
            }
        });
    });
}

$(document).ready(function () {

    noPrecisaFechaHandler();

    noPrecisaUbigeoAnimalHandler();

    noPrecisaPropietarioHandler();

    toggleRefugio();

    toggleTipoDocumento();

    validarDocumentoHandler();

    dateControlConfig();
});

