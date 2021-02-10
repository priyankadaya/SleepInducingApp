// Created by Connor Holmes
function attemptLogin() {
    const url = ''
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