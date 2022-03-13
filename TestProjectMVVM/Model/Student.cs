using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectMVVM
{
    public class Student : SelectedItem
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Group { get; set; }
        public Teacher Teacher { get; set; }
        public int GradePointAverage { get; set; }
                
    }
}
