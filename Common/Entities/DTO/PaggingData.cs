using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.Common.Entities
{
    public class PaggingData<Assets>
    {
        #region Property
        /// <summary>
        /// Danh sách tài sản
        /// </summary>
        public List<Assets> Data { get; set; } = new List<Assets>();

        /// <summary>
        /// Số lượng tài sản
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Tổng số lượng
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Tổng nguyên giá
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Tổng giá trị còn lại
        /// </summary>
        public decimal Loss { get; set; }

        #endregion
    }
}
