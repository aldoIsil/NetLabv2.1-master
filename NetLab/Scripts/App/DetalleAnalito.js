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


    $('body').on('submit', '#formValidaResultados', function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr('action'),
            data: $(this).serialize(),
            success: function (data) {
                jAlert("Los resultados han sido validados", "Validación")
                //$("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
            }
        });
    });


    $(".editDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        //idOrdenSel = $(this).attr('idOrden');
        $("#dialog-edit").dialog({
            title: "Detalle del Examen",
            autoOpen: false,
            resizable: false,
            height: 500,
            width: 950,
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
                height: 550,
                width: 1050,
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

    //$("#dialog-edit").on("click", ".conformeMuestra", function (e) {
    //    if ($(this).is(':checked')) {
    //        $(this).closest("tr").find('.criterioRechazo').attr('disabled', true);
    //        $(this).closest("tr").find('.SumoSelect').hide();
    //    }
    //    else {
    //        $(this).closest("tr").find('.criterioRechazo').removeAttr('disabled');
    //        $(this).closest("tr").find('.SumoSelect').show();
    //    }
    //});


    $("#dialog-edit").on("click", ".irIngresarResultados", function (e) {
        e.preventDefault();
        var idOrden = $(this).attr('idOrden');
        var idArea = $(this).attr('idArea');

        $.ajax({
            type: "POST",
            cache: false,
            url: $(this).attr('href'),
            data: { id: idOrden, idArea: idArea },
            success: function (result) {
                htmlToCharge = result;
                closeToChargeResultados = true;
                $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
            }
        });

        return false;
    });


    $("#dialog-edit").on("click", "#CerrarPopUp", function (e) {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
    });

    $(".editDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        //idOrdenSel = $(this).attr('idOrden');
        $("#dialog-edit").dialog({
            title: "Detalle del Examen",
            autoOpen: false,
            resizable: false,
            height: 500,
            width: 950,
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
});
