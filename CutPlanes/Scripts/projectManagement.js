var facilities = null;
var resourceBundles = null;
var generatingUnits = null;
var loads = null;


/**********************************Associated Resources Load***********************************/
function toggleSubEntitList(el) {
    var img = $(el).attr("src");
    img = img == "/Images/minus.GIF" ? "/Images/plus.GIF" : "/Images/minus.GIF";
    $(el).attr("src", img);
    //innerResources
    if ($("ul:hidden", $(el).parent()).length > 0) {
        $(el).parent().children(".innerResources").show();
    }
    else {
        $(el).parent().children(".innerResources").hide();
    }
}

/**********************************END Associated Resources Load***********************************/
/**********************************Project Facility Selection & Maintenance modal***********************************/

function postFacility(addToResourceList, serviceUrl, projectId, facilityId, tla, name, lat, lon, poi1, poi2, poiVoltage, poiCrctDesig, typeId, statusTypeId, ownershipTypeId, fuelTypeId, resourcePlanId, measures, voltageLevelArray, phaseId, masterList, powerCallId, probability) {
    if (addToResourceList == true) {
        $.ajaxSetup({ async: false, timeout: 10000 });   // need to wait for response to return Id.
    } else {
        $.ajaxSetup({ async: true, timeout: 10000 });
    }
    $.post(serviceUrl, {pjid: projectId, sid: facilityId, tla: tla, name: name, lat: lat, lon: lon, poi1: poi1, poi2: poi2, poiVoltage: poiVoltage, poiCrctDesig: poiCrctDesig, typeId: typeId, statusTypeId: statusTypeId, ownershipTypeId: ownershipTypeId, fuelTypeId: fuelTypeId, resourcePlanId: resourcePlanId, Pdgc: measures.Pdgc, Pmpo: measures.Pmpo, Pelcc: measures.Pelcc, PSys: measures.PSys, Pptp: measures.Pptp, Prmr: measures.Prmr, Pndc: measures.Pndc, Pdmx0: measures.Pdmx0, Pdmx1: measures.Pdmx1, Pnp: measures.Pnp, Pir: measures.Pir, LdGrSpk: measures.LdGrSpk, LdGrSmn: measures.LdGrSmn, LdGrWpk: measures.LdGrWpk, LdGrWmn: measures.LdGrWmn, voltageLevels: voltageLevelArray, phaseId: phaseId, UseMasterPdgc: masterList.UseMasterPdgc, UseMasterPelcc: masterList.UseMasterPelcc, UseMasterPptp: masterList.UseMasterPptp, UseMasterPndc: masterList.UseMasterPndc, UseMasterPdmx0: masterList.UseMasterPdmx0, UseMasterPdmx1: masterList.UseMasterPdmx1, UseMasterPmpo: masterList.UseMasterPmpo, UseMasterPrmr: masterList.UseMasterPrmr, UseMasterPnp: masterList.UseMasterPnp, pcId: powerCallId, prob: probability },
                        function (data) {
                            $("#ipFacilityId").val(data.Id);
                            if (addToResourceList) {
                                addFacilityToResourceList(data);
                            }
                        }, "json"
    );
}

function initFacilitySearch(serviceUrl) {
    $("#txtFacilitySearch").focusin(function () {
        if (facilities == null) {
            $.get(serviceUrl, function (data) {
                if (data == null) return;
                facilities = data;
                $("#txtFacilitySearch").autocomplete({
                    minLength: 0,
                    source: facilities,
                    select: function (event, ui) {                        
                        if (!validateNewResource(ui.item.Id, ui.item.Type)) {
                            alert("The selected facility is already part of this phase.");                            
                            return;
                        }
                        facilitySelected(ui.item.Id);
                        $("#divCreateFacility").show();
                    }
                });
            }, "json");
        }
    });
}

function initFacilityMeasuresGrids() {
    renderMeasuresGrid("divFacilityGenMeasures", "Summary_Generator");
    renderMeasuresGrid("divFacilityLoadMeasures", "Load");
}

function parentEntityDoubleClicked(el) {
    var entityId = $(".idColumn:first", $(el).parent()).text();
    var type = $(".resourceType:first", $(el).parent()).text();    
    if (type == "Resource Bundle") { // 
        moveBCEGHierarchyControl("divBcegResBundleTarget");
        resourceBundleSelected(entityId);
        displayUpdateResourceBundleAttributesMode();        
        $("#divResourceBundleMaintenance").dialog("open");
    } 
    else {    // substation has muliptle sub-types        
        moveBCEGHierarchyControl("divBcegFacilityTarget");
        facilitySelected(entityId);
        displayUpdateFacilityMode();        
        $("#divFacilityMaintenance").dialog("open");
    }    
}

function childEntityDoubleClicked(el) {
    var type = $(el).children(".resourceType").text();
    var id = $(el).children(".idColumn").text();
    var facilityId = $(el).parent().parent().children(".idColumn").text();
    if (type == "Load") {
        displayUpdateLoadAttributesMode();       
        loadVoltageLevels("ddlLoadVoltageLevel", facilityId);
        loadSelectedCallback(id);
        $("#ipLoadParentId").val(facilityId);
        $("#divLoadMaintenance").dialog("open");
    }
    else {
        displayUpdateGeneratingUnitAttributesMode();
        loadVoltageLevels("ddlGenUnitVoltageLevel", facilityId);
        generatingUnitSelectedCallback(id);
        $("#ipGenUnitParentId").val(facilityId);
        $("#divGenUnitMaintenance").dialog("open"); 
    }
}

