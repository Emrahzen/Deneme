using Deneme.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Deneme.ApplicationDBContext
{
    public class AppDBContext : DbContext
    {

        //Constructor with DbContextOptions and the context class itself
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        //Create the DataSet for the Employee         
        public DbSet<DenemeClass> denemeClass { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-LMFD98G\\SQLSERVERENES;Database=Deneme;Trusted_Connection=True;Integrated Security=True;"); // Veritabanı bağlantı dizesini buraya ekleyin
        }

    }
}
