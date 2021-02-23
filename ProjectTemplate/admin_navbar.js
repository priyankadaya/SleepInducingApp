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
            var x = document.getElementById("users");

            if (responseFromServer == false) {
                x.style.display = "none";
                
            }
        },
        error: function (e) {
            alert('Something is wrong.');
        }
    });
}

function logOff() {
    debugger;
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
