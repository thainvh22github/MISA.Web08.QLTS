using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.Common.Entities
{
    public class InsertData
    {
        #region Property

        /// <summary>
        /// ID tài sản
        /// </summary>
        public Guid assetID { get; set; }

        /// <summary>
        /// Số cột bị ảnh hưởng trong db
        /// </summary>
        public int numberOfAffectedRows { get; set; }

        #endregion

        #region Contructor

        public InsertData(Guid assetID, int numberOfAffectedRows)
        {
            this.assetID = assetID;
            this.numberOfAffectedRows = numberOfAffectedRows;
        } 

        #endregion
    }
}
