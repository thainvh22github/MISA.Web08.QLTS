using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    public class FundsBL : BaseBL<Funds>, IFundsBL
    {
        #region Field,

        private IFundsDL _fundDL;

        #endregion

        #region Contructor

        public FundsBL(IFundsDL fundDL) : base(fundDL)
        {
            _fundDL = fundDL;
        }
        #endregion
    }


}
