using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace stagesDEIS.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "You must choose a branch")]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "You must specify your grades")]
        public IList<Grade> Grades { get; set; }
        // TODO : Initialize all lists
    }
}