function clearFacilityDialog() {
    $("#ipFacilityId").val("");
    $("#ddlFacilityType").val("-1");
    $("input[id*='etbFacilityTla']").val("");
    $("input[id*='etbFacilityName']").val("");
    $("input[id*='etbFacilityLatitude']").val("");
    $("input[id*='etbFacilityLongitude']").val("");
    $("input[id*='etbFacilityProb']").val("");
    $("input[id*='etbFacilityPoi1']").val("");
    $("input[id*='etbFacilityPoi2']").val("");
    $("input[id*='etbFacilityPoiVoltage']").val("");
    $("input[id*='etbFacilityPoiCctDesig']").val("");
    $("#ddlFacilityOwnershipType").val("-1");
    $("#ddlFacilityFuelType").val("-1");
    $("#ddlFacilityPowerCall").val("-1");
    $("div[id*='emFacilityMetaData_divCreatedBy']").text("");
    $("div[id*='emFacilityMetaData_divDateCreated']").text("");
    $("div[id*='emFacilityMetaData_divModifiedBy']").text("");
    $("div[id*='emFacilityMetaData_divDateModified']").text("");
    // clear Gen Sum Measures
    clearMeasuresGrid("divFacilityGenMeasures");
    // clear Load Sum Measures
    clearMeasuresGrid("divFacilityLoadMeasures");    
    $("#txtFacilitySearch").val("");
    //clearElectricalGroupHierarchy();
    $("#divBCEGHierarchy").MapHierarchy("clear");
    clearVoltageLevels();
}

function showFacilityFields() {
    $('#divFacilityDetails').show();
}

function hideFacilityFields() {
    $('#divFacilityDetails').hide();
}

function loadFacilityDetails(facilityAttributes) {
    clearMeasuresGrid("divFacilityGenMeasures");
    clearMeasuresGrid("divFacilityLoadMeasures");
    $("#ddlFacilityType").attr("disabled", "disabled");    
    $("#ddlFacilityOwnershipType").attr("disabled", "disabled");
    $("#ddlFacilityFuelType").attr("disabled", "disabled");
    $("#ddlFacilityPowerCall").attr("disabled", "disabled");
    $("input[id*='etbFacilityTla']").attr("disabled", "disabled");
    $("input[id*='etbFacilityName']").attr("disabled", "disabled");
    $("input[id*='etbFacilityName']").attr("disabled", "disabled");
    $("input[id*='etbFacilityLatitude']").attr("disabled", "disabled");
    $("input[id*='etbFacilityLongitude']").attr("disabled", "disabled");
    $("input[id*='etbFacilityProb']").attr("disabled", "disabled");
    $("input[id*='etbFacilityPoi1']").attr("disabled", "disabled");
    $("input[id*='etbFacilityPoi2']").attr("disabled", "disabled");
    $("input[id*='etbFacilityPoiVoltage']").attr("disabled", "disabled");
    $("input[id*='etbFacilityPoiCctDesig']").attr("disabled", "disabled");
    $("#txtAddVoltageLevels").hide();
    $("#divAddButton").hide();
    $("#txtBCEGSearch").hide(); 

    $("input[id*='etbFacilityResourcePlan']").val($("#ddlProjectResourcePlan option:selected").text())
    $("#ddlFacilityType").val(facilityAttributes.TypeId);
    $("#ipFacilityId").val(facilityAttributes.Id);
    $("input[id*='etbFacilityTla']").val(facilityAttributes.Identifier);
    $("input[id*='etbFacilityName']").val(facilityAttributes.Name);
    $("input[id*='etbFacilityLatitude']").val(facilityAttributes.Latitude);
    $("input[id*='etbFacilityLongitude']").val(facilityAttributes.Longitude);
    $("input[id*='etbFacilityProb']").val(facilityAttributes.Probability);
    $("input[id*='etbFacilityPoi1']").val(facilityAttributes.Poi1);
    $("input[id*='etbFacilityPoi2']").val(facilityAttributes.Poi2);
    $("input[id*='etbFacilityPoiVoltage']").val(facilityAttributes.PoiVoltage);
    $("input[id*='etbFacilityPoiCctDesig']").val(facilityAttributes.PoiCircuitDesig);

    $("#ddlFacilityStatus").val(SetDropDown(facilityAttributes.StatusId));
    $("input[id*='etbFacilityStatus']").val("Planned");
    $("#ipFacilityStatusId").val(facilityAttributes.StatusId);
    $("#ddlFacilityOwnershipType").val(SetDropDown(facilityAttributes.OwnershipTypeId));
    $("#ddlFacilityFuelType").val(SetDropDown(facilityAttributes.FuelTypeId));
    $("#ddlFacilityPowerCall").val(SetDropDown(facilityAttributes.PowerCallId));
    $("div[id*='emFacilityMetaData_divCreatedBy']").text(facilityAttributes.UserCreated);
    $("div[id*='emFacilityMetaData_divDateCreated']").text(facilityAttributes.DateCreated);
    $("div[id*='emFacilityMetaData_divModifiedBy']").text(facilityAttributes.UserModified);
    $("div[id*='emFacilityMetaData_divDateModified']").text(facilityAttributes.DateModified);
    if (facilityAttributes.ChildEntities != undefined) {
        $("#divChildEntityCache").text($.toJSON(facilityAttributes.ChildEntities));
    }
    else {
        $("#divChildEntityCache").text("");
    }
    setSummaryGeneratorMeasures("divFacilityGenMeasures", facilityAttributes);
    setSummaryLoadMeasures("divFacilityLoadMeasures", facilityAttributes);
    clearVoltageLevels();
    if (facilityAttributes.VoltageLevels != null) {
        addVoltageLevels(facilityAttributes.VoltageLevels);
    }
    $("#divBCEGHierarchy").MapHierarchy("clear");
    if (facilityAttributes.BCEGLink != null) {
        $("#divBCEGHierarchy").MapHierarchy({
            "egId": facilityAttributes.BCEGLink.EGId, "mapId": facilityAttributes.BCEGLink.MapId
        });
    }   
}

