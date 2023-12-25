namespace SV20T1080033.Web
{
	public class Calculator
	{
		public static int AgeCalculate(DateTime birthDate)
		{
			//int age = DateTime.Now.Year - birthDate.Year;
			//if (DateTime.Now < new DateTime(DateTime.Now.Year, birthDate.Year, 1)) // Nếu ngày sinh chưa đến
			//{
			//	age--; // Trừ đi một năm
			//}
			//return age;

			DateTime currentDate = DateTime.Today;
			double age = (currentDate - birthDate).Days / 365.242199;
			return (int)Math.Floor(age);
		}
	}
}
