using System.ComponentModel.DataAnnotations;


namespace StudentCRUDWeb.Entities.ViewModels
{
    public partial class StudentModel
    {
        [Key]
        public int RollNo { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Department { get; set; } = null!;
        [Required]
        public string Course { get; set; } = null!;
    }
}
