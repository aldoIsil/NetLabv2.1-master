$(document).ready(function () {
    $("#selEnfermedad").chosen({ placeholder_text_single: "Seleccione una Enfermedad", no_results_text: "No existen coincidencias" });
    $(".datepickerMaxValue").setDatePickerWithMaxValue();

    $("#divTipoBusquedaEstablecimiento").hide();
    $("#divTipoBusquedaJurisdiccion").hide();
    $('input[type=radio][name=tipobusqueda]').change(function () {
        CambiarTipoBusqueda(this.value);
    });

    $("#selInstituciones").chosen({ placeholder_text_single: "Seleccione una Institución", no_results_text: "No existen coincidencias" });

    $("#selEstablecimientos").chosen({ placeholder_text_single: "Seleccione un Establecimiento", no_results_text: "No existen coincidencias" });

    $("#idExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
    
    $("#seldisa").chosen({ placeholder_text_single: "Seleccione un elemento", no_results_text: "No existen coincidencias" });
    $("#selRed").chosen({ placeholder_text_single: "Seleccione una Red", no_results_text: "No existen coincidencias" });
    $("#selmicrored").chosen({ placeholder_text_single: "Seleccione una Micro Red", no_results_text: "No existen coincidencias" });


    EstablecimientoChange();
    InstitucionesChange();

    $("#selEnfermedad").on("change", function () {
        var idEnfermedad = $("#selEnfermedad").val();
        $.ajax(
           {
               url: URL_BASE + "Plantilla/GetExamenes?idEnfermedad=" + idEnfermedad,
               cache: false,
               method: "GET"
           }).done(function (msg) {
               $("#dvPopupExamen").html(msg);
               $("#idExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
           }
           );
    });

   
});
//
//function Submit() {
//    $(".validation").hide();
//    var returnvalue = true;
//    if ($("#fechaDesde") == undefined || $("#fechaDesde").val() == "") {
//        $("#errormsgfechadesde").text("fecha no puede ser vacía");
//        returnvalue = false;
//    }
//    else {
//        //try {
//        //    $.datepicker.parseDate("dd/mm/yyyy", $("#fechaDesde").val());
//        //}
//        //catch (err) {
//        //    $("#errormsgfechadesde").text("fecha ingresada no es correcta");
//        //    returnvalue = false;
//        //}
//    }

//    if ($("#fechaHasta") == undefined || $("#fechaHasta").val() == "") {
//        $("#errormsgfechahasta").text("fecha no puede ser vacía");
//        returnvalue = false;
//    }
//    else {
//        //try {
//        //    $.datepicker.parseDate("dd/mm/yyyy", $("#fechaHasta").val());
//        //}
//        //catch (err) {
//        //    $("#errormsgfechafecha").text("fecha ingresada no es correcta");
//        //    returnvalue = false;
//        //}
//    }

//    if (!returnvalue) {
//        $(".validation").show();
//        e.preventDefault();
//    }

//    return returnvalue;
//}
function BuscaDatos(TipoReporte,indicador) {
    $(".validation").hide();
    var returnvalue = true;
    if ($("#fechaDesde") == undefined || $("#fechaDesde").val() == "") {
        $("#errormsgfechadesde").text("fecha no puede ser vacía");
        returnvalue = false;
    }
    else {
        //try {
        //    $.datepicker.parseDate("dd/mm/yyyy", $("#fechaDesde").val());
        //}
        //catch (err) {
        //    $("#errormsgfechadesde").text("fecha ingresada no es correcta");
        //    returnvalue = false;
        //}
    }

    if ($("#fechaHasta") == undefined || $("#fechaHasta").val() == "") {
        $("#errormsgfechahasta").text("fecha no puede ser vacía");
        returnvalue = false;
    }
    else {
        //try {
        //    $.datepicker.parseDate("dd/mm/yyyy", $("#fechaHasta").val());
        //}
        //catch (err) {
        //    $("#errormsgfechafecha").text("fecha ingresada no es correcta");
        //    returnvalue = false;
        //}
    }

    if (!returnvalue) {
        $(".validation").show();
        e.preventDefault();
    }
    debugger;
    var selectedElements = new Object();
    selectedElements.FechaInicio = $('#fechaDesde').val();
    selectedElements.FechaFin = $('#fechaHasta').val();
    selectedElements.NombreFiltro = $("#nombrefiltro").val();
    switch (selectedElements.NombreFiltro) {
        case "institucion":
            selectedElements.IdJurisdiccion = $('#hdnInstitucion').val();
            break;
        case "disa":
            selectedElements.IdJurisdiccion = $('#hdnDisa').val();
            break;
        case "red":
            selectedElements.IdJurisdiccion = $('#hdnRed').val();
            break;
        case "microred":
            selectedElements.IdJurisdiccion = $('#hdnMicroRed').val();
            break;
        case "establecimiento":
            selectedElements.IdJurisdiccion = $('#hdnEstablecimiento').val();
            break;
    }
    selectedElements.idEnfermedad = $("#selEnfermedad").val();
    selectedElements.IdExamen = $('#idExamen').val();
    selectedElements.IdUsuario = 0;
    selectedElements.TipoReporte = TipoReporte;
    
    if (TipoReporte == 1 && indicador == 13) {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "Reporte/GetCantidadResultadosConsultadosPorSolicitante",
            data: JSON.stringify(selectedElements),
            contentType: "application/json; charset=utf-8",
            success: function (htmlToCharge) {
                $("#chartdivid").html(htmlToCharge);
            }
        });
    } else if (TipoReporte == 2 && indicador == 13) {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "Reporte/GetDetalleResultadosConsultadosPorSolicitante",
            data: JSON.stringify(selectedElements),
            contentType: "application/json; charset=utf-8",
            success: function (htmlToCharge) {
                $("#chartdivid2").html(htmlToCharge);
            }
        });
    } else if (TipoReporte == 1 && indicador == 12) {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "Reporte/GetCantidadResultadosConsultados",
            data: JSON.stringify(selectedElements),
            contentType: "application/json; charset=utf-8",
            success: function (htmlToCharge) {
                $("#chartdivid").html(htmlToCharge);
            }
        });
    } else {
        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "Reporte/GeDetalleResultadosConsultados",
            data: JSON.stringify(selectedElements),
            contentType: "application/json; charset=utf-8",
            success: function (htmlToCharge) {
                $("#chartdivid2").html(htmlToCharge);
            }
        });
    }
}



