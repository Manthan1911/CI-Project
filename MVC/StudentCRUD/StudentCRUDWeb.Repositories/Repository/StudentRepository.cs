using StudentCRUDWeb.Entities.DataModels;
using StudentCRUDWeb.Entities.ViewModels;
using StudentCRUDWeb.Repositories.Repository.Interface;

namespace StudentCRUDWeb.Repositories.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _studentDbContext;

        public StudentRepository(StudentDbContext studentDbContext)
        {
            _studentDbContext= studentDbContext;    
        }

        public int AddStudent(StudentModel studentModel)
        {
            var student = new Student { 
                    RollNo=studentModel.RollNo,
                    Name=studentModel.Name,
                    Department=studentModel.Department,
                    Course=studentModel.Course
            };

            _studentDbContext.Add(student);
            _studentDbContext.SaveChanges();
            return studentModel.RollNo;
        }

        public IEnumerable<StudentModel> GetStudents()
        {
            return _studentDbContext.Students
                .Select(students => new StudentModel
                {
                    RollNo = students.RollNo,
                    Name = students.Name,
                    Department = students.Department,
                    Course = students.Course,
                });
        }

        public Student GetStudent(int? id)
        {
             return _studentDbContext.Students.Find(id);
        }

        public void EditStudent(StudentModel studentModel)
        {
            var student = new Student
            {
                RollNo = studentModel.RollNo,
                Name = studentModel.Name,
                Department = studentModel.Department,
                Course = studentModel.Course
            };

            _studentDbContext.Update(student);
            _studentDbContext.SaveChanges();
        }

    }
}
