

$(".venta").click(function () {
         
    alert("entro");
    debugger;
    var DatoCodEspec = this.attributes.CodEspec.value;
    var DatoCodHist = $('#CodHistoria').val();
    //$("#servicioTemp").val(DatoCodEspec);
    $.ajax({
        url: '../Pacientes/CargarVenta',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            'CodEspec': DatoCodEspec,
            'historia': DatoCodHist,
            //'descripcion': ''
        }),
        success: function (response) {
            alert('ok');
            //oDetalle.length = 0;
            //oCabecera.length = 0;
            //$('input:radio[name=ckServicio]').attr("disabled", false);
            //$('#dataTarifa tbody').empty();
            //$('#dataTarifa tbody').html(response.result);
            //printTabla();
        }
        //,
        //error: function () {
        //    alertaMessage("No se pudo generar los servicios", 0);
        //}
    });

    //$.get('@Url.Action("CargarVenta", "Pacientes")',

    //    function (response) {
   
    //    //$("#tbody2").empty();
    //    //$("#tbody2").append(response);
   
    //});
});

    



