using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.Search;
using EpicDataAccess.ProjectManagement;

namespace EpicDataAccess.Domain
{
    public class EpicTableHistory : EpicDomainBase
    {
        #region [Properties]

        #endregion

        #region [Constructors]

        public EpicTableHistory(string connectionString)
        {
            this.ConnectionString = connectionString;
        }


        private EpicProjects _epicProjects = null;
        private EpicProjects EpicProjects
        {
            get
            {
                if (_epicProjects == null) _epicProjects = new EpicProjects(ConnectionString);
                return _epicProjects;
            }
        }

        private EpicSubstations _epicSubstations = null;
        private EpicSubstations EpicSubstations
        {
            get
            {
                if (_epicSubstations == null) _epicSubstations = new EpicSubstations(ConnectionString);
                return _epicSubstations;
            }
        }


        private EpicGeneratingUnits _epicGeneratingUnits = null;
        private EpicGeneratingUnits EpicGeneratingUnits
        {
            get
            {
                if (_epicGeneratingUnits == null) _epicGeneratingUnits = new EpicGeneratingUnits(ConnectionString);
                return _epicGeneratingUnits;
            }
        }


        #endregion

        #region [Methods]

        //public List<Substation> GetSourceSubstations(ElectricalGroup parentGroup, List<Substation> addedSubstations)
        //{

        //    // int parentGroupId = GetParentGroupId(mapId);
        //    // ElectricalGroup parentGroup = GetElectricalParentGroups GetElectricalGroup(parentGroupId);

        //    if (parentGroup != null)
        //    {
        //        //First retrieve list of substations that are in this particular map.
        //        List<Substation> mappedSubstations = new List<Substation>();
        //        getChildSubstations(parentGroup, mappedSubstations);

        //        //Next compare mapped substations to those added into the cutplane,
        //        //and retrieve only those mapped substations that haven't been added to the cutplane.
        //        int[] addedarray = addedSubstations.Select(ads => ads.Id).ToArray();
        //        var substations = mappedSubstations.Where(ms => !addedarray.Contains(ms.Id)).Distinct(); //ms.ElectricalGroupId.HasValue && 
        //        return substations != null ? substations.ToList() : null;
        //    }
        //    else
        //        return null;
        //}

