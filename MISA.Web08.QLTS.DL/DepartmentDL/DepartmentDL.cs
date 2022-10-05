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
    public class DepartmentDL : IDepartmentDL
    {
        #region API Get

        /// <summary>
        /// Lấy ra danh sách phòng ban lọc theo tên phòng ban và mã phòng ban
        /// </summary>
        /// <param name="keword">Tên phòng ban hoặc mã phòng ban cần tìm kiếm</param>
        /// <returns>Danh sách phòng ban</returns>
        /// Author: NVHThai (28/09/2022)
        public IEnumerable<Department> GetAllDepartments(string? keword)
        {

            //khởi tạo kết nối db
            string connectionString = DataContext.MySqlConnectionString;
            var mySqlConnection = new MySqlConnection(connectionString);

            // Chuẩn bị tên Stored procedure
            string storedProcedureName = String.Format(Resource.Proc_GetAll, typeof(Department).Name);

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();


            string whereConditions = null;
            if (keword != null)
            {
                whereConditions = $"department_name LIKE '%{keword}%' OR department_code LIKE '%{keword}%'";
            }

            parameters.Add("@$v_Where", whereConditions);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var department = mySqlConnection.Query<Department>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);


            return department;

        } 
        
        #endregion
    }
}
