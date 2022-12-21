using Microsoft.Build.Framework;
using Microsoft.VisualBasic;

namespace to_do.Models
{
    public class ToDoCreate
    {
        [Required]
        public string Name { get; set; }
        
        public string Notes { get; set; }
        [Required]
        public DateTime Date { get; set; }



        

        
    }
}
