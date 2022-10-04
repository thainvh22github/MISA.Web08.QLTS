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

        #region Field
        
        private IAssetDL _assetDL;

        #endregion


        #region Contructor
        
        public AssetBL(IAssetDL assetDL)
        {
            _assetDL = assetDL;
        }


        #endregion

        /// <summary>
        /// lấy danh sách toàn bộ nhân viên
        /// </summary>
        /// <returns>Lấy danh sách toàn bộ tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        public IEnumerable<Assets> GetAllAssets()
        {
            return _assetDL.GetAllAssets();
        }

        /// <summary>
        /// lấy thông tin 1 tài sản theo id
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        public Assets GetAssetByID(Guid assetID)
        {
            return _assetDL.GetAssetByID(assetID);
        }

        /// <summary>
        /// Hàm tìm kiếm và phân trang
        /// </summary>
        /// <param name="keword">tìm kiếm theo mã tài sản và tên tài sản</param>
        /// <param name="assetCategoryID">lọc theo id loại tài sản</param>
        /// <param name="departmentID">lọc theo id phòng ban</param>
        /// <param name="limit">số trang trong 1 bản ghi</param>
        /// <param name="offset">số trang</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        public PaggingData<Assets> FilterAssets(string? keword, Guid? assetCategoryID, Guid? departmentID, int limit, int offset)
        {
            return _assetDL.FilterAssets(keword, assetCategoryID, departmentID, limit, offset);
        }

        /// <summary>
        /// API Lấy mã tài sản mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Author: NVHThai (16/09/2022)
        public string GetNewAssetCode()
        {
            return _assetDL.GetNewAssetCode();
        }

        /// <summary>
        /// Thêm mới 1 tài sản
        /// </summary>
        /// <param name="asset">Thông tin tài sản muốn thêm</param>
        /// <returns>id của tài sản thêm mới</returns>
        /// Author: NVHThai (19/09/2022)
        public InsertData InsertAsset(Assets asset)
        {
            return _assetDL.InsertAsset(asset);
        }

        /// <summary>
        /// Sửa 1 tài sản
        /// </summary>
        /// <param name="assetID">ID tài sản cần sửa</param>
        /// <param name="asset">Thông tin tài sản cần sửa</param>
        /// <returns>id của tài sản sửa</returns>
        /// Author: NVHThai (19/09/2022)
        public EditData UpdateAsset(Guid assetID, Assets asset)
        {
            return _assetDL.UpdateAsset(assetID, asset);
        }

        /// <summary>
        /// Xóa 1 tài sản bằng id
        /// </summary>
        /// <param name="assetID">ID tài sản cần xóa</param>
        /// <returns>id của tài sản xóa</returns>
        /// Author: NVHThai (19/09/2022)
        public int DeleteAsset(Guid assetID)
        {
            return _assetDL.DeleteAsset(assetID);
        }

        /// <summary>
        /// Xóa nhiều tài sản
        /// </summary>
        /// <param name="assetIDs">Danh sách ID tài sản cần xóa</param>
        /// <returns>Số tài sản bị ảnh hưởng</returns>
        /// Author: NVHThai (19/09/2022)
        public int DeleteMutipleAssets(List<string> assetList)
        {
            return _assetDL.DeleteMutipleAssets(assetList);
        }
    }
}