function cancelSaveFacility() {
    displayAddFacilityMode();
    clearFacilityDialog();
}

function displayAddFacilityMode() {
    $("#divAddFacility").show();
    $("#divSaveButton").show();
    $("#divCreateFacility").hide();
    $("#txtBCEGSearch").hide();
    $("#divFacilitySearch").show();
    $("#divUpdateFacility").hide();
}

function displayUpdateFacilityMode() {
    $("#divAddFacility").hide();
    $("#divSaveButton").hide();
    $("#divCreateFacility").hide();
    $("#divFacilitySearch").hide();
    $("#divUpdateFacility").show();
}


function addFacilityToResourceList(facilityAttributes) {
    var type = $("#ddlFacilityType option:selected").text();
    var tla = $("input[id*='etbFacilityTla']").val();
    var name = $("input[id*='etbFacilityName']").val();
    var newRow = $("<li style='width:100%;display:block;float:left;height:auto;'><img onclick='javascript:toggleSubEntitList(this); return;' class='expandImage' src='/Images/minus.GIF' style='float:left;padding:5px 0px 0px 15px;'><div class='idColumn'></div><div style='text-align:left;width:450px;' class='resourceId varStateChanged' ondblclick='javascript:parentEntityDoubleClicked(this); return false;'></div><div style='text-align:center;width:200px;' class='resourceType'></div></li>");
    var childList = null;
    $(newRow).children(".idColumn").text($("#ipFacilityId").val());
    $(newRow).children(".resourceId").text(tla + " - " + name);
    $(newRow).children(".resourceType").text(type);
    $(newRow).contextMenu({ menu: 'ulFacilityMenu' }, resourceContextMenuHandler);
    if (facilityAttributes.ChildEntities != undefined && facilityAttributes.ChildEntities.length > 0) {
        childList = $("<ul class='innerResources' style='list-style: none outside none; margin-left: 30px;display:block;'></ul>");
        var child = null;
        for (var i = 0; i < facilityAttributes.ChildEntities.length; i++) {
            child = $("<li style='width:100%;display:block;float:left;height:auto;' ondblclick='javascript:childEntityDoubleClicked(this); return false;'><div class='idColumn'>1</div><div class='resourceId varStateChanged' style='text-align:left;width:400px;'>G1</div><div class='resourceType' style='text-align:center;width:200px;'>GeneratingUnit</div></li>");
            $(child).children(".idColumn").text(facilityAttributes.ChildEntities[i].Id);
            $(child).children(".resourceId").text(facilityAttributes.ChildEntities[i].Name);
            $(child).children(".resourceType").text(facilityAttributes.ChildEntities[i].Type);
            $(child).contextMenu({ menu: 'ulFacilityMenu' }, resourceContextMenuHandler);
            $(childList).append(child);
        }
    }
    else {
        $(newRow).children(".expandImage").css("visibility", "hidden");
    }
    if (childList != null) {
        $(newRow).append(childList);
    }
    $("#ulResourceList").append(newRow);
}

// used to validate new facilities & resource bundles
function validateNewResource(newId, type) {
    var resourceList = $("#ulResourceList").children("li");
    if (resourceList.length == 0) return true;
    for (var i = 0; i < resourceList.length; i++) {
        var id = $(resourceList[i]).children(".idColumn").text();
        var entityType = $(resourceList[i]).children(".resourceType").text();
        if (newId == id && entityType == type) {
            return false;
        }
    }
    return true;
}


/**********************************END Project Facility Selection & Maintenance modal*******************************/
/********************************** Project Gen Unit Selection & Maintenance modal*******************************/


function initGeneratingUnitSearch(serviceUrl) {
    $("#txtGenUnitSearch").focusin(function () {
        $("#txtGenUnitSearch").val("");
        if (generatingUnits == null) {
            serviceUrl = serviceUrl.substr(0, serviceUrl.indexOf('sId=') + 4) + $("#ipGenUnitParentId").val();
            $.get(serviceUrl, function (data) {
                if (data == null) return;
                generatingUnits = data;
                $("#txtGenUnitSearch").autocomplete({
                    minLength: 0,
                    source: generatingUnits,
                    select: function (event, ui) {
                        if (!validateNewChildEntity($("#ipGenUnitParentId").val(), ui.item.Id)) {
                            alert("The selected Generating Unit is already part of this phase.");
                            return;
                        }
                        generatingUnitSelectedCallback(ui.item.Id);
                        $("#divGenUnitSave").show();
                    }
                });
            }, "json");
        }
    });
}

function showGeneratingUnitFields() {
    $('#divGenUnitDetails').show();
}

function hideGeneratingUnitFields() {
    $('#divGenUnitDetails').hide();
}


function getGeneratorAttributes(serviceUrl) {
    $.get(serviceUrl, function (data) {
        loadGeneratingUnit(data);
    }, "json");
}

