namespace SV20T1080033.Web.Models
{
    public class PersonDAL
    {


        public List<Person> List()
        {
            List<Person> list = new List<Person>();
            list.Add(new Person()
            {
                PersonId = 1,
                Name = "Trần Văn Hiếu",
                Address = "77 nguyễn huệ",
                Email = "tranvanhieu@gmail.com"

            });

            list.Add(new Person()
            {
                PersonId = 2,
                Name = "Trần Văn Hoàng",
                Address = "77 nguyễn huệ",
                Email = "tranvanhoang@gmail.com"

            });

            list.Add(new Person()
            {
                PersonId = 3,
                Name = "Nguyễn Thanh An",
                Address = "77 nguyễn huệ",
                Email = "nguyenthanhan@gmail.com"

            });
            return list;

        }
    }

}
