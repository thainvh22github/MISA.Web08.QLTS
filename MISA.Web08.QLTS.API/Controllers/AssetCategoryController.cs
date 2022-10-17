using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.Common.Entities;

namespace MISA.Web08.QLTS.API.Controllers
{
    public class AssetCategoryController : BaseController<AssetCategory>
    {
        #region Field
        
        private IAssetCategoryBL _assetCategoryBL;

        #endregion

        #region Contructor
        
        public AssetCategoryController(IAssetCategoryBL assetCategoryBL) : base(assetCategoryBL)
        {
            _assetCategoryBL = assetCategoryBL;
        }

        #endregion
    }
}
