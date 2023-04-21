//---------------- loaders or spinners ----------
//$(document).ajaxStart(function () {
//    setTimeout(() => {
//        let preloader = `<div id="loader" class="d-flex align-items-center justify-content-center h-100 w-100">
//			</div>`;
//        $('#loader').append('<img src="~/images/preloader.gif"/>');
//        $('#adminPagePartialContainer').html("");
//        $('#adminPagePartialContainer').html(preloader);
//        $('#loader').show();
//    },3000);
//});

//$(document).ajaxStop(function () {
//    $('#loader').hide();
//});


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

let url = "/Admin/GetUserPartial";
let tabToOpen = "user";

//ajaxCallForAdminPartial(url,tabToOpen);

const sidebarTabs = document.querySelectorAll("[data-item]");
sidebarTabs.forEach((tab) => {

    tab.addEventListener("click", (e) => {
        e.preventDefault();
        tinymce.remove("textarea#tiny");

        tab.classList.add("active-sidebar-item");
        tab.children[0].classList.add("icon-fill-orange");
        console.log(tab.classList);
        tabToOpen = tab.getAttribute("data-item");

        switch (tabToOpen) {
            case "user":
                url = "/Admin/GetUserPartial";
                ajaxCallForAdminPartial(url, tabToOpen);
                break;
            case "cms":
                url = "/Admin/GetCmsPartial";
                ajaxCallForAdminPartial(url, tabToOpen);
                break;
            case "skill":
                url = "/Admin/GetSkillPartial";
                ajaxCallForAdminPartial(url, tabToOpen);
                break;
            default:
                url = "/Admin/GetUserPartial";
                ajaxCallForAdminPartial(url, tabToOpen);
                break;
        }
    });

});

//-------------------- User --------------------

function ajaxCallForAdminPartial(url, tabToOpen) {
    tinymce.remove("textarea#tiny");
    $.ajax({
        url: url,
        method: "GET",
        dataType: "html",
        success: (data) => {
            $('#adminPagePartialContainer').html(data);

            switch (tabToOpen) {
                case "user":
                    callAddUserPartial();
                    deleteUserFromAdmin();
                    restoreUserFromAdmin();
                    editUserFromAdmin();
                    break;
                case "cms":
                    callAddCmsPartial();
                    callEditCmsPartial();
                    deleteCmsPageFromAdmin();
                    restoreCmsPageFromAdmin();
                    break;
                default:

                    break;
            }
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

                            ajaxCallForAdminPartial(url, tabToOpen);

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
                ajaxCallForAdminPartial(url, tabToOpen);
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

                            ajaxCallForAdminPartial(url, tabToOpen);

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

                            ajaxCallForAdminPartial(url, tabToOpen);

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

                            ajaxCallForAdminPartial(url, tabToOpen);

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
                ajaxCallForAdminPartial(url, tabToOpen);
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


//-------------------- CMS --------------------


const callAddCmsPartial = () => {
    $('#addCmsBtn').on('click', (e) => {
        e.preventDefault();
        $.ajax({
            url: "/Admin/GetAddCmsPartial",
            method: "GET",
            success: (data) => {
                $('#adminPagePartialContainer').html(data);
                $.getScript("/js/cmsTiny.js");
                $('#addCmsForm').on("submit", (e) => {
                    e.preventDefault();
                    let form = $('#addCmsForm');
                    if (isAdminFormValid(form)) {

                        let title = $("#cmsTitle").val();
                        let description = tinymce.get("tiny").getContent();
                        let slug = $("#cmsSlug").val();
                        let status = $("#cmsStatus").val();

                        const cmsModel = {
                            title: title,
                            description: description,
                            slug: slug,
                            status: status,
                        };

                        console.log(cmsModel);

                        $.ajax({
                            url: "/Admin/AddCmsPage",
                            method: "POST",
                            data: cmsModel,
                            success: (data, _, status) => {

                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'success',
                                    title: 'Cms Page Added successfully!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })

                                ajaxCallForAdminPartial(url, tabToOpen);

                            },
                            error: (error) => {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Adding Cms Page!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }
                        });
                    }
                });

                $("#addCmsCancelBtn").on("click", (e) => {
                    e.preventDefault();
                    ajaxCallForAdminPartial(url, tabToOpen);
                });
            },
            error: (error) => {
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: 'problem loading Add CMS partial!',
                    showConfirmButton: false,
                    timer: 3000
                })

            }
        });

        
    });
};

const callEditCmsPartial = () => {
    let editCmsPageBtns = document.querySelectorAll(".editCmsBtn");
    editCmsPageBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            console.log(btn);
            let cmsPageId = btn.getAttribute("data-editCmsPageId");
            console.log(cmsPageId);
            
            $.ajax({
                url: "/Admin/GetEditCmsPartial",
                method: "GET",
                data: { "cmsPageId": cmsPageId }, 
                success: (data) => {
                    $('#adminPagePartialContainer').html("");
                    $('#adminPagePartialContainer').html(data);

                    $.getScript("/js/cmsTiny.js");

                    $('#editCmsForm').on("submit", (e) => {
                        e.preventDefault();
                        let form = $('#editCmsForm');
                        if (isAdminFormValid(form)) {

                            let id = $("#cmsPageId").val();
                            let title = $("#cmsTitle").val();
                            let description = tinymce.get("tiny").getContent();
                            let slug = $("#cmsSlug").val();
                            let status = $("#cmsStatus").val();

                            const cmsModel = {
                                id: id,
                                title: title,
                                description: description,
                                slug: slug,
                                status: status,
                            };

                            console.log(cmsModel);

                            $.ajax({
                                url: "/Admin/EditCmsPage",
                                method: "POST",
                                data: cmsModel,
                                success: (data, _, status) => {

                                    Swal.fire({
                                        position: 'top-end',
                                        icon: 'success',
                                        title: 'Cms Page Added successfully!',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })

                                    ajaxCallForAdminPartial(url, tabToOpen);

                                },
                                error: (error) => {
                                    Swal.fire({
                                        position: 'top-end',
                                        icon: 'error',
                                        title: 'Error Adding Cms Page!',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })
                                    return;
                                }
                            });
                        }
                    });

                    $("#editCmsCancelBtn").on("click", (e) => {
                        e.preventDefault();

                        ajaxCallForAdminPartial(url, tabToOpen);

                    });
                },
                error: (error) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'problem loading Edit CMS partial!',
                        showConfirmButton: false,
                        timer: 3000
                    });

                }

            });

        });
    });
};

