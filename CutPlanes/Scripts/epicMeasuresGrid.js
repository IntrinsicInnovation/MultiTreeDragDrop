// Measures DTO class... contains Generating Units and Load attributes / properties
function Measures() {
    // gen
    this.Pdgc = null;
    this.Pelcc = null;
    this.Pptp = null;
    //this.PepaMx = null;
    this.Pndc = null;
    this.Pdmx0 = null;
    //this.Pavg = null;
    this.RatedMVA = null;
    this.Qoe = null;
    this.Pmpo = null;
    this.Psys = null;
    this.Prmr = null;
    //this.Pplant = null;
    this.Pnp = null;
    this.Pdmx1 = null;
    this.Pmnp = null;
    this.RatedPF = null;
    this.Que = null;
    this.Pmnr = null;
    this.Pir = null;
    // load
    this.LdGrSpk = null;
    this.LdGrSmn = null;
    this.LdGrWpk = null;
    this.LdGrWmn = null;
}

function MasterList() {
    this.UseMasterPdgc = null;
    this.UseMasterPelcc = null;
    this.UseMasterPptp = null;
    this.UseMasterPndc = null;
    this.UseMasterPdmx0 = null;
    this.UseMasterPdmx1 = null;
    this.UseMasterPmpo = null;
    this.UseMasterPrmr = null;
    this.UseMasterPnp = null;
}

function GridItem(name, description, hasMasterOption, isReadOnly) {
    this.Name = name;
    this.Description = description;
    this.HasMasterOption = hasMasterOption;
    this.IsReadOnly = isReadOnly;
}

function initNumericValidation(parentId) {
    $("input[id*='" + parentId + "'][type='text']").each(function (index) {
        try {
            $.numericInputValidation(this);
        }
        catch (exception) {
            alert('jquery-add-on.js can not be been found.  This file is required by initNumericValidation within epicMeasuresGrid.js');
            return false;
        }
    });
}

