function limpiarCombo(id) {
    $(id).prop("disabled", false);
    $(id).empty();
    var newOption = $('<option value="0"></option>');
    $(id).append(newOption);
    $(id + "_chosen").removeClass("chosen-disabled");
    $(id).val("0").trigger("liszt:updated");
    $(id).trigger("chosen:updated");
}

function onChangeInstitucion() {

    var idInstitucion = $("#CodigoInstitucion").val();

    if (idInstitucion != undefined) {
        limpiarCombo("#CodigoEstablecimiento");
        limpiarCombo("#CodigoRed");
        limpiarCombo("#CodigoMicroRed");
        limpiarCombo("#CodigoDisa");

        $.ajax({
            type: "POST",
            url: URL_BASE + "Usuario/GetDisa?idInstitucion=" + idInstitucion,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#CodigoDisa").html("");
                $.each(data,
                    function (index, item) {
                        if (index === "results") {
                            $.each(item,
                                function (index2, valueitem) {
                                    $("#CodigoDisa").append('<option value="' + valueitem.id + '">' + valueitem.text + "</option>");
                                });
                        }
                    });

                $("#CodigoDisa").trigger("chosen:updated");
                $("#CodigoDisa").chosen({ width: "95%" });
                $("#CodigoDisa").change(onChangeDisa);
            },
            error: function (data) {
                console.log(data.d);
            }
        });
    }
}

function onChangeDisa() {
    
    limpiarCombo("#CodigoEstablecimiento");
    limpiarCombo("#CodigoRed");
    limpiarCombo("#CodigoMicroRed");

    var idInstitucion = $("#CodigoInstitucion").val();
    var idDisa = $("#CodigoDisa").val();

    $.ajax({
        type: "POST",
        url: URL_BASE + "Usuario/GetRed?idInstitucion=" + idInstitucion + "&idDisa=" + idDisa,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#CodigoRed").html("");

            $.each(data,
                function (index, item) {
                    if (index === "results") {
                        $.each(item,
                            function (index2, valueitem) {
                                $("#CodigoRed").append('<option value="' + valueitem.id + '">' + valueitem.text + "</option>");
                            });
                    }
                });

            $("#CodigoRed").trigger("chosen:updated");
            $("#CodigoRed").chosen({ width: "95%" });
            $("#CodigoRed").change(onChangeRed);
        },
        error: function (data) {
            console.log(data.d);
        }
    });
}

function onChangeRed() {
    
    limpiarCombo("#CodigoEstablecimiento");
    limpiarCombo("#CodigoMicroRed");

    var idInstitucion = $("#CodigoInstitucion").val();
    var idDisa = $("#CodigoDisa").val();
    var idRed = $("#CodigoRed").val();

    $.ajax({
        type: "POST",
        url: URL_BASE + "Usuario/GetMicrored?idInstitucion=" + idInstitucion + "&idDisa=" + idDisa + "&idRed=" + idRed,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#CodigoMicroRed").html("");

            $.each(data,
                function (index, item) {
                    if (index === "results") {
                        $.each(item,
                            function (index2, valueitem) {
                                $("#CodigoMicroRed").append('<option value="' + valueitem.id + '">' + valueitem.text + "</option>");
                            });
                    }
                });

            $("#CodigoMicroRed").trigger("chosen:updated");
            $("#CodigoMicroRed").chosen({ width: "95%" });

            $("#CodigoMicroRed").change(function () {

                var idMicrored = $("#CodigoMicroRed").val();
                onChangeMicroRed(idInstitucion, idDisa, idRed, idMicrored);
            });
        },
        error: function (data) {
            console.log(data.d);
        }
    });
}

function onChangeMicroRed(idInstitucion, idDisa, idRed, idMicrored) {
    
    limpiarCombo("#CodigoEstablecimiento");

    $.ajax({
        type: "POST",
        url: URL_BASE + "Usuario/GetEstablecimientoByMicrored?idInstitucion=" + idInstitucion + "&idDisa=" + idDisa + "&idRed=" + idRed + "&idMicrored=" + idMicrored,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#CodigoEstablecimiento").html("");

            $.each(data,
                function (index, item) {
                    if (index === "results") {
                        $.each(item,
                            function (index2, valueitem) {
                                $("#CodigoEstablecimiento").append('<option value="' + valueitem.id + '">' + valueitem.text + "</option>");
                            });
                    }
                });

            $("#CodigoEstablecimiento").trigger("chosen:updated");
            $("#CodigoEstablecimiento").chosen({ width: "95%" });
        },
        error: function (data) {
            console.log(data.d);
        }
    });
}

function ActualizarCombosPorJerarquia() {

    $("#chkPorEstablecimiento").removeAttr("checked");

    $("#CodigoEstablecimiento").chosen("destroy");
    $("#CodigoEstablecimiento").chosen({ placeholder_text_single: "Seleccione un Establecimiento", no_results_text: "No existen coincidencias" });

    $("#CodigoInstitucion").removeAttr("disabled");
    $("#CodigoInstitucion").attr("disabled", false).trigger("chosen:updated");
    $("#CodigoInstitucion option:first").attr("selected", "selected");

    limpiarCombo("#CodigoEstablecimiento");
    limpiarCombo("#CodigoRed");
    limpiarCombo("#CodigoMicroRed");
    limpiarCombo("#CodigoDisa");

    $("#CodigoEstablecimiento").attr("disabled", "disabled");
    $("#CodigoEstablecimiento").attr("disabled", true).trigger("chosen:updated");

    $("#chkPorJerarquia").prop("checked", true);
}

