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
    public class AssetDL : IAssetDL
    {
        public int DeleteAsset(Guid assetID)
        {
            //khởi tạo kết nối db
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            // Chuẩn bị tên Stored procedure
            string storedProcedureName = "Proc_asset_DeleteByID";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@$v_AssetID", assetID);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return numberOfAffectedRows;
        }

        public PaggingData<Assets> FilterAssets(string? keword, Guid? assetCategoryID, Guid? departmentID, int limit, int offset)
        {
            //khởi tạo kết nối db
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            // Chuẩn bị tên Stored procedure
            string storedProcedureName = "Proc_asset_GetPaging";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@$v_Offset", (offset - 1) * limit);
            parameters.Add("@$v_Limit", limit);
            parameters.Add("@$v_Sort", "");


            var whereConditions = new List<string>();
            if (keword != null)
            {
                whereConditions.Add($"fixed_asset_code LIKE '%{keword}%' OR fixed_asset_name LIKE '%{keword}%'");
            }
            if (departmentID != null)
            {
                whereConditions.Add($"department_id LIKE '%{departmentID}%'");
            }
            if (assetCategoryID != null)
            {
                whereConditions.Add($"fixed_asset_category_id LIKE '%{assetCategoryID}%'");
            }

            string whereClause = string.Join(" AND ", whereConditions);
            parameters.Add("@$v_Where", whereClause);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            var asset = multipleResults.Read<Assets>();
            var totalCount = multipleResults.Read<int>().Single();
            return new PaggingData<Assets>()
                    {
                        Data = asset.ToList(),
                        TotalCount = totalCount
                    };
        }

        public IEnumerable<Assets> GetAllAssets()
        {
            //khởi tạo kết nối db
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            //khai báo tên stored procedure
            string storedProcedure = "Proc_asset_GetAll";


            // Thực hiện gọi vào db
            var assets = mySqlConnection.Query<Assets>(storedProcedure, commandType: System.Data.CommandType.StoredProcedure);

            return assets;
        }

        

        public Assets GetEmployeeByID(Guid assetID)
        {

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            // Chuẩn bị tên Stored procedure
            string storedProcedureName = "Proc_asset_GetByAssetID";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@$v_AssetID", assetID);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var asset = mySqlConnection.QueryFirstOrDefault<Assets>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return asset;
        }

        public string GetNewEmployeeCode()
        {
            // Khởi tạo kết nối tới DB MySQL
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            // Chuẩn bị tên stored procedure
            string storedProcedureName = "Proc_asset_GetMaxCode";

            // Thực hiện gọi vào DB để chạy stored procedure ở trên
            string maxAssetCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý sinh mã nhân viên mới tự động tăng
            // Cắt chuỗi mã nhân viên lớn nhất trong hệ thống để lấy phần số
            // Mã nhân viên mới = "NV" + Giá trị cắt chuỗi ở  trên + 1
            string newAssetCode = "TS-" + (Int64.Parse(maxAssetCode.Substring(3)) + 1).ToString();

            // Trả về dữ liệu cho client
            return newAssetCode;
        }   

        public InsertData InsertAsset(Assets asset)
        {

            //khởi tạo kết nối đến db mySQL
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            // khai báo tên procdure insert
            var storedProcedureName = "Proc_asset_InsertOne";

            // chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();

            var assetID = Guid.NewGuid();
            parameters.Add("$v_AssetID", assetID);
            parameters.Add("$v_AssetCode", asset.fixed_asset_code);
            parameters.Add("$v_AssetName", asset.fixed_asset_name);
            parameters.Add("$v_DepartmentID", asset.department_id);
            parameters.Add("$v_DepartmentCode", asset.department_code);
            parameters.Add("$v_DepartmentName", asset.department_name);
            parameters.Add("$v_AssetCategoryID", asset.fixed_asset_category_id);
            parameters.Add("$v_AssetCategoryCode", asset.fixed_asset_category_code);
            parameters.Add("$v_AssetCategoryName", asset.fixed_asset_category_name);
            parameters.Add("$v_PurchaseDate", asset.purchase_date);
            parameters.Add("$v_Cost", asset.cost);
            parameters.Add("$v_Quantity", asset.quantity);
            parameters.Add("$v_DepreciationRate", asset.depreciation_rate);
            parameters.Add("$v_TrackedYear", asset.tracked_year);
            parameters.Add("$v_LifeTime", asset.life_time);
            parameters.Add("$v_ProductionDay", asset.production_date);
            parameters.Add("$v_CreatedBy", "Nguyễn Vũ Hải Thái");
            parameters.Add("$v_CreateDate", DateTime.Now);
            parameters.Add("$v_ModifiedBy", "Nguyễn Vũ Hải Thái");
            parameters.Add("$v_ModifiedDate", DateTime.Now);

            //thực hiện gọi vào db để chạy procdure
            var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return new InsertData(assetID, numberOfAffectedRows);
        }

        public EditData UpdateAsset(Guid assetID, Assets asset)
        {
            //khởi tạo kết nối đến db mySQL
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            // khai báo tên procdure insert
            var storedProcedureName = "Proc_asset_EditByID";

            // chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("@$v_AssetID", assetID);
            parameters.Add("$v_AssetCode", asset.fixed_asset_code);
            parameters.Add("$v_AssetName", asset.fixed_asset_name);
            parameters.Add("$v_DepartmentID", asset.department_id);
            parameters.Add("$v_DepartmentCode", asset.department_code);
            parameters.Add("$v_DepartmentName", asset.department_name);
            parameters.Add("$v_AssetCategoryID", asset.fixed_asset_category_id);
            parameters.Add("$v_AssetCategoryCode", asset.fixed_asset_category_code);
            parameters.Add("$v_AssetCategoryName", asset.fixed_asset_category_name);
            parameters.Add("$v_PurchaseDate", asset.purchase_date);
            parameters.Add("$v_Cost", asset.cost);
            parameters.Add("$v_Quantity", asset.quantity);
            parameters.Add("$v_DepreciationRate", asset.depreciation_rate);
            parameters.Add("$v_LifeTime", asset.life_time);
            parameters.Add("$v_ProductionDay", asset.production_date);
            parameters.Add("$v_ModifiedBy", "Nguyễn Vũ Hải Thái");
            parameters.Add("$v_ModifiedDate", DateTime.Now);

            //thực hiện gọi vào db để chạy procdure
            var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return new EditData(assetID, numberOfAffectedRows);
        }
    }
}
