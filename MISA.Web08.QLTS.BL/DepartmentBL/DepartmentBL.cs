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
        private IDepartmentDL _departmentDL;

        public DepartmentBL(IDepartmentDL departmentDL)
        {
            _departmentDL = departmentDL;
        }
        public IEnumerable<Department> GetAllDepartments()
        {
            return _departmentDL.GetAllDepartments();
        }
    }

    
    
}
