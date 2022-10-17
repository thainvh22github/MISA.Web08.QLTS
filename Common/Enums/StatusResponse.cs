using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.Common.Enums
{
    public enum StatusResponse
    {
        /// <summary>
        /// Hoàn thành
        /// </summary>
        Done = 0,

        /// <summary>
        /// Lỗi
        /// </summary>
        Failed = 1,

        /// <summary>
        /// Lỗi exception
        /// </summary>
        Exception = 2,
    }
}
