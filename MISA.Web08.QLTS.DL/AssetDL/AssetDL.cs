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
        #region API Get
        
        /// <summary>
        /// lấy danh sách toàn bộ nhân viên
        /// </summary>
        /// <returns>Lấy danh sách toàn bộ tài sản</returns>
        /// Author: NVHThai (16/09/2022)
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

        /// <summary>
        /// lấy thông tin 1 tài sản theo id
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        public Assets GetAssetByID(Guid assetID)
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
            var quantity = multipleResults.Read<int>().Single();
            var cost = multipleResults.Read<float>().Single();
            var loss = multipleResults.Read<float>().Single();
            var assetCodeList = multipleResults.Read<string>();

            return new PaggingData<Assets>()
            {
                Data = asset.ToList(),
                TotalCount = totalCount,
                Quantity = quantity,
                Cost = cost,
                Loss = loss,
                AssetCodeList = (List<string>)assetCodeList
            };
        }

        /// <summary>
        /// API Lấy mã tài sản mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Author: NVHThai (16/09/2022)
        public string GetNewAssetCode()
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
        
        #endregion


        #region API Post
        
        /// <summary>
        /// Thêm mới 1 tài sản
        /// </summary>
        /// <param name="asset">Thông tin tài sản thêm vào cơ sở dữ liệu</param>
        /// <returns>id của tài sản thêm mới</returns>
        /// Author: NVHThai (19/09/2022)
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
            parameters.Add("$v_LossYear", asset.loss_year);

            //thực hiện gọi vào db để chạy procdure
            var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return new InsertData(assetID, numberOfAffectedRows);
        }
        
        #endregion


        #region API Put
  
        /// <summary>
        /// Sửa 1 tài sản
        /// </summary>
        /// <param name="assetID">ID của tài sản cần sửa</param>
        /// <param name="asset">Thông tin tài sản cần sửa</param>
        /// <returns>id của tài sản sửa và số cột bị ảnh hưởng</returns>
        /// Author: NVHThai (19/09/2022)
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
            parameters.Add("$v_LossYear", asset.loss_year);


            //thực hiện gọi vào db để chạy procdure
            var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return new EditData(assetID, numberOfAffectedRows);
        }
        
        #endregion

        #region API Delete
        
        /// <summary>
        /// Xóa 1 tài sản bằng id
        /// </summary>
        /// <param name="assetID">ID của tài sản muốn xóa</param>
        /// <returns>id của tài sản xóa</returns>
        /// Author: NVHThai (19/09/2022)
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


        /// <summary>
        /// Xóa nhiều tài sản bằng id tài sản
        /// </summary>
        /// <param name="assetIDs">Danh sách id của tài sản muốn xóa</param>
        /// <returns>số cột bị ảnh hưởng</returns>
        /// Author: NVHThai (19/09/2022)
        public int DeleteMutipleAssets(List<string> assetList)
        {
            // Khởi tạo kết nối tới DB MySQL
            string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nvhthai;Uid=root;Pwd=thaibqhg12;";
            var mySqlConnection = new MySqlConnection(connectionString);

            // Khai báo tên prodecure Insert
            string storedProcedureName = "Proc_asset_Deletes";

            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            var queryList = new List<string>();
            for (int i = 0; i < assetList.Count; i++)
            {
                queryList.Add($"fixed_asset_id LIKE '{assetList[i]}'");
            }
            string assetIds = string.Join(" OR ", queryList);
            parameters.Add("$v_data", assetIds);

            // Xử lý dữ liệu trả về

            var listAssetIds = mySqlConnection.Query(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return assetList.Count;
        }
        
        #endregion
    }
}
