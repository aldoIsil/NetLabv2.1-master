function agregarEventosPopupSolicitante() {
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
}
 
function createNewHandler() {
    $(document).on("click", "#openDialogSolicitante", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-open").dialog({
            title: "Nuevo Solicitante",
            autoOpen: false,
            resizable: false,
            height: 430,
            width: 680,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .2)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregarEventosPopupSolicitante();
                });
            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });

        $("#dialog-open").dialog("open");
        return false;
    });
}

function editNewHandler() {
    $(document).on("click", "#editDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Solicitante",
            autoOpen: false,
            resizable: false,
            height: 430,
            width: 680,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .2)),
                collision: "none"
            },
            show: { effect: "drop", direction: "up" },
            modal: true,
            draggable: true,
            open: function () {
                $(this).load(url, function () {
                    agregarEventosPopupSolicitante();
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

$(document).ready(function () {

    $("#btnAgregarSolicitante").click(function () {
        debugger;
        var SolicitanteViewModels = new Object();
        SolicitanteViewModels.Dni = $("#Dni").val();
        SolicitanteViewModels.codigoColegio = $("#codigoColegio").val();
        SolicitanteViewModels.apellidoPaterno = $("#apellidoPaterno").val();
        SolicitanteViewModels.apellidoMaterno = $("#apellidoMaterno").val();
        SolicitanteViewModels.Nombre = $("#Nombres").val();
        SolicitanteViewModels.correo = $("#correo").val();
        SolicitanteViewModels.idProfesion = $("#cmbProfesion").val();
        SolicitanteViewModels.telefonoContacto = $("#telefono").val();
        SolicitanteViewModels.idEstablecimiento = $("#hddDato").val();

        if (SolicitanteViewModels.apellidoPaterno == undefined || SolicitanteViewModels.apellidoPaterno == "" ) {
            jAlert("Ingrese el Apellido Paterno.", "Alerta!");
            return null;
        }

        if (SolicitanteViewModels.apellidoMaterno == undefined || SolicitanteViewModels.apellidoMaterno == "" ) {
            jAlert("Ingrese el Apellido Materno.", "Alerta!");
            return null;
        }

        if (SolicitanteViewModels.Nombre == undefined || SolicitanteViewModels.Nombre == "" || SolicitanteViewModels.Nombre == 0) {
            jAlert("Ingrese el Nombre.", "Alerta!");
            return null;
        }
        if (SolicitanteViewModels.idEstablecimiento == undefined || SolicitanteViewModels.idEstablecimiento == "" || SolicitanteViewModels.idEstablecimiento == 0) {
            jAlert("Seleccione Centro de Trabajo", "Alerta!");
            return null;
        }

        $.ajax(
                   {
                       url: URL_BASE + "Orden/AddSolicitante",
                       type: "POST",
                       data: JSON.stringify({
                           DNI: SolicitanteViewModels.Dni, codigoColegio: SolicitanteViewModels.codigoColegio, ApePat: SolicitanteViewModels.apellidoPaterno,
                           ApeMat: SolicitanteViewModels.apellidoMaterno, Nombre: SolicitanteViewModels.Nombre, Correo: SolicitanteViewModels.correo,
                           profesion: SolicitanteViewModels.idProfesion, laboratorio: SolicitanteViewModels.idEstablecimiento, telefono: SolicitanteViewModels.telefonoContacto
                       }),

                       contentType: "application/json; charset=utf-8",
                       success: function (result) {
                           if (result == 0) {
                               jAlert("Se registró el Solicitante satisfactoriamente", "¡Alerta!", function () {
                                   location.reload(true);
                               });
                           }
                           else {
                               jAlert("El Solicitante ya existe", "¡Alerta!")
                           }
                       }
                   });

    });

    $("#btnEditSolicitante").click(function () {
        debugger;
        var solicitante = new Object();
        solicitante.id = $('#hdnIdSolicitante').val();
        solicitante.dni = $('#Dni').val();
        solicitante.codigoColegio = $("#CodigoUnico").val();
        solicitante.ApePat = $('#apellidoPaterno').val();
        solicitante.ApeMat = $('#apellidoMaterno').val();
        solicitante.Nombre = $('#Nombres').val();
        solicitante.Correo = $('#correo').val();
        solicitante.telefono = $('#telefonoContacto').val();
        solicitante.profesion = $('#cmbProfesion').val();
        solicitante.idEstablecimiento = $('#idEstablecimiento').val();
        solicitante.laboratorio = ($('#hddDato').val() == "") ? solicitante.idEstablecimiento : $('#hddDato').val();


        $.ajax({
            type: "POST",
            cache: false,
            url: URL_BASE + "Solicitante/UpdateSolicitante",
            data: {
                id: solicitante.id, codigoColegio: solicitante.codigoColegio,
                Nombre: solicitante.Nombre, ApePat: solicitante.ApePat, ApeMat: solicitante.ApeMat,
                Correo: solicitante.Correo, dni: solicitante.dni, telefono: solicitante.telefono,
                profesion: solicitante.profesion, idEstablecimiento : solicitante.laboratorio
            },
            success: function () {
                jAlert("Se editó el Solicitante satisfactoriamente", "¡Alerta!", function () {
                    location.reload(true);
                });
            }
        });

        e.preventDefault();
        return false;
    });

    createNewHandler();
    editNewHandler();

    

});

//function AddSolicitante() {
//    $.ajax({
//        type: "POST",
//        cache: false,
//        url: URL_BASE + "Solicitante/ShowPopupSolicitante",
//        success: function (result) {
//            $('#DataAddSolicitante').html(result);
//        }
//    });
//}