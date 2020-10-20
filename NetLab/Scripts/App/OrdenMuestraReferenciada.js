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

var myDate = new Date();

var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
var anio = myDate.getFullYear();

var closeEditToCharge = false;
var htmlToCharge = "";
var activeEdit;

function agregarEventosEditarReferencia()
{
    $(".idLaboratorioDestinoR").each(function () {
        $(this).ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "OrdenMuestra/GetAllLaboratorios"
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, { placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias" }
       );
    });


    $("#tblDatosRecepcionEdicion").on("change", ".chkReferenciar", function () {
        
        if ($(this).is(':checked')) {
            $(this).closest("tr").find('input[type="text"]').removeAttr('disabled');
            $(this).closest("tr").find('select').attr('disabled', false).trigger("chosen:updated");        
        }
        else {
            $(this).closest("tr").find('input[type="text"]').attr('disabled', 'disabled');
            $(this).closest("tr").find('select').attr('disabled', true).trigger("chosen:updated");
        }
    });

    $("#btnCancelar").on("click", function ()
    {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    });

    //accordion de examenes etc QUE AUN NO SE AGREGA
    $("#accordionRecepcionado").accordion
     ({
         collapsible: true,
         heightStyle: "content",
         alwaysOpen: false,
         active: 3
     });

    $('#accordionRecepcionado').accordion("refresh");
    //accordion de examenes etc QUE AUN NO SE AGREGA
}



$(document).ready(function ()
{
    $.ajaxSetup({ cache: false });

    $(".datepicker").kendoDatePicker({
        culture: "es-PE"
    });

    $(".datepickerDesde").kendoDatePicker({
        culture: "es-PE"
    });

    $(".datepickerHasta").kendoDatePicker({
        culture: "es-PE"
    });               
    
   
    $(".referenciaDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");

        $("#dialog-edit").dialog({
            title: "Editar Referencia",
            autoOpen: false,
            resizable: false,
            height: 580,
            width: 950,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregarEventosEditarReferencia();
                });
            },
            close: function () {
                //closeEditForm();
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });

    $(':radio[name=esFechaRegistro]').change(function () {
        iBuscarClick();
    });

});

function iBuscarClick() {
    document.getElementById("btnBuscarOrdenes").click();
}


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
    var textbox = $.trim($('#' + controlId).val());
    var newTextValue = textbox + newvalue;
    var lengthToCompare = 20;

    if (textbox.length === lengthToCompare || newTextValue.length > lengthToCompare)
        return false;

    return true;
}