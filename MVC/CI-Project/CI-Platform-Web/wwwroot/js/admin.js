//---------------- loaders or spinners ----------
$(document).ajaxStart(function () {
    $("#loader").removeClass("d-none");
    setHeight(loader);
    $('#adminPagePartialContainer').addClass('d-none');
});

$(document).ajaxStop(function () {
    $("#loader").addClass("d-none");
    $('#adminPagePartialContainer').removeClass('d-none');
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
setHeight(adminPagePartialContainer);
function setHeight(element) {
    let navbarHeight = adminPageNavbar.offsetHeight;
    let partialContainerHeight = `calc(100% - ${navbarHeight}px)`;
    element.style.height = partialContainerHeight;
}


// ------------------ click on sidebar item ----------------

let url = "/Admin/GetUserPartial";
let tabToOpen = "user";


const sidebarTabs = document.querySelectorAll("[data-item]");

//-------------------- 
const handleSidebarClassOnClick = (tabToAddActiveClass) => {
    sidebarTabs.forEach((currTab) => {
        let tabName = currTab.getAttribute("data-item");
        if (tabName == tabToAddActiveClass) {
            currTab.classList.add("active-sidebar-item");
            currTab.children[0].classList.add("icon-fill-orange");
        }
        else {
            currTab.classList.remove("active-sidebar-item");
            currTab.children[0].classList.remove("icon-fill-orange");
        }
    });
}

$('#defaultTab').click();
sidebarTabs.forEach((tab) => {

    tab.addEventListener("click", (e) => {
        e.preventDefault();
        tinymce.remove("textarea#tiny");

        tabToOpen = tab.getAttribute("data-item");

        handleSidebarClassOnClick(tabToOpen);

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
                url = "/Admin/GetSkillsPartial";
                ajaxCallForAdminPartial(url, tabToOpen);
                break;
            case "application":
                url = "/Admin/GetMissionApplicationsPartial";
                ajaxCallForAdminPartial(url, tabToOpen);
                break;
            case "theme":
                url = "/Admin/GetMissionThemePartial";
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
                case "skill":
                    callAddSkillPartial();
                    callEditSkillPartial();
                    deleteSkillFromAdmin();
                    restoreSkillFromAdmin();
                    break;
                case "application":
                    acceptMissionApplication();
                    declineMissionApplication();
                    break;
                case "theme":
                    callAddThemePartial();
                    callEditThemePartial();
                    deleteThemeFromAdmin();
                    restoreThemeFromAdmin();
                    break;
                default:
                    callAddUserPartial();
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
                        method: "POST",
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

//-------------------- Skill --------------------

const callAddSkillPartial = () => {
    $('#addSkillBtn').on("click", (e) => {
        e.preventDefault();

        $.ajax({
            url: "/Admin/GetAddSkillsPartial",
            method: "GET",
            success: (data) => {
                $('#adminPagePartialContainer').html(data);

                $('#addSkillForm').on("submit", (e) => {
                    e.preventDefault();
                    let form = $('#addSkillForm');
                    if (isAdminFormValid(form)) {

                        let formData = form.serialize();

                        $.ajax({
                            url: "/Admin/AddSkill",
                            method: "POST",
                            data: formData,
                            success: (data, _, status) => {

                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'success',
                                    title: 'Skill Added successfully!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })

                                ajaxCallForAdminPartial(url, tabToOpen);

                            },
                            error: (error) => {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Adding Skill!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }
                        });
                    }
                });

                $("#addSkillCancelBtn").on("click", (e) => {
                    e.preventDefault();
                    ajaxCallForAdminPartial(url, tabToOpen);
                });
            },
            error: (error) => {
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: 'problem loading Add Skill partial!',
                    showConfirmButton: false,
                    timer: 3000
                })

            }
        });

    });
};

const callEditSkillPartial = () => {
    let editSkillBtns = document.querySelectorAll(".editSkillBtn");
    editSkillBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            let skillId = btn.getAttribute("data-skillId");
            console.log(skillId);

            $.ajax({
                url: "/Admin/GetEditSkillsPartial",
                method: "GET",
                data: { "skillId": skillId },
                success: (data) => {
                    $('#adminPagePartialContainer').html("");
                    $('#adminPagePartialContainer').html(data);

                    $('#editSkillForm').on("submit", (e) => {
                        e.preventDefault();
                        let form = $('#editSkillForm');
                        if (isAdminFormValid(form)) {

                            let formData = form.serialize();

                            $.ajax({
                                url: "/Admin/EditSkill",
                                method: "POST",
                                data: formData,
                                success: (data, _, status) => {

                                    Swal.fire({
                                        position: 'top-end',
                                        icon: 'success',
                                        title: 'Skill Edited successfully!',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })

                                    ajaxCallForAdminPartial(url, tabToOpen);

                                },
                                error: (error) => {
                                    Swal.fire({
                                        position: 'top-end',
                                        icon: 'error',
                                        title: 'Error Editing Skill!',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })
                                    return;
                                }
                            });
                        }
                    });

                    $("#editSkillCancelBtn").on("click", (e) => {
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

const deleteSkillFromAdmin = () => {
    let deleteSkillBtns = document.querySelectorAll(".deleteSkillBtn");
    deleteSkillBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            let skillId = btn.getAttribute("data-skillId");

            Swal.fire({
                title: 'Do you want to really want to delete this skill?',
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
                        url: "/Admin/SoftDeleteSkill",
                        method: "POST",
                        data: { "skillId": skillId },
                        success: function (data, _, status) {


                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Skill can\'t be soft deleted as it is already in use!',
                                    showConfirmButton: false,
                                    timer: 3000
                                });

                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Soft Deleted Skill!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Soft Deleting Skill!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        }
                    });

                } else if (result.isDenied) {
                    $.ajax({
                        url: "/Admin/HardDeleteSkill",
                        method: "DELETE",
                        data: { "skillId": skillId },
                        success: function (data, _, status) {
                            console.log(data)
                            console.log(status)
                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Skill can\'t be deleted as it is already in use!',
                                    showConfirmButton: false,
                                    timer: 3000
                                });

                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: data,
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);

                        },
                        error: function (error) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Hard Deleting Skill!',
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

const restoreSkillFromAdmin = () => {

    let restoreSkillBtns = document.querySelectorAll(".restoreSkillBtn");
    restoreSkillBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            Swal.fire({
                title: 'Are you sure you want to restore this Skill?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, restore it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    let skillId = btn.getAttribute("data-skillId");
                    $.ajax({
                        url: "/Admin/RestoreSkill",
                        method: "POST",
                        data: { "skillId": skillId },
                        success: function (data, _, status) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Restored Skill successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);

                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Restoring Skill!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        }
                    });
                }
            })

        });
    });

}

//-------------------- Mission Application --------------------

const acceptMissionApplication = () => {
    let approveBtns = document.querySelectorAll('.approveMissionApplicationBtn');
    approveBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            let missionApplicationId = btn.getAttribute("data-missionApplicationId");

            $.ajax({
                url: "/Admin/ApproveMissionApplication",
                method: "POST",
                data: { "missionApplicationId": missionApplicationId },
                success: (data, _, status) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: data,
                        showConfirmButton: false,
                        timer: 3000
                    });

                    ajaxCallForAdminPartial(url, tabToOpen);
                }
                ,
                error: (error, _, status) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Error Approving Application request !',
                        showConfirmButton: false,
                        timer: 3000
                    });
                }
            });

        });
    });
}

