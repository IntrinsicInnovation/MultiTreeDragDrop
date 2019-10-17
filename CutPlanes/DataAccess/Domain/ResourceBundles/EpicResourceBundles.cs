using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpicDataAccess.Domain.ResourceBundles
{
    public class EpicResourceBundles : IDisposable
    {
        #region [Properties]

        private string ConnectionString { get; set; }

        private EpicResourceBundleDataContext _context = null;
        private EpicResourceBundleDataContext Context
        {
            get
            {
                if (_context == null) _context = new EpicResourceBundleDataContext(ConnectionString);
                return _context;
            }
        }

        #endregion

        #region [Methods]

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }

        #endregion
    }
}
