function editExistingHandler() {

    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar EESS/Laboratorio existente",
            autoOpen: false,
            resizable: false,
            height: 560,
            width: 600,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);
                fileValidationHandler("logo", "divFileLogo");
                fileValidationHandler("sello", "divFileSello");
            },
            close: function () {
                $(this).dialog("close");
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });
}





function openExistingHandler() {

    $(document).on("click", ".openDialog", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear EESS/LAB",
            autoOpen: false,
            resizable: false,
            height: 560,
            width: 600,
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregarcombosPopup();
                });
                fileValidationHandler("logo", "divFileLogo");
                fileValidationHandler("sello", "divFileSello");
            },
            close: function () {
                $(this).dialog("close");
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });
}

function agregarcombosPopup() {

    $("#selEstablecimiento").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#seldisa").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#selInstituciones").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#selRed").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });
    $("#selmicrored").chosen({ placeholder_text_single: "----Todos----", no_results_text: "No existen coincidencias" });

    $('#btnEstablecimiento').click(function () {
        var selected = $('#selEstablecimiento').val();
        $('#hdnEstablecimiento').val(selected);

        selected = $('#selRed').val();
        $('#hdnRed').val(selected);

        selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        selected = $('#selmicrored').val();
        $('#hdnMicroRed').val(selected);

        $('#hdnTipo').val('Establecimientos');

        $('#frmInstituciones').submit();
    });

    $('#selInstituciones').change(function () {

        var selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        $('#hdnTipo').val('Instituciones');
        $('#frmInstituciones').submit();
    });

    $('#seldisa').change(function () {

        var selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        $('#hdnTipo').val('Disa');
        $('#frmInstituciones').submit();
    });

    $('#selRed').change(function () {

        var selected = $('#selRed').val();
        $('#hdnRed').val(selected);

        selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        $('#hdnTipo').val('Red');
        $('#frmInstituciones').submit();
    });

    $('#selmicrored').change(function () {

        var selected = $('#selRed').val();
        $('#hdnRed').val(selected);

        selected = $('#seldisa').val();
        $('#hdnDisa').val(selected);

        selected = $('#selInstituciones').val();
        $('#hdnInstitucion').val(selected);

        selected = $('#selmicrored').val();
        $('#hdnMicroRed').val(selected);

        $('#hdnTipo').val('MicroRed');
        $('#frmInstituciones').submit();
    });
}


function fileValidationHandler(fileId, classId) {
    
    $(document).on("change", "#"+ fileId, function () {
        validateFile(fileId, classId);
    });
}

function validateFile(fileId, classId) {
    var file = getNameFromPath($("#" + fileId).val());
    var flag;
    if (file != null) {
        var extension = file.substr((file.lastIndexOf(".") + 1));
        switch (extension) {
            case "png":
                flag = true;
                break;
            default:
                flag = false;
        }
    }
    if (flag === false) {
        $("." + classId + " > span").text("Solo se pueden subir imágenes con extensión .png");
        return false;
    }
    return true;
}

function getNameFromPath(strFilepath) {
    var objRe = new RegExp(/([^\/\\]+)$/);
    var strName = objRe.exec(strFilepath);

    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}

function submitValidation() {
    return validateFile("Logo", "divFileLogo") && validateFile("Sello", "divFileSello");
}

$(document).ready(function () {
    openExistingHandler();
   // editExistingHandler();
});