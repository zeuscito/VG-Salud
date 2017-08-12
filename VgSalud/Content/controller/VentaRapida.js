var oCabecera = new Array();
var oDetalle = new Array();
var historia = $("#historia").val();

$(document).ready(function () {

    // Validar usuaario diferente a factura
    $.ajax({
        url: '../AtencionVarias/EvaluaTipoDocUsuario',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.success) {
                $("#submitSend").attr("disabled", false);
            } else {
                alertaMessage(response.result);
                $("#submitSend").attr("disabled", true);
            }
        },
        error: function () {
            alertaMessage("No se pudo validar datos", 0);
        }
    });

    $.ajax({
        url: '../AtencionVarias/CargaServicios',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            'historia': historia,
            'descripcion' : ''
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

    $("#inclSus").click(function () {
        var inclSus = $('#inclSus:checked').val() ? 1 : 0;
        if (inclSus === 1) {
            var tarEspecial = $('#tarEspecial').val();
            var valorTarifaEsoecial = tarEspecial.split(",");
            $("#tarifaDos").empty();
            $("#tarifaDos").html(valorTarifaEsoecial[4] + ": S/.<span>" + valorTarifaEsoecial[3] + "</span>");
        }
        else {
            $("#tarifaDos").empty();
            $("#tarifaDos").html("<br>");
        }
    });

    $("#submitSendXpress").click(function () {
        var flag = 0;
        var inclSus = $('#inclSus:checked').val() ? 1 : 0;
        var tarEspecial = $('#tarEspecial').val();
        var espeServ = $('input:radio[name=ckServicio]:checked').val();
        var valorServicio = espeServ.split(",");
        var valorTarifaEsoecial = tarEspecial.split(",");

        if (valorServicio[2].trim() === "") {
            alertaMessage("No existen tarifas asignadas a este servicio");
            flag = 1;
        }
        if (espeServ === undefined)
        {
            alertaMessage("Seleccione un servicio");
            flag = 1;
        }
        if (flag !== 1)
        {
            oCabecera.length = 0;
            oDetalle.length = 0;
            if (inclSus === 1)
            {
                var precioTotal = parseFloat(valorServicio[4]) + parseFloat(valorTarifaEsoecial[3]);
                oCabecera[oCabecera.length] = historia + "," + precioTotal;
                oDetalle[oDetalle.length] = valorServicio[2] + "," + 1 + "," + parseFloat(valorServicio[4]) + "," + valorServicio[0] + "," + valorServicio[1] + "," + valorServicio[3];
                oDetalle[oDetalle.length] = valorTarifaEsoecial[0] + "," + 1 + "," + parseFloat(valorTarifaEsoecial[3]) + "," + valorTarifaEsoecial[2] + "," + valorTarifaEsoecial[1] + "," + valorTarifaEsoecial[4];
            }else
            {
                oCabecera[oCabecera.length] = historia + "," + parseFloat(valorServicio[4]);
                oDetalle[oDetalle.length] = valorServicio[2] + "," + 1 + "," + parseFloat(valorServicio[4]) + "," + valorServicio[0] + "," + valorServicio[1] + "," + valorServicio[3];
            }
            $.ajax({
                url: '../AtencionVarias/RegistroVentaRapida',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    'cabecera': oCabecera,
                    'detalle': oDetalle
                }), success: function (response) {
                    if (response.success) {
                        oCabecera.length = 0;
                        oDetalle.length = 0;
                        printTabla();
                        setTimeout("redireccionarPagina(" + response.codigoCaja + ")", 1000);
                        
                    } else {
                        alertaMessage("No se pudo procesar la venta");
                    }
                },
                error: function () {
                    alertaMessage("No se pudo procesar la venta", 0);
                }
            });
        }
    });

    $("#submitSend").click(function () {
        if (oDetalle.length === 0) {
            alertaMessage("No cargo ninguna tarifa");
        } else
        {
            $.ajax({
                url: '../AtencionVarias/RegistroVentaRapida',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    'cabecera': oCabecera,
                    'detalle' : oDetalle
                }), success: function (response) {
                    if (response.success) {
                        oCabecera.length = 0;
                        oDetalle.length = 0;
                        printTabla();
                        setTimeout("redireccionarPagina(" + response.codigoCaja + ")", 1000);
                    } else
                    {
                        alertaMessage("No se pudo procesar la venta");
                    }
                },
                error: function () {
                    alertaMessage("No se pudo procesar la venta", 0);
                }
            });
        }
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
            var espeServ = $('input:radio[name=ckServicio]:checked').val();
            var valorRetorno = espeServ.split(",");
            $("#tarifaUno").empty();
            $("#tarifaUno").html(valorRetorno[5] + ": S/.<span>" + valorRetorno[4] + "</span>");
            oDetalle.length = 0;
            oCabecera.length = 0;
            $('input:radio[name=ckServicio]').attr("disabled", false);
            $('#dataTarifa tbody').empty();
            $('#dataTarifa tbody').html(response.result);
            printTabla();
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
    var descTrar = $("#descTrar-" + obj).val();
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
        oDetalle[oDetalle.length] = tarifa + "," + cantidad + "," + precio + "," + valorRetorno[0] + "," + valorRetorno[1] + "," + valorRetorno[3] + "," + descTrar;
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

$("#buscaServicio").keyup(function () {
    var value = $(this).val();
    $.ajax({
        url: '../AtencionVarias/CargaServicios',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            'historia': historia,
            'descripcion': value
        }),
        success: function (response) {

            $('#dataServicio tbody').html(response.result);
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
        var data = "<tr><td width='70%'>" + valorRetorno[6] + "</td>" +
            "<td width='10%'>" + valorRetorno[1] + "</td>" +
            "<td width='10%'>" + valorRetorno[2] + "</td>" +
            "<td width='10%'><div class='tools'><a href='#' onclick=eliminaPrecio(" + key + ") class='btn btn-danger'><i class='fa fa-trash-o'></i></a></div></td>" +
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

function redireccionarPagina(codCaja) {
    window.location = "../Caja/ImprimirTicket?CodCaja=" + codCaja + "";
}
