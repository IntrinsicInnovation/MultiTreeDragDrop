using System;

namespace EpicDataAccess.Domain
{
    public interface IResource
    {
        #region [Properties]

        string ResourceIdentifer { get; }
        string DateLastModifiedString { get; }
        string TypeString { get; }
        string NavigationUrl { get;}
        decimal? Voltage { get; }
        #endregion
    }
}
