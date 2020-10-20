
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function agregarEventosNuevoUsuario() {
    
    //$("#CodigoUnico").autocomplete({
    //    source: function (request, response) {
    //        $.ajax({
    //            url: "/Catalogo/GetEESS",
    //            type: "POST",
    //            dataType: "json",
    //            contentType: "application/json; charset=utf-8",
    //            data: "{ 'prefix': '" + request.term + "'}",
    //            success: function (data) {
    //                response($.map(data, function (item) {
    //                    //
    //                    return { label: item.Nombre, value: item.IdEstablecimiento };
    //                }))
    //            }
    //        })
    //    },
    //    select: function (e, i) {
    //        e.preventDefault();
    //        $("#hddDato").val(i.item.value);
    //        $("#CodigoUnico").val(i.item.label);
    //    },
    //    minLength: 1
    //});

    $("#datepickerIngreso").kendoDatePicker({
        culture: "es-PE"
    });

    $("#datepickerCaducidad").kendoDatePicker({
        culture: "es-PE"
    });

    $("#btnVolverUsuarioAgregar").on("click", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();

        return false;
    });

    $("#btnBuscarValidar").on("click", function () {

        var txtDni = $("#txtDniAgregar").val();

        if (txtDni.length === 0) {
            jAlert("Debe ingresar el DNI", "Alerta");
            return false;
        }

        $.ajax({
            url: URL_BASE + "Usuario/ValidarPersona?NroDocumento=" + txtDni,
            context: document.body
        }).done(function (msg) {
            $("#dvTblDatos").html(msg);
        });

        return false;
    });
}

function limpiarCombo(id) {
    $(id).prop('disabled', false);
    $(id).empty();
    var newOption = $('<option value="0"></option>');
    $(id).append(newOption);
    $(id + "_chosen").removeClass("chosen-disabled");
    $(id).val("0").trigger("liszt:updated");
    $(id).trigger("chosen:updated");
}

function agregarEventosEditarUsuario() {
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

    $("#btnVolverUsuarioEditar").on("click", function () {
        $(this).closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();

        return false;
    });

    $("#btnBuscarValidarEdit").on("click", function () {

        var txtDni = $("#txtDni").val();

        if (txtDni.length === 0) {
            jAlert("Debe ingresar el DNI", "Alerta");
            return false;
        }

        $.ajax({
            url: URL_BASE + "Usuario/ValidarPersona?NroDocumento=" + txtDni,
            context: document.body
        }).done(function (msg) {
            $("#dvTblDatosEdit").html(msg);
        });

        return false;
    });
}

