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
    public class BaseDL<T> : IBaseDL<T>
    {

        /// <summary>
        /// lấy danh sách toàn bộ bản ghi trong 1 bảng
        /// </summary>
        /// <returns>Lấy danh sách toàn bộ bản ghi trong bảng</returns>
        /// Author: NVHThai (16/09/2022)
        public IEnumerable<T> GetAllRecords()
        {
           
            //khai báo tên stored procedure
            string storedProcedure = String.Format(Resource.Proc_GetAll, typeof(T).Name);

            //khởi tạo kết nối db
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào db
                var assets = mySqlConnection.Query<T>(storedProcedure, commandType: System.Data.CommandType.StoredProcedure);
                return assets;
            }
        }


        /// <summary>
        /// Lấy ra danh sách bản ghi có điều kiện
        /// </summary>
        /// <param name="keword">Tên bản ghi hoặc mã bản ghi tìm kiếm</param>
        /// <returns>Danh sách bản ghi có chọn lọc</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<T> GetFillterRecords(string? keword)
        {

            string storedProcedure = String.Format(Resource.Proc_GetAll, typeof(T).Name);

            string connectionString = DataContext.MySqlConnectionString;
            var parameters = new DynamicParameters();
            string whereConditions = null;

            string name = null;

            if(typeof(T).Name == Resource.ValueDepartment)
            {
                name = Resource.Department;
            }
            if(typeof(T).Name == Resource.ValueAssetCategory)
            {
                name = Resource.FixedAssetCategory;
            }

            if (keword != null)
            {
                whereConditions = $"{name}_name LIKE '%{keword}%' OR {name}_code LIKE '%{keword}%'";
            }

            parameters.Add("@$v_Where", whereConditions);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào db
                var department = mySqlConnection.Query<T>(storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return department;
            }
        }
    }
}
