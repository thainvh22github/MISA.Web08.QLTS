using MISA.Web08.QLTS.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.Common.Entities
{
    public class License
    {
        [PrimaryKey]
        public Guid LicenseID { get; set; }

        public string? LicenseCode { get; set; }

        public DateTime? LicenseDay { get; set; }

        public DateTime? WriteDay { get; set; }

        public float? TotalCost { get; set; }

        public string? Content { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<string> listAssetID { get; set; }

    }
}
