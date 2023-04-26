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
            case "mission":
                url = "/Admin/GetMissionPartial";
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
                case "mission":
                    callAddTimeMissionPartial();
                    callAddGoalMissionPartial();
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
                title: 'Are you sure you want to Deactivate this user?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Deactivate'
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
                                    title: 'Error Deactivating User!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'User Deactivated successfully!',
                                showConfirmButton: false,
                                timer: 3000
                            })

                            ajaxCallForAdminPartial(url, tabToOpen);

                        },
                        error: (error) => {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: '404 or 500 problem Deactivating user!',
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
                confirmButtonText: 'Deactivate',
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
                                title: 'Deactivated CMS Page!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Deactivating CMS Page!',
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
                confirmButtonText: 'Deactivate',
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
                                    title: 'Skill can\'t be Deactivated as it is already in use!',
                                    showConfirmButton: false,
                                    timer: 3000
                                });

                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Deactivated Skill!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Deactivating Skill!',
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
                                title: 'Error Deleting Skill!',
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
                        title: 'Error Declining Mission Application request!',
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
                confirmButtonText: 'Deactivate',
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
                                    title: 'Theme can\'t be Deactivated as it is already in use!',
                                    showConfirmButton: false,
                                    timer: 3000
                                });

                                return;
                            }

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Deactivated Theme!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Deactivating Theme!',
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
                                title: 'Error Deleting theme!',
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

//-------------------- Mission Theeme ----------------------
const changeCityListAccordingToCountry = () => {
    let countryDropdown = document.getElementById('countryDropdown');
    let cityDropdown = document.getElementById('cityDropdown');

    countryDropdown.addEventListener("change", (e) => {
        e.preventDefault();

        let countryId = countryDropdown.value;
        let cityList = cityDropdown.children;

        for (let i = 0; i < cityList.length; i++) {
            cityList[i].classList.remove("d-none");
        }

        for (let i = 0; i < cityList.length; i++) {
            let cityId = cityList[i].getAttribute("data-countryId");
            if (cityId != countryId) {
                cityList[i].classList.add("d-none");
            }
        }

    });

};

const callAddTimeMissionPartial = () => {
    $('#addTimeBtn').on("click", (e) => {
        e.preventDefault();

        $.ajax({
            url: "/Admin/GetTimeMissionPartial",
            method: "GET",
            success: (data) => {
                $('#adminPagePartialContainer').html(data);
                $.getScript("/js/cmsTiny.js");
                changeCityListAccordingToCountry();

                $('#addTimeMissionForm').on("submit", (e) => {
                    e.preventDefault();
                    let form = $('#addTimeMissionForm');

                    saveFilesArrToInput();

                    if (isAdminFormValid(form) && AreAdminMissionFormTimeFieldsValid()) {

                        const formData = new FormData(form[0]);
                        console.log(formData);
                        formData.set("Description", tinymce.get('tiny').getContent());

                        $.ajax({
                            url: "/Admin/AddTimeMission",
                            method: "POST",
                            processData: false,
                            contentType: false,
                            data: formData,
                            success: (data, _, status) => {

                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'success',
                                    title: 'Time Mission Added successfully!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })

                                ajaxCallForAdminPartial(url, tabToOpen);

                            },
                            error: (error) => {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Adding Time Mission!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }
                        });
                    }
                });

                $("#addTimeMissionCancelBtn").on("click", (e) => {
                    e.preventDefault();
                    ajaxCallForAdminPartial(url, tabToOpen);
                });

                adminPreviewImage();
                previewDocuments();
            },
            error: (error) => {
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: 'problem loading Add Time Mission partial!',
                    showConfirmButton: false,
                    timer: 3000
                })

            }
        });

    });
}

