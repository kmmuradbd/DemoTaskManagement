var nice = false;

$(function () {
    if (typeof $.validator != "undefined") {
        $.validator.setDefaults({ ignore: '.NoValidation' });
    }
    UyDatePickers();
    $(".TimePicker").timepicki();
    mainmenu();
    HelperDropDowns();
    HelperCascadingDropDowns();
    InitDialogs();
    MobileMenu();
    InitButtons();
    nice = $("html").niceScroll();
    $('table:not(.NoPaging)').footable().on('click', '.row-delete', function (e) {
        e.preventDefault();
        //get the footable object
        var footable = $('table').data('footable');

        //get the row we are wanting to delete
        var row = $(this).parents('tr:first');

        //delete the row
        footable.removeRow(row);
    });
});

$('#calendar').fullCalendar({
    defaultDate: '2014-07-09',
    editable: true,
});

function UyDatePickers() {
    $(".DatePicker").datepicker({ format: 'dd-M-yyyy', autoclose: true })
}

function ParseDate(input) {
    var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var parts = input.split('-');
    return new Date(parts[2], months.indexOf(parts[1]), parts[0]);
}

$('#mobileNav li a').on('click', function () {
    $(this).next('ul').slideToggle();
    $(this).find('i').toggleClass('fa fa-angle-right fa fa-angle-down');
});

$('#category-tabs li a').on('click', function () {
    $(this).next('ul').slideToggle();
    $(this).find('i').toggleClass('fa fa-angle-right fa fa-angle-down');
});

$('.sidebar ul li ul li').on('click', function () {
    $('.sidebar ul li ul li').removeClass("active")
    $(this).addClass("active")
});

$('.itemPagination ul li ul li').on('click', function () {
    $('.itemPagination ul li ul li').removeClass("active")
    $(this).addClass("active")
});

$('.dropdown li a').on('click', function () {
    $('.mainInner>section').hide();
    $('#menu_' + $(this).attr('id')).show();
});

$(document).on('click', '.close-list', function () {
    $(this).closest('li').hide(500);
});

//$('.user-form').find('input, textarea, select').attr('disabled', 'disabled');

$(".btn-active").on('click', function () {
    $('.user-form').find('input, textarea, select').removeAttr('disabled');
    $(this).text(function (i, text) {
        return text === "save" ? "save" : "save";
    })
});

$(".topMenulink").on('click', function () {
    $(".navigation").slideToggle(500);
});

function mainmenu() {
    $(" #nav ul ").css({ display: "none" }); // Opera Fix
    $(" #nav li").hover(function () {
        $(this).find('ul:first').css({ visibility: "visible", display: "none" }).show(400);
    }, function () {
        $(this).find('ul:first').css({ visibility: "hidden" });
    });
}

function displayResult() {
    document.getElementById("infoTable").insertRow(-1).innerHTML = '<td><a href="#">Bangladeshi Taka </a> </td><td>30417</td>  <td>--</td><td>18250.2</td> <td>9125.1</td> <td>2129.19</td> <td>912.51</td>  <td><a class="remove" href="#"><i class="fa fa-remove"></i></a></td>';
}

$('.table').on('click', 'tr td a.remove', function (e) {
    e.preventDefault();
    $(this).closest('tr').remove();
});

//=====================================================================================================================================
//=====================================================================================================================================
//=====================================================================================================================================
function HelperDropDowns() {
    var dropdownElements = $('select.Dropdown:not(.DropdownInited)');
    $.each(dropdownElements, function (index, element) {
        var dropdownEl = $(element);
        var url = dropdownEl.attr('data-url');
        var selected = dropdownEl.attr('data-selected');
        var dataCache = dropdownEl.attr('data-cache') ? true : false;
        $.ajax({
            url: url,
            type: 'GET',
            cache: dataCache,
            success: function (jsonData, textStatus, XMLHttpRequest) {
                var Listitems = '<option value="">--select--</option>';
                $.each(jsonData, function (i, item) {
                    if (selected && selected == item.Value) {
                        Listitems += "<option selected='selected' value='" + item.Value + "'>" + item.Text + "</option>";
                    }
                    else {
                        Listitems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                    }
                });
                dropdownEl.html(Listitems).addClass("DropdownInited");
            }
        });
    });
}

