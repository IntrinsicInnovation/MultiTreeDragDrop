using System;
using System.Collections.Generic;
using System.Linq;
using EpicAudit;
using EpicAudit.LinqToSql;


namespace EpicDataAccess.Domain
{

    public partial class EpicDomainDataContext
    {

        static private List<SubstationType> substationTypeTable;
        static private List<OwnershipType> ownershipTypeTable;
        static private List<FuelType> fuelTypeTable;
        static private List<ResourceStatuse> resourceStatusTable;
        static private List<PowerCall> powerCallTable;
        static private List<GeneratorType> genunitTypeTable;
        static private List<TvcFacilityStatuse> tvcfacilitystatusTypeTable;
        static private List<TvcFacilityStage> tvcfacilitystageTable;
        static private List<Customer> customerTable;

        partial void OnCreated()
        {

            substationTypeTable = new List<SubstationType>();
            foreach (SubstationType substationtype in SubstationTypes)
            {
                substationTypeTable.Add(substationtype);
            }

            ownershipTypeTable = new List<OwnershipType>();
            foreach (OwnershipType ownershiptype in OwnershipTypes)
            {
                ownershipTypeTable.Add(ownershiptype);
            }


            fuelTypeTable = new List<FuelType>();
            foreach (FuelType fueltype in FuelTypes)
            {
                fuelTypeTable.Add(fueltype);
            }


            resourceStatusTable = new List<ResourceStatuse>();
            foreach (ResourceStatuse resourcestatuse in ResourceStatuses)
            {
                resourceStatusTable.Add(resourcestatuse);
            }


            powerCallTable = new List<PowerCall>();
            foreach (PowerCall powercall in PowerCalls)
            {
                powerCallTable.Add(powercall);
            }

            genunitTypeTable = new List<GeneratorType>();
            foreach (GeneratorType generatortype in GeneratorTypes)
            {
                genunitTypeTable.Add(generatortype);
            }

            tvcfacilitystatusTypeTable = new List<TvcFacilityStatuse>();
            foreach (TvcFacilityStatuse facilitystatus in TvcFacilityStatuses)
            {
                tvcfacilitystatusTypeTable.Add(facilitystatus);
            }

            tvcfacilitystageTable = new List<TvcFacilityStage>();
            foreach (TvcFacilityStage facilitystage in TvcFacilityStages)
            {
                tvcfacilitystageTable.Add(facilitystage);
            }

            customerTable = new List<Customer>();
            foreach (Customer customer in Customers)
            {
                customerTable.Add(customer);
            }

            Substations.Audit().WithConfiguration<SubstationAuditConfig>();
            GeneratingUnits.Audit().WithConfiguration<GeneratingUnitAuditConfig>();
            TransmissionVoltageCustomers.Audit().WithConfiguration<TVCAuditConfig>();
        }


        private static IAuditableContext GetContext()
        {
            return new EpicDomainDataContext();
        }

    

        public override void SaveAuditedEntity(AuditedEntity auditedEntity)
        {
            
            string userName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            var audit = new AuditRecord
            {
                Action = (byte)auditedEntity.Action,
                AuditDate = DateTime.Now,
                EntityTable = auditedEntity.EntityType.Name,
                EntityTableKey = auditedEntity.EntityKey,
                AssociationTable = auditedEntity.ParentEntityType.Name,
                AssociationTableKey = auditedEntity.ParentKey,
                UserName = userName
            };

            foreach (var modifiedProperty in auditedEntity.ModifiedProperties)
            {
                if (modifiedProperty.DisplayName.Trim() != "Date Updated"
                    && modifiedProperty.DisplayName.Trim() != "User Updated Id")
                {
                    audit.AuditRecordFields.Add(
                        new AuditRecordField
                        {
                            MemberName = modifiedProperty.DisplayName,
                            OldValue = modifiedProperty.OldValue,
                            NewValue = modifiedProperty.NewValue
                        });
                }
            }
            if (audit.AuditRecordFields.Count > 0)
            {
                AuditRecords.InsertOnSubmit(audit);
            }
        }


        public class SubstationAuditConfig : EntityAuditConfiguration<Substation>
        {

