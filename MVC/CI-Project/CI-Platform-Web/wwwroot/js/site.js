const drop = document.querySelector(".cms-dropdown");

if (drop) {
    $.ajax({
        url: "/Home/GetCmsList",
        success: (result) => {
            for (var cms of result) {
                drop.innerHTML += `<li><a href="/Home/CmsPage/${cms.cmsPageId}" class="dropdown-item">${cms.title}</a></li>`;
            }
        },
        error: error => {
            console.log(error);
        }
    });
}

$("#logoutBtn").on("click", (e) => {
    e.preventDefault()

    Swal.fire({
        title: 'Do you really want to logout?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, Log out!'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: "/Authentication/Logout",
                method: "POST",
                success: (result) => {

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Logged out !',
                        showConfirmButton: false,
                        timer: 1000
                    }).then(() => {
                        window.location.href = result;
                    });
                },
                error: (error) => {

                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Error Logging out!',
                        showConfirmButton: false,
                        timer: 3000
                    });
                }
            });

        }
    })
});

// --------------------- notification ------------------------
let nUserId = 0;
$(document).ready(() => {
    nUserId = $("#notificationIconDiv").data("user_id");

    setListenerOnNotificationIconClick();
});

let notificationDiv = document.getElementById("notificationDiv");
let notificationOverlayDiv = document.getElementById("notification-overlay-div");
let notificationIconDiv = document.getElementById("notificationIconDiv");

function setListenerOnNotificationIconClick() {
    if (notificationIconDiv) {
        notificationIconDiv.addEventListener("click", (e) => {
            e.preventDefault();

            if (!notificationDiv.classList.contains("d-none")) {
                notificationDiv.classList.add("d-none");
                notificationOverlayDiv.classList.add("d-none");
            }
            else {
                notificationDiv.classList.remove("d-none");
                notificationOverlayDiv.classList.remove("d-none");

                if (nUserId != 0 && nUserId != null) {
                    loadNotification(nUserId);
                    loadNotificationSettings(nUserId);
                    toggleNotificationSettings();
                }

                notificationOverlayDiv.addEventListener("click", (e) => {
                    e.preventDefault();
                    notificationDiv.classList.add("d-none");
                    notificationOverlayDiv.classList.add("d-none");
                });
            }
        });
    }
}

const loadNotification = (nUserId) => {


    $.ajax({
        url: "/Notification/GetAllNotificationsOfUser",
        method: "GET",
        data: { "userId": nUserId },
        success: (data) => {
            $(".notification-container").html(data);
        },
        error: (error) => {

        }
    });
}

const loadNotificationSettings = (nUserId) => {
    $.ajax({
        url: "/Notification/GetNotificationSettingsPartial",
        method: "GET",
        data: { "userId": nUserId },
        success: (data) => {
            $("#notificationSettingsTogglerDiv").html(data);
            const notificationSettingsCancelBtn = document.getElementById("notificationSettingsCancelBtn");

            notificationSettingsOverlay.addEventListener("click", (e) => {
                e.preventDefault();
                notificationSettingsTogglerDiv.classList.add("d-none");
                notificationSettingsOverlay.classList.add("d-none");
                clearAllBtn.classList.remove("invisible");
            });

            if (notificationSettingsCancelBtn) {
                notificationSettingsCancelBtn.addEventListener("click", (e) => {
                    e.preventDefault();
                    notificationSettingsTogglerDiv.classList.add("d-none");
                    notificationSettingsOverlay.classList.add("d-none");
                });
            }

            setListenerOnNotificationSettingsFormSubmit();
        },
        error: (error) => {

        }
    });
}

const toggleNotificationSettings = () => {
    const notificationSettingsBtn = document.getElementById("notificationSettingsBtn");
    const notificationSettingsTogglerDiv = document.getElementById("notificationSettingsTogglerDiv");
    const notificationSettingsOverlay = document.getElementById("notificationSettingsOverlay");
    const clearAllBtn = document.getElementById("clearAllBtn");
    
    notificationSettingsBtn.addEventListener("click", (e) => {
        e.preventDefault();

        if (!notificationSettingsTogglerDiv.classList.contains("d-none")) {
            notificationSettingsTogglerDiv.classList.add("d-none");
            notificationSettingsOverlay.classList.add("d-none");
            clearAllBtn.classList.remove("invisible");
        }
        else {
            notificationSettingsTogglerDiv.classList.remove("d-none");
            notificationSettingsOverlay.classList.remove("d-none");
            clearAllBtn.classList.add("invisible");

            loadNotificationSettings(nUserId);
        }
    });
}

function setListenerOnNotificationSettingsFormSubmit() {


    $('#notificationSettingForm').on("submit", (e) => {
        e.preventDefault();
        let form = $('#notificationSettingForm');
        if (isNotificationFormValid(form)) {

            let formData = form.serialize();

            $.ajax({
                url: "/Notification/UpdateNotificationSettings",
                method: "PUT",
                data: formData,
                success: (data, _, status) => {

                    let successDiv = `
                        <div class="h-100 w-100 d-flex align-items-center justify-content-center">
                            <img src="images/success.gif" />
                        </div>
                    `;

                    $("#notificationSettingsTogglerDiv").html(successDiv);

                    setTimeout(function () {
                        setListenerOnNotificationIconClick();
                        document.getElementById('notificationSettingsOverlay').click();
                    }, 2500);

                },
                error: (error) => {
                   
                    console.log(error);
                }
            });
        }
    });
}

let isNotificationFormValid = (form) => {
    if (!form.valid()) {
        return false;
    }
    return true;
}