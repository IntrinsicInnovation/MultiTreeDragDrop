using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EpicBusinessLogic;
using EpicBusinessLogic.Domain.CutPlanes;
using EpicDataAccess.CutPlanes;
using EpicBusinessLogic.Extensions;

namespace EpicWeb.Pages.Data
{
    public partial class CutPlaneNavigation : Page 
    {
        #region [Properties]
            private EpicCutPlanesManager _cutPlanesManager = null;
            private EpicCutPlanesManager CutPlanesManager
            {
                get
                {
                    if (_cutPlanesManager == null) _cutPlanesManager = new EpicCutPlanesManager();
                    return _cutPlanesManager;
                }
            }
        #endregion

        #region [Event Handlers]

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                  
                }
            }

            protected override void OnUnload(EventArgs e)
            {
                base.OnUnload(e);
            }

        #endregion

       
    }
}