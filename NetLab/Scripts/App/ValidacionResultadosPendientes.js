function grabarDetallePendiente()
    {
        if ($("input[name='resultados']:checked").length === 0) {
            jAlert("Debe seleccionar un resultado", "Alerta!");
            return false;
        }

        if ($("input[name=radiogroup]:radio:checked").length === 0) {
            jAlert("Debe seleccionar si es o no conforme", "Alerta!");
            return false;
        }

        var comentario = $("#txtComentario").val();
        var esconforme = 0;
        if ($("#rbConforme").is(":checked")) {
            esconforme = 1;
        }

        var ok = true;
        var deferred = [];
        var idOrden = $("#CodigoOrdenEnvioRespuesta").val();
        
        $.each($("input[name='resultados']:checked"), function () {

            deferred.push($.ajax({
                type: "POST",
                cache: false,
                url: URL_BASE + "ValidacionResultados/Update?idOrdenExamen=" + $(this).val() + "&comentario=" + comentario + "&esconforme=" + esconforme + "&idOrden=" + idOrden,
                data: $(this).serialize(),
                failure: function () {
                    ok = false;
                    return false;
                }
            }));
        });
        //    //Inicio Juan Muga
        //    //EnviarRespuesta($("#CodigoOrdenEnvioRespuesta").val());
        //    //Fin Juan Muga
        //}

        $.when.apply(this, deferred).done(function() {
            if (ok) {
                jAlert("Los datos han sido procesados correctamente.", "Registro Correcto", function () {
                    document.getElementById("btnBuscar").click();
                });
                //jAlert("Los datos han sido grabados correctamente", "Aviso");   
                //$("#dialog-edit").closest(".ui-dialog").find(".ui-dialog-titlebar-close").click();
                //$(".editDialog").trigger("click");                
            } else {
                jAlert("Hubo un error al grabar los datos", "Alerta!");
            }
        });

        //Descripción:Envío de resultados a otros al EQhali 
        //Author: Juan Muga.
        function EnviarRespuesta(idPopup) {
            debugger;
            if (idPopup.length > 0) {
                var url = 'http://localhost:27110/Token';
                var body = {
                    grant_type: 'password',
                    username: 'wsnetlab2',
                    password: '0'
                };
                //Get Token
                $.ajax({
                    url: url,
                    method: 'POST',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: body,
                    success: function (result) {
                        debugger;
                        Token = result.access_token;
                        console.log(Token);
                        if (Token.length > 0) {
                            $.ajax({
                                url: URL_BASE + "OrdenMuestra/GetTramaResultadobyCodigoOrden?idPopup=" + idPopup,
                                cache: false,
                                method: "GET",
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    debugger;
                                    console.log(result);
                                    if (result.length > 0) {
                                        $.ajax({
                                            url: 'http://localhost:27110/api/paciente/ReadResultado?Resultado=' + JSON.stringify(result),
                                            type: 'GET',
                                            data: { Resultado: JSON.stringify(result) },
                                            contentType: "application/json;chartset=utf-8",
                                            headers: { "Authorization": Token },
                                            success: function (response) {
                                                debugger;
                                                console.log(JSON.stringify(response));
                                                $.ajax({
                                                    url: URL_BASE + "OrdenMuestra/UpdateTramaResultadobyCodigoOrden?idPopup=" + idPopup,
                                                    cache: false,
                                                    method: "GET"
                                                });
                                            },
                                            statusCode: {
                                                statusCode: {
                                                    200: function () {
                                                        alert('200');
                                                    },
                                                    404: function () {
                                                        alert('404');
                                                    },
                                                    500: function () {
                                                        alert('500');
                                                    }
                                                }
                                            },
                                            error: function (xhr) {
                                                alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
                                            }
                                        });
                                    }
                                },
                                error: function (jqXHR) {
                                    alert(jqXHR.responseText);
                                }
                            }).fail(function () {
                                //debugger;
                                jAlert("Se produjo un error de Conexión.", "Error")
                            })
                        }

                    },
                    error: function (jqXHR) {
                        alert(jqXHR.responseText);
                    }
                });
            }
        }
        return false;
}