using SV20T1080033.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080033.DataLayers
{
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Xac thuc tai khoan dang nhap cua nguoi dung (employee, customer)
        /// Neu xac thuc thanh cong thi tra ve thong tin tai khoan, nguoc lai tra ve null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount ?Authorize(string userName, string password);
        
        /// <summary>
        /// Thay doi mat khau
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePassword(string userName, string password);
    }
}
