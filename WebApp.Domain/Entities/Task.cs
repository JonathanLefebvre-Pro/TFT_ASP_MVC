namespace WebApp.Domain.Entities;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreationDate { get; set; }
    public bool Done { get; set; }
}
