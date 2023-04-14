$(document).ready((e) => {
    let userId = $('#userId').val();
    console.log(userId)

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
            console.log(data);
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
        url: "/VolunteeringTimesheet/GetGoalBasedPartialView",
        method: "GET",
        dataType: "html",
        data: { "userId": userId },
        success: function (data) {
            console.log(data);

            $('#goalBasedPartialDiv').html("");
            $('#goalBasedPartialDiv').html(data);
        },
        error: function (error) {
            alert("goal model not loaded!");
        }
    });
}