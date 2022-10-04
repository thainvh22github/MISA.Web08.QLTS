using MISA.Web08.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    public interface IAssetCategoryBL
    {
        #region API Get
        
        /// <summary>
        /// Lấy danh sách loại tài sản theo tên loại tài sản và mã loại tài sản
        /// </summary>
        /// <param name="keword">Tên loại tài sản hoặc mã loại tài sản</param>
        /// <returns>Danh sách mã loại tài sản</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<AssetCategory> GetAllAssetsCategory(string? keword); 
        
        #endregion
    }
}
