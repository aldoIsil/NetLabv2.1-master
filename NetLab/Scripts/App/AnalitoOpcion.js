function createNewHandler() {
    
    $(document).on("click", "#openDialog", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Crear Nueva Opción",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 500,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
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

function editExistingHandler() {
    
    $(document).on("click", ".editDialog", function () {
        var url = $(this).attr("href");
        $("#dialog-edit").dialog({
            title: "Editar Opción existente",
            autoOpen: false,
            resizable: false,
            height: 'auto',
            width: 500,
            position: {
                my: "center top",
                at: ("center top++" + (window.innerHeight * .1)),
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

function confirmDeleteHandler() {
    
    $(document).on("click", ".confirmDialog", function () {

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
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
        $("#dialog-confirm").dialog("open");
        return false;
    });
}

$(document).ready(function () {

    createNewHandler();

    editExistingHandler();

    confirmDeleteHandler();

    NuevoAnalitoOpciones()
});


/*SOTERO BUSTAMANTE 10/112/2017
NUEVA CONFIGURACION DE OPCIONES DE RESULTADOS*/

function NuevoAnalitoOpciones() {
    debugger

    $('.icon').on('click', function () {
        var $pRow = $(this).parents('tr');
        var $nextRow = $pRow.next('tr');
        $nextRow.toggle();
        $(this).toggleClass('icon-s icon-e');
    });

}

function mostrarListas(rpta) {
    debugger
    var contenido = "";
    if (rpta != "") {
        pos = 0;
        var listas = rpta.split("¬");
        muestras = listas[0].split(";");
        resultados = listas[1].split(";");
        var nRegistros = muestras.length;
        var campos;
        var nCampos;
        contenido += "<table style ='width:90%'>";
        for (var i = 0; i < nRegistros; i++) {
            contenido += "<tr><td>"
            campos = muestras[i].split("|");
            nCampos = campos.length;
            contenido = "<table style ='width:100%'><thead><tr class ='FilaCabecera'>";
            contenido += "<th style='width:10%'>Codigo Muestra</th>"
            contenido += "<th style='width:10%'>Fecha Muestra</th>"
            contenido += "<th style='width:10%'>Fecha Lab Regional</th>"
            contenido += "<th style='width:10%'>Fecha Recepcion INS</th>"
            contenido += "<th style='width:10%'>Num Doc.</th>"
            contenido += "<th style='width:20%'>Paciente</th>"
            contenido += "<th class='FilaInvisible'>Num DNI.</th>"
            contenido += "<th style='width:10%'>Fecha Nacimiento</th>"
            contenido += "<th class='FilaInvisible'>Sexo</th>"
            contenido += "<th class='FilaInvisible'>Gestante</th>"
            contenido += "<th class='FilaInvisible'>Motivo</th>"
            contenido += "<th class='FilaInvisible'>Medico</th>"
            contenido += "<th style='width:10%'>Tipo Muestra</th>"
            contenido += "<th class='FilaInvisible'>Distrito</th>"
            contenido += "<th class='FilaInvisible'>Provincia</th>"
            contenido += "<th class='FilaInvisible'>Dpto</th>";
            contenido += "</tr></thead><tbody><tr class='FilaDatos'>";
            for (var j = 0; j < nCampos; j++) {

                if (j == 0 | j == 1 | j == 2 | j == 3 | j == 4 | j == 5 | j == 7 | j == 12) {
                    contenido += "<td style='align:center'>";
                    contenido += campos[j];
                    contenido += "</td>";
                }
            }
            contenido += "</tr></tbody>";
            contenido += "</table>";
            contenido += "</td></tr>";
            contenido += "<tr><td>&nbsp;</td></tr>"
            contenido += "<tr><td>";
            contenido += crearTablaDetalle(campos[0]);
            contenido += "</td></tr>"
            contenido += "<tr><td>&nbsp;</td></tr>"
        }
        contenido += "</table>";
    }

    document.getElementById("divPreview").innerHTML = contenido;

}