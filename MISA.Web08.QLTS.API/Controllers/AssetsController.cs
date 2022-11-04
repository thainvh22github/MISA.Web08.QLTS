using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;
using MISA.Web08.QLTS.Common.Attributes;
using MySqlConnector;
using Dapper;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.API.Controllers;

namespace MISA.Web08.QLTS.API
{
    
    public class AssetsController : BaseController<Assets>
    {
        #region Field

        private IAssetBL _assetBL;

        #endregion

        #region Contructor
        
        public AssetsController(IAssetBL assetBL) : base(assetBL)   
        {
            _assetBL = assetBL;
        } 
        
        #endregion

        #region Api Get

        /// <summary>
        /// Api lấy thông tin 1 tài sản theo id
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("{assetID}")]
        public IActionResult GetEmployeeByID([FromRoute] Guid assetID)
        {
            try
            {
                var asset = _assetBL.GetAssetByID(assetID);

                if (asset != null)
                {
                    return StatusCode(StatusCodes.Status200OK, asset);
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
        /// Kiểm tra xem id tài sản này đã chứ từ bằng mã nào
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Mã chứng từ</returns>
        /// Author: NVHThai (3/11/2022)
        [HttpGet]
        [Route("check-active/{assetID}")]
        public IActionResult checkAssetIsActive([FromRoute] Guid assetID)
        {
            try
            {
                var licenseCode = _assetBL.checkAssetIsActive(assetID);

                if (licenseCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, licenseCode);
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
        /// Hàm tìm kiếm và phân trang
        /// </summary>
        /// <param name="keword">tìm kiếm theo mã tài sản và tên tài sản</param>
        /// <param name="assetCategoryID">lọc theo id loại tài sản</param>
        /// <param name="departmentID">lọc theo id phòng ban</param>
        /// <param name="limit">số trang trong 1 bản ghi</param>
        /// <param name="offset">số trang</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet("filter")]
        public IActionResult FilterAssets([FromQuery] string? keword, [FromQuery] Guid? assetCategoryID,
            [FromQuery] Guid? departmentID, [FromQuery] int limit = 20, [FromQuery] int offset = 1)
        {
            try
            {
                var multipleResults = _assetBL.FilterAssets(keword, assetCategoryID, departmentID, limit, offset);
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
        /// API Lấy mã tài sản mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet("new-code")]
        public IActionResult GetNewEmployeeCode()
        {

            try
            {

                var newAssetCode = _assetBL.GetNewAssetCode();
                if (newAssetCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, newAssetCode);
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

        #endregion

        #region API Post

        /// <summary>
        /// Thêm mới 1 tài sản
        /// </summary>
        /// <param name="asset">Thông tin tài sản muốn thêm</param>
        /// <returns>id của tài sản thêm mới</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpPost]
        public IActionResult InsertAsset([FromBody] Assets asset)
        {
            try
            {
                var insertData = _assetBL.InsertAsset(asset);
                if (insertData.Success == (int)StatusResponse.Failed)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, insertData.Data);
                }
                else if( insertData.Success == (int)StatusResponse.Done)
                {
                    return StatusCode(StatusCodes.Status201Created, insertData.Data);
                }
                else 
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                        AssetErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UseMsg_Exception,
                        Resource.MoreInfo,
                        HttpContext.TraceIdentifier));
                }
            }

            catch (MySqlException ex)
            {
                if (ex.ErrorCode.ToString() == Resource.DuplicateKeyEntry) 
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        AssetErrorCode.DuplicateCode,
                        Resource.DevMsg_ValidateDuplicateCode,
                        Resource.UseMsg_ValidateDuplicateCode,
                        Resource.MoreInfo,
                        HttpContext.TraceIdentifier));
                }
                else
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

        #endregion

        #region API Put

        /// <summary>
        /// Sửa 1 tài sản
        /// </summary>
        /// <param name="assetID">ID tài sản cần sửa</param>
        /// <param name="asset">Thông tin tài sản cần sửa</param>
        /// <returns>id của tài sản sửa</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpPut("{assetID}")]
        public IActionResult UpdateAsset([FromRoute] Guid assetID, [FromBody] Assets asset)
        {
            try
            {
                var dataUpdate = _assetBL.UpdateAsset(assetID, asset);
                if(dataUpdate.Success == (int)StatusResponse.Failed)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, dataUpdate.Data);
                }
                else if(dataUpdate.Success == (int)StatusResponse.Done)
                {
                    return StatusCode(StatusCodes.Status200OK, dataUpdate.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                        AssetErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UseMsg_Exception,
                        Resource.MoreInfo,
                        HttpContext.TraceIdentifier));
                }
            }
            catch (MySqlException ex)
            {
                if (ex.ErrorCode.ToString() == Resource.DuplicateKeyEntry)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        AssetErrorCode.DuplicateCode,
                        Resource.DevMsg_ValidateDuplicateCode,
                        Resource.UseMsg_ValidateDuplicateCode,
                        Resource.MoreInfo,
                        HttpContext.TraceIdentifier));
                }
                else
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

        #endregion

        #region API Delete

        /// <summary>
        /// Xóa 1 tài sản bằng id
        /// </summary>
        /// <param name="assetID">ID tài sản cần xóa</param>
        /// <returns>id của tài sản xóa</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpDelete("{assetID}")]
        public IActionResult DeleteAsset([FromRoute] Guid assetID)
        {
            try
            {
                var numberOfAffectedRows = _assetBL.DeleteAsset(assetID);
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, assetID);
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

        /// <summary>
        /// Xóa nhiều tài sản
        /// </summary>
        /// <param name="assetIDs">Danh sách ID tài sản cần xóa</param>
        /// <returns>Số tài sản bị ảnh hưởng</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpPost("batch-delete")]
        public IActionResult DeleteMutipleAssets([FromBody] List<string> assetList)
        {
            try
            {
                var numberOfAffectedRows = _assetBL.DeleteMutipleAssets(assetList);
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, numberOfAffectedRows);
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
                    AssetErrorCode.DeleteFailed,
                    Resource.DevMsg_Exception,
                    Resource.UseMsg_Exception,
                    Resource.MoreInfo,
                    HttpContext.TraceIdentifier));
            }
        }

        #endregion
    }
}
