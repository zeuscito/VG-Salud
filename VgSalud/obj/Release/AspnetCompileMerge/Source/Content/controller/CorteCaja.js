$(document).ready(function () {

    $("#submitSend").click(function () {
        $body = $("body");
        var corte = $("#corte").val();
        var horaInicio = $("#horaInicio").val();
        var totalUsuario = $("#totalUsuario").val();

        $body.addClass("loading");
        $.ajax({
            url: '../Caja/CorteCaja',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                'totalUsuario': totalUsuario,
                'horaInicio': horaInicio,
                'corte': corte
            }), success: function (response) {
                if (response.success) {
                    $body.removeClass("loading");
                    alertaMessageBien("Corte registrado");
                    window.location.href = 'ListadoCaja';
                } else {
                    $body.removeClass("loading");
                    alertaMessage("No se pudo procesar la venta");
                }                
            },
            error: function () {
                $body.removeClass("loading");
                alertaMessage("No se pudo hacer el corte", 0);
            }
        });
    });

});


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

