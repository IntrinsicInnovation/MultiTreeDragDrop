//CJ - Created Jquery plugin for Map control
//Usage:
//Display Map Hierarchy:
//
//   $("#divBCEGHierarchy").MapHierarchy({
//            "egId" : bcegId
//         }).css('color', 'black');
//NOTE: This function is chainable, so you can add other functionality as above
//
//Clear Hierarchy:
//
//$("#divBCEGHierarchy").MapHierarchy("clear");
//
//

(function ($) {
    var methods = {
        display: function (options) {
            return this.each(function () {
                var element = this;
                var $this = $(this);
                if (options.egId) {
                    var egid = options.egId;
                    if (isNaN(egid / 1) == false) {
                        var json = JSON.stringify({ egId: egid });
                        var url = "/Services/ElectricalGroupsManagement.asmx/GetMapHierarchy";
                        $.ajax({
                            type: 'POST',
                            contentType: "application/json; charset=utf-8",
                            url: url,
                            data: json,
                            error: function (response, error, details) {
                                //try {
                                var message = "An error has occurred calling GetElectricalGroupHierarchy.";
                                message += "\n";
                                message += "Response: " + response.responseText;
                                message += "\n";
                                message += "Error: " + error;
                                message += "\n";
                                message += "Details: " + details;
                                alert(message);
                                //}
                                //catch (exception) {
                                // HandleException(exception);
                                //}
                            },
                            success: function (data) {
                                var map = data.d;
                                $this.show();
                                loadMapHierarchy($this, map, options.mapId);

                            } //end- success
                        }); //end - ajax
                    }
                }
            });
        },
        clear: function () {
            var $this = $(this);
            $this.hide();
            if ($this.children("a").length > 0) {
                $this.children("a").remove();
            }
        }
    };

    $.fn.MapHierarchy = function (methodname) {
        if (methods[methodname]) {
            return methods[methodname].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodname === 'object' || !methodname) {
            // Default to "init"
            return methods.display.apply(this, arguments);
        } else {
            $.error('Method ' + methodname + ' does not exist on jQuery.MapHierarchy');
        }
    };

    //Miscellaneous functions here:
    function loadMapHierarchy(elements, map, mapId) {
        if (map == null) return;
        elements.children("#egtbElecGroupTreeBranch").remove();
        var parent = $("<div id='egtbElecGroupTreeBranch' class='treeBranch'></div>");
        var child = $("<div class='treeBranchItem'></div>");
        var spacer = $("<div class='treeBranchSpacer'></div>");
        for (var i = 0; i < map.length; i++) {
            var newChild = child.clone();
            $(newChild).text(map[i].Abbreviation);
            $(newChild).attr("title", map[i].Name);
            $(parent).append(newChild);
            if (i < map.length - 1) {
                var newSpacer = spacer.clone();
                $(parent).append(newSpacer);
            }
        }
        $(parent).children("div:first").addClass("first");
        $(parent).children("div:last").addClass("last");
        $(parent).children("div:last").attr("key", map[map.length - 1].Key);
        elements.append(parent);
        if (mapId != undefined && !isNaN(mapId / 1)) {
            $(parent).wrap("<a href='/Pages/Data/MapDefinition.aspx?mapid=" + mapId + "' target='_blank' class='aMap'/>");
        }
    }


})(jQuery);