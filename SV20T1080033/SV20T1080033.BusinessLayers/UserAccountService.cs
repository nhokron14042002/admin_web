using SV20T1080033.DataLayers;
using SV20T1080033.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SV20T1080033.BusinessLayers.UserAccountService;

namespace SV20T1080033.BusinessLayers
{
    public class UserAccountService
    {
        private static readonly IUserAccountDAL employeeUserAccountDB;

        static UserAccountService()
        {
            string connectionString = "server=DESKTOP-5K2RFED\\SQLEXPRESS;user=sa;password=0387002171;database=LiteCommerceDB;TrustServerCertificate=true";
            employeeUserAccountDB = new DataLayers.SQLServer.EmployeeUserAccountDAL(connectionString);
            //customerUserAccountDB = new DataLayers.SQLServer.CustomerUserAccountDAL(connectionString);
        }

        public static UserAccount? Authorize(string userName, string password, TypeOfAccounts typeOfAccounts)
        {
            switch (typeOfAccounts)
            {
                case TypeOfAccounts.Employee:
                    return employeeUserAccountDB.Authorize(userName, password);
                //case TypeOfAccouts.Customer:
                //    return customerUserAccountDB.Authorize(userName, password);
                default:
                    return null;
            }
        }

        public static bool ChangePassword(string userName, string password, TypeOfAccounts typeOfAccounts)
        {
            switch (typeOfAccounts)
            {
                case TypeOfAccounts.Employee:
                    return employeeUserAccountDB.ChangePassword(userName, password);
                //case TypeOfAccouts.Customer:
                //    return customerUserAccountDB.Authorize(userName, password);
                default:
                    return false;
            }
        }

        public enum TypeOfAccounts
        {
            Employee,
            Customer,
            Shipper
        }
    }
}