function renderMeasuresGrid(parentId, mode) {
    //gridItems used for grids labels and input Ids
    var gridItems = null;
    var width = null;
    var valueToolTip = null;
    var overrideToolTip = null;
    radioANs = 'yes';
    radioBNs = 'no';
    switch (mode) {
        case "Summary_Generator":
            gridItems = new Array(new GridItem("Pndc", "Nits Designated Capacity", true, false), new GridItem("Prmr", "Reliability Must Run Capacity", true, false), new GridItem("Pdgc", "Dependable Generating Capacity", true, false), new GridItem("Pdmx0", "Maximum Output For Non-Firm Transmission", true, false), new GridItem("Pelcc", "Effective Load Carrying Capacity", true, false), new GridItem("Pdmx1", "Maximum Output For Firm Transmission", true, false), new GridItem("Pmpo", "Maximum Power Output", true, false), new GridItem("Pnp", "Name Plate Capacity In MW", true, false), new GridItem("Pptp", "Point To Point Capacity", true, false), new GridItem("Psys", "System Capacity", false, true), new GridItem("Pir", "Interconnections Request Capacity", false, false));
            overrideToolTip = "Aggregated value";
            width = "654px";
            radioANs = 'yes';
            radioBNs = 'no';
            break;
        case "Generator":
            gridItems = new Array(new GridItem("Pdgc", "Dependable Generating Capacity", true, false), new GridItem("Pelcc", "Effective Load Carrying Capacity", true, false), new GridItem("Pptp", "Point To Point Capacity", true, false), new GridItem("Pndc", "Nits Designated Capacity", true, false), new GridItem("Pdmx0", "Maximum Output For Non-Firm Transmission", true, false), new GridItem("RatedMVA", "Rated MVA", false, false), new GridItem("Qoe", "Rated Over Excited Power Factor", false, true), new GridItem("Pmpo", "Maximum Power Output", true, false), new GridItem("Psys", "System Capacity", false, true), new GridItem("Prmr", "Regulatory Minimum Output", true, false), new GridItem("Pnp", "Name Plate Capacity In MW", true, false), new GridItem("Pdmx1", "Maximum Output For Firm Transmission", true, false), new GridItem("Pmnp", "Physical Minimum Output", false, false), new GridItem("RatedPF", "Rated Power Factor", false, false), new GridItem("Que", "Rated Under Excited Power Factor", false, true), new GridItem("Pmnr", "Regulatory Minimum Output", false, false));
            overrideToolTip = "Roll down value";
            width = "654px";
            radioANs = 'no';
            radioBNs = 'yes';
            break;
        case "ResourceBundle":
            gridItems = new Array(new GridItem("Pdgc", "Dependable Generating Capacity", false, false), new GridItem("Pelcc", "Effective Load Carrying Capacity", false, false), new GridItem("Psys", "System Capacity", false, false), new GridItem("Pmpo", "Maximum Power Output", false, false), new GridItem("Pndc", "Nits Designated Capacity", false, false), new GridItem("Pnp", "Name Plate Capacity In MW", false, false));
            overrideToolTip = "";
            width = "326px";
            break;
        case "Load":
            gridItems = new Array(new GridItem("LdGrSpk", "Load Gross Summer Peak", false, false), new GridItem("LdGrSmn", "Load Gross Summer Minimum", false, false), new GridItem("LdGrWpk", "Load Gross Winter Peak", false, false), new GridItem("LdGrWmn",  "Load Gross Winter Minimum", false, false));
            width = "654px";
            overrideToolTip = "Aggregated value";
            break;
    }
    var divParent = $("<div style='width:" + width + ";float:left;'></div>");
    var itemTemplate = "<div style='margin-bottom:3px;height:24px;width:326;float:left;'></div>";
    var labelTemplate = "<div class='formLabels' style='width:60px;padding:2px 5px 0px 5px;float:left; text-align:right; font-weight: bold; color: #000000'></div>";
    var valueTemplate = "<input class='formElements measureValue' type='text' style='width:100px;float:left;'>";    
    var masterRadioTemplate = "<input type='radio' title='Master value' style='float:left;' class='radio'/>";
    var overrideTemplate = "<div title='" + overrideToolTip + "' style='padding-left:5px;width:100px;float:left;' class='measureOverriden'></div>";
    var radioSpan = "<span class='radioButton'></span>";
    var span = null;
    for (var i = 0; i < gridItems.length; i++) {
        var divItem = $(itemTemplate);
        var divLabel = $(labelTemplate);
        $(divLabel).text(gridItems[i].Name + ":");
        $(divLabel).attr("title", gridItems[i].Description);
        $(divItem).append(divLabel);
        var radioA = $(masterRadioTemplate);
        var ns = gridItems[i].Name == "Pnp" ? radioBNs : radioANs;
        $(radioA).attr("id", "rb_" + ns + "_" + parentId + "_" + gridItems[i].Name);
        $(radioA).attr("name", parentId + "_" + gridItems[i].Name);
        if (!gridItems[i].HasMasterOption) {
            $(radioA).css("visibility", "hidden");
        }
        $(divItem).append(radioA);
        var inputValue = $(valueTemplate);
        $(inputValue).attr("id", "ip_" + parentId + "_" + gridItems[i].Name);
        if (gridItems[i].IsReadOnly) {
            $(inputValue).attr("readonly", "readonly");
        }
        $(divItem).append(inputValue);
        var radioB = $(masterRadioTemplate);
        ns = gridItems[i].Name == "Pnp" ? radioANs : radioBNs;
        $(radioB).attr("id", "rb_" + ns + "_" + parentId + "_" + gridItems[i].Name);
        $(radioB).attr("name", parentId + "_" + gridItems[i].Name);
        if (!gridItems[i].HasMasterOption) {
            $(radioB).css("visibility", "hidden");
        }
        $(divItem).append(radioB);        
        var override = $(overrideTemplate);
        $(override).attr("id", "div_" + parentId + "_" + gridItems[i].Name);
        $(divItem).append(override);
        $(divParent).append(divItem);
    }
    $("#" + parentId).append(divParent);
    initNumericValidation(parentId);
}


function clearMeasuresGrid(parentId) {
    $("input[id*='" + parentId + "'][type!='radio']").each(function (index) {        
        $(this).val("");
    });    
    $("input[id*='rb_yes_" + parentId + "']").each(function (index) {        
        $(this).attr('checked', true);        
    });    
    $(".measureOverriden").each(function (index) {
        $(this).text("");
    });
}

function disbaleMeasuresGrid(parentId) {
    $("input[id*='" + parentId + "']").each(function (index) {
        $(this).attr("disabled", "disabled");
    });
}

function enableMeasuresGrid(parentId) {
    $("input[id*='" + parentId + "']").each(function (index) {
        $(this).removeAttr("disabled");
    });
}

