function VerSolicitudUsuario(idSolicitudUsuario,tipo) {
    $.ajax({
        type: "GET",
        cache: false,
        url: URL_BASE + "Usuario/GetSolicitudUsuario",
        data: { id: idSolicitudUsuario, tipo:tipo },
        success: function (result) {
            $('#dvSolicitud').html(result);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

function validateEmail(email) {
    var pattern = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
    return pattern.test(email);
}

function iBuscarClick() {
    document.getElementById("btnBuscar").click();
}

function ValidarSolicitud(idSolicitudUsuario) {
    debugger;
    if ($("input[name=radiogroup]:radio:checked").length === 0) {
        jAlert("Debe seleccionar si es o no conforme", "Alerta!");
        return false;
    }

    var comentario = $("#txtComentario").val();
    var esconforme = 3;
    if ($("#rbConforme").is(":checked")) {
        esconforme = 2;
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: URL_BASE + "Usuario/ValidarSolicitud",
        data: { idSolicitudUsuario: idSolicitudUsuario, comentario: comentario, estado: esconforme},
        success: function (result) {
            jAlert("Solicitud validada correctamente.", "Registro Correcto", function () {
                $('#VerSolicitud').modal('hide');
                document.getElementById("btnBuscar").click();
            });
           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        }
    });
}

$(document).ready(function () {

    $("#btnBuscarValidar").click(function () {
        debugger;
        var tipoDoc = $('#tipoDocumento').val();
        var nroDocumento = $('#nroDocumento').val();
        if (tipoDoc == 1 && nroDocumento.length != 8) {
            jAlert('Ingrese el DNI correcto', 'Alerta');
            return false;
        }
        $.ajax({
            url: URL_BASE + "CuentaUsuario/ValidarPersona",
            cache: false,
            method: "POST",
            data: { nroDocumento: nroDocumento },
            success: function (response) {
                if (response.length > 0) {
                    var data = [];
                    data = response.split('|');
                    $("#nombre").val(data[1]);
                    $("#apellidoPaterno").val(data[2]);
                    $("#apellidoMaterno").val(data[3]);
                    $("#idtipoSolicitud").val(data[4]);
                    if (data[0] == "0") {
                        $("#tipoSol").css("display", "block");
                        $("#colegiatura").val(data[4]);
                        $("#correo").val(data[5]);
                        $("#cargo").val(data[6]);
                        $("#profesion").val(data[7]);
                        $("#login").val(data[8]);
                        $("#idtipoSolicitud").val(data[9]);
                    }
                } else {
                    jAlert('Comuniquese con el Administrador', 'Alerta');
                }
            },
        });
    });


    var myDate = new Date();
    var dia = (myDate.getDate() >= 10) ? myDate.getDate() : "0" + myDate.getDate();
    var mes = ((myDate.getMonth() + 1) >= 10) ? (myDate.getMonth() + 1) : "0" + (myDate.getMonth() + 1);
    var anio = myDate.getFullYear();

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

    $("#idtipoSolicitud").on("change", function () {
        var tipoSolicitud = $('#idtipoSolicitud').val();
        if (tipoSolicitud == 2) {
            $("#tipoSol").css("display", "block");
        } else {
            $("#tipoSol").css("display", "none");
        }
    });

    $("#selEnfermedad").chosen({ placeholder_text_single: "Seleccione una Enfermedad", no_results_text: "No existen coincidencias" });

    $("#selEnfermedad").on("change", function () {
        var contenido = "";
        var idEnfermedad = $("#selEnfermedad").val();
        $.ajax(
           {
               url: URL_BASE + "CuentaUsuario/GetExamenesByIdEnfermedadAgrupado?idEnfermedad=" + idEnfermedad,
               type: "POST",
               dataType: "json",
               contentType: "application/json; charset=utf-8",
               success: function (data) {
                   console.log(data);
                   $('#dvPopupExamen1').html('');
                   contenido += "<div id='examen' class='col-md-9'>";
                   if (data.length > 0) {
                       for (var i = 0; i < data.length; i++) {
                           contenido += "<div class='col-md-1'><input type='checkbox' name=" + data[i].nombre + " class='custom-control-input' id=" + data[i].Tipo + " /></div>";
                           contenido += "<div id='nombre' class='col-md-8'><span>" + data[i].nombre + "</span></div>"
                       }
                       contenido += "</div>";
                       $("#dvPopupExamen1").append(contenido);
                   }
               }
           });
    });

    $("#btnAgregar").click(function () {
        debugger;
        var id = [];
        var nombre = [];
        var contenido = "";
        var enfermedad = $('#selEnfermedad option:selected').text();

        $('#examen input:checked').each(function () {
            id.push($(this).attr('id'));
            nombre.push($(this).attr('name'));
        });

        if (id.length == 0 && enfermedad == 1005634) {
            jAlert("Debe seleccionar el examen","Alerta")
            return false;
        } else {
            $("#dtExamen").css("display", "block");

            for (var i = 0; i < id.length; i++) {
                contenido += "<tr style='text-align:center'><td class='idExamen'>" + id[i] + "</td><td class='Enfermedad'>" + enfermedad + "</td><td>" + nombre[i] + "</td>";
                contenido += "<td class='tipo'><select><option value=" + 1 + ">Procesamiento</option><option value=" + 2 + ">Verificacion</option><option value=" + 3 + ">Ambos</option></select></td>";
                contenido += "<td><button type='button' id = 'btnEliminar' class='btn btn-default'>Eliminar</button></td></tr>";
            }
            $("#tbExamen").append(contenido);
            $('#myModal').modal('hide');
        }

        $("#btnEliminar").click(function () {
            $(this).closest('tr').remove();
        });

    }); 



    if (window.File && window.FileReader && window.FileList && window.Blob) {
        document.getElementById('files').addEventListener('change', handleFileSelect, false);
    } else {
        alert('The File APIs are not fully supported in this browser.');
    }
    var NameFile = null;
    function handleFileSelect(evt) {
        debugger;
        var f = evt.target.files[0]; // FileList object
        var reader = new FileReader();
        // Closure to capture the file information.
        reader.onload = (function (theFile) {
            return function (e) {
                var binaryData = e.target.result;
                //Converting Binary Data to base 64
                var base64String = window.btoa(binaryData);
                //showing file converted to base64
                document.getElementById('base64').value = base64String;
                //alert('File converted to base64 successfuly!\nCheck in Textarea');
            };
        })(f);
        // Read in the image file as a data URL.
        reader.readAsBinaryString(f);
        NameFile = f.name;
    }

    $("#btnGuardar").click(function () {
        debugger;
        //dpUI.loading.start("Cargando ...");
        var idAcepta = $('input:checkbox[id=idAcepta]:checked').val();
        if (idAcepta == 1) {
            var idRol = [];
            var idExamen = [];
            var Enfermedad = [];

            var ok = true;
            var mensajeValidacion = "Por favor ingresar los siguientes campos: \n";

            $('#rol input:checked').each(function () {
                idRol.push($(this).attr('id'));
            });

            $('#examen input:checked').each(function () {
                idExamen.push($(this).attr('id'));
            });

            $('#tipo input:selected').each(function () {
                tipo.push($(this).attr('value'));
            });

            $(".tipo").parent("tr").find(".Enfermedad").each(function () {
                Enfermedad += $(this).html() + ',';
            });

            var archivo = $('#base64').val();
            var nombreArchivo = NameFile;
           
            var SolicitudUsuario = new Object();
            SolicitudUsuario.tipoSolicitud = $('#idtipoSolicitud').val();
            SolicitudUsuario.login = $('#login').val();
            SolicitudUsuario.tipoDocumento = $('#tipoDocumento').val();
            SolicitudUsuario.documentoIdentidad = $('#nroDocumento').val();
            SolicitudUsuario.nombre = $('#nombre').val();
            SolicitudUsuario.apellidoPaterno = $('#apellidoPaterno').val();
            SolicitudUsuario.apellidoMaterno = $('#apellidoMaterno').val();
            SolicitudUsuario.cargo = $('#cargo').val();
            SolicitudUsuario.condicionLaboral = $('#condicion').val();
            SolicitudUsuario.profesion = $('#profesion').val();
            SolicitudUsuario.codigoColegio = $('#colegiatura').val();
            SolicitudUsuario.iniciales = $('#siglas').val();
            SolicitudUsuario.correo = $('#correo').val();
            SolicitudUsuario.telefono = $('#celular').val();
            SolicitudUsuario.Establecimiento = $('#idEstablecimiento').val();
            SolicitudUsuario.Renipress = $('#CodigoUnico').val().substring(0,8);
            SolicitudUsuario.tipoEst = $('#tipoEst').val();
            SolicitudUsuario.componente = $('input:radio[name=componente]:checked').val()
            SolicitudUsuario.solicitante = $('input:checkbox[id=solicitante]:checked').val();
            SolicitudUsuario.consultor = $('input:checkbox[id=consultor]:checked').val();
            SolicitudUsuario.rol = idRol;
            SolicitudUsuario.examen = idExamen;
            SolicitudUsuario.jefeEESS = $('#UserJefe').val();
            SolicitudUsuario.cargoJf = $('#cargoJf').val();
            SolicitudUsuario.enfermedad = Enfermedad;
            //SolicitudUsuario.archivo = document.getElementById('fileUser');

            if (SolicitudUsuario.tipoSolicitud == 0) {
                mensajeValidacion = mensajeValidacion + "\n- Tipo de Solicitud";
                ok = false;
            }

            if (SolicitudUsuario.tipoSolicitud == 2) {
                if (SolicitudUsuario.login == '') {
                    mensajeValidacion = mensajeValidacion + "\n- Nombre de Usuario";
                    ok = false;
                }
            }

            if (SolicitudUsuario.tipoDocumento == 0) {
                mensajeValidacion = mensajeValidacion + "\n- Tipo de Documento";
                ok = false;
            }

            if (SolicitudUsuario.documentoIdentidad == '') {
                mensajeValidacion = mensajeValidacion + "\n- Número de documento de identidad";
                ok = false;
            }

            if (SolicitudUsuario.nombre == '' || SolicitudUsuario.apellidoPaterno == '' || SolicitudUsuario.apellidoMaterno == '') {
                mensajeValidacion = mensajeValidacion + "\n- Datos Usuario";
                ok = false;
            }

            if (SolicitudUsuario.cargo == '') {
                mensajeValidacion = mensajeValidacion + "\n- Cargo";
                ok = false;
            }

            if (SolicitudUsuario.condicionLaboral == '') {
                mensajeValidacion = mensajeValidacion + "\n- Condición Laboral";
                ok = false;
            }

            if (SolicitudUsuario.correo !== "" && !validateEmail(SolicitudUsuario.correo)) {
                mensajeValidacion = mensajeValidacion + "\n- Correo ingresado es inválido";
                ok = false;
            }

            if (SolicitudUsuario.Establecimiento == undefined || SolicitudUsuario.Establecimiento == 0) {
                mensajeValidacion = mensajeValidacion + "\n- Establecimiento";
                ok = false;
            }

            if (SolicitudUsuario.componente == undefined || SolicitudUsuario.componente == 0) {
                mensajeValidacion = mensajeValidacion + "\n- Componente";
                ok = false;
            }

            if (SolicitudUsuario.rol.length == 0) {
                mensajeValidacion = mensajeValidacion + "\n- Roles";
                ok = false;
            }

            if (SolicitudUsuario.jefeEESS == '' || SolicitudUsuario.cargoJf == '') {
                mensajeValidacion = mensajeValidacion + "\n- Datos Usuario Responsable EESS";
                ok = false;
            }

            if (SolicitudUsuario.solicitante != 1 && SolicitudUsuario.consultor != 1) {
                SolicitudUsuario.idTipoUsuario = 1;
            }
            else if (SolicitudUsuario.solicitante == 1) {
                SolicitudUsuario.idTipoUsuario = 4;
            }
            else {
                SolicitudUsuario.idTipoUsuario = 5;
            }

            if (ok) {
                $.ajax(
               {
                   url: URL_BASE + "CuentaUsuario/RegistrarSolicitud",
                   type: "POST",
                   data: JSON.stringify({
                       tipoSolicitud: SolicitudUsuario.tipoSolicitud, login: SolicitudUsuario.login,
                       tipoDocumento: SolicitudUsuario.tipoDocumento, documentoIdentidad: SolicitudUsuario.documentoIdentidad,
                       nombre: SolicitudUsuario.nombre, apellidoPaterno: SolicitudUsuario.apellidoPaterno, apellidoMaterno: SolicitudUsuario.apellidoMaterno,
                       cargo: SolicitudUsuario.cargo, condicionLaboral: SolicitudUsuario.condicionLaboral, profesion: SolicitudUsuario.profesion,
                       codigoColegio: SolicitudUsuario.codigoColegio, iniciales: SolicitudUsuario.iniciales, correo: SolicitudUsuario.correo,
                       telefono: SolicitudUsuario.telefono, Establecimiento: SolicitudUsuario.Establecimiento, Renipress: SolicitudUsuario.Renipress, tipoEst: SolicitudUsuario.tipoEst,
                       rol: SolicitudUsuario.rol, componente: SolicitudUsuario.componente, idTipoUsuario: SolicitudUsuario.idTipoUsuario, examen: SolicitudUsuario.examen,
                       jefeEESS: SolicitudUsuario.jefeEESS, cargoJf: SolicitudUsuario.cargoJf, enfermedad: SolicitudUsuario.enfermedad, archivo: archivo, nombreArchivo: nombreArchivo
                   }),

                   contentType: "application/json; charset=utf-8",
                   success: function () {
                       //jAlert("Solicitud de usuario enviado exitósamente", "¡Alerta!");
                       jAlert("Solicitud de usuario enviado exitósamente", "¡Alerta!", function () {
                           Cargar();
                       });
                   }
               });
            }
            else {
                jAlert(mensajeValidacion, 'Aviso');
            }
        }
        else {
            jAlert('¡Debe leer y aceptar el acuerdo de confidencialidad!', 'Aviso');
        }
    });


    $("#btnBuscar").click(function () {
        debugger;
        var tipoDocumento = $('#tipoDocumento').val();
        var documentoIdentidad = $('#documentoIdentidad').val();

        if (tipoDocumento == 0 || documentoIdentidad == '') {
            jAlert('Ingrese Tipo y Documento de identidad', '¡Alerta!');
            return false;
        }


    });
});

function Cargar() {
    debugger;
    location.reload(true);
}

function ImprimirSolicitud(idSolicitudUsuario) {
    debugger;
    $.ajax(
           {
               url: URL_BASE + "CuentaUsuario/ImprimirSolicitud?idSolicitudUsuario=" + idSolicitudUsuario,
               cache: false,
               method: "GET"
           });
}
