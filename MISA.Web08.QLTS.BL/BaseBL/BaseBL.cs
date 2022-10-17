using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Contructor

        public BaseBL(IBaseDL<T> baseDL) 
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// lấy danh sách toàn bộ bản ghi
        /// <returns>Lấy danh sách toàn bản ghi</returns>
        /// Author: NVHThai (16/09/2022)
        /// </summary>
        public IEnumerable<T> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Lấy danh sách bản ghi có điều kiện
        /// </summary>
        /// <param name="keword">Tên hoặc mã</param>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVHThai (16/09/2022)
        public IEnumerable<T> GetFillterRecords(string? keword)
        {
            return _baseDL.GetFillterRecords(keword);
        }

        #endregion
    }
}
