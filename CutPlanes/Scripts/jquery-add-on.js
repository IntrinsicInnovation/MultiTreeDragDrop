
$.urlParam = function (name) {
    var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (!results) {
        return 0;
    }
    return results[1] || 0;
}

$.numericInputValidation = function (textbox) {
    $(textbox).keydown(function (e) {
        if (e.keyCode == 190 || e.keyCode == 110) {  // only allow one decimal point
            if ($(this).val().indexOf(".") > -1) {
                e.preventDefault();
            } else return;
        }
        if ((!e.shiftKey && e.keyCode >= 48 && e.keyCode <= 57) ||
                             (!e.shiftKey && e.keyCode >= 96 && e.keyCode <= 105) ||
                              e.keyCode == 46 || e.keyCode == 8) {
            return;
        } else {
            e.preventDefault();
        }
    });
}

$.toPercentage = function (val) {
    var f = parseFloat(val);
    if (isNaN(f)) { return null; }
    return (f * 100).toString() + "%";
}

$.trim = function (val) {
    return val.replace(/^\s+|\s+$/g, '');
}

$.toFixedOrNull = function (number, fractionDigits) {
    if (number == null || isNaN(number / 1) || fractionDigits == null || isNaN(fractionDigits / 1)) return null;
    return number.toFixed(fractionDigits);
}
