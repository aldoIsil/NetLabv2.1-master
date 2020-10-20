// Descripción: Realiza la busqueda de ordenes cada vez que se selecciona un nuevo filtro en la pantalla de busqueda.
//              Realiza la impresion de los codigos de muestra.
// Author            : Terceros.
// Fecha Creación    : 01/01/2017.
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function imprimir() {
    $("#imprimirCodificacion").printArea();
    //$("#imprimirCodificacion").printPage();

    //$('#imprimirCodificacion').printPage();
    return false;

}
function printPartOfPage(elementId) {
    if ($.browser.msie || !!navigator.userAgent.match(/Trident.*rv[ :]?[1-9]{2}\./)) {
        var printContent = document.getElementById(elementId);
        var windowUrl = 'Job Receipt';
        var uniqueName = new Date();
        var windowName = 'Print' + uniqueName.getTime();
        var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');
        printWindow.document.write(printContent.innerHTML);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }
    else {
        $("#imprimirCodificacion").printArea();
        return false;
    }
}

$('body').on('click', '#btnImprimir', function (e) {
    printPartOfPage('imprimirCodificacion');
});
$('body').on('click', '#btnEditar', function (e) {
    e.preventDefault();   
    setTimeout(function () {
        $("#frmOrden").submit();
        dpUI.loading.start("Editando Orden ...");
    }, 3000);
});
$(document).ready(function () {
    $('#dtOrden').DataTable({
        //"scrollY": "100px",
        //"scrollX": "100%",
        "scrollCollapse": true,
    });
    $('.dataTables_length').addClass('bs-select');
    $('#estadoOrden').change(function (e) {
        iBuscarClick();
    });

    $('#tipoOrden').change(function (e) {
        iBuscarClick();
    });
    //Juan Muga - configuracion estará solo en setup.js
    //debugger;
    $(".datepickerTelerik").setDefaultDatePicker();
    //$(".datepickerTelerik").kendoDatePicker({
    //    culture: "es-PE"
    //});

    $(':radio[name=esFechaSolicitud]').change(function () {
        iBuscarClick();
    });

    $(".editDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href") + "?_=" + (new Date()).getTime();
        //console.log(url);
        $("#dialog-edit").dialog({
            title: "Ver Orden",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 1200,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
                collision: "none"
            },
            fluid: true,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    //agregarEventosMostrarReferencia();
                });
            },
            close: function () {
                //   closeEditForm();                
            }
        });

        $("#dialog-edit").dialog("open");
        return false;

    });

});

function iBuscarClick() {
    document.getElementById("iBuscar").click();
}

function ValidateTextboxLength(event, newvalue) {
    if (EsCombinacionEspecial(event))
        return true;

    var controlId = event.target.id;
    var textbox = $.trim($('#' + controlId).val());
    var newTextValue = textbox + newvalue;

    var lengthToCompare;

    switch (controlId) {
        case "nroOficio":
        case "docIdentidad":
            lengthToCompare = 50;
            break;
        default:
            lengthToCompare = 50;
    }

    if (textbox.length === lengthToCompare || newTextValue.length > lengthToCompare)
        return false;

    return true;
}

//Se creo para que estas acciones funcionen en firefox. IE y Chrome no tienen problemas.
function EsCombinacionEspecial(event) {
    return event.charCode === 0 ||  //Esto es para firefox: delete,supr,arrow keys.
           (event.ctrlKey && event.key === "x") ||
           (event.ctrlKey && event.key === "c") ||
           (event.ctrlKey && event.key === "v") ||
           (event.key === "Enter");
}
function VerOrden(idOrden, Origen, Controlador) {
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Orden/ShowEditRom",
        data: { idOrden: idOrden, Origen: Origen, Controlador: Controlador },
        success: function (result) {
            $('#DataVerOrden2').html(result);

            $(".datepickerMaxValue").setDatePickerWithMaxValue();
            LoadSolicitantesSearch();
            $(function () {
                $("#tabs").tabs();
                var hdnsolicitante = $("body #hdnsolicitante").val();
                var hdnsolicitanteid = $("body #hdnsolicitanteid").val();
                console.log("hdnsolicitante: ", hdnsolicitante);
                console.log("hdnsolicitanteid: ", hdnsolicitanteid);
                $('body #solicitante').append($('<option/>', {
                    value: hdnsolicitanteid,
                    text: hdnsolicitante
                }));
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

function LoadSolicitantesSearch() {
    $("#solicitante").ajaxChosen({
        dataType: "json",
        type: "GET",
        //minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Solicitante/GetSolicitantes"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, {
        placeholder_text_single: "Seleccione el Solicitante", no_results_text: "No existen coincidencias"
    }).change(function () {
        //alert($("#solicitante").val());
    });
}