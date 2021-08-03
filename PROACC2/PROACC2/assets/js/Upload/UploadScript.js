function Upload_Url(Server_url) {
    
}
    


$(function () {
    /*
     * For the sake keeping the code clean and the examples simple this file
     * contains only the plugin configuration & callbacks.
     *
     * UI functions ui_* can be located in: demo-ui.js
     */
    
    /*
 Global controls
*/
    
    //$('.btnApiStart').on('click', function (evt) {
    //    //console.log('in');
    //    evt.preventDefault();
    //    $('#drag-and-drop-zone').dmUploader('start',7);
    //});

    //$('#btnApiCancel').on('click', function (evt) {
    //    evt.preventDefault();

    //    $('#drag-and-drop-zone').dmUploader('cancel');
    //});
    //$('#btnReset').on('click', function (evt) {
    //    $("#drag-and-drop-zone").dmUploader("reset");
    //    clearQ();
    //});
    /*
      Each File element action
     */
    $('#files').on('click', 'button.start', function (evt) {
        evt.preventDefault();
        var id = $(this).closest('li.media').data('file-id');
        $('#drag-and-drop-zone').dmUploader('start', id);
    });

    $('#files').on('click', 'button.cancel', function (evt) {
        evt.preventDefault();

        var id = $(this).closest('li.media').data('file-id');
        $('#drag-and-drop-zone').dmUploader('cancel', id);
    });
   
});

/*
* Some helper functions to work with our UI and keep our code cleaner
*/

// Adds an entry to our debug area
function ui_add_log(message, color) {
    var d = new Date();

    var dateString = (('0' + d.getHours())).slice(-2) + ':' +
        (('0' + d.getMinutes())).slice(-2) + ':' +
        (('0' + d.getSeconds())).slice(-2);

    color = (typeof color === 'undefined' ? 'muted' : color);

    var template = $('#debug-template').text();
    template = template.replace('%%date%%', dateString);
    template = template.replace('%%message%%', message);
    template = template.replace('%%color%%', color);

    $('#debug').find('li.empty').fadeOut(); // remove the 'no messages yet'
    $('#debug').prepend(template);
}

// Creates a new file and add it to our list
function ui_multi_add_file(id, file) {
    var template = $('#files-template').text();
    template = template.replace('%%filename%%', file.name);

    template = $(template);
    template.prop('id', 'uploaderFile' + id);
    template.data('file-id', id);

    $('#files').find('li.empty').fadeOut(); // remove the 'no files yet'
    $('#files').prepend(template);
}

function ui_multi_add_file_SAP(id, file) {
    var template = $('#files-template_SAP').text();
    template = template.replace('%%filename%%', file.name);

    template = $(template);
    template.prop('id', 'uploaderFile' + id);
    template.data('file-id', id);

    $('#files1').find('li.empty').fadeOut(); // remove the 'no files yet'
    $('#files1').prepend(template);
}
// Changes the status messages on our list
function ui_multi_update_file_status(id, status, message) {
    $('#uploaderFile' + id).find('span').html(message).prop('class', 'status text-' + status);
}

// Updates a file progress, depending on the parameters it may animate it or change the color.
function ui_multi_update_file_progress(id, percent, color, active) {
    color = (typeof color === 'undefined' ? false : color);
    active = (typeof active === 'undefined' ? true : active);

    var bar = $('#uploaderFile' + id).find('div.progress-bar');

    bar.width(percent + '%').attr('aria-valuenow', percent);
    bar.toggleClass('progress-bar-striped progress-bar-animated', active);

    if (percent === 0) {
        bar.html('');
    } else {
        bar.html(percent + '%');
    }

    if (color !== false) {
        bar.removeClass('bg-success bg-info bg-warning bg-danger');
        bar.addClass('bg-' + color);
    }
}


function clearQ() {
    $('#drag-and-drop-zone').dmUploader('cancel');
    // clear any files currently in queue
    $('.media').remove();
    // restore 'No files uploaded' message in user-visible file queue
    $('#files').find('li.empty').fadeIn();
    // disable "Clear Queue" button until more files have been uploaded
    ResetColor();
    $('#btnReset').hide();
    $('.progress-bar').removeClass('bg-danger').removeClass('bg-primary').addClass('bg-secondary');

    var DefaultList = ['Activities', 'Bwextractors', 'CustomCode', 'HanaDatabaseTables', 'RecommendedFioriApps', 'RelevantSimplificationItems', 'SAP Readiness Check', 'UserList'];
    for (var i = 0; i < DefaultList.length; i++) {
        var text = DefaultList[i].toLocaleUpperCase();
        var id = text.substring(0, 3);
        $('#' + id).removeClass('bg-danger').addClass('bg-secondary').removeClass('bg-primary');
    }

    
    // get reference to dmUploader plugin in order to access queue-related variables
    //var myUploader = $('#drag-and-drop-zone').dmUploader().data();
    //myUploader.queue = [];
    //myUploader.queuePos = -1;
    //myUploader.activeFiles = 0;
}
function ResetColor() {

    var DefaultList = ['Activities', 'Bwextractors', 'CustomCode', 'HanaDatabaseTables', 'RecommendedFioriApps', 'RelevantSimplificationItems', 'SAP Readiness Check', 'UserList'];
    //for (var i = 0; i < DefaultList.length; i++) {
    //    var text = DefaultList[i].toLocaleUpperCase();
    //    var id = text.substring(0, 3);
    //    $('#' + id).addClass('bg-danger');
    //}

    for (var j = 0; j < DefaultList.length; j++) {
        var text = DefaultList[j].toLocaleUpperCase();
        var id = text.substring(0, 3);
        $('#' + id).removeClass('bg-danger').removeClass('bg-success').removeClass('bg-primary').addClass('bg-secondary');

    }

}