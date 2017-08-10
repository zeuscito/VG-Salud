var oCabecera = new Array();
var oDetalle = new Array();
var historia = $("#historia").val();

$(document).ready(function () {
    $.ajax({
        url: '../AtencionVarias/CargaServicios',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            'historia': historia
        }),
        success: function (response) {
            $('#dataServicio tbody').html(response.result);
        },
        error: function () {
            alertaMessage("No se pudo generar los servicios", 0);
        }
    });

    $("#changeServ").click(function () {
        
        oDetalle.length = 0;
        oCabecera.length = 0;
        $('input:radio[name=ckServicio]').attr("disabled", false);
        printTabla();
    });

});

function generaTarifa(e) {
    $("#servicioTemp").val(e);
    $.ajax({
        url: '../AtencionVarias/CargaTarifa',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            'CodServ': e,
            'historia': historia,
            'descripcion' : ''
        }),
        success: function (response) {
            $('input:radio[name=ckServicio]').attr("disabled", true);
            $('#dataTarifa tbody').empty();
            $('#dataTarifa tbody').html(response.result);
        },
        error: function () {
            alertaMessage("No se pudo generar los servicios", 0);
        }
    });
}

function registraItem(obj) {

    var precio = $("#precio-" + obj).val();
    var cantidad = $("#cantidad-" + obj).val();
    var tarifa = $("#tarifa-" + obj).val();
    var espeServ = $('input:radio[name=ckServicio]:checked').val();
    var valorRetorno = espeServ.split(",");
    var flag = 1;

    $.each(oDetalle, function (key, value) {
        var valorRetorno = value.split(",");
        if (valorRetorno[0] === tarifa)
        {
            alertaMessage("La tarifa ya esta asignada");
            flag = 2;
        }
    });

    if(flag === 1){
        if (oCabecera.length === 0) {
            var tot = (parseFloat(precio) * parseFloat(cantidad));
            oCabecera[0] = historia + "," + tot;
            $("#totalPagar").html(tot);
        }
        else {
            var valorCabecera = oCabecera[0].split(",");
            var tot = parseFloat(valorCabecera[1]) + (precio * cantidad);
            oCabecera[0] = valorCabecera[0] + "," + tot;
            $("#totalPagar").html(tot);
        }    
        oDetalle[oDetalle.length] = tarifa + "," + cantidad + "," + precio + "," + valorRetorno[0] + "," + valorRetorno[1] + "," + valorRetorno[3];
        printTabla();
    }
}

$("#buscaTarifa").keyup(function () {
    var value = $(this).val();
    var e = $("#servicioTemp").val();
    $.ajax({
        url: '../AtencionVarias/CargaTarifa',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            'CodServ': e,
            'historia': historia,
            'descripcion': value
        }),
        success: function (response) {
            $('#dataTarifa tbody').empty();
            $('#dataTarifa tbody').html(response.result);
        },
        error: function () {
            alertaMessage("No se pudo generar los servicios", 0);
        }
    });
});

function eliminaPrecio(e) {
    var valorDetalle = oDetalle[e].split(",");
    var valorCabecera = oCabecera[0].split(",");
    var tot = (parseFloat(valorCabecera[1]) - parseFloat(valorDetalle[2]));
    oCabecera[0] = valorCabecera[0] + "," + tot;
    $("#totalPagar").html(tot);

    oDetalle.splice(e, 1);
    if (oDetalle.length === 0) {
        oCabecera.length = 0;
        $("#totalPagar").html("0");
    }

    printTabla();
}

function printTabla() {
    $("#dataVenta tbody").empty();
    $.each(oDetalle, function (key, value) {
        var valorRetorno = value.split(",");
        var data = "<tr><td>" + valorRetorno[5] + "</td>" +
            "<td>" + valorRetorno[0] + "</td>" +
            "<td>" + valorRetorno[1] + "</td>" +
            "<td>" + valorRetorno[2] + "</td>" +
            "<td><div class='tools'><a href='#' onclick=eliminaPrecio(" + key + ") class='btn btn-danger'><i class='fa fa-trash-o'></i></a></div></td>" +
            "</tr>";
        $("#dataVenta tbody").append(data);
    });
}

function alertaMessage(msj, evt) {
    var mensaje = msj;
    if (mensaje !== "") {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "rtl": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": 300,
            "hideDuration": 1000000000000,
            "timeOut": 50000000000,
            "extendedTimeOut": 10000000000,
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
        if (eve === "1") {
            toastr.success(mensaje, "Aviso del Sistema", 'Aviso del Sistema', { timeOut: 500000 });
        } else { toastr.error(mensaje, "Aviso del Sistema", 'Aviso del Sistema', { timeOut: 500000 }); }
        
    }
}

function alertaMessageBien(msj, evt) {
    var mensaje = msj;
    if (mensaje !== "") {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "rtl": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": 300,
            "hideDuration": 1000000000000,
            "timeOut": 50000000000,
            "extendedTimeOut": 10000000000,
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
            toastr.success(mensaje, "Aviso del Sistema", 'Aviso del Sistema', { timeOut: 500000 });
    }
}

function redireccionarPagina() {
    window.location = "../Tarifario/RegistrarTarifario";
}
