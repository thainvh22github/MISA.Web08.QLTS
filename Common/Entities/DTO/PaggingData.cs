using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.Common.Entities
{
    public class PaggingData<Assets>
    {
        public List<Assets> Data { get; set; } = new List<Assets>();

        public int TotalCount { get; set; }
    }
}
