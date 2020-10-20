$(document).ready(function () {
    $('#btnSend').click(function () {
        var errores = '';
        var Contacto = new Object();
        // Validado Nombre ==============================
        if ($('#nombre').val() == '') {
            errores += '<p>Escriba un nombre</p>';
            $('#nombre').css("border-bottom-color", "#F14B4B");
        } else {
            $('#nombre').css("border-bottom-color", "#d1d1d1");
        }
        if ($('#apellido').val() == '') {
            errores += '<p>Escriba un Apellido</p>';
            $('#apellido').css("border-bottom-color", "#F14B4B");
        } else {
            $('#apellido').css("border-bottom-color", "#d1d1d1");
        }
        if ($('#cargo').val() == '') {
            errores += '<p>Escriba un Cargo</p>';
            $('#cargo').css("border-bottom-color", "#F14B4B");
        } else {
            $('#cargo').css("border-bottom-color", "#d1d1d1");
        }
        if ($('#hddDatoEESSOrigen').val() == '') {
            errores += '<p>Seleccione un Establecimiento</p>';
            $('#CodigoUnicoOrigen').css("border-bottom-color", "#F14B4B");
        } else {
            $('#CodigoUnicoOrigen').css("border-bottom-color", "#d1d1d1");
        }
        if ($('#celular').val() == '') {
            errores += '<p>Ingrese un celular</p>';
            $('#celular').css("border-bottom-color", "#F14B4B");
        } else {
            if (!esEntero($('#celular').val())) {
                errores += '<p>Ingrese un celular válido</p>';
                $('#celular').css("border-bottom-color", "#F14B4B");
            }
            else {
                $('#celular').css("border-bottom-color", "#d1d1d1");
            }
        }
        // Validado Correo ==============================
        if ($('#email').val() == '') {
            errores += '<p>Ingrese un correo</p>';
            $('#email').css("border-bottom-color", "#F14B4B");
        } else {
            if (!validateEmail($('#email').val())) {
                errores += '<p>Ingrese un correo válido</p>';
                $('#email').css("border-bottom-color", "#F14B4B");
            }
            else {
                $('#email').css("border-bottom-color", "#d1d1d1");
            }
        }

        // Validado Mensaje ==============================
        if ($('#mensaje').val() == '') {
            errores += '<p>Escriba un mensaje</p>';
            $('#mensaje').css("border-bottom-color", "#F14B4B");
        } else {
            $('#mensaje').css("border-bottom-color", "#d1d1d1");
        }

        // ENVIANDO MENSAJE ============================
        if (errores == '' == false) {
            var mensajeModal = '<div class="modal_wrap">' +
                                    '<div class="mensaje_modal">' +
                                        '<h3>Errores encontrados</h3>' +
                                        errores +
                                        '<span id="btnClose">Cerrar</span>' +
                                    '</div>' +
                                '</div>';

            $('body').append(mensajeModal);
            $('#btnClose').click(function () {
                $('.modal_wrap').remove();
            });
        }
        else {

            Contacto.nombres = $('#nombre').val();
            Contacto.apellidos = $('#apellido').val();
            Contacto.cargo = $('#cargo').val();
            Contacto.idEstablecimiento = $('#hddDatoEESSOrigen').val();
            Contacto.celular = $('#celular').val();
            Contacto.email = $('#email').val();
            Contacto.mensaje = $('#mensaje').val();

            $.ajax({
                url: URL_BASE + "Login/SendMessage",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(Contacto),
                success: function (data) {
                    var mensajeModal = '<div class="modal_wrap">' +
                                    '<div class="mensaje_modal">' +
                                        '<h3>Mensaje Enviado, gracias por su tiempo.</h3>' +
                                        '<span id="btnClose">Cerrar</span>' +
                                    '</div>' +
                                '</div>';
                    $('body').append(mensajeModal);
                    $('#btnClose').click(function () {
                        $('.modal_wrap').remove();
                        history.back();
                    });
                }
            })
        }
    });
    $("#CodigoUnicoOrigen").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Login/GetEstablecimientosNew",
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
            $("#hddDatoEESSOrigen").val(i.item.value);
            $("#CodigoUnicoOrigen").val(i.item.label);
        },
        minLength: 3
    });

    function validateEmail(email) {
        var pattern = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
        return pattern.test(email);
    }
    function esEntero(numero) {
        if (isNaN(numero)) {
            return false;
        } else {
            if (numero % 1 == 0) {
                return true;
            } else {
                return false;
            }
        }
    }
});