function setSummaryGeneratorMeasures(parentId, attributes) {    
    setSummaryGridValues(parentId, "Pdgc", attributes.Pdgc);
    setSummaryGridValues(parentId, "Pelcc", attributes.Pelcc);
    setSummaryGridValues(parentId, "Pptp", attributes.Pptp);
    //setSummaryGridValues(parentId, "PepaMx", attributes.PepaMx);
    setSummaryGridValues(parentId, "Pndc", attributes.Pndc);
    setSummaryGridValues(parentId, "Pmpo", attributes.Pmpo);
    setSummaryGridValues(parentId, "Psys", attributes.Psys);
    setSummaryGridValues(parentId, "Prmr", attributes.Prmr);
    //setSummaryGridValues(parentId, "Pplant", attributes.Pplant);
    setSummaryGridValues(parentId, "Pdmx0", attributes.Pdmx0);
    setSummaryGridValues(parentId, "Pdmx1", attributes.Pdmx1);
    setSummaryGridValues(parentId, "Pnp", attributes.Pnp);
    setSummaryGridValues(parentId, "Pir", attributes.Pir);
}

function setLoadMeasures(parentId, attributes) {    
    setGridValues(parentId, "LdGrSpk", attributes.LdGrSpk);
    setGridValues(parentId, "LdGrWpk", attributes.LdGrWpk);
    setGridValues(parentId, "LdGrSmn", attributes.LdGrSmn);
    setGridValues(parentId, "LdGrWmn", attributes.LdGrWmn);
}

function setSummaryLoadMeasures(parentId, attributes) {    
    setSummaryGridValues(parentId, "LdGrSpk", attributes.LdGrSpk);
    setSummaryGridValues(parentId, "LdGrWpk", attributes.LdGrWpk);
    setSummaryGridValues(parentId, "LdGrSmn", attributes.LdGrSmn);
    setSummaryGridValues(parentId, "LdGrWmn", attributes.LdGrWmn);
}

function setGridValues(parentId, gridId, measure) {
    if (measure == undefined) return;
    if(measure.Value != null) {
        $("#ip_" + parentId + "_" + gridId).val(measure.Value);
        $("#ip_" + parentId + "_" + gridId).attr("title", "Entity Value");
    }
    if (measure.Override != null) {
        $("#div_" + parentId + "_" + gridId).text(measure.Override);
        $("#div_" + parentId + "_" + gridId).attr("title", "Roll Down Value");
    }

    if (measure.UseMaster != null) {
        if (measure.UseMaster == true) { // 0 = child...  gen unit / load attribute level
            $("#rb_yes_" + parentId + "_" + gridId).prop("checked", true);
        } else {
            $("#rb_no_" + parentId + "_" + gridId).prop("checked", true);
        }
    }
    // state change formatting
    if (measure.ValueState == true) {
        $("#ip_" + parentId + "_" + gridId).addClass("varStateChanged");
    } else {
        $("#ip_" + parentId + "_" + gridId).removeClass("varStateChanged");
    }    
}

function setSummaryGridValues(parentId, gridId, measure) {
    if (measure == undefined) return;
    if (measure.Value != null) {
        $("#ip_" + parentId + "_" + gridId).val(measure.Value);
        $("#ip_" + parentId + "_" + gridId).attr("title", "Entity Value");
    }
    if (measure.Override != null) {
        $("#div_" + parentId + "_" + gridId).text(measure.Override);
        $("#div_" + parentId + "_" + gridId).attr("title", "Aggregated Value");
    }
    if (measure.UseMaster != null) {
        if (measure.UseMaster == true) { // 1 = parent /  Substation attribute level
            $("#rb_yes_" + parentId + "_" + gridId).prop("checked", true);
        } else {
            $("#rb_no_" + parentId + "_" + gridId).prop("checked", true);
        }
    }
    // state change formatting
    if (measure.ValueState == true) {
        $("#ip_" + parentId + "_" + gridId).addClass("varStateChanged");
    } else {
        $("#ip_" + parentId + "_" + gridId).removeClass("varStateChanged");
    }
}

function setGeneratorMeasures(parentId, attributes) {    
    setGridValues(parentId, "Pdgc", attributes.Pdgc);   
    setGridValues(parentId, "Pelcc", attributes.Pelcc);    
    setGridValues(parentId, "Pptp", attributes.Pptp);   
    //setGridValues(parentId, "PepaMx", attributes.PepaMx);    
    setGridValues(parentId, "Pndc", attributes.Pndc);   
    setGridValues(parentId, "Pdmx0", attributes.Pdmx0);   
    //setGridValues(parentId, "Pavg", attributes.Pavg);    
    setGridValues(parentId, "RatedMVA", attributes.RatedMVA);    
    setGridValues(parentId, "Qoe", attributes.Qoe);   
    setGridValues(parentId, "Pmpo", attributes.Pmpo);   
    setGridValues(parentId, "Psys", attributes.Psys);    
    setGridValues(parentId, "Prmr", attributes.Prmr);    
    //setGridValues(parentId, "Pplant", attributes.Pplant);    
    setGridValues(parentId, "Pnp", attributes.Pnp);    
    setGridValues(parentId, "Pdmx1", attributes.Pdmx1);    
    setGridValues(parentId, "Pmnp", attributes.Pmnp);    
    setGridValues(parentId, "RatedPF", attributes.RatedPF);   
    setGridValues(parentId, "Que", attributes.Que);    
    setGridValues(parentId, "Pmnr", attributes.Pmnr);
}