const deleteCmsPageFromAdmin = () => {
    let deleteCmsPageBtns = document.querySelectorAll(".deleteCmsBtn");
    deleteCmsPageBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            let cmsPageId = btn.getAttribute("data-editCmsPageId");

            Swal.fire({
                title: 'Do you want to save the changes?',
                showDenyButton: true,
                showCancelButton: true,
                confirmButtonText: 'Soft Delete',
                denyButtonText: `Delete`,
                confirmButtonColor: '#5cb85c',
                cancelButtonColor: '#3085d6',
                denyButtonColor: '#d33',
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/SoftDeleteCmsPage",
                        method: "POST",
                        data: { "cmsPageId": cmsPageId },
                        success: function (data) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Soft Deleted CMS Page!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Soft Deleting CMS Page!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        }
                    });

                } else if (result.isDenied) {
                    $.ajax({
                        url: "/Admin/HardDeleteCmsPage",
                        method: "DELETE",
                        data: { "cmsPageId": cmsPageId },
                        success: function (data) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Deleted CMS Page!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);

                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Hard Deleting CMS Page!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        }
                    });
                }
            })

        });
    });
};

const restoreCmsPageFromAdmin = () => {
    let restoreCmsPageBtns = document.querySelectorAll(".restoreCmsBtn");
    restoreCmsPageBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            Swal.fire({
                title: 'Are you sure you want to restore this CMS page?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, restore it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    let cmsPageId = btn.getAttribute("data-editCmsPageId");
                    $.ajax({
                        url: "/Admin/RestoreCmsPage",
                        method: "DELETE",
                        data: { "cmsPageId": cmsPageId },
                        success: function (data) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Restored CMS page successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);

                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Restoring CMS Page!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        }
                    });
                }
            })
           
        });
    });
};

//-------------------- CMS --------------------

    //--------------------

    let isAdminFormValid = (form) => {
        if (!form.valid()) {
            return false;
        }
        return true;
    }