using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;

namespace MISA.Web08.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Field

        private IUserBL _userBL;

        #endregion

        #region Contructor

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        #endregion

        [HttpPost]
        public IActionResult CheckUser([FromBody] User? user)
        {
            try
            {
                var check = _userBL.Login(user);
                if (check != null)
                {
                    return StatusCode(StatusCodes.Status201Created, check);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, check);
                }
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                        AssetErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UseMsg_Exception,
                        Resource.MoreInfo,
                        HttpContext.TraceIdentifier));
                }
            }
        }


    }
}
