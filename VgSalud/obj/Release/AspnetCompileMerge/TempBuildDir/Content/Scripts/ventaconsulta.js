

$(".venta").click(function () {
    alert("entro");
    var DatoCodEspec = this.attributes.CodEspec.value;
    alert("ESPECIALIDAD= " + DatoCodEspec);
    /*  $.get('@Url.Action("VenderConsulta", "Pacientes")',function (response) {
   
        $("#tbody2").empty();
        $("#tbody2").append(response);
   
    });*/
});