function loadGeneratingUnit(generatingUnitAttributes) {
    clearMeasuresGrid("divGenUnitMeasures");
    $("#ddlGenUnitType").attr("disabled", "disabled");
    $("#ddlGenUnitFuelType").attr("disabled", "disabled");
    $("#ddlGenUnitVoltageLevel").attr("disabled", "disabled");
    $("#ddlGenUnitPowerCall").attr("disabled", "disabled");   
    $("input[id*='etbGenUnitTLA']").attr("disabled", "disabled");
    $("input[id*='etbGenUnitName']").attr("disabled", "disabled");
    $("#ipGenUnitId").val(generatingUnitAttributes.Id);
    $("input[id*='etbGenUnitTLA']").val(generatingUnitAttributes.Identifier);
    $("input[id*='etbGenUnitName']").val(generatingUnitAttributes.Name);
    $("#ddlGenUnitFuelType").val(SetDropDown(generatingUnitAttributes.FuelTypeId));
    $("#ddlGenUnitType").val(SetDropDown(generatingUnitAttributes.TypeId));
    $("#ddlGenUnitVoltageLevel").val(generatingUnitAttributes.VoltageLevelId);
    $("#ddlGenUnitPowerCall").val(generatingUnitAttributes.PowerCallId);
    $("#ddlGenUnitStatus").val(SetDropDown(generatingUnitAttributes.StatusId));    
    $("input[id*='etbGenUnitResourcePlan']").val($("#ddlProjectResourcePlan option:selected").text());
    $("input[id*='etbGenUnitParentId']").val(generatingUnitAttributes.ParentTla);
    $("input[id*='etbGenUnitFacilityDistance']").val(generatingUnitAttributes.ParentDistance);    
    $("div[id*='emdGenUnit_divDateCreated']").text(generatingUnitAttributes.DateCreated);
    $("div[id*='emdGenUnit_divCreatedBy']").text(generatingUnitAttributes.UserCreated);
    $("div[id*='emdGenUnit_divDateModified']").text(generatingUnitAttributes.DateModified);
    $("div[id*='emdGenUnit_divModifiedBy']").text(generatingUnitAttributes.UserModified);
    setGeneratorMeasures("divGenUnitMeasures", generatingUnitAttributes);  
}

function SetDropDown(value) {
    return value != null ? value : -1;
}



function validateNewChildEntity(parentId, newId) {
    var li = $("div.idColumn:contains('" + parentId + "')").parent();
    var childList = $(li).children(".innerResources");
    if (childList.length == 0) return true;
    childList = childList.children("li");
    for (var i = 0; i < childList.length; i++) {
        var id = $(childList[i]).children(".idColumn").text();
        if (newId == id) {
            return false;
        }
    }
    return true;
}

function clearGeneratingUnitAttributesDialog() {
    $("#ipGenUnitId").val("");
    $("input[id*='etbGenUnitTLA']").val("");
    $("input[id*='etbGenUnitName']").val("");    
    $("#ddlGenUnitType").val("-1");      
    $("#ddlGenUnitFuelType").val("-1");
    $("#ddlGenUnitVoltageLevel").val("-1");
    $("#ddlGenUnitPowerCall").val("-1");
    $("div[id*='emdGenUnit_divDateCreated']").text("");
    $("div[id*='emdGenUnit_divCreatedBy']").text("");
    $("div[id*='emdGenUnit_divDateModified']").text("");
    $("div[id*='emdGenUnit_divModifiedBy']").text("");
    clearMeasuresGrid("divGenUnitMeasures");
}

function cancelSaveGeneratingUnitAttributes() {
    displayAddGeneratingUnitAttributesMode();
    clearGeneratingUnitAttributesDialog();
}

function displayAddGeneratingUnitAttributesMode() {
    $("#divGenUnitSearch").show();
    $("#divGenUnitAdd").show();
    $("#divGenUnitSave").hide();
    $("#divGenUnitUpdate").hide();
}

function displayUpdateGeneratingUnitAttributesMode() {
    $("#divGenUnitSearch").hide();
    $("#divGenUnitSave").hide();
    $("#divGenUnitUpdate").show();
}

function postGeneratingUnitAttributes(serviceUrl, projectId, genUnitId, name, typeId, statusTypeId, resourcePlanId, fuelTypeId, voltageLevelId, measures, phaseId, masterList, powerCallId) {
    if (genUnitId == "") {
        $.ajaxSetup({ async: false, timeout: 10000 });  // need to wait for response for genUnitId
    } else {
        $.ajaxSetup({ async: true, timeout: 10000 });
    }
    var facilityId = $("#ipGenUnitParentId").val();
    $.post(serviceUrl, { pjid: projectId, gid: genUnitId, name: name, typeId: typeId, statusTypeId: statusTypeId, resourcePlanId: resourcePlanId, fuelTypeId: fuelTypeId, Pdgc: measures.Pdgc, Pelcc: measures.Pelcc, Pptp: measures.Pptp, PepaMx: measures.PepaMx, Pndc: measures.Pndc, Pdmx0: measures.Pdmx0, RatedMVA: measures.RatedMVA, Qoe: measures.Qoe, Pmpo: measures.Pmpo, PSys: measures.PSys, Prmr: measures.Prmr, Pnp: measures.Pnp, Pdmx1: measures.Pdmx1, Pmnp: measures.Pmnp, RatedPF: measures.RatedPF, Que: measures.Que, Pmnr: measures.Pmnr, voltageLevelId: voltageLevelId, phaseId: phaseId, UseMasterPdgc: masterList.UseMasterPdgc, UseMasterPelcc: masterList.UseMasterPelcc, UseMasterPptp: masterList.UseMasterPptp, UseMasterPndc: masterList.UseMasterPndc, UseMasterPdmx0: masterList.UseMasterPdmx0, UseMasterPdmx1: masterList.UseMasterPdmx1, UseMasterPmpo: masterList.UseMasterPmpo, UseMasterPrmr: masterList.UseMasterPrmr, UseMasterPnp: masterList.UseMasterPnp, sid: facilityId, pcid: powerCallId },
                        function (data) {
                            $("#ipGenUnitId").val(data.Id);
                        }, "json"
    );
}

                    /**********************************END Project Gen Unit Selection & Maintenance modal*******************************/
                    /********************************** Project Load Selection & Maintenance modal*******************************/
