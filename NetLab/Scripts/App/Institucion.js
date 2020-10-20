$(document).ready(function () {
    createNewHandler();
    editNewHandler();

    $('#dialog-edit').on('click', '#btnGuardar', function (e) {
        $('#formEditarInstitucion').submit();
    });

    $("body").on("submit", "#formEditarInstitucion", function (e) {
        debugger;
        var codigo = $("#codigoInstitucion").val();
        var nombre = $("#nombreInstitucion").val();
        var descripcion = $("#descripcion").val();
        var estado = $("#chkActivoRol").is(":checked")?1:0;
        if (nombre == "" || descripcion == "") {
            jAlert("Debe registrar todos los campos");
            return false;
        }
        var datos = "?id=" + codigo + "&nombreInstitucion=" + nombre + "&descripcion=" + descripcion + "&estado=" + estado;
        $.ajax(
            {
                url: URL_BASE + "Institucion/EditarInstitucion" + datos,
                cache: false,
                method: "GET",
            }).success(function (response) {
                jAlert("Se actualizaron los datos correctamente.", "Alerta!");
            });
        e.preventDefault();
        return false;
    });
});

function editNewHandler() {
    $(document).on("click", "#editDialog", function (e) {
        debugger;
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Institución",
            autoOpen: false,
            resizable: false,
            height: 300,
            width: 450,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .2)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);
            },
            close: function () {
                $(this).dialog("close");
            }
        });
        $("#dialog-edit").dialog("open");
        return false;
    });
}

function createNewHandler() {
    $(document).on("click", "#openDialog", function (e) {
        debugger;
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-open").dialog({
            title: "Nueva Institución",
            autoOpen: false,
            resizable: false,
            height: 300,
            width: 450,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .2)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url);
            },
            close: function () {
                $(this).dialog("close");
            }
        });
        $("#dialog-open").dialog("open");
        return false;
    });
}