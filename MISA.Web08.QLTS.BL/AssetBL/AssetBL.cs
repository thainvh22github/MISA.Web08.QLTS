using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    public class AssetBL : IAssetBL
    {

        private IAssetDL _assetDL;


        public AssetBL(IAssetDL assetDL)
        {
            _assetDL = assetDL;
        }

        public int DeleteAsset(Guid assetID)
        {
            return _assetDL.DeleteAsset(assetID);
        }

        public PaggingData<Assets> FilterAssets(string? keword, Guid? assetCategoryID, Guid? departmentID, int limit, int offset)
        {
            return _assetDL.FilterAssets(keword, assetCategoryID, departmentID, limit, offset);
        }

        public IEnumerable<Assets> GetAllAssets()
        {
            return _assetDL.GetAllAssets();
        }

        

        public Assets GetEmployeeByID(Guid assetID)
        {
            return _assetDL.GetEmployeeByID(assetID);
        }

        public string GetNewEmployeeCode()
        {
            return _assetDL.GetNewEmployeeCode();
        }

        public InsertData InsertAsset(Assets asset)
        {
            return _assetDL.InsertAsset(asset);
        }

        public EditData UpdateAsset(Guid assetID, Assets asset)
        {
            return _assetDL.UpdateAsset(assetID, asset);
        }
    }
}
