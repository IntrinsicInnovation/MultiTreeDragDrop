function GetQueryStringParam(variableName) {
    try {
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');

        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] == variableName) {
                return sParameterName[1];
            }
        }
    }
    catch (exception) {
        HandleException(exception);
    }
}

function isDateValid(date) {
    try {
        var s = date.split('-');
        if (s.length != 3) {
            return false;
        }
        else {
            var year = s[0];
            if (year.length != 4) return false;
            var month = s[1];
            if (month.length != 2) return false;
            var day = s[2];
            if (day.length != 2) return false;
            if (!(year >= 0 && year <= 9999)) return false;
            if (!(month >= 1 && month <= 12)) return false;
            if (!(day >= 1 && day <= 31)) return false;

            return true;
        }
    }
    catch (exception) {
        HandleException(exception);
    }
}
