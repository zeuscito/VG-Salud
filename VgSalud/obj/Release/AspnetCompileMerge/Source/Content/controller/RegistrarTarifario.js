var oPrecioTarifa = new Array();

$(document).ready(function () {

    $.ajax({
        url: '../Tarifario/ListaCategoriaPacientes',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            'precio': 1.1
        }), success: function (response) {
            $.each(response.listado, function (index, valores) {
                oPrecioTarifa[oPrecioTarifa.length] = valores.CodCatPac + "," + valores.DescCatPac + "," + 0;
            });
            printTabla();
            $('#example1 tbody').html(response.result);
        },
        error: function () {
            alertaMessage("No se pudo asignar los precios", 0);
        }
    });

    printTabla();

    $("#agregar").click(function () {
        var flag = 0;
        var CodCatPac = $("#CodCatPac").val();
        var CatPacText = $("#CodCatPac option:selected").text();
        var precio = $("#prec").val();

        $.each(oPrecioTarifa, function (key, value) {
            var valorRetorno = value.split(",");
            if (valorRetorno[0] === CodCatPac) {
                oPrecioTarifa[key] = CodCatPac + "," + CatPacText + "," + precio;
                printTabla();
                flag = 1;
            }
        });

        if (flag === 0) {
            if (!(CodCatPac)) {
                $.each(oPrecioTarifa, function (indice, value) {
                    var valorRetorno = value.split(",");
                    oPrecioTarifa[indice] = valorRetorno[0] + "," + valorRetorno[1] + "," + precio;
                });
            } else {
                $.each(oPrecioTarifa, function (indice, value) {
                    var valorRetorno = value.split(",");
                    if (valorRetorno[0] === CodCatPac) {
                        oPrecioTarifa[indice] = CodCatPac + "," + CatPacText + "," + precio;
                    }
                });
            }
            printTabla();
        }

    });

    $("#grabar").click(function () {
        var flag = 0;
        var descripcion = $("#descripcionTar").val();
        var costo = $("#costo").val();
        var duracion = $("#duracion").val();
        var especialidad = $("#especialidad").val();
        var tipoTar = $("#tipoTarifa").val();
        var subTipTar = $("#subTipoTarifa").val();
        var perfil = $("#PerfFicha").val();
        var cuentaConta = $("#idCuenta").val();
        var moneda = $("#moneda").val();
        var estado = $('#estado:checked').val() ? 1 : 0;
        var afecIGV = $('#afecIGV:checked').val() ? 1 : 0;
        var regDoc = $('#regDoc:checked').val() ? 1 : 0;
        var precMod = $('#precMod:checked').val() ? 1 : 0;

        if (!(descripcion && costo && duracion && especialidad && tipoTar && perfil && cuentaConta && moneda)) {
            alertaMessage("Llene los campos obligatorios",0);
            flag = 1;
            return;
        }

        if (flag === 0) {
            $.ajax({
                url: '../Tarifario/RegistrarTarifario',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    'descripcion': descripcion,
                    'costo': costo,
                    'duracion': duracion,
                    'especialidad': especialidad,
                    'tipoTar': tipoTar,
                    'subTipTar': subTipTar,
                    'perfil': perfil,
                    'cuentaConta': cuentaConta,
                    'moneda': moneda,
                    'estado': estado,
                    'afecIGV': afecIGV,
                    'regDoc': regDoc,
                    'precMod': precMod,
                    'arrayPrecios': oPrecioTarifa
                }), success: function (response) {
                    alertaMessageBien("La tarifa se guardo correctamente", "1");
                    setTimeout("redireccionarPagina()", 1500);
                },
                error: function () {
                    alertaMessage("No se pudo guardar la tarifa",0);
                }
            });

        }

    });

});

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
