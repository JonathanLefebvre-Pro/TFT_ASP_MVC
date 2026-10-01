using System.ComponentModel.DataAnnotations;

namespace WebApp.Forms;

public class TaskForm
{
    [Required]
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "Title must be between 3 and 50 characters."
    )]
    public string Title { get; set; } = string.Empty;
}
