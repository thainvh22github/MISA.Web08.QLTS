using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using MISA.Web08.QLTS.BL;

namespace MISA.Web08.QLTS.API.Controllers
{
    public class AssetCategoryController : Controller
    {
        private IAssetCategoryBL _assetCategoryBL;


        public AssetCategoryController(IAssetCategoryBL assetCategoryBL)
        {
            _assetCategoryBL = assetCategoryBL;
        }
        /// <summary>
        /// API lấy danh sách toàn bộ phòng ban
        /// </summary>
        /// <returns>lấy danh sách toàn bộ phòng ban</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("api/v1/[controller]")]
        public IActionResult GetAllAssetsCategory()
        {
            try
            {
                var assetCategory = _assetCategoryBL.GetAllAssetsCategory();
                
                return StatusCode(StatusCodes.Status200OK, assetCategory);

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
