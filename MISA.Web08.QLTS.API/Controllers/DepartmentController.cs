using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MySqlConnector;
using Dapper;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;

namespace MISA.Web08.QLTS.API.Controllers
{
    public class DepartmentController : BaseController<Department>
    {
        #region Field
        
        private IDepartmentBL _departmentBL;

        #endregion

        #region Contructor
        
        public DepartmentController(IDepartmentBL departmentBL) : base(departmentBL)
        {
            _departmentBL = departmentBL;
        }

        #endregion
    }
}
