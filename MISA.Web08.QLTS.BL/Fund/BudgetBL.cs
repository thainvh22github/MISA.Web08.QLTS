using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    public class BudgetBL : BaseBL<Budget>, IBudgetBL
    {
        #region Field,

        private IBudgetDL _budgetDL;

        #endregion

        #region Contructor

        public BudgetBL(IBudgetDL budgetDL) : base(budgetDL)
        {
            _budgetDL = budgetDL;
        }
        #endregion
    }


}
