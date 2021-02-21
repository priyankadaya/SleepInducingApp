function RecommendSound() {

	var parameters = "{\"username\": \"" + encodeURI($("#current-username").val()) + "\","
		+ "\"emailAddress\": \"" + encodeURI($("#email-address").val()) + "\","
		+ "\"soundDesc\": \"" + encodeURI($("#sound-desc").val()) + "\"}";

	$.ajax({
		type: "POST",
		url: "ProjectServices.asmx/MakeRecommendations",
		data: parameters,
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (msg) {
			var responseFromServer = msg.d;
				alert("Thank you for recommending a sound! Now redirecting to the main page...");
				goToSounds();
		},
		error: function (e) {
			alert("this code will only execute if javascript is unable to access the webservice");
			console.log(e)
		}
	});
}

function goToSounds() {
	window.location.pathname = '/sounds.html'
}
