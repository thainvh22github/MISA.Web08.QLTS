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

        public List<Assets> Data { get; set; } = new List<Assets>();

        public int TotalCount { get; set; }

        public int Quantity { get; set; }

        public float Cost { get; set; }

        public float Loss { get; set; }

        public List<string> AssetCodeList { get; set; } = new List<string>(); 

        #endregion
    }
}
