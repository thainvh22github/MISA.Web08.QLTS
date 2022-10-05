using Dapper;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.Common.Resources;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.DL
{
    public class AssetCategoryDL : IAssetCategoryDL
    {
        #region API Get

        /// <summary>
        /// Lấy danh sách loại tài sản theo tên loại tài sản và mã loại tài sản
        /// </summary>
        /// <param name="keword">Tên loại tài sản và mã loại tài sản</param>
        /// <returns>Danh sách loại tài sản</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<AssetCategory> GetAllAssetsCategory(string? keword)
        {
            //khởi tạo kết nối db
            string connectionString = DataContext.MySqlConnectionString;
            var mySqlConnection = new MySqlConnection(connectionString);

            // Chuẩn bị tên Stored procedure
            string storedProcedureName = String.Format(Resource.Proc_GetAll, typeof(AssetCategory).Name);

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();


            string whereConditions = null;
            if (keword != null)
            {
                whereConditions = $"fixed_asset_category_name LIKE '%{keword}%' OR fixed_asset_category_code LIKE '%{keword}%'";
            }

            parameters.Add("@$v_Where", whereConditions);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var assetCategory = mySqlConnection.Query<AssetCategory>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return assetCategory;
        } 
        
        #endregion
    }
}
