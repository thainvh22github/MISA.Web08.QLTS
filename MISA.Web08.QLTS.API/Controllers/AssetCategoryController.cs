using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using MISA.Web08.QLTS.BL;

namespace MISA.Web08.QLTS.API.Controllers
{
    public class AssetCategoryController : Controller
    {
        #region Field
        
        private IAssetCategoryBL _assetCategoryBL;

        #endregion

        #region Contructor
        
        public AssetCategoryController(IAssetCategoryBL assetCategoryBL)
        {
            _assetCategoryBL = assetCategoryBL;
        }

        #endregion

        #region Method
        
        /// <summary>
        /// API lấy danh sách tài sản theo mã loại tài sản và tên loại tài sản
        /// <param name="keyword">Tên loại tài sản hoặc mã loại tài sản</param>
        /// <returns>Danh sách loại tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        /// </summary>
        [HttpGet]
        [Route("api/v1/[controller]")]
        public IActionResult GetAllAssetsCategory(string? keyword)
        {
            try
            {
                var assetCategory = _assetCategoryBL.GetAllAssetsCategory(keyword);

                return StatusCode(StatusCodes.Status200OK, assetCategory);

                // Xử lý kết quả trả về từ DB
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        } 
        
        #endregion
    }
}
