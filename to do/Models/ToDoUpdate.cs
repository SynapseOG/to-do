namespace to_do.Models
{
    public class ToDoUpdate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public DateTime? Date { get; set; }

        public string? Status { get; set; }

        public bool? Flag { get; set; }
    }
}
