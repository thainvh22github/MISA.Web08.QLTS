using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    
    public class DepartmentBL : IDepartmentBL
    {
        #region Field
        
        private IDepartmentDL _departmentDL;

        #endregion

        #region Contructor
       
        public DepartmentBL(IDepartmentDL departmentDL)
        {
            _departmentDL = departmentDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy danh sách phòng ban theo mã phòng ban và tên phòng ban
        /// </summary>
        /// <param name="keword">Tên phòng ban hoặc mã phòng ban</param>
        /// <returns>Danh sách phòng ban</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<Department> GetAllDepartments(string? keword)
        {
            return _departmentDL.GetAllDepartments(keword);
        
        } 
        
        #endregion
    }

    
    
}
