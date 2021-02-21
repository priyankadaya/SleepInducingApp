function createNewAccount() {

	var parameters = "{\"firstName\": \"" + encodeURI($("#first-name").val()) + "\","
		+ "\"lastName\": \"" + encodeURI($("#last-name").val()) + "\","
		+ "\"emailAddress\": \"" + encodeURI($("#email-address").val()) + "\","
		+ "\"username\": \"" + encodeURI($("#username").val()) + "\","
		+ "\"Pword\": \"" + encodeURI($("#password").val()) + "\"}";

	$.ajax({
		type: "POST", 
		url: "ProjectServices.asmx/CreateAccount",
		data: parameters, 
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (msg) {
			var responseFromServer = msg.d;

			if (responseFromServer == "This email is already associated with an account.") {
				$("#email-warning").html(responseFromServer);
				$("#password-warning,#username-warning,#fatal-error").html('');

			}
			else if (responseFromServer == "This username is already associated with an account.") {
				$("#username-warning").html(responseFromServer);
				$("#fatal-error,#password-warning,#email-warning").html('');

			}
			else if (responseFromServer == "Error.") {
				$("fatal-error").html(responseFromServer);
				$("#password-warning,#email-warning,#username-warning").html('');

			}
			else {
					alert("Account creation successful! Redirecting you to login page...")
					goToLogin();
			}
		},
		error: function (e) {
			alert("this code will only execute if javascript is unable to access the webservice");
			console.log(e)
		}
	});
}

function goToLogin() {
	window.location.pathname = '/log-in.html'
}

var check = function() {
	confirm_password = document.getElementById("confirmpassword").value;
	if (document.getElementById('password').value ==
		 document.getElementById('confirmpassword').value) {
		document.getElementById('message').style.color = 'green';
		document.getElementById('message').innerHTML = 'matching';
		document.getElementById('submit-btn').disabled = false;
	} else {
		document.getElementById('message').style.color = 'red';
		document.getElementById('message').innerHTML = 'not matching';
		document.getElementById('submit-btn').disabled = true;
	}
}


