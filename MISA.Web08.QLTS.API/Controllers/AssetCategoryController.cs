using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;
using MISA.Web08.QLTS.API.Entities;

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

                if(assetCategory != null)
                {
                    return StatusCode(StatusCodes.Status200OK, assetCategory);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, AssetErrorCode.GetFailed);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
