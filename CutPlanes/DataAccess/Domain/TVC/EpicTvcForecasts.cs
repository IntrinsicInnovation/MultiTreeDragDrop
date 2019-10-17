using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.TVC
{
    public class EpicTvcForecasts : EpicDomainBase
    {


        public EpicTvcForecasts(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public bool SaveTvcForecast(TransVoltageCustLoadForecast fcst) {

            var fc = this.Context.TransVoltageCustLoadForecasts.FirstOrDefault(x => x.Id == fcst.Id);
            
            if (fc != null)
            {
               
                fc.UserCreated = "system"; // TODO set correctly

                fc.Description = fcst.Description;
                fc.ForecastEndYear = fcst.ForecastEndYear;
                fc.ForecastStartYear = fcst.ForecastStartYear;
                fc.ForecastYear = fcst.ForecastYear;
                fc.Name = fcst.Name;
            }
            else
            {
                fcst.DateCreated = DateTime.Now;
                fcst.UserCreated = "system";
                this.Context.TransVoltageCustLoadForecasts.InsertOnSubmit(fcst);
            }

            this.Context.SubmitChanges();

            return true;
        }


        public DimTvcLdTransVoltCustomer SaveCustomer(DimTvcLdTransVoltCustomer dim)
        {

            DimTvcLdTransVoltCustomer existing = null;
            DateTime dt = DateTime.Now;

            if (dim.Id != 0)
            {
                existing = this.Context.DimTvcLdTransVoltCustomers.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = this.Context.DimTvcLdTransVoltCustomers.FirstOrDefault(x => x.Code == dim.Code && x.EndDate == null);
            }

            if (existing != null)
            {
                if (existing.Name == dim.Name &&
                    existing.Code == dim.Code)
                {
                    return existing;
                }
                else
                {
                    existing.EndDate = dt;
                }
            }

            dim.BeginDate = dt;
            dim.DateCreated = dt;
            dim.UserCreated = "system";

            this.Context.DimTvcLdTransVoltCustomers.InsertOnSubmit(dim);

            this.Context.SubmitChanges();

            return dim;
        }

        public DimTvcLdTransVoltCustomer GetCustomer(string Code)
        {
            var c = this.Context.DimTvcLdTransVoltCustomers.FirstOrDefault(x => x.Code == Code);

            return c;
        }



        public bool SaveTimePeriod(DimTvcLdTimePeriod timePeriod) 
        {
            var t = this.Context.DimTvcLdTimePeriods.FirstOrDefault(x => x.Id == timePeriod.Id);

            if (t != null)
            {
                t.Name = timePeriod.Name;
                t.Year = timePeriod.Year;
            }
            else
            {
                //timePeriod.BeginDate = DateTime.Now;
                timePeriod.DateCreated = DateTime.Now;
                timePeriod.UserCreated = "system";
                this.Context.DimTvcLdTimePeriods.InsertOnSubmit(timePeriod);
            }
            this.Context.SubmitChanges();

            return true;
        }

        public DimTvcLdTimePeriod GetTimePeriod(int year)
        {
            var t = this.Context.DimTvcLdTimePeriods.FirstOrDefault(x => x.Year == year);

            return t;
        }

        public bool SaveScenario(DimTvcLdScenario scenario) {
            var s = this.Context.DimTvcLdScenarios.FirstOrDefault(x => x.Id == scenario.Id);

            if (s != null)
            {
                s.Name = scenario.Name;
                s.BeginDate = scenario.BeginDate;
                s.EndDate = scenario.EndDate;
            }
            else
            {
                scenario.BeginDate = DateTime.Now;
                scenario.DateCreated = DateTime.Now;
                scenario.UserCreated = "system";
                this.Context.DimTvcLdScenarios.InsertOnSubmit(scenario);
            }
            this.Context.SubmitChanges();

            return true;
        }

        public DimTvcLdScenario GetScenario(string Name)
        {
            var s = this.Context.DimTvcLdScenarios.FirstOrDefault(x => x.Name == Name);

            return s;
        }



        public bool SaveFacility(DimTvcLdFacility facility)
        {
            DimTvcLdFacility f = null;

            if (facility == null)
            {
                return false;
            }

            if (facility.Id != 0)
            {
                f = Context.DimTvcLdFacilities.FirstOrDefault(x => x.Id == facility.Id);
                f.Name = facility.Name;
                f.Code = facility.Code;
            }
            else
            {
                facility.BeginDate = DateTime.Now;
                facility.DateCreated = DateTime.Now;
                facility.UserCreated = "system"; // TODO get real user ID
                Context.DimTvcLdFacilities.InsertOnSubmit(facility);
            }
            Context.SubmitChanges();

            return true;
        }

        public DimTvcLdFacility GetFacility(string Code)
        {
            var f = Context.DimTvcLdFacilities.FirstOrDefault(x => x.Code == Code);

            return f;
        }

        
        public bool SaveMeasures(TransVoltageCustForecastLoadFact fact) {
            var t = this.Context.TransVoltageCustForecastLoadFacts.FirstOrDefault(x => x.Id == fact.Id);


            return true;
        }

        public bool SaveForecastFact(TransVoltageCustForecastLoadFact fact)
        {
            TransVoltageCustForecastLoadFact f = null;

            if (fact == null)
            {
                return false;
            }

            if (fact.Id != 0)
            {
                f = this.Context.TransVoltageCustForecastLoadFacts.FirstOrDefault(x => x.Id == fact.Id);
            }

            if (fact.Id != 0 && f != null)
            {
                f.LoadMeasure = fact.LoadMeasure;
                f.TimePeriodId = fact.TimePeriodId;
                f.TransmissionVoltageCustomerId = fact.TransmissionVoltageCustomerId;
            }
            else
            {
                this.Context.TransVoltageCustForecastLoadFacts.InsertOnSubmit(fact);
            }
            this.Context.SubmitChanges();


            return true;
        }

        public Dictionary<DimTvcLdScenario, List<TransVoltageCustForecastLoadFact>> GetForecastData(string FacilityCode, int ForecastId)
        {
            List<ForecastData> forecastList = new List<ForecastData>();


            var facility = Context.DimTvcLdFacilities.FirstOrDefault(x => x.Code == FacilityCode && x.EndDate == null);

            if (facility != null)
            {
                var forecasts = Context.TransVoltageCustForecastLoadFacts.
                                Where(x => x.FacilityId == facility.Id && x.TransVoltageCustLdForecastId == ForecastId).
                                GroupBy(x => x.DimTvcLdScenario).ToDictionary(grp => grp.Key, grp => grp.ToList());

                return forecasts;
            }
            return null;
        }

        public List<TransVoltageCustLoadForecast> GetForecasts()
        {
            var f = Context.TransVoltageCustLoadForecasts.OrderBy(x => x.Id);

            return f.ToList();
        }

        public List<TransVoltageCustLoadForecast> GetForecasts(string tvcCode)
        {
            
            var forecasts = (from f in Context.TransVoltageCustLoadForecasts
                             join fact in Context.TransVoltageCustForecastLoadFacts on f.Id equals fact.TransVoltageCustLdForecastId
                             join tvc in Context.DimTvcLdTransVoltCustomers on fact.TransmissionVoltageCustomerId equals tvc.Id
                             where tvc.Code == tvcCode
                             select f).Distinct();

            return forecasts.ToList();
        }

        public class ForecastData
        {
            public string Name { get; set; }
            public List<TransVoltageCustForecastLoadFact> Facts { get; set; }
        }



    }

}
