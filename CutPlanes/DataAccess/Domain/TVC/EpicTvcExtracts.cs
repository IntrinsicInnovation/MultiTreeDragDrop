using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.Domain;


namespace EpicDataAccess.Domain.TVC
{
    public class EpicTvcExtracts : EpicDomainBase
    {
        public EpicTvcExtracts(string connectionString)
        {
            this.ConnectionString = connectionString;
            AutoSubmit = true;
        }

        public EpicTvcExtracts()
        {
            this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EpicDB"].ConnectionString;
            AutoSubmit = true;
        }

        private string UserName = "system";

        public bool AutoSubmit { get; set; }

        /// <summary>
        /// Create or Update the forecast anchor record in the TvcForecasts table
        /// </summary>
        /// <param name="Forecast"></param>
        /// <returns></returns>
        public TvcForecast SaveForecast(TvcForecast Forecast)
        {
            TvcForecast fcst = null;

            if (Forecast != null && Forecast.Id > 0)
            {
                fcst = GetForecast(Forecast.Id);
            }

            if (fcst != null)
            {
                fcst.Description = Forecast.Description;
                fcst.Name = Forecast.Name;
            }
            else
            {
                Forecast.DateCreated = DateTime.Now;
                Forecast.UserCreated = UserName;
                Context.TvcForecasts.InsertOnSubmit(Forecast);
            }
            Context.SubmitChanges();

            return null;
        }

        public void Submit()
        {
            Context.SubmitChanges();
        }


        /// <summary>
        /// Get a particular TvcForecast record identified by the PK
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TvcForecast GetForecast(int Id)
        {
            var fcst = Context.TvcForecasts.FirstOrDefault(x => x.Id == Id);

            return fcst;
        }


        public List<TvcForecastFact> GetFacts(int forecastId)
        {
            var facts = Context.TvcForecastFacts.Where(x => x.TvcForecastId == forecastId);

            return facts.ToList();
        }


        /// <summary>
        /// Save a fact record - note facts are immutable
        /// </summary>
        /// <param name="fact"></param>
        /// <returns>0 on failure, or attempt to overwrite, ID on success</returns>
        public int SaveFact(TvcForecastFact fact)
        {
            var f = Context.TvcForecastFacts.FirstOrDefault(x => x.Id == fact.Id);

            if (f != null)
            {
                return 0;
            }

            Context.TvcForecastFacts.InsertOnSubmit(fact);
            Context.SubmitChanges();

            return fact.Id;
        }



        /// <summary>
        /// Create a Transmission Service Request dimension record
        /// </summary>
        /// <param name="dtsr"></param>
        /// <returns></returns>
        public int CreateDimTvcTransServiceRequest(DimTvcTransServiceRequest dim)
        {
            DimTvcTransServiceRequest existing = null;
            DateTime dt = DateTime.Now;

            if (dim.Id != 0)
            {
                existing = Context.DimTvcTransServiceRequests.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = Context.DimTvcTransServiceRequests.FirstOrDefault(x => x.TsrNumber == dim.TsrNumber && x.EndDate == null);
            }


            // if existing
            if (existing != null)
            {
                if (dim.Description == existing.Description &&
                    dim.Assumption == existing.Assumption &&
                    dim.PostingDate == existing.PostingDate &&
                    dim.TsrNumber == existing.TsrNumber &&
                    dim.TsrType == existing.TsrType)
                {
                    // nothing has changed, exit
                    return existing.Id;
                }
                else
                {
                    // there are changes, close old record off
                    existing.EndDate = dt;
                }
            } 

            dim.DateCreated = dt;
            dim.BeginDate = dt;
            dim.UserCreated = UserName;
            Context.DimTvcTransServiceRequests.InsertOnSubmit(dim);

            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }


