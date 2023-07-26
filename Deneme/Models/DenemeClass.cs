using System.ComponentModel.DataAnnotations;

namespace Deneme.Models
{
    public class DenemeClass
    {
        [Key]
        public int Id { get; set; }
        public int Heat { get; set; }
       
    }
}
