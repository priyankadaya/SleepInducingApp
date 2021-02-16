// Created by Connor Holmes

// Used for determing background brightness
// Day = light
// Night = dark

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
    var data = {
        username: document.getElementById('username-entry').value,
        password: document.getElementById('password-entry').value
    }
    formattedData = JSON.stringify(data);
    axios({
        method: 'post',
        url: url,
        data: formattedData
    }).then(data => console.log(data) && alert('Log-in Successful')).catch(err => alert('Error\n' + err));
}

getBrightness();