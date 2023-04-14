$(document).ready((e) => {
    let userId = $('#userId').val();

    callTimeBasedPartial(userId);
    callGoalBasedPartial(userId);

});

function callTimeBasedPartial(userId) {
    $.ajax({
        url: "/MissionTimesheetController/GetTimeBasedPartialView",
        method: "GET",
        dataType: "html",
        data: { "userId": userId },
        success: function (data) {
            $('#timeBasedPartialDiv').html("");
            $('#timeBasedPartialDiv').html(data);
        },
        error: function (error) {
            alert("time model not loaded!");
        }
    });
}

function callGoalBasedPartial(userId) {
    $.ajax({
        url: "/MissionTimesheetController/GetGoalBasedPartialView",
        method: "GET",
        dataType: "html",
        data: { "userId": userId },
        success: function (data) {
            $('#goalBasedPartialDiv').html("");
            $('#goalBasedPartialDiv').html(data);
        },
        error: function (error) {
            alert("goal model not loaded!");
        }
    });
}