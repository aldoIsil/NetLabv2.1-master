
var sdata = "";
var xListaProtocolo = [];


function handleFiles(files) {
    // Check for the various File API support.
    if (window.FileReader) {
        // FileReader are supported.
        getAsText(files[0]);
    } else {
        alert('FileReader are not supported in this browser.');
    }
}

function getAsText(fileToRead) {
    var reader = new FileReader();
    // Read file into memory as UTF-8      
    reader.readAsText(fileToRead);
    // Handle errors load
    reader.onload = loadHandler;
    reader.onerror = errorHandler;
}

function loadHandler(event) {
    var csv = event.target.result;
    processData(csv);
}

function processData(csv) {
    var allTextLines = csv.split(/\r\n|\n/);
    var lines = [];
    for (var i = 0; i < allTextLines.length; i++) {
        var data = allTextLines[i].split(';');
        var tarr = [];
        for (var j = 0; j < data.length; j++) {
            tarr.push(data[j]);
        }
        lines.push(tarr);
    }

    //console.log(JSON.stringify(lines));
    //$.ajax({
    //    type: "POST",
    //    cache: false,
    //    dataType: 'json',
    //    data: JSON.stringify({ 'ListaValue': JSON.stringify(lines) }),
    //    url: URL_BASE + "ResultadosAnalisis/ProcesarResultadosCitometria",
    //    contentType: 'application/json; charset=utf-8',
    //    success: function (data) {
    //        jAlert(data, "Aviso", function () {
    //            iBuscarClick();
    //        });
    //    }
    //});
}

function errorHandler(evt) {
    if (evt.target.error.name == "NotReadableError") {
        alert("Canno't read file !");
    }
}
function ProcesarMuestras() {
    debugger;
    //var xList = [];
    //var Protocolo = new Object();
    //$("#tblData tbody tr").each(function (index) {
    //    $(this).children("td").each(function (index2) {
    //        console.log($(this).text());
    //        debugger;
    //        switch (index2) {
    //            case 1:
    //                Protocolo.nroProtocolo = $(this).text();
    //                break;
    //            case 2:
    //                Protocolo.codigoMuestra = $(this).text();
    //                break;
    //            case 3:
    //                Protocolo.kit = $("#ddMetodoInterfazMASED").val();
    //                break;
    //            case 4:
    //                Protocolo.observacion = $(this).text();
    //                break;
    //            case 5:
    //                Protocolo.observacion = $(this).text();

    //                xList.push(Protocolo);
    //                break;
    //        }
    //    });

    //});
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(xListaProtocolo),
        url: URL_BASE + "ResultadosAnalisis/ProcesarProtocolo",
        success: function (data) {
            $("#nroProtocolo").attr("disabled", false);
            $("#excelfile").attr("disabled", false);
            $("#ddMetodoInterfazMASED").attr("disabled", false);
            $("#filaMuestra").empty();
            var Protocolo1 = new Object();
            xListaProtocolo.push(Protocolo1);
        }
    });
}
function Limpiar() {
    $("#nroProtocolo").attr("disabled", false);
    $("#excelfile").attr("disabled", false);
    $("#ddMetodoInterfazMASED").attr("disabled", false);
    $("#filaMuestra").empty();
    var Protocolo2 = new Object();
    xListaProtocolo.push(Protocolo2);
}
function ExportToTable() {
    debugger;
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;
    var vdata = [];
    if ($("#nroProtocolo").val() == "") {
        alert("Por favor ingresar el Nro del protocolo a trabajar.!");
    }
    if ($("#ddMetodoInterfazMASED").val() == "") {
        alert("Por favor seleccionar el Kit.!");
    }
    /*Checks whether the file is a valid excel file*/
    if (regex.test($("#excelfile").val().toLowerCase())) {
        var xlsxflag = false; /*Flag for checking whether excel is .xls format or .xlsx format*/
        if ($("#excelfile").val().toLowerCase().indexOf(".xlsx") > 0) {
            xlsxflag = true;
        }
        /*Checks whether the browser supports HTML5*/
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                /*Converts the excel data in to object*/
                if (xlsxflag) {
                    var workbook = XLSX.read(data, { type: 'binary' });
                }
                else {
                    var workbook = XLS.read(data, { type: 'binary' });
                }
                /*Gets all the sheetnames of excel in to a variable*/
                var sheet_name_list = workbook.SheetNames;

                var cnt = 0; /*This is used for restricting the script to consider only first sheet of excel*/
                sheet_name_list.forEach(function (y) { /*Iterate through all sheets*/
                    debugger;
                    /*Convert the cell value to Json*/
                    if (xlsxflag) {
                        var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);
                    }
                    else {
                        var exceljson = XLS.utils.sheet_to_row_object_array(workbook.Sheets[y]);
                    }
                    if (exceljson.length > 0 && cnt == 0) {
                        //BindTable(exceljson, '#exceltable'); 
                        debugger;
                        cnt++;

                        $.each(exceljson, function (key, item) {
                            sdata += item[Object.keys(item)[0]] + ',';
                        });

                        debugger;
                        $.ajax({
                            type: "POST",
                            cache: false,
                            data: { 'ExcelValuesJson': sdata },
                            url: URL_BASE + "ResultadosAnalisis/ProcesarResultadosExcelJson2",
                            success: function (data) {
                                debugger;
                                //'kit': $("#ddMetodoInterfazMASED option:selected").text()
                                //Agregar datos a la grilla
                                console.log(data);
                                sdata = "";
                                $("#filaMuestra").empty();
                                //Recorremos el resultado de productos
                                $.each(data, function (key, val) {
                                    debugger;
                                    var Protocolo = new Object();
                                    var fila =
                                        "<tr>" +
                                        "<td>" + val.nroSecuencia + "</td>" +
                                        "<td>" + $("#nroProtocolo").val() + "</td>" +
                                        "<td>" + val.codigoMuestra + "</td>" +
                                        "<td>" + $("#ddMetodoInterfazMASED option:selected").text() + "</td>" +
                                        "<td>" + val.muestra_sin_recepcionar + "</td>" +
                                        "<td>" + val.muestra_con_resultado + "</td>"
                                    "</tr>";
                                    $(fila).appendTo($("#filaMuestra"));
                                    sdata += val.codigoMuestra + ',';
                                    Protocolo.nroProtocolo = $("#nroProtocolo").val();
                                    Protocolo.codigoMuestra = val.codigoMuestra;
                                    Protocolo.kit = $("#ddMetodoInterfazMASED").val();
                                    Protocolo.muestra_sin_recepcionar = val.muestra_sin_recepcionar;
                                    Protocolo.muestra_con_resultado = val.muestra_con_resultado;

                                    xListaProtocolo.push(Protocolo);
                                });
                                //bloquear botones para que no modifiquen nada
                                $("#nroProtocolo").attr("disabled", true);
                                $("#excelfile").attr("disabled", true);
                                $("#ddMetodoInterfazMASED").attr("disabled", true);
                            }
                        });
                    }
                });
            }
            if (xlsxflag) {/*If excel file is .xlsx extension than creates a Array Buffer from excel*/
                reader.readAsArrayBuffer($("#excelfile")[0].files[0]);
            }
            else {
                reader.readAsBinaryString($("#excelfile")[0].files[0]);
            }
        }
        else {
            alert("Verifique que tu browser soporte HTML5");
        }
    }
    else {
        alert("Por favor seleccionar un archivo excel válido!");
    }
}