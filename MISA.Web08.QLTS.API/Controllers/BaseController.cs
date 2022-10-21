using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;

namespace MISA.Web08.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Field
         
        private IBaseBL<T> _baseBL;

        #endregion

        #region Contructor

        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API lấy danh sách toàn bộ bản ghi
        /// </summary>
        /// <returns>Lấy danh sách toàn bản ghi</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("")]
        public IActionResult GetAllRecords()
        {
            try
            {
                var assets = _baseBL.GetAllRecords();

                if (assets != null)
                {
                    return StatusCode(StatusCodes.Status200OK, assets);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                   AssetErrorCode.GetFailed,
                   Resource.DevMsg_GetFailed,
                   Resource.UseMsg_GetFailed,
                   Resource.MoreInfo,
                   HttpContext.TraceIdentifier));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.DevMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }
        }


        /// <summary>
        /// API lấy danh sách phòng ban theo mã bản ghi và tên bản ghi
        /// <param name="keyword">Mã phòng ban hoặc tên phòng ban</param>
        /// <returns>Danh sách phòng ban</returns>
        /// Author: NVHThai (16/09/2022)
        /// </summary>
        [HttpGet]
        [Route("FillterCodeOrName")]
        public IActionResult GetFillterRecords(string? keyword)
        {
            try
            {
                var departments = _baseBL.GetFillterRecords(keyword);

                if (departments != null)
                {
                    return StatusCode(StatusCodes.Status200OK, departments);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                      AssetErrorCode.GetFailed,
                      Resource.DevMsg_GetFailed,
                      Resource.UseMsg_GetFailed,
                      Resource.MoreInfo,
                      HttpContext.TraceIdentifier));
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

        /// <summary>
        /// Xuất file excel danh sách bản ghi
        /// </summary>
        /// <returns>File excel danh sách bản ghi</returns>
        /// Author:NVHThai (17/10/2022)
        [HttpGet("export")]
        public IActionResult ExportExcel()
        {
            var stream = _baseBL.ExportExcel();
            string excelName = $"{"danhsachtaisan"}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        #endregion
    }
}
