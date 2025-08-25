"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/memberTaskHub").build();

$(function () {
    connection.start().then(function () {
		//alert('Connected to projectHub');

		InvokeProjects();
		//InvokeSales();
		//InvokeCustomers();

    }).catch(function (err) {
        return console.error(err.toString());
    });
});

// Product
function InvokeProjects() {
	connection.invoke("SendProjects").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedProjects", function (projects) {
	BindProjectsToGrid(projects);
});
function BindProjectsToGrid(products) {
	$('#tblProjects tbody').empty();
	console.log(products);
	var tr;
	$.each(products, function (index, product) {
		tr = $('<tr/>');
		tr.append(`<td>${product.id}</td>`);
		tr.append(`<td>${product.name}</td>`);
		tr.append(`<td>${product.startDate}</td>`);
		tr.append(`<td>${product.endDate}</td>`);
		tr.append(`<td>${product.managerName}</td>`);
		tr.append(`<td>${product.remarks}</td>`);

		// Add the edit link column
		tr.append(`
            <td class="RightAlign">
                <a href="/Project/Edit/${product.id}" class="row-edit">
                    <i class="fa fa-edit"></i>
                </a>
            </td>
        `);

		$('#tblProjects tbody').append(tr); // append to tbody, not table
	});
}
