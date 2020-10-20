$(function () {
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
                        return { label: item.Nombre, value: item.CodigoUnico };
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
});