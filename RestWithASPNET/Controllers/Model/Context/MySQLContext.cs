
using Microsoft.EntityFrameworkCore;

namespace RestWithASPNET.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext()
        {

        }

        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) // vai passar os options pro base
        {
            
        }

        public DbSet<Person> Persons { get; set; }
    }
}