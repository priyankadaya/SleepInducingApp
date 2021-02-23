// Created by Varun Shourie

$(document).ready(function () {
    loadRecs();

});


function loadRecs() {
    var webMethod = "ProjectServices.asmx/ListRecommendations";

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d.length > 0) {
                allRecs = msg.d;

                populateRecs(allRecs, "#recs");

            }
        },
        error: function (e) {
            alert('Something went seriously wrong.');
        }
    });
}

function populateRecs(recsArray, recContainer) {
    for (var i = 0; i < recsArray.length; i++) {
        var rec;
        if (recsArray[i].Id != null) {
            rec = `<div class='row' id='rec${recsArray[i].Id}' class='text-center'>
                    <div class='col-xs-8 col-sm-8'>
                    <p><strong>Email Address</strong>: ${recsArray[i].Email} | 
                    <strong>Username</strong>: ${recsArray[i].Username}</p>
                    <p><strong>Description</strong>: ${recsArray[i].Description}</p>
                    <p><strong>Date Submitted</strong>: ${recsArray[i].DateSubmitted}</p>
                    <hr>
                    </div>
                    <div class='col-xs-4 col-sm-4'>
                    </div>
                    </div>`
            $(recContainer).append(rec);
            console.log(rec)
        }
    }
}
