// Created by Varun Shourie

$(document).ready(function () {
    loadAccounts();
    toggleToActive();
});

function checkIfAdmin(boolFlag) {
    if (boolFlag) {
        return "Yes";
    }
    return "No";
}

function deleteAccount(id) {
    var webMethod = "ProjectServices.asmx/DeleteAccount";

    if (confirm("Do you want to delete the selected user's account?")) {
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
                    $(`#acct${id}`).remove();
                }
            },
            error: function (e) {
                alert("Something went wrong...");
            }
        });
    }
}

function loadAccounts() {
    var webMethod = "ProjectServices.asmx/GetAccounts";

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d.length > 0) {
                allAccounts = msg.d;

                populateAccounts(allAccounts[0], "#inactive-accounts");
                populateAccounts(allAccounts[1], "#active-accounts");
            }
        },
        error: function (e) {
            alert('Something went seriously wrong.');
        }
    });
}

function populateAccounts(accountsArray, accountContainer) {
    for (var i = 0; i < accountsArray.length; i++) {
        var acct;
        if (accountsArray[i].Id != null) {
            acct = `<div class='row' id='acct${accountsArray[i].Id}' class='text-center'>
                    <div class='col-xs-8 col-sm-8'>
                    <p><strong>${accountsArray[i].FirstName} ${accountsArray[i].LastName}</strong> | 
                    <strong>Admin</strong>: ${checkIfAdmin(accountsArray[i].IsAdmin)}</p>
                    <p><strong>Email Address</strong>: ${accountsArray[i].Email} | 
                    <strong>Username</strong>: ${accountsArray[i].Username}</p>
                    <p><strong>Days Inactive</strong>: ${accountsArray[i].DaysInactive}</p>
                    <hr>
                    </div>
                    <div class='col-xs-4 col-sm-4'>
                    <button type='button' onclick='deleteAccount(${accountsArray[i].Id})' class='btn btn-danger'>Delete</button>
                    </div>
                    </div>`
            $(accountContainer).append(acct);
        }
    }
}

function toggleToActive() {
    $("#inactive-account-container").css("display", "none");
    $("#active-account-container").css("display", "block");
    $("#toggle-button").attr("onclick", "toggleToInactive()");
}

function toggleToInactive() {
    $("#inactive-account-container").css("display", "block");
    $("#active-account-container").css("display", "none");
    $("#toggle-button").attr("onclick", "toggleToActive()");
}
