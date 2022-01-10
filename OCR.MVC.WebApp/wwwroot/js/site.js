// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function Delete(url) {
    Swal.fire({
        title: "Data Akan Dihapus !",
        text: "Data akan dihapus dan tidak bisa dikembalikan",
        icon: "error",
        showCancelButton: true,
        confirmButtonColor: '#00653b',//'#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirm',
        buttons: true,
        dangerMode: true,
        //add textbox
        input: 'text',
        inputPlaceholder: 'Alasan',
        inputValidator: (value) => {
            if (!value) {
                return 'Alasan Harus Diisi'
            }
        }
    }).then((result) => {
        if (result.value) {
            DeleteData(url);
        }
    })
}

function success(value)
{
    toastr.options = {
        "positionClass": "toast-bottom-right"
    }
    toastr.success(value);
}

function alert(value) {
    toastr.options = {
        "positionClass": "toast-bottom-right"
    }
    toastr.error(value);
}