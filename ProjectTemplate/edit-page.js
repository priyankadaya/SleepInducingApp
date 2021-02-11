

$(document).ready(function () {
	alert('Window has loaded.');
	var webMethod = "ProjectServices.asmx/GetAccount";

	$.ajax({
		type: "POST",
		url: webMethod,
		data: "{}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (msg) {
			var responseFromServer = msg.d;

			$("#first-name").val(responseFromServer.FirstName);
			$("#last-name").val(responseFromServer.LastName);
			$("#email-address").val(responseFromServer.Email);
			$("#current-username").val(responseFromServer.Username);
		},
		error: function (e) {
			alert("Something went wrong...");
		}
	});
});

function editAccount() {
	var webMethod = "ProjectServices.asmx/UpdateAccount";
	var fName = document.getElementById("first-name").value;
	var lName = document.getElementById("last-name").value;
	var email = document.getElementById("email-address").value;
	var currentPword = document.getElementById("current-password").value;
	var newPword = document.getElementById("new-password").value;
	var parameters = "{\"firstName\": \"" + encodeURI(fName) + "\","	
					 + "\"lastName\": \"" + encodeURI(lName) + "\","
					 + "\"emailAddress\": \"" + encodeURI(email) + "\","
					 + "\"currentPword\": \"" + encodeURI(currentPword) + "\","
					 + "\"newPword\": \"" + encodeURI(newPword) + "\"";

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

