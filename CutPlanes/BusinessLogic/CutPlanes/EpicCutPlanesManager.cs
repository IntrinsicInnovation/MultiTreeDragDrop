using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpicDataAccess.CutPlanes;
using CutPlanes.DataAccess.CutPlanes;
using EpicBusinessLogic.Domain.CutPlanes;


namespace EpicBusinessLogic.Domain.CutPlanes
{
    public class EpicCutPlanesManager
    {
        public EpicCutPlanesManager() 
        {
        
        }

        #region [Properties]

        private string ConnectionString { get; set; }

        private string ApplicationName { get; set; }

        private EpicCutPlanes _epicCutPlanes = null;
        private EpicCutPlanes EpicCutPlanes
        {
            get { if (_epicCutPlanes == null) _epicCutPlanes = new EpicCutPlanes(ConnectionString); return _epicCutPlanes; }
        }
        
        #endregion

        #region [Functions]

        /// <summary>
        /// Gets a list of all Cut Planes.
        /// </summary>
        /// <returns>List of CutPlane objects</returns>
        public List<CutPlane> GetCutPlanes()
        {
            List<CutPlane> cutPlanes = EpicCutPlanes.GetCutPlanes();
            if (cutPlanes == null) return null;
            return cutPlanes;
        }


        /// <summary>
        /// Gets Cut Plane object by ID.
        /// </summary>
        /// <returns>CutPlane object.</returns>
        public CutPlane GetCutPlane(int cutPlaneID)
        {
            CutPlane cutplane = EpicCutPlanes.GetCutPlane(cutPlaneID);
            return cutplane;
        }

        /// <summary>
        /// Gets a list of Cut Plane Type objects.
        /// </summary>
        /// <returns>list of CutPlane objects.</returns>
        public List<CutPlaneType> GetCutPlaneTypes()
        {
            List<CutPlaneType> cutplanetypes = EpicCutPlanes.GetCutPlaneTypes();
            return cutplanetypes;
        }



        /// <summary>
        /// Gets a list of Pool Types
        /// </summary>
        /// <returns>list of Pool Type objects.</returns>
        public List<PoolType> GetPoolTypes()
        {
            List<PoolType> pooltypes = EpicCutPlanes.GetPoolTypes();
            return pooltypes;
        }



        /// <summary>
        /// Gets a list of Pool objects.
        /// </summary>
        /// <returns>list of CutPlaneStreamType objects.</returns>
        public List<Pool> GetPools(int cpid)
        {
            List<Pool> pools = EpicCutPlanes.GetPools(cpid);
            return pools;
        }




        /// <summary>
        /// Inserts the input CutPlane Object into the Db or updates the existing CutPlane object (based on ID)
        /// </summary>
        /// <param name="substation">CutPlane Object</param>
        /// <returns>db identity</returns>
        public void SaveCutPlanebyID(CutPlane cp)
        {
            if (cp == null) return;
            EpicCutPlanes.SaveCutPlanebyID(cp);
        }


        #endregion
    }
}
