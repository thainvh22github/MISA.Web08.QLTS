using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// lấy danh sách toàn bộ bản ghi trong 1 bảng
        /// </summary>
        /// <returns>Lấy danh sách toàn bộ bản ghi trong bảng</returns>
        /// Author: NVHThai (16/09/2022)
        public IEnumerable<T> GetAllRecords();


        /// <summary>
        /// Lấy ra danh sách bản ghi có điều kiện
        /// </summary>
        /// <param name="keword">Tên bản ghi hoặc mã bản ghi tìm kiếm</param>
        /// <returns>Danh sách bản ghi có chọn lọc</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<T> GetFillterRecords(string? keword);
    }
}
