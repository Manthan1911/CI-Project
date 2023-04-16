$(document).ready((e) => {
    let userId = $('#userId').val();

    callTimeBasedPartial(userId);
    callGoalBasedPartial(userId);
});

function callTimeBasedPartial(userId) {
    $.ajax({
        url: "/VolunteeringTimesheet/GetTimeBasedPartialView",
        method: "GET",
        dataType: "html",
        data: { "userId": userId },
        success: function (data) {

            $('#timeBasedPartialDiv').html("");
            $('#timeBasedPartialDiv').html(data);
            openTimeBasedModal(userId);
        },
        error: function (error) {
            alert("time model not loaded!");
        }
    });
}

function callGoalBasedPartial(userId) {
    $.ajax({
        url: "/VolunteeringTimesheet/GetGoalBasedPartialView",
        method: "GET",
        dataType: "html",
        data: { "userId": userId },
        success: function (data) {

            $('#goalBasedPartialDiv').html("");
            $('#goalBasedPartialDiv').html(data);
            openGoalBasedModal(userId);

        },
        error: function (error) {
            alert("goal model not loaded!");
        }
    });
}

function openTimeBasedModal(userId) {
    console.log('userId' + userId);

    $('#openTimeModalBtn').on('click', () => {
        $.ajax({
            url: "/VolunteeringTimesheet/GetAddTimeModalPartial",
            method: "GET",
            dataType: "html",
            data: { "userId": userId },
            success: function (data) {

                $('#openAnyModal').html("");
                $('#openAnyModal').html(data);
                $('#addTimeModal').modal('show');
                saveAddTimeData(userId);
            },
            error: function (error) {
                alert("Add Time modal not loaded!");
            }
        });
    });
}

function openGoalBasedModal(userId) {
    console.log('userId' + userId);

    $('#openGoalModalBtn').on('click', () => {
        $.ajax({
            url: "/VolunteeringTimesheet/GetAddGoalModalPartial",
            method: "GET",
            dataType: "html",
            data: { "userId": userId },
            success: function (data) {

                $('#openAnyModal').html("");
                $('#openAnyModal').html(data);
                $('#addGoalModal').modal('show');

            },
            error: function (error) {
                alert("Add Goal modal not loaded!");
            }
        });
    });
}

function saveAddTimeData(userId) {
    $('#addTimeDataForm').on('submit', (e) => {
        console.log("submit");
        e.preventDefault();
        let form = $('#addTimeDataForm');
        if (isFormValid(form)) {
            let formData = form.serialize();
            $.ajax({
                url: "/VolunteeringTimesheet/SaveTimeData",
                method: "POST",
                dataType: "html",
                data: formData,
                success: function (data,_,status) {

                    if (status.status == 204)
                    {
                        $('#timeDateVolunteered').text("Your date should be between mission's start and end date!");
                        return;
                    }

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Timesheet data saved Successfully!',
                        showConfirmButton: false,
                        timer: 3000
                    })

                    $('#addTimeModal').modal('hide');

                    callTimeBasedPartial(userId);

                   
                },
                error: function (error) {
                    alert("Error in saving TimeData!");
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
    //Swal.fire({
    //        position: 'top-end',
    //        icon: 'warning',
    //        title: 'OldPassword and NewPassword Cannot be same!',
    //        showConfirmButton: false,
    //        timer: 3000
    //    })

}