function setResourceBundleMeasures(parentId, attributes) {
    $("#ip_" + parentId + "_Pdgc").val(attributes.Pdgc);
    if (attributes.PdgcState == true) { // state change
        $("#ip_" + parentId + "_Pdgc").addClass("varStateChanged");
    } else {
        $("#ip_" + parentId + "_Pdgc").removeClass("varStateChanged");
    }
    $("#ip_" + parentId + "_Pelcc").val(attributes.Pelcc);
    if (attributes.PelccState == true) { // state change
        $("#ip_" + parentId + "_Pelcc").addClass("varStateChanged");
    } else {
        $("#ip_" + parentId + "_Pelcc").removeClass("varStateChanged");
    }
    $("#ip_" + parentId + "_Pmpo").val(attributes.Pmpo);
    if (attributes.PmpoState == true) { // state change
        $("#ip_" + parentId + "_Pmpo").addClass("varStateChanged");
    } else {
        $("#ip_" + parentId + "_Pmpo").removeClass("varStateChanged");
    }
    $("#ip_" + parentId + "_Psys").val(attributes.Psys);
    if (attributes.PsysState == true) { // state change
        $("#ip_" + parentId + "_Psys").addClass("varStateChanged");
    } else {
        $("#ip_" + parentId + "_Psys").removeClass("varStateChanged");
    }
    $("#ip_" + parentId + "_Pndc").val(attributes.Pndc);
    if (attributes.PndcState == true) { // state change
        $("#ip_" + parentId + "_Pndc").addClass("varStateChanged");
    } else {
        $("#ip_" + parentId + "_Pndc").removeClass("varStateChanged");
    }
    $("#ip_" + parentId + "_Pnp").val(attributes.Pnp);
    if (attributes.PnpState == true) { // state change
        $("#ip_" + parentId + "_Pnp").addClass("varStateChanged");
    } else {
        $("#ip_" + parentId + "_Pnp").removeClass("varStateChanged");
    }
    $("#rb_val_" + parentId + "_Pdgc").css("visibility", "hidden");
    $("#rb_or_" + parentId + "_Pelcc").css("visibility", "hidden");
    $("#rb_val_" + parentId + "_Pelcc").css("visibility", "hidden");
    $("#rb_or_" + parentId + "_Pdgc").css("visibility", "hidden"); 
}

function getFacilityMeasures(generatorParentId, loadParentId) {
    var measures = new Measures();
    measures.Pdgc = $("#ip_" + generatorParentId + "_Pdgc").val();
    measures.Pmpo = $("#ip_" + generatorParentId + "_Pmpo").val();
    measures.Pnp = $("#ip_" + generatorParentId + "_Pnp").val();
    measures.Pelcc = $("#ip_" + generatorParentId + "_Pelcc").val();
    measures.Psys = $("#ip_" + generatorParentId + "_Psys").val();
    measures.Pptp = $("#ip_" + generatorParentId + "_Pptp").val();
    measures.Prmr = $("#ip_" + generatorParentId + "_Prmr").val();
    measures.Pir = $("#ip_" + generatorParentId + "_Pir").val();
    //measures.Pplant = $("#ip_" + generatorParentId + "_Pplant").val();
    measures.Pndc = $("#ip_" + generatorParentId + "_Pndc").val();
    measures.Pdmx0 = $("#ip_" + generatorParentId + "_Pdmx0").val();
    measures.Pdmx1 = $("#ip_" + generatorParentId + "_Pdmx1").val();
    measures.LdGrSpk = $("#ip_" + loadParentId + "_LdGrSpk").val();
    measures.LdGrSmn = $("#ip_" + loadParentId + "_LdGrSpk").val();
    measures.LdGrWpk = $("#ip_" + loadParentId + "_LdGrWpk").val();
    measures.LdGrWmn = $("#ip_" + loadParentId + "_LdGrWmn").val();
    return measures;
}

