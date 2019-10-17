using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicMeters : EpicDomainBase
    {
        #region [Constructors]
        public EpicMeters(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        #endregion



        #region [Methods]
        public IList<Note> GetNotes(int meterId)
        {
            var notes = from ln in this.Context.LinkNotes.Where(link => link.MeterId == meterId)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        where n.IsDeleted == false
                        select n;
            return notes != null ? notes.ToList() : null;
        }


        public MeterReading GetPeakReading(int meterId)
        {
            
            var readings = from m in this.Context.MeterReadings
                           where m.MeterId == meterId
                              select m;

            decimal max = 0;
            MeterReading maxReading = null;
            
            // TODO MTM there's probably a slick linq iterator to handle this....
            foreach (MeterReading r in readings)
            {
                if (r.Value > max)
                {
                    max = r.Value;
                    maxReading = r;
                }
            }

            return maxReading;
        }

        public decimal GetAverageGeneration(int meterId)
        {
            var meter = this.Context.Meters.FirstOrDefault(m => m.Id == meterId);

            var readings = meter.GetAggregatedMeterReadings(EpicMetersAggregateBy.Day);

            int dataPoints = readings.Count;

            decimal sum = 0;

            foreach (MeterReading reading in readings)
            {
                sum = decimal.Add(sum, reading.Value);
            }

            decimal dailyAverage = Decimal.Divide(sum, dataPoints);
            //decimal monthlyAverage = Decimal.Multiply(dailyAverage, new Decimal(30.4368499));
            // EPIC 347 returning daily average
            return dailyAverage;
        }

       // public void Dispose()
        //{
            //if (_epicGeneratingUnits != null) _epicGeneratingUnits.Dispose();
       // }

        #endregion
    }
}
