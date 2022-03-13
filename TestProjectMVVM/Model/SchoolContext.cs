using System.Data.Entity;
using TestProjectMVVM;

namespace TestProjectMVVM
{
    public class SchoolContext : DbContext
    {
        public SchoolContext()
            : base("name=SchoolContext")
        {
        }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }       
    }
}