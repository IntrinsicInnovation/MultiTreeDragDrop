
function InputLimiter() {
}

   InputLimiter.prototype.getCaretPosition = function (ctrl) {
        var CaretPos = 0;
        // IE Support
        if (document.selection) {
            ctrl.focus();
            var Sel = document.selection.createRange();
            //var ctrllen = (ctrl.val ? ctrl.val.length : 0);
            Sel.moveStart('character', -ctrl.val.length);
            CaretPos = Sel.text.length;
        }
        // Firefox support
        else if (ctrl.selectionStart || ctrl.selectionStart == '0')
            CaretPos = ctrl.selectionStart;
        return (CaretPos);
    }

    //CJ - Wrote function to only allow a well formed optionally negative decimal value.
    InputLimiter.prototype.checkNumeric = function (keypressed) {
        var ctrl = $("#" + keypressed.target.id);
        var key = keypressed.which || keypressed.keyCode;
        var strtext = ctrl.val();
        var caretposition = this.getCaretPosition(ctrl);
        if (!keypressed.shiftKey && !keypressed.altKey && !keypressed.ctrlKey &&
        // numbers   
                             key >= 48 && key <= 58 ||
        // Numeric keypad
                             key >= 96 && key <= 105 ||
        // comma, period and minus, . on keypad & main keyboard.
                             ((key == 190 || key == 110) && strtext.indexOf('.') == -1) ||
                             ((key == 109 || key == 189) && this.caretposition == 0 && strtext.indexOf('-') == -1) ||
        // Backspace and Tab and Enter
                            key == 8 || key == 9 || key == 13 ||
        // Home and End
                            key == 35 || key == 36 ||
        // left and right arrows
                            key == 37 || key == 39 ||
        // Del and Ins
                            key == 46 || key == 45) {
            return true;
        }
        return false;
    }


    function inputValidator() {
    }