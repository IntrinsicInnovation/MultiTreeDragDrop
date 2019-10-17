<%@ Page Title="Cut Plane Boundary Creation" Language="C#"  AutoEventWireup="true" CodeBehind="CutPlaneNavigation.aspx.cs"  Inherits="EpicWeb.Pages.Data.CutPlaneNavigation" %>




<html xmlns="http://www.w3.org/1999/xhtml" >

    <head>
        <title>
           JsTree Drag and Drop with Multiple Trees
      </title>


  <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>

  <%--<script type="text/javascript" src="/Scripts/jquery-ui.min.js"></script>  --%>

  <script type="text/javascript" src="/Scripts/jquery.jstree.js"></script>

  <script type="text/javascript">
      
      $(document).ready(function () {
          $.jstree._themes = "/CSS/themes/";
          LoadTrees();

      });


      function LoadTrees() {
          //var mapid = getMapId(1); //cutplaneid);
          //Can have more than one set of trees, or remove the parameter entirely for just one.
          loadAllGroupsTree(1);
          loadAddedGroupsTree(1);
      }

      

      var manualmove;
      function loadAllGroupsTree(mapid) {
          var substationlist;
          manualmove = false;
          var poolid = 1;
          var url = "/Services/CutPlaneManagement.asmx/GetAllElectricalGroupsSubstations?poolId=" + poolid + "&mapId=" + mapid;
          $("#divGroupingsTree").jstree({
              "plugins": ["themes", "html_data", "dnd", "ui", "types", "crrm"],
              "core": { "animation": 1 },
              "crrm": {
                  "move": {
                      "default_position": "first",
                      "check_move": function (m) {
                          var targetnode = m.o[0];
                          var targetnodeid = targetnode.id;
                          if (targetnodeid.toLowerCase().indexOf("subst_") >= 0) {
                              return true;
                          }
                          else {
                              var parentid = targetnode.children[1].attributes["pkey"];
                              return (parentid == undefined || parentid == null) ? false : true;
                          }
                      }
                  }
              },
              "html_data": {
                  "ajax": {
                      "url": url,
                      "success": function () {
                        
                      }
                  }
              },
              "types": {
                  "max_depth": -2,
                  "max_children": -2,
                  "valid_children": ["electricalgroup"],
                  "types": {
                      "default": {
                          "icon": {
                              "image": "/Images/substation.png"
                          },
                          "valid_children": "none"
                      },
                      "electricalgroup": {
                          "icon": {
                              "image": "/Images/folder.png"
                          },
                          "valid_children": ["default", "electricalgroup"]
                      }
                  }
              }
          }) // end jstree(
           .bind("move_node.jstree", function (e, data) {
               if (manualmove == true) {
                   manualmove = false;
                   return;
               }
               substationlist = new Array();
               var rootlist = new Array();
               data.rslt.o.each(function (i) {
                   var itemid = $(this).attr("id");

                   if (itemid != null) {
                       var egid;
                       if (itemid.indexOf("subst_") >= 0) {
                           egid = $(this).attr("egid");
                           var substationid = itemid.replace('subst_', '');
                           var arraytopush = new Array();
                           arraytopush[0] = egid;
                           arraytopush[1] = substationid;
                           rootlist.push(arraytopush);
                       }
                       else {
                           egid = itemid.replace('eg_', '');
                           var arraytopush = new Array();
                           arraytopush[0] = egid;
                           arraytopush[1] = -1;
                           rootlist.push(arraytopush);
                       }
                       substationlist.push(itemid);
                       createSubstationList(substationlist, this);
                   } //end if itemid != null
               });  //end data..each ....


               var poolid = 1; //hardcoded just for demo.  Was originally grouped by map and pool id's.
               var json = JSON.stringify({ substList: substationlist, poolId: poolid, mapId: mapid });
               var url = "/Services/CutPlaneManagement.asmx/RemoveSubstations";
               $.ajax({
                   type: 'POST',
                   contentType: "application/json; charset=utf-8",
                   url: url,
                   data: json,
                   error: function (response, error, details) {
                       try {
                           var message = "An error has occurred calling RemoveSubstations.";
                           message += "\n";
                           message += "Response: " + response.responseText;
                           message += "\n";
                           message += "Error: " + error;
                           message += "\n";
                           message += "Details: " + details;
                           alert(message);
                       }
                       catch (exception) {
                           // HandleException(exception);
                       }
                   },
                   success: function (data) {


                       var parenteglist = data.d;
                       var length = parenteglist.length;
                       var tree = jQuery.jstree._reference("#divGroupingsTree");

                       var parenteg = findChild(parenteglist, null);
                       for (var i = 0; i < length; i++) {
                           if (parenteg.length > 0) {
                               var parentid = parenteg[0].ParentPk;
                               var nodeid = parenteg[0].Id;
                               var nodetocreate = [];
                               var nodetocreate = $("div#divGroupingsTree li#eg_" + nodeid);

                               var newnode;
                               if (nodetocreate.length == 0) {
                                   var parentnode;
                                   $("div#divGroupingsTree li#eg_" + parentid).each(function () {
                                       if ($(this).attr("treetype") == "available") {
                                           parentnode = $(this);
                                       }
                                   });
                                   //******** CREATE NODE ********
                                   if (parentnode != undefined && parentnode != null && parentnode.length > 0) {
                                       var title = parenteg[0].Abbreviation + " - " + parenteg[0].Name;
                                       var shorttitle = parenteg[0].Name;
                                       newnode = tree.create_node(parentnode, 0, { "attr": { "id": "eg_" + nodeid, "rel": "electricalgroup", "treetype": "available" }, "data": title }, null, true);
                                       newnode.children("a").attr("class", "textHolder");
                                       newnode.children("a").attr("pkey", parentid);
                                       newnode.children("a").attr("title", shorttitle);
                                       newnode.children("a").attr("key", nodeid);
                                       newnode.children("a").removeAttr("href");
                                   } //end if(parentnode.length > 0) 
                               } //end if (nodetocreate.length == 0)

                               //Get next child eg in the tree (get eg with parent key equal to the current id)
                               parenteg = findChild(parenteglist, parenteg[0].Id);
                           } //end if (parenteg.length > 0) {
                       } //end for

                       var parentidtomove;
                       var parentnodetomove;
                       //Moving Substation
                       if (rootlist[0][1] != "-1") {
                           parentidtomove = rootlist[0][0];
                           $("div#divGroupingsTree li#eg_" + parentidtomove).each(function () {
                               if ($(this).attr("treetype") == "available") {
                                   parentnodetomove = $(this);
                               }
                           });
                       }
                       //** Moving EG **
                       else {
                           var egobject = findEG(parenteglist, rootlist[0][0]);
                           parentidtomove = egobject[0].ParentPk; //returns ARRAY with ONE element.
                           $("div#divGroupingsTree li#eg_" + parentidtomove).each(function () {
                               if ($(this).attr("treetype") == "available") {
                                   parentnodetomove = $(this);
                               }
                           });
                       }

                       //******** Recursively MOVE SUBNODES ********
                       moveNodes(rootlist, parentnodetomove, parentnodetomove, parenteglist, tree, "available");

                       //******** DELETE Duplicate SUBNODES  (and set treetype if not Duplicate) ********
                       $("div#divGroupingsTree li").each(function () {
                           if ($(this).attr("treetype") == "added") {
                               var potentialduplicateid = $(this).attr("id")
                               var potentialduplicatenodes = $("div#divGroupingsTree li#" + potentialduplicateid);
                               if (potentialduplicatenodes.length > 1) {
                                   var nodetodelete = $(this);
                                   tree.delete_node(nodetodelete);
                               }
                               else {
                                   var nodetochangetype = $(this);
                                   nodetochangetype.attr("treetype", "available");
                               }
                           }
                       });
                   } //end- success
               }); //end - ajax
           });           //end .bind...
       } //end loadAllGroupsTree.


      
       function loadAddedGroupsTree(mapid) {
           //      //Create added groups tree.
           var substationlist;
           manualmove = false;
           var poolid = 1; 
           var url = "/Services/CutPlaneManagement.asmx/GetAddedGroups?poolId=" + poolid + "&mapId=" + mapid;
           $("#divAddedGroups").jstree({
               "plugins": ["html_data", "themes", "dnd", "ui", "types", "crrm"],
               "core": { "animation": 1 },
               "crrm": {
                   "move": {
                       "default_position": "first",
                       "check_move": function (m) {
                           var targetnode = m.o[0];
                           var targetnodeid = targetnode.id;
                           if (targetnodeid.toLowerCase().indexOf("subst_") >= 0) {
                               return true;
                           }
                           else {
                               var parentid = targetnode.children[1].attributes["pkey"];
                               return (parentid == undefined || parentid == null) ? false : true;
                           }
                       }
                   }
               },
               "html_data": {
                   "ajax": {
                       "url": url,
                       success: function () {
                           $('selector-for-jstree-container').jstree('loaded');
                       }
                   }
               },
               "types": {
                   "max_depth": -2,
                   "max_children": -2,
                   "valid_children": ["electricalgroup"],
                   "types": {
                       "default": {
                           "icon": {
                               "image": "/Images/substation.png"
                           },
                           "valid_children": "none"
                       },
                       "electricalgroup": {
                           "icon": {
                               "image": "/Images/folder.png"
                           },
                           "valid_children": ["default", "electricalgroup"]
                       }
                   }
               }
           })
            .bind("move_node.jstree", function (e, data) {
                if (manualmove == true) {
                    manualmove = false;
                    return;
                }
                substationlist = new Array();
                var rootlist = new Array();
                data.rslt.o.each(function (i) {
                    var itemid = $(this).attr("id");
                    if (itemid != null) {
                        var egid;
                        if (itemid.indexOf("subst_") >= 0) {
                            egid = $(this).attr("egid");
                            var substationid = itemid.replace('subst_', '');
                            var arraytopush = new Array();
                            arraytopush[0] = egid;
                            arraytopush[1] = substationid;
                            rootlist.push(arraytopush);
                        }
                        else {
                            egid = itemid.replace('eg_', '');
                            var arraytopush = new Array();
                            arraytopush[0] = egid;
                            arraytopush[1] = -1;
                            rootlist.push(arraytopush);
                        }
                        substationlist.push(itemid);
                        createSubstationList(substationlist, this);
                    } //end if itemid != null
                });  //end data..each ....

                var json = JSON.stringify({ substList: substationlist, poolId: poolid, mapId: mapid });
                var url = "/Services/CutPlaneManagement.asmx/UpdateSubstations";
                $.ajax({
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    url: url,
                    data: json,
                    error: function (response, error, details) {
                        try {
                            var message = "An error has occurred calling UpdateSubstations.";
                            message += "\n";
                            message += "Response: " + response.responseText;
                            message += "\n";
                            message += "Error: " + error;
                            message += "\n";
                            message += "Details: " + details;
                            alert(message);
                        }
                        catch (exception) {
                             HandleException(exception);
                        }
                    },
                    success: function (data) {
                        var parenteglist = data.d;
                        var length = parenteglist.length;
                        var tree = jQuery.jstree._reference("#divAddedGroups");

                        var parenteg = findChild(parenteglist, null);
                        for (var i = 0; i < length; i++) {
                            if (parenteg.length > 0) {
                                var parentid = parenteg[0].ParentPk;
                                var nodeid = parenteg[0].Id;
                                var nodetocreate = [];
                                var nodetocreate = $("div#divAddedGroups li#eg_" + nodeid);

                                var newnode;
                                if (nodetocreate.length == 0) {
                                    var parentnode;
                                    $("div#divAddedGroups li#eg_" + parentid).each(function () {
                                        if ($(this).attr("treetype") == "added") {
                                            parentnode = $(this);
                                        }
                                    });
                                    //******** CREATE NODE ********
                                    if (parentnode != undefined && parentnode != null && parentnode.length > 0) {
                                        var title = parenteg[0].Abbreviation + " - " + parenteg[0].Name;
                                        var shorttitle = parenteg[0].Name;
                                        newnode = tree.create_node(parentnode, 0, { "attr": { "id": "eg_" + nodeid, "rel": "electricalgroup", "treetype": "added" }, "data": title }, null, true);
                                        newnode.children("a").attr("class", "textHolder");
                                        newnode.children("a").attr("pkey", parentid);
                                        newnode.children("a").attr("title", shorttitle);
                                        newnode.children("a").attr("key", nodeid);
                                        newnode.children("a").removeAttr("href");
                                    } //end if(parentnode.length > 0) 
                                } //end if (nodetocreate.length == 0)

                                //Get next child eg in the tree (get eg with parent key equal to the current id)
                                parenteg = findChild(parenteglist, parenteg[0].Id);
                            } //end if (parenteg.length > 0) {
                        } //end for

                        var parentidtomove;
                        var parentnodetomove;
                        //Moving Substation
                        if (rootlist[0][1] != "-1") {
                            parentidtomove = rootlist[0][0];
                            $("div#divAddedGroups li#eg_" + parentidtomove).each(function () {
                                if ($(this).attr("treetype") == "added") {
                                    parentnodetomove = $(this);
                                }
                            });
                        }
                        //** Moving EG **
                        else {
                            var egobject = findEG(parenteglist, rootlist[0][0]);
                            parentidtomove = egobject[0].ParentPk; //returns ARRAY with ONE element.
                            $("div#divAddedGroups li#eg_" + parentidtomove).each(function () {
                                if ($(this).attr("treetype") == "added") {
                                    parentnodetomove = $(this);
                                }
                            });
                        }
                        //******** Recursively MOVE SUBNODES ********
                        moveNodes(rootlist, parentnodetomove, parentnodetomove, parenteglist, tree, "added");
                        //******** DELETE Duplicate SUBNODES  (and set treetype if not Duplicate) ********
                        $("div#divAddedGroups li").each(function () {
                            if ($(this).attr("treetype") == "available") {
                                var potentialduplicateid = $(this).attr("id")
                                var potentialduplicatenodes = $("div#divAddedGroups li#" + potentialduplicateid);
                                if (potentialduplicatenodes.length > 1) {
                                    var nodetodelete = $(this);
                                    tree.delete_node(nodetodelete);
                                }
                                else {
                                    var nodetochangetype = $(this);
                                    nodetochangetype.attr("treetype", "added");
                                }
                            }
                        });

                        //Need a sorting mechanism here.
                      

                    } //end- success
                }); //end - ajax
            });   //end .bind("move_node.jstree", function (e, data)                                                          
        } // end - loadAddedGroupsTree


        //******** Move Nodes ********
        //Chris J: recursive function starts here:
        //can just use array of eg-substation here for nodes
        function moveNodes(sourcelist, destnode, newnode, parenteglist, tree, destinationtreetype) {
            var treetype1;
            var treetype2;
            var treediv;

            if (destinationtreetype == "added") {
                treetype1 = "added";
                treetype2 = "available";
                treediv = "div#divAddedGroups";
            }
            else {
                treetype1 = "available";
                treetype2 = "added";
                treediv = "div#divGroupingsTree";
            }
            var sourcelength = sourcelist.length;
            for (var sourceindex = 0; sourceindex < sourcelength; sourceindex++) {
                var egid = sourcelist[sourceindex][0];
                var substationid = sourcelist[sourceindex][1];
                var sourcenode = $(treediv + " li#eg_" + egid);
                if (sourcenode.length > 1 && substationid == -1) { 
               
                    var childsourcelist = new Array();
                    $(treediv + " li#eg_" + egid).each(function () {
                        if ($(this).attr("treetype") == treetype2) {
                            $(this).children("ul").children("li").each(function () {
                                //Test for substation node
                                var substationtag = this.id;
                                if (substationtag.indexOf("subst_") >= 0) {
                                    var substationid = substationtag.replace('subst_', '');
                                    var arraytopush = new Array();
                                    arraytopush[0] = $(this).attr("egid");
                                    arraytopush[1] = substationid;
                                    childsourcelist.push(arraytopush);
                                }
                                else {  //test for EG node.
                                    var parentkey = $(this).children("a").attr("pkey");
                                    var currentkey = $(this).children("a").attr("key");
                                    if (parentkey == egid) {
                                        var arraytopush = new Array();
                                        arraytopush[0] = currentkey;
                                        arraytopush[1] = -1;
                                        childsourcelist.push(arraytopush);
                                    }
                                }
                            });
                        }
                    });

                    var parentidtomove;
                    var parentnodetomove;
                    //Moving Substation
                    if (childsourcelist[0][1] != "-1") {
                        parentidtomove = childsourcelist[0][0];
                        $(treediv + " li#eg_" + parentidtomove).each(function () {
                            if ($(this).attr("treetype") == treetype1) {
                                parentnodetomove = $(this);
                            }
                        });
                    }
                    //** Moving EG **
                    else {
                        var egobject = findEG(parenteglist, childsourcelist[0][0]);
                        parentidtomove = egobject[0].ParentPk; //returns ARRAY with ONE element.
                        $(treediv + " li#eg_" + parentidtomove).each(function () {
                            if ($(this).attr("treetype") == treetype1) {
                                parentnodetomove = $(this);
                            }
                        });
                    }
                    moveNodes(childsourcelist, parentnodetomove, parentnodetomove, parenteglist, tree, destinationtreetype);
                }  //end    if (sourcenode.length > 1 && substationid == -1)  //if more than 1 EG found
                else {
                    if (substationid == -1) { //moving an EG node.
                        if (destnode != newnode) {
                            desteg = findParentKey(parenteglist, parseInt(egid));
                            //Destination should be in added tree.
                            $(treediv + " li#eg_" + desteg).each(function (m) {
                                var treetype = $(this).attr("treetype");
                                if (treetype == treetype1) {
                                    destnode = $(this);
                                }
                            });
                        }
                        //if it's an EG, set tree type to destination tree type.
                        sourcenode.attr("treetype", treetype1);
                    }
                    else {  //moving a Substation node.
                        var sourcenode = $(treediv + " li#subst_" + substationid);
                    }
                    manualmove = true;
                    tree.move_node(sourcenode, destnode, "inside");
                } // end else 
            } // end for (var sourceindex = 0; sourceindex < sourcelength; sourceindex++) {
        } // end function moveNode()


        function findChild(parenteglist, parentKey){
            return $.grep(parenteglist, function (item) {
                return item.ParentPk == parentKey;
            });
        }


        function findEG(parenteglist, egId){
            return $.grep(parenteglist, function (item) {
                return item.Id == egId;
            });
        }

      //CJ - custom recursive function to traverse Tree, and return all parents and children ID's in substlist array.
      function createSubstationList(substlist, parent) {
            if (substlist == null) {
                substlist = new Array();
            }
            var children = $(parent).children();
            var child3 = children[2];
            if (child3 != null) {
                var child3children = child3.children;
                var length = child3children.length;
                for (var i = 0; i < length; i++) {  
                    var childid = child3children[i].id;
                    if (childid != null) {
                        substlist.push(childid);
                        createSubstationList(substlist, child3children[i]);
                    } //end if (childid...
                }  //end for (i...
           }//end if (child3 != null)
      } //end createsubstationlist()

      //CJ - Generic function to get QueryString in Jquery.
      function getQueryString () {
        var urlParams = {};
          var e,
        a = /\+/g,  // Regex for replacing addition symbol with a space
        r = /([^&=]+)=?([^&]*)/g,
        d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
        q = window.location.search.substring(1);
          while (e = r.exec(q))
              urlParams[d(e[1])] = d(e[2]);
          return urlParams;
      }


      function getMapId(cutPlaneId) {  //Use if need more than one grouping of tree controls.
          var json = "{ \"cutPlaneID\" : \"" + cutPlaneId + "\"" + "}";
          $.ajaxSetup({ async: true, timeout: 15000 });
          $.ajax({
              type: "POST",
              url: "/Services/CutPlaneManagement.asmx/GetCutPlane",
              data: json,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              error: function (response, error, details) {
                  try {
                      var message = "An error has occurred in getMapId().";
                      message += "\n";
                      message += "Response: " + response.responseText;
                      message += "\n";
                      message += "Error: " + error;
                      message += "\n";
                      message += "Details: " + details;
                      alert(message);
                      return null;
                  }
                  catch (exception) {
                       HandleException(exception);
                  }
              },
              success: function (data) {
                  try {
                      var mapid = data.d.MapId;
                      loadAllGroupsTree(mapid);
                      loadAddedGroupsTree(mapid);
                  }
                  catch (exception) {
                       HandleException(exception);
                  }
              }
          });
      }



      function HandleException(exception) {
          try {
              var json = "{ \"Message\" : \"" + exception + "\" }";
              $.ajax({
                  type: "POST",
                  contentType: "application/json; charset=utf-8",
                  url: "/Services/ExceptionHandler.asmx/HandleException",
                  data: json,
                  dataType: "json"
              });
              alert("An exception has occurred.\n\nDetails: " + exception);
          }
          catch (exception) {
          }
      }

  </script>
  <style type="text/css">
       
    
        
        .cutPlaneDetailRow
        {
            display:block;
            margin:2px 0px 2px 0px;
            height:25px;
        }  
       
    
        .formBlock
        {
            border:2px solid #669900;    
            color:#1E90FF;
            
        }  

        #divGroupingsTree
        {
            margin:5px 20px 0px 0px;
            width:470px;
            float:left;
            overflow:scroll;
            height:520px;
           
        }
        
        #divAddedGroups
        {
            margin: 5px 5px 0px 0px;
            width:498px;
            float:left;
            overflow:scroll;
            height:437px;
        }
		
		
		
       .mytestclass {
          color: #F00;
          width: 25px;
          font-weight: bold;
        }


		
