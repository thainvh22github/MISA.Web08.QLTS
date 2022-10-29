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
    public class UserDL : IUserDL
    {
        public User Login(User? user)
        {
            string connectionString = DataContext.MySqlConnectionString;
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                string storedProcedureName = "Proc_User_CheckUser";
                var parameters = new DynamicParameters();

                var whereConditions = new List<string>();

                whereConditions.Add($"UserName = '{user.UserName}' And UserPassWord = '{user.Password}'");
                string whereClause = string.Join(" AND ", whereConditions);
                parameters.Add("@$v_Where", whereClause);

                var result = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
               
                if(result == "1")
                {
                    return user;
                }
                return null;
            }
        }

        
    }
}
