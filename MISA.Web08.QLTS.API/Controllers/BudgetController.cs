using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;

namespace MISA.Web08.QLTS.API.Controllers
{
    public class BudgetController : BaseController<Budget>
    {
        #region Field

        private IBudgetBL _budgetBL;

        #endregion

        #region Contructor

        public BudgetController(IBudgetBL budgetBL) : base(budgetBL)
        {
            _budgetBL = budgetBL;
        }

        #endregion
        
    }
}
