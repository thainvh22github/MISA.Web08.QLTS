using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.BL
{
    public class UserBL : IUserBL
    {
        private IUserDL _userDL;

        public UserBL(IUserDL userDL)
        {
            _userDL = userDL;
        }

        public User Login(User? user)
        {
            return _userDL.Login(user);
        }
    }
}
