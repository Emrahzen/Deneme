using Deneme.Models;
using Microsoft.EntityFrameworkCore;

namespace Deneme.ApplicationDBContext
{
    public class AppDBContext :DbContext
    {

        //Constructor with DbContextOptions and the context class itself
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        //Create the DataSet for the Employee         
        public DbSet<DenemeClass> denemeClass { get; set; }

    }
}
