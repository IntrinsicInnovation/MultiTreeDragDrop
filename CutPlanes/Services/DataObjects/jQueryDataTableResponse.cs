using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWeb.Services.DataObjects
{
    
    /// <summary>
    /// Class used to format response to jQuery Table 
    /// </summary>
    public sealed class jQueryDataTableResponse
    {
        private List<string[]> _aaData = null;
        public List<string[]> aaData
        {
            get { if (_aaData == null) _aaData = new List<string[]>(); return _aaData; }
        }
    }
}