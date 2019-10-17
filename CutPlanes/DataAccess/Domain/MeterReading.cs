using System;
using System.Collections.Generic;
using System.Linq;
using EpicDataAccess.Domain;
using System.Web.Script.Serialization;

namespace EpicDataAccess.Domain
{
    public partial class MeterReading 
    {
        #region [Properties]

        
        public string IntervalTimeString
        {
            get
            {
                double elapsedSeconds = this.SequenceNumber * this.ReadingInterval;
                return this.ReadingDate.AddSeconds(elapsedSeconds).ToString("hh:mm tt");
            }
        }

        public string ReadingDateString
        {
            get
            {
                return this.ReadingDate.ToString("yyyy-MM-dd");
            }
        }

        public string MegaWattValue
        {
            get
            {
                return Decimal.Divide(this.Value, 1000).ToString("F2");
            }
        }

        #endregion

        #region [Constructors]

        public MeterReading(int channel, DateTime readingDate, decimal value)
        {
            this.ChannelNumber = channel;
            this.ReadingDate = readingDate;
            this.Value = value;
            this.ReadingInterval = 0;
            this.SequenceNumber = 0;
        }

        public MeterReading(int channel, DateTime readingDate, int interval, int sequenceNumber, decimal value)
        {
            this.ChannelNumber = channel;
            this.ReadingDate = readingDate;
            this.Value = value;
            this.ReadingInterval = interval;
            this.SequenceNumber = sequenceNumber;
        }

        /// <summary>
        /// used to remove circular reference (required for serialization)
        /// </summary>
        /// <param name="meterReading"></param>
        public MeterReading(MeterReading meterReading)
        {
            this.ChannelNumber = meterReading.ChannelNumber;
            this.ReadingDate = meterReading.ReadingDate;
            this.Value = meterReading.Value;
            this.ReadingInterval = meterReading.ReadingInterval;
            this.SequenceNumber = meterReading.SequenceNumber;
        }

        #endregion

    }
}