const declineMissionApplication = () => {
    let declineBtns = document.querySelectorAll('.declineMissionApplicationBtn');
    declineBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            let missionApplicationId = btn.getAttribute("data-missionApplicationId");

            $.ajax({
                url: "/Admin/DeclineMissionApplication",
                method: "POST",
                data: { "missionApplicationId": missionApplicationId },
                success: (data, _, status) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: data,
                        showConfirmButton: false,
                        timer: 3000
                    });

                    ajaxCallForAdminPartial(url, tabToOpen);
                }
                ,
                error: (error, _, status) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Error Declining Application request!',
                        showConfirmButton: false,
                        timer: 3000
                    });
                }
            });

        });
    });
}


//-------------------- Mission Theeme ----------------------

const callAddThemePartial = () => {
    $('#addThemeBtn').on("click", (e) => {
        e.preventDefault();

        $.ajax({
            url: "/Admin/GetAddMissionThemePartial",
            method: "GET",
            success: (data) => {
                $('#adminPagePartialContainer').html(data);

                $('#addThemeForm').on("submit", (e) => {
                    e.preventDefault();
                    let form = $('#addThemeForm');
                    if (isAdminFormValid(form)) {

                        let formData = form.serialize();

                        $.ajax({
                            url: "/Admin/AddTheme",
                            method: "POST",
                            data: formData,
                            success: (data, _, status) => {

                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'success',
                                    title: 'Theme Added successfully!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })

                                ajaxCallForAdminPartial(url, tabToOpen);

                            },
                            error: (error) => {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Adding theme!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }
                        });
                    }
                });

                $("#addThemeCancelBtn").on("click", (e) => {
                    e.preventDefault();
                    ajaxCallForAdminPartial(url, tabToOpen);
                });
            },
            error: (error) => {
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: 'problem loading Add Theme partial!',
                    showConfirmButton: false,
                    timer: 3000
                })

            }
        });

    });
}

