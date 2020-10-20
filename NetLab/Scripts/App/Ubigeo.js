// Descripción: Obtiene el registro cada vez que se realiza una seleccion en el combo del departamento
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function onChangeDepartamento(ddlDepartamento, ddlProvincia, ddlDistrito, divProvincias, divDistritos, urlProvinciasPorDepartamento, urlDistritosPorProvincia) {
    debugger;
    var departamentoSelector = "#" + ddlDepartamento;
    var provinciaSelector = "#" + ddlProvincia;
    var divProvinciasSelector = "#" + divProvincias;
    var divDistritosSelector = "#" + divDistritos;

    var codigoDepartamento = $(departamentoSelector).val();
    var codigoProvincia = $(provinciaSelector).val();

    $(divProvinciasSelector).load(urlProvinciasPorDepartamento + "?departamentoSelectId=" + ddlDepartamento + "&provinciaSelectId=" + ddlProvincia + "&distritoSelectId=" + ddlDistrito + "&distritoDivId=" +divDistritos + "&codigoDepartamento=" + codigoDepartamento);
    $(divDistritosSelector).load(urlDistritosPorProvincia + "?distritoSelectId=" + ddlDistrito + "&codigoDepartamento=" + codigoDepartamento + "&codigoProvincia=" + codigoProvincia);
}
// Descripción: Obtiene el registro cada vez que se realiza una seleccion en el combo de la provincia.
// Author: Terceros.
// Fecha Creacion: 01/01/2017
// Fecha Modificación: 02/02/2017.
// Modificación: Se agregaron comentarios.
function onChangeProvincia(ddlDepartamento, ddlProvincia, ddlDistrito, divDistritos, urlDistritosPorProvincia) {

    var departamentoSelector = "#" + ddlDepartamento;
    var provinciaSelector = "#" + ddlProvincia;
    var divDistritosSelector = "#" + divDistritos;

    var codigoDepartamento = $(departamentoSelector).val();
    var codigoProvincia = $(provinciaSelector).val();

    $(divDistritosSelector).load(urlDistritosPorProvincia + "?distritoSelectId=" + ddlDistrito + "&codigoDepartamento=" + codigoDepartamento + "&codigoProvincia=" + codigoProvincia);
}
// Descripción: Obtiene el registro cada vez que se realiza una seleccion en el combo de la distrito.
// Author: Marcos Mejia.
// Fecha Creacion: 30/01/2018
function onChangeDistrito(ddlDistrito) {
    debugger;

    var distritosSelector = "#" + ddlDistrito;

    var codigoDepartamento = $("#ActualDepartamento").val();
    var codigoProvincia = $("#ActualProvincia").val();
    var codigoDistrito = $(distritosSelector).val();

    var ubigeo = codigoDepartamento + codigoProvincia + codigoDistrito

    if (ubigeo.length == 6) {
        //return ubigeo;
        $.ajax(
            {
                url: URL_BASE + "EESS/CodigoUbigeoLaboratorio?ubigeo=" + ubigeo,
                cache: false,
                method: "GET"
            })
    }
    else {
        return null;
    }

    
}