
function asignarEventos()
{
    $("#cmbEnfermedad").ajaxChosen({        
        dataType: "json",
        type: "POST",
        minTermLength: 5,
        afterTypeDelay: 500,
        cache: true,
        url: $("#cmbEnfermedad").attr("data-url")
    }, {
        loadingImg: $("#cmbEnfermedad").attr("data-loading-image")
    }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" } );

    $("#cmbEnfermedad").chosen({ placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" });
    $("#cmbExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
    $("#cmbTipoMuestra").chosen({ placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias" });

    $("#cmbEnfermedad").on("change", function () {

        var idEnfermedad = $("#cmbEnfermedad").val();

        $.ajax(
           {
               url: $("#cmbEnfermedad").attr("data-url-examenes") + "?idEnfermedad=" + idEnfermedad,
               cache: false,
               method: "GET"
           }).done(function (msg) {
               $("#dvCmbExamen").html(msg);
               $("#cmbExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
           }
           )
    });


    $("#dvCmbExamen").on("change", "#cmbExamen", function () {
        var idExamen = "";
        $("#cmbExamen :selected").each(function (i, selected) {

            idExamen = idExamen + "idExamen=" + $(selected).val() + "&";
        });
        $.ajax(
        {
            url: URL_BASE + "BancoSangre/GetTiposMuestraByIdExamen?" + idExamen + "_=_",
            cache: false,
            method: "GET"
        }).done(function(msg) {
                $("#dvCmbTipoMuestra").html(msg);
                $("#cmbTipoMuestra").chosen({ placeholder_text_single: "Seleccione el Tipo de Muestra", no_results_text: "No existen coincidencias" });
            }
        );
    });

    
    $("#AgregarDatosExamen").on("click", function () {

        var idEnfermedad = $("#cmbEnfermedad").val();
        var idExamen = $("#cmbExamen").val();

        var datos = "?idEnfermedad=" + idEnfermedad + "&" + "idExamen=" + idExamen;

        $.ajax(
        {
            url: URL_BASE + "BancoSangre/ShowTableExamen" + datos,
            cache: false,
            method: "GET"
        }).done(function(msg) {
                $("#dvTblExamen").html(msg);
                $("#dialog-open").dialog("close");
            }
        );
    });

    $("#CerrarPopUp").on("click", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    });
}

$(document).ready(function () {
    
    $("#cmbEstablecimiento").chosen({ placeholder_text_single: "Seleccione el Establecimiento", no_results_text: "No existen coincidencias" });

    $.datepicker.regional["es"] = {
        closeText: "Cerrar",
        prevText: "<Ant",
        nextText: "Sig>",
        currentText: "Hoy",
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
        dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
        dayNamesShort: ["Dom", "Lun", "Mar", "Mié", "Juv", "Vie", "Sáb"],
        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sá"],
        weekHeader: "Sm",
        dateFormat: "dd/mm/yy",
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ""
    };
    $.datepicker.setDefaults($.datepicker.regional["es"]);

    //$('.datepicker').datepicker();
    var myDate = new Date();

    var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
    var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
    var anio = myDate.getFullYear();
    

    //Busqueda de Establecimiento por nombre, provincia, distrito
    
    $("#cmbEstablecimiento").ajaxChosen({
        dataType: "json",
        type: "POST",
        minTermLength: 5,
        afterTypeDelay: 500,
        cache: true,
        url: URL_BASE + "BancoSangre/GetEstablecimientos"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    });


    //Busqueda de Enfermedades por nombre
    $("#btnAgregarEnfermedad").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-open").dialog({
            title: "Agregar Enfermedad",
            autoOpen: false,
            resizable: false,
            height: 300,
            width: 400,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    asignarEventos();
                });
                
            }/*,
            close: function (event, ui) {
                $(this).dialog('close');
            }*/
        });
        $("#dialog-open").dialog("open");
        return false;
    });

    var paso = $("#paso").val();
    if (paso == "2") {
        $("#tblMuestras .datepicker").datepicker();
        $("#tblMuestras .timepicker").timeEntry({ show24Hours: true });
    }

    var enabledAddMuestra = true;
    $("#lnkAgregarMuestra").on("click", function (e) {
        e.preventDefault();
        
        if (enabledAddMuestra) {
            var examenes = $("#dvTblExamen table tr").length;
            if (examenes <= 1) {
                jAlert("Agregue exámenes a la orden.", "Alerta!");
            } else {
                enabledAddMuestra = false;
                $("#tblMuestras tbody").append($('#hideTRMuestra').html());
                setTimeout(function () {
                    $("#tblMuestras .datepicker:last").datepicker();
                    $("#tblMuestras .datepicker:last").val(dia + "/" + mes + "/" + anio);

                    myDate = new Date();
                    var hora = (myDate.getHours() >= 10) ? myDate.getHours() : "0" + myDate.getHours();
                    var minutos = (myDate.getMinutes() >= 10) ? myDate.getMinutes() : "0" + myDate.getMinutes();


                    $("#tblMuestras .timepicker:last").timeEntry({ show24Hours: true });
                    $("#tblMuestras .timepicker:last").val(hora + ":" + minutos);
                    
                    enabledAddMuestra = true;

                }, 300);
            }
        }
        return false;
    });

    $("#lnkGuardar").on("click", function (e) {
        e.preventDefault();
        var ok = true;

        var idEstablecimiento = $("#cmbEstablecimiento").val();

        if (paso == "1" && (idEstablecimiento == undefined || idEstablecimiento == "")) {
            idEstablecimiento = $("#cmbEstablecimientoFrecuente").val();
            if (idEstablecimiento == undefined || idEstablecimiento == "") {
                jAlert("Seleccione un establecimiento.", "Alerta!");
                ok = false;
            }
        }

        var examenes = $("#dvTblExamen table tr").length;
        if (ok && examenes <= 1) {
            jAlert("Agregue exámenes a la orden.", "Alerta!");
            ok = false;
        }

        var muestras = $("#dvTblMuestra table tr").length;
        if (ok && muestras <= 1) {
            jAlert("Agregue muestras a la orden.", "Alerta!");
            ok = false;
        }

        if (ok) {
            if (paso == "1") {
                jConfirm('¿Está seguro de guardar la orden con el establecimineto seleccionado?', 'Confirmar Registro', function (r) {
                    if (r) {
                        $("#frmOrden").submit();
                    }
                });
            }
            if (paso == "2") {
                $("#frmOrden").submit();
            }
        }

        return false;
    });
    
    $("#frmOrden").on("click", ".lnkEliminarMuestra", function (e) {
        e.preventDefault();
        if (enabledAddMuestra) {
            $(this).closest("tr").remove();
        }
        return false;
    });

    $("#frmOrden").on("click", ".lnkEliminarExamen", function (e) {
        e.preventDefault();
        if (enabledAddMuestra) {
            var idEnfermedad = $(this).closest("tr").attr("idEnfermedad");
            var idExamen = $(this).closest("tr").attr("idExamen");

            var datos = "?idEnfermedad=" + idEnfermedad + "&" + "idExamen=" + idExamen;

            $.ajax(
            {
                url: URL_BASE + "BancoSangre/AjaxEliminarExamen" + datos,
                cache: false,
                method: "GET"
            }).done(function (msg) {
                $("#dvTblExamen").html(msg);
            }
            );
        }
        return false;
    });

    
    //Agregar tipo de muestra 
    //$('.chosen-select-width-agregarExamen').on('change', function (evt, params) {
    $("#dvExamenesSeleccionados").on("change", ".cmbTipoMuestra", function () {

        var idExamen = $(this).parent().attr("class");
        var idTipoMuestra = "";
        $(".cmbTipoMuestra :selected").each(function (i, selected) {

            idTipoMuestra = idTipoMuestra + "idTipoMuestra=" + $(selected).val() + "&";
        });
        $.ajax(
        {
            url: URL_BASE + "BancoSangre/GetTiposMuestraByIdTipoMuestra?" + idTipoMuestra + "idExamen=" + idExamen,
            cache: false,
            method: "GET"
        }).done(function(msg) {
            }
        );
    });

    
    $("#dvExamen").on("click", "#btnAgregarExamen", function () {
        var idExamen = $("#cmbExamen").val();
        //alert("as")
        $.ajax(
        {
            url: URL_BASE + "BancoSangre/AgregarExamen?idExamen=" + idExamen,
            cache: false,
            method: "GET"
        }).done(function(msg) {
                $("#dvExamenes").html("");
                $("#dvExamenes").append(msg);
            }
        );
    });

    $("#dvExamenes").on("click", ".eliminarExamen", function () {
        //$(".eliminarExamen").on("click", function (e) {
        //alert("asas")
        var idExamen = $(this).attr("id");

        console.log(idExamen); // just to check it works

        $.ajax(
        {
            url: URL_BASE + "BancoSangre/EliminarExamen?idExamen=" + idExamen,
            cache: false,
            method: "GET"
        }).done(function(msg) {
                $("#dvExamenes").html("");
                $("#dvExamenes").append(msg);
            }
        );
    });




});


//Eliminar examen de la lista de examenes
/*$("#dvExamenesSeleccionados").on('click', '.eliminarExamen', function (e) {
    var idExamen = $(this).parent().attr('class');
    $.ajax(
        {
            url: '/BancoSangre/EliminarExamen?idExamen=' + idExamen,
            cache: false,
            method: "GET"
        }).done(function (msg) {
            $("#dvExamenesSeleccionados").html("");
            $("#dvExamenesSeleccionados").append(msg);
        }
        )
});
*/


//Agregar examen a la lista de examenes
/* $("#btnExamen").on("click", function () {
     var idExamen = $("#cmbExamen").val();
     $.ajax(
         {
             url: '/BancoSangre/AgregarExamen?idExamen=' + idExamen,
             cache: false,
             method: "GET"
         }).done(function (msg) {
             $("#dvExamenesSeleccionados").html("");
             $("#dvExamenesSeleccionados").append(msg);
             $('.cmbTipoMuestra').chosen();
         }
         )
 });
 */