            public SubstationAuditConfig()
            {
                
                AuditMany(m => m.SubstationAttributes)
                  .WithForeignKey(m => m.SubstationId);

                AuditProperty(m => m.SubStationTypeId)
                .GetValueFrom(substationtypeId => substationTypeTable
                              .Where(s => s.Id == substationtypeId)
                              .Select(s => s.Value)
                              .SingleOrDefault() ?? string.Empty)
                .WithPropertyName("SubstationType");

                AuditProperty(m => m.OwnershipTypeId)
                .GetValueFrom(ownershiptypeId => ownershipTypeTable
                             .Where(s => s.Id == ownershiptypeId)
                             .Select(s => s.Value)
                             .SingleOrDefault() ?? string.Empty)
                .WithPropertyName("OwnershipType");

                AuditProperty(o => o.FuelTypeId)
                .GetValueFrom(fueltypeId => fuelTypeTable
                              .Where(s => s.Id == fueltypeId)
                              .Select(s => s.Description)  
                              .SingleOrDefault() ?? string.Empty)
                .WithPropertyName("FuelType");

                AuditProperty(r => r.ResourceStatusId)
                .GetValueFrom(resourcestatusId => resourceStatusTable
                              .Where(s => s.Id == resourcestatusId)
                              .Select(s => s.Name) 
                              .SingleOrDefault() ?? string.Empty)
                .WithPropertyName("ResourceStatus");

                AuditProperty(p => p.PowerCallId)
               .GetValueFrom(powercallId => powerCallTable
                            .Where(s => s.Id == powercallId)
                            .Select(s => s.Value)
                            .SingleOrDefault() ?? string.Empty)
               .WithPropertyName("PowerCall");
            }
        }

        public class GeneratingUnitAuditConfig : EntityAuditConfiguration<GeneratingUnit>
        {
            

            public GeneratingUnitAuditConfig()
            {
                AuditProperty(m => m.GeneratorTypeID)
                .GetValueFrom(generatingunittypeId => genunitTypeTable
                              .Where(s => s.Id == generatingunittypeId)
                              .Select(s => s.Value)
                              .SingleOrDefault() ?? string.Empty)
                .WithPropertyName("GeneratorType");


                AuditProperty(o => o.EnergySourceId)
                .GetValueFrom(fueltypeId => fuelTypeTable
                             .Where(s => s.Id == fueltypeId)
                             .Select(s => s.Description)  
                             .SingleOrDefault() ?? string.Empty)
                .WithPropertyName("FuelType");

                AuditProperty(r => r.GeneratorStatusID)
                .GetValueFrom(resourcestatusId => resourceStatusTable
                              .Where(s => s.Id == resourcestatusId)
                              .Select(s => s.Name)
                              .SingleOrDefault() ?? string.Empty)
                .WithPropertyName("ResourceStatus");

                AuditProperty(p => p.PowerCallId)
                .GetValueFrom(powercallId => powerCallTable
                             .Where(s => s.Id == powercallId)
                             .Select(s =>s.Value) 
                             .SingleOrDefault() ?? string.Empty)
                .WithPropertyName("PowerCall");

                AuditMany(m => m.GeneratorAttributes)
                    .WithForeignKey(m => m.GeneratingUnitId);
            }
        }


        public class TVCAuditConfig : EntityAuditConfiguration<TransmissionVoltageCustomer>
        {
            public TVCAuditConfig()
            {
                AuditMany(m => m.TvcStagedLoadMeasures)
                    .WithForeignKey(m => m.TvcId);

                AuditProperty(p => p.StatusId)
                    .GetValueFrom(statusId => tvcfacilitystatusTypeTable
                                  .Where(s => s.Id == statusId)
                                  .Select(s => s.Name)
                                  .SingleOrDefault() ?? string.Empty)
                    .WithPropertyName("Status");

                AuditProperty(p => p.StageId)
                    .GetValueFrom(stageId => tvcfacilitystageTable
                                 .Where(s => s.Id == stageId)
                                 .Select(s => s.Name)
                                 .SingleOrDefault() ?? string.Empty)
                    .WithPropertyName("Stage");

                AuditProperty(p => p.CustomerId)
                    .GetValueFrom(customerId => customerTable
                                  .Where(s => s.Id == customerId)
                                  .Select(s => s.Name)
                                  .SingleOrDefault() ?? string.Empty)
                    .WithPropertyName("Customer");

            }
        }

    }
}
