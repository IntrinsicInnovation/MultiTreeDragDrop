using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.Script.Services;
using System.Xml;
//using EpicDataAccess.SystemLevelForecast;
using System.Web.UI.HtmlControls;
//using EpicBusinessLogic.Domain.Substations;
using EpicBusinessLogic.Extensions;
using EpicDataAccess.CutPlanes;
using CutPlanes.DataAccess.CutPlanes;
//using EpicSecurity.Membership;
//using log4net;


namespace EpicWeb.Services
{
    /// <summary>
    /// Summary description for CutPlaneManagement
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CutPlaneManagement : System.Web.Services.WebService
    {

        #region Properties
            List<Substation> addedSubstations { get; set; }
            List<ElectricalGroup> egParents { get; set; }
            int destinationEGCount { get; set; }

            //private ILog _log = null;
            //private ILog Log
            //{
            //    get
            //    {
            //        if (_log == null)
            //        {
            //            log4net.Config.XmlConfigurator.Configure();
            //            _log = LogManager.GetLogger(typeof(ExceptionHandler));

            //        }
            //        return _log;
            //    }
            //}

            private string ConnectionString { get; set; }

            private EpicCutPlanes _epicCutPlanes = null;
            private EpicCutPlanes EpicCutPlanes
            {
                get
                {
                    if (_epicCutPlanes == null) _epicCutPlanes = new EpicCutPlanes(ConnectionString);
                    return _epicCutPlanes;
                }
            }


            //private ResourcePlan _rps = null;
            //private ResourcePlan ResourcePlans
            //{
            //    get
            //    {
            //        if (_rps == null) _rps = new ResourcePlan();
            //        return _rps;
            //    }
            //}

        #endregion



        #region [Constructors]

        public CutPlaneManagement(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public CutPlaneManagement()
        {
          //  this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
        }


        #endregion



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int SaveConstraintsSet(int constraintSetId, string constraintName, int poolId)
        {
            //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
            //EpicCutPlanes epicCutPlanes = new EpicCutPlanes(ConnectionString);
            try
            {
                DateTime createdDate = DateTime.Now;
             //   EpicSecurity.Membership.EpicMembershipUser user = EpicSecurity.Membership.EpicMembership.GetUser(HttpContext.Current.User.Identity.Name);
                ConstraintSet constraintSet = new ConstraintSet();

                constraintSet.Id = constraintSetId;
                constraintSet.Name = constraintName;
                constraintSet.PoolId = poolId;
                constraintSet.DateCreated = createdDate;
                constraintSet.UserCreatedId = 1; // user.EpicUserId;
                return EpicCutPlanes.SaveConstraintSet(constraintSet);

            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::SaveConstraintsSet: Exception" + ex.Message + ex.StackTrace);
                throw;
            }

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SaveConstraint(int colId, int row, string value, int constraintSetId)
        {
            //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
            //EpicCutPlanes epicCutPlanes = new EpicCutPlanes(ConnectionString);
            try
            {
                DateTime createdDate = DateTime.Now;
               // EpicSecurity.Membership.EpicMembershipUser user = EpicSecurity.Membership.EpicMembership.GetUser(HttpContext.Current.User.Identity.Name);
                EpicCutPlanes.SaveConstraint(colId, row, value, createdDate, 1 /* user.EpicUserId */, constraintSetId);
                //Log.Debug("SaveConstraint: Saving record for constraint col: " + colId.ToStringOrNull() + ", row: " + row.ToStringOrNull() + ", value: " + value + ", constraintsetid: " + constraintSetId.ToStringOrNull() );
            }
            catch (Exception ex)
            {
               // Log.Error("CutPlaneManagement::SaveConstraint: Exception" + ex.Message + ex.StackTrace + "Constraint info: col: " + colId.ToStringOrNull() + ", row: " + row.ToStringOrNull() + ", value: " + value + ", constraintsetid: " + constraintSetId.ToStringOrNull());
                throw;
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SaveConstraintRow(int rowId, int constraintSetId, string[] constraintArray)
        {
            //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
            //EpicCutPlanes epicCutPlanes = new EpicCutPlanes(ConnectionString);
            try
            {
                DateTime createdDate = DateTime.Now;
                //EpicSecurity.Membership.EpicMembershipUser user = EpicSecurity.Membership.EpicMembership.GetUser(HttpContext.Current.User.Identity.Name);

                Constraint currentConstraint = new Constraint();
                currentConstraint.DateCreated = createdDate;
                currentConstraint.UserCreatedId = 1; // user.EpicUserId;
                currentConstraint.Id = constraintArray[0].ToInt32();

                currentConstraint.FiscalYear = constraintArray[1].ToInt32();
                //currentConstraint.StartDate = constraintArray[1].ToDateTime();
                //currentConstraint.EndDate = constraintArray[2].ToDateTime();
                currentConstraint.ConstraintSetId = constraintSetId;
                currentConstraint.SummerN0 = constraintArray[2].ToInt32();
                currentConstraint.SummerN1 = constraintArray[3].ToInt32();
                currentConstraint.WinterN0 = constraintArray[4].ToInt32();
                currentConstraint.WinterN1 = constraintArray[5].ToInt32();
                currentConstraint.AreaLoss = constraintArray[6].ToInt32();
                currentConstraint.AreaLossInPercentage = constraintArray[7].ToInt32();
                currentConstraint.TotalLoss = constraintArray[8].ToInt32();
                currentConstraint.TotalLossInPercentage = constraintArray[9].ToInt32();
                currentConstraint.IntermittentRmr = constraintArray[10].ToInt32();
                EpicCutPlanes.SaveConstraintRow(currentConstraint); //colId, row, value, createdDate, user.EpicUserId, constraintSetId);
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::SaveConstraintRow: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool DeleteConstraintRow(int rowId)
        {
            //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
            //EpicCutPlanes epicCutPlanes = new EpicCutPlanes(ConnectionString);
            try
            {
                if (rowId <= 0)
                    return false;
                return EpicCutPlanes.DeleteConstraintRow(rowId);
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::DeleteConstraintRow: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        [WebMethod]
        public ConstraintSet GetConstraintSet(int constraintSetId)
        {
            //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
            //EpicCutPlanes epicCutPlanes = new EpicCutPlanes(ConnectionString);
            try
            {
                ConstraintSet constraintset = EpicCutPlanes.GetConstraintSet(constraintSetId);
                ConstraintSet serializableconstraintset = new ConstraintSet();
                serializableconstraintset.Id = constraintset.Id;
                serializableconstraintset.DateCreated = constraintset.DateCreated;
                serializableconstraintset.DateUpdated = constraintset.DateUpdated;
                serializableconstraintset.Name = constraintset.Name;
                serializableconstraintset.PoolId = constraintset.PoolId;
                serializableconstraintset.UserCreatedId = constraintset.UserCreatedId;
                serializableconstraintset.UserUpdatedId = constraintset.UserUpdatedId;
                return serializableconstraintset;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetConstraintSet: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<ConstraintSet> GetAllConstraintSets(int poolId)
        {
            try
            {
                List<ConstraintSet> constraintSets = EpicCutPlanes.GetAllConstraintSets(poolId);
                List<ConstraintSet> serializableConstraintSets = new List<ConstraintSet>();
                foreach (ConstraintSet constraintSet in constraintSets)
                {
                    ConstraintSet serializableConstraintSet = new ConstraintSet();
                    serializableConstraintSet.Id = constraintSet.Id;
                    serializableConstraintSet.Name = constraintSet.Name;
                    serializableConstraintSet.DateCreated = constraintSet.DateCreated;
                    serializableConstraintSet.DateUpdated = constraintSet.DateUpdated;
                    serializableConstraintSet.PoolId = constraintSet.PoolId;
                    serializableConstraintSet.UserCreatedId = constraintSet.UserCreatedId;
                    serializableConstraintSet.UserUpdatedId = constraintSet.UserUpdatedId;
                    serializableConstraintSets.Add(serializableConstraintSet);
                }
                return serializableConstraintSets;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetAllConstraintSets: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Constraint> GetConstraints(int constraintSetId)
        {
            //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
            //EpicCutPlanes epicCutPlanes = new EpicCutPlanes(ConnectionString);
            try
            {
                List<Constraint> constraints = EpicCutPlanes.GetConstraints(constraintSetId);
                List<Constraint> serializableconstraints = new List<Constraint>();
                foreach (Constraint constraint in constraints)
                {
                    Constraint serializableconstraint = new Constraint();
                    serializableconstraint.Id = constraint.Id;
                    serializableconstraint.AreaLoss = constraint.AreaLoss;
                    serializableconstraint.AreaLossInPercentage = constraint.AreaLossInPercentage;
                    serializableconstraint.ConstraintSetId = constraint.ConstraintSetId;
                    serializableconstraint.DateCreated = constraint.DateCreated;
                    serializableconstraint.DateUpdated = constraint.DateUpdated;
                    serializableconstraint.FiscalYear = constraint.FiscalYear;
                    serializableconstraint.SummerN0 = constraint.SummerN0;
                    serializableconstraint.SummerN1 = constraint.SummerN1;
                    serializableconstraint.TotalLoss = constraint.TotalLoss;
                    serializableconstraint.TotalLossInPercentage = constraint.TotalLossInPercentage;
                    serializableconstraint.UserCreatedId = constraint.UserCreatedId;
                    serializableconstraint.UserUpdatedId = constraint.UserUpdatedId;
                    serializableconstraint.WinterN0 = constraint.WinterN0;
                    serializableconstraint.WinterN1 = constraint.WinterN1;
                    serializableconstraint.IntermittentRmr = constraint.IntermittentRmr;
                    serializableconstraints.Add(serializableconstraint);
                }
                return serializableconstraints;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetConstraints: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public XmlDocument GetAddedGroups(int poolId, int mapId)
        {
            try
            {
                addedSubstations = new List<Substation>();
                egParents = new List<ElectricalGroup>();
                List<ElectricalGroup> mapEGs = new List<ElectricalGroup>();
              
                HtmlGenericControl parent = new HtmlGenericControl();
                int parentGroupId = EpicCutPlanes.GetParentGroupId(mapId);
                ElectricalGroup root = GetRootElectricalGroup(parentGroupId, true, mapEGs);

                addedSubstations = EpicCutPlanes.GetPoolSubstations(poolId);
                egParents = GetElectricalGroupParents(addedSubstations, mapEGs); //int mapId,

                GenerateElement(root, parent, "added");
                destinationEGCount = 1;  //Since 1 E.G. has been drawn.
                GenerateDestChildList(root, parent.Controls[parent.Controls.Count - 1] as HtmlGenericControl, "added"); //, addedSubstations, egParents);
                string contents = null;
                using (System.IO.StringWriter swriter = new System.IO.StringWriter())
                {
                    HtmlTextWriter writer = new HtmlTextWriter(swriter);
                    parent.RenderControl(writer);
                    contents = swriter.ToString();
                }
                contents = contents.Replace("<span>", "<ul>");
                contents = contents.Replace("</span>", "</ul>");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(contents);
                //"rel", "electricalgroup")
                //xmlDoc.LoadXml("<ul><li id='dhtml_1'><a href='#'>Root node 1</a><ul><li id='dhtml_2'><a href='#'>Child node 1</a>      </li><li id='dhtml_3'><a href='#'>Child node 2</a></li></ul></li><li id='dhtml_4'><a href='#'>Root node 2</a></li></ul>");
                    //<ul> <li id='eg1' rel='electricalgroup'>eg_1 <ul> <li id='sub1' rel='default'>  sub_1 </li>  <li id='sub2' rel='default'> sub2 </li></ul></li></ul>");
                return xmlDoc;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetAddedGroups: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public XmlDocument GetAllElectricalGroupsSubstations(int poolId, int mapId)
        {
            try
            {
                if (poolId <= 0 || mapId <= 0)
                    return null;
                //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
                //EpicCutPlanes epicCutPlanes = new EpicCutPlanes(ConnectionString);
                addedSubstations = new List<Substation>();
                List<Substation> sourceSubstations = new List<Substation>();
                egParents = new List<ElectricalGroup>();
                //mapEGs contain all EGs for that particular map only.
                List<ElectricalGroup> mapEGs = new List<ElectricalGroup>();
                addedSubstations = EpicCutPlanes.GetPoolSubstations(poolId);
                //for now, using Map 1 - since we are using maps now.

                // ElectricalGroup parentGroup = GetElectricalGroupChildren(
                int parentGroupId = EpicCutPlanes.GetParentGroupId(mapId);  
                HtmlGenericControl parent = new HtmlGenericControl();
                ElectricalGroup root = GetRootElectricalGroup(parentGroupId, true, mapEGs);

                sourceSubstations = EpicCutPlanes.GetSourceSubstations(root, addedSubstations);
                if (sourceSubstations != null)
                    egParents = GetElectricalGroupParents(sourceSubstations, mapEGs); //mapId

                GenerateElement(root, parent, "available");
                GenerateChildList(root, parent.Controls[parent.Controls.Count - 1] as HtmlGenericControl, "available"); //, addedSubstations, egParents);
                string contents = null;
                using (System.IO.StringWriter swriter = new System.IO.StringWriter())
                {
                    HtmlTextWriter writer = new HtmlTextWriter(swriter);
                    parent.RenderControl(writer);
                    contents = swriter.ToString();
                }

                contents = contents.Replace("<span>", "<ul>");
                contents = contents.Replace("</span>", "</ul>");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(contents);
                //xmlDoc.LoadXml("<ul><li id='phtml_1'><a href='#'>Root node 1</a><ul><li id='phtml_2'><a href='#'>Child node 1</a><ul><li id='phtml_5'><a href='#'>Child node 5</a></li><li id='phtml_6'><a href='#'>Child node 6</a></li></ul></li><li id='phtml_3'><a href='#'>Child node 2</a></li></ul></li><li id='phtml_4'><a href='#'>Root node 2</a></li></ul>");
                return xmlDoc;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetAllElectricalGroupsSubstations: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }

        public ElectricalGroup GetRootElectricalGroup(int parentGroupId, bool includeChildGroups, List<ElectricalGroup> mapEGs)
        {
            try
            {
                ElectricalGroup group = EpicCutPlanes.GetElectricalGroup(parentGroupId); // GetRootElectricalGroup();
                if (group == null) return null;
                mapEGs.Add(group);
                if (includeChildGroups)
                    group.ChildGroups = GetElectricalGroupChildren(group.Id, mapEGs);
                return group;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetRootElectricalGroup: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        private List<ElectricalGroup> GetElectricalGroupChildren(int key, List<ElectricalGroup> mapEGs)
        {
            try
            {
                List<ElectricalGroup> childGroups = EpicCutPlanes.GetElectricalChildGroups(key);
                if (childGroups != null && childGroups.Count > 0)
                    mapEGs.AddRange(childGroups);
                    foreach (ElectricalGroup eg in childGroups)
                        eg.ChildGroups = GetElectricalGroupChildren(eg.Id, mapEGs);
                return childGroups;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetElectricalGroupChildren: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        private List<ElectricalGroup> GetElectricalGroupParents(List<Substation> substationList, List<ElectricalGroup> mapEGs) //int mapId,
        {
           
            try
            {
                List<ElectricalGroup> parentGroups = new List<ElectricalGroup>();
                ElectricalGroup parentGroup;
                foreach (Substation s in substationList)
                {
                    parentGroup = EpicCutPlanes.GetElectricalGroupbySubstationId(s.Id, mapEGs); // mapId,
                        while (parentGroup != null)
                        {
                            if (!parentGroups.Contains(parentGroup))  //Implemented IEquatable.
                            {
                                parentGroups.Add(parentGroup);
                                if (parentGroup.ParentPk.HasValue)
                                    parentGroup = EpicCutPlanes.GetElectricalGroup(parentGroup.ParentPk.Value);
                                else
                                    break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    //}
                }
                return parentGroups;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetElectricalGroupParents: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        ///// <summary>
        ///// Gets Substation object by ID.
        ///// </summary>
        ///// <returns>Substation object.</returns>
        //public Substation GetSubstation(int substationId)
        //{
        //    try
        //    {
        //        return EpicCutPlanes.GetSubstation(substationId);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("CutPlaneManagement::GetSubstation: Exception" + ex.Message + ex.StackTrace);
        //        throw;
        //    }
        //}


        bool ContainsEg(ElectricalGroup electricalGroup, List<ElectricalGroup> groupList)
        {
            //if (electricalGroup.Abbreviation=="SI")
            //{
            //    int i = 1;
            //}
            if (groupList.Contains(electricalGroup))  //Implemented Ienumerable
                return true;
            else
                return false;
        }


        bool ContainsSubstation(int substIdtoCompare, List<Substation> substationList)
        {
                foreach (Substation s in substationList)
                {
                    if (s.Id == substIdtoCompare)
                        return true;
                }
                return false;
        }


        private void GenerateElement(ElectricalGroup group, HtmlGenericControl parentElement, string treeType)
        {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Attributes.Add("id", "eg_" + group.Id.ToString());
                li.Attributes.Add("rel", "electricalgroup");
                if (treeType == "available")
                {
                    li.Attributes.Add("treetype", "available");
                }
                else
                {
                    li.Attributes.Add("treetype", "added");
                }
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("class", "textHolder");
                a.Attributes.Add("key", group.Id.ToString());
                
                a.Attributes.Add("title", group.Name);
                if (group.ParentPk.HasValue)
                    a.Attributes.Add("pkey", group.ParentPk.Value.ToString());
                a.InnerText = string.Format("{0} - {1}", group.Abbreviation, group.Name);
                li.Controls.Add(a);
                parentElement.Controls.Add(li);
        }

        private void GenerateSubstationElement(Substation substation, ElectricalGroup group, HtmlGenericControl parentElement) //EpicDataAccess.Domain.
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes.Add("id", "subst_" +  substation.Id.ToString());
            li.Attributes.Add("egid", group.Id.ToString());
            li.Attributes.Add("rel", "default");
            HtmlGenericControl a = new HtmlGenericControl("a");
            a.Attributes.Add("class", "textHolder");
            a.Attributes.Add("key", substation.Id.ToString());
            
            a.Attributes.Add("title", substation.Name);
            a.InnerText = string.Format("{0} - {1}", substation.TLA, substation.Name);
            li.Controls.Add(a);
            parentElement.Controls.Add(li);
        }

        private void GenerateDestChildList(ElectricalGroup group, HtmlGenericControl parentElement, string treeType) //, List<Substation> addedSubstations, List<ElectricalGroup> egParents)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("style", "display: none;");
            if (ContainsEg(group, egParents))
            {
               // EpicSubstationManager epicSubstationManager = new EpicSubstationManager();
                IList<Substation> substations = EpicCutPlanes.GetSubstationsbyElectricalGroup(group.Id) as IList<Substation>; //EpicDataAccess.Domain.
                //IList<Substation> substations = EpicMaps.GetSubstationsbyElectricalGroup(group.Id) as IList<Substation>;
                if (substations != null && substations.Count > 0 && parentElement != null)
                {
                    parentElement.Controls.Add(ul);
                    foreach (Substation s in substations) //EpicDataAccess.Domain.
                    {
                        //if (s.ElectricalGroupId != null &&
                         if (ContainsSubstation(s.Id , addedSubstations))
                            GenerateSubstationElement(s, group, ul);
                    }
                }
            }
            if (group.ChildGroups != null && group.ChildGroups.Count > 0 && parentElement != null && destinationEGCount < egParents.Count)
            {
                parentElement.Controls.Add(ul);

                foreach (ElectricalGroup cg in group.ChildGroups)
                {
                    if (ContainsEg(cg, egParents))
                    {
                        GenerateElement(cg, ul, treeType);
                        destinationEGCount++;
                        GenerateDestChildList(cg, ul.Controls[ul.Controls.Count - 1] as HtmlGenericControl, treeType); //, addedSubstations, egParents);
                    }
                    else
                    {
                        GenerateDestChildList(cg, parentElement, treeType); //, addedSubstations, egParents);
                    }
                }
            }
        }


        private void GenerateChildList(ElectricalGroup group, HtmlGenericControl parentElement, string treeType) //, List<Substation> addedSubstations, List<ElectricalGroup> egParents)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("style", "display: none;");
            if (ContainsEg(group, egParents))
            {
                //EpicSubstationManager epicSubstationManager = new EpicSubstationManager();  //EpicDataAccess.Domain.
                IList<Substation> substations = EpicCutPlanes.GetSubstationsbyElectricalGroup(group.Id) as IList<Substation>; //EpicDataAccess.Domain.
                if (substations != null && substations.Count > 0 && parentElement != null)
                {
                    parentElement.Controls.Add(ul);
                    foreach (Substation s in substations) //EpicDataAccess.Domain.
                    {
                        //if (s.ElectricalGroupId != null &&
                        if (!ContainsSubstation(s.Id, addedSubstations))
                            GenerateSubstationElement(s, group, ul);
                    }
                }
            }
            if (group.ChildGroups != null && group.ChildGroups.Count > 0 && parentElement != null)
            {
                parentElement.Controls.Add(ul);
                foreach (ElectricalGroup cg in group.ChildGroups)
                {
                    if (ContainsEg(cg, egParents))
                    {
                        GenerateElement(cg, ul, treeType);
                        GenerateChildList(cg, ul.Controls[ul.Controls.Count - 1] as HtmlGenericControl, treeType); //, addedSubstations, egParents);
                    }
                    else
                    {
                        GenerateChildList(cg, parentElement, treeType); 
                    }
                }
            }
        }

        /// <summary>
        /// Save Cut Plane object by ID.
        /// </summary>
        /// <returns>void</returns>
        [WebMethod]
        public int SaveCutPlaneDetails(int cutPlaneId, string cutPlaneName, string cutPlaneDescription, string cutPlaneType, int mapId)
        {
            try
            {
                int cpid = 0;
                //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
                //EpicCutPlanes epicCutPlanes = new EpicCutPlanes(ConnectionString);
              //  EpicMembershipUser user = EpicMembership.GetUser(Context.User.Identity.Name);
                //Log.Debug("SaveCutPlaneDetails: Saving record for cutplane " + cutplaneId);
                CutPlane cutplane = new CutPlane();
                cutplane.Id = cutPlaneId; //.ToInt32();
                cutplane.DateCreated = DateTime.Now;
                cutplane.Name = cutPlaneName;
                cutplane.Description = cutPlaneDescription;
                cutplane.TypeId = EpicCutPlanes.GetCutPlaneTypeId(cutPlaneType); //.SelectedValue.ToInt32();
                cutplane.MapId = mapId;
                cutplane.UserCreatedId = 1; //  user.EpicUserId;
                //cutplane.UserUpdatedId =  user.EpicUserId;
                cpid = EpicCutPlanes.SaveCutPlanebyID(cutplane);
                    //cpid = cutplane.Id;
                //Log.Debug("CutPlaneManagement::SaveCutPlaneDetails: Save successful for cutplane " + cpid);
                return cpid;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::SaveCutPlaneDetails: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Gets Cut Plane object by ID.
        /// </summary>
        /// <returns>CutPlane object.</returns>
        [WebMethod]
        public CutPlane GetCutPlane(int cutPlaneID)
        {
            try
            {
                CutPlane cutplane = EpicCutPlanes.GetCutPlane(cutPlaneID);
                CutPlane serializablecutplane = new CutPlane();
                serializablecutplane.Id = cutplane.Id;
                serializablecutplane.DateCreated = cutplane.DateCreated;
                serializablecutplane.DateUpdated = cutplane.DateUpdated;
                serializablecutplane.Description = cutplane.Description;
                serializablecutplane.Name = cutplane.Name;
                serializablecutplane.TypeId = cutplane.TypeId;
                serializablecutplane.MapId = cutplane.MapId;
                serializablecutplane.UserCreatedId = cutplane.UserCreatedId;
                serializablecutplane.UserUpdatedId = cutplane.UserUpdatedId;
                return serializablecutplane;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetCutPlane: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Gets Cut Plane type object by ID.
        /// </summary>
        /// <returns>CutPlaneType object.</returns>
        [WebMethod]
        public CutPlaneType GetCutPlaneType(int cutPlaneTypeID)
        {
            try
            {
                CutPlaneType cutplanetype = EpicCutPlanes.GetCutPlaneTypeById(cutPlaneTypeID);
                CutPlaneType serializablecutplanetype = new CutPlaneType();
                serializablecutplanetype.DateCreated = cutplanetype.DateCreated;
                serializablecutplanetype.Id = cutplanetype.Id;
                serializablecutplanetype.Name = cutplanetype.Name;
                serializablecutplanetype.UserCreatedId = cutplanetype.UserCreatedId;
                serializablecutplanetype.Code = cutplanetype.Code;
                return serializablecutplanetype;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetCutPlaneType: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CutPlane> GetAllCutPlanes()
        {
            try
            {
                List<CutPlane> cutPlanes = EpicCutPlanes.GetCutPlanes();
                List<CutPlane> serializableCutPlanes = new List<CutPlane>();
                foreach (CutPlane cutPlane in cutPlanes)
                {
                    CutPlane serializablecutPlane = new CutPlane();
                    serializablecutPlane.Id = cutPlane.Id;
                    serializablecutPlane.DateCreated = cutPlane.DateCreated;
                    serializablecutPlane.DateUpdated = cutPlane.DateUpdated;
                    serializablecutPlane.Description = cutPlane.Description;
                    serializablecutPlane.Name = cutPlane.Name;
                    serializablecutPlane.TypeId = cutPlane.TypeId;
                    serializablecutPlane.UserCreatedId = cutPlane.UserCreatedId;
                    serializablecutPlane.UserUpdatedId = cutPlane.UserUpdatedId;
                    serializableCutPlanes.Add(serializablecutPlane);
                }
                return serializableCutPlanes;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetAllCutPlanes: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Inserts the input Pool Object into the Db or updates the existing Pool object (based on ID)
        /// </summary>
        /// <param name="substation">Pool Object</param>
        /// <returns>db identity</returns>
        [WebMethod]
        public int SavePool(int cutPlaneId, int poolId, string poolName, string poolType)
        {
            try
            {
                //Log.Debug("CutPlaneManagement::SavePool: Save successful for pool " + poolName);
                return EpicCutPlanes.SavePool(cutPlaneId, poolId, poolName, poolType);
            }
            catch (Exception ex)
            {
               // Log.Error("CutPlaneManagement::SavePool: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        [WebMethod]
        public Pool GetPool(int poolId)
        {
            try
            {
                Pool pool = EpicCutPlanes.GetPool(poolId);
                Pool serializablepool = new Pool();
                serializablepool.Id = pool.Id;
                serializablepool.DateCreated = pool.DateCreated;
                serializablepool.Name = pool.Name;
                serializablepool.CutPlaneId = pool.CutPlaneId;
                serializablepool.PoolTypeId = pool.PoolTypeId;
                serializablepool.UserCreatedId = pool.UserCreatedId;
                return serializablepool;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetPool: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Gets a list of Pool objects.
        /// </summary>
        /// <returns>List<Pool></returns>
        [WebMethod]
        public List<Pool> GetPoolList(int cutPlaneID)
        {
            try
            {
                List<Pool> pools = EpicCutPlanes.GetPools(cutPlaneID);
                List<Pool> serializablepools = new List<Pool>();
                foreach (Pool pool in pools)
                {
                    //Easiest method is just to create a new Pool object and assign only necessary properties to it.
                    //This will remove any circular references and allow it to be serializable.  
                    //Do we really need to create a new serializable Pool class?
                    Pool serializablepool = new Pool();
                    serializablepool.Id = pool.Id;
                    serializablepool.DateCreated = pool.DateCreated;
                    serializablepool.Name = pool.Name;
                    serializablepool.CutPlaneId = pool.CutPlaneId;
                    serializablepool.PoolTypeId = pool.PoolTypeId;
                    serializablepool.UserCreatedId = pool.UserCreatedId;
                    serializablepools.Add(serializablepool);
                }
                return serializablepools;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::GetPoolList: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Handles Drag and Drop of Tree nodes
        /// </summary>
        /// <param name="substation">Pool Object</param>
        /// <returns>db identity</returns>
        [WebMethod]
        public List<ElectricalGroup> UpdateSubstations(List<string> substList, int poolId, int mapId)
        {
            try
            {
                if (poolId <= 0) return null; // -1;
                addedSubstations = new List<Substation>();
                foreach (string substId in substList)
                {
                    if (substId.Contains("subst_"))
                    {
                        int substationid = substId.Replace("subst_", "").ToInt32();
                        EpicCutPlanes.SaveSubstationPool(substationid, poolId);
                        Substation substation = EpicCutPlanes.GetSubstation(substationid);
                        addedSubstations.Add(substation);
                    }
                }
                //Need to return E.G. parents hierarchy to add E.G.'s to hierarchy
                List<ElectricalGroup> mapEGs = new List<ElectricalGroup>();
                int parentGroupId = EpicCutPlanes.GetParentGroupId(mapId);
                ElectricalGroup root = GetRootElectricalGroup(parentGroupId, true, mapEGs);
                //addedSubstations = EpicCutPlanes.GetPoolSubstations(poolId);
                egParents = GetElectricalGroupParents(addedSubstations, mapEGs); //int mapId,
                List<ElectricalGroup> serializableEGs = new List<ElectricalGroup>();
                foreach (ElectricalGroup eg in egParents)
                {
                    ElectricalGroup serializableEg = new ElectricalGroup();
                    serializableEg.Abbreviation = eg.Abbreviation;
                    serializableEg.DateCreated = eg.DateCreated;
                    serializableEg.DateUpdated = eg.DateUpdated;
                    serializableEg.Id = eg.Id;
                    serializableEg.Name = eg.Name;
                    serializableEg.ParentPk = eg.ParentPk;
                    serializableEg.UserCreatedId = eg.UserCreatedId;
                    serializableEg.UserUpdatedId = eg.UserUpdatedId;
                    serializableEGs.Add(serializableEg);
                }
                return serializableEGs;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::UpdateSubstations: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Remove Substations that were dropped
        /// </summary>
        /// <param name="substation">substList</param>
        /// <param name="substation">poolId</param>
        /// <returns>void</returns>
        [WebMethod]
        public List<ElectricalGroup> RemoveSubstations(List<string> substList, int poolId, int mapId)
        {
            try
            {
                if (poolId <= 0) return null;
                addedSubstations = new List<Substation>();
                foreach (string substId in substList)
                {
                    if (substId.Contains("subst_"))
                    {
                        int substationid = substId.Replace("subst_", "").ToInt32();
                        EpicCutPlanes.DeleteSubstationPool(substationid, poolId);
                        Substation substation = EpicCutPlanes.GetSubstation(substationid);
                        addedSubstations.Add(substation);
                    }
                }
                //Need to return E.G. parents hierarchy to add E.G.'s to hierarchy
                List<ElectricalGroup> mapEGs = new List<ElectricalGroup>();
                int parentGroupId = EpicCutPlanes.GetParentGroupId(mapId);
                ElectricalGroup root = GetRootElectricalGroup(parentGroupId, true, mapEGs);
                //addedSubstations = EpicCutPlanes.GetPoolSubstations(poolId);
                egParents = GetElectricalGroupParents(addedSubstations, mapEGs); //int mapId,
                List<ElectricalGroup> serializableEGs = new List<ElectricalGroup>();
                foreach (ElectricalGroup eg in egParents)
                {
                    ElectricalGroup serializableEg = new ElectricalGroup();
                    serializableEg.Abbreviation = eg.Abbreviation;
                    serializableEg.DateCreated = eg.DateCreated;
                    serializableEg.DateUpdated = eg.DateUpdated;
                    serializableEg.Id = eg.Id;
                    serializableEg.Name = eg.Name;
                    serializableEg.ParentPk = eg.ParentPk;
                    serializableEg.UserCreatedId = eg.UserCreatedId;
                    serializableEg.UserUpdatedId = eg.UserUpdatedId;
                    serializableEGs.Add(serializableEg);
                }
                return serializableEGs;
            }
            catch (Exception ex)
            {
                //Log.Error("CutPlaneManagement::RemoveSubstations: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Get all Resource Plans
        /// </summary>
        /// <returns>list of Resource Plans</returns>
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<EpicDataAccess.CutPlanes.NitsReport> GetAllNitsResourcePlans()
        //{
        //    try
        //    {
        //        List<EpicDataAccess.CutPlanes.NitsReport> resourcePlans = EpicCutPlanes.GetAllResourcePlans();
        //        List<EpicDataAccess.CutPlanes.NitsReport> serializableResourcePlans = new List<EpicDataAccess.CutPlanes.NitsReport>();
        //        foreach (EpicDataAccess.CutPlanes.NitsReport resourcePlan in resourcePlans)
        //        {
        //            EpicDataAccess.CutPlanes.NitsReport serializableResourcePlan = new EpicDataAccess.CutPlanes.NitsReport();
        //            serializableResourcePlan.Id = resourcePlan.Id;
        //            serializableResourcePlan.UserCreatedId = resourcePlan.UserCreatedId;
        //            serializableResourcePlan.DateCreated = resourcePlan.DateCreated;
        //            serializableResourcePlan.DateApproved = resourcePlan.DateApproved;
        //            serializableResourcePlan.ForecastBeginYear = resourcePlan.ForecastBeginYear;
        //            serializableResourcePlan.ForecastEndYear = resourcePlan.ForecastEndYear;
        //            serializableResourcePlan.PlanName = resourcePlan.PlanName;
        //            serializableResourcePlan.DatePublished = resourcePlan.DatePublished;
        //            serializableResourcePlan.UserPublishedId = resourcePlan.UserPublishedId;
        //            serializableResourcePlan.ReportApprovalStatusId = resourcePlan.ReportApprovalStatusId;
        //            serializableResourcePlan.Uri = resourcePlan.Uri;
        //            serializableResourcePlan.UserApprovedId = resourcePlan.UserApprovedId;
        //            serializableResourcePlan.NitsReportTypeId = resourcePlan.NitsReportTypeId;
        //            serializableResourcePlan.NitsReportIndex = resourcePlan.NitsReportIndex;
        //            serializableResourcePlan.PlanDescription = resourcePlan.PlanDescription;
        //            serializableResourcePlans.Add(serializableResourcePlan);
        //        }
        //        return serializableResourcePlans != null ? serializableResourcePlans : null;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Error("CutPlaneManagement::GetAllNitsResourcePlans: Exception" + ex.Message + ex.StackTrace);
        //        throw;
        //    }
        //}


        //************* MAPS:


        /// <summary>
        /// Gets Map object by ID.
        /// </summary>
        /// <returns>Map object.</returns>
        [WebMethod]
        public Map GetMap(int mapId)
        {
            try
            {
                Map map = EpicCutPlanes.GetMap(mapId);
                //return map;

                Map serializablemap = new Map();
                serializablemap.Id = map.Id;
                serializablemap.DateCreated = map.DateCreated;
                serializablemap.DateUpdated = map.DateUpdated;
                serializablemap.Description = map.Description;
                serializablemap.MapDefinitionRootId = map.MapDefinitionRootId;
                serializablemap.Name = map.Name;
                serializablemap.TypeId = map.TypeId;
                serializablemap.UserCreatedId = map.UserCreatedId;
                serializablemap.UserUpatedId = map.UserUpatedId;
                return serializablemap;
            }
            catch (Exception ex)
            {
              //  Log.Error("MapManagement::GetMap: Exception" + ex.Message + ex.StackTrace);
                throw;
            }
        }



    }
}
