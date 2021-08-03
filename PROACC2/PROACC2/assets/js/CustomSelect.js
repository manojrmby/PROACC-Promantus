    function Select(TableWithHeaderId, DivId, DropdDownId, DynamicTableClassName) {
        var header = TableWithHeaderId.find('thead tr td');
        var select = '<select id="testselect" multiple>';
        var option = [];
        var headerObj = [];
        var className = [];
        for (var i = 0; i < header.length; i++) {
            className[i] = header[i].className;
            headerObj[i] = header[i].innerText.trim();
            if (className[i] == "addSelect") {
                option[i] = '<option value="' + i + '" class="' + className[i] + '" scope="col">' + headerObj[i].toUpperCase() + '</option>';
            }
        }
        select = select + option + "</select>";
        DivId.append(select);
        DropdDownId.attr('disabled', 'disabled');

        loadScript("~/Assets/DataTable/bootstrap-multiselect.js", "js");
        loadScript("~/Assets/DataTable/bootstrap-multiselect.css", "css");

        DropdDownId.multiselect({
            disableIfEmpty: true,
            includeResetOption: true,
            resetText: "Reset all",
            onChange: function (option, checked, select) {
                if (checked) {
                    var val = $(option).text().toUpperCase();
                    $.each(headerObj, function (data, item) {
                        var val2 = headerObj[data].trim().toUpperCase();
                        if (val2 == val) {
                            var originalindex = data;
                            DynamicTableClassName.find('tr :nth-child(' + (originalindex + 1) + ')').toggle();
                        }
                    });
                }
                else {
                    DynamicTableClassName.find('tr td:hidden').show();
                }
                $(".btn-block").click(function () {
                    TableClass.find('tr td:hidden').show();
                });
            }
        });
    }

function loadScript(filename, filetype) {
    if (filetype == "js") { //if filename is a external JavaScript file
        var fileref = document.createElement('script')
        fileref.setAttribute("type", "text/javascript")
        fileref.setAttribute("src", filename)
    }
    else if (filetype == "css") { //if filename is an external CSS file
        var fileref = document.createElement("link")
        fileref.setAttribute("rel", "stylesheet")
        fileref.setAttribute("type", "text/css")
        fileref.setAttribute("href", filename)
    }
    if (typeof fileref != "undefined")
        document.getElementsByTagName("head")[0].appendChild(fileref)
}
