using Dapper;
using SV20T1080033.DomainModels;
using System.Data;

namespace SV20T1080033.DataLayers.SQLServer
{
    /// <summary>
    /// cai dat cac xu ly lien quan den tai khoan
    /// </summary>
    public class EmployeeUserAccountDAL : _BaseDAL, IUserAccountDAL
    {
        public EmployeeUserAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string userName, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select EmployeeID as UserID, Email as UserName, FullName, Email, Photo 
                            from Employees 
                            where Email = @userName and Password = @password";
                var parameters = new
                {
                    userName = userName,    
                    password = password
                };
                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool ChangePassword(string userName, string password)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Employees set Password = @password where Email = @userName";
                var parameter = new
                {
                    userName = userName,
                    password = password
                };
                result = connection.Execute(sql: sql, param: parameter, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
