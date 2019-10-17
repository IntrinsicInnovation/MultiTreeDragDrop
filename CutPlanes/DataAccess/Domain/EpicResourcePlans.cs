using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain
{
    public class EpicResourcePlans : EpicDomainBase
    {
        #region [Constructors]

        public EpicResourcePlans(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Retrives the resource plan lined to the input id
        /// </summary>
        /// <param name="id"> Resource Plan id</param>
        /// <returns>ResourcePlan object</returns>
        public ResourcePlan Get(int id)
        {
            return this.Context.ResourcePlans.FirstOrDefault(rp => rp.Id == id);
        }

        /// <summary>
        /// Retrives the resource plan lined to the input id
        /// </summary>
        /// <param name="id"> Resource Plan id</param>
        /// <returns>ResourcePlan object</returns>
        public IEnumerable<ResourcePlan> Get()
        {            
            return this.Context.ResourcePlans;
        }

        /// <summary>
        /// Returns a list of ResourcePlan object linked to the input substation id
        /// </summary>
        /// <param name="substationId">substation id</param>
        /// <returns>list of ResourcePlan object</returns>
        public IList<ResourcePlan> GetList(int substationId)
        {
            var plans = from sa in this.Context.SubstationAttributes
                        join p in this.Context.ResourcePlans on sa.ResourcePlanId equals p.Id
                        where sa.SubstationId == substationId
                        select p;
            return plans != null ? plans.Distinct().ToList() : null;
        }

        #endregion
    }
}