        /// <summary>
        /// Retrieve TableHistory Information based on type and rowId
        /// </summary>
        /// <param name="type">Table Name</param>
        /// <param name="rowId">Row Id</param>
        public IList<TableHistoryResult> GetTableHistory(EpicTableHistoryType type, int rowId)
        {
            if (type == EpicTableHistoryType.Substation_SubstationAttributes)
            {
                var substationattributes = from ar in this.Context.AuditRecords.Where(ar => ar.AssociationTableKey.Equals(rowId)
                    && ar.AssociationTable.Equals("Substation"))
                                           join arf in this.Context.AuditRecordFields on ar.Id equals arf.AuditRecordId
                                           orderby ar.Id

                                           select new TableHistoryResult
                                               {
                                                   Id = ar.Id,
                                                   TableName = ar.EntityTable,
                                                   ChangedRowId = ar.EntityTableKey, // .ChangedRowId,
                                                   UserChangedByLoginName = ar.UserName, //  sa.UserChangedByLoginName,
                                                   ColumnName = arf.MemberName, //  sa.ColumnName,
                                                   NewValue = arf.NewValue, // sa.NewValue,
                                                   OldValue = arf.OldValue, // sa.OldValue,
                                                   ChangedDateTime = ar.AuditDate, // sa.ChangedDateTime,
                                                   IsDeleted = (byte)(ar.Action == 2 ? 1 : 0)   // sa.IsDeleted
                                               };
                return substationattributes != null ? substationattributes.ToList() : null;
            }
            else if (type == EpicTableHistoryType.GeneratingUnits)
            {
                var genunitattributes = from ar in this.Context.AuditRecords.Where(ar => ar.AssociationTableKey.Equals(rowId)
                    && ar.AssociationTable.Equals("GeneratingUnit"))
                                        join arf in this.Context.AuditRecordFields on ar.Id equals arf.AuditRecordId
                                        orderby ar.Id

                                        select new TableHistoryResult
                                            {
                                                Id = ar.Id,
                                                TableName = ar.EntityTable,
                                                ChangedRowId = ar.EntityTableKey,
                                                UserChangedByLoginName = ar.UserName,
                                                ColumnName = arf.MemberName,
                                                NewValue = arf.NewValue,
                                                OldValue = arf.OldValue,
                                                ChangedDateTime = ar.AuditDate,
                                                IsDeleted = (byte)(ar.Action == 2 ? 1 : 0)
                                            };
                return genunitattributes != null ? genunitattributes.ToList() : null;
            }
            else if (type == EpicTableHistoryType.Project)
            {
                string historytype = Enum.GetName(typeof(EpicTableHistoryType), type);
                if (string.IsNullOrEmpty(historytype))
                    return null;

                IList<ProjectManagement.Revision> revisions = EpicProjects.GetProjectRevisions(rowId);
                List<int> revisionlist = revisions.Select(r => r.Id).ToList();
                List<ProjectPhase> projectphases = new List<ProjectPhase>();
                foreach(int revisionId in revisionlist)
                {
                    IList<ProjectPhase> phaselist = EpicProjects.GetPhases(revisionId);
                    if (phaselist != null && phaselist.Count > 0)
                    {
                        projectphases.AddRange(phaselist);
                    }
                }
                List<int> phaseslist = projectphases.Select(r => r.Id).ToList();


                //ATTRIBUTES below
                List<SubstationAttribute> substationattributes = new List<SubstationAttribute>();
                foreach (int phaseId in phaseslist)
                {
                    IList<SubstationAttribute> attrlist = EpicSubstations.GetAttributeList(phaseId);
                    if (attrlist != null && attrlist.Count > 0)
                    {
                        substationattributes.AddRange(attrlist);
                    }
                }
                List<int> substationlist = substationattributes.Select(r => r.SubstationId).Distinct().ToList();

                IList<GeneratorAttribute> generatorattributes = EpicGeneratingUnits.GetGeneratorAttributesInList(phaseslist);
                List<int> generatorlist = generatorattributes.Select(r => r.GeneratingUnitId).Distinct().ToList();

                var auditedentity = (from ar in this.Context.AuditRecords.Where(ar => ar.AssociationTableKey.Equals(rowId)
                                        && ar.AssociationTable.Equals(historytype)
                                        || (revisionlist.Contains(ar.AssociationTableKey.HasValue ? ar.AssociationTableKey.Value : 0)
                                        && ar.AssociationTable.Equals("Revision"))

                                        || (substationlist.Contains(ar.AssociationTableKey.HasValue ? ar.AssociationTableKey.Value : 0)
                                        && ar.AssociationTable.Equals("Substation"))

                                        || (generatorlist.Contains(ar.AssociationTableKey.HasValue ? ar.AssociationTableKey.Value : 0)
                                        && ar.AssociationTable.Equals("GeneratingUnit"))
                                        )
                                     
                                     join arf in this.Context.AuditRecordFields on ar.Id equals arf.AuditRecordId
                                     orderby ar.Id

                                     select new TableHistoryResult
                                     {
                                         Id = ar.Id,
                                         TableName = ar.EntityTable,
                                         ChangedRowId = ar.EntityTableKey,
                                         UserChangedByLoginName = ar.UserName,
                                         ColumnName = arf.MemberName,
                                         NewValue = arf.NewValue,
                                         OldValue = arf.OldValue,
                                         ChangedDateTime = ar.AuditDate,
                                         IsDeleted = (byte)(ar.Action == 2 ? 1 : 0)
                                     });


                return auditedentity != null ? auditedentity.ToList() : null;
               
            }
            else if (type == EpicTableHistoryType.TVC)
            {
                var tvcstagedloadmeasures = from ar in this.Context.AuditRecords.Where(ar => ar.AssociationTableKey.Equals(rowId)
                    && ar.AssociationTable.Equals("TransmissionVoltageCustomer"))
                    join arf in this.Context.AuditRecordFields on ar.Id equals arf.AuditRecordId
                    orderby ar.Id

                        select new TableHistoryResult
                        {
                            Id = ar.Id,
                            TableName = ar.EntityTable,
                            ChangedRowId = ar.EntityTableKey,
                            UserChangedByLoginName = ar.UserName,
                            ColumnName = arf.MemberName,
                            NewValue = arf.NewValue,
                            OldValue = arf.OldValue,
                            ChangedDateTime = ar.AuditDate,
                            IsDeleted = (byte)(ar.Action == 2 ? 1 : 0)
                        };
                return tvcstagedloadmeasures != null ? tvcstagedloadmeasures.ToList() : null;
            }
            else
                return null;
        }

        /// <summary>
        /// Retrieves a TableHistory object based on input identity
        /// </summary>
        /// <param name="id">TableHistory identity</param>
        /// <returns>TableHistory object</returns>
        public TableHistory GetTableHistory(int id)
        {
            //TableHistory myTH = new TableHistory();
            return this.Context.TableHistories.FirstOrDefault(th => th.Id == id);
        }

        #endregion
    }
}
