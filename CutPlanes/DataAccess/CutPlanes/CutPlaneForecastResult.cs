using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using EpicDataAccess.Extensions;


namespace EpicDataAccess.CutPlanes
{



    public partial class CutPlaneForecastResult 
    {
        #region [Properties]

        int? ID { get; set; }
        string ResourcePlan { get; set; }
        string CutPlane { get; set; }
        string CutPlaneType { get; set; }

        List<NameValuePair> ForecastFacts { get; set; }

        public class NameValuePair
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public NameValuePair(string Name, string Value)
            {
                this.Name = Name;
                this.Value = Value;
            }
        }


        #endregion

        #region [Constructor]

          



       

        #endregion

      
    }


}
