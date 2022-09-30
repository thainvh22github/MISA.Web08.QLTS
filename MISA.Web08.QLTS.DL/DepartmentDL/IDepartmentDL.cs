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
        public IEnumerable<Department> GetAllDepartments();

    }
}
