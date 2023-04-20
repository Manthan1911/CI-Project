//---------------- loaders or spinners ----------
$(document).ajaxStart(function () {
    $('#loader').show();
});

$(document).ajaxStop(function () {
    $('#loader').hide();
});


//---------------- aside bar --------------
$(".hamburger").click(() => {
    $(".sidebar").addClass("sidebar-open");
    $(".overlay").removeClass("d-none");
})
$(".btn-close").click(() => {
    $(".sidebar").removeClass("sidebar-open");
    $(".overlay").addClass("d-none");
})
$(".overlay").click(() => $(".btn-close").click())



//---------------- current time --------------

function updateCurrentDateTime() {
    let currentDate = new Date();
    document.getElementById("currentTime").textContent = currentDate.toLocaleDateString('en-US', { weekday: "long", year: "numeric", month: "long", day: "numeric", hour12: true, hour: "2-digit", minute: "2-digit" });
}
updateCurrentDateTime();
setInterval(updateCurrentDateTime, 1000);



// ----------------- fixing height of adminPagePartialContainer ----------------
setHeight();
function setHeight() {
    let navbarHeight = adminPageNavbar.offsetHeight;
    let partialContainerHeight = `calc(100% - ${navbarHeight}px)`;
    adminPagePartialContainer.style.height = partialContainerHeight;
}


// ------------------ click on sidebar item ----------------
ajaxCallForUserPartial();
const sidebarTabs = document.querySelectorAll("[data-item]");
sidebarTabs.forEach((tab) => {

    tab.addEventListener("click", (e) => {
        e.preventDefault();

        tab.classList.add("active-sidebar-item");
        tab.children[0].classList.add("icon-fill-orange");
        console.log(tab.classList);
        let tabToOpen = tab.getAttribute("data-item");

        switch (tabToOpen) {
            case "user":
                ajaxCallForUserPartial();
                break;
            default:
                ajaxCallForUserPartial();
                break;
        }
    });

});

//-------------------- User --------------------
function ajaxCallForUserPartial() {

    $.ajax({
        url: "/Admin/GetUserPartial",
        method: "GET",
        dataType: "html",
        success: (data) => {
            $('#adminPagePartialContainer').html(data);
            callAddUserPartial();
            deleteUserFromAdmin();
            restoreUserFromAdmin();
            editUserFromAdmin();
        },
        error: (error) => {
            Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'problem loading partial!',
                showConfirmButton: false,
                timer: 3000
            })

        }
    });
}

function callAddUserPartial() {
    $('#addUserBtn').on("click", () => {
        ajaxCallForAddUserPartial();
    });
}

function ajaxCallForAddUserPartial() {
    $.ajax({
        url: "/Admin/GetAddUserPartial",
        method: "GET",
        dataType: "html",
        success: (data) => {
            $('#adminPagePartialContainer').html(data);

            $('#addUserForm').on("submit", (e) => {
                e.preventDefault();
                let form = $('#addUserForm');
                if (isAdminFormValid(form)) {
                    let formData = $("#addUserForm").serialize();
                    console.log(formData);
                    $.ajax({
                        url: "/Admin/SaveUser",
                        method: "POST",
                        dataType: "html",
                        data: formData,
                        success: (data, _, status) => {

                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Saving User!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'User added successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            })

                            ajaxCallForUserPartial();

                        },
                        error: (error) => {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'problem loading partial!',
                                showConfirmButton: false,
                                timer: 3000
                            })
                            return;
                        }
                    });

                }
            });

            $('#addUserCancelBtn').on('click', (e) => {
                e.preventDefault();
                ajaxCallForUserPartial();
            })
        },
        error: (error) => {
            Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'problem loading partial!',
                showConfirmButton: false,
                timer: 3000
            })

        }
    });
}

function deleteUserFromAdmin() {

    let userDeleteBtns = document.querySelectorAll('.deleteUserBtn');

    userDeleteBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            let userId = btn.getAttribute('data-userId');

            Swal.fire({
                title: 'Are you sure you want to delete this user?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/DeleteUser",
                        method: "DELETE",
                        dataType: "html",
                        data: { "userId": userId },
                        success: (data, _, status) => {

                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Deleting User!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'User deleted successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            })

                            ajaxCallForUserPartial();

                        },
                        error: (error) => {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: '404 or 500 problem deleting user!',
                                showConfirmButton: false,
                                timer: 3000
                            })
                            return;
                        }
                    });

                }
            })


        })
    });
}

function restoreUserFromAdmin() {

    let userRestoreBtns = document.querySelectorAll('.restoreUserBtn');

    userRestoreBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            let userId = btn.getAttribute('data-restoreUserId');

            Swal.fire({
                title: 'Are you sure you want to restore this user?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, restore it!'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/RestoreUser",
                        method: "POST",
                        dataType: "html",
                        data: { "userId": userId },
                        success: (data, _, status) => {

                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Restoring User!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'User Restored successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            })

                            ajaxCallForUserPartial();

                        },
                        error: (error) => {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: '404 or 500 problem restoring user!',
                                showConfirmButton: false,
                                timer: 3000
                            })
                            return;
                        }
                    });

                }
            })


        })
    });
}

function ajaxCallForEditUserPartial(userId) {
    $.ajax({
        url: "/Admin/GetEditUserPartial",
        method: "GET",
        dataType: "html",
        data: { "userId": userId },
        success: (data) => {
            $('#adminPagePartialContainer').html(data);

            $('#editUserForm').on("submit", (e) => {
                e.preventDefault();
                let form = $('#editUserForm');
                if (isAdminFormValid(form)) {
                    let formData = form.serialize();
                    console.log(formData);
                    $.ajax({
                        url: "/Admin/EditUser",
                        method: "POST",
                        dataType: "html",
                        data: formData,
                        success: (data, _, status) => {

                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Saving Edited User!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'User Edited successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            })

                            ajaxCallForUserPartial();

                        },
                        error: (error) => {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'problem Saving Edited User!',
                                showConfirmButton: false,
                                timer: 3000
                            })
                            return;
                        }
                    });

                }
           

            

            });

            $('#editUserCancelBtn').on('click', (e) => {
                e.preventDefault();
                ajaxCallForUserPartial();
            })

        },
        error: (error) => {
            Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'problem loading editUser partial!',
                showConfirmButton: false,
                timer: 3000
            })

        }
    });
}

function editUserFromAdmin() {

    let editUserBtns = document.querySelectorAll(".editUserBtn");

    editUserBtns.forEach((button) => {
        button.addEventListener("click", (e) => {
            e.preventDefault();

            let userId = button.getAttribute("data-userId");
            ajaxCallForEditUserPartial(userId);

        });
    });
}


//--------------------



//--------------------

let isAdminFormValid = (form) => {
    if (!form.valid()) {
        return false;
    }
    return true;
}