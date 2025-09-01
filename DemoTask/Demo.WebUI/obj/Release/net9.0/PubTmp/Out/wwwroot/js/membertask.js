"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/memberTaskHub").build();

$(function () {
    connection.start().then(function () {
		//alert('Connected to memberTaskHub');

		InvokeMemberTask();
		InvokeUsers();
		InvokeMemberLastUpdate();
		//InvokeEmailNotification();

    }).catch(function (err) {
        return console.error(err.toString());
    });
});

// Product
function InvokeMemberTask() {
	connection.invoke("SendMemberTasks").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedMemberTasks", function (memberTasks) {
	BindMemberTaskToGrid(memberTasks);
});
function BindMemberTaskToGrid(memberTasks) {
	$('#tblMemberTask tbody').empty();
	
	var tr;
	$.each(memberTasks, function (index, memberTask) {
		tr = $('<tr/>');
		tr.append(`<td>${memberTask.id}</td>`);
		tr.append(`<td>${memberTask.name}</td>`);
		tr.append(`<td>${memberTask.remarks}</td>`);

		// Add the edit link column
		tr.append(`
            <td class="RightAlign">
                <a href="/MemberTask/Edit/${memberTask.id}" class="row-edit">
                    <i class="fa fa-edit"></i>
                </a>
            </td>
        `);

		$('#tblMemberTask tbody').append(tr); // append to tbody, not table
	});
}

//function InvokeEmailNotification() {
//	connection.invoke("SendEmailNotification", "testy@gmail.com", "Hello", "This is a test email")
//		.catch(function (err) {
//			return console.error(err.toString());
//		});
//}
//connection.on("EmailStatus", function (successMessage) {
//    console.log(successMessage);
//});

// User
function InvokeUsers() {
	connection.invoke("SendUser").catch(function (err) {
		return console.error(err.toString());
	});
}
connection.on("ReceivedUser", function (userData) {
	
	BindUserButton(userData);
    
});

function BindUserButton(userData) {
	sessionStorage.setItem("roleid", userData.userRoleMasterId);
	if (userData.userRoleMasterId === 3) {
		$("#addnewbtn").hide();
		
	} else {
		$("#addnewbtn").show();

	}
}

function InvokeMemberLastUpdate() {
	connection.invoke("SendMemberTaskLastUpdate").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedMemberTaskLastUpdate", function (memberLastTasks) {

	BindNotification(memberLastTasks);

});
function BindNotification(memberLastTasks) {
	if (memberLastTasks && memberLastTasks.length > 0) {
		var count = parseInt($('span.count').html()) || 0;
		count += memberLastTasks.length;  // add all new tasks
		$('span.count').html(count);
	}
}
