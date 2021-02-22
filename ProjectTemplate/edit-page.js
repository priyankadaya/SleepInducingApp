// Written by Varun Shourie

$(document).ready(function () {
	loadDocument();
});


function deleteAccount(id) {
	var webMethod = "ProjectServices.asmx/DeleteAccount";

	if (confirm("Are you sure you want to delete your own account? This is permanent!")) {
		$.ajax({
			type: "POST",
			url: webMethod,
			data: `{"id": "${id}"}`,
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (msg) {
				var responseFromServer = msg.d;

				if (!responseFromServer) {
					alert("Account deletion not successful.");
				}
				else {
					$("#delete-btn,#cancel-btn,#submit-btn").prop("disabled", true);
					window.location.href = 'log-in.html'
				}
			},
			error: function (e) {
				alert("Something went wrong...");
			}
		});
	}
}

function editAccount() {
	var webMethod = "ProjectServices.asmx/UpdateAccount";
	var parameters = "{\"userId\": \"" + encodeURI($("#user-id").val()) + "\","
					+ "\"firstName\": \"" + encodeURI($("#first-name").val()) + "\","	
					+ "\"lastName\": \"" + encodeURI($("#last-name").val()) + "\","
					+ "\"emailAddress\": \"" + encodeURI($("#email-address").val()) + "\","
					+ "\"username\": \"" + encodeURI($("#current-username").val()) + "\","
					+ "\"currentPword\": \"" + encodeURI($("#current-password").val()) + "\","
					+ "\"newPword\": \"" + encodeURI($("#new-password").val()) + "\"}";

	$.ajax({
		type: "POST",
		url: webMethod, 
		data: parameters,  
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (msg) {  
			var responseFromServer = msg.d;  

			if (responseFromServer == "Incorrect password.") {
				$("#password-warning").html(responseFromServer);
				$("#email-warning,#username-warning,#fatal-error").html('');
			}
			else if (responseFromServer == "This email is already associated with an account.") {
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

		},
		error: function (e) {
			alert("this code will only execute if javascript is unable to access the webservice");
		}
	});
}

function loadDocument() {
	var webMethod = "ProjectServices.asmx/GetAccount";

	$.ajax({
		type: "POST",
		url: webMethod,
		data: "{}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (msg) {
			var responseFromServer = msg.d;
			$("#password-warning,#email-warning,#username-warning,#fatal-error").html('');
			$("input").val("");

			$("#user-id").val(responseFromServer.Id);
			$("#first-name").val(responseFromServer.FirstName);
			$("#last-name").val(responseFromServer.LastName);
			$("#email-address").val(responseFromServer.Email);
			$("#current-username").val(responseFromServer.Username);
			$("#delete-btn").attr("onclick", `deleteAccount(${responseFromServer.Id})`);
		},
		error: function (e) {
			alert("Something went wrong...");
		}
	});
}

