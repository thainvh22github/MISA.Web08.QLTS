using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.Common.Entities
{
    public class EditData
    {
        #region Property

        /// <summary>
        /// id tài sản
        /// </summary>
        public Guid assetID { get; set; }
        
        /// <summary>
        /// Số cột bị ảnh hưởng trong db
        /// </summary>
        public int numberOfAffectedRows { get; set; }

        #endregion

        #region Contructor

        public EditData(Guid assetID, int numberOfAffectedRows)
        {
            this.assetID = assetID;
            this.numberOfAffectedRows = numberOfAffectedRows;
        } 

        #endregion
    }
}