        /// <summary>
        /// Save or update a DimTvcSubstation record.
        /// 
        /// Because dimensions are immutable, if there are changes we need to create a new dimension record
        /// </summary>
        /// <param name="dim"></param>
        /// <returns></returns>
        public int SaveDimTvcSubstation(DimTvcSubstation dim)
        {
            DimTvcSubstation existing = null;
            DateTime dt = DateTime.Now;

            // get existing record if any
            if (dim.Id != 0)
            {
                existing = Context.DimTvcSubstations.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = Context.DimTvcSubstations.FirstOrDefault(x => x.Code == dim.Code && x.EndDate == null);
            }

            if (existing != null)  // we found a dimension record for this, UPDATE
            {
                // check for modifications
                if (existing.Code == dim.Code &&
                    existing.Name == dim.Name &&
                    existing.AccountNumber == dim.AccountNumber &&
                    existing.Latitude == dim.Latitude &&
                    existing.Longitude == dim.Longitude)
                {
                    // nothing has changed - exit
                    return existing.Id;
                }
                else  // something has changed, need to close off old record.
                {
                    existing.EndDate = dt;
                }
            }

            dim.DateCreated = dt;
            dim.BeginDate = dt;
            dim.UserCreated = UserName;
            Context.DimTvcSubstations.InsertOnSubmit(dim);

            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }


        public int CreateDimTvcScenario(DimTvcScenario dim)
        {
            DimTvcScenario existing = null;
            DateTime dt = DateTime.Now;
            
            if (dim.Id != 0)  // get the existing record if any
            {
                existing = Context.DimTvcScenarios.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = Context.DimTvcScenarios.FirstOrDefault(x => x.Name == dim.Name && x.EndDate == null);
            }
            
            if (existing != null)
            {
                if (existing.Name == dim.Name)
                {
                    return existing.Id;     // nothing has changed
                }
                else
                {
                    // close off old dimension
                    existing.EndDate = dt;               
                }
            }

            dim.DateCreated = dt;
            dim.BeginDate = dt;
            dim.UserCreated = UserName;
            Context.DimTvcScenarios.InsertOnSubmit(dim);
          
            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }
        
        // Note: TimePeriod doesn't have BeginDate/EndDate fields so the logic is a bit different
        public int CreateDimTvcTimePeriod(DimTvcTimePeriod dim)
        {
            DateTime dt = DateTime.Now;
            DimTvcTimePeriod existing = null;
  
            if (dim.Id != 0)
            {
                existing = Context.DimTvcTimePeriods.FirstOrDefault(x => x.Id == dim.Id);
            } 
            else
            {   
                existing = Context.DimTvcTimePeriods.FirstOrDefault(x => x.Name == dim.Name);
            }

            
            if (existing != null)  // it exists but has different values
            {
                if (existing.Name == dim.Name && existing.Year == dim.Year)
                {
                    return existing.Id;
                }
                else
                {
                    existing.Name = dim.Name;
                    existing.Year = dim.Year;
                }
            }
            else  // it doesn't exist, create a new record.
            {
                dim.DateCreated = DateTime.Now;
                dim.UserCreated = UserName;
                Context.DimTvcTimePeriods.InsertOnSubmit(dim);
            }
            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }


        public int CreateDimTvcKiloVolt(DimTvcKiloVolt dim)
        {
            DimTvcKiloVolt existing = null;
            DateTime dt = DateTime.Now;
            
            if (dim.Id != 0)
            {
                existing = Context.DimTvcKiloVolts.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = Context.DimTvcKiloVolts.FirstOrDefault(x => x.Name == dim.Name);
            }
            
            if (existing != null)
            {
                // check for modifications

                if (existing.KiloVolt == dim.KiloVolt && existing.Name == dim.Name)
                    return existing.Id;   // no changes, exit
                else
                    existing.EndDate = dt; // close off existing record
            }
            
            dim.DateCreated = dt;
            dim.UserCreated = UserName;
            dim.BeginDate = dt;

            dim.KiloVolt = dim.KiloVolt;
            dim.Name = dim.Name;
            
            Context.DimTvcKiloVolts.InsertOnSubmit(dim);
            
            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }


