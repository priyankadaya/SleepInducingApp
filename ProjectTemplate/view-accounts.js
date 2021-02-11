// Created by Varun Shourie

var accountsArray;

// Assume the user is not an admin unless we receive data from the server that only admins can see.
var admin = false;

$(document).ready(function () {
    alert('Window has loaded.');
    loadAccounts();
});

function loadAccounts() {
    var webMethod = "ProjectServices.asmx/GetAccounts";

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d.length > 0) {
                alert("Message has arrived.");
                accountsArray = msg.d;
                admin = false;

                for (var i = 0; i < accountsArray.length; i++) {
                    var acct;
                    if (accountsArray[i].Id != null) {
                        acct = "<div class='accountRow' id='acct" + accountsArray[i].Id + "'>" + accountsArray[i].FirstName
                            "<a href='#' onclick='LoadAccount(" + accountsArray[i].Id + ")' class='optionsTag'>edit</a></div><hr>"
                        admin = true;
                    }

                    $("#active-account-container").append(acct);
                }
            }
        },
        error: function (e) {
            alert('Something went seriously wrong.');
        }
    });
}