var oPrecioTarifa = new Array();

$(document).ready(function () {

    $.ajax({
        url: '../AtencionVarias/CargaServicios',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            $('#dataServicio tbody').html(response.result);
        },
        error: function () {
            alertaMessage("No se pudo generar los servicios", 0);
        }
    });  

});

function generaTarifa(e) {
    $.ajax({
        url: '../AtencionVarias/CargaServicios',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            $('#dataServicio tbody').html(response.result);
        },
        error: function () {
            alertaMessage("No se pudo generar los servicios", 0);
        }
    });
}


function eliminaPrecio(e) {
    var valorRetorno = oPrecioTarifa[e].split(",");
    oPrecioTarifa[e] = valorRetorno[0] + "," + valorRetorno[1] + "," + 0;
    printTabla();
}

function printTabla() {
    $("#example1 tbody").empty();
    $.each(oPrecioTarifa, function (key, value) {
        var valorRetorno = value.split(",");
        var data = "<tr><td>" + valorRetorno[0] + "</td>" +
            "<td>" + valorRetorno[1] + "</td>" +
            "<td>" + valorRetorno[2] + "</td>" +
            "<td align='center'><div class='tools'><a href='#' onclick=eliminaPrecio(" + key + ") class='btn btn-danger'><i class='glyphicon glyphicon-refresh'></i></a></div></td>" +
            "</tr>";
        $("#example1 tbody").append(data);
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
