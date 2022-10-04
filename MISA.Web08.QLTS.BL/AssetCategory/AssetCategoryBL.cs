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
        #region Field
        
        private IAssetCategoryDL _assetCategoryDL;

        #endregion

        #region Contructor
        
        public AssetCategoryBL(IAssetCategoryDL assetCategoryDL)
        {
            _assetCategoryDL = assetCategoryDL;
        }

        #endregion

        #region Method
        
        /// <summary>
        /// Lấy danh sách loại tài sản theo tên loại tài sản và mã loại tài sản
        /// </summary>
        /// <param name="keword">Tên loại tài sản hoặc mã loại tài sản</param>
        /// <returns>Danh sách mã loại tài sản</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<AssetCategory> GetAllAssetsCategory(string? keword)
        {
            return _assetCategoryDL.GetAllAssetsCategory(keword);
        } 
        
        #endregion


    }
}
