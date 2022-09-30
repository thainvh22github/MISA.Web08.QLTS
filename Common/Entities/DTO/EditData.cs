using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.Common.Entities
{
    public class EditData
    {
        public Guid assetID { get; set; }
        public int numberOfAffectedRows { get; set; }

        public EditData(Guid assetID, int numberOfAffectedRows)
        {
            this.assetID = assetID;
            this.numberOfAffectedRows = numberOfAffectedRows;
        }
    }
}