        public int CreateDimTvcStatus(DimTvcStatuse dim)
        {
            DimTvcStatuse existing = null;
            DateTime dt = DateTime.Now;

            if (dim.Id != 0)
            {
                existing = Context.DimTvcStatuses.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = Context.DimTvcStatuses.FirstOrDefault(x => x.Name == dim.Name && x.EndDate == null);
                if (existing != null) {
                    // there's only one field so no need to update if field == search criteria
                    return existing.Id;
                }
            }
            
            if (existing != null)
            {
                if (existing.Name == dim.Name)
                {
                    return existing.Id;  // no change, exit
                }
                existing.EndDate = dt;
            }

            dim.DateCreated = dt;
            dim.UserCreated = UserName;
            dim.BeginDate = dt;
            Context.DimTvcStatuses.InsertOnSubmit(dim);

            if (AutoSubmit) Context.SubmitChanges();

            return dim.Id;
        }


        public int CreateDimTvcRate(DimTvcRate dim)
        {
            DimTvcRate existing = null;
            DateTime dt = DateTime.Now;

            if (dim.Id != 0)
            {
                existing = Context.DimTvcRates.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = Context.DimTvcRates.FirstOrDefault(x => x.Name == dim.Name && x.EndDate == null);
            }

            
            if (existing != null)
            {
                if (existing.Name == dim.Name && existing.Rate == dim.Rate)
                {
                    return existing.Id;  // no change exit
                } 

                existing.EndDate = dt;
            }

            dim.DateCreated = dt;
            dim.UserCreated = UserName;
            dim.BeginDate = dt;
            Context.DimTvcRates.InsertOnSubmit(dim);

            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }

        public int CreateDimTvcLine(DimTvcLine dim)
        {
            DimTvcLine existing = null;
            DateTime dt = DateTime.Now;
            
            if (dim.Id != 0)
            {
                existing = Context.DimTvcLines.FirstOrDefault(x => x.Id == dim.Id);
            } else {
                existing = Context.DimTvcLines.FirstOrDefault(x => x.Name == dim.Name && x.EndDate == null);
            }

            if (existing != null)
            {
                if (existing.LineNumber == dim.LineNumber && existing.Name == dim.Name)
                {
                    // no change, exit
                    return existing.Id;
                }
                existing.EndDate = dt;
            }

            dim.DateCreated = dt;
            dim.BeginDate = dt;
            dim.UserCreated = UserName;
            Context.DimTvcLines.InsertOnSubmit(dim);
 
            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }


        public int CreateDimTvcStage(DimTvcStage dim)
        {
            DateTime dt = DateTime.Now;
            DimTvcStage existing = null;

            if (dim.Id != 0)
            {
                existing = Context.DimTvcStages.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = Context.DimTvcStages.FirstOrDefault(x => x.Name == dim.Name);
            }

            if (existing != null)
            {
                if (existing.Name == dim.Name)
                {
                    return existing.Id;
                }
                existing.EndDate = dt;
            }
            else
            {
                dim.DateCreated = dt;
                dim.UserCreated = UserName;
                dim.EndDate = dt;
                Context.DimTvcStages.InsertOnSubmit(dim);
            }

            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }

        public int CreateDimTvcCustomer(DimTvcCustomer dim)
        {
            DateTime dt = DateTime.Now;
            DimTvcCustomer existing = null;

            if (dim.Id != 0)
            {
                existing = Context.DimTvcCustomers.FirstOrDefault(x => x.Id == dim.Id);
            }
            else
            {
                existing = Context.DimTvcCustomers.FirstOrDefault(x => x.Name == dim.Name);
            }


            if (existing != null)
            {
                if (existing.Code == dim.Code && existing.Name == dim.Name)
                {
                    return existing.Id;  // no changes, return ID
                }
                existing.EndDate = dt;
            }

            dim.DateCreated = dt;
            dim.UserCreated = UserName;
            dim.BeginDate = dt;
            Context.DimTvcCustomers.InsertOnSubmit(dim);
  
            if (AutoSubmit) Context.SubmitChanges();
            return dim.Id;
        }





    }
}
