using Microsoft.AspNetCore.Identity;

namespace to_do.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public DateTime? Date { get; set; }

        public string? Status { get; set; }

        public bool? Flag { get; set; }

        public string UserId { get; set; }
        public IEnumerable<IdentityUser> User { get; set; }
    }

}
