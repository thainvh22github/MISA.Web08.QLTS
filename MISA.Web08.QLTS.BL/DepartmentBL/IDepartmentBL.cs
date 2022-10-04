using MISA.Web08.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    public interface IDepartmentBL
    {
        #region API Get

        /// <summary>
        /// Lấy danh sách phòng ban theo mã phòng ban và tên phòng ban
        /// </summary>
        /// <param name="keword">Tên phòng ban hoặc mã phòng ban</param>
        /// <returns>Danh sách phòng ban</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<Department> GetAllDepartments(string? keword); 
        
        #endregion

    }
}
