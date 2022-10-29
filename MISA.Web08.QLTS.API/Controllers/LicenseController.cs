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
    public class LicenseController : ControllerBase
    {
        #region Field

        private ILicenseBL _licenseBL;

        #endregion

        #region Contructor

        public LicenseController(ILicenseBL licenseBL)
        {
            _licenseBL = licenseBL;
        }

        #endregion

        /// <summary>
        /// Hàm lấy mã chứng từ mới nhất
        /// </summary>
        /// <returns>Mã chứng từ mới nhất</returns>
        /// Author: NVHThai (26/10/2022)
        [HttpGet("new-code")]
        public IActionResult GetNewLicenseCode()
        {
            try
            {

                var newLicenseCode = _licenseBL.GetNewLicenseCode();
                if (newLicenseCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, newLicenseCode);
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
                    Resource.MoreInfo,
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Hàm lấy danh sách tài sản theo id chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (26/10/2022)
        [HttpGet]
        [Route("list-asset/{licenseID}")]
        public IActionResult GetListAssetByLicenseID([FromRoute] Guid licenseID)
        {
            try
            {
                var license = _licenseBL.GetListAssetByLicenseID(licenseID);

                if (licenseID != null)
                {
                    return StatusCode(StatusCodes.Status200OK, license);
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
                    Resource.MoreInfo,
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Hàm lấy danh sách chứng từ theo id chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (26/10/2022)
        [HttpGet]
        [Route("list-license-detail/{licenseID}")]
        public IActionResult GetListLicenseDetailByLicenseID([FromRoute] Guid licenseID)
        {
            try
            {
                var licenseDetail = _licenseBL.GetListLicenseDetailByLicenseID(licenseID);

                if (licenseID != null)
                {
                    return StatusCode(StatusCodes.Status200OK, licenseDetail);
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
                    Resource.MoreInfo,
                    HttpContext.TraceIdentifier));
            }
        }

        /// <summary>
        /// Hàm lấy danh sách chứng từ và tìm kiếm
        /// </summary>
        /// <param name="keword">Mã chứng từ hoặc nội dung</param>
        /// <param name="limit">số bản ghi trong 1 trang</param>
        /// <param name="offset">số trang</param>
        /// <returns>Danh sách chứng từ</returns>
        /// Author: NVHThai (26/10/2022)
        [HttpGet("filter")]
        public IActionResult FilterLicense([FromQuery] string? keword, [FromQuery] int limit = 20, [FromQuery] int offset = 1)
        {
            try
            {
                var multipleResults = _licenseBL.FilterLicense(keword, limit, offset);
                if (multipleResults != null)
                {
                    return StatusCode(StatusCodes.Status200OK, multipleResults);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                      AssetErrorCode.GetFailed,
                      Resource.DevMsg_GetFailed,
                      Resource.UseMsg_GetFailed,
                      Resource.MoreInfo_Exception,
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
        /// Hàm tìm kiếm và phân trang
        /// </summary>
        /// <param name="keword">tìm kiếm theo mã tài sản và tên tài sản</param>
        /// <param name="assetCategoryID">lọc theo id loại tài sản</param>
        /// <param name="departmentID">lọc theo id phòng ban</param>
        /// <param name="limit">số trang trong 1 bản ghi</param>
        /// <param name="offset">số trang</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet("filter/asset")]
        public IActionResult FilterAssets([FromQuery] string? keword, [FromQuery] int limit = 20, [FromQuery] int offset = 1)
        {
            try
            {
                var multipleResults = _licenseBL.FilterAssets(keword, limit, offset);
                if (multipleResults != null)
                {
                    return StatusCode(StatusCodes.Status200OK, multipleResults);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                      AssetErrorCode.GetFailed,
                      Resource.DevMsg_GetFailed,
                      Resource.UseMsg_GetFailed,
                      Resource.MoreInfo_Exception,
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
        /// Hàm thêm 1 chứng từ
        /// </summary>
        /// <param name="license">Thông tin chứng từ</param>
        /// <returns>Số bản ghi thêm</returns>
        /// Author: NVHThai (26/10/2022)
        [HttpPost]
        public IActionResult InsertLicense([FromBody] License license)
        {
            try
            {
                var insertData = _licenseBL.InsertLicense(license);
                if (insertData > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, insertData);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, insertData);

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

        /// <summary>
        /// Hàm sửa 1 chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ cần sửa</param>
        /// <param name="license">Thông tin chứng từ</param>
        /// <returns>Số bản ghi đã sửa</returns>
        /// Author: NVHThai (26/10/2022)
        [HttpPut("{licenseID}")]
        public IActionResult UpdateLicense([FromRoute] Guid licenseID, [FromBody] License license)
        {
            try
            {
                var updateData = _licenseBL.UpdateLicense(licenseID,license);
                if (updateData > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, updateData);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, updateData);

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

        /// <summary>
        /// Hàm xóa 1 chứng từ
        /// </summary>
        /// <param name="licenseID">ID chứng từ cần xóa</param>
        /// <returns>Số bản ghi ảnh hưởng</returns>
        /// Author: NVHThai (26/10/2022)
        [HttpDelete("{licenseID}")]
        public IActionResult DeleteLicense([FromRoute] Guid licenseID)
        {
            try
            {
                var numberOfAffectedRows = _licenseBL.DeleteLicense(licenseID);
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, licenseID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AssetErrorCode.DeleteFailed,
                    Resource.DevMsg_DeleteFailed,
                    Resource.UseMsg_DeleteFailed,
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
                    Resource.MoreInfo,
                    HttpContext.TraceIdentifier));
            }
        }
    }
}
