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
            addListenerToTimeBasedEditButton(userId);
            addListenerToDeleteTimesheetButton(userId);

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
            addListenerToGoalBasedEditButton(userId);
            addListenerToDeleteTimesheetButton(userId);

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
                saveAddGoalData(userId);
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
                success: function (data, _, status) {

                    if (status.status == 204) {
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

function saveAddGoalData(userId) {
    $('#addGoalDataForm').on('submit', (e) => {
        console.log("goal form submit");
        e.preventDefault();
        let form = $('#addGoalDataForm');
        if (isFormValid(form)) {
            let formData = form.serialize();
            $.ajax({
                url: "/VolunteeringTimesheet/SaveGoalData",
                method: "POST",
                dataType: "html",
                data: formData,
                success: function (data, _, status) {

                    if (status.status == 204) {
                        $('#addGoalDateVolunteered').text("Your date should be between mission's start and end date!");
                        return;
                    }

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Timesheet data saved Successfully!',
                        showConfirmButton: false,
                        timer: 3000
                    })

                    $('#addGoalModal').modal('hide');

                    callGoalBasedPartial(userId);

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

function addListenerToTimeBasedEditButton(userId) {
    let timeBasedEditButtons = document.querySelectorAll(".editHour");

    timeBasedEditButtons.forEach((editButton) => {
        editButton.addEventListener("click", (e) => {
            e.preventDefault();
            let timesheetId = editButton.getAttribute('data-id');

            $.ajax({
                url: "/VolunteeringTimesheet/GetTimeBasedEditPartial",
                method: "GET",
                dataType: "html",
                data: { "userId": userId, "timesheetId": timesheetId },
                success: (data, _, status) => {

                    if (status.status == 204) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'error',
                            title: 'Data Not found to edit!',
                            showConfirmButton: false,
                            timer: 3000
                        })
                        return;
                    }

                    $('#openAnyModal').html("");
                    $('#openAnyModal').html(data);
                    $('#editTimeModal').modal('show');
                    editTimeData(userId);
                },
                error: (error) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'problem loading edit partial',
                        showConfirmButton: false,
                        timer: 3000
                    })
                }
            });

        });
    });
}

function addListenerToGoalBasedEditButton(userId) {
    let goalBasedEditButtons = document.querySelectorAll(".editGoal");

    goalBasedEditButtons.forEach((editButton) => {
        editButton.addEventListener("click", (e) => {
            e.preventDefault();
            let timesheetId = editButton.getAttribute('data-id');

            $.ajax({
                url: "/VolunteeringTimesheet/GetGoalBasedEditPartial",
                method: "GET",
                dataType: "html",
                data: { "userId": userId, "timesheetId": timesheetId },
                success: (data, _, status) => {

                    if (status.status == 204) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'error',
                            title: 'Data Not found to edit!',
                            showConfirmButton: false,
                            timer: 3000
                        })
                        return;
                    }

                    $('#openAnyModal').html("");
                    $('#openAnyModal').html(data);
                    $('#editGoalModal').modal('show');
                    editGoalData(userId);
                },
                error: (error) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'problem loading edit partial',
                        showConfirmButton: false,
                        timer: 3000
                    })
                }
            });

        });
    });
}

function editTimeData(userId) {
    $('#editTimeDataForm').on('submit', (e) => {
        console.log("submit");
        e.preventDefault();
        let form = $('#editTimeDataForm');
        if (isFormValid(form)) {
            let formData = form.serialize();
            $.ajax({
                url: "/VolunteeringTimesheet/EditTimeData",
                method: "PUT",
                dataType: "html",
                data: formData,
                success: function (data, _, status) {

                    if (status.status == 204) {
                        $('#editTimeFormDateVolunteered').text("Your date should be between mission's start and end date!");
                        return;
                    }

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Timesheet data Edited Successfully!',
                        showConfirmButton: false,
                        timer: 3000
                    })

                    $('#editTimeModal').modal('hide');

                    callTimeBasedPartial(userId);


                },
                error: function (error) {
                    alert("Error in Editing TimeData!");
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

function editGoalData(userId) {
    $('#editGoalDataForm').on('submit', (e) => {
        console.log("submit");
        e.preventDefault();
        let form = $('#editGoalDataForm');
        if (isFormValid(form)) {
            let formData = form.serialize();
            $.ajax({
                url: "/VolunteeringTimesheet/EditGoalData",
                method: "PUT",
                dataType: "html",
                data: formData,
                success: function (data, _, status) {

                    if (status.status == 204) {
                        $('#editGoalFormDateVolunteered').text("Your date should be between mission's start and end date!");
                        return;
                    }

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Timesheet data Edited Successfully!',
                        showConfirmButton: false,
                        timer: 3000
                    })

                    $('#editGoalModal').modal('hide');

                    callGoalBasedPartial(userId);


                },
                error: function (error) {
                    alert("Error in Editing TimeData!");
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

function addListenerToDeleteTimesheetButton(userId) {
    let deleteButtons = document.querySelectorAll('.deleteTimesheetDataBtn');
    console.log(deleteButtons);
    deleteButtons.forEach((deleteButton) => {
        deleteButton.addEventListener("click", (e) => {
            e.preventDefault();

            let timesheetId = deleteButton.getAttribute('data-id');
            console.log("timesheet id : " + timesheetId);

            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: "/VolunteeringTimesheet/DeleteTimeData",
                        method: "GET",
                        dataType: "html",
                        data: { "timesheetId": timesheetId },
                        success: (data, _, status) => {

                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Data Not found to delete!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Data deleted successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            })
                            callTimeBasedPartial(userId);
                            callGoalBasedPartial(userId);

                        },
                        error: (error) => {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'problem loading edit partial',
                                showConfirmButton: false,
                                timer: 3000
                            })
                        }
                    });
                }
            })
            
        });
    })
}


const isFormValid = (form) => {

    if (!form.valid()) {
        return false;
    }
    return true;

}



    //Swal.fire({
    //        position: 'top-end',
    //        icon: 'warning',
    //        title: 'OldPassword and NewPassword Cannot be same!',
    //        showConfirmButton: false,
    //        timer: 3000
    //    })