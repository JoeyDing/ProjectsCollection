//*------------------------------------------------------------------------------------------------------------------
//                                                  modal window
//*------------------------------------------------------------------------------------------------------------------

/* show/hide/draggable */

function ShowExistingWindow(windowName) {
    $('.modal').off('show.bs.modal', centerModals);
    $('.modal').on('show.bs.modal', centerModals);
    $("#" + windowName).closest("div.modal.fade").modal('show');
    $("#" + windowName).closest("div.modal.fade").draggable();
}

function CloseWindow(windowName) {
    $('#' + windowName).closest("div.modal.fade").modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}

function RefreshWindow(windowName) {
    $('#' + windowName).closest("div.modal.fade").modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
    ShowExistingWindow(windowName);
}

function BootStrap_CloseWindow(windowName, args) {
    var window = $('#' + windowName).closest("div.modal.fade");
    window.modal('hide');
    //$('body').removeClass('modal-open');
    //$('.modal-backdrop').remove();
    //args._domEvent.stopPropagation();
}

/* center */

function centerModals() {
    $('.modal').each(function (i) {
        var $clone = $(this).clone().css('display', 'block').appendTo('body');
        var top = Math.round(($clone.height() - $clone.find('.modal-content').height()) / 2);
        top = top > 0 ? top : 0;
        $clone.remove();
        $(this).find('.modal-content').css("margin-top", top);
    });
}

$(document).ready(function () {
    $(window).on('resize', centerModals);
    //Sys.Application.add_load(master_pageLoad);
});

//function master_pageLoad() {
//    $('.modal').on('show.bs.modal', centerModals);
//}

//*------------------------------------------------------------------------------------------------------------------

//*------------------------------------------------------------------------------------------------------------------
//                                                  radlistbox
//*------------------------------------------------------------------------------------------------------------------
function OnClientSelectedIndexChanging(sender, args) {
    args.set_cancel(true);
}

//function OnClientItemChecking(sender, args) {
//    //args.set_cancel(true);
//}

//*------------------------------------------------------------------------------------------------------------------

function OnRadPanelBarExpand(sender, args) {
    var handler = $telerik.$('span.rpExpandHandle');
    //get the clicked handler element
    if (handler) {
        //perform postback
        __doPostBack();
    }
    else {
        //do something else
    }
}

function clientCheckAll(sender, args) {
    __doPostBack(sender._uniqueId, '{"type":6,"ItemIndex":0}');
}