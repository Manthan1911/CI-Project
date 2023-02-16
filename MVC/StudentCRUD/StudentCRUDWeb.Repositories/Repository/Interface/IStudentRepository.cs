using StudentCRUDWeb.Entities.DataModels;
using StudentCRUDWeb.Entities.ViewModels;

namespace StudentCRUDWeb.Repositories.Repository.Interface
{
    public interface IStudentRepository
    {
        public IEnumerable<StudentModel> GetStudents();

        public int AddStudent(StudentModel studentModel);

        public Student GetStudent(int? id);

        public void EditStudent(StudentModel studentModel);
    }
}
