function LoadSolicitantesFn() {
    $.get(URL_BASE + "Solicitante/LoadSolicitante", function (data) {
        //alert(1234);
        $("body #divSolicitante").html(data);
        LoadSolicitantes();
    });
}
function agregarEventosPopupSolicitante() {
    //debugger;
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
                        //
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
    $("#btnAgregarSolicitante").on("click", function () {
        //debugger;
        var Dni = $("#Dni").val();
        var codigoColegio = $("#codigoColegio").val();
        var ApePat = $("#apellidoPaterno").val();
        var ApeMat = $("#apellidoMaterno").val();
        var Nombre = $("#Nombres").val();
        var Correo = $("#correo").val();
        var profesion = $("#cmbProfesion").val();
        var telefono = $("#telefono").val();
        var laboratorio = $("#hddDato").val();

        //if (codigoColegio == undefined || codigoColegio == "" || codigoColegio == 0) {
        //    jAlert("Ingrese el Código del Colegio Profesional.", "Alerta!");
        //    return null;
        //}
        if (ApePat == undefined || ApePat == "" || ApePat == 0) {
            jAlert("Ingrese el Apellido Paterno.", "Alerta!");
            return null;
        }
        if (Nombre == undefined || Nombre == "" || Nombre == 0) {
            jAlert("Ingrese el Nombres.", "Alerta!");
            return null;
        }
        if (laboratorio == undefined || laboratorio == "" || laboratorio == 0) {
            jAlert("Seleccione Centro de Trabajo", "Alerta!");
            return null;
        }
        $.get(URL_BASE + "Orden/ValidarCodigoColegio?codigoColegio=" + codigoColegio, function (data) {
            if (data == "True") {
                jAlert("Código Colegio ya está registrado", "Alerta!");
                return null;
            }
            else {
                var datos = "?Dni=" + Dni + "&" +
                    "codigoColegio=" + codigoColegio + "&" +
                    "ApePat=" + ApePat + "&" +
                    "ApeMat=" + ApeMat + "&" +
                    "Nombre=" + Nombre + "&" +
                    "Correo=" + Correo + "&" +
                    "profesion=" + profesion + "&" +
                    "laboratorio=" + laboratorio + "&" +
                    "telefono=" + telefono;

                dpUI.loading.start("Creando ...");
                $("#dialog-open").dialog("close");
                $.ajax({
                    url: URL_BASE + "Orden/AddSolicitante" + datos,
                    cache: false,
                    method: "GET"
                }).done(function (msg) {
                    LoadSolicitantesFn();
                }).fail(function () {
                    jAlert("Error, por favor comunicarse con el Administrador.", "Error");
                    dpUI.loading.stop();
                }).success(function () {
                    dpUI.loading.stop();
                });
            }
        });
    });
}