//
//funciones change
function InstitucionesChange() {
    debugger;
    $('#selInstituciones').change(function () {
        debugger;
        var selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);
        if (selected != "0") {
            GetDisas(selected);
        }
    });
}

function DisaChange() {
    $('#seldisa').change(function () {
        var selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);
        if (selected != "0") {
            GetRedes(selected);
        }
    });
}

function RedChange() {
    $('#selRed').change(function () {
        var selected = $('#selRed').val();
        $('#hdnRed').val(selected);
        if (selected != "0") {
            GetMicroRedes(selected);
        }
    });
}

function MicroRedChange() {
    $('#selmicrored').change(function (e) {
        e.preventDefault;
        var selected = $('#selmicrored').val();
        $('#hdnMicroRed').val(selected);
        //if (selected != "0") {
        //    GetEstablecimientosPorMicroRed(selected)
        //}
    });
}

function EstablecimientoChange() {
    $('#selEstablecimientos').change(function (e) {
        e.preventDefault;
        $("#nombrefiltro").val("establecimiento");
        var selected = $('#selEstablecimientos').val();
        $('#hdnEstablecimiento').val(selected);
        //-------------------------------------------------------------------
        debugger;
        var Jurisdiccion = $("#selEstablecimientos option:selected").text()
        $("#nombreJurisdiccion").val(Jurisdiccion);
    });
}

//obtener listas
function GetEstablecimientosAll() {
    $.ajax({
        url: '/Home/GetEstablecimientoAllList',
        success: function (result) {
            //debugger;
            $("#divddlestablecimiento").html(result);
            //console.log($("#selEstablecimientos"));
            
            EstablecimientoChange();
        },
        async: false
    });
}

function GetInstituciones() {
    $.ajax({
        url: '/Home/GetInstitucionList',
        success: function (result) {
            $("#divddlinstituciones").html(result);
            InstitucionesChange();
            //$("#selInstituciones").chosen({ placeholder_text_single: "Seleccione una Institución", no_results_text: "No existen coincidencias" });
        },
        async: false
    });
}

function GetDisas(idInstitucion) {
    $.ajax({
        url: '/Home/GetDisaList',
        data: { idInstitucion: parseInt(idInstitucion) },
        success: function (result) {
            //$("#selEstablecimientos option:first-child").attr("selected", true);
            $("#divddldisa").html(result);
            DisaChange();
            if ($("#hdndisacount").val() == "1") {
                //$("#seldisa").val($("#hdnfirstdisa").val());
                //$("#seldisa").change();
                $("#seldisa").val($("#hdnfirstdisa").val()).trigger("change");
                //$("#seldisa").trigger("change");
            }
            $("#seldisa").chosen({ placeholder_text_single: "Seleccione un elemento", no_results_text: "No existen coincidencias" });
            $("#nombrefiltro").val("institucion");

            //------------------------------------------------------------------
            debugger;
            var Jurisdiccion = $("#selInstituciones option:selected").text()
            $("#nombreJurisdiccion").val(Jurisdiccion);
        },
        async: false
    });
}

