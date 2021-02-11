function createAccount() {
	var webMethod = "ProjectServices.asmx/CreateAccount";
	var companyName = document.getElementById("company-name").value;
	var fName = document.getElementById("first-name").value;
	var lName = document.getElementById("last-name").value;
	var email = document.getElementById("email-address").value;

	var parameters = "{\"companyName\": \"" + encodeURI(fName) + "\","
	+ "\"firstName\": \"" + encodeURI(lName) + "\","	
	+ "\"lastName\": \"" + encodeURI(lName) + "\","
	+ "\"emailAddress\": \"" + encodeURI(email) + "\","
	+ "\"password\": \"" + encodeURI(password) + "\","

	console.log(`CompanyName: ${companyName}\n
			First Name: ${fName}\n
			Last Name: ${lName}\n
			Email: ${email}\n
			Password: ${password}`);

	///window.location.href=
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



				
	// var editAjaxCall = //jQuery ajax method
	// $.ajax({
	// 	type: "POST",
	// 	url: webMethod, // which web service we want to talk to
	// 	data: parameters,  // which information will be sent to the web service
	// 	contentType: "application/json; charset=utf-8",
	// 	dataType: "json",
	// 	success: function (msg) {  // msg represents whatever the server sends back to you in its raw form
	// 		var responseFromServer = msg.d;  // grab based off a property of msg - the data from the message.

	// 		for (var i = 0; i < responseFromServer.length; i++) {
	// 			// Visual Studio encodes our class data into JSON
	// 			alert(responseFromServer[i].fName);  // whatever value was returned will live in this variable, so we alert it.
	// 			alert(responseFromServer[i].lName);
	// 		}

	// 	},
	// 	error: function (e) {
	// 		alert("this code will only execute if javascript is unable to access the webservice");
	// 	}
	// });


