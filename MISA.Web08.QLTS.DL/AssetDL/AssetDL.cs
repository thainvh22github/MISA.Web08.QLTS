using Dapper;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.Common.Enums;
using MISA.Web08.QLTS.Common.Resources;
using MySqlConnector;
using MISA.Web08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;

namespace MISA.Web08.QLTS.DL
{
    public class AssetDL : BaseDL<Assets>, IAssetDL
    {
        #region API Get
     
        /// <summary>
        /// lấy thông tin 1 tài sản theo id
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        public Assets GetAssetByID(Guid assetID)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = String.Format(Resource.Proc_GetByAssetID, typeof(Assets).Name);
                var parameters = new DynamicParameters();
                parameters.Add("@$v_AssetID", assetID);
                var asset = mySqlConnection.QueryFirstOrDefault<Assets>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return asset;
            }
        }

        /// <summary>
        /// Kiểm tra xem id tài sản này đã chứ từ bằng mã nào
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Mã chứng từ</returns>
        /// Author: NVHThai (3/11/2022)
        public string checkAssetIsActive(Guid assetID)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = "Proc_Asset_CheckAsset";
                var parameters = new DynamicParameters();
                parameters.Add("@$v_AssetID", assetID);
                var licenseCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return licenseCode;
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
        public PaggingData<Assets> FilterAssets(string? keword, Guid? assetCategoryID, Guid? departmentID, int limit, int offset)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = String.Format(Resource.Proc_GetPaging, typeof(Assets).Name);

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

                var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var asset = multipleResults.Read<Assets>();
                var totalCount = multipleResults.Read<int>().Single();
                var quantity = multipleResults.Read<int>().Single();
                var cost = multipleResults.Read<decimal>().Single();
                var loss = multipleResults.Read<decimal>().Single();

                return new PaggingData<Assets>()
                {
                    Data = asset.ToList(),
                    TotalCount = totalCount,
                    Quantity = quantity,
                    Cost = cost,
                    Loss = loss
                };
            }

            
        }

        /// <summary>
        /// API Lấy mã tài sản mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Author: NVHThai (16/09/2022)
        public string GetNewAssetCode()
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = String.Format(Resource.Proc_GetMaxCode, typeof(Assets).Name);
                string maxAssetCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý sinh mã nhân viên mới tự động tăng
                // Tách chuỗi mã nhân viên lớn nhất trong hệ thống để lấy phần số
                string resultString = Regex.Match(maxAssetCode, @"\d+").Value;
                string newAssetCode = "TS-" + (Int64.Parse(resultString) + 1).ToString();
                return newAssetCode;
            }
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

            string connectionString = DataContext.MySqlConnectionString;
            using(var mySqlConnection = new MySqlConnection(connectionString))
            {
                var storedProcedureName = String.Format(Resource.Proc_InsertOne, typeof(Assets).Name);
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
                parameters.Add("$v_Budget", asset.budget);

                var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (numberOfAffectedRows > 0)
                {
                    return new InsertData(assetID, numberOfAffectedRows);
                }
                else
                {
                    return new InsertData(Guid.Empty, 0);
                }
            }
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
            string connectionString = DataContext.MySqlConnectionString;
            using(var mySqlConnection = new MySqlConnection(connectionString))
            {
                var storedProcedureName = String.Format(Resource.Proc_EditByID, typeof(Assets).Name);

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
                parameters.Add("$v_Budget", asset.budget);

                var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                
                return new EditData(assetID, numberOfAffectedRows);
            }

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
            var numberOfAffectedRows = 0;
            //khởi tạo kết nối db
            string connectionString = DataContext.MySqlConnectionString;
            using(var mySqlConnection = new MySqlConnection(connectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }
                using (var transaction = mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        string storedProcedureName = String.Format(Resource.Proc_DeleteByID, typeof(Assets).Name);

                        var parameters = new DynamicParameters();
                        parameters.Add("@$v_AssetID", assetID);

                        numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);

                        if (numberOfAffectedRows == 1)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            numberOfAffectedRows = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        //nếu thực hiện không thành công thì rollback
                        transaction.Rollback();
                        numberOfAffectedRows = 0;
                    }
                    finally
                    {
                        mySqlConnection.Close();
                    }
                }
                    
            }
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
            var numberOfAffectedRows = 0;
            string connectionString = DataContext.MySqlConnectionString;
            using(var mySqlConnection = new MySqlConnection(connectionString))
            {
                //nếu như kết nối đang đóng thì tiến hành mở lại
                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }
                using (var transaction = mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        string storedProcedureName = String.Format(Resource.Proc_Deletes, typeof(Assets).Name);
                        for (int i = 0; i < assetList.Count; i++)
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add("@$v_AssetID", assetList[i]);
                            numberOfAffectedRows += mySqlConnection.Execute(storedProcedureName, parameters, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
                        }
                        if (numberOfAffectedRows == assetList.Count)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            numberOfAffectedRows = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        //nếu thực hiện không thành công thì rollback
                        transaction.Rollback();
                        numberOfAffectedRows = 0;
                    }
                    finally
                    {
                        mySqlConnection.Close();
                    }
                }
            }
            return numberOfAffectedRows;
        }

        
        #endregion
    }
}