function HelperCascadingDropDowns() {
    var dependentElements = $('select.Cascading:not(.DropdownInited)');
    $.each(dependentElements, function (index, element) {
        var dependentEl = $(element);
        var parentEl = $('#' + dependentEl.attr('data-parent'));
        var url = dependentEl.attr('data-url');
        var selected = dependentEl.attr('data-selected');
        var dataCache = dependentEl.attr('data-cache') ? true : false;
        var loadDropDownItems = function () {
            if (!parentEl.val()) {
                if (selected) {
                    setTimeout(loadDropDownItems, 300);
                }
                return;
            }
            $.ajax({
                url: url + parentEl.val(),
                type: 'GET',
                cache: dataCache,
                success: function (jsonData, textStatus, XMLHttpRequest) {
                    var Listitems = '<option></option>';
                    $.each(jsonData, function (i, item) {
                        if (selected && selected == item.Value) {
                            Listitems += "<option selected='selected' value='" + item.Value + "'>" + item.Text + "</option>";
                        }
                        else {
                            Listitems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        }
                    });
                    dependentEl.html(Listitems).addClass("DropdownInited");
                }
            });
        };
        parentEl.change(loadDropDownItems);
        if (selected) {
            loadDropDownItems();
        }
    });
}

function InitDialogs() {
    $('a.Dialog:not(.DialogInited)').on("click", function () {
        var url = $(this).attr('href');
        $.ajax({
            url: url,
            type: 'GET',
            cache: false,
            success: function (responseText, textStatus, XMLHttpRequest) {
                var html = '' +
                    '<div class="modal fade" id="addNewForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                        '<div class="modal-dialog">' +
                            '<div class="modal-content">' +
                                '<div class="modal-body">' +
                                responseText +
                                '</div>' +
                            '</div>' +
                        '</div>' +
                    '</div>';
                var dialogWindow = $(html).appendTo('body');
                dialogWindow.modal({ backdrop: 'static' });
            }
        });
        return false;
    }).addClass("DialogInited");
}

function CloseModal(el, dataAction, dataUrl) {
    if (dataAction == "formsubmit") {
        $("#" + dataUrl).submit();
    }
    else if (dataAction == "refreshparent") {
        window.parent.location = window.parent.location;
    }
    else if (dataAction == "refreshself") {
        window.location = window.location;
    }
    else if (dataAction == "redirect") {
        window.location = dataUrl;
    }
    else if (dataAction == "function") {
        eval(dataUrl);
    }
    var win = $(el).closest(".modal");
    win.modal("hide");
    setTimeout(function () {
        win.next(".modal-backdrop").remove();
        win.remove();
        //$("body").removeClass("modal-open");
    }, 500);
}

function MobileMenu() {
    $(".topMenulink").click(function () {
        $(".navigation").slideToggle(500);
    });
}

function UYResult(msg, status, dataAction, dataUrl) {
    var html = '' +
            '<div id="userUpdate" class="modal fade">' +
                '<div class="modal-dialog">' +
                    '<div class="modal-content">' +
                        '<div class="modal-header modal-title-' + status + '">' +
                            '<h4 class="modal-title">SOFTWARE GAZE</h4>' +
                        '</div>' +
                        '<div class="modal-body">' +
                            '<p>' + msg + '</p>' +
                        '</div>' +
                        '<div class="modal-footer">' +
                            '<button type="button" class="btn btn-primary" data-dismiss="modal" onclick="CloseModal(this, \'' + dataAction + '\',\'' + dataUrl + '\')">OK</button>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>';
    var dialogWindow = $(html).appendTo('body');
    dialogWindow.modal({ backdrop: 'static' });
}

function UYAsk(msg, dataAction, dataUrl) {
    var html = '' +
            '<div id="userConfirm" class="modal fade">' +
                '<div class="modal-dialog">' +
                    '<div class="modal-content">' +
                        '<div class="modal-header">' +
                            '<h4 class="modal-title">SUNSOFT</h4>' +
                        '</div>' +
                    '<div class="modal-body">' +
                        msg +
                    '</div>' +
                        '<div class="modal-footer">' +
                            '<button type="button" class="btn btn-primary" onclick="CloseModal(this, \'' + dataAction + '\',\'' + dataUrl + '\')">YES</button>' +
                            '<button type="button" class="btn btn-primary btn-close" onclick="CloseModal(this)">NO</button>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>';
    var dialogWindow = $(html).appendTo('body');
    dialogWindow.modal({ backdrop: 'static' });
}

function InitButtons() {
    $('.AddRow:not(.AddRowInited)').on("click", function () {
        var url = $(this).attr('data-url');
        var container = $(this).attr('data-container');
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            success: function (html) {
                $("#" + container).append(html);
            }
        });
        return false;
    }).addClass("AddRowInited");

    $('.RemoveRow:not(.RemoveRowInited)').on("click", function () {
        $(this).parents("div.row:first").remove();
        return false;
    }).addClass("RemoveRowInited");
}

function SelectBoxValidation() {
    var dropdownElements = $('select)');
    var msg = dropdownElements.attr('data-val-required');
    var id = dropdownElements.attr('id');

    if ($("#" + id).val() == "--select--") {
        $("*[data-valmsg-for=" + id + "]").removeClass("field-validation-valid").addClass("field-validation-error").text(msg);
        return false;
    }
}