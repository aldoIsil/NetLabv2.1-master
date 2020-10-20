var myDate = new Date();

var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
var anio = myDate.getFullYear();
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

function createNewHandler() {

    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Control de Seguimiento",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 900,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregaeventosPopUpSeguimiento(false);
                });
            },
            close: function () {
                $(this).dialog("close");

            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });
}

function editExistingHandler() {

    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Criterio de Rechazo existente",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 600,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregaeventosPopUpSeguimiento(false);
                });
            },
            close: function () {
                $(this).dialog("close");
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });
}

function confirmDeleteHandler() {

    $(document).on("click", ".confirmDialog", function () {

        var url = $(this).attr("href");
        $("#dialog-confirm").dialog({
            autoOpen: false,
            resizable: false,
            height: 170,
            width: 350,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            buttons: {
                "OK": function () {
                    $(this).dialog("close");
                    window.location = url;

                },
                "Cancelar": function () {
                    $(this).dialog("close");

                }
            }
        });
        $("#dialog-confirm").dialog("open");
        return false;
    });
}

function closeDialogHandler() {

    $(document).on("click", "#closeDialog", function (e) {
        e.preventDefault();

        $("#dialog-edit").dialog("close");

        return false;
    });
}

function agregaeventosPopUpSeguimiento(Busqueda) {
    debugger;
    if (Busqueda) {
        $("#idEnfermedad").chosen({ placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" });
        $("#idExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });

        $("#idEnfermedad").ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "Orden/GetEnfermedades"
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" }).change(function () {
            $("#idExamen").ajaxChosen({
                dataType: "json",
                type: "POST",
                minTermLength: 3,
                afterTypeDelay: 100,
                cache: false,
                url: URL_BASE + "Orden/GetExamenes?genero=3&idenfermedad=" + $("#idEnfermedad").val()
            }, {
                loadingImg: URL_BASE + "Content/images/loading.gif"
            }, { placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
        });
        $("#CodigoUnico").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Catalogo/GetEESS",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: "{ 'prefix': '" + request.term + "'}",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Nombre, value: item.IdEstablecimiento };
                        }))
                    }
                })
            },
            select: function (e, i) {
                e.preventDefault();
                $("#hddDato").val(i.item.value);
                $("#CodigoUnico").val(i.item.label);
            },
            minLength: 1
        });
    }
    else {
        $("#NidEnfermedad").chosen({ placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" });
        $("#NidExamen").chosen({ placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });

        $("#NidEnfermedad").ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "Orden/GetEnfermedades"
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, { placeholder_text_single: "Seleccione la Enfermedad", no_results_text: "No existen coincidencias" }).change(function () {
            $("#NidExamen").ajaxChosen({
                dataType: "json",
                type: "POST",
                minTermLength: 3,
                afterTypeDelay: 100,
                cache: false,
                url: URL_BASE + "Orden/GetExamenes?genero=3&idenfermedad=" + $("#NidEnfermedad").val()
            }, {
                loadingImg: URL_BASE + "Content/images/loading.gif"
            }, { placeholder_text_single: "Seleccione el Examen", no_results_text: "No existen coincidencias" });
        });
        $("#NCodigoUnico").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Catalogo/GetEESS",
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: "{ 'prefix': '" + request.term + "'}",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Nombre, value: item.IdEstablecimiento };
                        }))
                    }
                })
            },
            select: function (e, i) {
                e.preventDefault();
                $("#NhddDato").val(i.item.value);
                $("#NCodigoUnico").val(i.item.label);
            },
            minLength: 1
        });
    }
    
}



