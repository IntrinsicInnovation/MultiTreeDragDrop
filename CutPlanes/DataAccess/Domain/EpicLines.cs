using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicLines : EpicDomainBase
    { 
        #region [Properties]

        #endregion

        #region [Constructors]

        public EpicLines(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Retrieves a Line object based on input identity
        /// </summary>
        /// <param name="id">Line identity</param>
        /// <returns>Line object</returns>
        public Line GetLine(int id)
        {
            return this.Context.Lines.FirstOrDefault(l => l.Id == id);
        }

        public List<Line> GetLines()
        {
            return this.Context.Lines.OrderBy(x => x.Name).ToList();
        }

        public VoltageLevel GetLineVoltage(int lineId)
        {
            var voltage = from ln in this.Context.LinkLinesVoltageLevels.Where(link => link.LineId == lineId)
                          join n in this.Context.VoltageLevels on ln.VoltageLevelId equals n.Id
                          select n;

            return voltage != null ? voltage.FirstOrDefault() : null;
        }

        public IList<Note> GetNotes(int lineId)
        {
            var notes = from ln in this.Context.LinkNotes.Where(link => link.LineId == lineId)
                        join n in this.Context.Notes on ln.NoteId equals n.Id
                        where n.IsDeleted == false
                        select n;
            return notes != null ? notes.ToList() : null;
        }

        #endregion
    }
}