function initLoadSearch(serviceUrl) {
    $("#txtLoadSelection").focusin(function () {
        $("#txtLoadSelection").val("");
        if (loads == null) {
            serviceUrl = serviceUrl.substr(0, serviceUrl.indexOf('sId=') + 4) + $("#ipLoadParentId").val();
            $.get(serviceUrl, function (data) {
                loads = data;
                $("#txtLoadSelection").autocomplete({
                    minLength: 0,
                    source: loads,
                    select: function (event, ui) {
                        if (!validateNewChildEntity($("#ipLoadParentId").val(), ui.item.Id)) {
                            alert("Selected Load is already part of the phase");
                            return;
                        }
                        loadSelectedCallback(ui.item.Id);
                        $("#divLoadSave").show();
                    }
                });
            }, "json");
        }
    });
}

function hideLoadFields() {
    $('#divLoadDetails').hide();
}

function showLoadFields() {
    $('#divLoadDetails').show();
}


function getLoadAttributes(serviceUrl) {
    $.get(serviceUrl, function (data) {
        loadLoadData(data);
    }, "json");
}

function loadLoadData(loadAttributes) {
    clearMeasuresGrid("divLoadMeasures");
    $("#ddlLoadVoltageLevel").attr("disabled", "disabled");    
    $("input[id*='etbLoadTLA']").attr("disabled", "disabled");
    $("input[id*='etbLoadName']").attr("disabled", "disabled");
    $("input[id*='etbLoadPowerFactor']").attr("disabled", "disabled");
    $("input[id*='etbLoadMW']").attr("disabled", "disabled");
    $("input[id*='etbLoadProbOfConnection']").attr("disabled", "disabled");
    $("input[id*='etbLoadAccountNumber']").attr("disabled", "disabled");
    $("#ipLoadId").val(loadAttributes.Id);
    $("input[id*='etbLoadTLA']").val(loadAttributes.Identifier);
    $("input[id*='etbLoadName']").val(loadAttributes.Name);
    $("input[id*='etbLoadParentTLA']").val(loadAttributes.ParentTla); 
    $("input[id*='etbLoadPowerFactor']").val(loadAttributes.PowerFactor);
    $("input[id*='etbLoadMW']").val(loadAttributes.LoadValue);
    $("input[id*='etbLoadAccountNumber']").val(loadAttributes.AccountNumber);
    $("input[id*='etbLoadProbOfConnection']").val(loadAttributes.Probability);
    $("#ddlLoadVoltageLevel").val(SetDropDown(loadAttributes.VoltageLevelId));
    $("#ddlLoadStatus").val(SetDropDown(loadAttributes.StatusId));
    $("input[id*='etbLoadResourcePlan']").val($("#ddlProjectResourcePlan option:selected").text());
    $("div[id*='emdLoad_divDateCreated']").text(loadAttributes.DateCreated);
    $("div[id*='emdLoad_divCreatedBy']").text(loadAttributes.Creator);
    $("div[id*='emdLoad_divDateModified']").text(loadAttributes.DateModified);
    $("div[id*='emdLoad_divModifiedBy']").text(loadAttributes.UserModified);

    setLoadMeasures("divLoadMeasures", loadAttributes);
}

function clearLoadData() {
    $("#ipLoadId").val("");
    $("input[id*='etbLoadTLA']").val("");
    $("input[id*='etbLoadName']").val("");
    $("input[id*='etbLoadPowerFactor']").val("");
    $("input[id*='etbLoadMW']").val("");
    $("input[id*='etbLoadAccountNumber']").val("");
    $("input[id*='etbLoadProbOfConnection']").val("");
    $("#ddlLoadVoltageLevel").val("-1");
    $("div[id*='emdLoad_divDateCreated']").text("");
    $("div[id*='emdLoad_divCreatedBy']").text("");
    $("div[id*='emdLoad_divDateModified']").text("");
    $("div[id*='emdLoad_divModifiedBy']").text("");
    clearMeasuresGrid("divLoadMeasures");
}

function cancelSaveLoadAttributes() {
    displayAddLoadAttributesMode();
    clearLoadData();
}

function displayAddLoadAttributesMode() {
    $("#divLoadSearch").show();
    $("#divLoadAdd").show();
    $("#divLoadSave").hide();
    $("#divLoadUpdate").hide();
}

function displayUpdateLoadAttributesMode() {
    $("#divLoadSearch").hide();
    $("#divLoadAdd").hide();
    $("#divLoadSave").hide();
    $("#divLoadUpdate").show();
}

function postLoadAttributes(serviceUrl, projectId, loadId, name, powerFactor, loadValue, statusId, accountNumber, probability, resourcePlanId, voltageLevelId, measures, phaseId) {
    if (loadId == "") {
        $.ajaxSetup({ async: false, timeout: 10000 });  // need to wait for response for LoadId
    } else {
        $.ajaxSetup({ async: true, timeout: 10000 });
    }        
    $.post(serviceUrl, {pjid: projectId, loadid: loadId, name: name, powerFactor: powerFactor, loadValue: loadValue, statusId: statusId, accountNumber: accountNumber, probability: probability, resourcePlanId: resourcePlanId, voltageLevelId: voltageLevelId, LdGrSpk: measures.LdGrSpk, LdGrSmn: measures.LdGrSmn, LdGrWpk: measures.LdGrWpk, LdGrWmn: measures.LdGrWmn, phaseId: phaseId },
                        function (data) {
                            $("#ipLoadId").val(data.Id);
                        }, "json"
    );
}



