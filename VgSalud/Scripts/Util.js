var inicio = function () { 
    $(".llamar").on("click", function () {
        debugger;
        var historia = this.attributes.historia.value;
        var carnet = this.attributes.carnet.value;
        $(this).fadeOut();
        $("." + carnet).fadeIn();
        $(".No" + carnet).fadeIn();
        $(".dropdown").addClass("open");
    });

    $(".Si").on("click", function () {
        debugger;
        var historia = this.attributes.historia.value;
        var carnet = this.attributes.carnet.value;
        var dni = $("#dni").val();
        var fecha = $("#fecha").val();
        var url = "./Carnet/Usp_Actualizar_CarnetSanidad_Entregado";
        $.get(url, { historia: historia, dni: dni, fecha: fecha, carnet: carnet },
            function (response) {
                $("#menu").empty();
                $(".dropdown").addClass("open");
                $("#menu").append(response);
                $("." + carnet).fadeOut();
                $(".Si").fadeOut();
                $(".No").fadeOut();
            });

    });

} 

$(document).ready(inicio);