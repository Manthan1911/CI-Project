//$(document).ready(() => {
    $('#contactUsBtn').on("click", (e) => {
        e.preventDefault();

        $.ajax({
            url: "/FooterPages/GetContactUsPartial",
            method: "GET",
            dataType: "html",
            success: (data) => {

                $('#addContactUsPartialDiv').html("");
                $('#addContactUsPartialDiv').html(data);
                $('#contactUsModal').modal('show');
                addListnerToSubmitCntactUsForm();

            },
            error: (error) => {
                alert(error);
            }
        });
    })
//});

function addListnerToSubmitCntactUsForm() {
    $('#contactUsForm').on('submit', (e) => {
        e.preventDefault();

        let form = $('#contactUsForm');
        if (isFormValid(form)) {
            let formData = form.serialize();
            $.ajax({
                url: "/FooterPages/SaveContactUsDat",
                method: "POST",
                dataType: "html",
                data: formData,
                success: function (data, _, status) {

                    if (status.status == 204) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'error',
                            title: 'ContactUs data not saved !',
                            showConfirmButton: false,
                            timer: 3000
                        })
                        return;
                    }

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Message received Successfully!',
                        showConfirmButton: false,
                        timer: 3000
                    })
                    $('#contactUsModal').modal('hide');

                },
                error: function (error,_,thrownError) {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Problem in sending message',
                        showConfirmButton: false,
                        timer: 3000
                    })
                }
            });
        }
        else {
            Swal.fire({
                position: 'top-end',
                icon: 'warning',
                title: 'form not valid!',
                showConfirmButton: false,
                timer: 3000
            })
        }
    });
}


const isFormValid = (form) => {

    if (!form.valid()) {
        return false;
    }
    return true;

}
