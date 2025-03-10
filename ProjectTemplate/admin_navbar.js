$(document).ready(function () {
    CheckAdmin();
});


function CheckAdmin() {
    var webMethod = "ProjectServices.asmx/CheckAdmin";

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var responseFromServer = msg.d;
            var viewUsersLink = document.getElementById("users");
            var recommendationList = document.getElementById("recommendation-list");

            if (responseFromServer == false) {
                viewUsersLink.style.display = "none";
                recommendationList.style.display = "none";
            }
        },
        error: function (e) {
            alert('Something is wrong.');
        }
    });
}

function logOff() {
    var webMethod = "ProjectServices.asmx/LogOff"

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            location.href = 'log-in.html'
        },
        error: function (e) {
            alert('Something is wrong.');
        }
    });
}
