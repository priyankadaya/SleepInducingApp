// Created by Connor Holmes

// Used for determing background brightness
// Day = light
// Night = dark

// Called after successful log-in
function navigateToSite() {

}

function getBrightness() {
    // Returns hours as an int between 0 and 23
    function getTimeOfDay() {
        var d = new Date();
        return d.getHours();
    }
    var timeOfDay = getTimeOfDay();
    if (timeOfDay > 5 || timeOfDay < 19) {
        document.getElementById('body').style.filter = "brightness(1.75)";
    } else {
        document.getElementById('body').style.filter = "brightness(50%)";
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
        // axios({
        //     method: 'post',
        //     url: url,
        //     data: JSON.stringify(data)
        // }).then(data => console.log(data) && alert('Log-in Successful') && navigateToSite()).catch(err => alert('Error\n' + err));
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function(msg) {
            alert('log-in succesful')
        },
        error: function(e) {
            alert('Something went seriously wrong.');
        }
    });

}


getBrightness();