using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MySqlConnector;
using Dapper;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;

namespace MISA.Web08.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        #region Field
        
        private IDepartmentBL _departmentBL;

        #endregion

        #region Contructor
        
        public DepartmentController(IDepartmentBL departmentBL)
        {
            _departmentBL = departmentBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API lấy danh sách phòng ban theo mã phòng ban và tên phòng ban
        /// <param name="keyword">Mã phòng ban hoặc tên phòng ban</param>
        /// <returns>Danh sách phòng ban</returns>
        /// Author: NVHThai (16/09/2022)
        /// </summary>
        [HttpGet]
        [Route("")]
        public IActionResult GetAllDepartments(string? keyword)
        {
            try
            {
                var departments = _departmentBL.GetAllDepartments(keyword);

                if(departments != null)
                {
                    return StatusCode(StatusCodes.Status200OK, departments);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, AssetErrorCode.GetFailed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.UseMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }
        } 

        #endregion
    }
}
