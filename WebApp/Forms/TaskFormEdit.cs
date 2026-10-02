using System.ComponentModel.DataAnnotations;
using DE = WebApp.Domain.Entities;

namespace WebApp.Forms;

public class TaskFormEdit
{
    public int Id { get; set; }

    [Required]
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "Title must be between 3 and 50 characters."
    )]
    public string Title { get; set; } = string.Empty;
    public bool Done { get; set; }
}

public static class TaskFormEditExtension
{
    public static TaskFormEdit ToTaskForm(this DE.Task task)
    {
        TaskFormEdit form = new TaskFormEdit();
        form.Id = task.Id;
        form.Title = task.Title;
        form.Done = task.Done;
        return form;
    }

    public static DE.Task ToTask(this TaskFormEdit form)
    {
        DE.Task task = new DE.Task();
        task.Id = form.Id;
        task.Title = form.Title;
        task.Done = form.Done;
        return task;
    }
}
