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
        #region Field

        private IAssetBL _assetBL;

        #endregion

        #region Contructor
        
        public AssetsController(IAssetBL assetBL)
        {
            _assetBL = assetBL;
        } 
        
        #endregion

        #region Api Get

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
                var asset = _assetBL.GetAssetByID(assetID);

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
                var multipleResults = _assetBL.FilterAssets(keword, assetCategoryID, departmentID, limit, offset);
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

                var newAssetCode = _assetBL.GetNewAssetCode();

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

        #region API PUT

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
        /// <param name="assetIDs">Danh sách ID tài sản cần xóa</param>
        /// <returns>Số tài sản bị ảnh hưởng</returns>
        /// Author: NVHThai (19/09/2022)
        [HttpPost("batch-delete")]
        public int DeleteMutipleAssets(List<string> assetList)
        {
            return _assetBL.DeleteMutipleAssets(assetList);
        }

        #endregion
    }
}