$(document).ready(function () {
    $.ajaxSetup({ cache: false });
    $("#ImageFirma").on("change", function previewFile() {
        var preview = document.querySelector('img');
        var file = document.querySelector('input[type=file]').files[0];
        var reader = new FileReader();

        reader.onloadend = function () {
            preview.src = reader.result;
        }

        if (file) {
            reader.readAsDataURL(file);
        } else {
            preview.src = "";
        }
    })
    $("#dvAccionesUsuario").on("click", "#btnLimpiar", function () {
        $("#login").val("");
        $("#nombres").val("");
        $("#apellidoPaterno").val("");
        $("#apellidoMaterno").val("");

        $("#tablaResultadoUsuario tr:gt(0)").remove();
    });

    //MODAL    
    $("#openDialog").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Nuevo Usuario",
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
                    agregarEventosNuevoUsuario();
                });
                fileValidationHandler("firma", "divFileFirma");
            },
            close: function () {
                $(this).dialog("close");
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });

    $(".editDialog").on("click", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Usuario",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 910,
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
                    agregarEventosEditarUsuario();
                });
                fileValidationHandler("firma", "divFileFirma");
            },
            close: function () {
                $(this).dialog("close");
            }
        });

        $("#dialog-edit").dialog("open");
        return false;
    });

    $(".confirmDialog").on("click", null, function () {

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
                "Cancel": function () {
                    $(this).dialog("close");

                }
            }
        });
        $("#dialog-confirm").dialog("open");
        return false;
    });

    $(".resetDialog").on("click", null, function () {

        var url = $(this).attr("href");
        $("#dialog-reset").dialog({
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
                "Cancel": function () {
                    $(this).dialog("close");

                }
            }
        });
        $("#dialog-reset").dialog("open");
        return false;
    });


    function fileValidationHandler(fileId, classId) {

        $(document).on("change", "#" + fileId, function () {
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
    //function leerArchivo() {
    //    if ($('#firmaU')[0].files && $('#firmaU')[0].files[0]) {
    //        var FR = new FileReader();
    //        FR.onload = function (e) {
    //            document.getElementById("imgFirma").value = e.target.result;
    //            console.log(e.target.result);
    //        };
    //        FR.readAsDataURL($('#firmaU')[0].files[0]);
    //    }
    //}
    function submitValidation() {
        //alert(document.getElementById("firmaU").addEventListener("change", leerArchivo, false));
        return validateFile("firmaU", "divFileFirma");
    }


    //VALIDACION DE NUEVO USUARIO

    $('body').on('submit', '#formRegistrarUsuario', function (e) {
        e.preventDefault();
        var ok = true;
        var mensajeValidacion = "Por favor ingresar los siguientes campos: \n";

        var dniA = $("#txtDniAgregar").val();
        var loginA = $("#txtLoginAgregar").val();
        var tiempoCaducidadA = $("#txtTiempoCaducidadAgregar").val();
        var email = $("#correo").val();

        var form = $("formRegistrarUsuario");

        var tipo = $('#cmbtipoUsuario').val();
        //var establecimiento = $("#hddDato").val();
        //if (tipo == 4) {
        //    if (establecimiento = undefined || establecimiento == '') {
        //        mensajeValidacion = mensajeValidacion + "- Establecimiento";
        //        ok = false;
        //    }
        //}

        if (dniA == "") {
            mensajeValidacion = mensajeValidacion + "- DNI";
            ok = false;
        }
        if (dniA.length < 8 && dniA.length > 0) {
            mensajeValidacion = mensajeValidacion + "- DNI debe tener 8 digitos.";
            ok = false;
        }

        if (loginA == "") {
            mensajeValidacion = mensajeValidacion + "\n- Login";
            ok = false;
        }
        if (tiempoCaducidadA == "") {
            mensajeValidacion = mensajeValidacion + "\n- Tiempo de Caducidad";
            ok = false;
        }
        if (email.trim().length === 0 || !validateEmail(email)) {
            mensajeValidacion = mensajeValidacion + "\n- Correo ingresado es inválido";
            ok = false;
        }
        //submitValidation();
        //console.log($('#firmaU').val());
        //console.log($('#firmaU')[0].files[0]);
        //leerArchivo();
        if (ok) {
            var formData = new FormData(this);
            $.ajax({
                type: "POST",
                cache: false,
                url: $(this).attr('action'),
                data: $(this).serialize(),
                success: function (data) {
                    var existeUsuario = data;

                    if (existeUsuario === 1) {
                        jAlert("El login ingresado ya existe. Por favor ingresar uno disponible.", "Aviso");
                    }
                    else {
                        if (existeUsuario === -1) {
                            jAlert("Ya existe un usuario con el dni ingresado.", "Aviso");
                        } else {
                            jAlert("El usuario fue registrado.", "Aviso", function () {
                                $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                                location.reload();
                            });
                        }
                    }
                }
            });
        }
        else {
            jAlert(mensajeValidacion, "Aviso");
        }

        return false;
    });

    function validateEmail(email) {
        var pattern = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
        return pattern.test(email);
    }

    //VALIDACION DE EDICION DE USUARIO 
    $("body").on("submit", "#formEditarUsuario", function (e) {
        e.preventDefault();
        debugger;
        var ok = true;
        var mensajeValidacion = "Por favor ingresar los siguientes campos: \n";

        var dniE = $("#txtDniEditar").val();
        var login = $("#txtLoginEditar").val();
        var tiempoCaducidad = $("#txtTiempoCaducidadEditar").val();
        var email = $("#correo").val();

        if (dniE === "") {
            mensajeValidacion = mensajeValidacion + "- DNI";
            ok = false;
        }

        if (dniE.length < 8 && dniE.length > 0) {
            mensajeValidacion = mensajeValidacion + "- DNI debe tener 8 digitos.";
            ok = false;
        }

        if (login == "") {
            mensajeValidacion = mensajeValidacion + "\n- Login";
            ok = false;
        }
        if (tiempoCaducidad == "") {
            mensajeValidacion = mensajeValidacion + "\n- Tiempo de Caducidad";
            ok = false;
        }
        if (email !== "" && !validateEmail(email)) {
            mensajeValidacion = mensajeValidacion + "\n- Correo ingresado es inválido";
            ok = false;
        }
        var id = $('#idUsuario').val();
        var apellidoPaterno = $('#apellidoPaterno').val();
        var apellidoMaterno = $('#apellidoMaterno').val();
        var nombres = $('#nombres').val();

        var codigoColegio = $('#codigoColegio').val();
        var profesion = $('#profesion').val();
        var rne = $('#rne').val();

        var cargo = $('#cargo').val();
        var telefono = $('#telefono').val();

        var chkActivo = $('input:checkbox[id=chkActivo]:checked').val();
        var chkActivoR = $('input:checkbox[id=chkActivoR]:checked').val();

        var tipo = $('#tipo').val();
        var componente = $('#componente').val();
        var idTipoUsuario = $('#idTipoUsuario').val();
        var nAprobacion = $('#nAprobacion').val();
        if (ok) {

            $.ajax(
              {
                  url: URL_BASE + "Usuario/EditarUsuario",
                  type: "POST",
                  data: JSON.stringify({
                      id: id, page: 1, apellidoPaterno: apellidoPaterno, apellidoMaterno: apellidoMaterno,
                      nombres: nombres, login: login, codigoColegio: codigoColegio, profesion: profesion, rne: rne, correo: email, cargo: cargo,
                      tiempoCaducidad: tiempoCaducidad, telefono: telefono, tipo: tipo, componente: componente, acceso: idTipoUsuario, nAprobacion: nAprobacion,
                      firmaEdit: null, chkActivo: chkActivo, chkActivoR: chkActivoR
                  }),
                  
                  contentType: "application/json; charset=utf-8",
                  success: function (data) {
                      var existeUsuario = data;

                      if (existeUsuario === 1) {
                          jAlert("El login ingresado ya existe. Por favor ingresar uno disponible.", "Aviso");
                      }
                      else {
                          jAlert("El usuario fue editado correctamente.", "Aviso", function () {
                              $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                              location.reload();
                          });
                      }
                  }
              });



            //$.ajax({
            //    type: "POST",
            //    cache: false,
            //    url: $(this).attr("action"),
            //    data: $(this).serialize(),
            //    success: function (data) {
            //        var existeUsuario = data;

            //        if (existeUsuario === 1) {
            //            jAlert("El login ingresado ya existe. Por favor ingresar uno disponible.", "Aviso");
            //        }
            //        else {
            //            jAlert("El usuario fue editado correctamente.", "Aviso", function () {
            //                $("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
            //                location.reload();
            //            });
            //        }
            //    }
            //});
        }
        else {
            jAlert(mensajeValidacion, "Aviso");
        }

        return false;
    });

    $("#btnBuscarUsuario").on("click", function () {
        var nroDocumentoBusqueda = $.trim($("#dniBusqueda").val());
        var loginBusqueda = $.trim($("#loginBusqueda").val());
        var nombresBusqueda = $.trim($("#nombresBusqueda").val());
        var apellidosMaternoBusqueda = $.trim($("#apellidoMaternoBusqueda").val());
        var apellidosPaternoBusqueda = $.trim($("#apellidoPaternoBusqueda").val());

        if (nroDocumentoBusqueda.length == 0 && loginBusqueda.length == 0 && nombresBusqueda.length == 0 && apellidosMaternoBusqueda.length == 0 && apellidosPaternoBusqueda.length == 0) {
            jAlert("Debe ingresar como mínimo un criterio de búsqueda.", "Alerta");
            return false;
        }

        if (nroDocumentoBusqueda.length > 0 && nroDocumentoBusqueda.length != 8) {
            jAlert("Ingrese Nro de DNI válido, debe ser de 8 dígitos.", "Alerta");
            return false;
        }
    });
});