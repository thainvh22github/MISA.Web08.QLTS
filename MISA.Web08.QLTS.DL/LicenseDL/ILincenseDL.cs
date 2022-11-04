using MISA.Web08.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.DL
{
    public interface ILincenseDL
    {
        /// <summary>
        /// Hàm lấy mã chứng từ mới nhất
        /// </summary>
        /// <returns>Mã chứng từ mới nhất</returns>
        /// Author: NVHThai (26/10/2022)
        public string GetNewLicenseCode();

        /// <summary>
        /// Hàm lấy danh sách tài sản theo id chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (26/10/2022)
        public IEnumerable<Assets> GetListAssetByLicenseID(Guid? licenseID);

        /// <summary>
        /// Hàm lấy danh sách chứng từ theo id chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (26/10/2022)
        public IEnumerable<LicenseDetail> GetListLicenseDetailByLicenseID(Guid? licenseID);

        /// <summary>
        /// Hàm lấy danh sách chứng từ và tìm kiếm
        /// </summary>
        /// <param name="keword">Mã chứng từ hoặc nội dung</param>
        /// <param name="limit">số bản ghi trong 1 trang</param>
        /// <param name="offset">số trang</param>
        /// <returns>Danh sách chứng từ</returns>
        /// Author: NVHThai (26/10/2022)
        public PaggingData<License> FilterLicense(string? keword,int limit, int offset);

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
        public PaggingData<Assets> FilterAssets(string? keword, int limit, int offset);


        /// <summary>
        /// Hàm thêm 1 chứng từ
        /// </summary>
        /// <param name="license">Thông tin chứng từ</param>
        /// <returns>Số bản ghi thêm</returns>
        /// Author: NVHThai (26/10/2022)
        public int InsertLicense(License license);

        /// <summary>
        /// Hàm sửa 1 chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ cần sửa</param>
        /// <param name="license">Thông tin chứng từ</param>
        /// <returns>Số bản ghi đã sửa</returns>
        /// Author: NVHThai (26/10/2022)
        public int UpdateLicense(Guid licenseID, License license);

        /// <summary>
        /// Hàm xóa 1 chứng từ
        /// </summary>
        /// <param name="licenseID">ID chứng từ cần xóa</param>
        /// <returns>Số bản ghi ảnh hưởng</returns>
        /// Author: NVHThai (26/10/2022)
        public int DeleteLicense(Guid licenseID);

        /// <summary>
        /// Xóa nhiều chứng từ bằng id chứng từ
        /// </summary>
        /// <param name="assetIDs">Danh sách id của tài sản muốn xóa</param>
        /// <returns>số cột bị ảnh hưởng</returns>
        /// Author: NVHThai (26/10/2022)
        public int DeleteMutipleLicense(List<string> licenseList);

    }
}
