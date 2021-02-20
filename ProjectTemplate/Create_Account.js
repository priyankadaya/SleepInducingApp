function createAccount() {
	var webMethod = "ProjectServices.asmx/CreateAccount";
	//var fName = document.getElementById("first-name").value;
	//var lName = document.getElementById("last-name").value;
	//var email = document.getElementById("email-address").value;

	var parameters = "{\"userId\": \"" + encodeURI($("#user-id").val()) + "\","
		+ "\"firstName\": \"" + encodeURI($("#first-name").val()) + "\","
		+ "\"lastName\": \"" + encodeURI($("#last-name").val()) + "\","
		+ "\"emailAddress\": \"" + encodeURI($("#email-address").val()) + "\","
		+ "\"username\": \"" + encodeURI($("#username").val()) + "\","
		+ "\"password\": \"" + encodeURI($("#password").val()) + "\"}";

	console.log(`First Name: ${fName}\n
			Last Name: ${lName}\n
			Email: ${email}\n
			Password: ${password}`);

	$.ajax({
		type: "POST",
		url: webMethod,
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
				function createAccount() {
					var webMethod = "ProjectServices.asmx/UpdateAccount";
					var parameters = "{\"userId\": \"" + encodeURI($("#user-id").val()) + "\","
						+ "\"firstName\": \"" + encodeURI($("#first-name").val()) + "\","
						+ "\"lastName\": \"" + encodeURI($("#last-name").val()) + "\","
						+ "\"emailAddress\": \"" + encodeURI($("#email-address").val()) + "\","
						+ "\"username\": \"" + encodeURI($("#username").val()) + "\","
						+ "\"Pword\": \"" + encodeURI($("#password").val()) + "\"}";
					goToLogin();
				}

			}
		},
		error: function (e) {
			alert("this code will only execute if javascript is unable to access the webservice");
		}
	});
}

function goToLogin() {
	window.location.pathname = '/log-in.html/'
}

var check = function() {
	var password = document.getElementById("password").value;
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


