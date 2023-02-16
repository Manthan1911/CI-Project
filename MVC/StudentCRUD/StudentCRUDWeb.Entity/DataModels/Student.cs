using System;
using System.Collections.Generic;

namespace StudentCRUDWeb.Entities.DataModels;

public partial class Student
{
    public int RollNo { get; set; }

    public string Name { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Course { get; set; } = null!;
}
