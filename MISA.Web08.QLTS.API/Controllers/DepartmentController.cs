using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MySqlConnector;
using Dapper;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.BL;

namespace MISA.Web08.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private IDepartmentBL _departmentBL;

        public DepartmentController(IDepartmentBL departmentBL)
        {
            _departmentBL = departmentBL;
        }

        /// <summary>
        /// API lấy danh sách toàn bộ phòng ban
        /// </summary>
        /// <returns>lấy danh sách toàn bộ phòng ban</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("")]
        public IActionResult GetAllDepartments()
        {
            try
            {
                var departments = _departmentBL.GetAllDepartments();

                return StatusCode(StatusCodes.Status200OK, departments);

                // Xử lý kết quả trả về từ DB
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }
    }
}
