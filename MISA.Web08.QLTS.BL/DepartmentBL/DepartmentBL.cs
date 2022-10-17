using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    
    public class DepartmentBL : BaseBL<Department>, IDepartmentBL
    {
        #region Field,
        
        private IDepartmentDL _departmentDL;

        #endregion

        #region Contructor
       
        public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
        {
            _departmentDL = departmentDL;
        }

        #endregion

    }

    
    
}
