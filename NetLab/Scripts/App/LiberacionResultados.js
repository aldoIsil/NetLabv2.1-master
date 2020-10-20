var myDate = new Date();

var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
var anio = myDate.getFullYear();

//var closeToChargeResultados = false;
//var htmlToCharge = "";
//var activeEdit;

var closeEditToCharge = false;
var idOrdenSel = "";

function agregarEventosValidarAnalitos() {
    // add multiple select / deselect functionality
    $("#selectall").click(function () {
        $('.case').attr('checked', this.checked);
    });

    // if all checkbox are selected, check the selectall checkbox
    // and viceversa
    $(".case").click(function () {

        if ($(".case").length == $(".case:checked").length) {
            $("#selectall").attr("checked", "checked");
        } else {
            $("#selectall").removeAttr("checked");
        }

    });

}


$(document).ready(function () {

    $.ajaxSetup({ cache: false });

    // This will make every element with the class "date-picker" into a DatePicker element
    if ($("#datepickerDesde").val() == "") {
        var modDateYesterday = new Date(myDate - 86400000);
        var diaY = (modDateYesterday.getDate() >= 10) ? modDateYesterday.getDate() : "0" + modDateYesterday.getDate();
        var mesY = ((modDateYesterday.getMonth() + 1) >= 10) ? (modDateYesterday.getMonth() + 1) : "0" + (modDateYesterday.getMonth() + 1);
        var anioY = modDateYesterday.getFullYear();
        $("#datepickerDesde").val(diaY + "/" + mesY + "/" + anioY);

        $("#datepickerHasta").val(dia + "/" + mes + "/" + anio);
    }

    $("#datepickerDesde").kendoDatePicker({
        culture: "es-PE"
    });

    $("#datepickerHasta").kendoDatePicker({
        culture: "es-PE"
    });


    $('body').on('submit', '#formLiberaResultados', function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr('action'),
            data: $(this).serialize(),
            success: function (data) {
                //jAlert("Los resultados han sido liberados", "Liberación")
                jAlert('Los resultados han sido liberados.', 'Liberación', function () {
                    iBuscarClick();
                });
                //window.location.reload();
                //$("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
            }
        });
    });


    $(".editDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        //idOrdenSel = $(this).attr('idOrden');
        $("#dialog-edit").dialog({
            title: "Liberar Resultados",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 1200,
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
                    //agregarEventosValidarAnalitos();
                });
            },
            close: function () {
                closeEditForm();
            }
        });


        //$("#dialog-edit").on('dialogclose', function () { alert(4); });
        $("#dialog-edit").dialog("open");
        return false;
    });

    function closeEditForm() {

        if (closeEditToCharge) {
            closeEditToCharge = false;
            $("#dialog-edit").dialog({
                title: "Resultados Validados",
                autoOpen: false,
                resizable: false,
                height: 'auto',
                width: 1200,
                position: {
                    my: "center top",
                    at: ("center top++" + (window.innerHeight * .1)),
                    collision: "none"
                },
                show: { effect: "drop", direction: "up" },
                modal: true,
                draggable: true,
                open: function () {

                    $(this).html(htmlToCharge);

                },
                close: function () {
                    $(this).dialog("close");
                }
            });
            $("#dialog-edit").dialog("open");
        }
    };


    $("#dialog-edit").on("click", "#CerrarPopUp", function (e) {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    });

    $('#tipoOrden').change(function (e) {
        iBuscarClick();
    });

    $(':radio[name=esFechaRegistro]').change(function () {
        iBuscarClick();
    });

});

function iBuscarClick() {
    document.getElementById("btnBuscar").click();
}
