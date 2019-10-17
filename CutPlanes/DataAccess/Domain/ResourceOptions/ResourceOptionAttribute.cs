using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace EpicDataAccess.Domain.ResourceOptions
{
    public partial class ResourceOptionAttribute
    {
        private ILog _log = null;
        private ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(typeof(ResourceOptionAttribute));
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _log;
            }
        }


        private decimal? _uEC = null;
        /// <summary>
        /// Unit Energy Cost --- 
        /// If Scenario Id is set the UEC measure for that scnenario will be returned, otherwise the POI UEC (total) will be returned
        /// </summary>
        public decimal UEC
        {
            get
            {
                if (!_uEC.HasValue)
                {   
                    var uecFinIndicators = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label.Contains("Unit Energy Cost"));
                    if (uecFinIndicators.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.UEC --> No UEC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _uEC = 0;
                    }
                    else
                    {
                        if (this.FinancialIndicatorPredicate != null)
                        {
                            var scenarioUEC = uecFinIndicators.FirstOrDefault(this.FinancialIndicatorPredicate);
                            _uEC = scenarioUEC != null ? scenarioUEC.Measure : 0;
                        }
                        else
                            _uEC = uecFinIndicators.Sum(efi => efi.Measure);
                    }
                }
                return _uEC.Value;
            }
        }

        private decimal? _uCC = null;
        /// <summary>
        /// Unit Capacity Cost --- 
        /// If Scenario Id is set the UCC measure for that scnenario will be returned, otherwise the POI UCC (total) will be returned
        /// </summary>
        public decimal UCC
        {
            get
            {
                if (!_uCC.HasValue)
                {
                    var uccFinIndicators = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label.Contains("Unit Energy Cost"));
                    if (uccFinIndicators.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.UCC --> No UCC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _uCC = 0;
                    }
                    else
                    {
                        if (this.FinancialIndicatorPredicate != null)
                        {
                            var scenarioUCC = uccFinIndicators.FirstOrDefault(this.FinancialIndicatorPredicate);
                            _uCC = scenarioUCC != null ? scenarioUCC.Measure : 0;
                        }
                        else
                            _uCC = uccFinIndicators.Sum(efi => efi.Measure);
                    }
                }
                return _uCC.Value;
            }
        }

        private decimal? _aCC = null;
        /// <summary>
        /// Annualized Capital Cost --- 
        /// If Scenario Id is set the ACC measure for that scnenario will be returned, otherwise the POI ACC (total) will be returned
        /// </summary>
        public decimal ACC
        {
            get
            {
                if (!_aCC.HasValue)
                {
                    var accFinIndicators = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label.Contains("Annualized Capital Cost"));
                    if (accFinIndicators.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.ACC --> No ACC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _aCC = 0;
                    }
                    else
                    {
                        if (this.FinancialIndicatorPredicate != null)
                        {
                            var scenarioACC = accFinIndicators.FirstOrDefault(this.FinancialIndicatorPredicate);
                            _aCC = scenarioACC != null ? scenarioACC.Measure : 0;
                        }                        
                        //else if (this.ScenarioId.HasValue)
                        //{
                        //    var scenarioACC = accFinIndicators.FirstOrDefault(efi => efi.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                        //    _aCC = scenarioACC != null ? scenarioACC.Measure : 0;
                        //}
                        else
                            _aCC = accFinIndicators.Sum(efi => efi.Measure);
                    }
                }
                return _aCC.Value;
            }
        }

        private decimal? _dGC = null;
        /// <summary>
        /// Average Annual Energy for all scenarios
        /// </summary>
        public decimal DGC
        {
            get
            {
                if (!_dGC.HasValue)
                {
                    var techIndicators = this.TechnicalIndicators.Where(eti => eti.TechnicalIndicatorType.Label == "Dependable Generating Capacity");
                    if (techIndicators.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.DGC --> No DGC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _dGC = 0;
                    }
                    else
                        _dGC = techIndicators.Sum(fi => fi.Measure);
                }
                return _dGC.Value;
            }
        }

        private decimal? _aAE = null;
        /// <summary>
        /// Average Annual Energy for all scenarios
        /// </summary>
        public decimal AAE
        {
            get
            {
                if (!_aAE.HasValue)
                {
                    var techIndicators = this.TechnicalIndicators.Where(eti => eti.TechnicalIndicatorType.Label == "Average Annual Energy");
                    if (techIndicators.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.AAE --> No AAE values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _aAE = 0;
                    }
                    else
                        _aAE = techIndicators.Sum(fi => fi.Measure);
                }
                return _aAE.Value;
            }
        }

        private decimal? _aFE = null;
        /// <summary>
        /// Average Annual Energy for all scenarios
        /// </summary>
        public decimal AFE
        {
            get
            {
                if (!_aFE.HasValue)
                {
                    var techIndicators = this.TechnicalIndicators.Where(eti => eti.TechnicalIndicatorType.Label == "Annual Firm Energy");
                    if (techIndicators.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.AFE --> No AFE values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _aFE = 0;
                    }
                    else
                        _aFE = techIndicators.Sum(fi => fi.Measure);
                }
                return _aFE.Value;
            }
        }

        private decimal? _mPO = null;
        /// <summary>
        /// Max Power Output / Installed Capacity (Unit of measure = MW) --- 
        /// If Scenario Id is set the MPO measure for that scnenario will be returned, otherwise the POI MPO (total) will be returned
        /// </summary>
        public decimal MPO
        {
            get
            {
                if (!_mPO.HasValue)
                {
                    var mpoTechInds = this.TechnicalIndicators.Where(eti => eti.TechnicalIndicatorType.Label.Contains("Maximum Power Output"));
                    if (mpoTechInds.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.MPO --> No AAE values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _mPO = 0;
                    }
                    else   // technical indicators only have 1 scenario                 
                       _mPO = mpoTechInds.Sum(eti => eti.Measure);
                    
                }
                return _mPO.Value;
            }
        }

        private decimal? _fOMA = null;
        /// <summary>
        /// Fixed Operational Maintenace Costs --- 
        /// If Scenario Id is set the FOMA measure for that scnenario will be returned, otherwise the POI FOMA (total) will be returned
        /// </summary>
        public decimal FOMA
        {
            get
            {
                if (!_fOMA.HasValue)
                {
                    var fomaFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Abbreviation.ToLower() == "foma");
                    if (fomaFinInds == null || fomaFinInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.FOMA --> No FOMA values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _fOMA = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {   
                            var scenarioFoma = fomaFinInds.FirstOrDefault(eti => eti.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _fOMA = scenarioFoma != null ? scenarioFoma.Measure : 0;
                        }
                        else
                            _fOMA = fomaFinInds.Sum(eti => eti.Measure);
                    }
                }
                return _fOMA.Value * 1000;
            }
        }

        private decimal? _vOMA = null;
        /// <summary>
        /// Variable Operational Maintenace Costs --- 
        /// If Scenario Id is set the VOMA measure for that scnenario will be returned, otherwise the POI FOMA (total) will be returned
        /// 
        /// </summary>
        public decimal VOMA
        {
            get
            {
                if (!_vOMA.HasValue)
                {
                    var vomaFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Abbreviation.ToLower() == "voma");
                    if (vomaFinInds == null || vomaFinInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.VOMA --> No VOMA values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _vOMA = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {   
                            var scenarioVoma = vomaFinInds.FirstOrDefault(eti => eti.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _vOMA = scenarioVoma != null ? scenarioVoma.Measure : 0;
                        }
                        else
                            _vOMA = vomaFinInds.Sum(eti => eti.Measure);
                    }
                }
                return _vOMA.Value * 1000;
            }
            set { this._vOMA = value; }
        }

        private decimal? _dCC = null;
        /// <summary>
        /// Direct Capital Cost --- 
        /// If Scenario Id is set the DCC measure for that scnenario will be returned, otherwise the POI DCC (total) will be returned
        /// </summary>
        public decimal DCC
        {
            get
            {
                if (!_dCC.HasValue)
                {
                    var dccFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Abbreviation.ToLower() == "dcc");
                    if (dccFinInds == null || dccFinInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.DCC --> No DCC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _dCC = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {
                            var scenarioDcc = dccFinInds.FirstOrDefault(eti => eti.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _dCC = scenarioDcc != null ? scenarioDcc.Measure : 0;
                        }
                        else
                            _dCC = dccFinInds.Sum(eti => eti.Measure);
                    }
                }
                return _dCC.Value;
            }
        }

        private decimal? _gTC = null;
        /// <summary>
        /// Gas Transportation Cost --- 
        /// If Scenario Id is set the  Gas Transportation Cost measure for that scnenario will be returned, otherwise the POI  Gas Transportation Cost (total) will be returned
        /// </summary>
        public decimal GTC
        {
            get
            {
                if (!_gTC.HasValue)
                {
                    var gtcFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label.Contains("Gas Transportation Cost"));
                    if (gtcFinInds == null || gtcFinInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.GTC --> No GTC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _gTC = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {
                            var scenarioGtc = gtcFinInds.FirstOrDefault(eti => eti.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _gTC = scenarioGtc != null ? scenarioGtc.Measure : 0;
                        }
                        else
                            _gTC = gtcFinInds.Sum(eti => eti.Measure);
                    }
                }
                return _gTC.Value;
            }
        }

        private decimal? _cCC = null;
        /// <summary>
        /// Construction Capital Cost --- 
        /// If Scenario Id is set the  Construction Capital Cost measure for that scnenario will be returned, otherwise the POI Construction Capital Cost (total) will be returned
        /// </summary>
        public decimal CCC
        {
            get
            {
                if (!_cCC.HasValue)
                {
                    var cccFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label == "Construction Cost");
                    if (cccFinInds == null || cccFinInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.CCC --> No CCC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _cCC = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {
                            var scenarioGtc = cccFinInds.FirstOrDefault(eti => eti.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _cCC = scenarioGtc != null ? scenarioGtc.Measure : 0;
                        }
                        else
                            _cCC = cccFinInds.Sum(eti => eti.Measure);
                    }
                }
                return _cCC.Value;
            }
        }

        private decimal? _aHR = null;
        /// <summary>
        /// Average Heat Rate --- 
        /// If Scenario Id is set the Average Heat Rate measure for that scnenario will be returned, otherwise the POI Average Heat Rate (total) will be returned
        /// </summary>
        public decimal AHR
        {
            get
            {
                if (!_aHR.HasValue)
                {
                    var ahrTechInds = this.TechnicalIndicators.Where(eti => eti.TechnicalIndicatorType.Label == "Average Heat Rate");
                    if (ahrTechInds == null || ahrTechInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.AHR --> No AHR values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _aHR = 0;
                    }
                    else // technical indicators only have 1 scenario                   
                       _aHR = ahrTechInds.Sum(eti => eti.Measure);
                    
                }
                return _aHR.Value;
            }
        }

        private decimal? _oC = null;
        /// <summary>
        /// Operational Capture 
        /// If Scenario Id is set the Operational Capture measure for that scnenario will be returned, otherwise the POI Operational Capture (total) will be returned
        /// Operational Capture is only applicable to biogas
        /// </summary>
        public decimal? OC
        {
            get
            {
                if (this.ResourceOption.FuelType.FuelClass != "Biogas") return null;
                if (!_oC.HasValue)
                {
                    var ocFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label == "Operational Capture");
                    if (ocFinInds == null || ocFinInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.OC --> No OC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _oC = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {
                            var scenarioOc = ocFinInds.FirstOrDefault(efi => efi.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _oC = scenarioOc != null ? scenarioOc.Measure : 0;
                        }
                        else
                            _oC = ocFinInds.Sum(efi => efi.Measure);
                    }
                }
                return _oC.Value;
            }
        }

        private decimal? _oE = null;
        /// <summary>
        /// Operational Electrical
        /// If Scenario Id is set the Operational Electrical measure for that scnenario will be returned, otherwise the POI Operational Electrical (total) will be returned
        /// Operational Electrical  is only applicable to biogas
        /// </summary>
        public decimal? OE
        {
            get
            {
                if (this.ResourceOption.FuelType.FuelClass != "Biogas") return null;
                if (!_oE.HasValue)
                {
                    var oeFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label == "Operational Electrical");
                    if (oeFinInds == null || oeFinInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.OE --> No OE values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _oE = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {
                            var scenarioOe = oeFinInds.FirstOrDefault(efi => efi.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _oE = scenarioOe != null ? scenarioOe.Measure : 0;
                        }
                        else
                            _oE = oeFinInds.Sum(efi => efi.Measure);
                    }
                }
                return _oE.Value;
            }
        }

        private decimal? _cC = null;
        /// <summary>
        /// Capital Capture 
        /// If Scenario Id is set the Capital Capture measure for that scnenario will be returned, otherwise the POI Capital Capture (total) will be returned
        /// Capital Capture  is only applicable to biogas
        /// </summary>
        public decimal? CC
        {
            get
            {
                if (this.ResourceOption.FuelType.FuelClass != "Biogas") return null;
                if (!_cC.HasValue)
                {
                    var ccFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label == "Capital Capture");
                    if (ccFinInds == null || ccFinInds.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.CC --> No CC values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _cC = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {
                            var scenarioCc = ccFinInds.FirstOrDefault(efi => efi.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _cC = scenarioCc != null ? scenarioCc.Measure : 0;
                        }
                        else
                            _cC = ccFinInds.Sum(efi => efi.Measure);
                    }
                }
                return _cC.Value;
            }
        }

        private decimal? _cE = null;
        /// <summary>
        /// Capital Electrical 
        /// If Scenario Id is set the Capital Electrical measure for that scnenario will be returned, otherwise the POI Capital Electrical (total) will be returned
        /// Capital Electrical  is only applicable to biogas
        /// </summary>
        public decimal? CE
        {
            get
            {
                if (this.ResourceOption.FuelType.FuelClass != "Biogas") return null;
                
                if (!_cE.HasValue)
                {
                    var ceFinInds = this.FinancialIndicators.Where(efi => efi.FinancialIndicatorType.Label == "Capital Electrical");
                    if (ceFinInds == null || ceFinInds.Count() == 0)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.CE --> No CE values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        _cE = 0;
                    }
                    else
                    {
                        if (this.ScenarioId.HasValue)
                        {                            
                            var scenarioCe = ceFinInds.FirstOrDefault(efi => efi.FinancialIndicatorScenarioId == this.ScenarioId.Value);
                            _cE = scenarioCe != null ? scenarioCe.Measure : 0;
                        }
                        else
                            _cE = ceFinInds.Sum(efi => efi.Measure);
                    }
                }
                return _cC.Value;
            }
        }

        private IList<KeyValuePair<string, decimal>> _mAE = null;
        public IList<KeyValuePair<string, decimal>> MonthlyAvgEnergy
        {
            get
            {
                if (_mAE == null)
                {
                    var techInds = this.TechnicalIndicators.Where(eti => eti.TechnicalIndicatorType.Subtype == "Monthly Indicators");
                    if (techInds == null || techInds.Count() < 1)
                    {
                        Log.Error(string.Format("ResourceOptionAttribute.MonthlyAvgEnergy --> No MonthlyAvgEnergy values found for resource option {0} (datasource id = {1})", this.ResourceOption.Name, this.DataSourceId.Value));
                        return null;
                    }
                    _mAE = new List<KeyValuePair<string, decimal>>();
                    foreach(var ti in techInds)                    
                        _mAE.Add(new KeyValuePair<string, decimal>(ti.TechnicalIndicatorType.Abbreviation, ti.Measure));                    
                }
                return _mAE;
            }
        }

        /// <summary>
        /// Indicator Scenario Id
        /// </summary>
        public int? ScenarioId { get; set; }
        /// <summary>
        /// DiscountRateId linked to Indicator
        /// </summary>
        public int? DiscountRateId { get; set; }        

        /// <summary>
        /// Predicate / Selector used in the summing of a given Financial Resource Option Attribute Property
        /// </summary>
        public Func<FinancialIndicator, bool> FinancialIndicatorPredicate { get; set; }

        /// <summary>
        /// Predicate / Selector used in the summing of a given Non-Financial Resource Option Attribute Property
        /// </summary>
        public Func<IIndicator, bool> IndicatorPredicate { get; set; }
    }
}
