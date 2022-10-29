using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;

namespace MISA.Web08.QLTS.API.Controllers
{
    public class FundController : BaseController<Funds>
    {
        #region Field

        private IFundsBL _fundBL;

        #endregion

        #region Contructor

        public FundController(IFundsBL fundBL) : base(fundBL)
        {
            _fundBL = fundBL;
        }

        #endregion
        
    }
}
