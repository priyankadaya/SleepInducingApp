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
    if (getTimeOfDay() > 5 || getTimeOfDay() < 19) {
        document.getElementsByName('body').style.filter = "brightness(1.75)";
    } else {
        document.getElementsByName('body').style.filter = "brightness(50%)";
    }
}


function attemptLogin() {
    const url = 'ProjectServices.asmx/LogOn'
    const data = {
        username: document.getElementById('username-entry').value,
        password: document.getElementById('password-entry').value
    }
    axios({
        method: 'post',
        url: url,
        data: data
    }).then(data => console.log(data)).catch(err => alert('Error\n' + err));
}

getBrightness();