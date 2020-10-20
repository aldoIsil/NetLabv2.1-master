//SCRIPT DE LAS TABLAS CON LAS OPCIONES DE OCULTAR DESACTIVAS - PASAR A DOCUMENTO JS INDEPEDENDIENTE Y REFERENCIAR EN PAGINAS CON TABLAS 

$(document).ready(function () {
    var table = $('#mydata1').DataTable({

        //"bFilter": false,
        //"paging": false,
        //"bInfo" : false,

        //          aLengthMenu: [
        //[10, 25,50, 100, 200, -1],
        //[10, 25,50, 100, 200, "All"]
        //          ],
        //          iDisplayLength: -1,
        //          "dom": '<"top"iflp<"clear">>rt<"bottom"iflp<"clear">>' 

    });

    $('a.toggle-vis').on('click', function (e) {
        e.preventDefault();

        // Get the column API object
        var column = table.column($(this).attr('data-column'));

        // Toggle the visibility
        column.visible(!column.visible());
    });
});