const callAddGoalMissionPartial = () => {
    $('#addGoalBtn').on("click", (e) => {
        e.preventDefault();

        $.ajax({
            url: "/Admin/GetGoalMissionPartial",
            method: "GET",
            success: (data) => {
                $('#adminPagePartialContainer').html("");
                $('#adminPagePartialContainer').html(data);
                $.getScript("/js/cmsTiny.js");

                $('#addGoalMissionForm').on("submit", (e) => {
                    e.preventDefault();
                    let form = $('#addGoalMissionForm');

                    saveFilesArrToInput();

                    if (isAdminFormValid(form) && AreAdminMissionFormTimeFieldsValid()) {

                        const formData = new FormData(form[0]);
                        console.log(formData);
                        formData.set("Description", tinymce.get('tiny').getContent());

                        $.ajax({
                            url: "/Admin/AddGoalMission",
                            method: "POST",
                            processData: false,
                            contentType: false,
                            data: formData,
                            success: (data, _, status) => {

                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'success',
                                    title: 'Goal Mission Added successfully!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })

                                ajaxCallForAdminPartial(url, tabToOpen);

                            },
                            error: (error) => {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Adding Goal Mission!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }
                        });
                    }
                });

                $("#addGoalMissionCancelBtn").on("click", (e) => {
                    e.preventDefault();
                    ajaxCallForAdminPartial(url, tabToOpen);
                });

                adminPreviewImage();
                previewDocuments();
                changeCityListAccordingToCountry();
            },
            error: (error) => {
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: 'problem loading Add Goal Mission partial!',
                    showConfirmButton: false,
                    timer: 3000
                })

            }
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

// -------------------------------------
let filesArr = [];
let dragDiv;
let inputFile;
let imagePreviewDiv;
let documentsInput;
let selectedDocuments;

function adminPreviewImage() {

    filesArr = [];
    dragDiv = document.getElementById('dragDiv');
    inputFile = document.getElementById('inputFile');
    imagePreviewDiv = document.getElementById('imagePreviewDiv');
    //let isStoryDraft = document.getElementById('isStoryDraft').value;

    console.log(dragDiv);
    console.log(inputFile);
    console.log(imagePreviewDiv);

    dragDiv.addEventListener("click", () => {
        inputFile.click();
    });

    dragDiv.addEventListener("dragover", (e) => {
        e.preventDefault();
        dragDiv.classList.add("drag-hover");
    });

    dragDiv.addEventListener("drop", (e) => {
        e.preventDefault();
        let droppedImages = e.dataTransfer.files;
        addImageToArray(droppedImages);
    });

    inputFile.addEventListener("change", () => {
        let chosenFiles = inputFile.files;
        addImageToArray(chosenFiles);
    });

    dragDiv.addEventListener("dragleave", (e) => {
        e.preventDefault();
        dragDiv.classList.remove("drag-hover");
    });

    const addImageToArray = (images) => {
        console.log(images);
        if (images.length == 0) return;
        for (let i = 0; i < images.length; i++) {
            if (images[i].type.split("/")[0] != 'image') continue;
            if (!filesArr.some(e => e.name == images[i].name)) {
                filesArr.push(images[i])
                previewImage(URL.createObjectURL(images[i]), i);
            }
        }
        document.querySelectorAll('.remove-from-preview').forEach((element) => {
            element.addEventListener("click", (e) => {
                filesArr.splice($(element).data('index'), 1);
                element.parentNode.remove();
                resetData();
                console.log(filesArr);
            });
        });
    }

    const previewImage = (src, i) => {
        let crossImg = "/images/cross.png";
        imagePreviewDiv.innerHTML += `
    <div class="position-relative d-inline-block m-1">
        <img src="${src}" class="object-fit-cover" style="height:100px;width:130px;" alt="prevImg" />
        <button data-index="${i}" class="remove-from-preview position-absolute top-0 end-0 d-flex align-items-center border-0 bg-dark p-0 m-0">
            <img src="${crossImg}" class=" bg-dark p-1 m-0" />
        </button>
    </div>
    `;
    }

    function resetData() {
        Array.from(document.querySelectorAll(`.remove-from-preview`)).forEach((element, i) => {
            element.setAttribute("data-index", i);
        })
    }


}

function saveFilesArrToInput() {
    let myFiles = new DataTransfer();
    filesArr.forEach(imageFile => myFiles.items.add(imageFile));
    inputFile.files = myFiles.files;
}

function previewDocuments() {
    documentsInput = document.querySelector("#DocumentsInput");
    selectedDocuments = document.querySelector(".selected-documents");
    documentsInput.addEventListener("change", () => {
        selectedDocuments.innerHTML = '';
        const documents = documentsInput.files;
        for (let i = 0; i < documentsInput.files.length; i++) {
            selectedDocuments.innerHTML += `<a target="_blank" href="${URL.createObjectURL(documents[i])}"
                       class="btn border border-dark rounded-pill p-2 d-flex align-items-center gap-2 text-15">${documents[i].name}</a>`
        }
    })
}

// --------------------------------------------------
//if (isStoryDraft == 1) {
//    Array.from(document.querySelectorAll('[data-imgurl]')).forEach((image, index) => {
//        //console.log("index : " + index);
//        const fileName = image.value;
//        console.log(fileName);
//        const url = $(image).data("path");
//        console.log(url);
//        const type = $(image).data("type");
//        console.log(type);
//        return fetch(url)
//            .then(response => response.arrayBuffer())
//            .then(buffer => {
//                const myFile = new File([buffer], fileName, { type: `image/${type.slice(1)}` });
//                addImageToArray([myFile]);
//            });
//    });
//}
//let shareStoryForm = document.getElementById('shareStoryForm');
//shareStoryForm.addEventListener('submit', (e) => {
//    e.preventDefault();
//    saveFilesArrToInput();
//    let submitVal = document.getElementById('action');
//    submitVal.value = e.submitter.getAttribute("value");
//    console.log("submit" + submitVal.value);
//    shareStoryForm.submit();
//});


function AreAdminMissionFormTimeFieldsValid() {
    let currentDate = new Date();
    let missionStartDate = document.getElementById("missionStartDate").value;
    let missionEndDate = document.getElementById("missionEndDate").value;
    let missionRegisterationDeadlineDate = document.getElementById("missionRegisterationDeadlineDate").value;
    let images = document.getElementById('inputFile').files;
    let skills = document.getElementById('missionSkills');
    let selectedSkills = Array.from(skills.selectedOptions).map(option => option.value);


    debugger;
    console.log(images);

    debugger;


    if (missionStartDate < currentDate) {
        debugger;

        document.getElementById('startDateValidationSpan').innerHTML = "Start Date must be After today's date";
        return false;
    }
    else {
        document.getElementById('startDateValidationSpan').innerHTML = "";
    }


    if (missionEndDate.trim() !== "") {

        if (missionStartDate > missionEndDate) {
            document.getElementById('startDateValidationSpan').innerHTML = "Start Date must be Before Mission's End date";
            return false;
        }
        else {
            document.getElementById('startDateValidationSpan').innerHTML = "";
        }

        if (missionRegisterationDeadlineDate > missionEndDate) {
            document.getElementById('registerationDeadlineValidationSpan').innerHTML = "Registeration Deadline Date mustnot be After Mission's End date";
            return false;
        }
        else {
            document.getElementById('registerationDeadlineValidationSpan').innerHTML = "";
        }
    }
    else {
        document.getElementById('startDateValidationSpan').innerHTML = "";
        document.getElementById('registerationDeadlineValidationSpan').innerHTML = "";
    }

    if (missionRegisterationDeadlineDate.trim() !== "") {
        if (missionRegisterationDeadlineDate < currentDate) {
            document.getElementById('registerationDeadlineValidationSpan').innerHTML = "Registeration Deadline Date must be After CurrentDate date";
            return false;
        }
        else {
            document.getElementById('registerationDeadlineValidationSpan').innerHTML = "";
        }
    }
    else {
        document.getElementById('registerationDeadlineValidationSpan').innerHTML = "";
    }


    if (images.length <= 0) {
        document.getElementById('imageValidationSpan').innerHTML = "Please upload atleast one mission image";
        return false;
    }
    else {
        document.getElementById('imageValidationSpan').innerHTML = "";
    }


    if (selectedSkills.length <= 0) {
        debugger;
        document.getElementById('skillError').innerHTML = "Please select atleast one mission skill";
        return false;
    }
    else {
        document.getElementById('skillError').innerHTML = "";
    }

    return true;
}
