using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{


    public class AssetCategoryBL : IAssetCategoryBL
    {
        private IAssetCategoryDL _assetCategoryDL;

        public AssetCategoryBL(IAssetCategoryDL assetCategoryDL)
        {
            _assetCategoryDL = assetCategoryDL;
        }
        public IEnumerable<AssetCategory> GetAllAssetsCategory()
        {
            return _assetCategoryDL.GetAllAssetsCategory();
        }
    }
}
