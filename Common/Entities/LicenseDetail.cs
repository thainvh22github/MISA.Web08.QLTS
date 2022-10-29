using MISA.Web08.QLTS.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.Common.Entities
{
    public class LicenseDetail
    {
        [PrimaryKey]
        public Guid LicenseDetailID { get; set; }

        public Guid? LicenseID { get; set; }

        public Guid? fixed_asset_id { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}