function getGeneratorMeasures(parentId) {
    var measures = new Measures();
    measures.Pdgc = $("#ip_" + parentId + "_Pdgc").val();
    measures.Pelcc = $("#ip_" + parentId + "_Pelcc").val();
    measures.Pptp = $("#ip_" + parentId + "_Pptp").val();
    //measures.PepaMx = $("#ip_" + parentId + "_PepaMx").val();
    measures.Pndc = $("#ip_" + parentId + "_Pndc").val();
    measures.Pdmx0 = $("#ip_" + parentId + "_Pdmx0").val();
    //measures.Pavg = $("#ip_" + parentId + "_Pavg").val();
    measures.RatedMVA = $("#ip_" + parentId + "_RatedMVA").val();
    measures.Qoe = $("#ip_" + parentId + "_Qoe").val();
    measures.Pmpo = $("#ip_" + parentId + "_Pmpo").val();
    measures.PSys = $("#ip_" + parentId + "_Psys").val();
    measures.Prmr = $("#ip_" + parentId + "_Prmr").val();
   //measures.Pplant = $("#ip_" + parentId + "_Pplant").val();
    measures.Pnp = $("#ip_" + parentId + "_Pnp").val();
    measures.Pdmx1 = $("#ip_" + parentId + "_Pdmx1").val();
    measures.Pmnp = $("#ip_" + parentId + "_Pmnp").val();
    measures.RatedPF = $("#ip_" + parentId + "_RatedPF").val();
    measures.Que = $("#ip_" + parentId + "_Que").val();
    measures.Pmnr = $("#ip_" + parentId + "_Pmnr").val();
    return measures;
}

function getLoadMeasures(loadParentId) {
    var measures = new Measures();
    measures.LdGrSpk = $("#ip_" + loadParentId + "_LdGrSpk").val();
    measures.LdGrSmn = $("#ip_" + loadParentId + "_LdGrSmn").val();
    measures.LdGrWpk = $("#ip_" + loadParentId + "_LdGrWpk").val();
    measures.LdGrWmn = $("#ip_" + loadParentId + "_LdGrWmn").val();
    return measures;
}

function getResourceBundleMeasures(parentId) {
    var measures = new Measures();
    measures.Pdgc = $("#ip_" + parentId + "_Pdgc").val();
    measures.Pelcc = $("#ip_" + parentId + "_Pelcc").val();
    measures.Pmpo = $("#ip_" + parentId + "_Pmpo").val();
    measures.Pndc = $("#ip_" + parentId + "_Pndc").val();
    measures.Pnp = $("#ip_" + parentId + "_Pnp").val();  
    return measures;
}

function getSummeryMasterList(parentId) {
    var masterList = new MasterList();
    masterList.UseMasterPdgc = $("#rb_yes_" + parentId + "_Pdgc").prop("checked");
    masterList.UseMasterPelcc = $("#rb_yes_" + parentId + "_Pelcc").prop("checked");
    masterList.UseMasterPdmx0 = $("#rb_yes_" + parentId + "_Pdmx0").prop("checked");
    masterList.UseMasterPdmx1 = $("#rb_yes_" + parentId + "_Pdmx1").prop("checked");
    masterList.UseMasterPmpo = $("#rb_yes_" + parentId + "_Pmpo").prop("checked");
    masterList.UseMasterPndc = $("#rb_yes_" + parentId + "_Pndc").prop("checked");
    masterList.UseMasterPnp = $("#rb_yes_" + parentId + "_Pnp").prop("checked");
    masterList.UseMasterPptp = $("#rb_yes_" + parentId + "_Pptp").prop("checked");
    masterList.UseMasterPrmr = $("#rb_yes_" + parentId + "_Prmr").prop("checked");
    return masterList;
}

function getMasterList(parentId) {
    var masterList = new MasterList();
    masterList.UseMasterPdgc = $("#rb_yes_" + parentId + "_Pdgc").prop("checked");
    masterList.UseMasterPelcc = $("#rb_yes_" + parentId + "_Pelcc").prop("checked");
    masterList.UseMasterPdmx0 = $("#rb_yes_" + parentId + "_Pdmx0").prop("checked");
    masterList.UseMasterPdmx1 = $("#rb_yes_" + parentId + "_Pdmx1").prop("checked");
    masterList.UseMasterPmpo = $("#rb_yes_" + parentId + "_Pmpo").prop("checked");
    masterList.UseMasterPndc = $("#rb_yes_" + parentId + "_Pndc").prop("checked");
    masterList.UseMasterPnp = $("#rb_yes_" + parentId + "_Pnp").prop("checked");
    masterList.UseMasterPptp = $("#rb_yes_" + parentId + "_Pptp").prop("checked");
    masterList.UseMasterPrmr = $("#rb_yes_" + parentId + "_Prmr").prop("checked");
    return masterList;
}