$(document).ready(function () {
    $.ajaxSetup({ cache: false });
    createNewHandler();

    editExistingHandler();

    confirmDeleteHandler();

    closeDialogHandler();
    if ($("#datepickerDesde").val() == "") {
        var modDateYesterday = new Date(myDate - 86400000);
        var diaY = (modDateYesterday.getDate() >= 10) ? modDateYesterday.getDate() : "0" + modDateYesterday.getDate();
        var mesY = ((modDateYesterday.getMonth() + 1) >= 10) ? (modDateYesterday.getMonth() + 1) : "0" + (modDateYesterday.getMonth() + 1);
        var anioY = modDateYesterday.getFullYear();
        $("#datepickerDesde").val(diaY + "/" + mesY + "/" + anioY);

        $("#datepickerHasta").val(dia + "/" + mes + "/" + anio);
    }
    $(".datepicker").kendoDatePicker({
        culture: "es-PE"
    });

    $(".datepickerDesde").kendoDatePicker({
        culture: "es-PE"
    });

    $(".datepickerHasta").kendoDatePicker({
        culture: "es-PE"
    });
   
    agregaeventosPopUpSeguimiento(true);
    //CRUD
    $("body").on("submit", "#frmNuevoSeguimiento", function (e) {
        var ok= true;
        var Enfermedad = $("#NidEnfermedad").val();
        var Examen = $("#NidExamen").val();
        var Establecimiento = $("#NhddDato").val();
        var ejecutacc = 0;
        var ejecutadx = 0;
        var cumplecc = 0;

        var material = 0;
        var personal = 0;

        if (Establecimiento == undefined || Establecimiento == "" || Establecimiento == 0) {
            jAlert("Ingrese un establecimiento.", "Alerta!");
            ok = false;
        }
        if (Enfermedad == undefined || Enfermedad == "" || Enfermedad == 0) {
            jAlert("Ingrese una enfermedad.", "Alerta!");
            ok = false;
        }
        if (Examen == undefined || Examen == "" || Examen == 0) {
            jAlert("Ingrese un examen.", "Alerta!");
            ok = false;
        }
        if (($('#chkejecutacc').is(":checked"))) {
            ejecutacc = 1;
        }
        if (($('#chkejecutadx').is(":checked"))) {
            ejecutadx = 1;
        }
        if (($('#chkcumplecc').is(":checked"))) {
            cumplecc = 1;
        }

        if (($('chkcuentamaterial').is(":checked"))) {
            material = 1;
        }
        if (($('chkpersonalcapacitado').is(":checked"))) {
            personal = 1;
        }

        if (ok)
        {
            var datos = "?idEstablecimiento=" + Establecimiento + "&" +
                "idEnfermedad=" + Enfermedad + "&" +
                "idExamen=" + Examen + "&" +
                "ejecutadx=" + ejecutadx + "&" +
                "ejecutacc=" + ejecutacc + "&" +
                "cumpliocc=" + cumplecc + "&" +
                "infraestructura=" + $("#selInfraestructura").val() + "&" +
                "equipo=" + $("#selEquipo").val() + "&" +
                "material=" + material + "&" +
                "personal=" + personal;

            dpUI.loading.start("Creando ...");
            $("#dialog-open").dialog("close");
            $.ajax({
                type: "POST",
                cache: false,
                url: URL_BASE + "CCSeguimiento/AddSeguimiento" + datos,
                data: $(this).serialize(),
                success: function (response) {
                    jAlert(response, "Aviso");
                }
            });
        }        
        e.preventDefault();
        return false;
    });
    $("body").on("submit", "#frmEditarSeguimiento", function (e) {
        //var nombre = $.trim($('#Criterio_Glosa').val());
        //var ok2=false;
        //var statesAvailable = @Html.Raw(@ViewBag.nombresLista);
        //var nom=nombre.toUpperCase();

        //var someDataFromPartialSomehow = $("#someDataFromPartialSomehow").val();
        //var nombreController=someDataFromPartialSomehow.toUpperCase();

        //if (nom==nombreController) {
        //    ok2=true;
        //}else {

        //    if (statesAvailable.includes(nom)  ) {
        //        jAlert("Ya existe un criterio de rechazo con ese nombre.", "¡Alerta!");
        //    }else {
        //        ok2=true;
        //    }

        //}

        //if (ok2 )
        //{
        //    $.ajax({
        //        type: "POST",
        //        cache: false,
        //        url: $(this).attr("action"),
        //        data: $(this).serialize(),
        //        success: function (response) {

        //            var $resp = $(response);

        //            $("#btnGuardar").addClass("hidden");
        //            $("#btnCerrar").removeClass("hidden");
        //            $("#btnCerrar").on("click", function () {
        //                location.reload();
        //                $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
        //            });

        //            jAlert("Criterio de rechazo Asignado Satisfactoriamente", "Aviso");
        //        }
        //    });

        //}

        //e.preventDefault();
        return false;
    });
});
function EsCombinacionEspecial(event) {
    return event.charCode === 0 ||  //Esto es para firefox: delete,supr,arrow keys.
           (event.ctrlKey && event.key === "x") ||
           (event.ctrlKey && event.key === "c") ||
           (event.ctrlKey && event.key === "v");
}