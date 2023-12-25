using System.Text.RegularExpressions;
using System.Net.Mail;

namespace SV20T1080033.Web
{
	public class CheckString
	{
		public static bool IsPhone(string phoneNum) // phương thức kiểm tra số
		{
			Regex? regex = null; // khai báo một đối tượng Regex
			try // thử thực hiện
			{
				//Chỉ chấp nhận các format SĐT sau:
				//1-234-567-8901
				//1 (234) 567-8901
				//1.234.567.8901
				//12345678901
				regex = new Regex(@"^\+?\d{1,3}?[- .]?\(?(?:\d{2,3})\)?[- .]?\d\d\d[- .]?\d\d\d\d$"); // khởi tạo đối tượng Regex với biểu thức chính quy cho số điện thoại
				return regex.IsMatch(phoneNum); // trả về kết quả kiểm tra xem chuỗi có khớp với biểu thức chính quy không
			}
			catch // nếu có lỗi xảy ra
			{
				return false; // trả về giá trị sai
			}
		}

		public static bool IsEmail(string email) // phương thức kiểm tra Email
		{
			try // thử thực hiện
			{
				MailAddress m = new MailAddress(email);
				return true;
			}
			catch // nếu có lỗi xảy ra
			{
				return false; // trả về giá trị sai
			}
		}

		public static bool ContainsNumber(string str)
		{
			return Regex.IsMatch(str, @"\d");
		}

		public static bool ContainsSpecial(string str)
		{
			return !Regex.IsMatch(str, @"^[,a-zA-Z0-9_àảãáạăằẳẵắặâầẩẫấậđèẻẽéẹêềểễếệìỉĩíịòỏõóọôồổỗốộơờởỡớợùủũúụưừửữứựỳỷỹýỵ\s]+$");
		}
	}
}