/**********************************END Project Load Selection & Maintenance modal*******************************/

/**********************************Phases*******************************/

function postPhaseDetails(serviceUrl, projectId, revisionId, planId, cloneId) {
    var phaseId = $(".ui-tabs-selected", $('#divProjectPhases')).attr("key");
    if (phaseId == undefined) { // new phase
        phaseId == null;
        $.ajaxSetup({ async: false, timeout: 10000 });
    } else {
        $.ajaxSetup({ async: true, timeout: 10000 });
    }
    $.post(serviceUrl, {pjid: projectId, estimatedStartDate: $("input[id*='edPhaseCODStartDate']").val(), actualStartDate: $("input[id*='edPhaseActualStartDate']").val(), phaseDesc: $("input[id*='etbPhaseDesc']").val(), isFinalPhase: $('#cbProjectFinalPhase').is(':checked'), rvId: revisionId, phaseId: phaseId, planid: planId, phaseCloneId: cloneId },
                        function (data) {
                            if (phaseId != null) return; // no need to load... current phase
                            $(".ui-tabs-selected", $('#divProjectPhases')).attr("key", data.Id);
                            $(".ui-tabs-selected", $('#divProjectPhases')).attr("startdate", data.CODDate);
                            loadPhase(data);
                        }, "json"
    );
}

function getPhase(serviceUrl) {
    $.get(serviceUrl, function (data) {
        loadPhase(data);        
    }, "json");
}

function loadPhase(phase) {
    var selectedRevisionId = $("#ddlProjectRevisions option:selected").val();
    var editableRevisionId = $("#ddlProjectRevisions option:last").val();   
    if (phase.IsFinalPhase || (selectedRevisionId != editableRevisionId)) {// this condition needs to change
        disablePhase(phase.IsFinalPhase);
        $("input[id*='edPhaseCODStartDate']").dateEntry("change", "minDate", null);
        $("input[id*='edPhaseCODStartDate']").dateEntry("change", "maxDate", null);
    }
    else {        
        $("#cbProjectFinalPhase").prop("checked", false);
        //$("#cbProjectFinalPhase").removeAttr("disabled"); 
        $("#cbProjectFinalPhase").attr("onclick", "javascript:finalPhaseChecked();");       
        $("div[id*='edPhaseCODStartDate'] > div").text("COD/Phase Start Date:");
        $("input[id*='edPhaseCODStartDate']").dateEntry('enable');
        $("input[id*='etbPhaseDesc']").prop("disabled", false);
        if(phase.estimatedMinDate != null) {            
            $("input[id*='edPhaseCODStartDate']").dateEntry("change", "minDate", phase.estimatedMinDate);
        }else{            
            $("input[id*='edPhaseCODStartDate']").dateEntry("change", "minDate", null);
        }
        if (phase.estimatedMaxDate != null) {            
            $("input[id*='edPhaseCODStartDate']").dateEntry("change", "maxDate", phase.estimatedMaxDate);
        }else{            
            $("input[id*='edPhaseCODStartDate']").dateEntry("change", "maxDate", null);
        }
        $("div[id*='edPhaseActualStartDate'] > div").text("Actual In-Service Date:");
        $("input[id*='edPhaseActualStartDate']").dateEntry('enable');
        $("input[id*='btnSavePhase']").removeAttr("disabled");
        $("input[id*='btnAddFacility']").removeAttr("disabled");
        $("input[id*='btnAddResourceBundle']").removeAttr("disabled");
        $("#ulResourceList").enableContextMenu();        
    }
    //if (phase.actualMinDate != null) { // actaul date always editable        
    //    $("input[id*='edPhaseActualStartDate']").dateEntry("change", "minDate", phase.actualMinDate);
    //} else {        
    //    $("input[id*='edPhaseActualStartDate']").dateEntry("change", "minDate", null);
    //}
    //if (phase.actualMaxDate != null) {        
    //    $("input[id*='edPhaseActualStartDate']").dateEntry("change", "maxDate", phase.actualMaxDate);
    //} else {       
    //    $("input[id*='edPhaseActualStartDate']").dateEntry("change", "maxDate", null);
    //}       
    $("input[id*='edPhaseCODStartDate']").dateEntry('setDate', phase.CODDate);
    // state check
    if (phase.CODDateState == true) {
        $("input[id*='edPhaseCODStartDate']").addClass("varStateChanged");
    } else {
        $("input[id*='edPhaseCODStartDate']").removeClass("varStateChanged");
    }
    $("input[id*='edPhaseActualStartDate']").dateEntry('setDate', phase.AISDate);
    $("input[id*='etbPhaseDesc']").val(phase.Desc);
    $("#ulResourceList").empty();
    if (phase.Resources == null || phase.Resources.length == 0 || phase.IsFinalPhase) return; // *** hide expand img
    for (var i = 0; i < phase.Resources.length; i++) {
        addResource(phase.Resources[i].Id, phase.Resources[i].TLA, phase.Resources[i].Name, phase.Resources[i].Type, phase.Resources[i].IsNew, phase.Resources[i].ChildEntities);
    }
}

function getDate(dateString) {
    var dateParts = dateString.split("-");
    return new Date(dateParts[0], Math.round(dateParts[1]) -1, dateParts[2]);   
}

