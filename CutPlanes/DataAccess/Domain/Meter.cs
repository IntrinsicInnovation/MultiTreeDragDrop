using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.Domain;
using System.Web;

namespace EpicDataAccess.Domain
{
    public partial class Meter : IResource
    {
        public string ResourceIdentifer
        {
            get { return this.MeterNumber; }
        }

        public string DateLastModifiedString
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public string TypeString
        {
            get { return typeof(Meter).Name; }
        }

        
        /// <summary>
        /// Url to Navigate to Meter Navigation Page
        /// </summary>
        public string NavigationUrl
        {
            get
            {
                string queryStringParams = HttpContext.Current.Request.Url.Query;
                return string.Format("/Pages/Data/MeterNavigation.aspx{0}", queryStringParams);
            }
        }

        /// <summary>
        /// Returns MeterReadings ordered by ChannelNumber, Readingdate and SequenceNumber
        /// </summary>
        public IList<MeterReading> OrderedMeterReadings
        {
            get
            {
                if (this.MeterReadings != null)
                    return this.MeterReadings.OrderBy(mr => mr.ChannelNumber).ThenBy(mr => mr.ReadingDate).ThenBy(mr => mr.SequenceNumber).ToList();
                else
                    return null;
            }
        }

        /// <summary>
        /// Total of all MeterReadings
        /// </summary>
        public decimal MeterReadingSum { get { return this.MeterReadings.Sum(mr => mr.Value); } }

        public decimal? Voltage
        {
            get
            {
                return null;
            }
        }
        #region [Methods]

        public IList<MeterReading> GetAggregatedMeterReadings(EpicMetersAggregateBy aggregateBy)
        {
            if (this.MeterReadings == null) return null;
            List<MeterReading> meterReadings = new List<MeterReading>();
            switch (aggregateBy)
            {
                // Hour is everything... not relevant
                //case EpicMetersAggregateBy.Hour:
                //    meterReadings.AddRange(this.MeterReadings);
                //    break;
                case EpicMetersAggregateBy.Day:
                    var readings = from mr in this.MeterReadings
                                   group mr by mr.ChannelNumber into cg
                                   select new
                                   {
                                       Channel = cg.Key,
                                       Readings = from r in cg
                                                  group r by r.ReadingDate.Day into mrg
                                                  select new MeterReading(cg.Key, mrg.First().ReadingDate, mrg.Sum(mr => mr.Value))
                                   };
                    foreach (var group in readings)
                        meterReadings.AddRange(group.Readings);

                    break;
                case EpicMetersAggregateBy.Month:
                    readings = from mr in this.MeterReadings
                               group mr by mr.ChannelNumber into cg
                               select new
                               {
                                   Channel = cg.Key,
                                   Readings = from r in cg
                                              group r by r.ReadingDate.Month into mrg
                                              select new MeterReading(cg.Key, mrg.First().ReadingDate, mrg.Sum(mr => mr.Value))
                               };
                    foreach (var group in readings)
                        meterReadings.AddRange(group.Readings);
                    break;
                case EpicMetersAggregateBy.Year:
                    readings = from mr in this.MeterReadings
                               group mr by mr.ChannelNumber into cg
                               select new
                               {
                                   Channel = cg.Key,
                                   Readings = from r in cg
                                              group r by r.ReadingDate.Year into mrg
                                              select new MeterReading(cg.Key, mrg.First().ReadingDate, mrg.Sum(mr => mr.Value))
                               };
                    foreach (var group in readings)
                        meterReadings.AddRange(group.Readings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("AggregateColumn property must be set to aggregate meter readings");
            }
            return meterReadings;
        }

        public IList<MeterReading> GetPeakMeterReadings(EpicMetersAggregateBy aggregateBy)
        {
            if (this.MeterReadings == null) return null;
            List<MeterReading> meterReadings = new List<MeterReading>();            
            switch (aggregateBy)
            {
                case EpicMetersAggregateBy.Day:
                    var readings = from mr in this.MeterReadings
                                   group mr by mr.ChannelNumber into cg
                                   select new
                                   {
                                       Channel = cg.Key,
                                       Readings = from r in cg
                                                  group r by r.ReadingDate.Day into mrg
                                                  select new MeterReading(mrg.OrderByDescending(mr => mr.Value).First())
                                                  
                                   };
                    foreach (var group in readings)
                        meterReadings.AddRange(group.Readings);
                    break;
                case EpicMetersAggregateBy.Month:
                    readings = from mr in this.MeterReadings
                               group mr by mr.ChannelNumber into cg
                               select new
                               {
                                   Channel = cg.Key,
                                   Readings = from r in cg
                                              group r by r.ReadingDate.Month into mrg
                                              //select new MeterReading(cg.Key, mrg.First().ReadingDate, mrg.Max(mr => mr.Value))
                                              select new MeterReading(mrg.OrderByDescending(mr => mr.Value).First())
                               };
                    foreach (var group in readings)
                        meterReadings.AddRange(group.Readings);
                    break;
                case EpicMetersAggregateBy.Year:
                    readings = from mr in this.MeterReadings
                               group mr by mr.ChannelNumber into cg
                               select new
                               {
                                   Channel = cg.Key,
                                   Readings = from r in cg
                                              group r by r.ReadingDate.Year into mrg
                                              //select new MeterReading(cg.Key, mrg.First().ReadingDate, mrg.Max(mr => mr.Value))
                                              select new MeterReading(mrg.OrderByDescending(mr => mr.Value).First())
                               };
                    foreach (var group in readings)
                        meterReadings.AddRange(group.Readings);
                    break;                    
            }

            return meterReadings;
        }

        #endregion
    }
}
