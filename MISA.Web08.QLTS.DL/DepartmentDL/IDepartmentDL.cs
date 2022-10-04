using MISA.Web08.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.DL
{
    public interface IDepartmentDL
    {
        #region API Get

        /// <summary>
        /// Lấy ra danh sách phòng ban lọc theo tên phòng ban và mã phòng ban
        /// </summary>
        /// <param name="keword">Tên phòng ban hoặc mã phòng ban cần tìm kiếm</param>
        /// <returns>Danh sách phòng ban</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<Department> GetAllDepartments(string? keword); 
        
        #endregion
    }
}