function disablePhase(isFinal) {
    if (isFinal) {
        $("#cbProjectFinalPhase").prop("checked", true); //*
        $("div[id*='edPhaseCODStartDate']").children("div").text("COD/Phase End Date:");        
        $("div[id*='edPhaseActualStartDate']").children("div").text("Actual End Date:");                
    } else {
        $("div[id*='edPhaseCODStartDate']").children("div").text("COD/Phase Start Date:");
        $("div[id*='edPhaseActualStartDate']").children("div").text("Actual In-Service Date:");
        $("input[id*='edPhaseActualStartDate']").dateEntry('disable');
        disbaleMeasuresGrid('divFacilityGenMeasures');
        disbaleMeasuresGrid('divGenUnitMeasures');
        disbaleMeasuresGrid('divLoadMeasures');
        disbaleMeasuresGrid('divResBundleMeasures');         
    }
    //$("#cbProjectFinalPhase").attr("disabled", "disabled"); //*
    disableCheckbox("cbProjectFinalPhase");
    $("#divNewPhase").hide();
    $("input[id*='edPhaseCODStartDate']").dateEntry('disable');
    $("input[id*='etbPhaseDesc']").prop("disabled", true);
    $("input[id*='btnSavePhase']").attr("disabled", "disabled");
    $("input[id*='btnAddFacility']").attr("disabled", "disabled");
    $("input[id*='btnAddResourceBundle']").attr("disabled", "disabled");
    $("#ulResourceList").disableContextMenu();

}

function addResource(id, tla, name, type, isNew, childEntities) {
    var selectedRevisionId = $("#ddlProjectRevisions option:selected").val();
    var editableRevisionId = $("#ddlProjectRevisions option:last").val();
    var newRow = $("<li style='width:100%;display:block;float:left;height:auto;'><img onclick='javascript:toggleSubEntitList(this); return;' class='expandImage' src='/Images/minus.GIF' style='float:left;padding:5px 0px 0px 15px;'><div class='idColumn'></div><div style='text-align:left;width:450px;' class='resourceId' ondblclick='javascript:parentEntityDoubleClicked(this); return false;'></div><div style='text-align:center;width:200px;' class='resourceType'></div></li>");
    var childList = null;
    $(newRow).children(".idColumn").text(id);
    $(newRow).children(".resourceId").text(tla + " - " + name);
    // new to project
    if (isNew == true) {
        $(newRow).children(".resourceId").addClass("varStateChanged");
    } else {
        $(newRow).children(".resourceId").removeClass("varStateChanged");
    }
    $(newRow).children(".resourceType").text(type);
    if (selectedRevisionId == editableRevisionId) { // last revision only
        if (type == 'Resource Bundle') {
            $(newRow).contextMenu({ menu: 'ulResourceDeleteMenu' }, resourceContextMenuHandler);
        } else {
            $(newRow).contextMenu({ menu: 'ulFacilityMenu' }, resourceContextMenuHandler);
        }
    }
    if (childEntities != undefined && childEntities != null && childEntities.length > 0) {
        childList = $("<ul class='innerResources' style='list-style: none outside none; margin-left: 30px;display:block;'></ul>");
        var child = null;
        for (var i = 0; i < childEntities.length; i++) {
            child = $("<li style='width:100%;display:block;float:left;height:auto;' ondblclick='javascript:childEntityDoubleClicked(this); return false;'><div class='idColumn'>1</div><div class='resourceId' style='text-align:left;width:400px;'>G1</div><div class='resourceType' style='text-align:center;width:200px;'>GeneratingUnit</div></li>");
            $(child).children(".idColumn").text(childEntities[i].Id);
            $(child).children(".resourceId").text(childEntities[i].Name);
            $(child).children(".resourceType").text(childEntities[i].Type);
            // new to entity
            if (childEntities[i].IsNew == true) {
                $(child).children(".resourceId").addClass("varStateChanged");
            } else {
                $(child).children(".resourceId").removeClass("varStateChanged");
            }
            if (selectedRevisionId == editableRevisionId) { // last revision only
                $(child).contextMenu({ menu: 'ulFacilityMenu' }, resourceContextMenuHandler);
            }
            $(childList).append(child);
        }
    }
    else {
        $(newRow).children(".expandImage").css("visibility", "hidden");        
    }
    if (childList != null) {
        $(newRow).append(childList);
    }
    $("#ulResourceList").append(newRow);
}

/**********************************END Phases*******************************/

/**********************************Resource Bundles*******************************/

function initResourceBundleSearch(serviceUrl) {   
    $("#txtResBundleSearch").focusin(function () {
        $("#txtResBundleSearch").val("");
        if (resourceBundles == null) {
            $.get(serviceUrl, function (data) {
                if (data == null) return;
                resourceBundles = data;
                $("#txtResBundleSearch").autocomplete({
                    minLength: 0,
                    source: resourceBundles,
                    select: function (event, ui) {
                        if (!validateNewResource(ui.item.Id, ui.item.Type)) {
                            alert("The selected resource bundle is already part of this phase.");
                            return;
                        }
                        resourceBundleSelected(ui.item.Id);
                        $("#divResBundleSave").show();
                    }
                });
            }, "json");
        }
    });
}

function showResourceBundleFields() {
    $('#divResBundleDetails').show();
}

function hideResourceBundleFields() {
    $('#divResBundleDetails').hide();
}



