// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const drop = document.querySelector(".cms-dropdown");
console.log("drop : ");
console.log(drop);
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
$(document).ready(() => {
    const userId = $("#notificationIconDiv").data("user_id");
    console.log("userId : " + userId);

    if (userId != 0) {
        loadNotification(userId);
        loadNotificationSettings(userId);
    }

    toggleNotificationSettings();
});

let notificationDiv = document.getElementById("notificationDiv");
let notificationOverlayDiv = document.getElementById("notification-overlay-div");
let notificationIconDiv = document.getElementById("notificationIconDiv");
console.log(notificationDiv);
console.log(notificationOverlayDiv);
console.log(notificationIconDiv);
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

            notificationOverlayDiv.addEventListener("click", (e) => {
                e.preventDefault();
                notificationDiv.classList.add("d-none");
                notificationOverlayDiv.classList.add("d-none");
            });
        }
    });
}

const loadNotification = (userId) => {


    $.ajax({
        url: "/Notification/GetAllNotificationsOfUser",
        method: "GET",
        data: { "userId": userId },
        success: (data) => {
            $(".notification-container").html(data);
        },
        error: (error) => {
            
        }
    });
}


const loadNotificationSettings = (userId) => {
    $.ajax({
        url: "/Notification/GetNotificationSettingsPartial",
        method: "GET",
        data: { "userId": userId },
        success: (data) => {
            $("#notificationSettingsTogglerDiv").html(data);
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
            
            notificationSettingsOverlay.addEventListener("click", (e) => {
                e.preventDefault();
                notificationSettingsTogglerDiv.classList.add("d-none");
                notificationSettingsOverlay.classList.add("d-none");
                clearAllBtn.classList.remove("invisible");
            });

            notificationSettingsCancelBtn.addEventListener("click", (e) => {
                e.preventDefault();
                notificationSettingsTogglerDiv.classList.add("d-none");
                notificationSettingsOverlay.classList.add("d-none");
            });
        }
    });
}