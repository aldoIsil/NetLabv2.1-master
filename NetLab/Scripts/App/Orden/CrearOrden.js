function LoadSolicitantes() {
    //alert("load solicitantes");
    $("#solicitante").ajaxChosen({
        dataType: "json",
        type: "GET",
        //minTermLength: 3,
        afterTypeDelay: 300,
        cache: false,
        url: URL_BASE + "Solicitante/GetSolicitantes"
    }, {
        loadingImg: URL_BASE + "Content/images/loading.gif"
    }, {
        placeholder_text_single: "Seleccione el Solicitante", no_results_text: "No existen coincidencias"
    }).change(function () {
        //alert($("#solicitante").val());
    });
}

$(document).ready(function () {
    $(".idLaboratorioDestinoMaterial").each(function () {
        $(this).ajaxChosen({
            dataType: "json",
            type: "POST",
            minTermLength: 3,
            afterTypeDelay: 300,
            cache: false,
            url: URL_BASE + "OrdenMuestra/GetAllLaboratorios"
        }, {
            loadingImg: URL_BASE + "Content/images/loading.gif"
        }, { placeholder_text_single: "Seleccione el Laboratorio", no_results_text: "No existen coincidencias" }
       );
    });

    $.validator.addMethod("date",
      function (value, element) {
          if (this.optional(element)) {
              return true;
          }

          var ok = true;
          try {
              $.datepicker.parseDate("dd/mm/yyyy", value);
          }
          catch (err) {
              ok = false;
              ok = true;
          }
          return ok;
      });

    //Muestra el popup para seleccionar y agregar la enfermedad y examen
    $("#btnShowPopupEnfermedadExamen").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href") + "?_=" + (new Date()).getTime();
        $("#dialog-open").html("");
        $("#dialog-open").dialog({
            title: "Agregar Examen",
            autoOpen: false,
            resizable: false,
            draggable: true,
            height: 300,
            width: 500,
            show: { effect: "drop", direction: "up" },
            modal: true,
            responsive: true,

            fluid: true,
            helper: 'clone',
            open: function () {
                $(this).load(url, function () {
                    agregarEventosPopupEnfermedadExamen();
                });
            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });
        $("#dialog-open").dialog("open");
        return false;
    });

    $("#btnShowPopupSolicitante").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-open").dialog({
            title: "Nuevo Solicitante",
            autoOpen: false,
            resizable: true,
            height: 430,
            width: 600,
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
    LoadSolicitantes();
     //console.log($("#ActualDepartamentoEESS").val());
     //                   console.log($("#ActualProvinciaEESS").val());
    //                   console.log($("#ActualDistritoEESS").val());
    $("#CodigoUnicoOrigen").autocomplete({        
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Orden/GetEstablecimientosNew",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ 'prefix': '" + request.term + "'," + "'tipo': '" + 1 + "'," + "  'Departamento': '" + $("#ActualDepartamentoEESS").val() + "'," + "  'Provincia': '" + $("#ActualProvinciaEESS").val() + "'," + "  'Distrito': '" + $("#ActualDistritoEESS").val() + "'" + "}",
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
            $("#idEstablecimientoFrecuente option:selected").prop("selected", false);
        },
        minLength: 3
    });

    $("#CodigoUnicoEnvio").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Orden/GetEstablecimientosNew",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ 'prefix': '" + request.term + "'," + "  'Departamento': '" + $("#ActualDepartamentoEESS").val() + "'," + "  'Provincia': '" + $("#ActualProvinciaEESS").val() + "'," + "  'Distrito': '" + $("#ActualDistritoEESS").val() + "'" + "}",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Nombre, value: item.IdEstablecimiento };
                    }));
                }
            });
        },
        select: function (e, i) {
            e.preventDefault();
            $("#hddDatoEESSEnvio").val(i.item.value);
            $("#CodigoUnicoEnvio").val(i.item.label);            
        },
        minLength: 3
    });
    
    $("#ciudad").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: URL_BASE + "Orden/GetCiudadProcedencia?prefix=" + request.term,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.NombreCiudad + ' - ' + item.NombrePais, value: item.idCiudad };
                    }))
                }
            })
        },
        select: function (e, i) {
            e.preventDefault();
            $("#id").val(i.item.value);
            $("#ciudad").val(i.item.label);
        },
        minLength: 3
    });
    $("#ciudad").click(function () {
        $("#id").val("");
        $("#ciudad").val("")

    });

    $("#btnAgregar").click(function () {
        var city = $('#ciudad').val();
        var idCity = $('#id').val();
        var contenido = '';
        contenido += "<tr><td class='id' style='display:none;'>" + idCity + "</td><td>" + city + "</td><td><input type='button' class='borrar' value='Quitar' /></td></tr>";
        $("#dtCiudad").css("display", "block");
        $('#TbCiudad').append(contenido);
    });

    $(document).on('click', '.borrar', function (event) {
        event.preventDefault();
        $(this).closest('tr').remove();
    });

    $("#btnBuscarValidar").click(function () {
        var dni = $('#dniPr').val();
        $.ajax({
            url: URL_BASE + "Orden/ValidarPersona",
            cache: false,
            method: "POST",
            data: { nroDocumento:dni },
            success: function (response) { 
                $("#ejecutor").val(response);
            },
        });
    });
});
