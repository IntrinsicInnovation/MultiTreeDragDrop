using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Reflection;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public class EpicResourceOptions : IDisposable
    {
        #region [Properties]

        private string ConnectionString { get; set; }

        private EpicResourceOptionDataContext _context = null;
        private EpicResourceOptionDataContext Context
        {
            get
            {
                if (_context == null) _context = new EpicResourceOptionDataContext(ConnectionString);
                return _context;
            }
        }

       

        #endregion

        #region [Constructors]

        public EpicResourceOptions(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        public ResourceOption Get(int id)
        {
            return this.Context.ResourceOptions.FirstOrDefault(ro => ro.Id == id);
        }

        /// <summary>
        /// Returns the ResourceOptionAttribute object linked to the input id and data source id
        /// </summary>
        /// <param name="rOAId">ResourceOptionAttribute Id</param>
        /// <param name="dataSourceId">data Source Id</param>
        /// <returns>ResourceOptionAttribute object</returns>
        public ResourceOptionAttribute GetAttribute(int rOAId, int dataSourceId)
        {
            return this.Context.ResourceOptionAttributes.FirstOrDefault(eroa => eroa.Id == rOAId && eroa.DataSourceId == dataSourceId);
        }

        public IEnumerable<ResourceOptionAttribute> GetAttribute(int resourceBundleAttrId)
        {
            return this.Context.ResourceOptionAttributes.Where(eroa => eroa.ResourceBundleAttributeId == resourceBundleAttrId);
        }

        public IEnumerable<TechnicalIndicator> GetTechnicalIndicators(int resourceBundleAttrId)
        {
            return from roa in this.Context.ResourceOptionAttributes
                      join ti in this.Context.TechnicalIndicators on roa.Id equals ti.ResourceOptionAttributeId
                      join tit in this.Context.TechnicalIndicatorTypes on ti.TechnicalIndicatorTypeId equals tit.Id
                      where roa.ResourceBundleAttributeId.HasValue && roa.ResourceBundleAttributeId.Value == resourceBundleAttrId
                      select ti;
        }

        public IEnumerable<EnvironmentalIndicator> GetEnvironmentalIndicators(int resourceBundleAttrId)
        {
            return from roa in this.Context.ResourceOptionAttributes
                   join ei in this.Context.EnvironmentalIndicators on roa.Id equals ei.ResourceOptionAttributeId
                   join eit in this.Context.EnvironmentalIndicatorTypes on ei.EnvironmentalIndicatorTypeId equals eit.Id
                   where roa.ResourceBundleAttributeId.HasValue && roa.ResourceBundleAttributeId.Value == resourceBundleAttrId
                   select ei;
        }

        public IEnumerable<FinancialIndicator> GetFinancialIndicators(int resourceBundleAttrId)
        {
            return from roa in this.Context.ResourceOptionAttributes
                   join fi in this.Context.FinancialIndicators on roa.Id equals fi.ResourceOptionAttributeId
                   join fit in this.Context.FinancialIndicatorTypes on fi.FinancialIndicatorTypeId equals fit.Id
                   where roa.ResourceBundleAttributeId.HasValue && roa.ResourceBundleAttributeId.Value == resourceBundleAttrId
                   select fi;
        }

        public IEnumerable<FinancialIndicator> GetFinancialIndicators(int resourceBundleAttrId, int discountRateId)
        {
            return from roa in this.Context.ResourceOptionAttributes
                   join fi in this.Context.FinancialIndicators on roa.Id equals fi.ResourceOptionAttributeId
                   join lnk in this.Context.LinkFinIndicatorDiscountRates on fi.Id equals lnk.FinancialIndicatorId
                   join fit in this.Context.FinancialIndicatorTypes on fi.FinancialIndicatorTypeId equals fit.Id
                   where roa.ResourceBundleAttributeId.HasValue && roa.ResourceBundleAttributeId.Value == resourceBundleAttrId && lnk.DiscountRateId == discountRateId
                   select fi;
        }

        public IEnumerable<BcStatsIndicatorType> GetBCStatsIndicatorTypes()
        {
            return this.Context.BcStatsIndicatorTypes;
        }

        public IEnumerable<BcStatsIndicatorModel> GetEconomicStatsModels(int fuelTypeId)
        {
            return this.Context.BcStatsIndicatorModels.Where(eeim => eeim.FuelTypeId == fuelTypeId);            
        }

        public IEnumerable<ResourceOptionAttribute> GetAttributes(int dataSourceId, int fuelTypeId, int discountRateId)
        {
            return  (from ro in this.Context.ResourceOptions                    
                    join roa in this.Context.ResourceOptionAttributes on ro.Id equals roa.ResourceOptionId
                    join fi in this.Context.FinancialIndicators on roa.Id equals fi.ResourceOptionAttributeId
                    join ld in this.Context.LinkFinIndicatorDiscountRates on fi.Id equals ld.FinancialIndicatorId
                    where ro.FuelTypeId == fuelTypeId && roa.DataSourceId == dataSourceId && ld.DiscountRateId == discountRateId
                          && (!roa.SentToSysOptiimizer.HasValue || !roa.SentToSysOptiimizer.Value)
                    select roa).Distinct();
        }

        /// <summary>
        /// Returns a collection of ResourceOptionAttribute connected to the input criteria 
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <param name="fuelTypeId"></param>
        /// <param name="discountRateId"></param>
        /// <param name="blackList">An array of ResourceOption Ids to be exclued from the returned collection</param>
        /// <returns>collection of ResourceOptionAttribute</returns>
        public IEnumerable<ResourceOptionAttribute> GetAttributes(int dataSourceId, int fuelTypeId, int discountRateId, int[] blackList)
        {
            return (from ro in this.Context.ResourceOptions
                    join roa in this.Context.ResourceOptionAttributes on ro.Id equals roa.ResourceOptionId
                    join fi in this.Context.FinancialIndicators on roa.Id equals fi.ResourceOptionAttributeId
                    join ld in this.Context.LinkFinIndicatorDiscountRates on fi.Id equals ld.FinancialIndicatorId
                    where ro.FuelTypeId == fuelTypeId && roa.DataSourceId == dataSourceId && ld.DiscountRateId == discountRateId
                          && !blackList.Contains(ro.Id) && (!roa.SentToSysOptiimizer.HasValue || !roa.SentToSysOptiimizer.Value)
                    select roa).Distinct();
        }

        /// <summary>
        /// Returns a collection of resource bundle Attributes who's Ids are found in the input Array of resource bundle ids
        /// </summary>
        /// <param name="dataSourceId">data Source Id</param>
        /// <returns>collection of resource bundles</returns>
        public IEnumerable<ResourceBundleAttribute> GetBundleAttributes(int dataSourceId)
        {
            return from rba in this.Context.ResourceBundleAttributes
                   join roa in this.Context.ResourceOptionAttributes on rba.Id equals roa.ResourceBundleAttributeId
                   where roa.DataSourceId == dataSourceId
                   select rba;
        }

        /// <summary>
        /// Returns a collection of ResourceOptionAttribute connected to the input data source Id and 
        /// where ResourceOption Id is within the input array
        /// </summary>
        
        /// <param name="rOIds">Array of ResourceOption Ids</param>
        /// <returns>collection of ResourceOptionAttribute</returns>
        public IEnumerable<ResourceOptionAttribute> GetAttributes(int[] rOIds)
        {
            return this.Context.ResourceOptionAttributes.Where(eroa => rOIds.Contains(eroa.ResourceOptionId));
        }

        public IEnumerable<ResourceOption> GetOptions()
        {
            return this.Context.ResourceOptions;
        }

        public IEnumerable<ResourceOption> GetOptions(int? dataSourceId)
        {
            return this.Context.ResourceOptions.Where(ro => ro.ResourceOptionAttributes.Any(roa => roa.DataSourceId == dataSourceId));
        }

        public IEnumerable<ResourceOption> GetUngroupedOptions()
        {
            return this.Context.ResourceOptions.Where(ero => ero.ResourceOptionAttributes.Any(eroa => !eroa.ResourceBundleAttributeId.HasValue));
        }

        public IEnumerable<ResourceOption> GetUngroupedOptions(int? dataSourceId)
        {
            return this.Context.ResourceOptions.Where(ro => ro.ResourceOptionAttributes.Any(eroa => eroa.DataSourceId == dataSourceId && !eroa.ResourceBundleAttributeId.HasValue));
        }

        public IEnumerable<ResourceOption> GetOptions(int resourceBundleAttrId)
        {
            return this.Context.ResourceOptions.Where(ero => ero.ResourceOptionAttributes.Any(eroa => eroa.ResourceBundleAttributeId.HasValue && eroa.ResourceBundleAttributeId.Value == resourceBundleAttrId));
        }

        public FinancialIndicatorType GetFinancialIndicatorType(string label)
        {
            return this.Context.FinancialIndicatorTypes.FirstOrDefault(efi => efi.Label.Contains(label));
        }

        /// <summary>
        /// Returns the maximum POI UEC value for all Resource Option linked to the input discount rate
        /// </summary>
        /// <param name="discountRateId">discount rate id</param>
        /// <returns>maximum POI UEC</returns>
        public decimal GetMaxPOIUEC(int discountRateId)
        {
            var finIndicatorGroups = from roa in this.Context.ResourceOptionAttributes
                                join fi in this.Context.FinancialIndicators on roa.Id equals fi.ResourceOptionAttributeId
                                join lnk in this.Context.LinkFinIndicatorDiscountRates on fi.Id equals lnk.FinancialIndicatorId
                                where lnk.DiscountRateId == discountRateId && fi.FinancialIndicatorType.Label.Contains("Unit Energy Cost")
                                group fi by fi.ResourceOptionAttributeId into g
                                select new { POI_UEC = g.Sum(fi => fi.Measure)};

            if(finIndicatorGroups.Count() == 0)
                return 0;

            return finIndicatorGroups.Max(fig => fig.POI_UEC);
        }

        public DiscountRate GetDiscountRate(decimal rate)
        {
            return this.Context.DiscountRates.FirstOrDefault(dr => dr.DiscountRate1 == rate);
        }
        
        public IEnumerable<DiscountRate> GetDiscountRates()
        {
            return this.Context.DiscountRates;
        }        

        public IEnumerable<DataSource> GetDataSources()
        {
            if (!this.Context.DataSources.Any()) return null;
            return this.Context.DataSources;
        }

        public IEnumerable<ResourceTypeUncertainty> GetResourceTypeUncertainties()
        {
            if (!this.Context.ResourceTypeUncertainties.Any()) return null;
            return this.Context.ResourceTypeUncertainties;
        }

        public IEnumerable<CostUncertainty> GetCostUncertainties()
        {
            if (!this.Context.CostUncertainties.Any()) return null;
            return this.Context.CostUncertainties;
        }

        public IEnumerable<TechnicalIndicatorType> GetTechnicalIndicatorTypes()
        {
            return this.Context.TechnicalIndicatorTypes;
        }

        public IEnumerable<FinancialIndicatorType> GetFinancialIndicatorTypes()
        {
            return this.Context.FinancialIndicatorTypes;
        }

        public IEnumerable<FinancialIndicatorScenario> GetFinancialScenarios()
        {
            return this.Context.FinancialIndicatorScenarios;
        }

        public FuelType GetFuelType(int id)
        {
            return this.Context.FuelTypes.FirstOrDefault(eft => eft.Id == id);
        }

        public IEnumerable<EnvironmentalIndicatorType> GetEnvironmentalIndicatorTypes()
        {
            return this.Context.EnvironmentalIndicatorTypes;
        }

        public IEnumerable<EnvironmentalIndicatorScenario> GetEnvironmentalScenarios()
        {
            return this.Context.EnvironmentalIndicatorScenarios;
        }

        public void CreateGroup(int fuelTypeId, decimal start, decimal end, int discountRateId, int financialIndicatorTypeId)
        {
            var group = from ro in this.Context.ResourceOptions
                        join roa in this.Context.ResourceOptionAttributes on ro.Id equals roa.ResourceOptionId
                        join fi in this.Context.FinancialIndicators on roa.Id equals fi.ResourceOptionAttributeId
                        join lnk in this.Context.LinkFinIndicatorDiscountRates on fi.Id equals lnk.FinancialIndicatorId
                        where ro.FuelTypeId == fuelTypeId && fi.FinancialIndicatorTypeId == financialIndicatorTypeId &&
                              lnk.DiscountRateId == discountRateId && fi.Measure >= start && fi.Measure <= end
                        select ro;
        }

        /// <summary>
        /// Delete resource option and all child elements
        /// </summary>
        /// <param name="id">resource option Id</param>
        public void Delete(int id)
        {
            var rO = this.Context.ResourceOptions.FirstOrDefault(ero => ero.Id == id);
            if (rO == null) return;
            DeleteAttributes(id);
            var maps = this.Context.LinkResourceOptionMapDefinitions.Where(m => m.ResourceOptionId == id);
            if (maps != null && maps.Count() > 0)
                this.Context.LinkResourceOptionMapDefinitions.DeleteAllOnSubmit(maps);
            this.Context.ResourceOptions.DeleteOnSubmit(rO);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Delete all resource optio attributes and all child elements
        /// </summary>
        /// <param name="resourceOptionId">resource option Id</param>
        public void DeleteAttributes(int resourceOptionId)
        {
            DeleteIndicators(resourceOptionId);
            var rOA = this.Context.ResourceOptionAttributes.Where(roa => roa.ResourceOptionId == resourceOptionId);
            this.Context.ResourceOptionAttributes.DeleteAllOnSubmit(rOA);
            this.Context.SubmitChanges();
        }

        /// <summary>
        /// Deletes all indicators linked to the input resource option Id
        /// </summary>
        /// <param name="resourceOptionId">resource option Id</param>
        public void DeleteIndicators(int resourceOptionId)
        {
            var eI = from roa in this.Context.ResourceOptionAttributes
                                join ei in this.Context.EnvironmentalIndicators on roa.Id equals ei.ResourceOptionAttributeId
                                where roa.ResourceOptionId == resourceOptionId
                                select ei;
            if (eI != null && eI.Count() > 0)
                this.Context.EnvironmentalIndicators.DeleteAllOnSubmit(eI);
            var links = from roa in this.Context.ResourceOptionAttributes
                       join fi in this.Context.FinancialIndicators on roa.Id equals fi.ResourceOptionAttributeId
                       join lnk in this.Context.LinkFinIndicatorDiscountRates on fi.Id equals lnk.FinancialIndicatorId
                       where roa.ResourceOptionId == resourceOptionId
                       select lnk;
            if (links != null && links.Count() > 0)
                this.Context.LinkFinIndicatorDiscountRates.DeleteAllOnSubmit(links);
            var fI = from roa in this.Context.ResourceOptionAttributes
                                join fi in this.Context.FinancialIndicators on roa.Id equals fi.ResourceOptionAttributeId
                                where roa.ResourceOptionId == resourceOptionId
                                select fi;
            if (fI != null && fI.Count() > 0)
                this.Context.FinancialIndicators.DeleteAllOnSubmit(fI);
            var tI = from roa in this.Context.ResourceOptionAttributes
                                join ti in this.Context.TechnicalIndicators on roa.Id equals ti.ResourceOptionAttributeId
                                where roa.ResourceOptionId == resourceOptionId
                                select ti;
            if (tI != null && tI.Count() > 0)
                this.Context.TechnicalIndicators.DeleteAllOnSubmit(tI);
            this.Context.SubmitChanges();
        }

        public void SaveChanges()
        {
            this.Context.SubmitChanges();
        }

        public Type GetType(string name)
        {
            return this.GetType().Assembly.GetType(name);
        }

        public void Dispose()
        {
            if (this._context != null) this._context.Dispose();
        }

        #endregion

        
    }
}