const callEditThemePartial = () => {
    let editThemeBtns = document.querySelectorAll(".editThemeBtn");
    editThemeBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            let themeId = btn.getAttribute("data-missionThemeId");

            $.ajax({
                url: "/Admin/GetEditMissionThemePartial",
                method: "GET",
                data: { "themeId": themeId },
                success: (data) => {
                    $('#adminPagePartialContainer').html("");
                    $('#adminPagePartialContainer').html(data);

                    $('#editThemeForm').on("submit", (e) => {
                        e.preventDefault();
                        let form = $('#editThemeForm');
                        if (isAdminFormValid(form)) {

                            let formData = form.serialize();

                            $.ajax({
                                url: "/Admin/EditMissionTheme",
                                method: "POST",
                                data: formData,
                                success: (data, _, status) => {

                                    Swal.fire({
                                        position: 'top-end',
                                        icon: 'success',
                                        title: 'Theme Edited successfully!',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })

                                    ajaxCallForAdminPartial(url, tabToOpen);

                                },
                                error: (error) => {
                                    Swal.fire({
                                        position: 'top-end',
                                        icon: 'error',
                                        title: 'Error Editing Theme!',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })
                                    return;
                                }
                            });
                        }
                    });

                    $("#editThemeCancelBtn").on("click", (e) => {
                        e.preventDefault();

                        ajaxCallForAdminPartial(url, tabToOpen);

                    });
                },
                error: (error) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'problem loading Edit Theme!',
                        showConfirmButton: false,
                        timer: 3000
                    });

                }

            });

        });
    });
};

const deleteThemeFromAdmin = () => {
    let deleteThemeBtns = document.querySelectorAll(".deleteThemeBtn");
    deleteThemeBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();
            let themeId = btn.getAttribute("data-missionThemeId");
            Swal.fire({
                title: 'Do you want to really want to delete this theme?',
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
                        url: "/Admin/SoftDeleteMissionTheme",
                        method: "POST",
                        data: { "themeId": themeId },
                        success: function (data, _, status) {


                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Theme can\'t be soft deleted as it is already in use!',
                                    showConfirmButton: false,
                                    timer: 3000
                                });

                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Soft Deleted Theme!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Soft Deleting Theme!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        }
                    });

                } else if (result.isDenied) {
                    $.ajax({
                        url: "/Admin/HardDeleteMissionTheme",
                        method: "DELETE",
                        data: { "themeId": themeId },
                        success: function (data, _, status) {
                            console.log(data)
                            console.log(status)
                            if (status.status == 204) {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Theme can\'t be deleted as it is already in use!',
                                    showConfirmButton: false,
                                    timer: 3000
                                });

                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: data,
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);

                        },
                        error: function (error) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Hard Deleting theme!',
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

const restoreThemeFromAdmin = () => {
    let restoreThemeBtns = document.querySelectorAll(".restoreThemeBtn");
    restoreThemeBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            Swal.fire({
                title: 'Are you sure you want to restore this Theme?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, restore it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    let themeId = btn.getAttribute("data-missionThemeId");
                    $.ajax({
                        url: "/Admin/RestoreMissionTheme",
                        method: "POST",
                        data: { "themeId": themeId },
                        success: function (data, _, status) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Restored Theme successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);

                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Restoring Theme!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        }
                    });
                }
            })

        });
    });
}

//--------------------

let isAdminFormValid = (form) => {
    if (!form.valid()) {
        return false;
    }
    return true;
}