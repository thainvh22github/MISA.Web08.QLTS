using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.DL
{
    public interface IAssetDL
    {
        #region Api get
        /// <summary>
        /// lấy danh sách toàn bộ nhân viên
        /// </summary>
        /// <returns>Lấy danh sách toàn bộ tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        public IEnumerable<Assets> GetAllAssets();




        /// <summary>
        /// lấy thông tin 1 tài sản theo id
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 tài sản</returns>
        /// Author: NVHThai (16/09/2022)

        public Assets GetEmployeeByID(Guid assetID);


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
        public PaggingData<Assets> FilterAssets(string? keword, Guid? assetCategoryID,
            Guid? departmentID, int limit, int offset);

        /// <summary>
        /// API Lấy mã tài sản mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Author: NVHThai (16/09/2022)
        public string GetNewEmployeeCode();

        #endregion


        #region Api post
        /// <summary>
        /// Thêm mới 1 tài sản
        /// </summary>
        /// <param name="asset"></param>
        /// <returns>id của tài sản thêm mới</returns>
        /// Author: NVHThai (19/09/2022)
        public InsertData InsertAsset(Assets asset);

        #endregion

        #region api put
        /// <summary>
        /// Sửa 1 tài sản
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="asset"></param>
        /// <returns>id của tài sản sửa</returns>
        /// Author: NVHThai (19/09/2022)
        public EditData UpdateAsset(Guid assetID, Assets asset);
        #endregion

        #region api delete

        /// <summary>
        /// Xóa 1 tài sản bằng id
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns>id của tài sản xóa</returns>
        /// Author: NVHThai (19/09/2022)
        public int DeleteAsset(Guid assetID);

        /// <summary>
        /// Xóa nhiều tài sản
        /// </summary>
        /// <param name="assetIDs"></param>
        /// <returns>1 list các id tài sản vừa bị xóa</returns>
        //public IActionResult DeleteMutipleAssets(List<string> assetIDs);
        #endregion



        
    }
}