function GetRedes(idDisa) {
    $.ajax({
        url: '/Home/GetRedList',
        data: { idDisa: idDisa, idInstitucion: $('#selInstituciones').val() },
        success: function (result) {
            $("#divddlred").html(result);
            RedChange();
            if ($("#hdnredcount").val() == "1") {
                //$("#selRed").val($("#hdnfirstred").val());
                //$("#selRed").change();
                $("#selRed").val($("#hdnfirstred").val()).trigger("change");
                //$("#selRed").trigger("change");
            }
            $("#selRed").chosen({ placeholder_text_single: "Seleccione una Red", no_results_text: "No existen coincidencias" });
            $("#nombrefiltro").val("disa");

            //------------------------------------------------------------------
            debugger;
            var Jurisdiccion = $("#seldisa option:selected").text()
            $("#nombreJurisdiccion").val(Jurisdiccion);
        },
        async: false
    });
}

function GetMicroRedes(idRed) {
    $.ajax({
        url: '/Home/GetMicroRedList',
        data: { idRed: idRed, idDisa: $('#seldisa').val(), idInstitucion: $('#selInstituciones').val() },
        success: function (result) {
            $("#divddlmicrored").html(result);
            MicroRedChange();
            if ($("#hdnmicroredcount").val() == "1") {
                //$("#selmicrored").val($("#hdnfirstmicrored").val());
                //$("#selmicrored").change();
                $("#selmicrored").val($("#hdnfirstmicrored").val()).trigger("change");
                //$("#selmicrored").trigger("change");
            }
            $("#selmicrored").chosen({ placeholder_text_single: "Seleccione una Micro Red", no_results_text: "No existen coincidencias" });
            $("#nombrefiltro").val("red");

            //------------------------------------------------------------------
            debugger;
            var Jurisdiccion = $("#selRed option:selected").text()
            $("#nombreJurisdiccion").val(Jurisdiccion);
        },
        async: false
    });
}

function GetEstablecimientosPorMicroRed(idMicroRed) {
    $.ajax({
        url: '/Home/GetEstablecimientoList',
        data: { idMicroRed: idMicroRed, idRed: $('#selRed').val(), idDisa: $('#seldisa').val(), idInstitucion: $('#selInstituciones').val() },
        success: function (result) {
            $("#divddlestablecimiento").html(result);
            EstablecimientoChange();
            if ($("#hdneesscount").val() == "1") {
                $("#selEstablecimiento").val($("#hdnfirstmicrored").val());
                $("#selEstablecimiento").change();
                $("#selEstablecimiento").trigger("change");
            }
            $("#selEstablecimiento").chosen({ placeholder_text_single: "Seleccione un Establecimiento", no_results_text: "No existen coincidencias" });
            $("#nombrefiltro").val("microred");


            //------------------------------------------------------------------
            debugger;
            var Jurisdiccion = $("#selmicrored option:selected").text()
            $("#nombreJurisdiccion").val(Jurisdiccion);
        },
        async: false
    });
}

//
function CambiarTipoBusqueda(tiposelected) {
    if (tiposelected == '1') { //Por jurisdiccion -> limpiar filtro establecimiento
        
        GetEstablecimientosAll();
        GetInstituciones();
        $("#selInstituciones").chosen({ placeholder_text_single: "Seleccione una Institución", no_results_text: "No existen coincidencias", width: "100%" });
        $("#divTipoBusquedaEstablecimiento").hide();
        $("#divTipoBusquedaJurisdiccion").show();
    }
    else if (tiposelected == '2') { //Por establecimiento -> limpiar filtros disa/red/microred/...
        
        GetEstablecimientosAll();
        GetInstituciones();
        $("#selEstablecimientos").chosen({ placeholder_text_single: "Seleccione un Establecimiento", no_results_text: "No existen coincidencias", width:"100%" });
        $("#divddldisa").html("<select id='seldisa' name='seldisa' class='form-control'></select>");
        $("#seldisa").chosen({ placeholder_text_single: "Seleccione un elemento", no_results_text: "No existen coincidencias" });
        $("#divddlred").html("<select id='selRed' name='selRed' class='form-control'></select>");
        $("#selRed").chosen({ placeholder_text_single: "Seleccione una Red", no_results_text: "No existen coincidencias" });
        $("#divddlmicrored").html("<select id='selmicrored' name='selmicrored' class='form-control'></select>");
        $("#selmicrored").chosen({ placeholder_text_single: "Seleccione una Micro Red", no_results_text: "No existen coincidencias" });

        $("#divTipoBusquedaJurisdiccion").hide();
        $("#divTipoBusquedaEstablecimiento").show();
    }
}

//$("#btnBuscar").click(function (e) {
//    if ($("#ActualDepartamento").val() == "") {
//        jAlert("Debe seleccionar Departamento", "Alerta");
//        return false;
//    }
    
//});