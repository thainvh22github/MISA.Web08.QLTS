using Dapper;
using MISA.Web08.QLTS.Common.Entities;
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
        public IEnumerable<AssetCategory> GetAllAssetsCategory()
        {
            // khởi tạo kết nối đến db
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            //khai báo tên stored procedure
            string storedProcedure = "Proc_asset_category_GetAll";

            // Thực hiện gọi vào db
            var assetCategory = mySqlConnection.Query<AssetCategory>(storedProcedure, commandType: System.Data.CommandType.StoredProcedure);

            return assetCategory;
        }
    }
}
