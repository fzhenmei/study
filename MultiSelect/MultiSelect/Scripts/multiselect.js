function multiSelector() {
    this.timoutID = null;
    this.init = function (eleID) {
        var rootJqID = "#" + eleID;
        var textbox = $(rootJqID + " input:text");
        var checkboxes = $(rootJqID + " input:checkbox").each(function (index) {
            $(this).click(function () {
                var text = "";
                var needComma = false;
                $(rootJqID + " input:checked").each(function () {
                    var labelText = $(this).next().text();
                    text += needComma ? "," + labelText : labelText;
                    needComma = true;
                });
                textbox.val(text);
            });
        });

        $(rootJqID + " .multi_drop_btn").click(function () {
            showBox(eleID);
        });
    };
    this.show = function (eleID) {
        showBox(eleID);
    };
    showBox = function (eleID) {
        var rootJqID = "#" + eleID;
        $(rootJqID + " .checkBoxList").css("display", "block");
    }
    this.hide = function (eleID) {
        this.timoutID = setTimeout('hideBox("' + eleID + '")', 750);
    };
    hideBox = function (eleID) {
        var rootJqID = "#" + eleID.replace(/\$/g, "\\$");
        $(rootJqID + " .checkBoxList").css("display", "none");
    };
}