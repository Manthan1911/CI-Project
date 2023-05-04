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


if ($('#redirectToStory').val() == 1) {
    ajaxCallForAdminPartial("/Admin/GetStoryPartial", "story");
}
else {
    ajaxCallForAdminPartial("/Admin/GetUserPartial", "user");
}

sidebarTabs.forEach((tab) => {

    tab.addEventListener("click", (e) => {
        e.preventDefault();
        tinymce.remove("textarea#tiny");

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
            case "story":
                url = "/Admin/GetStoryPartial";
                ajaxCallForAdminPartial(url, tabToOpen);
                break;
            case "banner":
                url = "/Admin/GetBannerPartial";
                ajaxCallForAdminPartial(url, tabToOpen);
                break;
            case "timesheet":
                url = "/Admin/GetMissionTimesheetPartial";
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
    console.log(url);
    console.log(tabToOpen);
    $.ajax({
        url: url,
        method: "GET",
        success: (data) => {
            console.log(data);
            $('#adminPagePartialContainer').html(data);

            createPagination(5);
            handleSidebarClassOnClick(tabToOpen);

            assignEventsToPartialPage(tabToOpen);


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

const assignEventsToPartialPage = (tabToOpen) => {
    switch (tabToOpen) {
        case "user":
            searchEvent("/Admin/GetSearchedUserPartial", tabToOpen);
            callAddUserPartial();
            deleteUserFromAdmin();
            restoreUserFromAdmin();
            editUserFromAdmin();
            break;
        case "cms":
            searchEvent("/Admin/GetSearchedCmsPartial", tabToOpen);
            callAddCmsPartial();
            callEditCmsPartial();
            deleteCmsPageFromAdmin();
            restoreCmsPageFromAdmin();
            break;
        case "skill":
            searchEvent("/Admin/GetSearchedSkillPartial", tabToOpen);
            callAddSkillPartial();
            callEditSkillPartial();
            deleteSkillFromAdmin();
            restoreSkillFromAdmin();
            break;
        case "application":
            searchEvent("/Admin/GetSearchedMissionApplicationPartial", tabToOpen);
            acceptMissionApplication();
            declineMissionApplication();
            break;
        case "theme":
            searchEvent("/Admin/GetSearchedMissionThemePartial", tabToOpen);
            callAddThemePartial();
            callEditThemePartial();
            deleteThemeFromAdmin();
            restoreThemeFromAdmin();
            break;
        case "mission":
            searchEvent("/Admin/GetSearchedMissionPartial", tabToOpen);
            callAddTimeMissionPartial();
            callAddGoalMissionPartial();
            callEditMissionPartial();
            deactivateMission();
            activateMission();
            break;
        case "story":
            searchEvent("/Admin/GetSearchedStoryPartial", tabToOpen);
            approveStory();
            declineStory();
            break;
        case "banner":
            //debugger;
            searchEvent("/Admin/GetSearchedBannerPartial", tabToOpen);
            callAddBannerPartial();
            callEditBannerPartial();
            callDeleteBannerPartial();
            break;
        case "timesheet":
            approveMissionTimesheet(); 
            declineMissionTimesheet();
            break;
        default:
            callAddUserPartial();
            deleteUserFromAdmin();
            restoreUserFromAdmin();
            editUserFromAdmin();
            break;
    }
}

const searchBar = debounce((url, query, tabToOpen) => {
    $.ajax({
        url: url,
        method: "GET",
        data: { "searchInput": query },
        success: (result) => {
            //$(".spinner-border").removeClass("opacity-1");
            //$(".spinner-border").addClass("opacity-0");
            $('#adminPagePartialContainer').html(result);
            createPagination(5);
            $('#adminSearch').val(query);
            console.log(document.getElementById("adminSearch"));
            document.getElementById("adminSearch").focus;
            assignEventsToPartialPage(tabToOpen);
        },
        error: error => {
            console.log(error);
            sweetAlertError("Something went wrong!");
        }
    });
});

function debounce(cb, delay = 800) {
    let timeout;
    return (...arg) => {
        clearTimeout(timeout);
        timeout = setTimeout(() => {
            cb(...arg);
        }, delay);
    }
}

function searchEvent(url, tabToOpen) {
    $("#adminSearch").on("input", () => {
        //$(".spinner-border").removeClass("opacity-0");
        //$(".spinner-border").addClass("opacity-1");
        const query = $('#adminSearch').val();
        searchBar(url, query, tabToOpen);
    })
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

                    //debugger;
                    let isEmailUnique = checkIsEmailAlreadyUsed(EmailId.value);
                    //debugger;
                    if (isEmailUnique) {
                        sweetAlertError("Email already taken !");
                        return;
                    }
                    //debugger;
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
        error: (error, _, status) => {
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


function checkIsEmailAlreadyUsed(email) {
    let isEmailAlreadyUsed = false;
    $.ajax({
        url: '/Admin/checkIsEmailAlreadyUsed',
        async: false,
        data: { "email": email },
        success: (result) => {
            console.log(result);
            isEmailAlreadyUsed = result;
            //debugger;
        },
        error: (error) => { sweetAlertError("cannot checkUserEmail"); return; }
    });
    return isEmailAlreadyUsed;
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


//-------------------- Mission Theme ----------------------

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

//-------------------- Mission ----------------------
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
};

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

const callEditMissionPartial = () => {
    let editMissionButtons = document.querySelectorAll('.editMissionBtn');
    editMissionButtons.forEach((button) => {
        button.addEventListener("click", (e) => {
            e.preventDefault();


            const missionType = button.getAttribute("data-missionType");
            const missionId = button.getAttribute("data-missionId");

            switch (missionType) {
                case "time":
                    $.ajax({
                        url: "/Admin/GetEditTimeMissionPartial",
                        method: "GET",
                        data: { "missionId": missionId },
                        success: function (data) {
                            $('#adminPagePartialContainer').html(data);
                            documents = [];

                            $.getScript("/js/cmsTiny.js");

                            $('#editTimeMissionForm').on('submit', (e) => {
                                e.preventDefault();

                                //debugger;
                                saveFilesArrToInput();
                                //debugger;

                                let form = $('#editTimeMissionForm');

                                if (isAdminFormValid(form) && AreAdminMissionFormTimeFieldsValid()) {

                                    const formData = new FormData(form[0]);
                                    console.log(formData);
                                    formData.set("Description", tinymce.get('tiny').getContent());

                                    $.ajax({
                                        url: "/Admin/EditTimeMission",
                                        method: "PUT",
                                        processData: false,
                                        contentType: false,
                                        data: formData,
                                        success: (data, _, status) => {

                                            Swal.fire({
                                                position: 'top-end',
                                                icon: 'success',
                                                title: 'Time Mission Edited successfully!',
                                                showConfirmButton: false,
                                                timer: 3000
                                            })

                                            ajaxCallForAdminPartial(url, tabToOpen);

                                        },
                                        error: (error) => {
                                            Swal.fire({
                                                position: 'top-end',
                                                icon: 'error',
                                                title: 'Error Editing Time Mission!',
                                                showConfirmButton: false,
                                                timer: 3000
                                            })
                                            return;
                                        }
                                    });
                                }
                            });

                            $("#editTimeMissionCancelBtn").on("click", (e) => {
                                e.preventDefault();
                                ajaxCallForAdminPartial(url, tabToOpen);
                            });


                            adminPreviewImage();
                            previewDocuments();
                            changeCityListAccordingToCountry();
                            initDocumentsOnEdit();
                        },
                        error: function (error) {

                        }
                    });
                    break;
                case "goal":
                    $.ajax({
                        url: "/Admin/GetEditGoalMissionPartial",
                        method: "GET",
                        data: { "missionId": missionId },
                        success: function (data) {
                            $('#adminPagePartialContainer').html(data);
                            documents = [];

                            $.getScript("/js/cmsTiny.js");

                            $('#editGoalMissionForm').on('submit', (e) => {
                                e.preventDefault();

                                //debugger;
                                saveFilesArrToInput();
                                //debugger;

                                let form = $('#editGoalMissionForm');

                                if (isAdminFormValid(form) && AreAdminMissionFormTimeFieldsValid()) {

                                    const formData = new FormData(form[0]);
                                    formData.set("Description", tinymce.get('tiny').getContent());

                                    $.ajax({
                                        url: "/Admin/EditGoalMission",
                                        method: "PUT",
                                        processData: false,
                                        contentType: false,
                                        data: formData,
                                        success: (data, _, status) => {

                                            Swal.fire({
                                                position: 'top-end',
                                                icon: 'success',
                                                title: 'Goal Mission Edited successfully!',
                                                showConfirmButton: false,
                                                timer: 3000
                                            })

                                            ajaxCallForAdminPartial(url, tabToOpen);

                                        },
                                        error: (error) => {
                                            Swal.fire({
                                                position: 'top-end',
                                                icon: 'error',
                                                title: 'Error Editing Goal Mission!',
                                                showConfirmButton: false,
                                                timer: 3000
                                            })
                                            return;
                                        }
                                    });
                                }
                            });

                            $("#editGoalMissionCancelBtn").on("click", (e) => {
                                e.preventDefault();
                                ajaxCallForAdminPartial(url, tabToOpen);
                            });


                            adminPreviewImage();
                            previewDocuments();
                            changeCityListAccordingToCountry();
                            initDocumentsOnEdit();
                        },
                        error: function (error) {

                        }
                    });
                    break;
            }
        });
    });
};

const deactivateMission = () => {
    const deleteMissionBtns = document.querySelectorAll(".deleteMissionBtn");

    deleteMissionBtns.forEach((btn) => {

        btn.addEventListener("click", (e) => {
            e.preventDefault();

            const missionId = btn.getAttribute("data-missionId");



            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Deactivate'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/SoftDeleteMission",
                        method: "POST",
                        data: { "missionId": missionId },
                        success: function (data, _, status) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Deactivated Mission!',
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

                }
            })

        });

    });

}

const activateMission = () => {
    const restoreMissionBtns = document.querySelectorAll(".restoreMissionBtn");

    restoreMissionBtns.forEach((btn) => {

        btn.addEventListener("click", (e) => {
            e.preventDefault();

            const missionId = btn.getAttribute("data-missionId");

            Swal.fire({
                title: 'Are you sure?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#5cb85c',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Activate'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/RestoreMission",
                        method: "POST",
                        data: { "missionId": missionId },
                        success: function (data, _, status) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Restored Mission!',
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

//-------------------- Story ------------------------
const sweetAlertError = (message) => {
    Swal.fire({
        position: 'top-end',
        icon: 'error',
        title: message,
        showConfirmButton: false,
        timer: 3000
    })
}

const approveStory = () => {
    const approveStoryBtns = document.querySelectorAll(".approveStoryBtn");

    approveStoryBtns.forEach((btn) => {

        btn.addEventListener("click", (e) => {
            e.preventDefault();

            const storyId = btn.getAttribute("data-storyId");

            Swal.fire({
                title: 'Are you sure you want to approve this story?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#5cb85c',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Approve'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/ApproveStory",
                        method: "POST",
                        data: { "storyId": storyId },
                        success: function (data, _, status) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Approved Story!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Approving Story!',
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

const declineStory = () => {
    const declineStoryBtns = document.querySelectorAll(".declineStoryBtn");

    declineStoryBtns.forEach((btn) => {

        btn.addEventListener("click", (e) => {
            e.preventDefault();

            const storyId = btn.getAttribute("data-storyId");

            Swal.fire({
                title: 'Are you sure you want to decline this story?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Decline'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/DeclineStory",
                        method: "POST",
                        data: { "storyId": storyId },
                        success: function (data, _, status) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Story Declined!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Declining Story!',
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


//-------------- Banner ------------------

const callAddBannerPartial = () => {
    $('#addBannerBtn').on("click", (e) => {
        e.preventDefault();


        $.ajax({
            url: "/Admin/GetAddBannerPartial",
            method: "GET",
            success: (data) => {
                $('#adminPagePartialContainer').html(data);

                $('#addBannerForm').on("submit", (e) => {
                    e.preventDefault();
                    let form = $('#addBannerForm');

                    if (isAdminFormValid(form)) {

                        const formData = new FormData(form[0]);
                        console.log(formData);

                        $.ajax({
                            url: "/Admin/SaveBanner",
                            method: "POST",
                            processData: false,
                            contentType: false,
                            data: formData,
                            success: (data, _, status) => {

                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'success',
                                    title: 'Banner Added successfully!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })

                                ajaxCallForAdminPartial(url, tabToOpen);

                            },
                            error: (error) => {
                                Swal.fire({
                                    position: 'top-end',
                                    icon: 'error',
                                    title: 'Error Adding Banner!',
                                    showConfirmButton: false,
                                    timer: 3000
                                })
                                return;
                            }
                        });
                    }
                });

                $("#addBannerCancelBtn").on("click", (e) => {
                    e.preventDefault();
                    ajaxCallForAdminPartial(url, tabToOpen);
                });

                singlePreviewImage();
            },
            error: (error) => {
                Swal.fire({
                    position: 'top-end',
                    icon: 'error',
                    title: 'problem loading Add Banner partial!',
                    showConfirmButton: false,
                    timer: 3000
                })

            }
        });

    });
}

const callEditBannerPartial = () => {
    let editBannerBtns = document.querySelectorAll(".editBannerBtn");
    editBannerBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            let bannerId = btn.getAttribute("data-bannerId");

            $.ajax({
                url: "/Admin/GetEditBannerPartial",
                method: "GET",
                data: { "bannerId": bannerId },
                success: (data) => {
                    $('#adminPagePartialContainer').html(data);

                    $('#editBannerForm').on("submit", (e) => {
                        e.preventDefault();
                        let form = $('#editBannerForm');

                        if (isAdminFormValid(form)) {

                            const formData = new FormData(form[0]);
                            console.log(formData);

                            $.ajax({
                                url: "/Admin/EditBanner",
                                method: "POST",
                                processData: false,
                                contentType: false,
                                data: formData,
                                success: (data, _, status) => {

                                    Swal.fire({
                                        position: 'top-end',
                                        icon: 'success',
                                        title: 'Banner Edited successfully!',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })

                                    ajaxCallForAdminPartial(url, tabToOpen);

                                },
                                error: (error) => {
                                    Swal.fire({
                                        position: 'top-end',
                                        icon: 'error',
                                        title: 'Error Editing Banner!',
                                        showConfirmButton: false,
                                        timer: 3000
                                    })
                                    return;
                                }
                            });
                        }
                    });

                    $("#editBannerCancelBtn").on("click", (e) => {
                        e.preventDefault();
                        ajaxCallForAdminPartial(url, tabToOpen);
                    });

                    editSinglePreviewImage();
                },
                error: (error) => {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'problem loading Edit Banner partial!',
                        showConfirmButton: false,
                        timer: 3000
                    })

                }
            });
        });
    });
}

const callDeleteBannerPartial = () => {
    let deleteBannerBtns = document.querySelectorAll(".deleteBannerBtn");
    deleteBannerBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            e.preventDefault();

            let bannerId = btn.getAttribute("data-bannerId");


            Swal.fire({
                title: 'Do you really want to delete this banner?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Delete'
            }).then((result) => {
                if (result.isConfirmed) {


                    $.ajax({
                        url: "/Admin/DeleteBanner",
                        method: "POST",
                        data: { "bannerId": bannerId },
                        success: (result) => {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Deleted Banner!',
                                showConfirmButton: false,
                                timer: 2000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);

                        },
                        error: (error) => {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Deleting Banner!',
                                showConfirmButton: false,
                                timer: 2000
                            });
                        }
                    });

                }
            });


        });
    });
}

//----------- FORM Validation --------------------
const approveMissionTimesheet = () => {
    const approveMissionTimesheetBtns = document.querySelectorAll(".approveMissionTimesheetBtn");

    approveMissionTimesheetBtns.forEach((btn) => {

        btn.addEventListener("click", (e) => {
            e.preventDefault();

            const timesheetId = btn.getAttribute("data-missionTimesheetId");

            Swal.fire({
                title: 'Are you sure you want to approve this timesheet?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#5cb85c',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Approve'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/ApproveMissionTimesheet",
                        method: "POST",
                        data: { "timesheetId": timesheetId },
                        success: function (data, _, status) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Approved Timesheet!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            Swal.fire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Approving Timesheet!',
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

const declineMissionTimesheet = () => {
    const declineMissionTimesheetBtns = document.querySelectorAll(".declineMissionTimesheetBtn");

    declineMissionTimesheetBtns.forEach((btn) => {

        btn.addEventListener("click", (e) => {
            e.preventDefault();

            const timesheetId = btn.getAttribute("data-missionTimesheetId");

            Swal.fire({
                title: 'Are you sure you want to decline this timesheet?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Decline'
            }).then((result) => {
                if (result.isConfirmed) {

                    $.ajax({
                        url: "/Admin/DeclineMissionTimesheet",
                        method: "POST",
                        data: { "timesheetId": timesheetId },
                        success: function (data, _, status) {

                            Swal.fire({
                                position: 'top-end',
                                icon: 'success',
                                title: 'Timesheet Declined!',
                                showConfirmButton: false,
                                timer: 3000
                            });

                            ajaxCallForAdminPartial(url, tabToOpen);
                        },
                        error: function (error) {
                            SwalMissionTimesheetfire({
                                position: 'top-end',
                                icon: 'error',
                                title: 'Error Declining Timesheet!',
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


//----------- FORM Validation --------------------

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
let documents = [];

function adminPreviewImage() {

    filesArr = [];
    dragDiv = document.getElementById('dragDiv');
    inputFile = document.getElementById('inputFile');
    imagePreviewDiv = document.getElementById('imagePreviewDiv');

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
            });
        });
    }

    const previewImage = (src, i) => {
        let crossImg = "/images/cross.png";
        imagePreviewDiv.innerHTML += `
    <div class="position-relative d-inline-block m-1">
        <img src="${src}" class="object-fit-cover" style="height:100px;width:130px;" alt="prevImg" />
        <button type="button" data-index="${i}" class="remove-from-preview position-absolute top-0 end-0 d-flex align-items-center border-0 bg-dark p-0 m-0">
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

    let fetchMissionImages = document.getElementById('fetchMissionImages');

    if (fetchMissionImages != null) {
        if (fetchMissionImages.value == 1) {
            Array.from(document.querySelectorAll('[data-imgurl]')).forEach((image) => {
                const fileName = image.value;
                //console.log(fileName);
                const url = $(image).data("path");
                //console.log(url);
                const type = $(image).data("type");
                //console.log(type);
                return fetch(url)
                    .then(response => response.arrayBuffer())
                    .then(buffer => {
                        const myFile = new File([buffer], fileName, { type: `image/${type.slice(1)}` });
                        addImageToArray([myFile]);
                        saveFilesArrToInput();
                    });
            });
        }
    }

}

function saveFilesArrToInput() {

    let myFiles = new DataTransfer();
    filesArr.forEach((imageFile) => {
        myFiles.items.add(imageFile);
    });
    inputFile.files = myFiles.files;
}

async function initDocumentsOnEdit() {
    const docImages = Array.from(document.querySelectorAll('[data-doc]'));
    documentsInput = document.querySelector("#DocumentsInput");
    selectedDocuments = document.querySelector(".selected-documents");

    for (const image of docImages) {
        const fileName = image.value;
        const url = $(image).data("doc");
        const type = $(image).data("type");
        //const title = $(image).data("title");

        const response = await fetch(url);
        const buffer = await response.arrayBuffer();
        const myFile = new File([buffer], fileName, { type: `image/${type.slice(1)}` });
        documents.push(myFile);
        //titles.push(title);
    }

    let myFiles = new DataTransfer();
    documents.forEach((document) => {
        myFiles.items.add(document);
    });
    documentsInput.files = myFiles.files;

    documents.forEach((doc) => {
        selectedDocuments.innerHTML += `<a target="_blank" href="${URL.createObjectURL(doc)}"
                       class="btn border border-dark rounded-pill p-2 d-flex align-items-center gap-2 text-15">${doc.name}</a>`
    });
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

const singlePreviewImage = () => {
    let singleImagePreviewDiv = document.getElementById('singleImagePreviewDiv');
    let imgInput = document.getElementById('imageInput');
    let image;

    imgInput.addEventListener("change", (e) => {
        e.preventDefault();
        console.log(imgInput.files[0]);
        image = imgInput.files[0];
        imagePreview(URL.createObjectURL(image))
    });

    let imagePreview = (src) => {
        singleImagePreviewDiv.innerHTML = "";
        singleImagePreviewDiv.innerHTML += `
    <div class="position-relative d-inline-block m-1">
        <img src="${src}" class="object-fit-cover" style="height:100px;width:130px;" alt="prevImg" />
    </div>
    `;
    }



}

const editSinglePreviewImage = () => {
    let singleImagePreviewDiv = document.getElementById('singleImagePreviewDiv');
    let imgInput = document.getElementById('imageInput');
    let image;

    imgInput.addEventListener("change", (e) => {
        e.preventDefault();
        console.log(imgInput.files[0]);
        image = imgInput.files[0];
        imagePreview(URL.createObjectURL(image))
    });

    let imagePreview = (src) => {
        singleImagePreviewDiv.innerHTML = "";
        singleImagePreviewDiv.innerHTML += `
    <div class="position-relative d-inline-block m-1">
        <img src="${src}" class="object-fit-cover" style="height:100px;width:130px;" alt="prevImg" />
    </div>
    `;
    }

    fetchBannerImage();

    async function fetchBannerImage() {
        const bannerImage = document.getElementById("preloadedBannerImage");
        const fileName = $(bannerImage).data("name");
        const url = $(bannerImage).data("url");
        const type = $(bannerImage).data("type");

        const response = await fetch(url);
        const buffer = await response.arrayBuffer();
        const myFile = new File([buffer], fileName, { type: `image/${type.slice(1)}` });
        let myFiles = new DataTransfer();
        myFiles.items.add(myFile);
        imgInput.files = myFiles.files;

        imagePreview(url);

    }

}

// --------------------------------------------------

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


    console.log(images);



    if (missionStartDate < currentDate) {

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
        document.getElementById('skillError').innerHTML = "Please select atleast one mission skill";
        return false;
    }
    else {
        document.getElementById('skillError').innerHTML = "";
    }

    return true;
}