</style>
</head>
    <body>
        <form id="Mainform" runat="server">
        <div>
            <div id="mainBlock" class="standardBlock" style="height:875px;">
                <div>
                    <h1>JsTree Drag and Drop with Multiple Trees</h1>
                </div>
                <hr />
                <div class="cutPlaneDetailRow" style="height:560px;width:975px;float:left; margin:0px 0px 2px 2px; padding: 5px 5px 2px 2px; color:#3BB9FF; border: 1px solid #222222;">
                    <div class="cutPlaneDetailRow" style="height:550px;width:475px;float:left; margin:0px 0px 2px 2px; padding: 5px 2px 2px 5px; background-color:#FFFFFF;  border: 1px solid #222222;">
                        <div class="cutPlaneDetailRow" style="height:15px;width:150px;float:left;">
                            <div class="formLabels" style="height:15px; width:140px; margin:0px 0px 2px 0px; padding-left:0px;padding-right:5px;padding-top:0px; float:left; text-align:left; font-weight:bold; text-decoration:underline; color: #000000">Available</div>
                        </div>
                        <div id="divGroupingsTree" class="formBlock" style="float:left; border:2px solid #669900; height:520px;width:470px;">
                        </div>
                    </div>
                    <div class="cutPlaneDetailRow" style="height:550px;width:475px;float:left; margin: 0px 0px 2px 2px; padding: 5px 2px 2px 5px; border: 1px solid #222222;">
                        <div class="cutPlaneDetailRow" style="height:15px;width:150px;float:left;">
                            <div class="formLabels" style="height:15px; width:140px; margin:0px 0px 2px 0px; padding-left:0px;padding-right:5px;padding-top:0px; float:left; text-align:left; font-weight:bold; text-decoration:underline; color: #000000">Added</div>
                        </div>
                        <div id="divAddedGroups" class="formBlock" style="float:left; border:2px solid #669900; margin: 5px 5px 0px 0px; height:520px;width:470px;">
                        </div>
                    </div>
                </div> <!-- end CutPlaneDetailRow --> 
            </div> <!-- end mainBlock --> 
        </div>   <!-- end main Div -->
        </form>
    </body>
</html>

