using Microsoft.AspNetCore.Mvc;
using StudentCRUDWeb.Entities.ViewModels;
using StudentCRUDWeb.Repositories.Repository;
using StudentCRUDWeb.Repositories.Repository.Interface;

namespace StudentCRUDWeb.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
                _studentRepository= studentRepository;  
        }

        //GET
        public IActionResult Index()
        {
            var students = _studentRepository.GetStudents();
            return View(students);
        }

        //get
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(StudentModel studentModel) {
            
            _studentRepository.AddStudent(studentModel);
            return RedirectToAction("Index");
        }

        //get
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View( _studentRepository.GetStudent(id));
        }


        [HttpPost]
        public IActionResult Edit(StudentModel studentModel)
        {

            _studentRepository.EditStudent(studentModel);
            return RedirectToAction("Index");
        }
    }
}
