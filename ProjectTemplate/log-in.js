// Created by Connor Holmes

// Used for determing background brightness
// Day = light
// Night = dark

// Called after successful log-in
function navigateToMainSite() {
    window.location.pathname = '/sounds.html'
}

function getBrightness() {
    // Returns hours as an int between 0 and 23
    function getTimeOfDay() {
        var d = new Date();
        return d.getHours();
    }
    var timeOfDay = getTimeOfDay();
    if (timeOfDay > 5 || timeOfDay <= 9) {
        document.getElementById('body').style.filter = "brightness(1.10)";
    }
    else if (timeOfDay > 9 || timeOfDay < 12) {
        document.getElementById('body').style.filter = "brightness(1.25)";
    }
    else if (timeOfDay => 12 || timeOfDay < 19) {
        document.getElementById('body').style.filter = "brightness(1.50)";
    }
    else {
        document.getElementById('body').style.filter = "brightness(75%)";
    }
}

function attemptLogin() {
    const url = 'ProjectServices.asmx/LogOn'
    var username = document.getElementById('username-entry').value;
    var password = document.getElementById('password-entry').value;
    var data = {
            username: document.getElementById('username-entry').value,
            password: document.getElementById('password-entry').value
        }
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function(msg) {
            if (msg.d) {
                navigateToMainSite();
            }
            else {
                alert("Log-in failed.");
            }
            
        },
        error: function(e) {
            alert('Something went seriously wrong.');
        }
    });
}

getBrightness();