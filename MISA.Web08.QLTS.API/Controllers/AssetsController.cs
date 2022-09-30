// using: sử dụng các hàm
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

namespace MISA.Web08.QLTS.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private IAssetBL _assetBL;

        public AssetsController(IAssetBL assetBL)
        {
            _assetBL = assetBL;
        }

        #region Api get
        /// <summary>
        /// API lấy danh sách toàn bộ nhân viên
        /// </summary>
        /// <returns>Lấy danh sách toàn bộ tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("")]
        public IActionResult GetAllAssets()
        {

            try
            {
                var assets = _assetBL.GetAllAssets();
                // Xử lý kết quả trả về từ DB

                return StatusCode(StatusCodes.Status200OK, assets);
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

                var asset = _assetBL.GetEmployeeByID(assetID);

                // Xử lý kết quả trả về từ DB
                if (asset != null)
                {
                    return StatusCode(StatusCodes.Status200OK, asset);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
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
                var multipleResults = _assetBL.FilterAssets(keword,assetCategoryID,departmentID,limit,offset);  
                // Xử lý kết quả trả về từ DB
                if (multipleResults != null)
                {
                    return StatusCode(StatusCodes.Status200OK, multipleResults);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
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
        /// API Lấy mã tài sản mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet("new-code")]
        public IActionResult GetNewEmployeeCode()
        {

            try
            {

                var newAssetCode = _assetBL.GetNewEmployeeCode();

                // Trả về dữ liệu cho client
                return StatusCode(StatusCodes.Status200OK, newAssetCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }

        #endregion


        #region Api post
        /// <summary>
        /// Thêm mới 1 tài sản
        /// </summary>
        /// <param name="asset"></param>
        /// <returns>id của tài sản thêm mới</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpPost]
        public IActionResult InsertAsset([FromBody] Assets asset)
        {

            try
            {
                var insertData = _assetBL.InsertAsset(asset);

                // xử lý trả về dữ liệu
                if (insertData.numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, insertData.assetID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
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

        #endregion

        #region api put
        /// <summary>
        /// Sửa 1 tài sản
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="asset"></param>
        /// <returns>id của tài sản sửa</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpPut("{assetID}")]
        public IActionResult UpdateAsset([FromRoute] Guid assetID, [FromBody] Assets asset)
        {
            try { 
                var dataUpdate = _assetBL.UpdateAsset(assetID, asset);

                    // xử lý trả về dữ liệu
                    if (dataUpdate.numberOfAffectedRows > 0)
                    {
                        return StatusCode(StatusCodes.Status201Created, dataUpdate.assetID);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest);
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
        #endregion

        #region api delete

        /// <summary>
        /// Xóa 1 tài sản bằng id
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns>id của tài sản xóa</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpDelete("{assetID}")]
        public IActionResult DeleteAsset([FromRoute] Guid assetID)
        {
            try
            {
                var numberOfAffectedRows = _assetBL.DeleteAsset(assetID);
                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, assetID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
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
        /// Xóa nhiều tài sản
        /// </summary>
        /// <param name="assetIDs"></param>
        /// <returns>1 list các id tài sản vừa bị xóa</returns>
        [HttpPost("batch-delete")]
        public IActionResult DeleteMutipleAssets([FromBody] List<string> assetIDs)
        {
            return StatusCode(StatusCodes.Status200OK);
        }
        #endregion
    }
}