function ActualizarCombosPorEstablecimiento() {

    $("#chkPorJerarquia").removeAttr("checked");

    $("#CodigoInstitucion option:first").attr("selected", "selected");
    limpiarCombo("#CodigoEstablecimiento");
    limpiarCombo("#CodigoRed");
    limpiarCombo("#CodigoMicroRed");
    limpiarCombo("#CodigoDisa");

    $("#CodigoInstitucion").attr("disabled", "disabled");
    $("#CodigoInstitucion").attr("disabled", true).trigger("chosen:updated");

    $("#CodigoDisa").attr("disabled", "disabled");
    $("#CodigoDisa").attr("disabled", true).trigger("chosen:updated");

    $("#CodigoRed").attr("disabled", "disabled");
    $("#CodigoRed").attr("disabled", true).trigger("chosen:updated");

    $("#CodigoMicroRed").attr("disabled", "disabled");
    $("#CodigoMicroRed").attr("disabled", true).trigger("chosen:updated");

    onChangeMicroRed(0, 0, 0, 0);

    $("#CodigoEstablecimiento").removeAttr("disabled");
    $("#CodigoEstablecimiento").attr("disabled", false).trigger("chosen:updated");

    $("#CodigoEstablecimiento").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 5,
        afterTypeDelay: 1000,
        cache: false,
        url: URL_BASE + "Orden/GetEstablecimientos?dato=1"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, { placeholder_text_single: "Seleccione un Establecimiento", no_results_text: "No existen coincidencias" });


    $("#chkPorEstablecimiento").prop("checked", true);
}

$(document).ready(function() {

    $("#CodigoInstitucion").chosen({ placeholder_text_single: "Seleccione un Sub Sector", no_results_text: "No existen coincidencias" });
    $("#CodigoDisa").chosen({ placeholder_text_single: "Seleccione una DISA/DIRESA/GERESA", no_results_text: "No existen coincidencias" });
    $("#CodigoRed").chosen({ placeholder_text_single: "Seleccione una Red", no_results_text: "No existen coincidencias" });
    $("#CodigoMicroRed").chosen({ placeholder_text_single: "Seleccione una Micro Red", no_results_text: "No existen coincidencias" });
    $("#CodigoEstablecimiento").chosen({ placeholder_text_single: "Seleccione un Establecimiento", no_results_text: "No existen coincidencias" });

    $("#chkPorJerarquia").on("change", function () {

        if ($(this).is(":checked")) {
            ActualizarCombosPorJerarquia();
        }
        else {
            ActualizarCombosPorEstablecimiento();
        }
    });

    $("#chkPorEstablecimiento").on("change", function () {

        if ($(this).is(":checked")) {
            ActualizarCombosPorEstablecimiento();
        }
        else {
            ActualizarCombosPorJerarquia();
        }
    });

    $("#btnAgregarInstitucion").on("click", function(e) {

        var codigoInstitucion = $("#CodigoInstitucion").val();

        if (codigoInstitucion === "0") {
            e.preventDefault();
            jAlert("Debe seleccionar una institución", "Aviso");
        }
    });

    $("#btnAgregarDisa").on("click", function (e) {

        var codigoDisa = $("#CodigoDisa").val();

        if (codigoDisa === "0") {
            e.preventDefault();
            jAlert("Debe seleccionar una DISA/DIRESA/GERESA", "Aviso");
        }
    });

    $("#btnAgregarRed").on("click", function (e) {

        var codigoRed = $("#CodigoRed").val();

        if (codigoRed === "0") {
            e.preventDefault();
            jAlert("Debe seleccionar una Red", "Aviso");
        }
    });

    $("#btnAgregarMicroRed").on("click", function (e) {

        var codigoMicroRed = $("#CodigoMicroRed").val();

        if (codigoMicroRed === "0") {
            e.preventDefault();
            jAlert("Debe seleccionar una MicroRed", "Aviso");
        }
    });

    $("#btnAgregarEstablecimiento").on("click", function (e) {

        var codigoEstablecimiento = $("#CodigoEstablecimiento").val();

        if (codigoEstablecimiento === "0") {
            e.preventDefault();
            jAlert("Debe seleccionar un establecimiento", "Aviso");
        }
    });

    $("#chkSeleccionar").change(function () {
        $("input[name='chkEliminar']").prop("checked", $(this).prop("checked"));
    });

    $("#btnEliminar").on("click", function (e) {

        var selectedElements = [];

        $.each($("input[name='chkEliminar']:checked"), function () {
            selectedElements.push($(this).val());
        });

        if (selectedElements.length === 0) {
            e.preventDefault();
            jAlert("Debe seleccionar al menos un establecimiento", "Aviso");
            return false;
        } else {
            return confirm("¿ Está seguro de eliminar los establecimientos seleccionados ?");
        }
    });

    $("#btnEliminarTodos").on("click", function (e) {
        return confirm("¿ Está seguro de eliminar todos los establecimientos del usuario ?");
    });
});