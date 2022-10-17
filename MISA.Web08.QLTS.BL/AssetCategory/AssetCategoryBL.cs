using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{


    public class AssetCategoryBL : BaseBL<AssetCategory>, IAssetCategoryBL
    {
        #region Field
        
        private IAssetCategoryDL _assetCategoryDL;

        #endregion

        #region Contructor
        
        public AssetCategoryBL(IAssetCategoryDL assetCategoryDL) : base(assetCategoryDL)
        {
            _assetCategoryDL = assetCategoryDL;
        }

        #endregion
    }
}
