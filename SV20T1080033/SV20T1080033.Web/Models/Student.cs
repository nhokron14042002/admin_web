using System.Net;
using System.Xml.Linq;

namespace SV20T1080033.Web.Models
{
    public class Student
    {
        public String? StudentId { get; set; }
        public String? StudentName { get; set; }
    }

    public class StudentDAL 
    {
        public List<Student> List() {
            List<Student> students = new List<Student>();


            students.Add(new Student
            {
                StudentId = "1",
                StudentName = "Trần Văn Hiếu",


            });
             students.Add(new Student
             {
                 StudentId = "2",
                 StudentName = "Trần Văn Hoàng",


             });


            return students;
        }
       
    }

}
