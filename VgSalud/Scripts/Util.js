var inicio = function () {


    $(".Si").on("click", function () {
        var historia = this.attributes.historia.value;
        var carnet = this.attributes.carnet.value;
        var dni = $("#dni").val();
        var fecha = $("#fecha").val();
        var url = "/Carnet/Usp_Actualizar_CarnetSanidad_Entregado";
        $.get(url, { historia: historia, dni: dni, fecha: fecha, carnet: carnet },
            function (response) {

                $("#menu").empty();
                $(".popup").addClass("open");
                $("#menu").append(response);
                $("." + carnet).fadeOut();
                $(".Si").fadeOut();
                $(".No").fadeOut();


            });

    });


    $(".Si1").on("click", function () {
        var historia = this.attributes.historia.value;
        var carnet = this.attributes.carnet.value;
        var dni = $("#dni").val();
        var fecha = $("#fecha").val();
        var url = "/Carnet/Get_Actualizar_Entrega_Carnet_Sanidad_index";
        $.get(url, { historia: historia, carnet: carnet },
            function (response) {

                $("#menu").empty();
                $(".popup").addClass("open");
                $("#menu").append(response);
                $("." + carnet).fadeOut();
                $(".Si").fadeOut();
                $(".No").fadeOut();


            });

    });

    $(".No1").on("click", function () {
        var url = "/Carnet/Get_Entregar_Carnet_Inicio";
        $.get(url, {},
                   function (response) {
                       $("#menu").empty();
                       $(".popup").addClass("open");
                       $("#menu").append(response);
                       $(".Si").fadeOut();
                       $(".No").fadeOut();
                   });

    });

    $(".No").on("click", function () {
        var url = "/Carnet/Get_Entregar_Carnet_Inicio";
        $.get(url, {},
                   function (response) {
                       $("#menu").empty();
                       $(".popup").addClass("open");
                       $("#menu").append(response);
                       $(".Si").fadeOut();
                       $(".No").fadeOut();
                   });

    });


    $(".llamar").on("click", function () {
        var historia = this.attributes.historia.value;
        var carnet = this.attributes.carnet.value;
        var dni = $("#dni").val();
        var fecha = $("#fecha").val();
        var url = "/Carnet/GetFiltroDniFecha";
        $.get(url, { dni: dni, fecha: fecha },
            function (response) {
               
                $("#menu").empty();
                $(".popup").addClass("open");
                $("#menu").append(response);
                $(".Si").fadeOut();
                $(".No").fadeOut();
                $("." + carnet).fadeIn();
                $("." + historia).fadeOut();
            
            });




    });

    $(".llamar1").on("click", function () {
        var historia = this.attributes.historia.value;
        var carnet = this.attributes.carnet.value;
        var url = "/Carnet/Get_Entregar_Carnet_Inicio";
        $.get(url, {},
            function (response) {
                $("#menu").empty();
                $(".popup").addClass("open");
                $("#menu").append(response);
                $(".Si").fadeOut();
                $(".No").fadeOut();
                $("." + carnet).fadeIn();
                $("." + historia).fadeOut();
            });

    });


}

$(document).ready(inicio);