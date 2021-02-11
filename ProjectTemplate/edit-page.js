function editAccount() {
	var webMethod = "ProjectServices.asmx/EditAccount";
	var companyName = document.getElementById("company-name").value;
	var fName = document.getElementById("first-name").value;
	var lName = document.getElementById("last-name").value;
	var email = document.getElementById("email-address").value;
	var currentPword = document.getElementById("current-password").value;
	var newPword = document.getElementById("new-password").value;
	var checkbox =  document.getElementById("checkbox-1").checked;
	var parameters = "{\"firstName\": \"" + encodeURI(fName) + "\","	
					 + "\"lastName\": \"" + encodeURI(lName) + "\","
					 + "\"emailAddress\": \"" + encodeURI(email) + "\","
					 + "\"currentPword\": \"" + encodeURI(currentPword) + "\","
					 + "\"newPword\": \"" + encodeURI(newPword) + "\","
					 + "\"checkbox\": \"" + encodeURI(checkbox) + "\"";

	console.log(`First Name: ${fName}\n
				Last Name: ${lName}\n
				Email: ${email}\n
				Current Password: ${currentPword}\n
				New Password: ${newPword}\n
				Checkbox: ${checkbox}`);

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
}
