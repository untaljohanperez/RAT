/// <reference path="sweetalert.min.js" />

var hayArchivo = false;
function abrirFileUpload() {
    $("input[type='file']").click();
}

function ponerRutaArchivoEnCliente(ruta) {
    validacionHayArchivo(ruta);
    if (hayArchivo) {
        $("#txtRutaArchivo").text("");
        $("#txtRutaArchivo").text(ruta);
        $(".lblNombreArchivo").text("");
        $(".lblNombreArchivo").text(ruta);
    } else {
        $("#txtRutaArchivo").text('No ha seleccionado ningun archivo');
        sweetAlert('No ha seleccionado ningun archivo');
    }
}

function validacionHayArchivo(ruta) {
    if (ruta.length > 0) {
        hayArchivo = true;
    } else {
        hayArchivo = false;
    }

}

function cargarArchivo() {
    console.log(hayArchivo);
    if (hayArchivo) {
        console.log('Hay archivo');
        $('#prog').fadeIn();
        $('#btnCargarArchivo').click();
    } else {
        console.log('Seleccione un archivo');
        sweetAlert('Seleccione un archivo');
    }
}

function mostrarGrid() {
    $('.gvArchivoPlano').removeClass('hide');
    $('.pnlInformacion').removeClass('hide');
    $('.lblNombreArchivo').removeClass('hide');
}

function mostrarAlerta(msj) {
    sweetAlert(msj)
}

function ImportarArchivo() {
    console.log('ImportarArchivo');
    $('#btnImportarArchivo').click();
    console.log('ImportarArchivo');
}

function MostrarResultadoProceso() {
    //$('.gvResultadoProceso').removeClass('hide');
    $('.wellResultadoProceso').fadeIn();
    sweetAlert('Importación exitosa');
}

/* $(document).on("ready", */

function animacionGrid() {

    $("tbody>*").on('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass("slideInUp animated");
        $(this).next().addClass("slideInUp animated");
        $(this).next().removeClass("hide");
    });
    $("tbody>*:first-child").addClass("slideInUp animated").removeClass("hide");
}/*);*/