function loadResourceBundleData(resourceBundleAttributes) {
    $("input[id*='etbResBundleTLA']").attr("disabled", "disabled");
    $("input[id*='etbResBundleName']").attr("disabled", "disabled");
    $("input[id*='etbResBundleLatitude']").attr("disabled", "disabled");
    $("input[id*='etbResBundleLongitude']").attr("disabled", "disabled");
    $("input[id*='etbResBundlePoiCctDesig']").attr("disabled", "disabled");
    $("input[id*='etbResBundlePoi1']").attr("disabled", "disabled");
    $("input[id*='etbResBundlePoi2']").attr("disabled", "disabled");
    $("input[id*='etbResBundlePoiVoltage']").attr("disabled", "disabled");
    $("#ddlResBundleFuelType").attr("disabled", "disabled");
    $("#ddlResBundleStatus").attr("disabled", "disabled");
    $("#txtBCEGSearch").hide();
    $("#ipResBundleId").val(resourceBundleAttributes.Id);
    $("input[id*='etbResBundleTLA']").val(resourceBundleAttributes.Identifier);
    $("input[id*='etbResBundleName']").val(resourceBundleAttributes.Name);
    $("input[id*='etbResBundleLatitude']").val(resourceBundleAttributes.Latitude);
    $("input[id*='etbResBundleLongitude']").val(resourceBundleAttributes.Longitude);
    $("input[id*='etbResBundlePoiCctDesig']").val(resourceBundleAttributes.PoiCircDesig);
    $("input[id*='etbResBundlePoi1']").val(resourceBundleAttributes.Poi1);
    $("input[id*='etbResBundlePoi2']").val(resourceBundleAttributes.Poi2);
    $("input[id*='etbResBundlePoiVoltage']").val(resourceBundleAttributes.PoiVoltage);
    $("div[id*='emdResBundle_divDateCreated']").text(resourceBundleAttributes.DateCreated);
    $("div[id*='emdResBundle_divCreatedBy']").text(resourceBundleAttributes.UserCreated);
    $("div[id*='emdResBundle_divDateModified']").text(resourceBundleAttributes.DateModified);
    $("div[id*='emdResBundle_divModifiedBy']").text(resourceBundleAttributes.UserModified);
    $("input[id*='etbResBundleResourcePlan']").val($("#ddlProjectResourcePlan option:selected").text());
    $("#ddlResBundleFuelType").val(SetDropDown(resourceBundleAttributes.FuelTypeId));
    $("#ddlResBundleStatus").val(SetDropDown(resourceBundleAttributes.StatusId));
    setResourceBundleMeasures("divResBundleMeasures", resourceBundleAttributes);
    $("#divBCEGHierarchy").MapHierarchy("clear");
    if (resourceBundleAttributes.BCEGLink != null) {
        $("#divBCEGHierarchy").MapHierarchy({
            "egId": resourceBundleAttributes.BCEGLink.EGId, "mapId": resourceBundleAttributes.BCEGLink.MapId
        });
    }
    
}

function clearResourceBundleData() {
    $("#ipResBundleId").val("");
    $("input[id*='etbResBundleTLA']").val("");
    $("input[id*='etbResBundleName']").val("");
    $("input[id*='etbResBundleLatitude']").val("");
    $("input[id*='etbResBundleLongitude']").val("");
    $("input[id*='etbResBundlePoiCctDesig']").val("");
    $("input[id*='etbResBundlePoi1']").val("");
    $("input[id*='etbResBundlePoi2']").val("");
    $("input[id*='etbResBundlePoiVoltage']").val("");
    $("#ddlResBundleFuelType").val("-1");
    $("#ddlResBundleStatus").val("-1");
    clearMeasuresGrid("divResBundleMeasures");
    //clearElectricalGroupHierarchy();
    $("#divBCEGHierarchy").MapHierarchy("clear");
}

function cancelSaveResourceBundleAttributes() {
    displayAddResourceBundleAttributesMode();
    clearResourceBundleData();
}

function displayAddResourceBundleAttributesMode() {
    $("#divResBundleUpdate").hide();
    $("#divResBundleSave").hide();
    $("#divResBundleAdd").show();
    $("#divResBundleSearch").show();
    $("#txtBCEGSearch").hide();
}

function displayUpdateResourceBundleAttributesMode() {
    $("#divResBundleUpdate").show();
    $("#divResBundleSave").hide()
    $("#divResBundleAdd").hide();
    $("#divResBundleSearch").hide();
}

function postResourceBundleAttributes(serviceUrl, pId, rbId, tla, name, lat, lon, poiCircDesig, poi1, poi2, poiVoltage, statusId, resourcePlanId, fuelTypeId, measures, phaseId) {
    if (rbId == "") {
        $.ajaxSetup({ async: false, timeout: 10000 });   // need to wait for the Id to be returned
    } else {
        $.ajaxSetup({ async: true, timeout: 10000 });
    }
    $.post(serviceUrl, { pjid: pId, rbId: rbId, tla: tla, name: name, lat: lat, lon: lon, poiCrctDesig: poiCircDesig, poi1: poi1, poi2: poi2, poiVoltage: poiVoltage, statusId: statusId, resourcePlanId: resourcePlanId, fuelTypeId: fuelTypeId, Pdgc: measures.Pdgc, Pelcc: measures.Pelcc, Pmpo: measures.Pmpo, Pndc: measures.Pndc, Pnp: measures.Pnp, phaseId: phaseId },  
                        function (data) {
                            $("#ipResBundleId").val(data.Id);
                        }, "json"
    );
}



/**********************************END Resource Bundles*******************************/

function disableCheckbox(selector) {
    $("#" + selector).attr("onclick", "return false");
    $("#" + selector).attr("onkeydown", "return false");
}