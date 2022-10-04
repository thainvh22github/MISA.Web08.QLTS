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

        public Guid assetID { get; set; }
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
