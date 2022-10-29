using Dapper;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.Common.Resources;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Web08.QLTS.DL
{
    public class LicenseDL : ILincenseDL
    {
        /// <summary>
        /// Hàm lấy mã chứng từ mới nhất
        /// </summary>
        /// <returns>Mã chứng từ mới nhất</returns>
        /// Author: NVHThai (26/10/2022)
        public string GetNewLicenseCode()
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = "Proc_LicenseMaster_GetMaxCode";
                string maxLicenseCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý sinh mã nhân viên mới tự động tăng
                // Tách chuỗi mã nhân viên lớn nhất trong hệ thống để lấy phần số
                string resultString = Regex.Match(maxLicenseCode, @"\d+").Value;
                string NewLicenseCode = "G" + (Int64.Parse(resultString) + 1).ToString();
                return NewLicenseCode;
            }
        }

        /// <summary>
        /// Hàm lấy danh sách tài sản theo id chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (26/10/2022)
        public IEnumerable<Assets> GetListAssetByLicenseID(Guid? licenseID)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = "Proc_ListAsset_GetByLicenseID";
                var parameters = new DynamicParameters();
                parameters.Add("@$v_Where", licenseID);
                var listAsset = mySqlConnection.Query<Assets>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return listAsset;
            }
        }

        /// <summary>
        /// Hàm lấy danh sách chứng từ theo id chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ</param>
        /// <returns>Danh sách tài sản</returns>
        /// Author: NVHThai (26/10/2022)
        public IEnumerable<LicenseDetail> GetListLicenseDetailByLicenseID(Guid? licenseID)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = "Proc_ListLicenseDetail_GetByLicenseIID";
                var parameters = new DynamicParameters();
                parameters.Add("@$v_Where", licenseID);
                var listLicenseDetail = mySqlConnection.Query<LicenseDetail>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return listLicenseDetail;
            }
        }

        /// <summary>
        /// Hàm lấy danh sách chứng từ và tìm kiếm
        /// </summary>
        /// <param name="keword">Mã chứng từ hoặc nội dung</param>
        /// <param name="limit">số bản ghi trong 1 trang</param>
        /// <param name="offset">số trang</param>
        /// <returns>Danh sách chứng từ</returns>
        /// Author: NVHThai (26/10/2022)
        public PaggingData<License> FilterLicense(string? keword, int limit, int offset)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = "Proc_LicenseDetail_GetPaging";

                var parameters = new DynamicParameters();
                parameters.Add("@$v_Offset", (offset - 1) * limit);
                parameters.Add("@$v_Limit", limit);
                parameters.Add("@$v_Sort", "");

                var whereConditions = new List<string>();
                if (keword != null)
                {
                    whereConditions.Add($"LicenseCode LIKE '%{keword}%' OR Content LIKE '%{keword}%'");
                }

                string whereClause = string.Join(" AND ", whereConditions);
                parameters.Add("@$v_Where", whereClause);

                var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var license = multipleResults.Read<License>();
                var totalCount = multipleResults.Read<int>().Single();
                var sum = multipleResults.Read<int>().Single();

                return new PaggingData<License>()
                {
                    Data = license.ToList(),
                    TotalCount = totalCount,
                    Quantity = sum,
                    Cost = 0,
                    Loss = 0
                };
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
        public PaggingData<Assets> FilterAssets(string? keword, int limit, int offset)
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
                whereConditions.Add($"active = 0");
                

                string whereClause = string.Join(" AND ", whereConditions);
                parameters.Add("@$v_Where", whereClause);

                var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var asset = multipleResults.Read<Assets>();
                var totalCount = multipleResults.Read<int>().Single();
                var quantity = multipleResults.Read<int>().Single();
                var cost = multipleResults.Read<float>().Single();
                var loss = multipleResults.Read<float>().Single();

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
        /// Hàm thêm 1 chứng từ
        /// </summary>
        /// <param name="license">Thông tin chứng từ</param>
        /// <returns>Số bản ghi thêm</returns>
        /// Author: NVHThai (26/10/2022)
        public int InsertLicense(License license)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                var storedProcedureNameLicense = "Proc_LicenseMaster_Insert";
                var storedProcedureNameLicenseDetail = "Proc_LicenseDetail_Insert";
                var parameterMaster = new DynamicParameters();

                var licenseID = Guid.NewGuid();
                parameterMaster.Add("$v_LicenseID", licenseID);
                parameterMaster.Add("$v_LicenseCode", license.LicenseCode);
                parameterMaster.Add("$v_LicenseDay", license.LicenseDay);
                parameterMaster.Add("$v_WriteDay", license.WriteDay);
                parameterMaster.Add("$v_TotalCost", license.TotalCost);
                parameterMaster.Add("$v_Content", license.Content);
                parameterMaster.Add("$v_CreateBy", "Nguyễn Vũ Hải Thái");
                parameterMaster.Add("$v_CreateDate", DateTime.Now);
                parameterMaster.Add("$v_ModifiedBy", "Nguyễn Vũ Hải Thái");
                parameterMaster.Add("$v_ModifiedDate", DateTime.Now);
                var numberOfAffectedRowsMaster = mySqlConnection.Execute(storedProcedureNameLicense, parameterMaster, commandType: System.Data.CommandType.StoredProcedure);

                var parameterDetail = new DynamicParameters();

                int count = license.listAssetID.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var licenseDetailID = Guid.NewGuid();
                        parameterDetail.Add("$v_LicenseDetailID", licenseDetailID);
                        parameterDetail.Add("$v_LicenseID", licenseID);
                        parameterDetail.Add("$v_AssetID", license.listAssetID[i]);
                        parameterDetail.Add("$v_CreateBy", "Nguyễn Vũ Hải Thái");
                        parameterDetail.Add("$v_CreateDate", DateTime.Now);
                        parameterDetail.Add("$v_ModifiedBy", "Nguyễn Vũ Hải Thái");
                        parameterDetail.Add("$v_ModifiedDate", DateTime.Now);
                        var numberOfAffectedRowsDetail = mySqlConnection.Execute(storedProcedureNameLicenseDetail, parameterDetail, commandType: System.Data.CommandType.StoredProcedure);
                    }
                }
                return numberOfAffectedRowsMaster;
            }
        }

        /// <summary>
        /// Hàm sửa 1 chứng từ
        /// </summary>
        /// <param name="licenseID">Id chứng từ cần sửa</param>
        /// <param name="license">Thông tin chứng từ</param>
        /// <returns>Số bản ghi đã sửa</returns>
        /// Author: NVHThai (26/10/2022)
        public int UpdateLicense(Guid licenseID, License license)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                var storedProcedureNameLicense = "Proc_LicenseMaster_EditByID"; 
                var storedProcedureNameLicenseDetailDelete = "Proc_LicenseDetail_DeleteByIDLicenseDetail";
                var storedProcedureNameLicenseDetail = "Proc_LicenseDetail_Insert"; 

                var parameterMaster = new DynamicParameters();
                parameterMaster.Add("$v_LicenseID", licenseID);
                parameterMaster.Add("$v_LicenseCode", license.LicenseCode);
                parameterMaster.Add("$v_LicenseDay", license.LicenseDay);
                parameterMaster.Add("$v_WriteDay", license.WriteDay);
                parameterMaster.Add("$v_TotalCost", license.TotalCost);
                parameterMaster.Add("$v_Content", license.Content);
                parameterMaster.Add("$v_ModifiedBy", "Nguyễn Vũ Hải Thái");
                parameterMaster.Add("$v_ModifiedDate", DateTime.Now);
                var numberOfAffectedRowsMaster = mySqlConnection.Execute(storedProcedureNameLicense, parameterMaster, commandType: System.Data.CommandType.StoredProcedure);

                var parameterLicenseDetailDelete = new DynamicParameters();
                parameterLicenseDetailDelete.Add("@$v_Where", licenseID);
                int numberOfAffectedRows = mySqlConnection.Execute(storedProcedureNameLicenseDetailDelete, parameterLicenseDetailDelete, commandType: System.Data.CommandType.StoredProcedure);

                var parameterDetail = new DynamicParameters();
                int count = license.listAssetID.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var licenseDetailID = Guid.NewGuid();
                        parameterDetail.Add("$v_LicenseDetailID", licenseDetailID);
                        parameterDetail.Add("$v_LicenseID", licenseID);
                        parameterDetail.Add("$v_AssetID", license.listAssetID[i]);
                        parameterDetail.Add("$v_CreateBy", "Nguyễn Vũ Hải Thái");
                        parameterDetail.Add("$v_CreateDate", DateTime.Now);
                        parameterDetail.Add("$v_ModifiedBy", "Nguyễn Vũ Hải Thái");
                        parameterDetail.Add("$v_ModifiedDate", DateTime.Now);
                        var numberOfAffectedRowsDetail = mySqlConnection.Execute(storedProcedureNameLicenseDetail, parameterDetail, commandType: System.Data.CommandType.StoredProcedure);
                    }
                }
                return numberOfAffectedRowsMaster + numberOfAffectedRows;
            }
        }

        /// <summary>
        /// Hàm xóa 1 chứng từ
        /// </summary>
        /// <param name="licenseID">ID chứng từ cần xóa</param>
        /// <returns>Số bản ghi ảnh hưởng</returns>
        /// Author: NVHThai (26/10/2022)
        public int DeleteLicense(Guid licenseID)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = "Proc_LicenseMaster_Delete";

                var parameters = new DynamicParameters();
                parameters.Add("@$v_Where", licenseID);

                int numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return numberOfAffectedRows;
            }

        }

    }
}
